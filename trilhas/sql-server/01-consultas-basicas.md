# 01 - Consultas Básicas

## 📘 Nível Básico

### SELECT - Consultar Dados

**SELECT** é o comando mais usado em SQL para consultar dados.

## 🎯 SELECT Básico

### Selecionar Tudo

```sql
-- Seleciona todas as colunas
SELECT * FROM Clientes;
```

### Selecionar Colunas Específicas

```sql
-- Seleciona apenas colunas desejadas
SELECT Nome, Email FROM Clientes;
```

### Alias (Apelidos)

```sql
-- Renomear colunas na saída
SELECT 
    Nome AS NomeCompleto,
    Email AS EmailContato
FROM Clientes;
```

## 🔍 WHERE - Filtrar Dados

### Operadores de Comparação

```sql
-- Igual
SELECT * FROM Clientes WHERE Id = 1;

-- Diferente
SELECT * FROM Clientes WHERE Id != 1;
SELECT * FROM Clientes WHERE Id <> 1;

-- Maior/Menor
SELECT * FROM Produtos WHERE Preco > 100;
SELECT * FROM Produtos WHERE Preco >= 100;
SELECT * FROM Produtos WHERE Preco < 50;
SELECT * FROM Produtos WHERE Preco <= 50;

-- Entre valores
SELECT * FROM Produtos WHERE Preco BETWEEN 50 AND 100;

-- Em lista
SELECT * FROM Clientes WHERE Id IN (1, 2, 3);

-- LIKE (busca de texto)
SELECT * FROM Clientes WHERE Nome LIKE 'João%';  -- Começa com "João"
SELECT * FROM Clientes WHERE Email LIKE '%@gmail.com';  -- Termina com "@gmail.com"
SELECT * FROM Clientes WHERE Nome LIKE '%Silva%';  -- Contém "Silva"

-- NULL
SELECT * FROM Clientes WHERE Email IS NULL;
SELECT * FROM Clientes WHERE Email IS NOT NULL;
```

### Operadores Lógicos

```sql
-- AND (E)
SELECT * FROM Produtos 
WHERE Preco > 50 AND Estoque > 0;

-- OR (OU)
SELECT * FROM Clientes 
WHERE Nome LIKE 'João%' OR Nome LIKE 'Maria%';

-- NOT (NÃO)
SELECT * FROM Produtos 
WHERE NOT Ativo = 1;
```

## 📊 ORDER BY - Ordenar

```sql
-- Ordenar crescente
SELECT * FROM Clientes ORDER BY Nome ASC;

-- Ordenar decrescente
SELECT * FROM Produtos ORDER BY Preco DESC;

-- Múltiplas colunas
SELECT * FROM Pedidos 
ORDER BY DataPedido DESC, Total ASC;
```

## 🔢 Funções Agregadas

### COUNT

```sql
-- Contar registros
SELECT COUNT(*) FROM Clientes;

-- Contar com condição
SELECT COUNT(*) FROM Produtos WHERE Ativo = 1;
```

### SUM, AVG, MIN, MAX

```sql
-- Soma
SELECT SUM(Total) AS TotalGeral FROM Pedidos;

-- Média
SELECT AVG(Preco) AS PrecoMedio FROM Produtos;

-- Mínimo
SELECT MIN(Preco) AS PrecoMinimo FROM Produtos;

-- Máximo
SELECT MAX(Preco) AS PrecoMaximo FROM Produtos;
```

### GROUP BY

```sql
-- Agrupar e agregar
SELECT 
    ClienteId,
    COUNT(*) AS TotalPedidos,
    SUM(Total) AS ValorTotal
FROM Pedidos
GROUP BY ClienteId;

-- Com HAVING (filtro após agrupamento)
SELECT 
    ClienteId,
    COUNT(*) AS TotalPedidos
FROM Pedidos
GROUP BY ClienteId
HAVING COUNT(*) > 5;  -- Apenas clientes com mais de 5 pedidos
```

## 📝 INSERT - Inserir Dados

### Inserir Um Registro

```sql
INSERT INTO Clientes (Nome, Email, CPF)
VALUES ('João Silva', 'joao@email.com', '12345678901');
```

### Inserir Múltiplos

```sql
INSERT INTO Clientes (Nome, Email, CPF)
VALUES 
    ('Maria Santos', 'maria@email.com', '98765432100'),
    ('Pedro Costa', 'pedro@email.com', '11122233344');
```

### Inserir com SELECT

```sql
-- Copiar dados de outra tabela
INSERT INTO ClientesBackup (Nome, Email)
SELECT Nome, Email FROM Clientes;
```

## ✏️ UPDATE - Atualizar Dados

### Atualizar Um Registro

```sql
UPDATE Clientes
SET Nome = 'João Silva Santos'
WHERE Id = 1;
```

### Atualizar Múltiplos

```sql
UPDATE Produtos
SET Preco = Preco * 1.10  -- Aumentar 10%
WHERE Categoria = 'Eletrônicos';
```

### Atualizar com JOIN

```sql
UPDATE Pedidos
SET Status = 'Cancelado'
FROM Pedidos p
INNER JOIN Clientes c ON p.ClienteId = c.Id
WHERE c.Email = 'cliente@email.com';
```

## 🗑️ DELETE - Deletar Dados

### Deletar Um Registro

```sql
DELETE FROM Clientes WHERE Id = 1;
```

### Deletar Múltiplos

```sql
DELETE FROM Pedidos 
WHERE DataPedido < '2024-01-01';
```

### Deletar Todos (Cuidado!)

```sql
-- ⚠️ CUIDADO: Deleta TODOS os registros!
DELETE FROM Clientes;

-- Mais seguro: usar TRUNCATE (mais rápido)
TRUNCATE TABLE Clientes;
```

## 🎯 Exemplos Práticos

### Consulta Complexa

```sql
-- Clientes que fizeram pedidos acima de R$ 500
SELECT DISTINCT
    c.Id,
    c.Nome,
    c.Email,
    COUNT(p.Id) AS TotalPedidos,
    SUM(p.Total) AS ValorTotal
FROM Clientes c
INNER JOIN Pedidos p ON c.Id = p.ClienteId
WHERE p.Total > 500
GROUP BY c.Id, c.Nome, c.Email
HAVING COUNT(p.Id) > 1
ORDER BY ValorTotal DESC;
```

### Com Entity Framework Core

```csharp
// SELECT
var clientes = await context.Clientes
    .Where(c => c.Nome.Contains("Silva"))
    .OrderBy(c => c.Nome)
    .ToListAsync();

// COUNT
var total = await context.Clientes.CountAsync();

// SUM
var totalPedidos = await context.Pedidos
    .SumAsync(p => p.Total);

// GROUP BY
var pedidosPorCliente = await context.Pedidos
    .GroupBy(p => p.ClienteId)
    .Select(g => new {
        ClienteId = g.Key,
        TotalPedidos = g.Count(),
        ValorTotal = g.Sum(p => p.Total)
    })
    .ToListAsync();

// INSERT
var cliente = new Cliente {
    Nome = "João",
    Email = "joao@email.com"
};
context.Clientes.Add(cliente);
await context.SaveChangesAsync();

// UPDATE
var cliente = await context.Clientes.FindAsync(1);
cliente.Nome = "João Silva";
await context.SaveChangesAsync();

// DELETE
var cliente = await context.Clientes.FindAsync(1);
context.Clientes.Remove(cliente);
await context.SaveChangesAsync();
```

## ✅ Verificação

- [ ] Sabe usar SELECT básico
- [ ] Conhece WHERE e operadores
- [ ] Sabe ordenar com ORDER BY
- [ ] Conhece funções agregadas
- [ ] Sabe usar GROUP BY e HAVING
- [ ] Sabe inserir dados (INSERT)
- [ ] Sabe atualizar dados (UPDATE)
- [ ] Sabe deletar dados (DELETE)
- [ ] Viu exemplos com EF Core

---

[← Voltar: Introdução ao SQL](./00-introducao-sql.md) | [Próximo: Relacionamentos →](./02-relacionamentos.md)