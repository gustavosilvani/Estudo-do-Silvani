# 04 - Domain Services

## 📗 Nível Intermediário

### O que são Domain Services?

**Domain Services** contêm lógica de domínio que **não pertence naturalmente** a uma entidade ou value object específico.

## 🎯 Quando Usar Domain Services?

### ❌ Lógica que NÃO pertence a uma entidade

```csharp
// ❌ Problema: Transferência entre duas contas
public class Conta {
    public void Transferir(Conta destino, decimal valor) {
        // Problema: Conta precisa conhecer outra Conta
        // Problema: Lógica de transferência não é responsabilidade de Conta
    }
}
```

### ✅ Solução: Domain Service

```csharp
// ✅ Domain Service para lógica que envolve múltiplas entidades
public class TransferenciaService {
    public void Transferir(Conta origem, Conta destino, decimal valor) {
        // Lógica de transferência entre duas contas
        if (origem.Saldo < valor) {
            throw new SaldoInsuficienteException();
        }
        
        origem.Debitar(valor);
        destino.Creditar(valor);
    }
}
```

## 📚 Tipos de Domain Services

### 1. **Services com Lógica de Domínio**

```csharp
public class CalculadoraDescontoService {
    public decimal CalcularDesconto(Cliente cliente, Pedido pedido) {
        // Lógica complexa que não pertence a Cliente nem Pedido
        
        decimal desconto = 0;
        
        // Desconto por fidelidade
        if (cliente.EhClienteVip()) {
            desconto += pedido.Total * 0.10m;  // 10%
        }
        
        // Desconto por volume
        if (pedido.Total > 1000) {
            desconto += pedido.Total * 0.05m;  // 5% adicional
        }
        
        // Desconto máximo
        return Math.Min(desconto, pedido.Total * 0.20m);  // Máximo 20%
    }
}
```

### 2. **Services para Validações Complexas**

```csharp
public class ValidadorPedidoService {
    public Result ValidarPedido(Pedido pedido, Estoque estoque) {
        // Validação que envolve múltiplas entidades
        
        if (!pedido.Itens.Any()) {
            return Result.Fail("Pedido não pode estar vazio");
        }
        
        foreach (var item in pedido.Itens) {
            var produtoEmEstoque = estoque.BuscarProduto(item.ProdutoId);
            
            if (produtoEmEstoque == null) {
                return Result.Fail($"Produto {item.ProdutoId} não encontrado");
            }
            
            if (produtoEmEstoque.QuantidadeDisponivel < item.Quantidade) {
                return Result.Fail($"Estoque insuficiente para produto {item.ProdutoId}");
            }
        }
        
        return Result.Ok();
    }
}
```

### 3. **Services para Transformações**

```csharp
public class ConversorMoedaService {
    public Dinheiro Converter(Dinheiro valor, string moedaDestino) {
        // Lógica de conversão que não pertence a Dinheiro
        
        var taxa = ObterTaxaCambio(valor.Moeda, moedaDestino);
        var valorConvertido = valor.Valor * taxa;
        
        return new Dinheiro(valorConvertido, moedaDestino);
    }
    
    private decimal ObterTaxaCambio(string origem, string destino) {
        // Lógica de busca de taxa
        return 5.50m;  // Exemplo
    }
}
```

## 🎯 Domain Service vs Application Service

### Domain Service
- **Lógica de negócio pura**
- **Não depende de infraestrutura**
- **Pode ser testado isoladamente**

```csharp
// ✅ Domain Service
public class CalculadoraFreteService {
    public decimal CalcularFrete(Endereco origem, Endereco destino, decimal peso) {
        // Lógica de cálculo pura
        var distancia = CalcularDistancia(origem, destino);
        return distancia * peso * 0.10m;
    }
    
    private decimal CalcularDistancia(Endereco origem, Endereco destino) {
        // Cálculo matemático
        return 100;  // Exemplo
    }
}
```

### Application Service
- **Orquestra casos de uso**
- **Pode depender de infraestrutura**
- **Coordena Domain Services e Repositories**

```csharp
// ✅ Application Service
public class PedidoService {
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IEstoqueRepository _estoqueRepository;
    private readonly CalculadoraFreteService _calculadoraFrete;
    
    public async Task<Pedido> CriarPedidoAsync(CriarPedidoDto dto) {
        // Orquestra: busca estoque, calcula frete, salva pedido
        var estoque = await _estoqueRepository.BuscarAsync();
        var frete = _calculadoraFrete.CalcularFrete(dto.Origem, dto.Destino, dto.Peso);
        
        var pedido = new Pedido(dto.ClienteId);
        // ...
        
        await _pedidoRepository.SalvarAsync(pedido);
        return pedido;
    }
}
```

## 📊 Exemplo Completo

### Sistema de E-commerce

```csharp
// Domain Service: Cálculo de frete
public class CalculadoraFreteService {
    public decimal CalcularFrete(
        Endereco origem,
        Endereco destino,
        List<ItemPedido> itens) {
        
        var pesoTotal = itens.Sum(i => i.Peso);
        var distancia = CalcularDistancia(origem, destino);
        var tipoFrete = DeterminarTipoFrete(distancia, pesoTotal);
        
        return CalcularPorTipo(tipoFrete, distancia, pesoTotal);
    }
    
    private TipoFrete DeterminarTipoFrete(decimal distancia, decimal peso) {
        if (distancia < 50 && peso < 10) {
            return TipoFrete.Expresso;
        }
        if (distancia < 200) {
            return TipoFrete.Padrao;
        }
        return TipoFrete.Economico;
    }
    
    private decimal CalcularPorTipo(TipoFrete tipo, decimal distancia, decimal peso) {
        return tipo switch {
            TipoFrete.Expresso => distancia * 2.0m + peso * 5.0m,
            TipoFrete.Padrao => distancia * 1.0m + peso * 3.0m,
            TipoFrete.Economico => distancia * 0.5m + peso * 2.0m,
            _ => throw new TipoFreteInvalidoException()
        };
    }
    
    private decimal CalcularDistancia(Endereco origem, Endereco destino) {
        // Lógica de cálculo de distância
        return 150;  // Exemplo
    }
}

// Domain Service: Validação de estoque
public class ValidadorEstoqueService {
    public Result ValidarDisponibilidade(
        List<ItemPedido> itens,
        Estoque estoque) {
        
        foreach (var item in itens) {
            var produto = estoque.BuscarProduto(item.ProdutoId);
            
            if (produto == null) {
                return Result.Fail($"Produto {item.ProdutoId} não encontrado");
            }
            
            if (produto.QuantidadeDisponivel < item.Quantidade) {
                return Result.Fail(
                    $"Estoque insuficiente. Disponível: {produto.QuantidadeDisponivel}, " +
                    $"Solicitado: {item.Quantidade}");
            }
        }
        
        return Result.Ok();
    }
}
```

## ✅ Verificação

- [ ] Entendeu o que são Domain Services
- [ ] Sabe quando usar Domain Services
- [ ] Diferenciou Domain Service de Application Service
- [ ] Implementou Domain Services
- [ ] Viu exemplos práticos
- [ ] Entendeu quando NÃO usar (preferir métodos em entidades)

---

[← Voltar: Aggregates e Repositories](./03-aggregates-repositories.md) | [Próximo: Bounded Contexts →](./05-bounded-contexts.md)