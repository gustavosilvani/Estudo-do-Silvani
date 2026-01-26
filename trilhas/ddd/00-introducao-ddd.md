# 00 - Introdução ao DDD

## 📘 Nível Básico

### O que é Domain-Driven Design (DDD)?

Domain-Driven Design é uma abordagem de desenvolvimento de software criada por **Eric Evans** em 2003, focada em **modelar o domínio do negócio** de forma rica e expressiva.

## 🎯 Conceito Central

DDD coloca o **domínio** (área de negócio) no centro do desenvolvimento, não a tecnologia.

```
❌ ANTES (Tecnologia no centro):
- "Preciso de uma tabela de usuários"
- "Vou criar um controller para isso"
- "Como salvar no banco?"

✅ DEPOIS (Domínio no centro):
- "O que é um Usuário no negócio?"
- "Quais são as regras de negócio?"
- "Como o domínio se comporta?"
```

## 🏛️ Pilares do DDD

### 1. **Linguagem Ubíqua (Ubiquitous Language)**
Linguagem comum entre desenvolvedores e especialistas do negócio.

```csharp
// ❌ RUIM - Linguagem técnica
public class User {
    public int Id { get; set; }
    public string Email { get; set; }
}

// ✅ BOM - Linguagem do negócio
public class Cliente {
    public ClienteId Id { get; }
    public Email Email { get; }
    
    public void RealizarPedido(Pedido pedido) {
        // Regra de negócio expressa na linguagem do domínio
    }
}
```

### 2. **Modelo Rico de Domínio**
O domínio contém lógica de negócio, não apenas dados.

```csharp
// ❌ RUIM - Anêmico (só dados)
public class Pedido {
    public decimal Total { get; set; }
    public string Status { get; set; }
}

// Lógica em outro lugar
public class PedidoService {
    public void CalcularTotal(Pedido pedido) {
        // Lógica fora do domínio
    }
}

// ✅ BOM - Rico (comportamento no domínio)
public class Pedido {
    private readonly List<ItemPedido> _itens;
    public decimal Total { get; private set; }
    public StatusPedido Status { get; private set; }
    
    public void AdicionarItem(Produto produto, int quantidade) {
        // Regra de negócio no domínio
        if (Status != StatusPedido.Rascunho) {
            throw new InvalidOperationException("Não é possível adicionar itens a pedido finalizado");
        }
        
        var item = new ItemPedido(produto, quantidade);
        _itens.Add(item);
        RecalcularTotal();
    }
    
    private void RecalcularTotal() {
        Total = _itens.Sum(i => i.Subtotal);
    }
}
```

### 3. **Separação de Responsabilidades**
DDD organiza código em camadas claras.

```
┌─────────────────────┐
│   Presentation      │ ← Controllers, Views
├─────────────────────┤
│   Application       │ ← Use Cases, Orquestração
├─────────────────────┤
│   Domain            │ ← Entidades, Regras de Negócio
├─────────────────────┤
│   Infrastructure    │ ← Persistência, APIs Externas
└─────────────────────┘
```

## 🎯 Quando Usar DDD?

### ✅ Use DDD quando:
- Domínio complexo com muitas regras de negócio
- Equipe grande precisa de comunicação clara
- Software de longa duração (anos)
- Mudanças frequentes no domínio
- Necessidade de modelagem rica

### ❌ Evite DDD quando:
- Domínio simples (CRUD básico)
- Projeto pequeno e de curta duração
- Equipe pequena sem especialistas de domínio
- Foco apenas em persistência de dados

## 📚 Conceitos Fundamentais

### Entidades (Entities)
Objetos com identidade única que persistem ao longo do tempo.

```csharp
public class Cliente {
    public ClienteId Id { get; }  // Identidade única
    public string Nome { get; private set; }
    
    // Mesmo cliente, mesmo ID, mesmo objeto
}
```

### Value Objects
Objetos definidos apenas por seus valores, sem identidade.

```csharp
public class Email {
    public string Valor { get; }
    
    // Dois emails com mesmo valor são iguais
    // Não têm ID próprio
}
```

### Aggregates
Grupo de entidades e value objects tratados como uma unidade.

```csharp
public class Pedido {  // Aggregate Root
    public PedidoId Id { get; }
    private List<ItemPedido> _itens;  // Parte do aggregate
    
    // Pedido controla seus itens
    // Itens não existem sem pedido
}
```

### Repositories
Abstração para persistência, escondendo detalhes de banco de dados.

```csharp
public interface IPedidoRepository {
    Task<Pedido> BuscarPorIdAsync(PedidoId id);
    Task SalvarAsync(Pedido pedido);
}
```

## 🎓 Benefícios do DDD

### 1. **Comunicação Clara**
Linguagem ubíqua elimina mal-entendidos entre devs e negócio.

### 2. **Código Expressivo**
Código reflete o domínio, facilitando entendimento.

### 3. **Manutenibilidade**
Mudanças no domínio são refletidas naturalmente no código.

### 4. **Testabilidade**
Domínio isolado é fácil de testar.

### 5. **Escalabilidade**
Arquitetura preparada para crescer.

## 📖 Histórico

### Eric Evans (2003)
Publicou "Domain-Driven Design: Tackling Complexity in the Heart of Software", estabelecendo DDD como metodologia.

### Vaughn Vernon (2013)
Publicou "Implementing Domain-Driven Design", trazendo padrões práticos e implementações.

### Eventos Recentes
- CQRS (Command Query Responsibility Segregation)
- Event Sourcing
- Microservices com DDD

## 🎯 Exemplo Prático - E-commerce

### Sem DDD (Anêmico)

```csharp
// ❌ Modelo anêmico
public class Pedido {
    public int Id { get; set; }
    public decimal Total { get; set; }
    public string Status { get; set; }
}

// Lógica espalhada
public class PedidoService {
    public void Processar(Pedido pedido) {
        // Toda lógica aqui
    }
}
```

### Com DDD (Rico)

```csharp
// ✅ Modelo rico
public class Pedido {
    private readonly List<ItemPedido> _itens;
    public PedidoId Id { get; }
    public StatusPedido Status { get; private set; }
    
    public void AdicionarItem(Produto produto, int quantidade) {
        // Regra: não adicionar itens a pedido finalizado
        if (Status != StatusPedido.Rascunho) {
            throw new PedidoJaFinalizadoException();
        }
        
        // Regra: quantidade mínima
        if (quantidade <= 0) {
            throw new QuantidadeInvalidaException();
        }
        
        var item = new ItemPedido(produto, quantidade);
        _itens.Add(item);
    }
    
    public void Finalizar() {
        // Regra: pedido vazio não pode ser finalizado
        if (!_itens.Any()) {
            throw new PedidoVazioException();
        }
        
        Status = StatusPedido.Finalizado;
    }
}
```

## ✅ Verificação

- [ ] Entendeu o que é DDD
- [ ] Conhece os pilares (Linguagem Ubíqua, Modelo Rico, Separação)
- [ ] Sabe quando usar DDD
- [ ] Conhece conceitos básicos (Entidades, Value Objects, Aggregates)
- [ ] Entende diferença entre modelo anêmico e rico
- [ ] Viu exemplo prático

---

[Próximo: Linguagem Ubíqua →](./01-linguagem-ubiqua.md) | [Voltar à Trilha](./README.md)