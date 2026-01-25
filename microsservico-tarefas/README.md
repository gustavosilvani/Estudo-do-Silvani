# 🚀 Microsserviço: Gerenciamento de Tarefas

## 📋 Visão Geral

Microsserviço **completo e profissional** aplicando **TODOS** os conceitos das trilhas:

✅ **Clean Code** - Código limpo e legível  
✅ **SOLID** - Todos os 5 princípios  
✅ **DDD** - Domain-Driven Design  
✅ **Testes Unitários** - xUnit + Moq  
✅ **APIs RESTful** - ASP.NET Core  
✅ **Dependency Injection** - DI nativo  
✅ **Unit of Work Pattern** - Transações atômicas  
✅ **Result Pattern** - Sem exceções para fluxo

---

## 🏗️ Arquitetura

```
TaskManagement/
├── Domain/                  # 🎯 Regras de Negócio
│   ├── Entities/            # Entidades (TarefaTask, Comentario)
│   ├── ValueObjects/        # Value Objects (Titulo, Enums)
│   ├── Interfaces/          # Contratos (ITarefaRepository, IUnitOfWork)
│   └── Common/              # Result Pattern
│
├── Application/             # 📦 Casos de Uso
│   ├── Services/            # Serviços de aplicação
│   └── DTOs/                # Data Transfer Objects
│
├── Infrastructure/          # 🔧 Infraestrutura
│   ├── Data/                # DbContext (EF Core)
│   └── Repositories/        # Implementações + Unit of Work
│
├── Api/                     # 🌐 API REST
│   ├── Controllers/         # Endpoints HTTP
│   └── Program.cs           # Configuração DI + Middleware
│
└── Tests/                   # 🧪 Testes Automatizados
    ├── Domain/              # Testes de entidades
    └── Application/         # Testes de serviços (com mocks)
```

### **Fluxo de Dependências (DIP/SOLID)**

```
Api → Application → Domain ← Infrastructure
```

- **Domain** não depende de ninguém
- **Application** depende apenas do Domain
- **Infrastructure** implementa contratos do Domain
- **Api** orquestra tudo

---

## 🎯 Conceitos Aplicados

### **1. Result Pattern**
```csharp
// ✅ Sem exceções para fluxo de negócio
public Result<TarefaTask> Criar(...) {
    if (invalido) {
        return Result.Fail<TarefaTask>("Erro descritivo");
    }
    return Result.Ok(tarefa);
}
```

### **2. Unit of Work Pattern**
```csharp
// ✅ Transação atômica
using (var uow = new UnitOfWork(context)) {
    await uow.Tarefas.AdicionarAsync(tarefa);
    await uow.CommitAsync();  // Tudo ou nada
}
```

### **3. Domain-Driven Design**
- **Aggregate Root**: `TarefaTask`
- **Value Objects**: `Titulo` (imutável)
- **Entities**: `Comentario`
- **Repository Pattern**: `ITarefaRepository`

### **4. SOLID**
- **SRP**: Cada classe uma responsabilidade
- **OCP**: Extensível via interfaces
- **LSP**: Substituição via polimorfismo
- **ISP**: Interfaces segregadas
- **DIP**: Depende de abstrações

### **5. Clean Code**
- Nomes descritivos
- Funções pequenas
- Sem código duplicado
- Comentários apenas onde necessário

---

## 🚀 Como Executar

### **1. Navegar para a API**
```bash
cd microsservico-tarefas/TaskManagement.Api
```

### **2. Restaurar dependências**
```bash
dotnet restore
```

### **3. Executar**
```bash
dotnet run
```

### **4. Acessar Swagger**
```
http://localhost:5000
```

---

## 🧪 Executar Testes

```bash
cd microsservico-tarefas/TaskManagement.Tests
dotnet test --logger "console;verbosity=detailed"
```

### **Cobertura de Testes**
- ✅ Testes de entidades (regras de negócio)
- ✅ Testes de value objects
- ✅ Testes de serviços (com mocks)
- ✅ AAA Pattern (Arrange-Act-Assert)
- ✅ TDD mindset

---

## 📡 Endpoints da API

### **Criar Tarefa**
```http
POST /api/tarefas
Content-Type: application/json

{
  "titulo": "Implementar feature X",
  "descricao": "Descrição detalhada",
  "prioridade": 2,  // 0=Baixa, 1=Media, 2=Alta, 3=Urgente
  "dataVencimento": "2026-02-01T00:00:00Z"
}
```

### **Listar Tarefas**
```http
GET /api/tarefas
```

### **Buscar por ID**
```http
GET /api/tarefas/{id}
```

### **Concluir Tarefa**
```http
PATCH /api/tarefas/{id}/concluir
```

### **Cancelar Tarefa**
```http
PATCH /api/tarefas/{id}/cancelar
```

### **Iniciar Execução**
```http
PATCH /api/tarefas/{id}/iniciar
```

### **Adicionar Comentário**
```http
POST /api/tarefas/{id}/comentarios
Content-Type: application/json

{
  "texto": "Progresso atualizado"
}
```

### **Deletar Tarefa**
```http
DELETE /api/tarefas/{id}
```

---

## 📊 Estrutura do Banco (EF Core InMemory)

```sql
Tarefas
  - Id (Guid)
  - Titulo (string, 100)
  - Descricao (string, 1000)
  - Status (int)
  - Prioridade (int)
  - DataCriacao (DateTime)
  - DataConclusao (DateTime?)
  - DataVencimento (DateTime?)
  - UsuarioId (Guid)

Comentarios
  - Id (Guid)
  - Texto (string, 500)
  - UsuarioId (Guid)
  - DataCriacao (DateTime)
  - TarefaTaskId (Guid, FK)
```

---

## 💡 Exemplos de Uso

### **1. Fluxo Completo**
```bash
# 1. Criar tarefa
curl -X POST http://localhost:5000/api/tarefas \
  -H "Content-Type: application/json" \
  -d '{
    "titulo": "Estudar SOLID",
    "descricao": "Revisar todos os princípios",
    "prioridade": 2
  }'

# 2. Iniciar execução
curl -X PATCH http://localhost:5000/api/tarefas/{id}/iniciar

# 3. Adicionar comentário
curl -X POST http://localhost:5000/api/tarefas/{id}/comentarios \
  -H "Content-Type: application/json" \
  -d '{"texto": "50% concluído"}'

# 4. Concluir
curl -X PATCH http://localhost:5000/api/tarefas/{id}/concluir
```

---

## 🎓 O que você pode aprender aqui

1. **Result Pattern** - Tratamento de erros sem exceções
2. **Unit of Work** - Gerenciamento de transações
3. **DDD** - Modelagem rica de domínio
4. **Clean Architecture** - Separação de responsabilidades
5. **Dependency Injection** - Inversão de controle
6. **RESTful APIs** - Design de APIs
7. **Entity Framework Core** - ORM e persistência
8. **Testes Unitários** - xUnit + Moq
9. **SOLID** - Todos os 5 princípios na prática
10. **Clean Code** - Código profissional

---

## 🔥 Diferenciais

✨ **100% Conceitos das Trilhas**  
✨ **Código Produção-Ready**  
✨ **Totalmente Testado**  
✨ **Documentado**  
✨ **Executável Imediatamente**  
✨ **Swagger Integrado**  
✨ **Clean Architecture**  
✨ **Patterns Avançados**

---

## 📚 Referências

- [Documentação Completa](./DOCUMENTACAO-COMPLETA.md)
- Trilha SOLID
- Trilha Clean Code
- Trilha Testes Unitários
- Trilha C#/.NET

---

**🎉 Microsserviço criado por: AI Assistant**  
**📅 Data: Janeiro 2026**  
**🎯 Propósito: Demonstração completa de todos os conceitos ensinados**
