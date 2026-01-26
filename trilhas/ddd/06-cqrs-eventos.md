# 06 - CQRS e Eventos

## 📕 Nível Avançado

### O que é CQRS?

**CQRS** (Command Query Responsibility Segregation) separa **operações de leitura** (Queries) das **operações de escrita** (Commands).

## 🎯 Problema: Modelo Único

### ❌ Modelo Tradicional

```csharp
// ❌ Mesmo modelo para leitura e escrita
public class Pedido {
    public PedidoId Id { get; set; }
    public ClienteId ClienteId { get; set; }
    public List<ItemPedido> Itens { get; set; }
    // ... muitas propriedades para leitura
    public ClienteNome ClienteNome { get; set; }  // Denormalizado para leitura
    public decimal Total { get; set; }  // Calculado para leitura
}

// Mesmo repositório para tudo
public interface IPedidoRepository {
    Task<Pedido> BuscarPorIdAsync(PedidoId id);
    Task SalvarAsync(Pedido pedido);
    Task<List<Pedido>> BuscarTodosAsync();
}
```

### ✅ CQRS: Separação

```csharp
// ✅ Commands (Escrita)
public class CriarPedidoCommand {
    public ClienteId ClienteId { get; set; }
    public List<ItemPedidoDto> Itens { get; set; }
}

public class CriarPedidoCommandHandler {
    public async Task<PedidoId> HandleAsync(CriarPedidoCommand command) {
        var pedido = new Pedido(command.ClienteId);
        // Lógica de escrita
        await _repository.SalvarAsync(pedido);
        return pedido.Id;
    }
}

// ✅ Queries (Leitura)
public class PedidoDto {
    public PedidoId Id { get; set; }
    public string ClienteNome { get; set; }  // Denormalizado
    public decimal Total { get; set; }  // Já calculado
    public List<ItemPedidoDto> Itens { get; set; }
}

public class BuscarPedidoQuery {
    public PedidoId Id { get; set; }
}

public class BuscarPedidoQueryHandler {
    public async Task<PedidoDto> HandleAsync(BuscarPedidoQuery query) {
        // Leitura otimizada, pode vir de view/materializada
        return await _queryRepository.BuscarPorIdAsync(query.Id);
    }
}
```

## 📊 Arquitetura CQRS

```
┌─────────────────────────────────┐
│         API Layer               │
└─────────────┬───────────────────┘
              │
      ┌───────┴───────┐
      │               │
┌─────▼─────┐   ┌─────▼─────┐
│ Commands  │   │  Queries  │
│ (Write)   │   │  (Read)   │
└─────┬─────┘   └─────┬─────┘
      │               │
┌─────▼─────┐   ┌─────▼─────┐
│  Domain   │   │   Views   │
│  Model    │   │  (Read)   │
└─────┬─────┘   └───────────┘
      │
┌─────▼─────┐
│  Database │
│  (Write)  │
└───────────┘
```

## 🎯 Domain Events

### O que são Domain Events?

**Domain Events** representam algo importante que aconteceu no domínio.

### Exemplo

```csharp
// Domain Event
public class PedidoFinalizadoEvent : IDomainEvent {
    public PedidoId PedidoId { get; }
    public ClienteId ClienteId { get; }
    public decimal Total { get; }
    public DateTime OcorreuEm { get; }
    
    public PedidoFinalizadoEvent(PedidoId pedidoId, ClienteId clienteId, decimal total) {
        PedidoId = pedidoId;
        ClienteId = clienteId;
        Total = total;
        OcorreuEm = DateTime.UtcNow;
    }
}

// Entidade publica evento
public class Pedido {
    private readonly List<IDomainEvent> _eventos;
    public IReadOnlyList<IDomainEvent> Eventos => _eventos.AsReadOnly();
    
    public void Finalizar() {
        // Lógica de finalização
        Status = StatusPedido.Finalizado;
        
        // Publica evento
        _eventos.Add(new PedidoFinalizadoEvent(Id, ClienteId, Total));
    }
    
    public void LimparEventos() {
        _eventos.Clear();
    }
}
```

### Event Handlers

```csharp
// Handler que escuta o evento
public class EnviarEmailConfirmacaoHandler : IEventHandler<PedidoFinalizadoEvent> {
    private readonly IEmailService _emailService;
    
    public async Task HandleAsync(PedidoFinalizadoEvent evento) {
        var cliente = await _clienteRepository.BuscarPorIdAsync(evento.ClienteId);
        
        await _emailService.EnviarAsync(
            cliente.Email,
            "Pedido Confirmado",
            $"Seu pedido {evento.PedidoId} foi finalizado com sucesso!"
        );
    }
}

// Handler para atualizar estoque
public class ReservarEstoqueHandler : IEventHandler<PedidoFinalizadoEvent> {
    private readonly IEstoqueService _estoqueService;
    
    public async Task HandleAsync(PedidoFinalizadoEvent evento) {
        var pedido = await _pedidoRepository.BuscarPorIdAsync(evento.PedidoId);
        
        foreach (var item in pedido.Itens) {
            await _estoqueService.ReservarAsync(item.ProdutoId, item.Quantidade);
        }
    }
}
```

## 📚 Exemplo Completo CQRS + Events

### Commands

```csharp
// Command
public class CriarPedidoCommand {
    public ClienteId ClienteId { get; set; }
    public List<ItemPedidoDto> Itens { get; set; }
}

// Command Handler
public class CriarPedidoCommandHandler {
    private readonly IPedidoRepository _repository;
    private readonly IEventDispatcher _eventDispatcher;
    
    public async Task<PedidoId> HandleAsync(CriarPedidoCommand command) {
        var pedido = new Pedido(command.ClienteId);
        
        foreach (var itemDto in command.Itens) {
            pedido.AdicionarItem(itemDto.ProdutoId, itemDto.Quantidade, itemDto.Preco);
        }
        
        await _repository.SalvarAsync(pedido);
        
        // Dispara eventos
        foreach (var evento in pedido.Eventos) {
            await _eventDispatcher.DispatchAsync(evento);
        }
        
        pedido.LimparEventos();
        await _repository.SalvarAsync(pedido);
        
        return pedido.Id;
    }
}
```

### Queries

```csharp
// Query
public class BuscarPedidosPorClienteQuery {
    public ClienteId ClienteId { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

// Query Handler
public class BuscarPedidosPorClienteQueryHandler {
    private readonly IPedidoQueryRepository _queryRepository;
    
    public async Task<PaginatedList<PedidoDto>> HandleAsync(BuscarPedidosPorClienteQuery query) {
        return await _queryRepository.BuscarPorClienteAsync(
            query.ClienteId,
            query.PageNumber,
            query.PageSize
        );
    }
}
```

### Event Sourcing (Opcional)

```csharp
// Event Store
public interface IEventStore {
    Task SalvarEventosAsync(Guid aggregateId, IEnumerable<IDomainEvent> eventos);
    Task<List<IDomainEvent>> BuscarEventosAsync(Guid aggregateId);
}

// Reconstruir aggregate a partir de eventos
public class Pedido {
    public static Pedido Reconstruir(List<IDomainEvent> eventos) {
        var pedido = new Pedido();
        
        foreach (var evento in eventos) {
            pedido.Aplicar(evento);
        }
        
        return pedido;
    }
    
    private void Aplicar(IDomainEvent evento) {
        switch (evento) {
            case PedidoCriadoEvent e:
                Id = e.PedidoId;
                ClienteId = e.ClienteId;
                Status = StatusPedido.Rascunho;
                break;
                
            case ItemAdicionadoEvent e:
                var item = new ItemPedido(e.ProdutoId, e.Quantidade, e.Preco);
                _itens.Add(item);
                break;
                
            case PedidoFinalizadoEvent e:
                Status = StatusPedido.Finalizado;
                break;
        }
    }
}
```

## ✅ Verificação

- [ ] Entendeu o que é CQRS
- [ ] Sabe quando usar CQRS
- [ ] Conhece Domain Events
- [ ] Implementou Command/Query handlers
- [ ] Implementou Event Handlers
- [ ] Viu exemplo completo

---

[← Voltar: Bounded Contexts](./05-bounded-contexts.md) | [Próximo: Aplicação Prática →](./07-aplicacao-pratica.md)