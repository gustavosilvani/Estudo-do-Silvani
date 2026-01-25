# Exercício 1.3 - Aplicar SRP em Código Real

## 📋 Objetivo

Aplicar o Single Responsibility Principle em um cenário real de processamento de pedidos.

## 🎯 Tarefa

Refatore a classe `ProcessadorPedido` que viola o SRP. Esta classe está fazendo muitas coisas diferentes no processamento de um pedido.

### Cenário

Você trabalha em um e-commerce e a classe `ProcessadorPedido` está crescendo demais. Ela valida o pedido, calcula frete, processa pagamento, envia notificações e salva no banco. Isso viola claramente o SRP.

### Código Original (❌ Violação do SRP)

```csharp
public class ProcessadorPedido {
    public bool Processar(Pedido pedido) {
        // Validação do pedido
        if (string.IsNullOrEmpty(pedido.ClienteNome) || pedido.Itens.Count == 0) {
            throw new Exception("Pedido inválido");
        }

        // Cálculo do frete
        decimal frete = 0;
        foreach (var item in pedido.Itens) {
            frete += item.Peso * 0.5m; // R$ 0,50 por kg
        }
        pedido.ValorFrete = frete;

        // Processamento do pagamento
        bool pagamentoAprovado = ProcessarPagamento(pedido.ValorTotal + frete);
        if (!pagamentoAprovado) {
            return false;
        }

        // Salvamento no banco
        SalvarPedido(pedido);

        // Envio de notificações
        EnviarEmailConfirmacao(pedido);
        EnviarSmsConfirmacao(pedido);

        return true;
    }

    private bool ProcessarPagamento(decimal valor) {
        // Simulação de processamento de pagamento
        Console.WriteLine($"Processando pagamento de R$ {valor:F2}");
        return true; // Sempre aprova para exemplo
    }

    private void SalvarPedido(Pedido pedido) {
        // Simulação de salvamento
        Console.WriteLine($"Salvando pedido {pedido.Id} no banco");
    }

    private void EnviarEmailConfirmacao(Pedido pedido) {
        Console.WriteLine($"Enviando email para {pedido.ClienteEmail}");
    }

    private void EnviarSmsConfirmacao(Pedido pedido) {
        Console.WriteLine($"Enviando SMS para {pedido.ClienteTelefone}");
    }
}
```

### Sua Tarefa

Refatore este código aplicando SRP. Crie classes separadas para cada responsabilidade:

1. **Validação** - Validar dados do pedido
2. **Cálculo** - Calcular frete e totais
3. **Pagamento** - Processar pagamentos
4. **Persistência** - Salvar dados
5. **Notificação** - Enviar comunicações

### Refatoração Esperada (✅ Aplicando SRP)

```csharp
// 💡 Implemente aqui a refatoração completa

// Dica: Crie classes como:
// - IValidadorPedido
// - ICalculadoraFrete
// - IProcessadorPagamento
// - IRepositorioPedido
// - INotificacaoService

// Depois refatore ProcessadorPedido para usar essas abstrações
```

## 📝 Entrega

1. Crie as interfaces necessárias
2. Implemente as classes concretas
3. Refatore `ProcessadorPedido` para usar injeção de dependência
4. Mantenha a mesma API pública (assinatura do método `Processar`)

## 💡 Dicas

- Comece criando as interfaces (abstrações)
- Implemente classes simples para cada responsabilidade
- Use construtor injection no `ProcessadorPedido`
- Cada classe deve ter um nome que deixe clara sua responsabilidade

## ✅ Critérios de Avaliação

- [ ] Pelo menos 5 responsabilidades separadas identificadas
- [ ] Interfaces criadas para abstrair dependências
- [ ] Injeção de dependência aplicada corretamente
- [ ] Cada classe tem apenas uma responsabilidade
- [ ] API pública mantida compatível
- [ ] Código está bem testável

## 🧪 Teste sua Solução

```csharp
// Código de teste
var processador = new ProcessadorPedido(
    new ValidadorPedido(),
    new CalculadoraFrete(),
    new ProcessadorPagamentoCartao(),
    new RepositorioPedidoBD(),
    new NotificacaoService()
);

var pedido = new Pedido {
    Id = "PED001",
    ClienteNome = "João Silva",
    ClienteEmail = "joao@email.com",
    Itens = new List<ItemPedido> { /* ... */ }
};

bool sucesso = processador.Processar(pedido);
Console.WriteLine($"Processamento: {(sucesso ? "Sucesso" : "Falhou")}");
```

---

## 🔍 Discussão

### Quais os benefícios dessa refatoração?

1. **Separação de responsabilidades** claras
2. **Testabilidade** individual de cada componente
3. **Reutilização** de componentes em outros contextos
4. **Manutenibilidade** - mudanças isoladas
5. **Flexibilidade** - fácil trocar implementações

### Como isso se relaciona com outros princípios SOLID?

Esta refatoração prepara o terreno para aplicar OCP (podemos adicionar novas formas de cálculo sem modificar código), LSP (todas as implementações são substituíveis), ISP (interfaces específicas) e DIP (dependências invertidas).

---

[← Voltar aos exercícios](../README.md)