# 04 - ASP.NET Core Básico

## 📖 Introdução ao ASP.NET Core

ASP.NET Core é o framework web moderno, cross-platform e de alta performance da Microsoft.

## 🎯 Criar Primeiro Projeto Web

```bash
# API Web
dotnet new webapi -n MinhaApi
cd MinhaApi
dotnet run

# MVC Web App
dotnet new mvc -n MinhaWebApp
cd MinhaWebApp
dotnet run

# Razor Pages
dotnet new webapp -n MinhaWebPage
```

## 🏗️ Estrutura de Projeto ASP.NET Core

```
MinhaApi/
├── Controllers/        # Controladores MVC/API
├── Models/            # Modelos de dados
├── Services/          # Lógica de negócio
├── Program.cs         # Ponto de entrada e configuração
├── appsettings.json   # Configurações
└── Properties/
    └── launchSettings.json
```

## 🔧 Program.cs (Minimal API - .NET 6+)

```csharp
var builder = WebApplication.CreateBuilder(args);

// Adicionar serviços ao container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar injeção de dependência
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddSingleton<ICacheService, CacheService>();

var app = builder.Build();

// Configure o pipeline HTTP
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
```

## 🎮 Controllers

```csharp
[ApiController]
[Route("api/[controller]")]
public class ClientesController : ControllerBase {
    private readonly IClienteService _clienteService;
    private readonly ILogger<ClientesController> _logger;
    
    public ClientesController(
        IClienteService clienteService,
        ILogger<ClientesController> logger) {
        _clienteService = clienteService;
        _logger = logger;
    }
    
    // GET: api/clientes
    [HttpGet]
    public async Task<ActionResult<List<Cliente>>> GetTodos() {
        try {
            var clientes = await _clienteService.BuscarTodosAsync();
            return Ok(clientes);
        } catch (Exception ex) {
            _logger.LogError(ex, "Erro ao buscar clientes");
            return StatusCode(500, "Erro interno do servidor");
        }
    }
    
    // GET: api/clientes/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Cliente>> GetPorId(int id) {
        var cliente = await _clienteService.BuscarPorIdAsync(id);
        
        if (cliente == null) {
            return NotFound($"Cliente {id} não encontrado");
        }
        
        return Ok(cliente);
    }
    
    // POST: api/clientes
    [HttpPost]
    public async Task<ActionResult<Cliente>> Criar(CriarClienteDto dto) {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }
        
        var cliente = await _clienteService.CriarAsync(dto);
        
        return CreatedAtAction(
            nameof(GetPorId),
            new { id = cliente.Id },
            cliente
        );
    }
    
    // PUT: api/clientes/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(int id, AtualizarClienteDto dto) {
        if (id != dto.Id) {
            return BadRequest("ID não corresponde");
        }
        
        try {
            await _clienteService.AtualizarAsync(dto);
            return NoContent();
        } catch (ClienteNaoEncontradoException) {
            return NotFound();
        }
    }
    
    // DELETE: api/clientes/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Deletar(int id) {
        var deletado = await _clienteService.DeletarAsync(id);
        
        if (!deletado) {
            return NotFound();
        }
        
        return NoContent();
    }
}
```

## 📝 Data Transfer Objects (DTOs)

```csharp
// DTO para criar cliente
public class CriarClienteDto {
    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(100, MinimumLength = 3)]
    public string Nome { get; set; }
    
    [Required]
    [EmailAddress(ErrorMessage = "Email inválido")]
    public string Email { get; set; }
    
    [Phone(ErrorMessage = "Telefone inválido")]
    public string Telefone { get; set; }
}

// DTO para atualizar
public class AtualizarClienteDto {
    public int Id { get; set; }
    
    [Required]
    public string Nome { get; set; }
    
    [EmailAddress]
    public string Email { get; set; }
}

// DTO para resposta
public class ClienteDto {
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public DateTime DataCadastro { get; set; }
}
```

## 🔄 Middleware

```csharp
// Middleware customizado
public class LoggingMiddleware {
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggingMiddleware> _logger;
    
    public LoggingMiddleware(
        RequestDelegate next,
        ILogger<LoggingMiddleware> logger) {
        _next = next;
        _logger = logger;
    }
    
    public async Task InvokeAsync(HttpContext context) {
        _logger.LogInformation($"Request: {context.Request.Method} {context.Request.Path}");
        
        await _next(context);
        
        _logger.LogInformation($"Response: {context.Response.StatusCode}");
    }
}

// Registrar middleware
app.UseMiddleware<LoggingMiddleware>();

// Ou usar expressão lambda
app.Use(async (context, next) => {
    // Antes da requisição
    await next();
    // Depois da resposta
});
```

## ⚙️ Configuration (appsettings.json)

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=MeuBanco;..."
  },
  "EmailSettings": {
    "SmtpServer": "smtp.gmail.com",
    "Port": 587,
    "SenderEmail": "app@email.com"
  }
}
```

```csharp
// Ler configuração
public class EmailService {
    private readonly EmailSettings _settings;
    
    public EmailService(IOptions<EmailSettings> settings) {
        _settings = settings.Value;
    }
}

// Registrar no Program.cs
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings")
);
```

## 🎯 Exercício Prático - API de Produtos

```csharp
// Modelo
public class Produto {
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public decimal Preco { get; set; }
    public int EstoqueAtual { get; set; }
}

// Controller
[ApiController]
[Route("api/[controller]")]
public class ProdutosController : ControllerBase {
    private static List<Produto> _produtos = new() {
        new Produto { Id = 1, Nome = "Notebook", Preco = 3500, EstoqueAtual = 10 },
        new Produto { Id = 2, Nome = "Mouse", Preco = 50, EstoqueAtual = 50 }
    };
    
    [HttpGet]
    public ActionResult<List<Produto>> GetTodos() {
        return Ok(_produtos);
    }
    
    [HttpGet("{id}")]
    public ActionResult<Produto> GetPorId(int id) {
        var produto = _produtos.FirstOrDefault(p => p.Id == id);
        if (produto == null) return NotFound();
        return Ok(produto);
    }
    
    [HttpPost]
    public ActionResult<Produto> Criar(Produto produto) {
        produto.Id = _produtos.Max(p => p.Id) + 1;
        _produtos.Add(produto);
        return CreatedAtAction(nameof(GetPorId), new { id = produto.Id }, produto);
    }
    
    [HttpPut("{id}")]
    public IActionResult Atualizar(int id, Produto produto) {
        var existente = _produtos.FirstOrDefault(p => p.Id == id);
        if (existente == null) return NotFound();
        
        existente.Nome = produto.Nome;
        existente.Preco = produto.Preco;
        existente.EstoqueAtual = produto.EstoqueAtual;
        
        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public IActionResult Deletar(int id) {
        var produto = _produtos.FirstOrDefault(p => p.Id == id);
        if (produto == null) return NotFound();
        
        _produtos.Remove(produto);
        return NoContent();
    }
}
```

## ✅ Verificação

- [ ] Criou projeto ASP.NET Core
- [ ] Implementou controller com CRUD
- [ ] Usou DTOs para entrada/saída
- [ ] Configurou middleware
- [ ] Implementou validação
- [ ] Testou endpoints com Swagger

---

[← Voltar: Programação Assíncrona](./03-programacao-assincrona.md) | [Próximo: APIs RESTful →](./05-apis-restful.md)