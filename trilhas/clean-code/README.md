# 🎯 Clean Code - Princípios de Código Limpo

## 📖 Sobre

Esta trilha ensina os princípios de **Clean Code** (Código Limpo) de Robert C. Martin. Aprenda a escrever código que seja legível, manutenível e profissional.

## 🎯 Objetivos de Aprendizado

Ao final desta trilha, você será capaz de:

- Escrever código que outros desenvolvedores queiram ler
- Aplicar princípios de nomenclatura significativa
- Criar funções pequenas e focadas
- Evitar code smells comuns
- Refatorar código legacy de forma segura
- Seguir convenções e boas práticas da indústria

## 📋 Pré-requisitos

- C#/.NET básico (variáveis, métodos, classes)
- Git básico
- Experiência com algum projeto prático

## 📚 Módulos

### 📘 Fundamentos
1. **[00 - Introdução ao Clean Code](./00-introducao.md)** - O que é e por que importa
2. **[01 - Nomes Significativos](./01-nomes-significativos.md)** - Nomenclatura que comunica intenção

### 📗 Práticas de Código
3. **[02 - Funções](./02-funcoes.md)** - Funções pequenas e bem nomeadas
4. **[03 - Comentários](./03-comentarios.md)** - Quando e como comentar
5. **[04 - Formatação](./04-formatacao.md)** - Código visualmente organizado

### 📕 Avançado
6. **[05 - Objetos e Estruturas](./05-objetos-e-estruturas.md)** - Abstração e encapsulamento
7. **[06 - Tratamento de Erros](./06-tratamento-de-erros.md)** - Exceções e validações

## 🎓 Progressão Sugerida

Comece pelos fundamentos e evolua para práticas avançadas.

**Tempo estimado**: 12-15 horas

## ✅ Checklist de Progresso

- [ ] Módulo 00 - Introdução ao Clean Code
- [ ] Módulo 01 - Nomes Significativos
- [ ] Módulo 02 - Funções
- [ ] Módulo 03 - Comentários
- [ ] Módulo 04 - Formatação
- [ ] Módulo 05 - Objetos e Estruturas
- [ ] Módulo 06 - Tratamento de Erros

## 📖 O que é Clean Code?

> "Código limpo é aquele que foi escrito por alguém que se importa." - Michael Feathers

### Características de Código Limpo

- ✅ **Legível**: Qualquer desenvolvedor entende rapidamente
- ✅ **Manutenível**: Fácil de modificar e estender
- ✅ **Testável**: Estrutura permite testes automatizados
- ✅ **Expressivo**: Nomes revelam intenção
- ✅ **Simples**: Sem complexidade desnecessária
- ✅ **Consistente**: Segue padrões estabelecidos

### Benefícios

- **Redução de bugs**: Código claro tem menos erros
- **Velocidade de desenvolvimento**: Menos tempo debugando
- **Onboarding rápido**: Novos devs aprendem rapidamente
- **Manutenção barata**: Mudanças são seguras e rápidas
- **Escalabilidade**: Código cresce de forma organizada

## 🔧 Princípios Fundamentais

### 1. Nomes Significativos
- Variáveis, métodos e classes devem revelar intenção
- Evite abreviações obscuras
- Use nomes pronunciáveis

### 2. Funções Pequenas
- Uma função deve fazer apenas uma coisa
- Máximo 20 linhas por função
- Parâmetros limitados (máximo 3)

### 3. Boa Formatação
- Indentação consistente
- Espaçamento adequado
- Estrutura visual clara

### 4. Comentários Apenas Quando Necessário
- Código deve ser auto-explicativo
- Comentários explicam "porquê", não "o quê"
- Evite comentários obsoletos

### 5. Tratamento Adequado de Erros
- Use exceções apropriadas
- Não retorne códigos de erro
- Separe lógica de tratamento de erro

## 🎯 Exercícios Práticos

Cada módulo inclui:
- **Exemplos ruins** (❌) vs **Exemplos bons** (✅)
- **Exercícios de refatoração** passo a passo
- **Code smells** comuns e como evitá-los

### Exemplo de Refatoração

**❌ Código Ruim:**
```csharp
public class usr {
    public string n;
    public int a;
    public void sv() { /* ... */ }
}
```

**✅ Código Limpo:**
```csharp
public class Usuario {
    public string Nome { get; set; }
    public int Idade { get; set; }
    public void Salvar() { /* ... */ }
}
```

## 🛠️ Ferramentas

### Análise de Código
- **SonarQube**: Análise automática de qualidade
- **StyleCop**: Regras de estilo para C#
- **Roslyn Analyzers**: Análise estática avançada

### VS Code Extensions
- **C# Formatter**: Formatação automática
- **CodeMetrics**: Métricas de complexidade
- **Better Comments**: Comentarios organizados

## 📊 Métricas de Qualidade

### Complexidade Ciclomática
- **Boa**: ≤ 10
- **Aceitável**: 11-20
- **Ruim**: > 20

### Tamanho de Funções
- **Ideal**: 1-10 linhas
- **Aceitável**: 11-20 linhas
- **Refatorar**: > 20 linhas

### Cobertura de Testes
- **Mínimo**: 70%
- **Ideal**: 80-90%

## 🎯 Boas Práticas

### Nomenclatura
```csharp
// ✅ BOM
public class ClienteService
{
    public Cliente BuscarPorId(int id) { }
    public void AtualizarCliente(Cliente cliente) { }
}

// ❌ RUIM
public class cls
{
    public object Get(int i) { }
    public void Upd(object o) { }
}
```

### Funções
```csharp
// ✅ BOM - Uma responsabilidade
public bool ClientePodeComprarCredito(Cliente cliente, decimal valor)
{
    return cliente.Idade >= 18 &&
           cliente.ScoreCredito >= 600 &&
           valor <= cliente.LimiteCredito;
}

// ❌ RUIM - Múltiplas responsabilidades
public bool ValidarCompra(Cliente c, decimal v)
{
    if (c.Idade < 18) return false;
    if (c.ScoreCredito < 600) return false;
    if (v > c.LimiteCredito) return false;
    // Salvar no banco...
    // Enviar email...
    return true;
}
```

## 📚 Recursos

### Livros Essenciais
- **Clean Code** - Robert C. Martin
- **Clean Coder** - Robert C. Martin
- **Refactoring** - Martin Fowler

### Ferramentas Online
- [Refactoring Guru](https://refactoring.guru/)
- [SourceMaking](https://sourcemaking.com/)

## 🔗 Integração com Outras Trilhas

Clean Code é fundamental para:
- **C#/.NET**: Aplicação prática dos princípios
- **SOLID**: Base para princípios de design
- **Testes**: Código testável é código limpo
- **DDD**: Domínio expressivo e claro
- **Projeto Final**: Qualidade profissional

## 🚨 Code Smells Comuns

### 1. Nomes Ruins
```csharp
// ❌ Evite
int x; string str; var data;
public void Proc() { }

// ✅ Prefira
int quantidadeProdutos; string nomeCliente; var dataNascimento;
public void ProcessarPedido() { }
```

### 2. Funções Longas
```csharp
// ❌ Evite
public void ProcessarPedido(Pedido pedido) {
    // 50+ linhas de código...
    // Validação...
    // Cálculo...
    // Salvamento...
    // Notificação...
}

// ✅ Prefira
public void ProcessarPedido(Pedido pedido) {
    ValidarPedido(pedido);
    CalcularTotais(pedido);
    SalvarPedido(pedido);
    NotificarCliente(pedido);
}
```

### 3. Classes Grandes
```csharp
// ❌ Evite
public class PedidoService {
    // 1000+ linhas
    // Validação, cálculo, persistência, notificação...
}

// ✅ Prefira
public class ValidadorPedido { /* ... */ }
public class CalculadoraPedido { /* ... */ }
public class RepositorioPedido { /* ... */ }
public class NotificadorPedido { /* ... */ }
```

## ✅ Verificação de Qualidade

Use esta checklist em seus projetos:

- [ ] Nomes revelam intenção?
- [ ] Funções fazem apenas uma coisa?
- [ ] Classes têm responsabilidade única?
- [ ] Código é auto-documentado?
- [ ] Testes são fáceis de escrever?
- [ ] Mudanças são seguras de fazer?

---

**🎯 "Código é lido muito mais vezes do que escrito. Escreva para humanos, não para máquinas."**

[← Voltar ao índice principal](../../INDEX.md)