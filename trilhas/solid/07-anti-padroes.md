# 07 - Anti-padrões e Violações Comuns

## 📖 Visão Geral

Neste módulo, vamos identificar os anti-padrões mais comuns que violam os princípios SOLID e aprender como evitá-los e corrigi-los.

## 📘 Nível Básico: Identificando Violações

### 1. God Class / God Object

**Problema**: Uma classe que faz tudo.

```csharp
// ❌ ANTI-PADRÃO: God Class
public class Sistema {
  // Gerencia usuários
  criarUsuariostring nome, string email { }
  atualizarUsuariostring id, any dados { }
  deletarUsuariostring id { }
  
  // Gerencia pedidos
  criarPedidostring usuarioId, List itens<Item> { }
  processarPedidostring pedidoId { }
  cancelarPedidostring pedidoId { }
  
  // Gerencia pagamentos
  processarPagamentostring pedidoId, double valor { }
  reembolsarstring pedidoId { }
  
  // Envia emails
  enviarEmailstring destino, string assunto { }
  
  // Salva no banco
  salvarNoBancostring tabela, any dados { }
  
  // Gera relatórios
  gerarRelatoriostring tipo { }
}

// ✅ SOLUÇÃO: Separar responsabilidades
public class GerenciadorUsuario {
  Usuario criarstring nome, string email { }
  atualizarstring id, any dados { }
  deletarstring id { }
}

public class GerenciadorPedido {
  Pedido criarstring usuarioId, List itens<Item> { }
  processarstring pedidoId { }
  cancelarstring pedidoId { }
}

public class ProcessadorPagamento {
  processarstring pedidoId, double valor { }
  reembolsarstring pedidoId { }
}



```

**Violação**: SRP - Múltiplas responsabilidades

### 2. Feature Envy

**Problema**: Uma classe usa mais dados de outra classe do que os seus próprios.

```csharp
// ❌ ANTI-PADRÃO: Feature Envy
public class Pedido {
  public 
    public string cliente { get; set; },
    List<Item> public itens { get; set; }
   {}
}

public class Calculadora {
  double calcularTotalPedido pedido {
    // Usa muitos dados do Pedido
    let total = 0;
    for (const item of pedido.itens) {
      total += item.preco * item.quantidade;
    }
    
    // Lógica de desconto baseada no cliente
    if (pedido.cliente.Contains('VIP')) {
      total *= 0.8;
    }
    
    return total;
  }
}

// ✅ SOLUÇÃO: Mover lógica para onde os dados estão
public class Pedido {
  double calcularTotal() {
    let subtotal = itens.Aggregate(
      (sum, item) => sum + (item.preco * item.quantidade),
      0
    );
    
    const desconto = calcularDesconto();
    return subtotal - desconto;
  }
  
  private double calcularDesconto() {
    // Lógica de desconto baseada no cliente
    if (cliente.Contains('VIP')) {
      return getSubtotal() * 0.2;
    }
    return 0;
  }
}



```

**Violação**: SRP - Lógica deveria estar na classe que possui os dados

### 3. Switch Statements / If-Else Chains

**Problema**: Múltiplos if-else ou switch que precisam ser modificados para novos casos.

```csharp
// ❌ ANTI-PADRÃO: Switch Statement
public class ProcessadorPagamento {
  processarstring tipo, double valor {
    if (tipo == 'cartao') {
      // Processa cartão
      Console.WriteLine('Processando cartão...');
    } else if (tipo == 'boleto') {
      // Processa boleto
      Console.WriteLine('Processando boleto...');
    } else if (tipo == 'pix') {
      // Processa PIX
      Console.WriteLine('Processando PIX...');
    }
    // Para adicionar novo tipo, precisa MODIFICAR esta classe
  }
}

// ✅ SOLUÇÃO: Strategy Pattern (OCP)
public interface ProcessadorPagamento {
  processardouble valor;
}

public class ProcessadorCartao : ProcessadorPagamento {
  processardouble valor {
    Console.WriteLine('Processando cartão...');
  }
}

public class ProcessadorBoleto : ProcessadorPagamento {
  processardouble valor {
    Console.WriteLine('Processando boleto...');
  }
}

public class ProcessadorPix : ProcessadorPagamento {
  processardouble valor {
    Console.WriteLine('Processando PIX...');
  }
}



```

**Violação**: OCP - Precisa modificar para adicionar novos casos

## 📗 Nível Intermediário: Violações Comuns

### 4. Violação de LSP: Classes que Não Podem ser Substituídas

```csharp
// ❌ ANTI-PADRÃO: Violação de LSP
public class Retangulo {
  protected double largura = 0;
  protected double altura = 0;

  setLarguradouble largura {
    largura = largura;
  }

  setAlturadouble altura {
    altura = altura;
  }

  double calcularArea() {
    return largura * altura;
  }
}

public class Quadrado : Retangulo {
  setLarguradouble largura {
    largura = largura;
    altura = largura; // ⚠️ Comportamento inesperado
  }

  setAlturadouble altura {
    altura = altura;
    largura = altura; // ⚠️ Comportamento inesperado
  }
}

// Código cliente quebra
function testarRetangulo retangulo {
  retangulo.setLargura(5);
  retangulo.setAltura(4);
  // Espera área = 20, mas com Quadrado será 16!
  Console.WriteLine(retangulo.calcularArea());
}

// ✅ SOLUÇÃO: Não usar herança quando comportamento é diferente
public interface Forma {
  double calcularArea();
}

public class Retangulo : Forma {
  private double largura,
    private double altura
   {}

  double calcularArea() {
    return largura * altura;
  }
}

public class Quadrado : Forma {
  private double lado {}

  double calcularArea() {
    return lado * lado;
  }
}



```

**Violação**: LSP - Subclasse não pode ser substituída

### 5. Interface Bloat (Interface Monolítica)

```csharp
// ❌ ANTI-PADRÃO: Interface muito grande
public interface Trabalhador {
  trabalhar();
  comer();
  dormir();
  nadar();
  voar();
  correr();
  pular();
  // ... muitos outros métodos
}

public class Humano : Trabalhador {
  void trabalhar() { }
  void comer() { }
  void dormir() { }
  void nadar() { }
  void voar() {
    throw new Exception('Humanos não voam!'); // ❌ Forçado a implementar
  }
  void correr() { }
  void pular() { }
}

// ✅ SOLUÇÃO: Interfaces segregadas
public interface Trabalhador {
  trabalhar();
}

public interface Comedor {
  comer();
}

public interface Dorminhoco {
  dormir();
}

public interface Nadador {
  nadar();
}

public class Humano : Trabalhador, Comedor, Dorminhoco, Nadador {
  void trabalhar() { }
  void comer() { }
  void dormir() { }
  void nadar() { }
  // Não precisa implementar voar()
}



```

**Violação**: ISP - Clientes forçados a depender de métodos não usados

### 6. Dependency Hell (Dependências Diretas)

```csharp
// ❌ ANTI-PADRÃO: Dependência direta de baixo nível
public class ServicoPedido {
  private MySQLDatabase database; // ❌ Depende de implementação específica

  public  {
    database = new MySQLDatabase(); // ❌ Cria dependência
  }

  criarPedidoDadosPedido dados {
    database.conectar();
    database.inserir('pedidos', dados);
  }
}

// ✅ SOLUÇÃO: Dependency Inversion
public interface RepositorioPedido {
  salvarPedido pedido;
}

public class ServicoPedido {
  private RepositorioPedido repositorio {} // ✅ Depende de abstração

  criarPedidoDadosPedido dados {
    const pedido = new Pedido(dados);
    repositorio.salvar(pedido);
  }
}



```

**Violação**: DIP - Alto nível depende de baixo nível

## 📕 Nível Avançado: Anti-padrões Complexos

### 7. Anemic Domain Model

**Problema**: Classes de domínio sem comportamento, apenas dados.

```csharp
// ❌ ANTI-PADRÃO: Modelo anêmico
public class Pedido {
  public string id { get; set; };
  public string cliente { get; set; };
  List<Item> public itens { get; set; };
  public double total { get; set; };
  // Apenas dados, sem comportamento
}

public class ServicoPedido {
  double calcularTotalPedido pedido {
    // Toda lógica está fora da classe de domínio
    return pedido.itens.Aggregate((sum, item) => sum + item.preco, 0);
  }

  aplicarDescontoPedido pedido, double percentual {
    pedido.total = pedido.total * (1 - percentual / 100);
  }

  bool validarPedido pedido {
    return pedido.itens.length > 0;
  }
}

// ✅ SOLUÇÃO: Rich Domain Model
public class Pedido {
  public 
    public string id { get; set; },
    public string cliente { get; set; },
    private List<itens> Item
   {}

  double calcularTotal() {
    // Lógica de negócio na classe de domínio
    const subtotal = itens.Aggregate(
      (sum, item) => sum + (item.preco * item.quantidade),
      0
    );
    return subtotal - calcularDesconto();
  }

  aplicarDescontodouble percentual {
    // Comportamento encapsulado
    const desconto = calcularTotal() * (percentual / 100);
    // Aplicar desconto...
  }

  bool validar() {
    // Regras de negócio na classe
    return itens.length > 0 && cliente.length > 0;
  }

  adicionarItemItem item {
    // Comportamento de domínio
    itens.push(item);
  }
}



```

**Violação**: SRP e encapsulamento - Lógica de negócio fora do domínio

### 8. Primitive Obsession

**Problema**: Uso excessivo de tipos primitivos ao invés de objetos de valor.

```csharp
// ❌ ANTI-PADRÃO: Obsessão por primitivos
public class Usuario {
  public 
    public string nome { get; set; },        // ❌ String genérica
    public string email { get; set; },        // ❌ String genérica
    public string cpf { get; set; },         // ❌ String genérica
    public string telefone { get; set; }      // ❌ String genérica
   {}

  bool validarEmail() {
    // Validação espalhada
    return email.Contains('@');
  }

  bool validarCPF() {
    // Validação espalhada
    return cpf.length == 11;
  }
}

// ✅ SOLUÇÃO: Value Objects
public class Email {
  private string valor {
    if (!isValid(valor)) {
      throw new Exception('Email inválido');
    }
  }

  private bool isValidstring email {
    return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email);
  }

  string getValor() {
    return valor;
  }
}

public class CPF {
  private string valor {
    if (!isValid(valor)) {
      throw new Exception('CPF inválido');
    }
  }

  private bool isValidstring cpf {
    // Validação completa de CPF
    return cpf.length == 11 && /^\d+$/.test(cpf);
  }

  string getValor() {
    return valor;
  }
}

public class Usuario {
  public 
    public string nome { get; set; },
    public Email email { get; set; },      // ✅ Value Object
    public CPF cpf { get; set; },          // ✅ Value Object
    public string telefone { get; set; }
   {}
}



```

**Benefício**: Validação centralizada, tipos mais seguros

### 9. Shotgun Surgery

**Problema**: Uma mudança requer modificações em muitas classes.

```csharp
// ❌ ANTI-PADRÃO: Mudança espalhada
public class Pedido {
  double calcularTotal() {
    // Lógica de desconto espalhada
    if (cliente.tipo == 'VIP') {
      return subtotal * 0.8;
    }
    return subtotal;
  }
}

public class Carrinho {
  double calcularTotal() {
    // Mesma lógica duplicada
    if (cliente.tipo == 'VIP') {
      return subtotal * 0.8;
    }
    return subtotal;
  }
}

public class Orcamento {
  double calcularTotal() {
    // Mesma lógica duplicada novamente
    if (cliente.tipo == 'VIP') {
      return subtotal * 0.8;
    }
    return subtotal;
  }
}

// Para mudar desconto VIP, precisa modificar 3 classes!

// ✅ SOLUÇÃO: Centralizar lógica
public interface EstrategiaDesconto {
  double aplicardouble subtotal;
}

public class DescontoVIP : EstrategiaDesconto {
  double aplicardouble subtotal {
    return subtotal * 0.8;
  }
}

public class CalculadoraDesconto {
  double calculardouble subtotal, EstrategiaDesconto estrategia {
    return estrategia.aplicar(subtotal);
  }
}

// Todas as classes usam a mesma calculadora
public class Pedido {
  double calcularTotalCalculadoraDesconto calculadora, EstrategiaDesconto estrategia {
    return calculadora.calcular(subtotal, estrategia);
  }
}



```

**Violação**: DRY e OCP - Lógica duplicada e difícil de mudar

## 🔍 Como Identificar Violações

### Checklist de Verificação

**SRP:**
- [ ] A classe tem mais de uma razão para mudar?
- [ ] Posso descrever a classe em uma frase sem "e" ou "ou"?
- [ ] Mudanças em uma parte afetam outras partes não relacionadas?

**OCP:**
- [ ] Preciso modificar código existente para adicionar novas funcionalidades?
- [ ] Há muitos if-else ou switch statements?
- [ ] Novos tipos requerem mudanças em classes existentes?

**LSP:**
- [ ] Subclasses lançam exceções que a classe base não lança?
- [ ] Subclasses têm comportamento diferente do esperado?
- [ ] Código cliente quebra quando usa subclasses?

**ISP:**
- [ ] Interfaces têm muitos métodos não relacionados?
- [ ] Classes implementam métodos que nunca usam?
- [ ] Há muitos `throw new Error` em implementações?

**DIP:**
- [ ] Alto nível depende diretamente de baixo nível?
- [ ] Difícil trocar implementações?
- [ ] Difícil testar (precisa de dependências reais)?

## ✅ Técnicas de Refatoração

### 1. Extract Class
Separe responsabilidades em classes diferentes.

### 2. Extract Interface
Crie interfaces específicas para reduzir acoplamento.

### 3. Replace Conditional with Polymorphism
Substitua if-else por polimorfismo.

### 4. Introduce Parameter Object
Agrupe parâmetros relacionados em objetos.

### 5. Dependency Injection
Inverta dependências usando injeção.

## ✅ Checkpoint

### Auto-avaliação

- [ ] Consigo identificar os principais anti-padrões
- [ ] Sei como refatorar código que viola SOLID
- [ ] Entendo as técnicas de refatoração comuns
- [ ] Posso aplicar correções em código real
- [ ] Compreendo quando aplicar cada técnica

### Exercícios

Pratique identificando e corrigindo violações nos exercícios em [exercicios/](./exercicios/README.md).

## 🔗 Próximos Passos

- [Voltar ao índice da trilha](./README.md)
- [Revisar módulos anteriores](./README.md#módulos)

---

[← Voltar ao índice principal](../../INDEX.md)
