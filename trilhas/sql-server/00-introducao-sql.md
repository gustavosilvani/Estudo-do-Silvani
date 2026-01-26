# 00 - Introdução ao SQL

## 📘 Nível Básico

### O que é SQL?

**SQL** (Structured Query Language) é a linguagem padrão para gerenciar bancos de dados relacionais.

## 🎯 Por que SQL?

### Benefícios

- **Padrão universal**: Funciona em todos bancos relacionais
- **Poderoso**: Consultas complexas e eficientes
- **Declarativo**: Diz o que quer, não como fazer
- **Integrado**: .NET tem excelente suporte

## 🗄️ Conceitos Fundamentais

### Banco de Dados Relacional

```
┌─────────────┐
│   Tabela    │
│  (Table)    │
├─────────────┤
│  Colunas    │
│  (Columns)  │
│             │
│  Linhas     │
│  (Rows)     │
└─────────────┘
```

### Exemplo: Tabela Clientes

```sql
CREATE TABLE Clientes (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nome NVARCHAR(100) NOT NULL,
    Email NVARCHAR(255) NOT NULL UNIQUE,
    DataCriacao DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);
```

### Tipos de Dados Comuns

```sql
-- Texto
NVARCHAR(100)      -- Texto variável (Unicode)
VARCHAR(100)       -- Texto variável (ASCII)
TEXT               -- Texto grande

-- Números
INT                -- Número inteiro
BIGINT             -- Número inteiro grande
DECIMAL(18,2)      -- Número decimal (18 dígitos, 2 decimais)
FLOAT              -- Número de ponto flutuante

-- Data/Hora
DATETIME2          -- Data e hora
DATE               -- Apenas data
TIME               -- Apenas hora

-- Booleano
BIT                -- 0 ou 1 (True/False)

-- Binário
VARBINARY(MAX)     -- Dados binários
```

## 🔑 Chaves e Relacionamentos

### Primary Key (Chave Primária)

```sql
CREATE TABLE Produtos (
    Id INT PRIMARY KEY IDENTITY(1,1),  -- Chave primária auto-incremento
    Nome NVARCHAR(100) NOT NULL,
    Preco DECIMAL(18,2) NOT NULL
);
```

### Foreign Key (Chave Estrangeira)

```sql
CREATE TABLE Pedidos (
    Id INT PRIMARY KEY IDENTITY(1,1),
    ClienteId INT NOT NULL,
    DataPedido DATETIME2 NOT NULL,
    
    FOREIGN KEY (ClienteId) REFERENCES Clientes(Id)  -- Relacionamento
);
```

## 📊 Operações Básicas (CRUD)

### CREATE (INSERT)

```sql
-- Inserir um registro
INSERT INTO Clientes (Nome, Email)
VALUES ('João Silva', 'joao@email.com');

-- Inserir múltiplos
INSERT INTO Clientes (Nome, Email)
VALUES 
    ('Maria Santos', 'maria@email.com'),
    ('Pedro Costa', 'pedro@email.com');
```

### READ (SELECT)

```sql
-- Selecionar todos
SELECT * FROM Clientes;

-- Selecionar colunas específicas
SELECT Nome, Email FROM Clientes;

-- Com condição
SELECT * FROM Clientes WHERE Id = 1;

-- Ordenar
SELECT * FROM Clientes ORDER BY Nome ASC;
```

### UPDATE

```sql
-- Atualizar um registro
UPDATE Clientes
SET Nome = 'João Silva Santos'
WHERE Id = 1;

-- Atualizar múltiplos
UPDATE Clientes
SET DataCriacao = GETUTCDATE()
WHERE DataCriacao IS NULL;
```

### DELETE

```sql
-- Deletar um registro
DELETE FROM Clientes WHERE Id = 1;

-- Deletar todos (cuidado!)
DELETE FROM Clientes;
```

## 🎯 Entity Framework Core

### O que é EF Core?

**Entity Framework Core** é um ORM (Object-Relational Mapping) que permite trabalhar com banco de dados usando objetos C#.

### Vantagens

- **Código C#**: Não precisa escrever SQL manualmente
- **Type-safe**: Compilador verifica erros
- **Migrations**: Controle de versão do banco
- **LINQ**: Consultas usando C#

### Exemplo Básico

```csharp
// Modelo
public class Cliente {
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
}

// DbContext
public class AppDbContext : DbContext {
    public DbSet<Cliente> Clientes { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder options) {
        options.UseSqlServer("Server=localhost;Database=MeuDb;Trusted_Connection=True;");
    }
}

// Uso
using var context = new AppDbContext();

// CREATE
var cliente = new Cliente { Nome = "João", Email = "joao@email.com" };
context.Clientes.Add(cliente);
await context.SaveChangesAsync();

// READ
var clientes = await context.Clientes.ToListAsync();
var cliente = await context.Clientes.FindAsync(1);

// UPDATE
cliente.Nome = "João Silva";
await context.SaveChangesAsync();

// DELETE
context.Clientes.Remove(cliente);
await context.SaveChangesAsync();
```

## 📚 Estrutura de Banco de Dados

### Exemplo: E-commerce

```sql
-- Tabela de Clientes
CREATE TABLE Clientes (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nome NVARCHAR(100) NOT NULL,
    Email NVARCHAR(255) NOT NULL UNIQUE,
    CPF NVARCHAR(11) NOT NULL UNIQUE,
    DataCriacao DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

-- Tabela de Produtos
CREATE TABLE Produtos (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nome NVARCHAR(100) NOT NULL,
    Descricao NVARCHAR(MAX),
    Preco DECIMAL(18,2) NOT NULL,
    Estoque INT NOT NULL DEFAULT 0,
    Ativo BIT NOT NULL DEFAULT 1
);

-- Tabela de Pedidos
CREATE TABLE Pedidos (
    Id INT PRIMARY KEY IDENTITY(1,1),
    ClienteId INT NOT NULL,
    DataPedido DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    Status NVARCHAR(50) NOT NULL DEFAULT 'Pendente',
    Total DECIMAL(18,2) NOT NULL,
    
    FOREIGN KEY (ClienteId) REFERENCES Clientes(Id)
);

-- Tabela de Itens do Pedido
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

## ✅ Verificação

- [ ] Entendeu o que é SQL
- [ ] Conhece tipos de dados básicos
- [ ] Sabe criar tabelas
- [ ] Entende Primary Key e Foreign Key
- [ ] Conhece operações CRUD básicas
- [ ] Entendeu o que é Entity Framework Core
- [ ] Viu exemplo básico de EF Core

---

[Próximo: Consultas Básicas →](./01-consultas-basicas.md) | [Voltar à Trilha](./README.md)