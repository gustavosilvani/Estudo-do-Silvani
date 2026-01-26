# 03 - Entity Framework Core

## 📗 Nível Intermediário

### O que é Entity Framework Core?

**Entity Framework Core** (EF Core) é um ORM (Object-Relational Mapping) que permite trabalhar com banco de dados usando objetos C# ao invés de SQL direto.

## 🎯 Vantagens do EF Core

### ✅ Benefícios

- **Código C#**: Não precisa escrever SQL manualmente
- **Type-safe**: Compilador verifica erros
- **LINQ**: Consultas usando C#
- **Migrations**: Controle de versão do banco
- **Multi-database**: Funciona com vários bancos

## 🔧 Configuração Básica

### Instalação

```bash
# Pacote NuGet
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
```

### DbContext

```csharp
public class AppDbContext : DbContext {
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Pedido> Pedidos { get; set; }
    public DbSet<Produto> Produtos { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder options) {
        options.UseSqlServer(
            "Server=localhost;Database=EcommerceDb;Trusted_Connection=True;TrustServerCertificate=True;"
        );
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);
        
        // Configurações de modelo
        modelBuilder.Entity<Cliente>(entity => {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Nome).HasMaxLength(100).IsRequired();
            entity.Property(c => c.Email).HasMaxLength(255).IsRequired();
            entity.HasIndex(c => c.Email).IsUnique();
        });
    }
}
```

## 📊 Operações CRUD

### CREATE

```csharp
using var context = new AppDbContext();

// Adicionar um
var cliente = new Cliente {
    Nome = "João Silva",
    Email = "joao@email.com"
};
context.Clientes.Add(cliente);
await context.SaveChangesAsync();

// Adicionar múltiplos
var clientes = new List<Cliente> {
    new Cliente { Nome = "Maria", Email = "maria@email.com" },
    new Cliente { Nome = "Pedro", Email = "pedro@email.com" }
};
context.Clientes.AddRange(clientes);
await context.SaveChangesAsync();
```

### READ

```csharp
// Buscar todos
var clientes = await context.Clientes.ToListAsync();

// Buscar por ID
var cliente = await context.Clientes.FindAsync(1);

// Buscar com condição
var clientes = await context.Clientes
    .Where(c => c.Nome.Contains("Silva"))
    .ToListAsync();

// Primeiro ou padrão
var cliente = await context.Clientes
    .FirstOrDefaultAsync(c => c.Email == "joao@email.com");
```

### UPDATE

```csharp
// Atualizar
var cliente = await context.Clientes.FindAsync(1);
cliente.Nome = "João Silva Santos";
await context.SaveChangesAsync();

// Atualizar múltiplos
var clientes = await context.Clientes
    .Where(c => c.Nome.Contains("João"))
    .ToListAsync();

foreach (var c in clientes) {
    c.Nome = c.Nome + " Atualizado";
}
await context.SaveChangesAsync();
```

### DELETE

```csharp
// Deletar
var cliente = await context.Clientes.FindAsync(1);
context.Clientes.Remove(cliente);
await context.SaveChangesAsync();

// Deletar múltiplos
var clientes = await context.Clientes
    .Where(c => c.Nome.Contains("Teste"))
    .ToListAsync();

context.Clientes.RemoveRange(clientes);
await context.SaveChangesAsync();
```

## 🔍 Consultas com LINQ

### Filtros

```csharp
// WHERE
var produtos = await context.Produtos
    .Where(p => p.Preco > 100 && p.Ativo)
    .ToListAsync();

// Múltiplas condições
var pedidos = await context.Pedidos
    .Where(p => p.Total > 500 && p.DataPedido > DateTime.UtcNow.AddDays(-30))
    .ToListAsync();
```

### Ordenação

```csharp
// ORDER BY
var clientes = await context.Clientes
    .OrderBy(c => c.Nome)
    .ToListAsync();

// ORDER BY DESC
var produtos = await context.Produtos
    .OrderByDescending(p => p.Preco)
    .ToListAsync();

// Múltiplas ordenações
var pedidos = await context.Pedidos
    .OrderByDescending(p => p.DataPedido)
    .ThenBy(p => p.Total)
    .ToListAsync();
```

### Agregações

```csharp
// COUNT
var total = await context.Clientes.CountAsync();

// SUM
var totalPedidos = await context.Pedidos
    .SumAsync(p => p.Total);

// AVG
var precoMedio = await context.Produtos
    .AverageAsync(p => p.Preco);

// MIN/MAX
var precoMinimo = await context.Produtos
    .MinAsync(p => p.Preco);

var precoMaximo = await context.Produtos
    .MaxAsync(p => p.Preco);
```

### Agrupamento

```csharp
// GROUP BY
var pedidosPorCliente = await context.Pedidos
    .GroupBy(p => p.ClienteId)
    .Select(g => new {
        ClienteId = g.Key,
        TotalPedidos = g.Count(),
        ValorTotal = g.Sum(p => p.Total)
    })
    .ToListAsync();
```

## 🔗 Relacionamentos

### Include (Eager Loading)

```csharp
// Carregar relacionamento
var pedido = await context.Pedidos
    .Include(p => p.Cliente)
    .Include(p => p.Itens)
    .ThenInclude(i => i.Produto)
    .FirstOrDefaultAsync(p => p.Id == 1);
```

### Select (Projection)

```csharp
// Selecionar apenas campos necessários
var pedidos = await context.Pedidos
    .Select(p => new {
        p.Id,
        ClienteNome = p.Cliente.Nome,
        p.Total,
        p.DataPedido
    })
    .ToListAsync();
```

## 📝 Configuração Avançada

### Fluent API

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder) {
    // Configurar entidade
    modelBuilder.Entity<Pedido>(entity => {
        // Primary Key
        entity.HasKey(p => p.Id);
        
        // Propriedades
        entity.Property(p => p.Total)
            .HasColumnType("decimal(18,2)")
            .IsRequired();
        
        // Relacionamentos
        entity.HasOne(p => p.Cliente)
            .WithMany(c => c.Pedidos)
            .HasForeignKey(p => p.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);
        
        // Índices
        entity.HasIndex(p => p.DataPedido);
        
        // Tabela
        entity.ToTable("Pedidos");
    });
}
```

### Data Annotations

```csharp
[Table("Clientes")]
public class Cliente {
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Nome { get; set; }
    
    [Required]
    [MaxLength(255)]
    [EmailAddress]
    public string Email { get; set; }
}
```

## 🎯 Exemplo Completo

### Modelos

```csharp
public class Cliente {
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public List<Pedido> Pedidos { get; set; } = new();
}

public class Pedido {
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public Cliente Cliente { get; set; }
    public DateTime DataPedido { get; set; }
    public decimal Total { get; set; }
    public List<ItemPedido> Itens { get; set; } = new();
}

public class ItemPedido {
    public int Id { get; set; }
    public int PedidoId { get; set; }
    public Pedido Pedido { get; set; }
    public int ProdutoId { get; set; }
    public Produto Produto { get; set; }
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
}

public class Produto {
    public int Id { get; set; }
    public string Nome { get; set; }
    public decimal Preco { get; set; }
    public int Estoque { get; set; }
}
```

### Consultas Complexas

```csharp
// Relatório: Clientes com resumo
var relatorio = await context.Clientes
    .Select(c => new {
        c.Id,
        c.Nome,
        TotalPedidos = c.Pedidos.Count,
        ValorTotal = c.Pedidos.Sum(p => p.Total),
        UltimoPedido = c.Pedidos
            .OrderByDescending(p => p.DataPedido)
            .FirstOrDefault()
    })
    .ToListAsync();
```

## ✅ Verificação

- [ ] Configurou DbContext
- [ ] Implementou CRUD completo
- [ ] Usou LINQ para consultas
- [ ] Configurou relacionamentos
- [ ] Usou Include para eager loading
- [ ] Configurou modelo com Fluent API
- [ ] Viu exemplos práticos

---

[← Voltar: Relacionamentos](./02-relacionamentos.md) | [Próximo: Migrations →](./04-migrations.md)