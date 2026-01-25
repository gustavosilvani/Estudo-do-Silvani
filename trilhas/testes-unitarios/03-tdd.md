# 03 - TDD - Test-Driven Development

## 📖 O que é TDD?

TDD (Test-Driven Development) é uma prática onde você **escreve testes ANTES** de escrever o código de produção.

## 🔄 Ciclo Red-Green-Refactor

```
1. RED ❌
   Escreva um teste que falha

2. GREEN ✅
   Escreva o mínimo de código para passar

3. REFACTOR 🔄
   Melhore o código mantendo testes verdes
```

## 🎯 Exemplo Prático - Calculadora

### Passo 1: RED ❌

```csharp
// Teste primeiro (vai falhar - classe não existe)
public class CalculadoraTests {
    [Fact]
    public void Somar_DoisNumeros_RetornaSoma() {
        // Arrange
        var calc = new Calculadora();  // Não existe ainda!
        
        // Act
        var resultado = calc.Somar(2, 3);
        
        // Assert
        Assert.Equal(5, resultado);
    }
}

// Resultado: ❌ Teste falha - Calculadora não existe
```

### Passo 2: GREEN ✅

```csharp
// Implementação mínima para passar
public class Calculadora {
    public int Somar(int a, int b) {
        return 5;  // Hard-coded para passar!
    }
}

// Resultado: ✅ Teste passa
```

### Passo 3: Adicionar mais testes (RED ❌)

```csharp
[Theory]
[InlineData(2, 3, 5)]
[InlineData(0, 0, 0)]
[InlineData(-1, 1, 0)]
[InlineData(10, 5, 15)]
public void Somar_VariosNumeros_RetornaSomaCorreta(int a, int b, int esperado) {
    var calc = new Calculadora();
    var resultado = calc.Somar(a, b);
    Assert.Equal(esperado, resultado);
}

// Resultado: ❌ Alguns testes falham
```

### Passo 4: Implementação real (GREEN ✅)

```csharp
public class Calculadora {
    public int Somar(int a, int b) {
        return a + b;  // Implementação real
    }
}

// Resultado: ✅ Todos testes passam
```

### Passo 5: REFACTOR 🔄

```csharp
// Código já está bom, não precisa refatorar
// Mas se precisasse, testes garantiriam que funciona
```

## 🎯 Exemplo Completo - Validador de CPF

### Iteração 1

```csharp
// 1. RED - Teste primeiro
[Fact]
public void ValidarCPF_CPFValido_RetornaTrue() {
    var validador = new ValidadorCPF();
    var resultado = validador.Validar("12345678909");
    Assert.True(resultado);
}

// 2. GREEN - Implementação mínima
public class ValidadorCPF {
    public bool Validar(string cpf) {
        return true;  // Sempre true para passar
    }
}
```

### Iteração 2

```csharp
// 1. RED - Teste que falha
[Fact]
public void ValidarCPF_CPFInvalido_RetornaFalse() {
    var validador = new ValidadorCPF();
    var resultado = validador.Validar("00000000000");
    Assert.False(resultado);
}

// 2. GREEN - Implementação real
public class ValidadorCPF {
    public bool Validar(string cpf) {
        if (string.IsNullOrEmpty(cpf) || cpf.Length != 11) {
            return false;
        }
        
        // Verifica se todos dígitos são iguais
        if (cpf.Distinct().Count() == 1) {
            return false;
        }
        
        // Implementação completa do algoritmo...
        return CalcularDigitosVerificadores(cpf);
    }
    
    private bool CalcularDigitosVerificadores(string cpf) {
        // Implementação do algoritmo de CPF
        return true;  // Simplificado
    }
}
```

## 🏗️ TDD na Prática - Sistema de Pedidos

### Feature: Calcular Total do Pedido

```csharp
// ITERAÇÃO 1: Pedido vazio
[Fact]
public void CalcularTotal_PedidoSemItens_RetornaZero() {
    var pedido = new Pedido();
    Assert.Equal(0, pedido.CalcularTotal());
}

public class Pedido {
    public decimal CalcularTotal() => 0;
}

// ITERAÇÃO 2: Um item
[Fact]
public void CalcularTotal_UmItem_RetornaPrecoDoItem() {
    var pedido = new Pedido();
    pedido.AdicionarItem(new ItemPedido { Preco = 10, Quantidade = 1 });
    Assert.Equal(10, pedido.CalcularTotal());
}

public class Pedido {
    private List<ItemPedido> _itens = new();
    
    public void AdicionarItem(ItemPedido item) {
        _itens.Add(item);
    }
    
    public decimal CalcularTotal() {
        return _itens.Sum(i => i.Preco * i.Quantidade);
    }
}

// ITERAÇÃO 3: Múltiplos itens
[Fact]
public void CalcularTotal_VariosItens_RetornaSomaTotal() {
    var pedido = new Pedido();
    pedido.AdicionarItem(new ItemPedido { Preco = 10, Quantidade = 2 });
    pedido.AdicionarItem(new ItemPedido { Preco = 5, Quantidade = 3 });
    
    Assert.Equal(35, pedido.CalcularTotal());  // (10*2) + (5*3)
}

// Código já funciona! Não precisa mudar

// ITERAÇÃO 4: Desconto
[Fact]
public void CalcularTotal_ComDesconto_AplicaDesconto() {
    var pedido = new Pedido();
    pedido.AdicionarItem(new ItemPedido { Preco = 100, Quantidade = 1 });
    pedido.AplicarDesconto(10);  // 10%
    
    Assert.Equal(90, pedido.CalcularTotal());
}

public class Pedido {
    private List<ItemPedido> _itens = new();
    private decimal _descontoPercentual = 0;
    
    public void AdicionarItem(ItemPedido item) => _itens.Add(item);
    
    public void AplicarDesconto(decimal percentual) {
        _descontoPercentual = percentual;
    }
    
    public decimal CalcularTotal() {
        var subtotal = _itens.Sum(i => i.Preco * i.Quantidade);
        var desconto = subtotal * (_descontoPercentual / 100);
        return subtotal - desconto;
    }
}
```

## 🎯 Benefícios do TDD

### 1. Design Emergente
```csharp
// TDD força você a pensar na interface primeiro
[Fact]
public void ProcessarPagamento_CartaoValido_AprovaPagamento() {
    // Você define como quer usar antes de implementar
    var processador = new ProcessadorPagamento();
    var cartao = new Cartao("1234-5678-9012-3456");
    
    var resultado = processador.Processar(cartao, 100m);
    
    Assert.True(resultado.Aprovado);
}
```

### 2. Documentação Viva
```csharp
// Testes documentam como usar o código
[Fact]
public void ValidarEmail_EmailComArroba_RetornaTrue() { }

[Fact]
public void ValidarEmail_EmailSemArroba_RetornaFalse() { }

[Fact]
public void ValidarEmail_EmailNulo_LancaExcecao() { }
```

### 3. Refatoração Segura
```csharp
// Pode refatorar com confiança
public decimal CalcularTotal() {
    // Versão 1
    return _itens.Sum(i => i.Preco * i.Quantidade);
    
    // Versão 2 (refatorada)
    return _itens.Aggregate(0m, (total, item) => total + item.CalcularSubtotal());
}
// Testes garantem que funciona igual
```

## 🚫 Erros Comuns em TDD

```csharp
// ❌ RUIM - Teste depois do código
public class Produto {
    public decimal CalcularPrecoComDesconto(decimal desconto) {
        return Preco * (1 - desconto);
    }
}

// Teste escrito depois
[Fact]
public void TesteCalcularDesconto() { }

// ✅ BOM - Teste antes do código
[Fact]
public void CalcularPrecoComDesconto_10Porcento_Retorna90() {
    // Teste primeiro!
}

// Depois implementa
```

## 🎯 Exercício Prático - String Calculator

```csharp
// Criar calculadora que soma números em string
// Requisitos:
// 1. "" retorna 0
// 2. "1" retorna 1
// 3. "1,2" retorna 3
// 4. "1,2,3" retorna 6
// 5. Aceita \n como delimitador
// 6. Números negativos lançam exceção

// Siga TDD: um teste de cada vez!

[Fact]
public void Add_StringVazia_RetornaZero() {
    var calc = new StringCalculator();
    Assert.Equal(0, calc.Add(""));
}

[Fact]
public void Add_UmNumero_RetornaNumero() {
    var calc = new StringCalculator();
    Assert.Equal(1, calc.Add("1"));
}

[Fact]
public void Add_DoisNumeros_RetornaSoma() {
    var calc = new StringCalculator();
    Assert.Equal(3, calc.Add("1,2"));
}

// Continue...
```

## ✅ Verificação

- [ ] Entendeu ciclo Red-Green-Refactor
- [ ] Escreveu teste ANTES do código
- [ ] Implementou mínimo para passar
- [ ] Refatorou mantendo testes verdes
- [ ] Praticou com exercício completo
- [ ] Viu benefícios do TDD

---

[← Voltar: Mocks e Stubs](./02-mocks-stubs.md) | [Próximo: Testes de Integração →](./04-testes-integracao.md)