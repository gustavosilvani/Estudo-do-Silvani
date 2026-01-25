# 00 - Introdução ao Clean Code

## 📖 O que é Clean Code?

> "Código limpo é código que foi escrito por alguém que se importa." - Michael Feathers

Clean Code não é apenas sobre fazer o código funcionar. É sobre escrever código que seja **legível, manutenível e profissional**.

## 🎯 Por que Clean Code importa?

### Cenário Real

Imagine você recebe um projeto legado com 1000+ linhas em uma única classe. Nomes como `x`, `y`, `z`. Funções de 100 linhas. Sem comentários. Sem testes.

**Quanto tempo leva para:**
- Entender o que o código faz?
- Encontrar um bug?
- Adicionar uma nova feature?
- Refatorar com segurança?

### Benefícios do Clean Code

- **🚀 Produtividade**: Menos tempo debugando, mais tempo criando
- **🐛 Menos Bugs**: Código claro tem menos erros
- **👥 Colaboração**: Equipe trabalha mais eficientemente
- **🔄 Manutenibilidade**: Mudanças são seguras e rápidas
- **📈 Escalabilidade**: Código cresce de forma organizada

## 📋 Princípios Fundamentais

### 1. **Legibilidade** - Código que se lê como prosa
```csharp
// ❌ Difícil de entender
public void prc(int x, int y) {
    var z = x + y * 2;
    if (z > 100) {
        // código...
    }
}

// ✅ Fácil de entender
public void CalcularDescontoClienteVip(int valorCompra, int quantidadeItens) {
    var descontoTotal = valorCompra + (quantidadeItens * 2);
    if (descontoTotal > 100) {
        AplicarDescontoMaximo();
    }
}
```

### 2. **Intenção Clara** - Nomes revelam propósito
```csharp
// ❌ O que significa 'd'? Dias? Dinheiro? Distância?
public decimal calc(decimal v, int d) {
    return v * (1 - d / 100m);
}

// ✅ Intenção clara
public decimal CalcularValorComDesconto(decimal valorOriginal, int percentualDesconto) {
    return valorOriginal * (1 - percentualDesconto / 100m);
}
```

### 3. **Simplicidade** - Menos é mais
```csharp
// ❌ Complexidade desnecessária
public bool ValidarCliente(Cliente cliente) {
    if (cliente != null) {
        if (!string.IsNullOrEmpty(cliente.Nome)) {
            if (cliente.Idade >= 18) {
                if (cliente.Email.Contains("@")) {
                    return true;
                }
            }
        }
    }
    return false;
}

// ✅ Simples e direto
public bool ClienteEhValido(Cliente cliente) {
    return cliente != null &&
           !string.IsNullOrEmpty(cliente.Nome) &&
           cliente.Idade >= 18 &&
           cliente.Email.Contains("@");
}
```

## 🎯 Os 4 Pilares do Clean Code

### 1. **Nomes Significativos**
- Variáveis, métodos e classes devem revelar intenção
- Evite abreviações obscuras
- Use nomes pronunciáveis

### 2. **Funções Pequenas**
- Uma função deve fazer apenas uma coisa
- Máximo 20 linhas por função
- Parâmetros limitados (máximo 3)

### 3. **Boa Formatação**
- Indentação consistente
- Espaçamento adequado
- Estrutura visual clara

### 4. **Tratamento Adequado de Erros**
- Use exceções apropriadas
- Não retorne códigos de erro
- Valide entrada no início das funções

## 🧪 Code Smells - Sinais de Problema

### 1. **Nomes Ruins**
```csharp
// ❌ Não revela intenção
int d; // dias? dinheiro? distância?
string str; // que string?
var data; // data de quê?

// ✅ Nomes claros
int diasTrabalho;
string nomeCliente;
var dataNascimento;
```

### 2. **Funções Longas**
```csharp
// ❌ Função faz tudo
public void ProcessarPedido(Pedido pedido) {
    // Validação (10 linhas)
    // Cálculo (15 linhas)
    // Salvamento (20 linhas)
    // Notificação (15 linhas)
    // TOTAL: 60 linhas!
}

// ✅ Funções focadas
public void ProcessarPedido(Pedido pedido) {
    ValidarPedido(pedido);
    CalcularTotais(pedido);
    SalvarPedido(pedido);
    NotificarCliente(pedido);
}
```

### 3. **Classes Grandes**
```csharp
// ❌ Classe faz tudo
public class SistemaVendas {
    // Validação de clientes (50 linhas)
    // Cálculos de preço (100 linhas)
    // Persistência (150 linhas)
    // Relatórios (80 linhas)
    // TOTAL: 380 linhas!
}

// ✅ Classes especializadas
public class ValidadorCliente { /* ... */ }
public class CalculadoraPreco { /* ... */ }
public class RepositorioVendas { /* ... */ }
public class GeradorRelatorios { /* ... */ }
```

## 🚀 Refatoração Segura

### Processo de 4 Passos

1. **Escreva Testes** - Garanta que o código funciona
2. **Refatore** - Melhore o código mantendo funcionalidade
3. **Execute Testes** - Confirme que nada quebrou
4. **Commit** - Salve as melhorias

### Exemplo Prático

**Código Original:**
```csharp
public class Calc {
    public int c(int[] arr) {
        int s = 0;
        for(int i = 0; i < arr.Length; i++) {
            s += arr[i];
        }
        return s;
    }
}
```

**Passo 1 - Teste:**
```csharp
[Test]
public void CalcularSoma_ArrayComValores_RetornaSomaCorreta() {
    var calc = new Calc();
    var resultado = calc.c(new[] { 1, 2, 3 });
    Assert.Equal(6, resultado);
}
```

**Passo 2 - Refatoração:**
```csharp
public class Calculadora {
    public int CalcularSoma(int[] numeros) {
        int soma = 0;
        foreach (int numero in numeros) {
            soma += numero;
        }
        return soma;
    }
}
```

**Passo 3 - Atualizar Teste:**
```csharp
[Test]
public void CalcularSoma_ArrayComValores_RetornaSomaCorreta() {
    var calculadora = new Calculadora();
    var resultado = calculadora.CalcularSoma(new[] { 1, 2, 3 });
    Assert.Equal(6, resultado);
}
```

## 💻 Ferramentas para Clean Code

### Análise Estática
- **SonarQube**: Detecta code smells automaticamente
- **StyleCop**: Regras de estilo para C#
- **Roslyn Analyzers**: Análise avançada

### VS Code Extensions
- **C# Formatter**: Formatação automática
- **CodeMetrics**: Complexidade ciclomática
- **Better Comments**: Organização de comentários

## 🎯 Exercício Prático

### Refatore este código ruim:

```csharp
// ❌ Código para refatorar
public class usr_svc {
    public void prc_usr(string n, int a, string e) {
        if (string.IsNullOrEmpty(n)) throw new Exception("inv");
        if (a < 18) throw new Exception("jvn");
        if (!e.Contains("@")) throw new Exception("eml inv");

        // salvar no "banco"
        Console.WriteLine($"usr {n} slv");
    }
}
```

### Para código limpo:

```csharp
// ✅ Código refatorado
public class UsuarioService {
    public void ProcessarUsuario(string nome, int idade, string email) {
        ValidarDadosUsuario(nome, idade, email);
        SalvarUsuario(nome, idade, email);
        EnviarEmailBoasVindas(email);
    }

    private void ValidarDadosUsuario(string nome, int idade, string email) {
        if (string.IsNullOrEmpty(nome)) {
            throw new ArgumentException("Nome é obrigatório");
        }

        if (idade < 18) {
            throw new ArgumentException("Usuário deve ser maior de idade");
        }

        if (!email.Contains("@")) {
            throw new ArgumentException("Email inválido");
        }
    }

    private void SalvarUsuario(string nome, int idade, string email) {
        // Simulação de salvamento
        Console.WriteLine($"Usuário {nome} salvo com sucesso");
    }

    private void EnviarEmailBoasVindas(string email) {
        Console.WriteLine($"Email de boas-vindas enviado para {email}");
    }
}
```

## ✅ Checklist de Clean Code

Use esta lista em seus projetos:

- [ ] **Nomes** revelam intenção clara?
- [ ] **Funções** fazem apenas uma coisa?
- [ ] **Classes** têm responsabilidade única?
- [ ] **Comentários** explicam o "porquê", não o "o quê"?
- [ ] **Testes** são fáceis de escrever?
- [ ] **Mudanças** são seguras de fazer?

## 🎯 Próximos Passos

No próximo módulo, vamos focar em **Nomes Significativos** - a base de todo código limpo!

---

[← Voltar à trilha](../README.md) | [Próximo: Nomes Significativos →](./01-nomes-significativos.md)