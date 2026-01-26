# 07 - Aplicação Prática

## 📕 Nível Avançado

### Sistema Completo com DDD

Vamos construir um sistema de **E-commerce** completo aplicando todos os conceitos de DDD aprendidos.

## 🏗️ Arquitetura Completa

```
┌─────────────────────────────────────┐
│         API Layer                  │
│  (Controllers, DTOs)               │
└──────────────┬─────────────────────┘
               │
┌──────────────▼─────────────────────┐
│      Application Layer              │
│  (Commands, Queries, Handlers)      │
└──────────────┬─────────────────────┘
               │
┌──────────────▼─────────────────────┐
│         Domain Layer                │
│  (Entities, Value Objects,          │
│   Aggregates, Domain Services)      │
└──────────────┬─────────────────────┘
               │
┌──────────────▼─────────────────────┐
│      Infrastructure Layer           │
│  (Repositories, EF Core, Events)    │
└─────────────────────────────────────┘
```

## 📦 Domain Layer

### Value Objects

```csharp
// Value Object: Email
public class Email {
    public string Valor { get; }
    
    public Email(string valor) {
        if (string.IsNullOrWhiteSpace(valor) || !valor.Contains("@")) {
            throw new EmailInvalidoException(valor);
        }
        Valor = valor.ToLowerInvariant();
    }
    
    public override bool Equals(object? obj) {
        return obj is Email outro && Valor == outro.Valor;
    }
    
    public override int GetHashCode() => Valor.GetHashCode();
}

// Value Object: CPF
public class CPF {
    public string Valor { get; }
    
    public CPF(string valor) {
        if (!Validar(valor)) {
            throw new CPFInvalidoException(valor);
        }
        Valor = Limpar(valor);
    }
    
    private static bool Validar(string cpf) {
        // Lógica de validação
        return cpf.Length == 11;
    }
    
    private static string Limpar(string cpf) {
        return cpf.Replace(".", "").Replace("-", "");
    }
}

// Value Object: Dinheiro
public class Dinheiro {
    public decimal Valor { get; }
    public string Moeda { get; }
    
    public Dinheiro(decimal valor, string moeda = "BRL") {
        if (valor < 0) {
            throw new ValorNegativoException();
        }
        Valor = valor;
        Moeda = moeda;
    }
    
    public Dinheiro Somar(Dinheiro outro) {
        if (Moeda != outro.Moeda) {
            throw new MoedasDiferentesException();
        }
        return new Dinheiro(Valor + outro.Valor, Moeda);
    }
}
```

### Entities

```csharp
// Entity: Cliente
public class Cliente {
    public ClienteId Id { get; }
    public CPF Cpf { get; }
    public Email Email { get; }
    public string Nome { get; private set; }
    
    public Cliente(ClienteId id, CPF cpf, Email email, string nome) {
        Id = id;
        Cpf = cpf;
        Email = email;
        Nome = nome;
    }
    
    public void AlterarNome(string novoNome) {
        if (string.IsNullOrWhiteSpace(novoNome)) {
            throw new NomeInvalidoException();
        }
        Nome = novoNome;
    }
}
```

### Aggregate: Pedido

```csharp
// Aggregate Root: Pedido
public class Pedido {
    public PedidoId Id { get; }
    public ClienteId ClienteId { get; }
    private readonly List<ItemPedido> _itens;
    public EnderecoEntrega Endereco { get; private set; }
    public StatusPedido Status { get; private set; }
    private readonly List<IDomainEvent> _eventos;
    
    public IReadOnlyList<ItemPedido> Itens => _itens.AsReadOnly();
    public IReadOnlyList<IDomainEvent> Eventos => _eventos.AsReadOnly();
    public Dinheiro Total => new Dinheiro(_itens.Sum(i => i.Subtotal.Valor));
    
    public Pedido(ClienteId clienteId) {
        Id = PedidoId.Novo();
        ClienteId = clienteId;
        _itens = new List<ItemPedido>();
        Status = StatusPedido.Rascunho;
        _eventos = new List<IDomainEvent>();
        
        _eventos.Add(new PedidoCriadoEvent(Id, ClienteId));
    }
    
    public void AdicionarItem(ProdutoId produtoId, int quantidade, Dinheiro precoUnitario) {
        ValidarPodeAlterar();
        
        if (quantidade <= 0) {
            throw new QuantidadeInvalidaException();
        }
        
        var itemExistente = _itens.FirstOrDefault(i => i.ProdutoId == produtoId);
        if (itemExistente != null) {
            itemExistente.AlterarQuantidade(itemExistente.Quantidade + quantidade);
        } else {
            var novoItem = new ItemPedido(produtoId, quantidade, precoUnitario);
            _itens.Add(novoItem);
        }
        
        _eventos.Add(new ItemAdicionadoEvent(Id, produtoId, quantidade));
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
        _eventos.Add(new PedidoFinalizadoEvent(Id, ClienteId, Total));
    }
    
    public void LimparEventos() {
        _eventos.Clear();
    }
    
    private void ValidarPodeAlterar() {
        if (Status != StatusPedido.Rascunho) {
            throw new PedidoJaFinalizadoException();
        }
    }
}

// Entidade dentro do aggregate
public class ItemPedido {
    public ItemPedidoId Id { get; }
    public ProdutoId ProdutoId { get; }
    public int Quantidade { get; private set; }
    public Dinheiro PrecoUnitario { get; }
    public Dinheiro Subtotal => new Dinheiro(Quantidade * PrecoUnitario.Valor);
    
    public ItemPedido(ProdutoId produtoId, int quantidade, Dinheiro precoUnitario) {
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

### Domain Events

```csharp
public interface IDomainEvent {
    DateTime OcorreuEm { get; }
}

public class PedidoCriadoEvent : IDomainEvent {
    public PedidoId PedidoId { get; }
    public ClienteId ClienteId { get; }
    public DateTime OcorreuEm { get; }
    
    public PedidoCriadoEvent(PedidoId pedidoId, ClienteId clienteId) {
        PedidoId = pedidoId;
        ClienteId = clienteId;
        OcorreuEm = DateTime.UtcNow;
    }
}

public class PedidoFinalizadoEvent : IDomainEvent {
    public PedidoId PedidoId { get; }
    public ClienteId ClienteId { get; }
    public Dinheiro Total { get; }
    public DateTime OcorreuEm { get; }
    
    public PedidoFinalizadoEvent(PedidoId pedidoId, ClienteId clienteId, Dinheiro total) {
        PedidoId = pedidoId;
        ClienteId = clienteId;
        Total = total;
        OcorreuEm = DateTime.UtcNow;
    }
}
```

### Domain Service

```csharp
public class CalculadoraFreteService {
    public Dinheiro CalcularFrete(EnderecoEntrega origem, EnderecoEntrega destino, decimal peso) {
        var distancia = CalcularDistancia(origem, destino);
        var tipoFrete = DeterminarTipoFrete(distancia, peso);
        
        return tipoFrete switch {
            TipoFrete.Expresso => new Dinheiro(distancia * 2.0m + peso * 5.0m),
            TipoFrete.Padrao => new Dinheiro(distancia * 1.0m + peso * 3.0m),
            TipoFrete.Economico => new Dinheiro(distancia * 0.5m + peso * 2.0m),
            _ => throw new TipoFreteInvalidoException()
        };
    }
    
    private decimal CalcularDistancia(EnderecoEntrega origem, EnderecoEntrega destino) {
        // Lógica de cálculo
        return 150;  // Exemplo
    }
    
    private TipoFrete DeterminarTipoFrete(decimal distancia, decimal peso) {
        if (distancia < 50 && peso < 10) return TipoFrete.Expresso;
        if (distancia < 200) return TipoFrete.Padrao;
        return TipoFrete.Economico;
    }
}
```

### Repository Interface

```csharp
public interface IPedidoRepository {
    Task<Pedido?> BuscarPorIdAsync(PedidoId id);
    Task<List<Pedido>> BuscarPorClienteAsync(ClienteId clienteId);
    Task SalvarAsync(Pedido pedido);
}
```

## 📦 Application Layer

### Command

```csharp
public class CriarPedidoCommand {
    public ClienteId ClienteId { get; set; }
    public List<ItemPedidoDto> Itens { get; set; } = new();
}

public class CriarPedidoCommandHandler {
    private readonly IPedidoRepository _repository;
    private readonly IEventDispatcher _eventDispatcher;
    
    public async Task<PedidoId> HandleAsync(CriarPedidoCommand command) {
        var pedido = new Pedido(command.ClienteId);
        
        foreach (var itemDto in command.Itens) {
            var preco = new Dinheiro(itemDto.PrecoUnitario);
            pedido.AdicionarItem(itemDto.ProdutoId, itemDto.Quantidade, preco);
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

### Query

```csharp
public class BuscarPedidoQuery {
    public PedidoId Id { get; set; }
}

public class BuscarPedidoQueryHandler {
    private readonly IPedidoQueryRepository _queryRepository;
    
    public async Task<PedidoDto> HandleAsync(BuscarPedidoQuery query) {
        return await _queryRepository.BuscarPorIdAsync(query.Id);
    }
}
```

## 📦 Infrastructure Layer

### Repository Implementation

```csharp
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
}
```

### Event Handler

```csharp
public class EnviarEmailConfirmacaoHandler : IEventHandler<PedidoFinalizadoEvent> {
    private readonly IEmailService _emailService;
    private readonly IClienteRepository _clienteRepository;
    
    public async Task HandleAsync(PedidoFinalizadoEvent evento) {
        var cliente = await _clienteRepository.BuscarPorIdAsync(evento.ClienteId);
        
        await _emailService.EnviarAsync(
            cliente.Email,
            "Pedido Confirmado",
            $"Seu pedido {evento.PedidoId} foi finalizado com sucesso! Total: {evento.Total.Valor:C}"
        );
    }
}
```

## ✅ Verificação

- [ ] Implementou Value Objects
- [ ] Implementou Entities
- [ ] Implementou Aggregate Root
- [ ] Implementou Domain Events
- [ ] Implementou Domain Service
- [ ] Implementou Repository Pattern
- [ ] Implementou Commands e Queries
- [ ] Implementou Event Handlers
- [ ] Seguiu separação de camadas
- [ ] Aplicou Linguagem Ubíqua

---

**🎉 Parabéns! Você completou a trilha de DDD!**

[← Voltar: CQRS e Eventos](./06-cqrs-eventos.md) | [Voltar à Trilha](./README.md)