# 05 - Otimização e Performance

## 📕 Nível Avançado

### Por que Otimizar?

Queries lentas podem:
- **Degradar experiência do usuário**
- **Aumentar custos de infraestrutura**
- **Causar timeouts**
- **Sobrecarregar banco de dados**

## 🔍 Índices

### O que são Índices?

**Índices** são estruturas que aceleram buscas no banco de dados, similar a um índice de livro.

### Criar Índices

```sql
-- Índice simples
CREATE INDEX IX_Clientes_Email ON Clientes(Email);

-- Índice único
CREATE UNIQUE INDEX IX_Clientes_CPF ON Clientes(CPF);

-- Índice composto
CREATE INDEX IX_Pedidos_ClienteData ON Pedidos(ClienteId, DataPedido);
```

### No EF Core

```csharp
// Fluent API
modelBuilder.Entity<Cliente>()
    .HasIndex(c => c.Email)
    .IsUnique();

modelBuilder.Entity<Pedido>()
    .HasIndex(p => new { p.ClienteId, p.DataPedido });
```

### Quando Criar Índices?

✅ **Crie índices para:**
- Colunas usadas em WHERE
- Colunas usadas em JOIN
- Colunas usadas em ORDER BY
- Foreign Keys

❌ **Evite índices em:**
- Colunas raramente consultadas
- Colunas com poucos valores distintos
- Tabelas pequenas (< 1000 registros)

## 🚀 Otimização de Queries

### 1. Selecionar Apenas Campos Necessários

```csharp
// ❌ RUIM - Carrega tudo
var clientes = await context.Clientes.ToListAsync();

// ✅ BOM - Apenas campos necessários
var clientes = await context.Clientes
    .Select(c => new {
        c.Id,
        c.Nome,
        c.Email
    })
    .ToListAsync();
```

### 2. Usar Paginação

```csharp
// ❌ RUIM - Carrega tudo
var pedidos = await context.Pedidos.ToListAsync();

// ✅ BOM - Paginação
var pedidos = await context.Pedidos
    .OrderByDescending(p => p.DataPedido)
    .Skip((pageNumber - 1) * pageSize)
    .Take(pageSize)
    .ToListAsync();
```

### 3. Evitar N+1 Queries

```csharp
// ❌ RUIM - N+1 queries
var pedidos = await context.Pedidos.ToListAsync();
foreach (var pedido in pedidos) {
    var cliente = await context.Clientes.FindAsync(pedido.ClienteId);  // Query por pedido!
}

// ✅ BOM - Uma query com JOIN
var pedidos = await context.Pedidos
    .Include(p => p.Cliente)
    .ToListAsync();
```

### 4. Usar AsNoTracking

```csharp
// ✅ Para leitura apenas (mais rápido)
var clientes = await context.Clientes
    .AsNoTracking()  // Não rastreia mudanças
    .ToListAsync();
```

## 📊 Análise de Performance

### Execution Plan

```sql
-- Ver plano de execução
SET STATISTICS IO ON;
SET STATISTICS TIME ON;

SELECT * FROM Clientes WHERE Email = 'joao@email.com';

-- Analisar:
-- - Índices usados
-- - Scans vs Seeks
-- - Custo estimado
```

### Query Profiler

```csharp
// Habilitar logging no EF Core
optionsBuilder.UseSqlServer(connectionString)
    .LogTo(Console.WriteLine, LogLevel.Information)
    .EnableSensitiveDataLogging();
```

## 🎯 Otimizações Avançadas

### Stored Procedures

```sql
-- Criar stored procedure
CREATE PROCEDURE sp_BuscarPedidosPorCliente
    @ClienteId INT
AS
BEGIN
    SELECT 
        p.Id,
        p.DataPedido,
        p.Total,
        COUNT(i.Id) AS TotalItens
    FROM Pedidos p
    LEFT JOIN ItensPedido i ON p.Id = i.PedidoId
    WHERE p.ClienteId = @ClienteId
    GROUP BY p.Id, p.DataPedido, p.Total
    ORDER BY p.DataPedido DESC;
END;
```

### Views

```sql
-- Criar view
CREATE VIEW vw_ClientesComPedidos AS
SELECT 
    c.Id AS ClienteId,
    c.Nome AS ClienteNome,
    COUNT(p.Id) AS TotalPedidos,
    SUM(p.Total) AS ValorTotal
FROM Clientes c
LEFT JOIN Pedidos p ON c.Id = p.ClienteId
GROUP BY c.Id, c.Nome;
```

### Computed Columns

```sql
-- Coluna calculada
ALTER TABLE ItensPedido
ADD Subtotal AS (Quantidade * PrecoUnitario) PERSISTED;
```

## 🔧 Configurações de Performance

### Connection Pooling

```csharp
// Configurar connection string
"Server=localhost;Database=MeuDb;Trusted_Connection=True;" +
"Min Pool Size=5;Max Pool Size=100;Connection Timeout=30;"
```

### Batch Operations

```csharp
// ✅ Inserir em lote (mais eficiente)
var clientes = new List<Cliente> { /* muitos clientes */ };
context.Clientes.AddRange(clientes);
await context.SaveChangesAsync();  // Uma transação
```

### Bulk Operations

```csharp
// Para operações em massa, considere bibliotecas:
// - EntityFrameworkCore.BulkExtensions
// - Z.EntityFramework.Extensions
```

## 📈 Monitoramento

### Métricas Importantes

- **Tempo de execução**: Queries devem ser < 100ms
- **IO Statistics**: Ler menos páginas possível
- **CPU Usage**: Queries não devem sobrecarregar CPU
- **Lock Contention**: Evitar bloqueios desnecessários

### Ferramentas

- **SQL Server Profiler**: Analisar queries em tempo real
- **Activity Monitor**: Ver processos ativos
- **Query Store**: Histórico de performance

## ✅ Verificação

- [ ] Criou índices apropriados
- [ ] Otimizou queries (SELECT específico)
- [ ] Implementou paginação
- [ ] Evitou N+1 queries
- [ ] Usou AsNoTracking quando apropriado
- [ ] Analisou execution plans
- [ ] Configurou connection pooling
- [ ] Monitorou performance

---

**🎉 Parabéns! Você completou a trilha de SQL Server!**

[← Voltar: Migrations](./04-migrations.md) | [Voltar à Trilha](./README.md)