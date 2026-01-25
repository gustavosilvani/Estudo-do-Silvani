# Exercício 2.2 - Refatorar usando Strategy Pattern

## 📋 Objetivo

Aplicar o Open/Closed Principle usando o Strategy Pattern para refatorar código que viola OCP.

## 🎯 Tarefa

Refatore a classe `CalculadoraFrete` que viola OCP. Atualmente, ela precisa ser modificada toda vez que um novo tipo de frete é adicionado.

### Código Original (❌ Violação do OCP)

```csharp
public class CalculadoraFrete {
    public decimal CalcularFrete(string tipoFrete, Pedido pedido) {
        if (tipoFrete == "Padrao") {
            return pedido.PesoTotal * 0.5m; // R$ 0,50 por kg
        }
        else if (tipoFrete == "Expresso") {
            return pedido.PesoTotal * 1.0m + 10.0m; // R$ 1,00 por kg + taxa fixa
        }
        else if (tipoFrete == "Econômico") {
            return Math.Max(pedido.PesoTotal * 0.3m, 5.0m); // R$ 0,30 por kg, mínimo R$ 5,00
        }
        else {
            throw new Exception("Tipo de frete não suportado");
        }
    }
}
```

### Sua Tarefa

1. **Crie uma interface** `IEstrategiaFrete` que defina o contrato para cálculo de frete
2. **Implemente classes concretas** para cada tipo de frete (Padrão, Expresso, Econômico)
3. **Refatore `CalculadoraFrete`** para aceitar uma estratégia via construtor
4. **Adicione um novo tipo** de frete sem modificar código existente

### Refatoração Esperada (✅ Aplicando OCP)

```csharp
// 💡 Implemente aqui a refatoração usando Strategy Pattern

// 1. Crie a interface IEstrategiaFrete
// 2. Implemente FretePadrao, FreteExpresso, FreteEconomico
// 3. Refatore CalculadoraFrete para usar injeção de dependência
// 4. Adicione FreteGratis como exemplo de extensão
```

## 📝 Entrega

Implemente a solução completa no espaço acima. Inclua:

1. Interface `IEstrategiaFrete`
2. Classes concretas para cada estratégia
3. `CalculadoraFrete` refatorada
4. Exemplo de uso com diferentes estratégias
5. Demonstração de extensão (adicionar `FreteGratis`)

## 💡 Dicas

- Use propriedades auto-implementadas para tipo de frete
- Cada estratégia deve encapsular sua própria lógica de cálculo
- A `CalculadoraFrete` deve apenas delegar para a estratégia
- Teste com diferentes tipos de pedido

## ✅ Critérios de Avaliação

- [ ] Interface `IEstrategiaFrete` criada corretamente
- [ ] Pelo menos 3 estratégias concretas implementadas
- [ ] `CalculadoraFrete` refatorada para OCP
- [ ] Novo tipo de frete adicionado sem modificar código existente
- [ ] Código está testável e bem estruturado

## 🧪 Teste sua Solução

```csharp
// Exemplo de uso
var calculadoraPadrao = new CalculadoraFrete(new FretePadrao());
var calculadoraExpresso = new CalculadoraFrete(new FreteExpresso());
var calculadoraGratis = new CalculadoraFrete(new FreteGratis());

var pedido = new Pedido { PesoTotal = 5.0m, ValorTotal = 150.0m };

Console.WriteLine($"Frete Padrão: R$ {calculadoraPadrao.CalcularFrete(pedido):F2}");
Console.WriteLine($"Frete Expresso: R$ {calculadoraExpresso.CalcularFrete(pedido):F2}");
Console.WriteLine($"Frete Grátis: R$ {calculadoraGratis.CalcularFrete(pedido):F2}");
```

**Saída esperada:**
```
Frete Padrão: R$ 2,50
Frete Expresso: R$ 15,00
Frete Grátis: R$ 0,00
```

## 🔍 Discussão

### Por que Strategy Pattern implementa OCP?

O Strategy Pattern permite que o comportamento seja **extendido através de novas classes** (estratégias), ao invés de **modificar classes existentes**. Isso significa:

- **Aberto para extensão**: Novas estratégias podem ser adicionadas
- **Fechado para modificação**: Código existente não precisa mudar
- **Polimorfismo**: Todas as estratégias são intercambiáveis

### Quando usar Strategy Pattern?

Use Strategy quando:
- Você tem uma família de algoritmos
- Precisa trocar algoritmos em tempo de execução
- Quer evitar condicionais complexas
- O algoritmo é independente do contexto que o usa

### Benefícios dessa refatoração

1. **Extensibilidade**: Novos tipos de frete sem modificar código
2. **Testabilidade**: Cada estratégia testada isoladamente
3. **Manutenibilidade**: Lógica de cálculo centralizada
4. **Flexibilidade**: Estratégias podem ser trocadas dinamicamente

---

[← Voltar aos exercícios](../README.md)