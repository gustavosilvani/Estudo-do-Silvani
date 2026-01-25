# 02 - Mocks e Stubs

## 📖 Isolando Dependências

Mocks e stubs permitem testar código isoladamente, sem depender de recursos externos.

## 🎯 Mock vs Stub

```
STUB:
- Fornece respostas pre-programadas
- Verifica estado
- "Qual foi o resultado?"

MOCK:
- Verifica comportamento/interações
- "O método foi chamado corretamente?"
```

## 🔧 Moq - Framework de Mocking

```bash
dotnet add package Moq
```

### Mock Básico

```csharp
using Moq;
using Xunit;

public class ClienteServiceTests {
    [Fact]
    public void CriarCliente_ClienteValido_SalvaNoRepositorio() {
        // Arrange
        var mockRepositorio = new Mock<IRepositorioCliente>();
        var service = new ClienteService(mockRepositorio.Object);
        
        var cliente = new Cliente {
            Nome = "João",
            Email = "joao@email.com"
        };
        
        // Act
        service.CriarCliente(cliente);
        
        // Assert - Verifica que método foi chamado
        mockRepositorio.Verify(
            r => r.Adicionar(It.IsAny<Cliente>()),
            Times.Once
        );
    }
}
```

## 🎭 Configurando Retornos

```csharp
[Fact]
public void BuscarCliente_ClienteExiste_RetornaCliente() {
    // Arrange
    var mockRepositorio = new Mock<IRepositorioCliente>();
    
    // Configurar retorno do mock
    var clienteEsperado = new Cliente {
        Id = 1,
        Nome = "Maria",
        Email = "maria@email.com"
    };
    
    mockRepositorio
        .Setup(r => r.BuscarPorId(1))
        .Returns(clienteEsperado);
    
    var service = new ClienteService(mockRepositorio.Object);
    
    // Act
    var cliente = service.BuscarCliente(1);
    
    // Assert
    Assert.NotNull(cliente);
    Assert.Equal("Maria", cliente.Nome);
}

[Fact]
public async Task BuscarCliente_ClienteNaoExiste_LancaExcecao() {
    // Arrange
    var mockRepositorio = new Mock<IRepositorioCliente>();
    
    mockRepositorio
        .Setup(r => r.BuscarPorIdAsync(999))
        .ReturnsAsync((Cliente)null);  // Retorna null
    
    var service = new ClienteService(mockRepositorio.Object);
    
    // Act & Assert
    await Assert.ThrowsAsync<ClienteNaoEncontradoException>(
        () => service.BuscarClienteAsync(999)
    );
}
```

## 🎯 Matchers de Argumentos

```csharp
[Fact]
public void EnviarEmail_EmailValido_ChamaServicoCorretamente() {
    // Arrange
    var mockEmailService = new Mock<IEmailService>();
    var service = new NotificacaoService(mockEmailService.Object);
    
    // Act
    service.NotificarCliente(1);
    
    // Assert com matchers
    
    // Qualquer string
    mockEmailService.Verify(
        e => e.Enviar(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
        Times.Once
    );
    
    // String específica
    mockEmailService.Verify(
        e => e.Enviar("cliente@email.com", It.IsAny<string>(), It.IsAny<string>()),
        Times.Once
    );
    
    // Condição customizada
    mockEmailService.Verify(
        e => e.Enviar(
            It.Is<string>(email => email.Contains("@")),
            It.IsAny<string>(),
            It.IsAny<string>()
        ),
        Times.Once
    );
}
```

## 🔄 Verificações

```csharp
[Fact]
public void ProcessarPedido_PedidoValido_ChamaServiçosNaOrdem() {
    // Arrange
    var mockValidador = new Mock<IValidadorPedido>();
    var mockRepositorio = new Mock<IRepositorioPedido>();
    var mockNotificacao = new Mock<INotificacaoService>();
    
    var service = new PedidoService(
        mockValidador.Object,
        mockRepositorio.Object,
        mockNotificacao.Object
    );
    
    var pedido = new Pedido { Id = 1 };
    
    // Act
    service.ProcessarPedido(pedido);
    
    // Assert - Verificar chamadas
    mockValidador.Verify(v => v.Validar(pedido), Times.Once);
    mockRepositorio.Verify(r => r.Salvar(pedido), Times.Once);
    mockNotificacao.Verify(n => n.Notificar(pedido), Times.Once);
    
    // Verificar ordem
    var sequence = new MockSequence();
    mockValidador.InSequence(sequence).Setup(v => v.Validar(pedido));
    mockRepositorio.InSequence(sequence).Setup(r => r.Salvar(pedido));
    mockNotificacao.InSequence(sequence).Setup(n => n.Notificar(pedido));
}

[Fact]
public void CancelarPedido_PedidoCancelado_NaoEnviaNotificacao() {
    // Arrange
    var mockNotificacao = new Mock<INotificacaoService>();
    var service = new PedidoService(mockNotificacao.Object);
    
    // Act
    service.CancelarPedido(1);
    
    // Assert - Verificar que NÃO foi chamado
    mockNotificacao.Verify(
        n => n.Notificar(It.IsAny<Pedido>()),
        Times.Never
    );
}
```

## 🎯 Exemplo Completo

```csharp
// Interfaces
public interface IRepositorioCliente {
    Cliente BuscarPorId(int id);
    void Adicionar(Cliente cliente);
    bool ExistePorEmail(string email);
}

public interface IEmailService {
    void EnviarBoasVindas(string email);
}

// Serviço a ser testado
public class ClienteService {
    private readonly IRepositorioCliente _repositorio;
    private readonly IEmailService _emailService;
    
    public ClienteService(
        IRepositorioCliente repositorio,
        IEmailService emailService) {
        _repositorio = repositorio;
        _emailService = emailService;
    }
    
    public Cliente CriarCliente(string nome, string email) {
        if (string.IsNullOrEmpty(nome)) {
            throw new ArgumentException("Nome obrigatório");
        }
        
        if (_repositorio.ExistePorEmail(email)) {
            throw new ClienteDuplicadoException("Email já existe");
        }
        
        var cliente = new Cliente { Nome = nome, Email = email };
        _repositorio.Adicionar(cliente);
        _emailService.EnviarBoasVindas(email);
        
        return cliente;
    }
}

// Testes completos
public class ClienteServiceTests {
    private readonly Mock<IRepositorioCliente> _mockRepositorio;
    private readonly Mock<IEmailService> _mockEmailService;
    private readonly ClienteService _service;
    
    public ClienteServiceTests() {
        _mockRepositorio = new Mock<IRepositorioCliente>();
        _mockEmailService = new Mock<IEmailService>();
        _service = new ClienteService(
            _mockRepositorio.Object,
            _mockEmailService.Object
        );
    }
    
    [Fact]
    public void CriarCliente_DadosValidos_CriaClienteComSucesso() {
        // Arrange
        _mockRepositorio
            .Setup(r => r.ExistePorEmail(It.IsAny<string>()))
            .Returns(false);
        
        // Act
        var cliente = _service.CriarCliente("João", "joao@email.com");
        
        // Assert
        Assert.NotNull(cliente);
        Assert.Equal("João", cliente.Nome);
        
        _mockRepositorio.Verify(
            r => r.Adicionar(It.Is<Cliente>(c => c.Nome == "João")),
            Times.Once
        );
        
        _mockEmailService.Verify(
            e => e.EnviarBoasVindas("joao@email.com"),
            Times.Once
        );
    }
    
    [Fact]
    public void CriarCliente_NomeVazio_LancaExcecao() {
        // Act & Assert
        Assert.Throws<ArgumentException>(
            () => _service.CriarCliente("", "joao@email.com")
        );
        
        // Verificar que repositório NÃO foi chamado
        _mockRepositorio.Verify(
            r => r.Adicionar(It.IsAny<Cliente>()),
            Times.Never
        );
    }
    
    [Fact]
    public void CriarCliente_EmailDuplicado_LancaExcecao() {
        // Arrange
        _mockRepositorio
            .Setup(r => r.ExistePorEmail("joao@email.com"))
            .Returns(true);
        
        // Act & Assert
        Assert.Throws<ClienteDuplicadoException>(
            () => _service.CriarCliente("João", "joao@email.com")
        );
    }
}
```

## ✅ Verificação

- [ ] Criou mocks com Moq
- [ ] Configurou retornos com Setup
- [ ] Verificou chamadas com Verify
- [ ] Usou matchers (It.IsAny, It.Is)
- [ ] Testou cenários de sucesso e erro
- [ ] Isolou dependências completamente

---

[← Voltar: xUnit](./01-xunit-primeiros-testes.md) | [Próximo: TDD →](./03-tdd.md)