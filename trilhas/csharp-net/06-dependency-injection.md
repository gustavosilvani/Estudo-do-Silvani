# 06 - Dependency Injection

## 📖 Injeção de Dependência (DI)

DI é um padrão de design que implementa Inversão de Controle (IoC) para resolver dependências.

## 🎯 Por que DI?

```csharp
// ❌ SEM DI - Forte acoplamento
public class PedidoService {
    private EmailService _emailService;
    private RepositorioPedido _repositorio;
    
    public PedidoService() {
        _emailService = new EmailService();  // Acoplado
        _repositorio = new RepositorioPedido();  // Acoplado
    }
    
    // Difícil de testar, inflexível
}

// ✅ COM DI - Fraco acoplamento
public class PedidoService {
    private readonly IEmailService _emailService;
    private readonly IRepositorioPedido _repositorio;
    
    public PedidoService(
        IEmailService emailService,
        IRepositorioPedido repositorio) {
        _emailService = emailService;
        _repositorio = repositorio;
    }
    
    // Fácil de testar, flexível
}
```

## 🔧 DI Nativo do ASP.NET Core

### Lifetimes de Serviços

```csharp
// Program.cs
var builder = WebApplication.CreateBuilder(args);

// 1. TRANSIENT - Nova instância a cada injeção
builder.Services.AddTransient<IEmailService, EmailService>();

// 2. SCOPED - Uma instância por requisição HTTP
builder.Services.AddScoped<IPedidoService, PedidoService>();
builder.Services.AddScoped<IRepositorioPedido, RepositorioPedidoBD>();

// 3. SINGLETON - Uma única instância para toda aplicação
builder.Services.AddSingleton<ICacheService, CacheService>();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
```

### Quando Usar Cada Lifetime?

```csharp
// TRANSIENT - Serviços leves e stateless
public interface IEmailService {
    Task EnviarAsync(string para, string assunto, string corpo);
}

// SCOPED - Serviços com DbContext, UnitOfWork
public interface IPedidoService {
    Task<Pedido> CriarAsync(CriarPedidoDto dto);
}

// SINGLETON - Configurações, caches, serviços caros
public interface ICacheService {
    T Get<T>(string key);
    void Set<T>(string key, T value);
}
```

## 💉 Tipos de Injeção

### 1. Constructor Injection (Recomendado)

```csharp
public class ClienteService : IClienteService {
    private readonly IRepositorioCliente _repositorio;
    private readonly ILogger<ClienteService> _logger;
    private readonly IEmailService _emailService;
    
    // Todas dependências injetadas no construtor
    public ClienteService(
        IRepositorioCliente repositorio,
        ILogger<ClienteService> logger,
        IEmailService emailService) {
        _repositorio = repositorio;
        _logger = logger;
        _emailService = emailService;
    }
    
    public async Task<Cliente> CriarAsync(CriarClienteDto dto) {
        _logger.LogInformation("Criando cliente");
        var cliente = new Cliente(dto.Nome, dto.Email);
        await _repositorio.AdicionarAsync(cliente);
        await _emailService.EnviarBoasVindasAsync(cliente.Email);
        return cliente;
    }
}
```

### 2. Property Injection (Evitar)

```csharp
// ❌ NÃO RECOMENDADO
public class ServicoComPropriedade {
    [Inject]  // Requer biblioteca adicional
    public ILogger Logger { get; set; }
    
    // Dependência opcional pode ser null
}
```

### 3. Method Injection (Casos Especiais)

```csharp
// Use quando dependência é necessária apenas em um método
public class RelatorioService {
    public async Task GerarRelatorioAsync(
        [FromServices] IEmailService emailService) {  // Injetado apenas aqui
        // ...
    }
}
```

## 🏭 Registrando Serviços

### Registro Simples

```csharp
// Interface → Implementação
builder.Services.AddScoped<IProdutoService, ProdutoService>();
builder.Services.AddScoped<IRepositorioProduto, RepositorioProdutoBD>();

// Classe concreta
builder.Services.AddScoped<EmailService>();

// Instância existente
builder.Services.AddSingleton<IConfiguration>(configuration);

// Factory
builder.Services.AddScoped<IPedidoService>(provider => {
    var repository = provider.GetRequiredService<IRepositorioPedido>();
    var logger = provider.GetRequiredService<ILogger<PedidoService>>();
    return new PedidoService(repository, logger);
});
```

### Registro por Convenção

```csharp
// Registrar todos serviços de um assembly
public static class ServiceCollectionExtensions {
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services) {
        
        // Buscar todas interfaces e implementações
        var assembly = Assembly.GetExecutingAssembly();
        
        var serviceTypes = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("Service"));
        
        foreach (var serviceType in serviceTypes) {
            var interfaceType = serviceType.GetInterfaces()
                .FirstOrDefault(i => i.Name == $"I{serviceType.Name}");
            
            if (interfaceType != null) {
                services.AddScoped(interfaceType, serviceType);
            }
        }
        
        return services;
    }
}

// Uso
builder.Services.AddApplicationServices();
```

## 🎯 Exemplo Completo

```csharp
// Interfaces
public interface IRepositorioPedido {
    Task<Pedido> BuscarPorIdAsync(int id);
    Task AdicionarAsync(Pedido pedido);
}

public interface IEmailService {
    Task EnviarAsync(string para, string assunto, string corpo);
}

public interface INotificacaoService {
    Task NotificarPedidoCriadoAsync(Pedido pedido);
}

// Implementações
public class RepositorioPedidoBD : IRepositorioPedido {
    private readonly AppDbContext _context;
    
    public RepositorioPedidoBD(AppDbContext context) {
        _context = context;
    }
    
    public async Task<Pedido> BuscarPorIdAsync(int id) {
        return await _context.Pedidos.FindAsync(id);
    }
    
    public async Task AdicionarAsync(Pedido pedido) {
        _context.Pedidos.Add(pedido);
        await _context.SaveChangesAsync();
    }
}

public class EmailService : IEmailService {
    private readonly IConfiguration _config;
    private readonly ILogger<EmailService> _logger;
    
    public EmailService(
        IConfiguration config,
        ILogger<EmailService> logger) {
        _config = config;
        _logger = logger;
    }
    
    public async Task EnviarAsync(string para, string assunto, string corpo) {
        _logger.LogInformation($"Enviando email para {para}");
        // Implementação real de envio
        await Task.CompletedTask;
    }
}

public class NotificacaoService : INotificacaoService {
    private readonly IEmailService _emailService;
    private readonly ILogger<NotificacaoService> _logger;
    
    public NotificacaoService(
        IEmailService emailService,
        ILogger<NotificacaoService> logger) {
        _emailService = emailService;
        _logger = logger;
    }
    
    public async Task NotificarPedidoCriadoAsync(Pedido pedido) {
        var assunto = $"Pedido #{pedido.Id} criado";
        var corpo = $"Seu pedido foi criado com sucesso!";
        
        await _emailService.EnviarAsync(
            pedido.Cliente.Email,
            assunto,
            corpo
        );
        
        _logger.LogInformation($"Notificação enviada para pedido {pedido.Id}");
    }
}

// Serviço de aplicação
public class PedidoService {
    private readonly IRepositorioPedido _repositorio;
    private readonly INotificacaoService _notificacao;
    private readonly ILogger<PedidoService> _logger;
    
    public PedidoService(
        IRepositorioPedido repositorio,
        INotificacaoService notificacao,
        ILogger<PedidoService> logger) {
        _repositorio = repositorio;
        _notificacao = notificacao;
        _logger = logger;
    }
    
    public async Task<Pedido> CriarPedidoAsync(CriarPedidoDto dto) {
        _logger.LogInformation("Criando novo pedido");
        
        var pedido = new Pedido {
            ClienteId = dto.ClienteId,
            Itens = dto.Itens,
            DataCriacao = DateTime.UtcNow
        };
        
        await _repositorio.AdicionarAsync(pedido);
        await _notificacao.NotificarPedidoCriadoAsync(pedido);
        
        return pedido;
    }
}

// Registro no Program.cs
builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddScoped<IRepositorioPedido, RepositorioPedidoBD>();
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddScoped<INotificacaoService, NotificacaoService>();
builder.Services.AddScoped<PedidoService>();
```

## 🧪 Testando com DI

```csharp
public class PedidoServiceTests {
    [Fact]
    public async Task CriarPedido_DadosValidos_CriaPedidoComSucesso() {
        // Arrange - Criar mocks
        var mockRepositorio = new Mock<IRepositorioPedido>();
        var mockNotificacao = new Mock<INotificacaoService>();
        var mockLogger = new Mock<ILogger<PedidoService>>();
        
        var service = new PedidoService(
            mockRepositorio.Object,
            mockNotificacao.Object,
            mockLogger.Object
        );
        
        var dto = new CriarPedidoDto {
            ClienteId = 1,
            Itens = new List<ItemPedidoDto>()
        };
        
        // Act
        var pedido = await service.CriarPedidoAsync(dto);
        
        // Assert
        Assert.NotNull(pedido);
        mockRepositorio.Verify(r => r.AdicionarAsync(It.IsAny<Pedido>()), Times.Once);
        mockNotificacao.Verify(n => n.NotificarPedidoCriadoAsync(It.IsAny<Pedido>()), Times.Once);
    }
}
```

## 📦 Service Locator (Anti-Pattern)

```csharp
// ❌ EVITE - Service Locator
public class PedidoService {
    public void ProcessarPedido() {
        // Buscar serviço diretamente do container
        var emailService = ServiceLocator.GetService<IEmailService>();  // RUIM!
        emailService.Enviar(...);
    }
}

// ✅ USE - Constructor Injection
public class PedidoService {
    private readonly IEmailService _emailService;
    
    public PedidoService(IEmailService emailService) {
        _emailService = emailService;  // BOM!
    }
}
```

## ✅ Verificação

- [ ] Entendeu lifetimes (Transient, Scoped, Singleton)
- [ ] Usou Constructor Injection
- [ ] Registrou serviços no container
- [ ] Aplicou em controllers e services
- [ ] Testou com mocks
- [ ] Evitou Service Locator

---

[← Voltar: APIs RESTful](./05-apis-restful.md) | [Próximo: Testes de Integração →](./07-testes-integracao.md)