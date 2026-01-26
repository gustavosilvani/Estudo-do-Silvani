# 🚀 Projeto Final - E-commerce Completo

## 📋 Visão Geral

Sistema de **E-commerce** completo aplicando **TODOS** os conceitos aprendidos nas trilhas:

✅ **Clean Code** - Código limpo e legível  
✅ **SOLID** - Todos os 5 princípios aplicados  
✅ **DDD** - Domain-Driven Design completo  
✅ **Testes Unitários** - xUnit + Moq  
✅ **SQL Server** - Entity Framework Core + Migrations  
✅ **APIs RESTful** - ASP.NET Core  
✅ **Dependency Injection** - DI nativo  
✅ **Unit of Work Pattern** - Transações atômicas  
✅ **Result Pattern** - Sem exceções para fluxo de negócio  

---

## 🏗️ Arquitetura

```
Ecommerce/
├── Domain/                  # 🎯 Regras de Negócio (DDD)
│   ├── Entities/            # Cliente, Pedido, ItemPedido
│   ├── ValueObjects/        # Email, Dinheiro
│   ├── Interfaces/          # IRepository, IUnitOfWork
│   └── Common/              # Result Pattern
│
├── Application/             # 📦 Casos de Uso
│   ├── Services/            # ClienteService, PedidoService
│   └── DTOs/                # Data Transfer Objects
│
├── Infrastructure/          # 🔧 Infraestrutura
│   ├── Data/                # DbContext (EF Core)
│   └── Repositories/        # Implementações + Unit of Work
│
├── Api/                     # 🌐 API REST
│   └── Controllers/         # Endpoints HTTP
│
└── Tests/                   # 🧪 Testes Automatizados
    └── Domain/              # Testes de entidades
```

### **Fluxo de Dependências (DIP/SOLID)**

```
Api → Application → Domain ← Infrastructure
```

---

## 🎯 Conceitos Aplicados

### **SOLID Principles**

- ✅ **SRP**: Cada classe tem uma única responsabilidade
- ✅ **OCP**: Extensível sem modificar código existente
- ✅ **LSP**: Substituição de implementações
- ✅ **ISP**: Interfaces segregadas (IClienteRepository, IPedidoRepository)
- ✅ **DIP**: Dependências de abstrações, não concretizações

### **DDD (Domain-Driven Design)**

- ✅ **Entities**: Cliente, Pedido (com identidade)
- ✅ **Value Objects**: Email, Dinheiro (imutáveis)
- ✅ **Aggregates**: Pedido é Aggregate Root
- ✅ **Repository Pattern**: Abstração de persistência
- ✅ **Domain Services**: Lógica de domínio

### **Clean Code**

- ✅ Nomes significativos
- ✅ Funções pequenas e focadas
- ✅ Comentários apenas quando necessário
- ✅ Código auto-explicativo

### **Testes Unitários**

- ✅ Testes de entidades
- ✅ Testes de value objects
- ✅ Testes de regras de negócio

---

## 🚀 Como Executar

### **Pré-requisitos**

- .NET 10.0 SDK
- SQL Server (ou Docker)
- Visual Studio / VS Code / Rider

### **Passos**

```bash
# 1. Restaurar pacotes
dotnet restore

# 2. Configurar banco de dados
# Editar appsettings.json com connection string:
# "ConnectionStrings": {
#   "DefaultConnection": "Server=localhost;Database=EcommerceDb;Trusted_Connection=True;TrustServerCertificate=True;"
# }

# 3. Executar aplicação
cd Ecommerce.Api
dotnet run

# 4. Executar testes
cd ../Ecommerce.Tests
dotnet test
```

### **Endpoints da API**

```
POST   /api/clientes          - Criar cliente
GET    /api/clientes/{id}     - Buscar cliente por ID
GET    /api/clientes          - Listar todos clientes

POST   /api/pedidos           - Criar pedido
GET    /api/pedidos/{id}      - Buscar pedido por ID
POST   /api/pedidos/{id}/finalizar - Finalizar pedido
```

---

## 📊 Exemplos de Uso

### **Criar Cliente**

```http
POST /api/clientes
Content-Type: application/json

{
  "nome": "João Silva",
  "email": "joao@email.com"
}
```

### **Criar Pedido**

```http
POST /api/pedidos
Content-Type: application/json

{
  "clienteId": "guid-do-cliente",
  "itens": [
    {
      "produtoId": "guid-do-produto",
      "quantidade": 2,
      "precoUnitario": 100.00
    }
  ]
}
```

---

## ✅ Checklist de Qualidade

### **Código**

- [x] SOLID aplicado
- [x] Clean Code
- [x] DDD implementado
- [x] Testes unitários
- [x] Result Pattern
- [x] Unit of Work

### **Arquitetura**

- [x] Separação de camadas
- [x] Dependency Injection
- [x] Repository Pattern
- [x] Value Objects
- [x] Entities com invariantes

---

## 🎓 Aprendizados Aplicados

Este projeto demonstra:

1. **Git**: Versionamento profissional
2. **C#**: Linguagem avançada
3. **Clean Code**: Qualidade de código
4. **Testes**: Qualidade automatizada
5. **SOLID**: Design principles
6. **DDD**: Modelagem complexa
7. **SQL Server**: Persistência robusta

---

**🎉 Projeto Final Completo - Integrando todos os conceitos aprendidos!**