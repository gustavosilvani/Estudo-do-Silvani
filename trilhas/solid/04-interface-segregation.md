# 04 - Interface Segregation Principle (ISP)

## 📘 Nível Básico

### O que é o Interface Segregation Principle?

O **Interface Segregation Principle (ISP)** ou **Princípio de Segregação de Interface** afirma que:

> **Clientes não devem ser forçados a depender de interfaces que não utilizam.**

Em outras palavras, é melhor ter várias interfaces específicas do que uma interface grande e genérica.

### Definição Formal

Robert C. Martin define ISP como:

> Muitas interfaces específicas do cliente são melhores do que uma interface de propósito geral.

### Contexto Histórico

O Princípio de Segregação de Interface foi formalizado por **Robert C. Martin (Uncle Bob)** no início dos anos 2000, como parte dos princípios SOLID. O ISP surgiu da observação de que interfaces grandes e monolíticas forçavam classes a implementar métodos que não utilizavam, criando acoplamento desnecessário. Este princípio complementa o LSP, pois interfaces menores e específicas facilitam a substituição correta de implementações.

### Exemplo Básico: Violação do ISP

```csharp
// ❌ VIOLAÇÃO: Interface muito grande, força implementação de métodos não usados

public interface Trabalhador {
  trabalhar();
  comer();
  dormir();
  nadar();
  voar();
}

// Humanos precisam implementar todos os métodos, mesmo os que não fazem sentido
public class Humano : Trabalhador {
  void trabalhar() {
    Console.WriteLine('Humano trabalhando');
  }

  void comer() {
    Console.WriteLine('Humano comendo');
  }

  void dormir() {
    Console.WriteLine('Humano dormindo');
  }

  void nadar() {
    Console.WriteLine('Humano nadando');
  }

  void voar() {
    // ❌ Humanos não voam! Mas são forçados a implementar
    throw new Exception('Humanos não podem voar');
  }
}

// Pássaros também são forçados a implementar métodos irrelevantes
public class Passaro : Trabalhador {
  void trabalhar() {
    throw new Exception('Pássaros não trabalham');
  }

  void comer() {
    Console.WriteLine('Pássaro comendo');
  }

  void dormir() {
    Console.WriteLine('Pássaro dormindo');
  }

  void nadar() {
    throw new Exception('Este pássaro não nada');
  }

  void voar() {
    Console.WriteLine('Pássaro voando');
  }
}




```

**Problemas:**
- Classes são forçadas a implementar métodos que não usam
- Código com muitos `throw new Error` ou implementações vazias
- Acoplamento desnecessário
- Violação do princípio "não use o que não precisa"

### Exemplo Básico: Aplicando ISP

```csharp
// ✅ CORRETO: Interfaces segregadas e específicas

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

public interface Voador {
  voar();
}

// Humanos implementam apenas o que faz sentido
public class Humano : Trabalhador, Comedor, Dorminhoco, Nadador {
  void trabalhar() {
    Console.WriteLine('Humano trabalhando');
  }

  void comer() {
    Console.WriteLine('Humano comendo');
  }

  void dormir() {
    Console.WriteLine('Humano dormindo');
  }

  void nadar() {
    Console.WriteLine('Humano nadando');
  }
}

// Pássaros implementam apenas o que faz sentido
public class Passaro : Comedor, Dorminhoco, Voador {
  void comer() {
    Console.WriteLine('Pássaro comendo');
  }

  void dormir() {
    Console.WriteLine('Pássaro dormindo');
  }

  void voar() {
    Console.WriteLine('Pássaro voando');
  }
}

// Peixes implementam apenas o que faz sentido
public class Peixe : Comedor, Dorminhoco, Nadador {
  void comer() {
    Console.WriteLine('Peixe comendo');
  }

  void dormir() {
    Console.WriteLine('Peixe dormindo');
  }

  void nadar() {
    Console.WriteLine('Peixe nadando');
  }
}




```

**Benefícios:**
- Classes implementam apenas métodos relevantes
- Sem código desnecessário ou exceções
- Interfaces específicas e focadas
- Fácil adicionar novos comportamentos

## 📗 Nível Intermediário

### Identificando Violações do ISP

Sinais de que uma interface viola ISP:

1. **Métodos não implementados**: Muitos `throw new Error` ou implementações vazias
2. **Dependências não usadas**: Clientes dependem de métodos que nunca chamam
3. **Interfaces grandes**: Interface com muitos métodos não relacionados
4. **Mudanças frequentes**: Mudanças em uma parte afetam clientes não relacionados

### Casos de Uso Comuns

#### 1. Interfaces de Repositório

```csharp
// ❌ Antes: Interface monolítica
public interface Repositorio<T> {
  criarT entidade;
  T lerstring id?;
  atualizarstring id, T entidade;
  deletarstring id;
  T listar()[];
  T List<filtro> buscarFiltro;
  double contar();
  string exportar();
  importarstring dados;
}

// ✅ Depois: Interfaces segregadas
public interface Leitor<T> {
  T lerstring id?;
  T listar()[];
  T List<filtro> buscarFiltro;
}

public interface Escritor<T> {
  criarT entidade;
  atualizarstring id, T entidade;
  deletarstring id;
}

public interface Contador {
  double contar();
}

public interface ImportadorExportador {
  string exportar();
  importarstring dados;
}

// Clientes podem depender apenas do que precisam
public class ServicoLeitura<T> {
  private Leitor leitor<T> {}
  
  // Usa apenas métodos de leitura
  T obterPorIdstring id? {
    return leitor.ler(id);
  }
}

public class ServicoEscrita<T> {
  private Escritor escritor<T> {}
  
  // Usa apenas métodos de escrita
  salvarT entidade {
    escritor.criar(entidade);
  }
}




```

#### 2. Interfaces de Dispositivos

```csharp
// ❌ Antes
public interface Dispositivo {
  ligar();
  desligar();
  aumentarVolume();
  diminuirVolume();
  mudarCanal();
  gravar();
  reproduzir();
}

// ✅ Depois
public interface Ligavel {
  ligar();
  desligar();
}

public interface ControlavelVolume {
  aumentarVolume();
  diminuirVolume();
}

public interface ControlavelCanal {
  mudarCanal();
}

public interface Gravavel {
  gravar();
}

public interface Reproduzivel {
  reproduzir();
}

// Dispositivos implementam apenas o que têm
public class TV : Ligavel, ControlavelVolume, ControlavelCanal {
  void ligar() { }
  void desligar() { }
  void aumentarVolume() { }
  void diminuirVolume() { }
  void mudarCanal() { }
}

public class Radio : Ligavel, ControlavelVolume {
  void ligar() { }
  void desligar() { }
  void aumentarVolume() { }
  void diminuirVolume() { }
}




```

#### 3. Interfaces de Autenticação

```csharp
// ❌ Antes
public interface Autenticador {
  loginstring usuario, string senha;
  logout();
  registrarstring usuario, string senha;
  recuperarSenhastring email;
  bool validarTokenstring token;
  string renovarTokenstring token;
}

// ✅ Depois
public interface Login {
  loginstring usuario, string senha;
  logout();
}

public interface Registro {
  registrarstring usuario, string senha;
}

public interface RecuperacaoSenha {
  recuperarSenhastring email;
}

public interface GerenciamentoToken {
  bool validarTokenstring token;
  string renovarTokenstring token;
}

// Serviços podem depender apenas do que precisam
public class ServicoLogin {
  private Login autenticador {}
  
  fazerLoginstring usuario, string senha {
    autenticador.login(usuario, senha);
  }
}




```

### Benefícios Práticos

1. **Desacoplamento**: Clientes não dependem de métodos que não usam
2. **Manutenibilidade**: Mudanças em uma interface não afetam clientes não relacionados
3. **Testabilidade**: Mais fácil criar mocks específicos
4. **Clareza**: Interfaces menores são mais fáceis de entender
5. **Flexibilidade**: Fácil combinar comportamentos conforme necessário

### Quando Aplicar ISP

Aplique ISP quando:

- Interface tem muitos métodos não relacionados
- Clientes implementam métodos que nunca usam
- Mudanças em uma parte afetam clientes não relacionados
- Você quer reduzir acoplamento entre componentes

## 📕 Nível Avançado

### ISP e Dependency Inversion

ISP trabalha bem com DIP:

```csharp
// Interfaces segregadas (ISP)
public interface LeitorConfiguracao {
  Configuracao ler();
}

public interface EscritorConfiguracao {
  escreverConfiguracao config;
}

// Dependência de abstrações (DIP)
public class GerenciadorConfiguracao {
  private LeitorConfiguracao leitor,
    private EscritorConfiguracao escritor
   {}
  
  atualizarConfiguracao config {
    var atual = leitor.ler();
    var nova = { ...atual, ...config };
    escritor.escrever(nova);
  }
}

// Implementações específicas
public class LeitorArquivo : LeitorConfiguracao {
  Configuracao void ler() {
    // Lê de arquivo
    return {};
  }
}

public class EscritorBancoDados : EscritorConfiguracao {
  escreverConfiguracao config {
    // Escreve no banco
  }
}




```

### ISP e Adapter Pattern

ISP facilita o uso de adapters:

```csharp
// Interface específica
public interface Pagador {
  pagardouble valor;
}

// Adapter para sistema externo
public class AdapterPagamentoExterno : Pagador {
  private SistemaExterno sistemaExterno {}
  
  pagardouble valor {
    // Adapta public interface externa para nossa public interface específica
    sistemaExterno.processarPagamento(valor);
  }
}




```

### ISP em Arquitetura de Microserviços

ISP é importante em arquiteturas distribuídas:

```csharp
// Cada serviço tem public interface específica
public interface ServicoUsuario {
  Usuario buscarUsuariostring id;
  Usuario criarUsuarioDadosUsuario dados;
}

public interface ServicoPedido {
  Pedido criarPedidoDadosPedido dados;
  Pedido buscarPedidostring id;
}

public interface ServicoPagamento {
  processarPagamentostring pedidoId, double valor;
}

// Serviços dependem apenas do que precisam
public class ServicoOrdem {
  private ServicoUsuario usuarios,
    private ServicoPedido pedidos,
    private ServicoPagamento pagamentos
   {}
  
  criarOrdemstring usuarioId, DadosPedido dadosPedido {
    var usuario = usuarios.buscarUsuario(usuarioId);
    var pedido = pedidos.criarPedido(dadosPedido);
    pagamentos.processarPagamento(pedido.Id, pedido.valor);
  }
}




```

### ISP e TypeScript

TypeScript facilita ISP com tipos:

```csharp
// Tipos específicos
type Leitor<T> = {
  T lerstring id?;
  T listar()[];
};

type Escritor<T> = {
  criarT entidade;
  atualizarstring id, T entidade;
  deletarstring id;
};

// Intersection types para combinar
type RepositorioCompleto<T> = Leitor<T> & Escritor<T>;

// Clientes podem usar tipos específicos
function processarLeitura<T>Leitor leitor<T> {
  var itens = leitor.listar();
  // Processa apenas leitura
}

function processarEscrita<T>Escritor escritor<T>, T entidade {
  escritor.criar(entidade);
}




```

### Armadilhas Comuns

#### 1. Over-segregation

```csharp
// ❌ Não segrege demais
public interface GetterNome {
  string getNome();
}

public interface SetterNome {
  setNomestring nome;
}

// ✅ Interfaces relacionadas podem estar juntas
public interface GerenciadorNome {
  string getNome();
  setNomestring nome;
}




```

**Regra**: Segregue quando métodos não estão relacionados ou não são sempre usados juntos.

#### 2. Interfaces Vazias

```csharp
// ❌ Evite interfaces com um único método (a menos que faça sentido)
public interface UnicoMetodo {
  fazer();
}

// ✅ Considere se realmente precisa de interface
public class ClasseDireta {
  void fazer() { }
}




```

### ISP e Outros Princípios SOLID

- **SRP → ISP**: Responsabilidades únicas levam a interfaces menores
- **LSP → ISP**: Substituição correta funciona melhor com interfaces específicas
- **ISP → DIP**: Interfaces segregadas facilitam inversão de dependências

## ⚠️ Armadilhas Comuns

1. **Sobre-segregação**: Não crie interfaces muito pequenas sem necessidade
2. **Ignorar relacionamentos**: Métodos relacionados podem estar juntos
3. **Complexidade desnecessária**: Às vezes uma interface maior é mais simples
4. **Aplicar cegamente**: Use bom senso, não dogma

## ✅ Checkpoint

### Auto-avaliação

- [ ] Entendo o que é Interface Segregation Principle
- [ ] Consigo identificar violações do ISP
- [ ] Sei como segregar interfaces grandes
- [ ] Entendo quando aplicar ISP e quando não aplicar
- [ ] Compreendo a relação entre ISP e outros princípios SOLID

### Exercícios

Pratique com os exercícios do módulo em [exercicios/](./exercicios/README.md).

## 🔗 Próximos Passos

- [Próximo: Dependency Inversion Principle →](./05-dependency-inversion.md)
- [Voltar ao índice da trilha](./README.md)

---

[← Voltar ao índice principal](../../INDEX.md)
