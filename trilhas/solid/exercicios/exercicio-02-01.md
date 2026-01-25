# Exercício 2.1 - Identificar Violações de OCP

## 📋 Objetivo

Identificar violações do Open/Closed Principle em código fornecido.

## 🎯 Tarefa

Analise o código abaixo e identifique onde o OCP está sendo violado. Explique por que cada caso é uma violação e como poderia ser corrigido.

### Código para Análise

```csharp
// Exemplo 1: Calculadora de Descontos
public class CalculadoraDesconto {
    public decimal CalcularDesconto(Cliente cliente, decimal valorCompra) {
        if (cliente.Tipo == "VIP") {
            return valorCompra * 0.1m; // 10% desconto
        }
        else if (cliente.Tipo == "Premium") {
            return valorCompra * 0.15m; // 15% desconto
        }
        else if (cliente.Tipo == "Gold") {
            return valorCompra * 0.2m; // 20% desconto
        }
        else {
            return 0; // Sem desconto
        }
    }
}

// Exemplo 2: Processador de Arquivos
public class ProcessadorArquivo {
    public void Processar(string tipoArquivo, string conteudo) {
        if (tipoArquivo == "XML") {
            // Processa XML
            Console.WriteLine("Processando XML...");
        }
        else if (tipoArquivo == "JSON") {
            // Processa JSON
            Console.WriteLine("Processando JSON...");
        }
        else if (tipoArquivo == "CSV") {
            // Processa CSV
            Console.WriteLine("Processando CSV...");
        }
        else {
            throw new Exception("Tipo de arquivo não suportado");
        }
    }
}

// Exemplo 3: Calculadora de Impostos
public class CalculadoraImposto {
    public decimal CalcularImposto(string pais, decimal valor) {
        if (pais == "BR") {
            return valor * 0.17m; // 17% ICMS
        }
        else if (pais == "US") {
            return valor * 0.08m; // 8% Sales Tax
        }
        else if (pais == "DE") {
            return valor * 0.19m; // 19% IVA
        }
        else {
            return 0;
        }
    }
}
```

## 📝 Sua Análise

Para cada exemplo, responda:

### Exemplo 1: CalculadoraDesconto

**1. Onde está a violação do OCP?**
```
Sua resposta aqui...
```

**2. Por que isso viola OCP?**
```
Sua resposta aqui...
```

**3. Como corrigir aplicando OCP?**
```
Sua resposta aqui...
```

### Exemplo 2: ProcessadorArquivo

**1. Onde está a violação do OCP?**
```
Sua resposta aqui...
```

**2. Por que isso viola OCP?**
```
Sua resposta aqui...
```

**3. Como corrigir aplicando OCP?**
```
Sua resposta aqui...
```

### Exemplo 3: CalculadoraImposto

**1. Onde está a violação do OCP?**
```
Sua resposta aqui...
```

**2. Por que isso viola OCP?**
```
Sua resposta aqui...
```

**3. Como corrigir aplicando OCP?**
```
Sua resposta aqui...
```

## 💡 Conceitos-Chave do OCP

Lembre-se dos conceitos fundamentais:

- **Aberto para extensão**: Podemos adicionar novas funcionalidades
- **Fechado para modificação**: Não devemos alterar código existente
- **Strategy Pattern**: Uma das formas mais comuns de aplicar OCP
- **Polimorfismo**: Interfaces e herança ajudam a implementar OCP

## ✅ Critérios de Avaliação

- [ ] Identificou corretamente as violações em todos os exemplos
- [ ] Explicou claramente por que cada caso viola OCP
- [ ] Propôs soluções viáveis usando padrões adequados
- [ ] Demonstrou compreensão dos conceitos de extensão vs modificação

## 🔍 Discussão

### Qual a diferença entre OCP e outros princípios?

OCP se concentra especificamente na **extensibilidade sem modificação**. Enquanto SRP fala sobre responsabilidades únicas e DIP sobre dependências, OCP trata especificamente de como adicionar novas funcionalidades ao sistema.

### Por que OCP é importante em sistemas grandes?

Em sistemas grandes, modificar código existente pode introduzir bugs e quebrar funcionalidades testadas. OCP permite que o sistema cresça através de extensões, mantendo a estabilidade do código existente.

---

[← Voltar aos exercícios](../README.md)