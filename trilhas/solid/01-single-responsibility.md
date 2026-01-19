# 01 - Single Responsibility Principle (SRP)

## 📘 Nível Básico

### O que é o Single Responsibility Principle?

O **Single Responsibility Principle (SRP)** ou **Princípio da Responsabilidade Única** afirma que:

> **Uma classe deve ter apenas uma razão para mudar.**

Em outras palavras, uma classe deve ter apenas uma responsabilidade ou motivo para ser modificada.

### Definição Formal

Robert C. Martin define responsabilidade como "uma razão para mudar". Se uma classe tem mais de uma razão para mudar, ela tem mais de uma responsabilidade e viola o SRP.

### Contexto Histórico

O Princípio da Responsabilidade Única foi formalizado por **Robert C. Martin (Uncle Bob)** no início dos anos 2000, como parte dos princípios SOLID. Embora o conceito de separação de responsabilidades já existisse na programação estruturada e modular, Martin o adaptou e refinou especificamente para programação orientada a objetos. O SRP é considerado a base dos outros princípios SOLID, pois sem responsabilidades bem definidas, é difícil aplicar os demais princípios efetivamente.

### Exemplo Básico: Violação do SRP

```csharp
// ❌ VIOLAÇÃO: A classe tem múltiplas responsabilidades
public class Usuario
{
    private string nome;
    private string email;

    // Responsabilidade 1: Gerenciar dados do usuário
    public Usuario(string nome, string email)
    {
        this.nome = nome;
        this.email = email;
    }

    // Responsabilidade 2: Validar email
    public bool ValidarEmail()
    {
        return email.Contains("@");
    }

    // Responsabilidade 3: Salvar no banco de dados
    public void Salvar()
    {
        // Código para salvar no banco
        Console.WriteLine($"Salvando {nome} no banco de dados...");
    }

    // Responsabilidade 4: Enviar email
    public void EnviarEmail(string mensagem)
    {
        Console.WriteLine($"Enviando email para {email}: {mensagem}");
    }
}




```

**Problemas:**
- Se a lógica de validação mudar, precisamos modificar `Usuario`
- Se a forma de salvar mudar, precisamos modificar `Usuario`
- Se a forma de enviar email mudar, precisamos modificar `Usuario`
- A classe tem **múltiplas razões para mudar**

### Exemplo Básico: Aplicando SRP

```csharp
// ✅ CORRETO: Cada classe tem uma única responsabilidade

// Responsabilidade: Representar dados do usuário
public class Usuario
{
    public string Nome { get; set; }
    public string Email { get; set; }

    public Usuario(string nome, string email) {
        Nome = nome;
        Email = email;
    }
}

// Responsabilidade: Validar emails
public class ValidadorEmail
{
    public bool Validar(string email) {
        return email.Contains("@") && email.Contains(".");
    }
}

// Responsabilidade: Persistir usuários
public class RepositorioUsuario
{
    public Salvar(Usuario usuario) {
        Console.WriteLine($"Salvando {usuario.Nome} no banco de dados...");
    }
}

// Responsabilidade: Enviar emails
public class ServicoEmail
{
    public Enviar(string email, string mensagem) {
        Console.WriteLine($"Enviando email para {email}: {mensagem}";
    }
}




```

**Benefícios:**
- Cada classe tem uma única responsabilidade
- Mudanças em validação não afetam persistência
- Mudanças em email não afetam dados do usuário
- Código mais fácil de entender e manter

## 📗 Nível Intermediário

### Identificando Responsabilidades

Como identificar se uma classe tem múltiplas responsabilidades?

**Perguntas a fazer:**

1. **Quantas razões essa classe tem para mudar?**
   - Se mais de uma, provavelmente viola SRP

2. **Posso descrever o que essa classe faz em uma frase?**
   - Se precisar de "e" ou "ou", pode ter múltiplas responsabilidades

3. **Mudanças em uma parte afetam outras partes?**
   - Se sim, pode indicar responsabilidades acopladas

### Casos de Uso Comuns

#### 1. Separação de Dados e Lógica de Negócio

```csharp
// ❌ Antes
public class Pedido
{
    List<Item> private itens;
    private double total;

    public double CalcularTotal() {
        // Lógica de cálculo
        return itens.Sum(item => item.Preco);
    }

    public AplicarDesconto(double percentual) {
        // Lógica de desconto
        total = total * (1 - percentual / 100);
    }

    public Salvar() {
        // Lógica de persistência
        // ...
    }
}

// ✅ Depois
public class Pedido
{
    List<Item> public Itens { get; set; }

    public Pedido(List<Item> itens) {
        Itens = itens;
    }
}

public class CalculadoraPedido
{
    public double CalcularTotal(Pedido pedido) {
        return pedido.Itens.Sum(item => item.Preco);
    }

    public double AplicarDesconto(double total, double percentual) {
        return total * (1 - percentual / 100);
    }
}

public class RepositorioPedido
{
    public Salvar(Pedido pedido) {
        // Lógica de persistência
    }
}




```

#### 2. Separação de Apresentação e Lógica

```csharp
// ❌ Antes
public class RelatorioVendas
{
    List<Venda> public void GerarDados() {
        // Busca dados
        return List<Venda> new();
    }

    public string FormatarHTML() {
        // Formata em HTML
        return "<html>...</html>";
    }

    public EnviarPorEmail() {
        // Envia email
    }
}

// ✅ Depois
public class ServicoVendas
{
    List<Venda> public void ObterVendas() {
        return List<Venda> new();
    }
}

public class FormatadorRelatorio
{
    public string FormatarHTML(List<Venda> vendas) {
        return "<html>...</html>";
    }
}

public class EnviadorRelatorio
{
    public Enviar(string relatorio) {
        // Envia email
    }
}




```

### Benefícios Práticos

1. **Manutenibilidade**: Mudanças são localizadas
2. **Testabilidade**: Classes pequenas são mais fáceis de testar
3. **Reutilização**: Classes focadas são mais reutilizáveis
4. **Compreensão**: Código mais fácil de entender
5. **Colaboração**: Menos conflitos em equipes

### Quando Aplicar SRP

Aplique SRP quando:

- Classe está ficando grande e complexa
- Mudanças em uma parte afetam outras partes
- Dificuldade para testar a classe
- Múltiplos desenvolvedores modificam a mesma classe frequentemente

## 📕 Nível Avançado

### SRP em Diferentes Níveis

SRP não se aplica apenas a classes - pode ser aplicado em diferentes níveis:

#### Nível de Método

```csharp
// ❌ Método com múltiplas responsabilidades
public ProcessarPedido(Pedido pedido) {
    // Validar
    if (pedido.Itens.Count == 0) 
        throw new Exception("Pedido vazio");
    
    // Calcular
    double total = pedido.Itens.Sum(i => i.Preco);
    
    // Salvar
    database.Save(pedido);
    
    // Notificar
    emailService.Send(pedido.Cliente.Email, "Pedido processado");
}

// ✅ Métodos com responsabilidades únicas
public ValidarPedido(Pedido pedido) {
    if (pedido.Itens.Count == 0) 
        throw new Exception("Pedido vazio");
}

public double CalcularTotal(Pedido pedido) {
    return pedido.Itens.Sum(i => i.Preco);
}

public ProcessarPedido(Pedido pedido) {
    ValidarPedido(pedido);
    double total = CalcularTotal(pedido);
    repositorio.Salvar(pedido);
    notificador.Notificar(pedido.Cliente);
}




```

#### Nível de Módulo/Pacote

```
// Estrutura de pastas seguindo SRP
src/
├── Domain/          // Responsabilidade: Modelos de domínio
├── Services/        // Responsabilidade: Lógica de negócio
├── Repositories/    // Responsabilidade: Acesso a dados
├── Controllers/     // Responsabilidade: HTTP/API
└── Utils/           // Responsabilidade: Utilitários
```

### SRP e Coesão

SRP está diretamente relacionado ao conceito de **coesão**:

- **Alta coesão**: Elementos de uma classe trabalham juntos para um propósito comum
- **Baixa coesão**: Elementos não têm relação clara

SRP promove alta coesão ao garantir que todos os elementos de uma classe servem a uma única responsabilidade.

### SRP e Acoplamento

SRP também ajuda a reduzir **acoplamento**:

- Classes com responsabilidades únicas têm menos dependências
- Mudanças são isoladas
- Testes são mais simples (menos mocks necessários)

### Armadilhas Comuns

#### 1. Over-engineering

```csharp
// ❌ Não faça isso - muito granular
public class NomeUsuario
{
    private string valor;

    public NomeUsuario(string valor) {
        valor = valor;
    }

    public string GetValor() => valor;
}

public class EmailUsuario
{
    private string valor;

    public EmailUsuario(string valor) {
        valor = valor;
    }

    public string GetValor() => valor;
}

// ✅ Melhor - responsabilidades relacionadas podem estar juntas
public class Usuario
{
    public string Nome { get; set; }
    public string Email { get; set; }

    public Usuario(string nome, string email) {
        Nome = nome;
        Email = email;
    }
}




```

**Regra**: Se responsabilidades estão intimamente relacionadas e sempre mudam juntas, podem estar na mesma classe.

#### 2. Confundir Responsabilidade com Funcionalidade

Uma classe pode ter múltiplos métodos e ainda ter uma única responsabilidade:

```csharp
// ✅ OK: Todos os métodos servem à responsabilidade de gerenciar usuários
public class GerenciadorUsuario
{
    public Usuario void Criar(string nome, string email) { }
    public Atualizar(Usuario usuario) { }
    public Deletar(string id) { }
    public Usuario void Buscar(string id) { }
}




```

### SRP e Outros Princípios SOLID

- **SRP → OCP**: Classes com responsabilidade única são mais fáceis de estender
- **SRP → DIP**: Responsabilidades isoladas facilitam inversão de dependências
- **SRP → ISP**: Interfaces menores são mais fáceis quando classes são focadas

## ⚠️ Armadilhas Comuns

1. **Criar classes muito pequenas**: Nem tudo precisa ser uma classe separada
2. **Ignorar contexto**: Responsabilidades relacionadas podem estar juntas
3. **Aplicar cegamente**: Use bom senso, não dogma
4. **Confundir com "uma classe, um método"**: Uma classe pode ter vários métodos relacionados

## ✅ Checkpoint

### Auto-avaliação

- [ ] Entendo o que é Single Responsibility Principle
- [ ] Consigo identificar violações do SRP
- [ ] Sei como refatorar código para seguir SRP
- [ ] Entendo quando aplicar SRP e quando não aplicar
- [ ] Compreendo a relação entre SRP e coesão

### Exercícios

Pratique com os exercícios do módulo em [exercicios/](./exercicios/README.md).

## 🔗 Próximos Passos

- [Próximo: Open/Closed Principle →](./02-open-closed.md)
- [Voltar ao índice da trilha](./README.md)

---

[← Voltar ao índice principal](../../INDEX.md)
