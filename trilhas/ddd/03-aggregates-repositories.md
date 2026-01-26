# 03 - Aggregates e Repositories

## 📗 Nível Intermediário

### O que são Aggregates?

**Aggregate** é um cluster de objetos de domínio (entidades e value objects) tratados como uma **unidade única** para propósito de mudanças de dados.

## 🎯 Aggregate Root

O **Aggregate Root** é a entidade principal que controla acesso a todo o aggregate.

```
Pedido (Aggregate Root)
├── ItemPedido (Entidade - parte do aggregate)
├── EnderecoEntrega (Value Object)
└── StatusPedido (Value Object)
```

### Regras dos Aggregates

1. **Acesso apenas pelo Root**: Outros objetos não podem ser acessados diretamente
2. **Consistência transacional**: Todo aggregate é salvo ou nada é salvo
3. **Identidade única**: Apenas o Root tem ID público
4. **Referências externas**: Apenas IDs, não objetos completos

## 📚 Exemplo Prático

### Aggregate: Pedido

```csharp
// ✅ Aggregate Root
public class Pedido {
    public PedidoId Id { get; }
    public ClienteId ClienteId { get; }  // Referência externa (apenas ID)
    private readonly List<ItemPedido> _itens;  // Parte do aggregate
    public EnderecoEntrega Endereco { get; private set; }
    public StatusPedido Status { get; private set; }
    
    // Acesso controlado
    public IReadOnlyList<ItemPedido> Itens => _itens.AsReadOnly();
    
    public Pedido(ClienteId clienteId) {
        Id = PedidoId.Novo();
        ClienteId = clienteId;
        _itens = new List<ItemPedido>();
        Status = StatusPedido.Rascunho;
    }
    
    // Operações do aggregate
    public void AdicionarItem(ProdutoId produtoId, int quantidade, decimal preco) {
        if (Status != StatusPedido.Rascunho) {
            throw new PedidoJaFinalizadoException();
        }
        
        var item = new ItemPedido(produtoId, quantidade, preco);
        _itens.Add(item);
    }
    
    public void RemoverItem(ItemPedidoId itemId) {
        if (Status != StatusPedido.Rascunho) {
            throw new PedidoJaFinalizadoException();
        }
        
        var item = _itens.FirstOrDefault(i => i.Id == itemId);
        if (item != null) {
            _itens.Remove(item);
        }
    }
    
    public void Finalizar() {
        if (!_itens.Any()) {
            throw new PedidoVazioException();
        }
        Status = StatusPedido.Finalizado;
    }
}

// Entidade dentro do aggregate
public class ItemPedido {
    public ItemPedidoId Id { get; }
    public ProdutoId ProdutoId { get; }
    public int Quantidade { get; private set; }
    public decimal PrecoUnitario { get; }
    public decimal Subtotal => Quantidade * PrecoUnitario;
    
    public ItemPedido(ProdutoId produtoId, int quantidade, decimal precoUnitario) {
        Id = ItemPedidoId.Novo();
        ProdutoId = produtoId;
        Quantidade = quantidade;
        PrecoUnitario = precoUnitario;
    }
    
    public void AlterarQuantidade(int novaQuantidade) {
        if (novaQuantidade <= 0) {
            throw new QuantidadeInvalidaException();
        }
        Quantidade = novaQuantidade;
    }
}
```

## 🗄️ Repository Pattern

### O que é Repository?

**Repository** abstrai a lógica de persistência, escondendo detalhes de banco de dados do domínio.

### Interface no Domínio

```csharp
// ✅ Interface no Domain (não depende de infraestrutura)
public interface IPedidoRepository {
    Task<Pedido?> BuscarPorIdAsync(PedidoId id);
    Task<List<Pedido>> BuscarPorClienteAsync(ClienteId clienteId);
    Task SalvarAsync(Pedido pedido);
    Task RemoverAsync(Pedido pedido);
}
```

### Implementação na Infrastructure

```csharp
// ✅ Implementação na Infrastructure
public class PedidoRepository : IPedidoRepository {
    private readonly AppDbContext _context;
    
    public PedidoRepository(AppDbContext context) {
        _context = context;
    }
    
    public async Task<Pedido?> BuscarPorIdAsync(PedidoId id) {
        return await _context.Pedidos
            .Include(p => p.Itens)
            .FirstOrDefaultAsync(p => p.Id == id);
    }
    
    public async Task SalvarAsync(Pedido pedido) {
        var existe = await _context.Pedidos.AnyAsync(p => p.Id == pedido.Id);
        
        if (existe) {
            _context.Pedidos.Update(pedido);
        } else {
            await _context.Pedidos.AddAsync(pedido);
        }
        
        await _context.SaveChangesAsync();
    }
    
    // ...
}
```

## 🎯 Regras de Aggregates

### 1. Acesso Apenas pelo Root

```csharp
// ❌ RUIM - Acesso direto
var item = pedido.Itens[0];  // Pode quebrar invariantes
item.Quantidade = -1;  // Estado inválido!

// ✅ BOM - Acesso controlado
pedido.AlterarQuantidadeItem(itemId, novaQuantidade);  // Validação no root
```

### 2. Consistência Transacional

```csharp
// ✅ Todo aggregate é salvo junto
public async Task CriarPedidoAsync(CriarPedidoDto dto) {
    var pedido = new Pedido(dto.ClienteId);
    pedido.AdicionarItem(dto.ProdutoId, dto.Quantidade, dto.Preco);
    
    // Salva pedido + itens em uma transação
    await _pedidoRepository.SalvarAsync(pedido);
}
```

### 3. Referências Externas

```csharp
// ❌ RUIM - Referência a objeto completo
public class Pedido {
    public Cliente Cliente { get; }  // Carrega objeto completo
}

// ✅ BOM - Referência apenas por ID
public class Pedido {
    public ClienteId ClienteId { get; }  // Apenas ID
}

// Se precisar do Cliente, busca separadamente
var cliente = await _clienteRepository.BuscarPorIdAsync(pedido.ClienteId);
```

## 📊 Exemplo Completo

### Aggregate Completo

```csharp
public class Pedido {
    public PedidoId Id { get; }
    public ClienteId ClienteId { get; }
    private readonly List<ItemPedido> _itens;
    public EnderecoEntrega Endereco { get; private set; }
    public StatusPedido Status { get; private set; }
    public DateTime DataCriacao { get; }
    public DateTime? DataFinalizacao { get; private set; }
    
    public IReadOnlyList<ItemPedido> Itens => _itens.AsReadOnly();
    public decimal Total => _itens.Sum(i => i.Subtotal);
    
    public Pedido(ClienteId clienteId) {
        Id = PedidoId.Novo();
        ClienteId = clienteId;
        _itens = new List<ItemPedido>();
        Status = StatusPedido.Rascunho;
        DataCriacao = DateTime.UtcNow;
    }
    
    public void AdicionarItem(ProdutoId produtoId, int quantidade, decimal preco) {
        ValidarPodeAlterar();
        
        var itemExistente = _itens.FirstOrDefault(i => i.ProdutoId == produtoId);
        if (itemExistente != null) {
            itemExistente.AlterarQuantidade(itemExistente.Quantidade + quantidade);
        } else {
            var novoItem = new ItemPedido(produtoId, quantidade, preco);
            _itens.Add(novoItem);
        }
    }
    
    public void RemoverItem(ItemPedidoId itemId) {
        ValidarPodeAlterar();
        
        var item = _itens.FirstOrDefault(i => i.Id == itemId);
        if (item != null) {
            _itens.Remove(item);
        }
    }
    
    public void DefinirEnderecoEntrega(EnderecoEntrega endereco) {
        ValidarPodeAlterar();
        Endereco = endereco;
    }
    
    public void Finalizar() {
        if (Status != StatusPedido.Rascunho) {
            throw new PedidoJaFinalizadoException();
        }
        
        if (!_itens.Any()) {
            throw new PedidoVazioException();
        }
        
        if (Endereco == null) {
            throw new EnderecoNaoDefinidoException();
        }
        
        Status = StatusPedido.Finalizado;
        DataFinalizacao = DateTime.UtcNow;
    }
    
    private void ValidarPodeAlterar() {
        if (Status != StatusPedido.Rascunho) {
            throw new PedidoJaFinalizadoException();
        }
    }
}
```

## ✅ Verificação

- [ ] Entendeu o que são Aggregates
- [ ] Conhece o conceito de Aggregate Root
- [ ] Sabe as regras dos Aggregates
- [ ] Implementou Repository Pattern
- [ ] Entendeu acesso controlado
- [ ] Viu exemplo completo

---

[← Voltar: Entidades e Value Objects](./02-entidades-value-objects.md) | [Próximo: Domain Services →](./04-domain-services.md)