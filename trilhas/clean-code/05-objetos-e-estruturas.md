# 05 - Objetos e Estruturas de Dados

## 📖 Abstração de Dados

Objetos escondem dados e expõem comportamento. Estruturas expõem dados e não têm comportamento significativo.

## 🎯 Lei de Demeter

**"Fale apenas com seus amigos imediatos"**

```csharp
// ❌ RUIM - Viola Lei de Demeter
public class Pedido {
    public void ProcessarPagamento() {
        var endereco = Cliente.Endereco.Rua;  // Trem de chamadas
        var cep = Cliente.Endereco.CEP;
        var taxa = Cliente.Endereco.Cidade.TaxaEntrega;
    }
}

// ✅ BOM - Respeita Lei de Demeter
public class Pedido {
    public void ProcessarPagamento() {
        var taxaEntrega = Cliente.ObterTaxaEntrega();
    }
}

public class Cliente {
    public decimal ObterTaxaEntrega() {
        return Endereco.ObterTaxaEntrega();
    }
}
```

## 🔐 Encapsulamento

```csharp
// ❌ RUIM - Expõe implementação
public class ContaBancaria {
    public decimal Saldo { get; set; }  // Qualquer um pode modificar!
    
    public void Sacar(decimal valor) {
        Saldo -= valor;  // Sem validação
    }
}

// ✅ BOM - Encapsula comportamento
public class ContaBancaria {
    private decimal _saldo;
    
    public decimal ConsultarSaldo() => _saldo;
    
    public void Depositar(decimal valor) {
        if (valor <= 0) {
            throw new ArgumentException("Valor deve ser positivo");
        }
        _saldo += valor;
    }
    
    public void Sacar(decimal valor) {
        if (valor <= 0) {
            throw new ArgumentException("Valor deve ser positivo");
        }
        
        if (valor > _saldo) {
            throw new InvalidOperationException("Saldo insuficiente");
        }
        
        _saldo -= valor;
    }
}
```

## 🎭 Objetos vs Estruturas de Dados

### DTOs (Data Transfer Objects)
```csharp
// ✅ DTO - Apenas dados
public class ClienteDTO {
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public DateTime DataNascimento { get; set; }
}

// ✅ Objeto - Comportamento + dados encapsulados
public class Cliente {
    private readonly string _email;
    
    public int Id { get; private set; }
    public string Nome { get; private set; }
    public DateTime DataNascimento { get; private set; }
    
    public Cliente(string nome, string email, DateTime dataNascimento) {
        ValidarEmail(email);
        Nome = nome;
        _email = email;
        DataNascimento = dataNascimento;
    }
    
    public int CalcularIdade() {
        var hoje = DateTime.Today;
        var idade = hoje.Year - DataNascimento.Year;
        if (DataNascimento.Date > hoje.AddYears(-idade)) idade--;
        return idade;
    }
    
    public bool EhMaiorDeIdade() => CalcularIdade() >= 18;
    
    private void ValidarEmail(string email) {
        if (string.IsNullOrEmpty(email) || !email.Contains("@")) {
            throw new ArgumentException("Email inválido");
        }
    }
}
```

## 🔄 Imutabilidade

```csharp
// ✅ Objeto imutável (preferível)
public class Dinheiro {
    public decimal Valor { get; }
    public string Moeda { get; }
    
    public Dinheiro(decimal valor, string moeda) {
        if (valor < 0) throw new ArgumentException("Valor não pode ser negativo");
        Valor = valor;
        Moeda = moeda ?? throw new ArgumentNullException(nameof(moeda));
    }
    
    // Retorna novo objeto ao invés de modificar
    public Dinheiro Adicionar(Dinheiro outro) {
        if (Moeda != outro.Moeda) {
            throw new InvalidOperationException("Moedas diferentes");
        }
        return new Dinheiro(Valor + outro.Valor, Moeda);
    }
}

// Uso
var dinheiro1 = new Dinheiro(100, "BRL");
var dinheiro2 = new Dinheiro(50, "BRL");
var total = dinheiro1.Adicionar(dinheiro2);  // Novo objeto
// dinheiro1 permanece inalterado
```

## 🎯 Value Objects

```csharp
// ✅ Value Object bem implementado
public class Email {
    public string Endereco { get; }
    
    public Email(string endereco) {
        if (string.IsNullOrWhiteSpace(endereco)) {
            throw new ArgumentException("Email não pode ser vazio");
        }
        
        if (!endereco.Contains("@")) {
            throw new ArgumentException("Email inválido");
        }
        
        Endereco = endereco.ToLower().Trim();
    }
    
    // Value objects devem ser comparáveis por valor
    public override bool Equals(object obj) {
        return obj is Email other && Endereco == other.Endereco;
    }
    
    public override int GetHashCode() => Endereco.GetHashCode();
    
    public override string ToString() => Endereco;
    
    // Operadores opcionais
    public static bool operator ==(Email left, Email right) {
        return Equals(left, right);
    }
    
    public static bool operator !=(Email left, Email right) {
        return !Equals(left, right);
    }
}

// Uso
var email1 = new Email("joao@email.com");
var email2 = new Email("JOAO@email.com");
Console.WriteLine(email1 == email2);  // true - comparação por valor
```

## 🏗️ Builder Pattern

```csharp
// ✅ Builder para objetos complexos
public class Pedido {
    public Cliente Cliente { get; private set; }
    public List<ItemPedido> Itens { get; private set; }
    public Endereco EnderecoEntrega { get; private set; }
    public string Observacoes { get; private set; }
    
    private Pedido() {
        Itens = new List<ItemPedido>();
    }
    
    public class Builder {
        private readonly Pedido _pedido = new Pedido();
        
        public Builder ParaCliente(Cliente cliente) {
            _pedido.Cliente = cliente;
            return this;
        }
        
        public Builder ComItem(ItemPedido item) {
            _pedido.Itens.Add(item);
            return this;
        }
        
        public Builder EntregarEm(Endereco endereco) {
            _pedido.EnderecoEntrega = endereco;
            return this;
        }
        
        public Builder ComObservacoes(string observacoes) {
            _pedido.Observacoes = observacoes;
            return this;
        }
        
        public Pedido Build() {
            Validar();
            return _pedido;
        }
        
        private void Validar() {
            if (_pedido.Cliente == null) {
                throw new InvalidOperationException("Cliente obrigatório");
            }
            if (!_pedido.Itens.Any()) {
                throw new InvalidOperationException("Pedido sem itens");
            }
        }
    }
}

// Uso fluente
var pedido = new Pedido.Builder()
    .ParaCliente(cliente)
    .ComItem(item1)
    .ComItem(item2)
    .EntregarEm(endereco)
    .ComObservacoes("Entregar pela manhã")
    .Build();
```

## 🎯 Exercício Prático

```csharp
// ❌ CÓDIGO PARA REFATORAR
public class Order {
    public string CustomerName;
    public string CustomerEmail;
    public string CustomerAddress;
    public List<Product> Products;
    public decimal Total;
    
    public void Calculate() {
        Total = 0;
        foreach (var p in Products) {
            Total += p.Price * p.Quantity;
        }
    }
}

// ✅ SOLUÇÃO REFATORADA
public class Pedido {
    public Cliente Cliente { get; private set; }
    private readonly List<ItemPedido> _itens;
    
    public IReadOnlyList<ItemPedido> Itens => _itens.AsReadOnly();
    
    public Pedido(Cliente cliente) {
        Cliente = cliente ?? throw new ArgumentNullException(nameof(cliente));
        _itens = new List<ItemPedido>();
    }
    
    public void AdicionarItem(Produto produto, int quantidade) {
        if (produto == null) throw new ArgumentNullException(nameof(produto));
        if (quantidade <= 0) throw new ArgumentException("Quantidade inválida");
        
        var item = new ItemPedido(produto, quantidade);
        _itens.Add(item);
    }
    
    public decimal CalcularTotal() {
        return _itens.Sum(item => item.CalcularSubtotal());
    }
}

public class Cliente {
    public string Nome { get; }
    public Email Email { get; }
    public Endereco Endereco { get; }
    
    public Cliente(string nome, Email email, Endereco endereco) {
        if (string.IsNullOrWhiteSpace(nome)) {
            throw new ArgumentException("Nome obrigatório");
        }
        Nome = nome;
        Email = email ?? throw new ArgumentNullException(nameof(email));
        Endereco = endereco ?? throw new ArgumentNullException(nameof(endereco));
    }
}

public class ItemPedido {
    public Produto Produto { get; }
    public int Quantidade { get; }
    
    public ItemPedido(Produto produto, int quantidade) {
        Produto = produto ?? throw new ArgumentNullException(nameof(produto));
        
        if (quantidade <= 0) {
            throw new ArgumentException("Quantidade deve ser positiva");
        }
        Quantidade = quantidade;
    }
    
    public decimal CalcularSubtotal() => Produto.Preco * Quantidade;
}
```

## ✅ Checklist

- [ ] Objetos encapsulam dados?
- [ ] Comportamento está nos objetos certos?
- [ ] Respeita Lei de Demeter?
- [ ] Value Objects são imutáveis?
- [ ] DTOs são apenas dados?
- [ ] Validações no construtor?

---

[← Voltar: Formatação](./04-formatacao.md) | [Próximo: Tratamento de Erros →](./06-tratamento-de-erros.md)