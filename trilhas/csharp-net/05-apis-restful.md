# 05 - APIs RESTful

## 📖 REST - Representational State Transfer

REST é um estilo arquitetural para APIs web que usa HTTP de forma padronizada.

## 🎯 Princípios REST

### 1. **Recursos** (Resources)
Tudo é um recurso identificado por URI:
- `/api/clientes` - Coleção de clientes
- `/api/clientes/123` - Cliente específico
- `/api/clientes/123/pedidos` - Pedidos do cliente

### 2. **Métodos HTTP**
- **GET**: Buscar recursos
- **POST**: Criar novo recurso
- **PUT**: Atualizar recurso completo
- **PATCH**: Atualizar parcialmente
- **DELETE**: Remover recurso

### 3. **Stateless**
Cada requisição contém toda informação necessária.

### 4. **Representações**
Recursos representados em JSON, XML, etc.

## 🔧 Implementação Completa de API REST

```csharp
// Modelo
public class Produto {
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public decimal Preco { get; set; }
    public int EstoqueAtual { get; set; }
    public string Categoria { get; set; }
    public bool Ativo { get; set; }
    public DateTime DataCriacao { get; set; }
}

// DTOs
public class CriarProdutoDto {
    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(100, MinimumLength = 3)]
    public string Nome { get; set; }
    
    [StringLength(500)]
    public string Descricao { get; set; }
    
    [Required]
    [Range(0.01, 999999.99)]
    public decimal Preco { get; set; }
    
    [Range(0, int.MaxValue)]
    public int EstoqueAtual { get; set; }
    
    [Required]
    public string Categoria { get; set; }
}

// Controller RESTful completo
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ProdutosController : ControllerBase {
    private readonly IProdutoService _service;
    private readonly ILogger<ProdutosController> _logger;
    
    public ProdutosController(
        IProdutoService service,
        ILogger<ProdutosController> logger) {
        _service = service;
        _logger = logger;
    }
    
    /// <summary>
    /// Retorna todos os produtos
    /// </summary>
    /// <param name="categoria">Filtro por categoria (opcional)</param>
    /// <param name="pageNumber">Número da página</param>
    /// <param name="pageSize">Tamanho da página</param>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedList<ProdutoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PaginatedList<ProdutoDto>>> GetTodos(
        [FromQuery] string categoria = null,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10) {
        
        var produtos = await _service.BuscarTodosAsync(categoria, pageNumber, pageSize);
        return Ok(produtos);
    }
    
    /// <summary>
    /// Retorna um produto específico
    /// </summary>
    /// <param name="id">ID do produto</param>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ProdutoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProdutoDto>> GetPorId(int id) {
        var produto = await _service.BuscarPorIdAsync(id);
        
        if (produto == null) {
            return NotFound(new { message = $"Produto {id} não encontrado" });
        }
        
        return Ok(produto);
    }
    
    /// <summary>
    /// Cria um novo produto
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ProdutoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProdutoDto>> Criar(
        [FromBody] CriarProdutoDto dto) {
        
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }
        
        var produto = await _service.CriarAsync(dto);
        
        return CreatedAtAction(
            nameof(GetPorId),
            new { id = produto.Id },
            produto
        );
    }
    
    /// <summary>
    /// Atualiza um produto existente
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Atualizar(
        int id,
        [FromBody] AtualizarProdutoDto dto) {
        
        if (id != dto.Id) {
            return BadRequest(new { message = "ID não corresponde" });
        }
        
        try {
            await _service.AtualizarAsync(dto);
            return NoContent();
        } catch (ProdutoNaoEncontradoException) {
            return NotFound();
        }
    }
    
    /// <summary>
    /// Atualiza parcialmente um produto
    /// </summary>
    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AtualizarParcial(
        int id,
        [FromBody] JsonPatchDocument<Produto> patchDoc) {
        
        var produto = await _service.BuscarPorIdAsync(id);
        if (produto == null) {
            return NotFound();
        }
        
        patchDoc.ApplyTo(produto);
        await _service.AtualizarAsync(produto);
        
        return NoContent();
    }
    
    /// <summary>
    /// Remove um produto
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Deletar(int id) {
        var deletado = await _service.DeletarAsync(id);
        
        if (!deletado) {
            return NotFound();
        }
        
        return NoContent();
    }
    
    /// <summary>
    /// Busca produtos por categoria
    /// </summary>
    [HttpGet("categoria/{categoria}")]
    [ProducesResponseType(typeof(List<ProdutoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ProdutoDto>>> PorCategoria(string categoria) {
        var produtos = await _service.BuscarPorCategoriaAsync(categoria);
        return Ok(produtos);
    }
}
```

## 📄 Paginação

```csharp
public class PaginatedList<T> {
    public List<T> Items { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }
    
    public bool HasPrevious => PageNumber > 1;
    public bool HasNext => PageNumber < TotalPages;
    
    public PaginatedList(List<T> items, int count, int pageNumber, int pageSize) {
        Items = items;
        TotalCount = count;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
    }
}

// Uso no serviço
public async Task<PaginatedList<ProdutoDto>> BuscarTodosAsync(
    string categoria,
    int pageNumber,
    int pageSize) {
    
    var query = _context.Produtos.AsQueryable();
    
    if (!string.IsNullOrEmpty(categoria)) {
        query = query.Where(p => p.Categoria == categoria);
    }
    
    var count = await query.CountAsync();
    var items = await query
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();
    
    var dtos = items.Select(MapToDto).ToList();
    
    return new PaginatedList<ProdutoDto>(dtos, count, pageNumber, pageSize);
}
```

## 📝 Versionamento de API

```csharp
// Versionamento por URL
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class ProdutosV1Controller : ControllerBase {
    // Implementação v1
}

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("2.0")]
public class ProdutosV2Controller : ControllerBase {
    // Implementação v2 com mudanças
}

// Configuração no Program.cs
builder.Services.AddApiVersioning(options => {
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});
```

## 🔒 CORS (Cross-Origin Resource Sharing)

```csharp
// Program.cs
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", builder => {
        builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
    
    // Ou específico
    options.AddPolicy("Production", builder => {
        builder
            .WithOrigins("https://meusite.com")
            .WithMethods("GET", "POST", "PUT", "DELETE")
            .AllowAnyHeader();
    });
});

app.UseCors("AllowAll");
```

## 📚 Documentação com Swagger/OpenAPI

```csharp
// Program.cs
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo {
        Title = "API de Produtos",
        Version = "v1",
        Description = "API RESTful para gerenciamento de produtos",
        Contact = new OpenApiContact {
            Name = "Suporte",
            Email = "suporte@email.com"
        }
    });
    
    // Incluir comentários XML
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

// Habilitar em produção (opcional)
if (app.Environment.IsDevelopment() || app.Environment.IsProduction()) {
    app.UseSwagger();
    app.UseSwaggerUI(c => {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
    });
}
```

## 🎯 Status Codes HTTP

```csharp
// 2xx - Sucesso
return Ok(data);                    // 200
return Created(uri, data);          // 201
return NoContent();                 // 204

// 4xx - Erro do cliente
return BadRequest(error);           // 400
return Unauthorized();              // 401
return Forbidden();                 // 403
return NotFound();                  // 404
return Conflict();                  // 409
return UnprocessableEntity(error);  // 422

// 5xx - Erro do servidor
return StatusCode(500, error);      // 500
return StatusCode(503);             // 503
```

## 🔧 Content Negotiation

```csharp
// Suportar múltiplos formatos
builder.Services.AddControllers()
    .AddXmlSerializerFormatters()
    .AddXmlDataContractSerializerFormatters();

// Cliente escolhe formato
// Accept: application/json
// Accept: application/xml

[HttpGet("{id}")]
[Produces("application/json", "application/xml")]
public async Task<ActionResult<ProdutoDto>> Get(int id) {
    // Retorna no formato solicitado
}
```

## ✅ Verificação

- [ ] Implementou CRUD completo RESTful
- [ ] Usou métodos HTTP apropriados
- [ ] Implementou paginação
- [ ] Configurou Swagger/OpenAPI
- [ ] Documentou endpoints
- [ ] Configurou CORS
- [ ] Usou status codes corretos
- [ ] Implementou validações

---

[← Voltar: ASP.NET Core](./04-aspnet-core-basico.md) | [Próximo: Dependency Injection →](./06-dependency-injection.md)