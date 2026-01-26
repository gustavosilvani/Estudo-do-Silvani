# 04 - Testes de Integração

## 📖 O que são Testes de Integração?

Testes de integração verificam a **interação entre múltiplos componentes** do sistema, incluindo banco de dados, APIs, serviços externos, etc.

## 🎯 Diferença: Unitário vs Integração

```
Teste Unitário:
- Testa uma unidade isolada
- Usa mocks para dependências
- Rápido (milissegundos)
- Muitos testes (80% da pirâmide)

Teste de Integração:
- Testa componentes juntos
- Usa dependências reais
- Mais lento (segundos)
- Menos testes, mais críticos (15% da pirâmide)
```

## 📊 Pirâmide de Testes

```
     /\
    /  \
   / E2E \
  /--------\
 / Integração \
/--------------\
/   Unitários   \
-----------------
```

- **Unitários (80%)**: Rápidos, isolados, muitos
- **Integração (15%)**: Testam colaboração entre componentes
- **E2E (5%)**: Testam fluxo completo, lentos

## 🔧 WebApplicationFactory

Para testar APIs ASP.NET Core, usamos `WebApplicationFactory`:

```csharp
// 1. Adicionar pacote
<PackageReference Include="Microsoft.AspNetCore.Mvc.Testing" Version="9.0.0" />

// 2. Classe base para testes
public class IntegrationTestBase : IClassFixture<WebApplicationFactory<Program>> {
    protected readonly HttpClient _client;
    protected readonly WebApplicationFactory<Program> _factory;
    
    public IntegrationTestBase(WebApplicationFactory<Program> factory) {
        _factory = factory;
        _client = factory.CreateClient();
    }
}

// 3. Testes de integração
public class TarefasIntegrationTests : IntegrationTestBase {
    public TarefasIntegrationTests(WebApplicationFactory<Program> factory)
        : base(factory) { }
    
    [Fact]
    public async Task GetTodos_RetornaListaDeTarefas() {
        // Act
        var response = await _client.GetAsync("/api/tarefas");
        
        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var tarefas = JsonSerializer.Deserialize<List<TarefaDto>>(content);
        
        Assert.NotNull(tarefas);
    }
}
```

## 🗄️ Banco de Dados em Memória

Para testes de integração, usamos banco em memória:

```csharp
public class CustomWebApplicationFactory : WebApplicationFactory<Program> {
    protected override void ConfigureWebHost(IWebHostBuilder builder) {
        builder.ConfigureServices(services => {
            // Remover DbContext existente
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
            
            if (descriptor != null) {
                services.Remove(descriptor);
            }
            
            // Adicionar DbContext com banco em memória
            services.AddDbContext<AppDbContext>(options => {
                options.UseInMemoryDatabase("TestDatabase");
            });
            
            // Seed database
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            
            context.Database.EnsureCreated();
            SeedTestData(context);
        });
    }
    
    private static void SeedTestData(AppDbContext context) {
        context.Tarefas.AddRange(
            new Tarefa { Id = Guid.NewGuid(), Titulo = "Tarefa 1", Status = "Pendente" },
            new Tarefa { Id = Guid.NewGuid(), Titulo = "Tarefa 2", Status = "Concluida" }
        );
        context.SaveChanges();
    }
}

// Uso
public class TarefasTests : IClassFixture<CustomWebApplicationFactory> {
    private readonly HttpClient _client;
    
    public TarefasTests(CustomWebApplicationFactory factory) {
        _client = factory.CreateClient();
    }
}
```

## 🧪 Testes de Endpoint Completos

### CRUD Completo

```csharp
public class TarefasEndpointTests : IClassFixture<CustomWebApplicationFactory> {
    private readonly HttpClient _client;
    
    public TarefasEndpointTests(CustomWebApplicationFactory factory) {
        _client = factory.CreateClient();
    }
    
    [Fact]
    public async Task CrudCompleto_Tarefa() {
        // 1. CREATE
        var criarDto = new {
            Titulo = "Nova Tarefa",
            Descricao = "Teste de integração",
            Prioridade = 2
        };
        
        var createResponse = await _client.PostAsync(
            "/api/tarefas",
            JsonContent.Create(criarDto)
        );
        
        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);
        
        var tarefaCriada = await createResponse.Content
            .ReadFromJsonAsync<TarefaDto>();
        var id = tarefaCriada.Id;
        
        // 2. READ
        var getResponse = await _client.GetAsync($"/api/tarefas/{id}");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
        
        var tarefa = await getResponse.Content.ReadFromJsonAsync<TarefaDto>();
        Assert.Equal("Nova Tarefa", tarefa.Titulo);
        
        // 3. UPDATE (via PATCH)
        var updateResponse = await _client.PatchAsync(
            $"/api/tarefas/{id}/concluir",
            null
        );
        
        Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);
        
        // 4. DELETE
        var deleteResponse = await _client.DeleteAsync($"/api/tarefas/{id}");
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
        
        // 5. Verificar que foi deletado
        var getDeletedResponse = await _client.GetAsync($"/api/tarefas/{id}");
        Assert.Equal(HttpStatusCode.NotFound, getDeletedResponse.StatusCode);
    }
}
```

### Testes de Validação

```csharp
[Theory]
[InlineData("")]
[InlineData("   ")]
[InlineData(null)]
public async Task CriarTarefa_TituloInvalido_Retorna400(string tituloInvalido) {
    // Arrange
    var dto = new {
        Titulo = tituloInvalido,
        Descricao = "Descrição válida",
        Prioridade = 1
    };
    
    // Act
    var response = await _client.PostAsync(
        "/api/tarefas",
        JsonContent.Create(dto)
    );
    
    // Assert
    Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
}

[Fact]
public async Task CriarTarefa_DadosValidos_Retorna201ComLinks() {
    // Arrange
    var dto = new {
        Titulo = "Tarefa válida",
        Descricao = "Descrição",
        Prioridade = 2
    };
    
    // Act
    var response = await _client.PostAsync(
        "/api/tarefas",
        JsonContent.Create(dto)
    );
    
    // Assert
    Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    
    var tarefa = await response.Content.ReadFromJsonAsync<TarefaHateoasDto>();
    Assert.NotNull(tarefa);
    Assert.NotNull(tarefa.Links);
    Assert.Contains(tarefa.Links, l => l.Rel == "self");
}
```

## 🔐 Testes com Autenticação

```csharp
public class AuthenticatedTestBase : IClassFixture<CustomWebApplicationFactory> {
    protected readonly HttpClient _client;
    
    public AuthenticatedTestBase(CustomWebApplicationFactory factory) {
        _client = factory.CreateClient();
        
        // Adicionar token de autenticação
        var token = GetTestToken();
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
    }
    
    private string GetTestToken() {
        // Gerar token JWT para testes
        // Em produção, use biblioteca de autenticação
        return "test_token_here";
    }
}

[Fact]
public async Task GetPedidos_ComAutenticacao_Retorna200() {
    // Cliente já tem token configurado
    var response = await _client.GetAsync("/api/pedidos");
    
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
}

[Fact]
public async Task GetPedidos_SemAutenticacao_Retorna401() {
    // Cliente sem token
    var client = _factory.CreateClient();
    var response = await client.GetAsync("/api/pedidos");
    
    Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
}
```

## 📊 Testes de Performance

```csharp
[Fact]
public async Task GetTodos_Performance_MenorQue500ms() {
    var stopwatch = Stopwatch.StartNew();
    
    var response = await _client.GetAsync("/api/tarefas");
    
    stopwatch.Stop();
    
    Assert.True(
        stopwatch.ElapsedMilliseconds < 500,
        $"Requisição demorou {stopwatch.ElapsedMilliseconds}ms"
    );
}

[Fact]
public async Task ConcurrentRequests_SucessoParaTodas() {
    var tasks = Enumerable.Range(1, 10)
        .Select(_ => _client.GetAsync("/api/tarefas"))
        .ToList();
    
    var responses = await Task.WhenAll(tasks);
    
    Assert.All(responses, r => {
        Assert.Equal(HttpStatusCode.OK, r.StatusCode);
    });
}
```

## 🎯 Quando Usar Testes de Integração?

### ✅ Use para:
- Testar fluxos completos (end-to-end dentro do sistema)
- Verificar integração com banco de dados
- Validar APIs RESTful
- Testar autenticação e autorização
- Verificar validações de entrada

### ❌ Evite para:
- Lógica de negócio simples (use unitários)
- Métodos isolados (use unitários)
- Performance crítica (use unitários)
- Testes muito lentos (use unitários)

## 🔄 Isolamento de Testes

### Problema: Testes Interferindo

```csharp
// ❌ RUIM - Testes compartilham estado
[Fact]
public async Task CriarTarefa_PrimeiroTeste() {
    // Cria tarefa no banco
}

[Fact]
public async Task ListarTarefas_SegundoTeste() {
    // Pode ver tarefa do primeiro teste!
}
```

### Solução: Banco Isolado por Teste

```csharp
// ✅ BOM - Cada teste tem seu próprio banco
public class CustomWebApplicationFactory : WebApplicationFactory<Program> {
    protected override void ConfigureWebHost(IWebHostBuilder builder) {
        builder.ConfigureServices(services => {
            // Criar banco único por teste
            var databaseName = Guid.NewGuid().ToString();
            
            services.AddDbContext<AppDbContext>(options => {
                options.UseInMemoryDatabase(databaseName);
            });
        });
    }
}
```

## 📝 Exemplo Completo

```csharp
public class TarefasIntegrationTests : IClassFixture<CustomWebApplicationFactory> {
    private readonly HttpClient _client;
    
    public TarefasIntegrationTests(CustomWebApplicationFactory factory) {
        _client = factory.CreateClient();
    }
    
    [Fact]
    public async Task FluxoCompleto_Tarefa() {
        // 1. Criar tarefa
        var criarDto = new {
            Titulo = "Tarefa de teste",
            Descricao = "Descrição",
            Prioridade = 2
        };
        
        var criarResponse = await _client.PostAsync(
            "/api/tarefas",
            JsonContent.Create(criarDto)
        );
        
        var tarefa = await criarResponse.Content.ReadFromJsonAsync<TarefaHateoasDto>();
        var id = tarefa.Id;
        
        // 2. Verificar links HATEOAS
        Assert.Contains(tarefa.Links, l => l.Rel == "iniciar");
        
        // 3. Iniciar execução
        var iniciarResponse = await _client.PatchAsync(
            $"/api/tarefas/{id}/iniciar",
            null
        );
        
        Assert.Equal(HttpStatusCode.OK, iniciarResponse.StatusCode);
        
        // 4. Verificar novo estado
        var getResponse = await _client.GetAsync($"/api/tarefas/{id}");
        var tarefaAtualizada = await getResponse.Content.ReadFromJsonAsync<TarefaHateoasDto>();
        
        Assert.Equal("EmAndamento", tarefaAtualizada.Status);
        Assert.Contains(tarefaAtualizada.Links, l => l.Rel == "concluir");
        Assert.DoesNotContain(tarefaAtualizada.Links, l => l.Rel == "iniciar");
        
        // 5. Concluir
        var concluirResponse = await _client.PatchAsync(
            $"/api/tarefas/{id}/concluir",
            null
        );
        
        Assert.Equal(HttpStatusCode.OK, concluirResponse.StatusCode);
    }
}
```

## ✅ Verificação

- [ ] Entendeu diferença entre teste unitário e integração
- [ ] Usou WebApplicationFactory para testar APIs
- [ ] Configurou banco em memória para testes
- [ ] Testou CRUD completo de endpoints
- [ ] Testou cenários de erro e validação
- [ ] Implementou isolamento entre testes
- [ ] Testou autenticação quando necessário
- [ ] Validou performance básica

---

[← Voltar: TDD](./03-tdd.md) | [Próximo: Cobertura e Métricas →](./05-cobertura-metricas.md)