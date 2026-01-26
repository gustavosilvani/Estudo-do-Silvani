# 01 - Linguagem Ubíqua

## 📘 Nível Básico

### O que é Linguagem Ubíqua?

**Linguagem Ubíqua** (Ubiquitous Language) é uma linguagem comum e compartilhada entre desenvolvedores e especialistas do negócio, usada em todo o código, documentação e conversas.

## 🎯 Por que é Importante?

### Problema: Babel de Linguagens

```
Desenvolvedor: "Vou criar uma tabela User"
Negócio: "Mas o que é User? Nós temos Clientes e Funcionários"
Desenvolvedor: "Ah, então são duas tabelas?"
Negócio: "Não, são pessoas, mas com papéis diferentes"
Desenvolvedor: "Entendi... então uma tabela Person com Role?"
Negócio: "Não é bem assim..."
```

### Solução: Linguagem Compartilhada

```
Desenvolvedor: "Vou criar a entidade Cliente"
Negócio: "Perfeito! Cliente tem CPF e pode fazer Pedidos"
Desenvolvedor: "E Funcionário? É diferente?"
Negócio: "Sim, Funcionário tem Matrícula e pode ProcessarPedidos"
Desenvolvedor: "Entendi! São dois agregados diferentes"
```

## 📚 Regras da Linguagem Ubíqua

### 1. **Usar Termos do Negócio**

```csharp
// ❌ RUIM - Termos técnicos
public class User {
    public int Id { get; set; }
    public string Email { get; set; }
    public bool IsActive { get; set; }
}

// ✅ BOM - Termos do negócio
public class Cliente {
    public ClienteId Id { get; }
    public Email Email { get; }
    public bool EstaAtivo { get; }
}
```

### 2. **Evitar Jargão Técnico**

```csharp
// ❌ RUIM
public class UserDTO {
    public int UserId { get; set; }
}

// ✅ BOM
public class DadosCliente {
    public ClienteId Id { get; set; }
}
```

### 3. **Consistência em Todo Código**

```csharp
// ✅ BOM - Mesma linguagem em todo lugar
public class Cliente { }  // Domain
public class ClienteDto { }  // Application
public class ClienteController { }  // API
public interface IClienteRepository { }  // Infrastructure
```

### 4. **Documentação com Linguagem do Negócio**

```csharp
/// <summary>
/// Cliente representa uma pessoa que pode realizar pedidos no sistema.
/// Um Cliente deve ter CPF válido e email confirmado.
/// </summary>
public class Cliente {
    // ...
}
```

## 🎯 Exemplo Prático - E-commerce

### Domínio: Loja Online

**Termos do Negócio:**
- **Cliente**: Pessoa que compra produtos
- **Produto**: Item vendido na loja
- **Pedido**: Compra realizada por um Cliente
- **ItemPedido**: Produto específico em um Pedido
- **Carrinho**: Pedido em construção (rascunho)
- **Pagamento**: Processo de pagamento de um Pedido
- **Entrega**: Envio do Pedido ao Cliente

### Implementação

```csharp
// ✅ Linguagem Ubíqua aplicada
public class Cliente {
    public ClienteId Id { get; }
    public CPF Cpf { get; }
    public Email Email { get; }
    
    public Pedido CriarPedido() {
        // Termo do negócio: "Criar Pedido"
        return new Pedido(this);
    }
}

public class Pedido {
    private readonly List<ItemPedido> _itens;
    public PedidoId Id { get; }
    public Cliente Cliente { get; }
    public StatusPedido Status { get; private set; }
    
    public void AdicionarProduto(Produto produto, int quantidade) {
        // Termo do negócio: "Adicionar Produto"
        var item = new ItemPedido(produto, quantidade);
        _itens.Add(item);
    }
    
    public void Finalizar() {
        // Termo do negócio: "Finalizar Pedido"
        if (!_itens.Any()) {
            throw new PedidoVazioException();
        }
        Status = StatusPedido.Finalizado;
    }
}
```

## 🔄 Evolução da Linguagem

### Processo Iterativo

1. **Conversar** com especialistas do negócio
2. **Documentar** termos acordados
3. **Implementar** no código
4. **Revisar** quando surgem novos termos
5. **Refatorar** código para refletir linguagem atualizada

### Exemplo de Evolução

```
Iteração 1:
- Termo: "Pedido"
- Implementação: public class Pedido { }

Iteração 2:
- Novo termo: "Carrinho" (Pedido em construção)
- Refatoração: Separar Carrinho de Pedido

Iteração 3:
- Termo: "Orçamento" (Pedido não finalizado)
- Decisão: Usar "Carrinho" ou "Orçamento"?
- Acordo: "Carrinho" para B2C, "Orçamento" para B2B
```

## 📝 Glossário de Domínio

### Criando Glossário

```markdown
# Glossário - E-commerce

## Cliente
Pessoa física ou jurídica que realiza compras no sistema.
- Deve ter CPF/CNPJ válido
- Email confirmado
- Pode ter múltiplos endereços

## Pedido
Compra realizada por um Cliente.
- Contém um ou mais ItensPedido
- Tem status (Rascunho, Finalizado, Cancelado)
- Pode ter desconto aplicado

## Produto
Item vendido na loja.
- Tem SKU único
- Preço e estoque
- Pode estar ativo ou inativo
```

## ✅ Verificação

- [ ] Entendeu o que é Linguagem Ubíqua
- [ ] Sabe por que é importante
- [ ] Conhece as regras básicas
- [ ] Viu exemplo prático
- [ ] Entende processo de evolução
- [ ] Sabe criar glossário de domínio

---

[← Voltar: Introdução ao DDD](./00-introducao-ddd.md) | [Próximo: Entidades e Value Objects →](./02-entidades-value-objects.md)