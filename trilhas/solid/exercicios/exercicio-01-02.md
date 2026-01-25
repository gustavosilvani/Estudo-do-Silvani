# Exercício 1.2 - Refatorar Classe com Múltiplas Responsabilidades

## 📋 Objetivo

Refatorar uma classe que viola o Single Responsibility Principle aplicando SRP.

## 🎯 Tarefa

A classe `RelatorioService` abaixo viola o SRP porque tem múltiplas responsabilidades. Refatore o código aplicando o princípio da responsabilidade única.

### Código Original (❌ Violação do SRP)

```csharp
public class RelatorioService {
    public void GerarRelatorioVendas(List<Venda> vendas) {
        // Responsabilidade 1: Calcular totais
        decimal total = 0;
        foreach (var venda in vendas) {
            total += venda.Valor;
        }

        // Responsabilidade 2: Formatar dados
        var dadosFormatados = new List<string>();
        foreach (var venda in vendas) {
            dadosFormatados.Add($"{venda.Data:dd/MM/yyyy} - {venda.Produto} - R$ {venda.Valor:F2}");
        }

        // Responsabilidade 3: Salvar em arquivo
        File.WriteAllLines("relatorio.txt", dadosFormatados);

        // Responsabilidade 4: Enviar por email
        EnviarEmail("relatorio@empresa.com", "Relatório de Vendas", "relatorio.txt");
    }

    private void EnviarEmail(string destinatario, string assunto, string arquivoAnexo) {
        // Código de envio de email
        Console.WriteLine($"Enviando email para {destinatario} com anexo {arquivoAnexo}");
    }
}
```

### Sua Tarefa

1. **Identifique as responsabilidades** na classe `RelatorioService`
2. **Crie classes separadas** para cada responsabilidade
3. **Aplique injeção de dependência** para conectar as classes
4. **Mantenha a mesma funcionalidade** do código original

### Refatoração Esperada (✅ Aplicando SRP)

```csharp
// 💡 Implemente aqui a refatoração seguindo SRP

// Dica: Crie classes separadas como:
// - CalculadoraTotais (responsável por cálculos)
// - FormatadorDados (responsável por formatação)
// - EscritorArquivo (responsável por salvar)
// - ServicoEmail (responsável por envio)

// Depois injete essas dependências no RelatorioService
```

## 📝 Entrega

Implemente a refatoração no espaço indicado acima. Cada classe deve ter apenas uma responsabilidade bem definida.

## 💡 Dicas

- Use interfaces para abstrair as dependências
- Aplique Dependency Inversion Principle junto com SRP
- Cada método deve ter uma única responsabilidade
- Teste cada classe isoladamente

## ✅ Critérios de Avaliação

- [ ] Cada classe tem apenas uma responsabilidade
- [ ] Interfaces foram criadas para abstrair dependências
- [ ] Injeção de dependência foi aplicada
- [ ] Funcionalidade original foi mantida
- [ ] Código está bem estruturado e legível

---

## 🔍 Discussão

### Por que essa refatoração melhora o código?

1. **Testabilidade**: Cada classe pode ser testada isoladamente
2. **Manutenibilidade**: Mudanças em uma responsabilidade não afetam outras
3. **Reutilização**: Classes podem ser reutilizadas em outros contextos
4. **Legibilidade**: Código fica mais claro e fácil de entender

### Qual o benefício de aplicar DIP junto com SRP?

Aplicar DIP (Dependency Inversion Principle) junto com SRP permite que as classes de alto nível (como `RelatorioService`) não dependam de implementações concretas, apenas de abstrações (interfaces). Isso torna o código ainda mais flexível e testável.

---

[← Voltar aos exercícios](../README.md)