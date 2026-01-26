# 05 - Bounded Contexts

## 📗 Nível Intermediário

### O que são Bounded Contexts?

**Bounded Context** é um limite explícito dentro do qual um modelo de domínio específico se aplica. Cada contexto tem sua própria linguagem ubíqua e modelo.

## 🎯 Problema: Modelo Único Grande

### ❌ Sem Bounded Contexts

```csharp
// ❌ Problema: "Cliente" significa coisas diferentes em contextos diferentes
public class Cliente {
    // E-commerce: Cliente compra produtos
    public List<Pedido> Pedidos { get; set; }
    
    // CRM: Cliente tem leads e oportunidades
    public List<Lead> Leads { get; set; }
    
    // Financeiro: Cliente tem faturas
    public List<Fatura> Faturas { get; set; }
    
    // Confuso! Muitas responsabilidades!
}
```

### ✅ Com Bounded Contexts

```csharp
// ✅ Contexto: E-commerce
namespace Ecommerce.Domain {
    public class Cliente {
        public ClienteId Id { get; }
        public List<Pedido> Pedidos { get; }
        // Focado apenas em compras
    }
}

// ✅ Contexto: CRM
namespace CRM.Domain {
    public class Cliente {
        public ClienteId Id { get; }
        public List<Lead> Leads { get; }
        // Focado apenas em relacionamento
    }
}

// ✅ Contexto: Financeiro
namespace Financeiro.Domain {
    public class Cliente {
        public ClienteId Id { get; }
        public List<Fatura> Faturas { get; }
        // Focado apenas em pagamentos
    }
}
```

## 🗺️ Mapeamento de Contextos

### Exemplo: Sistema E-commerce

```
┌─────────────────────┐
│   E-commerce        │
│   - Cliente         │
│   - Produto         │
│   - Pedido          │
└─────────────────────┘
         │
         │ Integração
         │
┌─────────────────────┐
│   Estoque           │
│   - Produto         │
│   - Movimentacao    │
│   - Almoxarifado    │
└─────────────────────┘
         │
         │ Integração
         │
┌─────────────────────┐
│   Pagamento         │
│   - Transacao       │
│   - Gateway         │
│   - Reembolso       │
└─────────────────────┘
```

## 🔗 Integração entre Contextos

### 1. **Shared Kernel**
Contextos compartilham parte do modelo.

```csharp
// Shared Kernel
namespace Shared.Domain {
    public class ClienteId {
        public Guid Valor { get; }
    }
}

// E-commerce usa
namespace Ecommerce.Domain {
    using Shared.Domain;
    
    public class Cliente {
        public ClienteId Id { get; }  // Do Shared Kernel
    }
}
```

### 2. **Conformist**
Um contexto segue o modelo de outro (sem questionar).

```csharp
// Contexto: E-commerce (Conformist)
namespace Ecommerce.Domain {
    // Usa modelo de Estoque sem modificá-lo
    public class Produto {
        public Estoque.ProdutoId EstoqueId { get; }  // Referência externa
    }
}
```

### 3. **Anti-Corruption Layer (ACL)**
Camada que traduz entre contextos.

```csharp
// ACL entre E-commerce e Sistema Legado
public class SistemaLegadoAdapter {
    public ClienteEcommerce AdaptarClienteLegado(ClienteLegado legado) {
        // Traduz modelo legado para modelo do contexto
        return new ClienteEcommerce {
            Id = new ClienteId(legado.Codigo),
            Nome = legado.NomeCompleto,
            Email = new Email(legado.EmailPrincipal)
        };
    }
}
```

## 📊 Exemplo Completo

### Sistema Multi-Contexto

```csharp
// ===== CONTEXTO: E-COMMERCE =====
namespace Ecommerce.Domain {
    public class Cliente {
        public ClienteId Id { get; }
        public Email Email { get; }
        public List<Pedido> Pedidos { get; }
        
        public Pedido CriarPedido() {
            return new Pedido(this);
        }
    }
    
    public class Pedido {
        public PedidoId Id { get; }
        public ClienteId ClienteId { get; }
        public List<ItemPedido> Itens { get; }
    }
}

// ===== CONTEXTO: ESTOQUE =====
namespace Estoque.Domain {
    public class Produto {
        public ProdutoId Id { get; }
        public string SKU { get; }
        public int QuantidadeDisponivel { get; }
        
        public void Reservar(int quantidade) {
            if (QuantidadeDisponivel < quantidade) {
                throw new EstoqueInsuficienteException();
            }
            QuantidadeDisponivel -= quantidade;
        }
    }
}

// ===== CONTEXTO: PAGAMENTO =====
namespace Pagamento.Domain {
    public class Transacao {
        public TransacaoId Id { get; }
        public PedidoId PedidoId { get; }  // Referência ao contexto E-commerce
        public decimal Valor { get; }
        public StatusTransacao Status { get; }
        
        public void Processar() {
            // Lógica de pagamento
            Status = StatusTransacao.Processada;
        }
    }
}
```

## 🎯 Identificando Bounded Contexts

### Perguntas para Identificar

1. **Qual é o propósito?** (E-commerce, Estoque, Pagamento)
2. **Quem usa?** (Equipe de vendas, equipe de logística)
3. **Qual linguagem?** (Termos diferentes em cada contexto)
4. **Quais regras?** (Regras de negócio específicas)

### Exemplo: Identificação

```
Sistema de E-commerce:

1. E-commerce Context
   - Propósito: Vender produtos
   - Usuários: Clientes, Vendedores
   - Linguagem: Pedido, Carrinho, Cliente
   - Regras: Desconto, Frete, Cupons

2. Estoque Context
   - Propósito: Gerenciar estoque
   - Usuários: Almoxarife, Gerente
   - Linguagem: Produto, Movimentação, Reserva
   - Regras: Controle de entrada/saída

3. Pagamento Context
   - Propósito: Processar pagamentos
   - Usuários: Financeiro, Gateway
   - Linguagem: Transação, Reembolso, Gateway
   - Regras: Validação, Estorno
```

## ✅ Verificação

- [ ] Entendeu o que são Bounded Contexts
- [ ] Sabe por que são importantes
- [ ] Conhece padrões de integração
- [ ] Sabe identificar contextos
- [ ] Viu exemplos práticos
- [ ] Entendeu ACL (Anti-Corruption Layer)

---

[← Voltar: Domain Services](./04-domain-services.md) | [Próximo: CQRS e Eventos →](./06-cqrs-eventos.md)