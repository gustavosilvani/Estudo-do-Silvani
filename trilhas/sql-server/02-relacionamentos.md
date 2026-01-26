# 02 - Relacionamentos

## 📗 Nível Intermediário

### Tipos de Relacionamentos

Em bancos relacionais, existem três tipos principais:

1. **Um para Um** (1:1)
2. **Um para Muitos** (1:N)
3. **Muitos para Muitos** (N:N)

## 🔗 Um para Um (1:1)

### Exemplo: Cliente e Endereço Principal

```sql
-- Tabela Clientes
CREATE TABLE Clientes (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nome NVARCHAR(100) NOT NULL,
    Email NVARCHAR(255) NOT NULL
);

-- Tabela Enderecos (um cliente, um endereço principal)
CREATE TABLE Enderecos (
    Id INT PRIMARY KEY IDENTITY(1,1),
    ClienteId INT NOT NULL UNIQUE,  -- UNIQUE garante 1:1
    Rua NVARCHAR(200) NOT NULL,
    Cidade NVARCHAR(100) NOT NULL,
    CEP NVARCHAR(10) NOT NULL,
    
    FOREIGN KEY (ClienteId) REFERENCES Clientes(Id)
);
```

### Consulta com JOIN

```sql
-- INNER JOIN (apenas clientes com endereço)
SELECT 
    c.Nome,
    c.Email,
    e.Rua,
    e.Cidade
FROM Clientes c
INNER JOIN Enderecos e ON c.Id = e.ClienteId;

-- LEFT JOIN (todos clientes, mesmo sem endereço)
SELECT 
    c.Nome,
    c.Email,
    e.Rua,
    e.Cidade
FROM Clientes c
LEFT JOIN Enderecos e ON c.Id = e.ClienteId;
```

## 🔗 Um para Muitos (1:N)

### Exemplo: Cliente e Pedidos

```sql
-- Tabela Clientes (1)
CREATE TABLE Clientes (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nome NVARCHAR(100) NOT NULL
);

-- Tabela Pedidos (N - muitos)
CREATE TABLE Pedidos (
    Id INT PRIMARY KEY IDENTITY(1,1),
    ClienteId INT NOT NULL,  -- Múltiplos pedidos podem ter mesmo ClienteId
    DataPedido DATETIME2 NOT NULL,
    Total DECIMAL(18,2) NOT NULL,
    
    FOREIGN KEY (ClienteId) REFERENCES Clientes(Id)
);
```

### Consulta com JOIN

```sql
-- Cliente com seus pedidos
SELECT 
    c.Nome AS ClienteNome,
    p.Id AS PedidoId,
    p.DataPedido,
    p.Total
FROM Clientes c
INNER JOIN Pedidos p ON c.Id = p.ClienteId
ORDER BY c.Nome, p.DataPedido DESC;

-- Cliente com total de pedidos
SELECT 
    c.Nome,
    COUNT(p.Id) AS TotalPedidos,
    SUM(p.Total) AS ValorTotal
FROM Clientes c
LEFT JOIN Pedidos p ON c.Id = p.ClienteId
GROUP BY c.Id, c.Nome;
```

## 🔗 Muitos para Muitos (N:N)

### Exemplo: Produtos e Categorias

```sql
-- Tabela Produtos
CREATE TABLE Produtos (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nome NVARCHAR(100) NOT NULL,
    Preco DECIMAL(18,2) NOT NULL
);

-- Tabela Categorias
CREATE TABLE Categorias (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nome NVARCHAR(100) NOT NULL
);

-- Tabela de junção (junction table)
CREATE TABLE ProdutoCategoria (
    ProdutoId INT NOT NULL,
    CategoriaId INT NOT NULL,
    
    PRIMARY KEY (ProdutoId, CategoriaId),
    FOREIGN KEY (ProdutoId) REFERENCES Produtos(Id),
    FOREIGN KEY (CategoriaId) REFERENCES Categorias(Id)
);
```

### Consulta com JOIN

```sql
-- Produtos com suas categorias
SELECT 
    p.Nome AS ProdutoNome,
    c.Nome AS CategoriaNome
FROM Produtos p
INNER JOIN ProdutoCategoria pc ON p.Id = pc.ProdutoId
INNER JOIN Categorias c ON pc.CategoriaId = c.Id;

-- Categoria com quantidade de produtos
SELECT 
    c.Nome AS CategoriaNome,
    COUNT(p.Id) AS TotalProdutos
FROM Categorias c
LEFT JOIN ProdutoCategoria pc ON c.Id = pc.CategoriaId
LEFT JOIN Produtos p ON pc.ProdutoId = p.Id
GROUP BY c.Id, c.Nome;
```

## 🔍 Tipos de JOIN

### INNER JOIN

Retorna apenas registros que têm correspondência em ambas tabelas.

```sql
-- Apenas clientes que têm pedidos
SELECT c.Nome, p.Total
FROM Clientes c
INNER JOIN Pedidos p ON c.Id = p.ClienteId;
```

### LEFT JOIN (LEFT OUTER JOIN)

Retorna todos registros da tabela esquerda, mesmo sem correspondência.

```sql
-- Todos clientes, mesmo sem pedidos
SELECT c.Nome, p.Total
FROM Clientes c
LEFT JOIN Pedidos p ON c.Id = p.ClienteId;
```

### RIGHT JOIN (RIGHT OUTER JOIN)

Retorna todos registros da tabela direita, mesmo sem correspondência.

```sql
-- Todos pedidos, mesmo sem cliente (improvável, mas possível)
SELECT c.Nome, p.Total
FROM Clientes c
RIGHT JOIN Pedidos p ON c.Id = p.ClienteId;
```

### FULL OUTER JOIN

Retorna todos registros de ambas tabelas.

```sql
SELECT c.Nome, p.Total
FROM Clientes c
FULL OUTER JOIN Pedidos p ON c.Id = p.ClienteId;
```

## 📊 Exemplo Completo: E-commerce

### Estrutura de Tabelas

```sql
-- Clientes
CREATE TABLE Clientes (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nome NVARCHAR(100) NOT NULL,
    Email NVARCHAR(255) NOT NULL UNIQUE
);

-- Produtos
CREATE TABLE Produtos (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nome NVARCHAR(100) NOT NULL,
    Preco DECIMAL(18,2) NOT NULL
);

-- Pedidos (1:N com Clientes)
CREATE TABLE Pedidos (
    Id INT PRIMARY KEY IDENTITY(1,1),
    ClienteId INT NOT NULL,
    DataPedido DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    Total DECIMAL(18,2) NOT NULL,
    
    FOREIGN KEY (ClienteId) REFERENCES Clientes(Id)
);

-- ItensPedido (N:N entre Pedidos e Produtos)
CREATE TABLE ItensPedido (
    Id INT PRIMARY KEY IDENTITY(1,1),
    PedidoId INT NOT NULL,
    ProdutoId INT NOT NULL,
    Quantidade INT NOT NULL,
    PrecoUnitario DECIMAL(18,2) NOT NULL,
    
    FOREIGN KEY (PedidoId) REFERENCES Pedidos(Id),
    FOREIGN KEY (ProdutoId) REFERENCES Produtos(Id)
);
```

### Consultas Complexas

```sql
-- Pedido completo com cliente e itens
SELECT 
    p.Id AS PedidoId,
    c.Nome AS ClienteNome,
    p.DataPedido,
    pr.Nome AS ProdutoNome,
    ip.Quantidade,
    ip.PrecoUnitario,
    (ip.Quantidade * ip.PrecoUnitario) AS Subtotal
FROM Pedidos p
INNER JOIN Clientes c ON p.ClienteId = c.Id
INNER JOIN ItensPedido ip ON p.Id = ip.PedidoId
INNER JOIN Produtos pr ON ip.ProdutoId = pr.Id
ORDER BY p.Id, pr.Nome;

-- Relatório: Cliente com resumo de pedidos
SELECT 
    c.Id AS ClienteId,
    c.Nome AS ClienteNome,
    COUNT(DISTINCT p.Id) AS TotalPedidos,
    SUM(p.Total) AS ValorTotalGasto,
    AVG(p.Total) AS TicketMedio
FROM Clientes c
LEFT JOIN Pedidos p ON c.Id = p.ClienteId
GROUP BY c.Id, c.Nome
ORDER BY ValorTotalGasto DESC;
```

## 🎯 Entity Framework Core

### Relacionamentos no EF Core

```csharp
// Modelo com relacionamento 1:N
public class Cliente {
    public int Id { get; set; }
    public string Nome { get; set; }
    public List<Pedido> Pedidos { get; set; } = new();  // Navegação
}

public class Pedido {
    public int Id { get; set; }
    public int ClienteId { get; set; }  // Foreign Key
    public Cliente Cliente { get; set; }  // Navegação
    public List<ItemPedido> Itens { get; set; } = new();
}

// Configuração no DbContext
protected override void OnModelCreating(ModelBuilder modelBuilder) {
    modelBuilder.Entity<Pedido>()
        .HasOne(p => p.Cliente)
        .WithMany(c => c.Pedidos)
        .HasForeignKey(p => p.ClienteId);
}

// Consulta com Include (JOIN automático)
var clientesComPedidos = await context.Clientes
    .Include(c => c.Pedidos)
    .ThenInclude(p => p.Itens)
    .ToListAsync();
```

## ✅ Verificação

- [ ] Entendeu tipos de relacionamentos (1:1, 1:N, N:N)
- [ ] Sabe criar Foreign Keys
- [ ] Conhece tipos de JOIN (INNER, LEFT, RIGHT, FULL)
- [ ] Sabe fazer consultas com múltiplos JOINs
- [ ] Implementou relacionamentos no EF Core
- [ ] Usou Include para carregar relacionamentos
- [ ] Viu exemplos práticos

---

[← Voltar: Consultas Básicas](./01-consultas-basicas.md) | [Próximo: Entity Framework Core →](./03-entity-framework.md)