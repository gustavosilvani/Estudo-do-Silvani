# 04 - Migrations e Versionamento

## 📗 Nível Intermediário

### O que são Migrations?

**Migrations** são uma forma de versionar mudanças no esquema do banco de dados, permitindo evoluir o banco de forma controlada.

## 🎯 Por que Migrations?

### Benefícios

- **Versionamento**: Histórico de mudanças no banco
- **Colaboração**: Time sincroniza banco facilmente
- **Deploy**: Aplicar mudanças em produção de forma segura
- **Rollback**: Desfazer mudanças se necessário

## 🔧 Instalação

```bash
# Instalar ferramentas
dotnet tool install --global dotnet-ef

# Verificar instalação
dotnet ef --version
```

## 📝 Criando Migrations

### Primeira Migration

```bash
# Criar migration inicial
dotnet ef migrations add InitialCreate

# Isso cria:
# - Migrations/20240125120000_InitialCreate.cs
# - Snapshot do modelo atual
```

### Migration de Mudança

```csharp
// 1. Alterar modelo
public class Cliente {
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Telefone { get; set; }  // Nova propriedade
}

// 2. Criar migration
dotnet ef migrations add AdicionarTelefoneCliente
```

## 🚀 Aplicando Migrations

### Aplicar no Banco

```bash
# Aplicar todas migrations pendentes
dotnet ef database update

# Aplicar até migration específica
dotnet ef database update AdicionarTelefoneCliente

# Aplicar migration inicial (cria banco se não existir)
dotnet ef database update InitialCreate
```

### Em Código (Program.cs)

```csharp
var app = builder.Build();

// Aplicar migrations automaticamente
using (var scope = app.Services.CreateScope()) {
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.Migrate();  // Aplica migrations pendentes
}

app.Run();
```

## 📊 Estrutura de Migration

### Arquivo Gerado

```csharp
public partial class AdicionarTelefoneCliente : Migration {
    protected override void Up(MigrationBuilder migrationBuilder) {
        // O que fazer ao aplicar migration
        migrationBuilder.AddColumn<string>(
            name: "Telefone",
            table: "Clientes",
            type: "nvarchar(20)",
            maxLength: 20,
            nullable: true);
    }
    
    protected override void Down(MigrationBuilder migrationBuilder) {
        // O que fazer ao reverter migration
        migrationBuilder.DropColumn(
            name: "Telefone",
            table: "Clientes");
    }
}
```

## 🔄 Operações Comuns

### Adicionar Coluna

```csharp
// Modelo
public class Cliente {
    public string Telefone { get; set; }  // Nova propriedade
}

// Migration gerada automaticamente
migrationBuilder.AddColumn<string>(
    name: "Telefone",
    table: "Clientes",
    nullable: true);
```

### Remover Coluna

```csharp
// Remover propriedade do modelo
// Migration gerada:
migrationBuilder.DropColumn(
    name: "Telefone",
    table: "Clientes");
```

### Alterar Coluna

```csharp
// Alterar tipo/tamanho
migrationBuilder.AlterColumn<string>(
    name: "Nome",
    table: "Clientes",
    type: "nvarchar(200)",  // Era 100, agora 200
    maxLength: 200,
    nullable: false);
```

### Criar Tabela

```csharp
migrationBuilder.CreateTable(
    name: "Produtos",
    columns: table => new {
        Id = table.Column<int>(type: "int", nullable: false)
            .Annotation("SqlServer:Identity", "1, 1"),
        Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
        Preco = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
    },
    constraints: table => {
        table.PrimaryKey("PK_Produtos", x => x.Id);
    });
```

### Criar Índice

```csharp
migrationBuilder.CreateIndex(
    name: "IX_Clientes_Email",
    table: "Clientes",
    column: "Email",
    unique: true);
```

## 🔙 Reverter Migrations

### Remover Última Migration

```bash
# Remover última migration (não aplicada)
dotnet ef migrations remove

# Reverter banco para migration anterior
dotnet ef database update NomeDaMigrationAnterior
```

### Rollback Manual

```csharp
// No método Down da migration
protected override void Down(MigrationBuilder migrationBuilder) {
    // Código para desfazer mudanças
    migrationBuilder.DropColumn(
        name: "Telefone",
        table: "Clientes");
}
```

## 📋 Scripts SQL

### Gerar Script

```bash
# Gerar script SQL de todas migrations
dotnet ef migrations script

# Gerar script de migration específica
dotnet ef migrations script InitialCreate AdicionarTelefoneCliente

# Gerar script para aplicar no banco
dotnet ef migrations script --output migration.sql
```

### Script Gerado

```sql
IF NOT EXISTS (SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20240125120000_InitialCreate')
BEGIN
    CREATE TABLE [Clientes] (
        [Id] int NOT NULL IDENTITY,
        [Nome] nvarchar(100) NOT NULL,
        [Email] nvarchar(255) NOT NULL,
        CONSTRAINT [PK_Clientes] PRIMARY KEY ([Id])
    );
    
    CREATE UNIQUE INDEX [IX_Clientes_Email] ON [Clientes] ([Email]);
    
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240125120000_InitialCreate', N'9.0.0');
END;
```

## 🎯 Boas Práticas

### 1. Nomes Descritivos

```bash
# ✅ BOM
dotnet ef migrations add AdicionarTelefoneCliente
dotnet ef migrations add CriarTabelaProdutos
dotnet ef migrations add AdicionarIndiceEmail

# ❌ RUIM
dotnet ef migrations add Migration1
dotnet ef migrations add Update
```

### 2. Migrations Pequenas

```bash
# ✅ BOM - Uma mudança por migration
dotnet ef migrations add AdicionarTelefone
dotnet ef migrations add AdicionarEndereco

# ❌ RUIM - Muitas mudanças juntas
dotnet ef migrations add AdicionarTelefoneEEnderecoECPF
```

### 3. Revisar Antes de Aplicar

```bash
# Sempre revisar migration gerada
# Antes de aplicar em produção
```

## 📊 Exemplo Completo

### Evolução do Banco

```bash
# 1. Migration inicial
dotnet ef migrations add InitialCreate
# Cria: Clientes, Produtos

# 2. Adicionar Pedidos
dotnet ef migrations add CriarTabelaPedidos
# Cria: Pedidos, ItensPedido

# 3. Adicionar campo
dotnet ef migrations add AdicionarTelefoneCliente
# Altera: Clientes (adiciona Telefone)

# 4. Aplicar todas
dotnet ef database update
```

### Histórico de Migrations

```
Migrations/
├── 20240125120000_InitialCreate.cs
├── 20240125130000_CriarTabelaPedidos.cs
├── 20240125140000_AdicionarTelefoneCliente.cs
└── AppDbContextModelSnapshot.cs
```

## ✅ Verificação

- [ ] Instalou dotnet-ef
- [ ] Criou primeira migration
- [ ] Aplicou migrations no banco
- [ ] Criou migrations de alteração
- [ ] Reverteu migrations
- [ ] Gerou scripts SQL
- [ ] Seguiu boas práticas

---

[← Voltar: Entity Framework Core](./03-entity-framework.md) | [Próximo: Otimização →](./05-otimizacao.md)