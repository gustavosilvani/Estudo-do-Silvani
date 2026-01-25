# 07 - Testes de Integração

## 📖 O que são Testes de Integração?

Testes que verificam a interação entre múltiplos componentes do sistema, incluindo banco de dados, APIs externas, etc.

## 🎯 Diferença: Unitário vs Integração

```
Teste Unitário:
- Testa uma unidade isolada
- Usa mocks para dependências
- Rápido (milissegundos)
- Muitos testes

Teste de Integração:
- Testa componentes juntos
- Usa dependências reais
- Mais lento (segundos)
- Menos testes, mais críticos
```

## 🔧 WebApplicationFactory

```csharp
// Projeto de Testes
<PackageReference Include="Microsoft.AspNetCore.Mvc.Testing" Version="8.0.0" />

// Classe base para testes de integração
public class IntegrationTestBase : IClassFixture<WebApplicationFactory<Program>> {
    protected readonly HttpClient _client;
    protected readonly WebApplicationFactory<Program> _factory;
    
    public IntegrationTestBase(WebApplicationFactory<Program> factory) {
        _factory = factory;
        _client = factory.CreateClient();
    }
}

// Testes
public class ProdutosIntegrationTests : IntegrationTestBase {
    public ProdutosIntegrationTests(WebApplicationFactory<Program> factory)
        : base(factory) { }
    
    [Fact]
    public async Task GetTodos_RetornaListaDeProdutos() {
        // Act
        var response = await _client.GetAsync("/api/produtos");
        
        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var produtos = JsonSerializer.Deserialize<List<ProdutoDto>>(content);
        
        Assert.NotNull(produtos);
        Assert.NotEmpty(produtos);
    }
    
    [Fact]
    public async Task Post_CriarProduto_Retorna201Created() {
        // Arrange
        var novoProduto = new {
            Nome = "Teste Produto",
            Preco = 99.99m,
            EstoqueAtual = 10
        };
        
        var json = JsonSerializer.Serialize(novoProduto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        // Act
        var response = await _client.PostAsync("/api/produtos", content);
        
        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(response.Headers.Location);
    }
}
```

## 🗄️ Banco de Dados em Memória

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
        context.Produtos.AddRange(
            new Produto { Id = 1, Nome = "Produto 1", Preco = 10 },
            new Produto { Id = 2, Nome = "Produto 2", Preco = 20 }
        );
        context.SaveChanges();
    }
}

// Uso
public class ProdutosTests : IClassFixture<CustomWebApplicationFactory> {
    private readonly HttpClient _client;
    
    public ProdutosTests(CustomWebApplicationFactory factory) {
        _client = factory.CreateClient();
    }
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
        // ...
        return "test_token_here";
    }
}

[Fact]
public async Task GetPedidos_ComAutenticacao_Retorna200() {
    // Cliente já tem token configurado
    var response = await _client.GetAsync("/api/pedidos");
    
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
}
```

## 🧪 Testes de Endpoint Completos

```csharp
public class ProdutosEndpointTests : IClassFixture<CustomWebApplicationFactory> {
    private readonly HttpClient _client;
    
    public ProdutosEndpointTests(CustomWebApplicationFactory factory) {
        _client = factory.CreateClient();
    }
    
    [Fact]
    public async Task CrudCompleto_Produto() {
        // 1. CREATE
        var criarDto = new {
            Nome = "Novo Produto",
            Descricao = "Teste",
            Preco = 150.00m,
            EstoqueAtual = 20
        };
        
        var createResponse = await _client.PostAsync(
            "/api/produtos",
            JsonContent.Create(criarDto)
        );
        
        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);
        
        var produtoCriado = await createResponse.Content
            .ReadFromJsonAsync<ProdutoDto>();
        var id = produtoCriado.Id;
        
        // 2. READ
        var getResponse = await _client.GetAsync($"/api/produtos/{id}");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
        
        var produto = await getResponse.Content.ReadFromJsonAsync<ProdutoDto>();
        Assert.Equal("Novo Produto", produto.Nome);
        
        // 3. UPDATE
        var atualizarDto = new {
            Id = id,
            Nome = "Produto Atualizado",
            Preco = 200.00m
        };
        
        var updateResponse = await _client.PutAsync(
            $"/api/produtos/{id}",
            JsonContent.Create(atualizarDto)
        );
        
        Assert.Equal(HttpStatusCode.NoContent, updateResponse.StatusCode);
        
        // 4. DELETE
        var deleteResponse = await _client.DeleteAsync($"/api/produtos/{id}");
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
        
        // 5. Verificar que foi deletado
        var getDeletedResponse = await _client.GetAsync($"/api/produtos/{id}");
        Assert.Equal(HttpStatusCode.NotFound, getDeletedResponse.StatusCode);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task CriarProduto_NomeInvalido_Retorna400(string nomeInvalido) {
        // Arrange
        var dto = new {
            Nome = nomeInvalido,
            Preco = 100m
        };
        
        // Act
        var response = await _client.PostAsync(
            "/api/produtos",
            JsonContent.Create(dto)
        );
        
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
```

## 📊 Testes de Performance

```csharp
[Fact]
public async Task GetTodos_Performance_MenorQue500ms() {
    var stopwatch = Stopwatch.StartNew();
    
    var response = await _client.GetAsync("/api/produtos");
    
    stopwatch.Stop();
    
    Assert.True(
        stopwatch.ElapsedMilliseconds < 500,
        $"Requisição demorou {stopwatch.ElapsedMilliseconds}ms"
    );
}

[Fact]
public async Task ConcurrentRequests_SucessoParaTodas() {
    var tasks = Enumerable.Range(1, 10)
        .Select(_ => _client.GetAsync("/api/produtos"))
        .ToList();
    
    var responses = await Task.WhenAll(tasks);
    
    Assert.All(responses, r => {
        Assert.Equal(HttpStatusCode.OK, r.StatusCode);
    });
}
```

## ✅ Verificação

- [ ] Entendeu diferença entre teste unitário e integração
- [ ] Usou WebApplicationFactory
- [ ] Testou endpoints completos
- [ ] Configurou banco em memória
- [ ] Testou CRUD completo
- [ ] Testou cenários de erro

---

**🎉 Parabéns! Você completou a trilha C#/.NET!**

[← Voltar: Dependency Injection](./06-dependency-injection.md) | [Voltar à Trilha](./README.md)