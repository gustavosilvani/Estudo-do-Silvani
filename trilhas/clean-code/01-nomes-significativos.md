# 01 - Nomes Significativos

## 📖 A Importância dos Nomes

> "Há apenas duas coisas difíceis em Ciência da Computação: invalidação de cache e nomear coisas." - Phil Karlton

Nomes são a documentação mais lida do seu código. Escolha-os com cuidado.

## 🎯 Princípio Fundamental

**Um bom nome revela sua intenção sem precisar de comentários.**

## 📋 Regras de Nomenclatura

### 1. Use Nomes que Revelam Intenção

```csharp
// ❌ RUIM - Não revela intenção
int d; // dias

// ✅ BOM - Claro e explícito
int diasDesdeModificacao;
int diasAteVencimento;
int idadeEmDias;
```

### 2. Evite Desinformação

```csharp
// ❌ RUIM - "List" mas não é uma List
var accountList = new Dictionary<int, Account>();

// ✅ BOM - Nome correto
var accounts = new Dictionary<int, Account>();
var accountMap = new Dictionary<int, Account>();

// ❌ RUIM - Nomes muito similares
class XYZControllerForEfficientHandlingOfStrings { }
class XYZControllerForEfficientStorageOfStrings { }

// ✅ BOM - Distinção clara
class StringParser { }
class StringRepository { }
```

### 3. Faça Distinções Significativas

```csharp
// ❌ RUIM - Noise words
public class ProductInfo { }
public class ProductData { }
public void GetProduct() { }
public void GetProductInfo() { }

// ✅ BOM - Distinções claras
public class Product { }
public class ProductDetails { }
public Product GetProduct(int id) { }
public ProductDetails GetProductDetails(int id) { }
```

### 4. Use Nomes Pronunciáveis

```csharp
// ❌ RUIM - Impossível pronunciar
DateTime genymdhms;  // generation year, month, day, hour, minute, second

// ✅ BOM - Pronunciável
DateTime generationTimestamp;
DateTime createdAt;
```

### 5. Use Nomes Buscáveis

```csharp
// ❌ RUIM - Números mágicos
if (s == 4) { }
tasks[4] = new Task();

// ✅ BOM - Constantes nomeadas
const int STATUS_ATIVO = 4;
const int PRIORIDADE_MAXIMA = 4;

if (status == STATUS_ATIVO) { }
tasks[PRIORIDADE_MAXIMA] = new Task();
```

## 🏷️ Convenções C#

### Classes e Interfaces

```csharp
// PascalCase
public class ClienteService { }
public class PedidoRepository { }

// Interfaces começam com I
public interface IRepositorio { }
public interface INotificador { }
```

### Métodos e Propriedades

```csharp
public class Cliente {
    // PascalCase
    public string Nome { get; set; }
    public DateTime DataNascimento { get; set; }

    public void CalcularIdade() { }
    public bool ValidarEmail() { }
}
```

### Variáveis e Parâmetros

```csharp
// camelCase
public void ProcessarPedido(int pedidoId, decimal valorTotal) {
    var clienteAtual = BuscarCliente(pedidoId);
    var descontoAplicado = CalcularDesconto(valorTotal);
}
```

### Constantes

```csharp
// SCREAMING_SNAKE_CASE ou PascalCase
public const int MAX_TENTATIVAS = 3;
public const string URL_PADRAO = "https://api.exemplo.com";

// Ou
public const int MaxTentativas = 3;
public const string UrlPadrao = "https://api.exemplo.com";
```

## 🎯 Contexto é Importante

```csharp
// ❌ RUIM - Repetição desnecessária
public class Cliente {
    public string ClienteNome { get; set; }
    public string ClienteEmail { get; set; }
    public string ClienteTelefone { get; set; }
}

// ✅ BOM - Contexto dado pela classe
public class Cliente {
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Telefone { get; set; }
}
```

## 🧪 Exemplos Práticos

### Sistema de E-commerce

```csharp
// ❌ VERSÃO RUIM
public class Sys {
    public void Prc(int x, int y, double z) {
        var t = GetT(x);
        var p = GetP(y);
        var r = CalcR(p, z);
        SaveR(t, r);
    }
}

// ✅ VERSÃO BOA
public class PedidoService {
    public void ProcessarPedido(int clienteId, int produtoId, decimal quantidade) {
        var cliente = BuscarCliente(clienteId);
        var produto = BuscarProduto(produtoId);
        var pedido = CriarPedido(produto, quantidade);
        SalvarPedido(cliente, pedido);
    }

    private Cliente BuscarCliente(int clienteId) { }
    private Produto BuscarProduto(int produtoId) { }
    private Pedido CriarPedido(Produto produto, decimal quantidade) { }
    private void SalvarPedido(Cliente cliente, Pedido pedido) { }
}
```

### Sistema Bancário

```csharp
// ❌ RUIM
public class Acc {
    public decimal bal;
    public void Dep(decimal amt) { bal += amt; }
    public bool Wdw(decimal amt) {
        if (amt <= bal) { bal -= amt; return true; }
        return false;
    }
}

// ✅ BOM
public class ContaBancaria {
    private decimal _saldo;

    public decimal Saldo => _saldo;

    public void Depositar(decimal valor) {
        if (valor > 0) {
            _saldo += valor;
        }
    }

    public bool Sacar(decimal valor) {
        if (valor > 0 && valor <= _saldo) {
            _saldo -= valor;
            return true;
        }
        return false;
    }
}
```

## 🎯 Exercício Prático

### Refatorar Código Ruim

```csharp
// ❌ CÓDIGO PARA REFATORAR
public class Mgr {
    private List<object> lst;
    
    public void Add(object x) {
        if (x != null) lst.Add(x);
    }
    
    public object Get(int i) {
        return i < lst.Count ? lst[i] : null;
    }
    
    public void Del(int i) {
        if (i < lst.Count) lst.RemoveAt(i);
    }
}

// ✅ SOLUÇÃO
public class GerenciadorProdutos {
    private List<Produto> _produtos;
    
    public void AdicionarProduto(Produto produto) {
        if (produto != null) {
            _produtos.Add(produto);
        }
    }
    
    public Produto BuscarProdutoPorIndice(int indice) {
        return indice < _produtos.Count ? _produtos[indice] : null;
    }
    
    public void RemoverProduto(int indice) {
        if (indice >= 0 && indice < _produtos.Count) {
            _produtos.RemoveAt(indice);
        }
    }
}
```

## ✅ Checklist de Nomes

Antes de commitar código, pergunte:

- [ ] O nome revela intenção?
- [ ] É pronunciável?
- [ ] É buscável?
- [ ] Evita abreviações obscuras?
- [ ] Segue convenções C#?
- [ ] Tem contexto apropriado?
- [ ] Evita "noise words"?

---

[← Voltar: Introdução](./00-introducao.md) | [Próximo: Funções →](./02-funcoes.md)