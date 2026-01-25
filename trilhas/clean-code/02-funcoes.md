# 02 - Funções

## 📖 Funções Pequenas e Focadas

> "A primeira regra das funções é que elas devem ser pequenas. A segunda regra é que elas devem ser menor ainda." - Robert C. Martin

## 🎯 Tamanho Ideal

```csharp
// ❌ RUIM - Função muito grande
public void ProcessarPedido(Pedido pedido) {
    // Validação (15 linhas)
    if (pedido == null) throw new ArgumentNullException();
    if (string.IsNullOrEmpty(pedido.ClienteNome)) throw new Exception();
    // ... mais validações ...

    // Cálculos (20 linhas)
    decimal subtotal = 0;
    foreach (var item in pedido.Itens) {
        subtotal += item.Preco * item.Quantidade;
    }
    // ... mais cálculos ...

    // Salvamento (10 linhas)
    using var connection = new SqlConnection();
    // ... código de banco ...

    // Notificação (10 linhas)
    var email = new EmailService();
    // ... envio de email ...
}

// ✅ BOM - Funções pequenas e focadas
public void ProcessarPedido(Pedido pedido) {
    ValidarPedido(pedido);
    CalcularTotais(pedido);
    SalvarPedido(pedido);
    NotificarCliente(pedido);
}

private void ValidarPedido(Pedido pedido) {
    if (pedido == null) throw new ArgumentNullException(nameof(pedido));
    if (string.IsNullOrEmpty(pedido.ClienteNome)) throw new ValidationException("Nome obrigatório");
    if (!pedido.Itens.Any()) throw new ValidationException("Pedido sem itens");
}
```

## 📋 Uma Coisa por Função

```csharp
// ❌ RUIM - Faz múltiplas coisas
public bool ProcessarUsuario(Usuario usuario) {
    // Valida
    if (string.IsNullOrEmpty(usuario.Email)) return false;
    
    // Salva
    _repository.Salvar(usuario);
    
    // Envia email
    _emailService.Enviar(usuario.Email, "Bem-vindo");
    
    // Registra log
    _logger.Log($"Usuário {usuario.Nome} criado");
    
    return true;
}

// ✅ BOM - Uma responsabilidade
public void CriarUsuario(Usuario usuario) {
    ValidarUsuario(usuario);
    SalvarUsuario(usuario);
    EnviarEmailBoasVindas(usuario);
    RegistrarCriacao(usuario);
}
```

## 🔢 Poucos Parâmetros

```csharp
// ❌ RUIM - Muitos parâmetros
public void CriarCliente(
    string nome,
    string email,
    string telefone,
    string endereco,
    string cidade,
    string estado,
    string cep,
    DateTime dataNascimento
) { }

// ✅ BOM - Objeto como parâmetro
public class DadosCliente {
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Telefone { get; set; }
    public Endereco Endereco { get; set; }
    public DateTime DataNascimento { get; set; }
}

public void CriarCliente(DadosCliente dados) { }

// ✅ BOM - Máximo 3 parâmetros
public void EnviarEmail(string destinatario, string assunto, string corpo) { }
```

## 🎯 Nomenclatura Descritiva

```csharp
// ❌ RUIM
public void Process() { }
public void DoStuff(int x) { }
public bool Check() { }

// ✅ BOM
public void ProcessarPagamento() { }
public void AtualizarEstoque(int produtoId) { }
public bool ClienteEstaAtivo() { }
```

## 🔄 Níveis de Abstração

```csharp
// ❌ RUIM - Mistura níveis
public void PrepararRelatorio() {
    // Alto nível
    var dados = BuscarDadosRelatorio();
    
    // Baixo nível
    using var connection = new SqlConnection(_connectionString);
    connection.Open();
    var command = new SqlCommand("SELECT * FROM ...", connection);
    
    // Alto nível
    GerarPDF(dados);
}

// ✅ BOM - Mesmo nível
public void PrepararRelatorio() {
    var dados = BuscarDadosRelatorio();
    ValidarDados(dados);
    var relatorio = GerarRelatorio(dados);
    SalvarRelatorio(relatorio);
}
```

## ⚠️ Tratamento de Erros

```csharp
// ❌ RUIM - Mistura lógica com erro
public decimal CalcularDesconto(decimal valor, string codigoPromocao) {
    try {
        if (valor <= 0) throw new Exception("Valor inválido");
        
        var promocao = _repository.BuscarPromocao(codigoPromocao);
        if (promocao == null) throw new Exception("Promoção inválida");
        
        return valor * (promocao.Percentual / 100);
    }
    catch (Exception ex) {
        _logger.Log(ex.Message);
        return 0;
    }
}

// ✅ BOM - Separação clara
public decimal CalcularDesconto(decimal valor, string codigoPromocao) {
    ValidarValor(valor);
    var promocao = BuscarPromocaoValida(codigoPromocao);
    return AplicarDesconto(valor, promocao);
}

private void ValidarValor(decimal valor) {
    if (valor <= 0) {
        throw new ArgumentException("Valor deve ser positivo", nameof(valor));
    }
}
```

## 🎯 Exercício Prático

```csharp
// ❌ CÓDIGO PARA REFATORAR
public class OrderProcessor {
    public bool Process(Order order) {
        if (order == null || order.Items == null || order.Items.Count == 0) {
            Console.WriteLine("Invalid order");
            return false;
        }
        
        decimal total = 0;
        foreach (var item in order.Items) {
            if (item.Price < 0) {
                Console.WriteLine("Invalid price");
                return false;
            }
            total += item.Price * item.Quantity;
        }
        
        if (order.CustomerAge < 18 && total > 100) {
            Console.WriteLine("Menor de idade não pode comprar acima de 100");
            return false;
        }
        
        using (var db = new DbContext()) {
            db.Orders.Add(order);
            db.SaveChanges();
        }
        
        var email = new EmailService();
        email.Send(order.CustomerEmail, "Pedido confirmado", $"Total: {total}");
        
        Console.WriteLine($"Order {order.Id} processed");
        return true;
    }
}

// ✅ SOLUÇÃO REFATORADA
public class ProcessadorPedido {
    private readonly IRepositorioPedido _repositorio;
    private readonly IEmailService _emailService;
    private readonly ILogger _logger;

    public void ProcessarPedido(Pedido pedido) {
        ValidarPedido(pedido);
        var total = CalcularTotal(pedido);
        ValidarRestricoes(pedido, total);
        SalvarPedido(pedido);
        EnviarConfirmacao(pedido, total);
        RegistrarProcessamento(pedido);
    }

    private void ValidarPedido(Pedido pedido) {
        if (pedido == null) {
            throw new ArgumentNullException(nameof(pedido));
        }

        if (!pedido.Itens.Any()) {
            throw new ValidationException("Pedido sem itens");
        }
    }

    private decimal CalcularTotal(Pedido pedido) {
        ValidarPrecos(pedido.Itens);
        return pedido.Itens.Sum(item => item.Preco * item.Quantidade);
    }

    private void ValidarPrecos(List<ItemPedido> itens) {
        if (itens.Any(item => item.Preco < 0)) {
            throw new ValidationException("Preço inválido encontrado");
        }
    }

    private void ValidarRestricoes(Pedido pedido, decimal total) {
        if (pedido.IdadeCliente < 18 && total > 100) {
            throw new BusinessException("Menor de idade não pode comprar acima de R$ 100");
        }
    }

    private void SalvarPedido(Pedido pedido) {
        _repositorio.Salvar(pedido);
    }

    private void EnviarConfirmacao(Pedido pedido, decimal total) {
        var assunto = "Pedido Confirmado";
        var mensagem = $"Seu pedido foi confirmado. Total: R$ {total:F2}";
        _emailService.Enviar(pedido.EmailCliente, assunto, mensagem);
    }

    private void RegistrarProcessamento(Pedido pedido) {
        _logger.LogInformation($"Pedido {pedido.Id} processado com sucesso");
    }
}
```

## ✅ Checklist de Funções

- [ ] Função tem menos de 20 linhas?
- [ ] Faz apenas uma coisa?
- [ ] Nome descreve o que faz?
- [ ] Tem 3 ou menos parâmetros?
- [ ] Não tem efeitos colaterais ocultos?
- [ ] Não mistura níveis de abstração?
- [ ] Tratamento de erro separado?

---

[← Voltar: Nomes Significativos](./01-nomes-significativos.md) | [Próximo: Comentários →](./03-comentarios.md)