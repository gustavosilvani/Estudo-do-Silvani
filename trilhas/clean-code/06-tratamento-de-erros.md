# 06 - Tratamento de Erros

## 📖 Erros Fazem Parte do Código

Tratamento de erros é importante, mas não deve obscurecer a lógica do código.

## 🎯 Use Exceções, Não Códigos de Erro

```csharp
// ❌ RUIM - Códigos de erro
public int ProcessarPagamento(decimal valor) {
    if (valor <= 0) return -1;  // Erro: valor inválido
    if (valor > saldo) return -2;  // Erro: saldo insuficiente
    
    saldo -= valor;
    return 0;  // Sucesso
}

// Uso ruim
int resultado = ProcessarPagamento(100);
if (resultado == -1) {
    // tratar erro
} else if (resultado == -2) {
    // tratar outro erro
}

// ✅ BOM - Exceções
public void ProcessarPagamento(decimal valor) {
    if (valor <= 0) {
        throw new ArgumentException("Valor deve ser positivo", nameof(valor));
    }
    
    if (valor > _saldo) {
        throw new SaldoInsuficienteException($"Saldo: {_saldo}, Valor: {valor}");
    }
    
    _saldo -= valor;
}

// Uso bom
try {
    ProcessarPagamento(100);
} catch (ArgumentException ex) {
    Console.WriteLine($"Parâmetro inválido: {ex.Message}");
} catch (SaldoInsuficienteException ex) {
    Console.WriteLine($"Saldo insuficiente: {ex.Message}");
}
```

## 🏷️ Exceções Customizadas

```csharp
// ✅ Exceção de negócio
public class SaldoInsuficienteException : Exception {
    public decimal SaldoAtual { get; }
    public decimal ValorSolicitado { get; }
    
    public SaldoInsuficienteException(decimal saldoAtual, decimal valorSolicitado)
        : base($"Saldo insuficiente. Disponível: {saldoAtual}, Solicitado: {valorSolicitado}") {
        SaldoAtual = saldoAtual;
        ValorSolicitado = valorSolicitado;
    }
}

// ✅ Exceção de validação
public class ValidationException : Exception {
    public List<string> Erros { get; }
    
    public ValidationException(List<string> erros)
        : base("Erros de validação encontrados") {
        Erros = erros;
    }
    
    public ValidationException(string erro) : base(erro) {
        Erros = new List<string> { erro };
    }
}
```

## 🔄 Não Retorne Null

```csharp
// ❌ RUIM - Retorna null
public Cliente BuscarCliente(int id) {
    var cliente = _repository.Find(id);
    return cliente;  // Pode ser null!
}

// Uso perigoso
var cliente = BuscarCliente(123);
var nome = cliente.Nome;  // NullReferenceException!

// ✅ BOM - Lança exceção
public Cliente BuscarCliente(int id) {
    var cliente = _repository.Find(id);
    if (cliente == null) {
        throw new ClienteNaoEncontradoException(id);
    }
    return cliente;
}

// ✅ BOM - Retorna Optional/Maybe (C# 8+)
public Cliente? BuscarClienteOpcional(int id) {
    return _repository.Find(id);  // Nullable explícito
}

// Uso seguro
var cliente = BuscarClienteOpcional(123);
if (cliente != null) {
    var nome = cliente.Nome;
}

// ✅ BOM - Retorna coleção vazia ao invés de null
public List<Cliente> BuscarTodos() {
    var clientes = _repository.GetAll();
    return clientes ?? new List<Cliente>();  // Nunca null
}
```

## 🚫 Não Passe Null

```csharp
// ❌ RUIM - Aceita null
public void ProcessarPedido(Pedido pedido) {
    if (pedido != null) {  // Programação defensiva ruim
        // processar...
    }
}

// ✅ BOM - Valida no início
public void ProcessarPedido(Pedido pedido) {
    if (pedido == null) {
        throw new ArgumentNullException(nameof(pedido));
    }
    
    // processar sem preocupação com null
    CalcularTotal(pedido);
    SalvarPedido(pedido);
}

// ✅ Ainda melhor - Nullable reference types (C# 8+)
public void ProcessarPedido(Pedido pedido) {  // Nunca null
    // Compilador garante que pedido não é null
    CalcularTotal(pedido);
}

public void ProcessarPedidoOpcional(Pedido? pedido) {  // Pode ser null
    if (pedido == null) return;  // Compilador força verificação
    CalcularTotal(pedido);
}
```

## 🎯 Separe Lógica de Tratamento de Erro

```csharp
// ❌ RUIM - Misturado
public void ProcessarPedidos(List<Pedido> pedidos) {
    foreach (var pedido in pedidos) {
        try {
            ValidarPedido(pedido);
            CalcularTotal(pedido);
            SalvarPedido(pedido);
            NotificarCliente(pedido);
        } catch (ValidationException ex) {
            _logger.LogWarning($"Pedido {pedido.Id} inválido: {ex.Message}");
        } catch (Exception ex) {
            _logger.LogError(ex, $"Erro ao processar pedido {pedido.Id}");
        }
    }
}

// ✅ BOM - Separado
public void ProcessarPedidos(List<Pedido> pedidos) {
    foreach (var pedido in pedidos) {
        ProcessarPedidoComTratamento(pedido);
    }
}

private void ProcessarPedidoComTratamento(Pedido pedido) {
    try {
        ProcessarPedido(pedido);
    } catch (ValidationException ex) {
        TratarErroValidacao(pedido, ex);
    } catch (Exception ex) {
        TratarErroGenerico(pedido, ex);
    }
}

private void ProcessarPedido(Pedido pedido) {
    ValidarPedido(pedido);
    CalcularTotal(pedido);
    SalvarPedido(pedido);
    NotificarCliente(pedido);
}

private void TratarErroValidacao(Pedido pedido, ValidationException ex) {
    _logger.LogWarning($"Pedido {pedido.Id} inválido: {ex.Message}");
}

private void TratarErroGenerico(Pedido pedido, Exception ex) {
    _logger.LogError(ex, $"Erro ao processar pedido {pedido.Id}");
}
```

## 🔧 Result Pattern

```csharp
// ✅ Pattern para retornar sucesso/erro sem exceções
public class Result<T> {
    public bool Success { get; }
    public T Value { get; }
    public string Error { get; }
    
    private Result(bool success, T value, string error) {
        Success = success;
        Value = value;
        Error = error;
    }
    
    public static Result<T> Ok(T value) => new Result<T>(true, value, null);
    public static Result<T> Fail(string error) => new Result<T>(false, default, error);
}

// Uso
public Result<Cliente> CriarCliente(string nome, string email) {
    if (string.IsNullOrEmpty(nome)) {
        return Result<Cliente>.Fail("Nome obrigatório");
    }
    
    if (!email.Contains("@")) {
        return Result<Cliente>.Fail("Email inválido");
    }
    
    var cliente = new Cliente(nome, email);
    return Result<Cliente>.Ok(cliente);
}

// Consumo
var resultado = CriarCliente("João", "joao@email.com");
if (resultado.Success) {
    Console.WriteLine($"Cliente criado: {resultado.Value.Nome}");
} else {
    Console.WriteLine($"Erro: {resultado.Error}");
}
```

## 🎯 Exercício Prático

```csharp
// ❌ CÓDIGO PARA REFATORAR
public class UserService {
    public int CreateUser(string name, string email) {
        if (string.IsNullOrEmpty(name)) return -1;
        if (!email.Contains("@")) return -2;
        
        var user = _repo.Find(email);
        if (user != null) return -3;
        
        try {
            _repo.Add(new User { Name = name, Email = email });
            return 1;
        } catch {
            return -4;
        }
    }
}

// ✅ SOLUÇÃO REFATORADA
public class UsuarioService {
    private readonly IRepositorioUsuario _repositorio;
    private readonly ILogger<UsuarioService> _logger;
    
    public UsuarioService(
        IRepositorioUsuario repositorio,
        ILogger<UsuarioService> logger) {
        _repositorio = repositorio;
        _logger = logger;
    }
    
    public Usuario CriarUsuario(string nome, string email) {
        ValidarDados(nome, email);
        VerificarDuplicidade(email);
        
        var usuario = new Usuario(nome, email);
        
        try {
            _repositorio.Adicionar(usuario);
            _logger.LogInformation($"Usuário criado: {email}");
            return usuario;
        } catch (Exception ex) {
            _logger.LogError(ex, $"Erro ao criar usuário: {email}");
            throw new ErroAoCriarUsuarioException("Erro ao salvar usuário", ex);
        }
    }
    
    private void ValidarDados(string nome, string email) {
        if (string.IsNullOrWhiteSpace(nome)) {
            throw new ValidationException("Nome é obrigatório");
        }
        
        if (string.IsNullOrWhiteSpace(email) || !email.Contains("@")) {
            throw new ValidationException("Email inválido");
        }
    }
    
    private void VerificarDuplicidade(string email) {
        if (_repositorio.ExistePorEmail(email)) {
            throw new UsuarioDuplicadoException($"Usuário já existe: {email}");
        }
    }
}

// Exceções customizadas
public class ValidationException : Exception {
    public ValidationException(string message) : base(message) { }
}

public class UsuarioDuplicadoException : Exception {
    public UsuarioDuplicadoException(string message) : base(message) { }
}

public class ErroAoCriarUsuarioException : Exception {
    public ErroAoCriarUsuarioException(string message, Exception innerException)
        : base(message, innerException) { }
}
```

## ✅ Checklist

- [ ] Usa exceções ao invés de códigos de erro?
- [ ] Exceções são específicas e informativas?
- [ ] Não retorna null quando pode lançar exceção?
- [ ] Valida parâmetros no início?
- [ ] Tratamento de erro separado da lógica?
- [ ] Exceções customizadas quando apropriado?
- [ ] Log de erros implementado?

---

**🎉 Parabéns! Você completou a trilha Clean Code!**

[← Voltar: Objetos e Estruturas](./05-objetos-e-estruturas.md) | [Voltar à Trilha](./README.md)