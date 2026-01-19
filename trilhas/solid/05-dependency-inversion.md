# 05 - Dependency Inversion Principle (DIP)

## 📘 Nível Básico

### O que é o Dependency Inversion Principle?

O **Dependency Inversion Principle (DIP)** ou **Princípio de Inversão de Dependência** afirma que:

> **Módulos de alto nível não devem depender de módulos de baixo nível. Ambos devem depender de abstrações.**
> 
> **Abstrações não devem depender de detalhes. Detalhes devem depender de abstrações.**

### Definição Formal

Robert C. Martin define DIP como:

1. Módulos de alto nível não devem depender de módulos de baixo nível. Ambos devem depender de abstrações.
2. Abstrações não devem depender de detalhes. Detalhes devem depender de abstrações.

### Contexto Histórico

O Princípio de Inversão de Dependência foi formalizado por **Robert C. Martin (Uncle Bob)** no início dos anos 2000, como parte dos princípios SOLID. O DIP é um dos princípios mais importantes para arquitetura de software, pois estabelece a base para camadas de aplicação independentes de detalhes de implementação. Este princípio é fundamental para arquiteturas em camadas, Clean Architecture e Domain-Driven Design. A técnica de Dependency Injection, que implementa o DIP, foi popularizada por frameworks como Spring (Java) e .NET Core.

### Exemplo Básico: Violação do DIP

```csharp
// ❌ VIOLAÇÃO: Alto nível depende de baixo nível

// Módulo de baixo nível (detalhe de implementação)
public class MySQLDatabase {
  void conectar() {
    Console.WriteLine('Conectando ao MySQL...');
  }

  salvarstring dados {
    Console.WriteLine('Salvando no MySQL:', dados);
  }
}

// Módulo de alto nível (lógica de negócio)
public class ServicoUsuario {
  private MySQLDatabase database; // ❌ Depende de implementação específica

  public  {
    database = new MySQLDatabase(); // ❌ Cria dependência direta
  }

  salvarUsuariostring nome {
    database.conectar();
    database.salvar`Usuario: ${nome}`;
  }
}




```

**Problemas:**
- `ServicoUsuario` (alto nível) depende de `MySQLDatabase` (baixo nível)
- Se quisermos trocar para PostgreSQL, precisamos modificar `ServicoUsuario`
- Difícil testar (precisa de banco real)
- Violação do princípio de inversão

### Exemplo Básico: Aplicando DIP

```csharp
// ✅ CORRETO: Ambos dependem de abstração

// Abstração (interface)
public interface Repositorio {
  salvarstring dados;
}

// Módulo de baixo nível (detalhe) depende da abstração
public class MySQLDatabase : Repositorio {
  void conectar() {
    Console.WriteLine('Conectando ao MySQL...');
  }

  salvarstring dados {
    conectar();
    Console.WriteLine('Salvando no MySQL:', dados);
  }
}

public class PostgreSQLDatabase : Repositorio {
  void conectar() {
    Console.WriteLine('Conectando ao PostgreSQL...');
  }

  salvarstring dados {
    conectar();
    Console.WriteLine('Salvando no PostgreSQL:', dados);
  }
}

// Módulo de alto nível depende da abstração
public class ServicoUsuario {
  private Repositorio repositorio; // ✅ Depende de abstração

  public repositorio: Repositorio { // ✅ Injeção de dependência
    repositorio = repositorio;
  }

  salvarUsuariostring nome {
    repositorio.salvar`Usuario: ${nome}`;
  }
}

// Uso: alto nível controla qual implementação usar
var mysqlRepo = new MySQLDatabase();
var servico = new ServicoUsuario(mysqlRepo);

// Fácil trocar implementação
var postgresRepo = new PostgreSQLDatabase();
var servico2 = new ServicoUsuario(postgresRepo);




```

**Benefícios:**
- Alto nível não depende de baixo nível
- Fácil trocar implementações
- Fácil testar (pode usar mocks)
- Código mais flexível e manutenível

## 📗 Nível Intermediário

### Dependency Injection

DIP é frequentemente implementado através de **Dependency Injection (DI)**:

#### 1. Constructor Injection (Recomendado)

```csharp
public interface Logger {
  logstring mensagem;
}

public class ConsoleLogger : Logger {
  logstring mensagem {
    Console.WriteLine(mensagem);
  }
}

public class FileLogger : Logger {
  logstring mensagem {
    // Escreve em arquivo
  }
}

public class ServicoPedido {
  private RepositorioPedido repositorio,
    private Logger logger // ✅ Injetado via construtor
   {}

  criarPedidoDadosPedido dados {
    logger.log('Criando pedido...');
    repositorio.salvar(new Pedido(dados));
    logger.log('Pedido criado');
  }
}




```

#### 2. Property Injection

```csharp
public class ServicoPedido {
  private logger?: Logger;

  setLoggerLogger logger {
    logger = logger;
  }

  criarPedidoDadosPedido dados {
    logger?.log('Criando pedido...');
  }
}




```

#### 3. Method Injection

```csharp
public class ServicoPedido {
  criarPedidoDadosPedido dados, Logger logger {
    logger.log('Criando pedido...');
  }
}




```

### DIP vs Dependency Injection

É importante entender a diferença entre estes dois conceitos relacionados, mas distintos:

**Dependency Inversion Principle (DIP)**
- **Foco**: Estrutura de dependências
- **Define**: Como classes devem depender de abstrações
- **Objetivo**: Reduzir acoplamento entre componentes
- **Nível**: Princípio arquitetural

**Dependency Injection (DI)**
- **Foco**: Implementação de dependências
- **Fornece**: Mecanismos para injetar dependências
- **Objetivo**: Facilitar a modularidade e testabilidade
- **Nível**: Padrão de implementação

**Conclusão**: DIP é um princípio arquitetural, enquanto DI é um padrão de implementação. DI é uma técnica concreta que ajuda a realizar os objetivos do DIP, criando software mais flexível e desacoplado.

### Casos de Uso Comuns

#### 1. Repositórios e Persistência

```csharp
// Abstração
public interface RepositorioUsuario {
  Usuario buscarstring id?;
  salvarUsuario usuario;
}

// Implementações de baixo nível
public class RepositorioUsuarioMemoria : RepositorioUsuario {
  private Dictionary<string, Usuario> usuarios = new Map();

  Usuario buscarstring id? {
    return usuarios[id] || null;
  }

  salvarUsuario usuario {
    usuarios[usuario.Id] = usuario;
  }
}

public class RepositorioUsuarioBD : RepositorioUsuario {
  Usuario buscarstring id? {
    // Busca no banco de dados
    return null;
  }

  salvarUsuario usuario {
    // Salva no banco de dados
  }
}

// Alto nível depende de abstração
public class ServicoUsuario {
  private RepositorioUsuario repositorio {}

  Usuario obterUsuariostring id? {
    return repositorio.buscar(id);
  }

  Usuario criarUsuarioDadosUsuario dados {
    var usuario = new Usuario(dados);
    repositorio.salvar(usuario);
    return usuario;
  }
}




```

#### 2. Serviços Externos

```csharp
// Abstração
public interface ServicoEmail {
  enviarstring destino, string assunto, string corpo;
}

// Implementações
public class ServicoEmailSMTP : ServicoEmail {
  enviarstring destino, string assunto, string corpo {
    // Envia via SMTP
  }
}

public class ServicoEmailSendGrid : ServicoEmail {
  enviarstring destino, string assunto, string corpo {
    // Envia via SendGrid API
  }
}

public class ServicoEmailMock : ServicoEmail {
  enviarstring destino, string assunto, string corpo {
    // Mock para testes
    Console.WriteLine`Enviando Mock para ${destino}`;
  }
}

// Alto nível
public class ServicoNotificacao {
  private ServicoEmail email {}

  notificarUsuarioUsuario usuario, string mensagem {
    email.enviar(usuario.email, 'Notificação', mensagem);
  }
}




```

#### 3. Logging e Monitoramento

```csharp
public interface Logger {
  infostring mensagem;
  errostring mensagem, erro?: Error;
  debugstring mensagem;
}

public class LoggerConsole : Logger {
  infostring mensagem {
    Console.WriteLine(`[INFO] ${mensagem}`);
  }

  errostring mensagem, erro?: Error {
    Console.Error.WriteLine(`[ERRO] ${mensagem}`, erro);
  }

  debugstring mensagem {
    console.debug(`[DEBUG] ${mensagem}`);
  }
}

public class LoggerArquivo : Logger {
  infostring mensagem {
    // Escreve em arquivo
  }

  errostring mensagem, erro?: Error {
    // Escreve erro em arquivo
  }

  debugstring mensagem {
    // Escreve debug em arquivo
  }
}

public class ServicoPedido {
  private RepositorioPedido repositorio,
    private Logger logger
   {}

  processarPedido pedido {
    logger.info(`Processando pedido ${pedido.Id}`);
    try {
      repositorio.salvar(pedido);
      logger.info(`Pedido ${pedido.Id} processado com sucesso`);
    } catch (erro) {
      logger.erro(`Erro ao processar pedido ${pedido.Id}`, erro);
    }
  }
}




```

### Benefícios Práticos

1. **Flexibilidade**: Fácil trocar implementações
2. **Testabilidade**: Fácil criar mocks e stubs
3. **Desacoplamento**: Módulos são independentes
4. **Manutenibilidade**: Mudanças em baixo nível não afetam alto nível
5. **Reutilização**: Abstrações podem ser reutilizadas

### Quando Aplicar DIP

Aplique DIP quando:

- Você tem dependências de frameworks externos
- Precisa trocar implementações facilmente
- Quer facilitar testes
- Tem código que depende de detalhes de implementação
- Quer reduzir acoplamento entre módulos

## 📕 Nível Avançado

### DIP e Inversion of Control (IoC)

DIP é frequentemente implementado com **Inversion of Control Containers**:

```csharp
// Container IoC simples
public class Container {
  private Dictionary<string, any> dependencias = new Map();

  registrar<T>string nome, factory: ( => T) {
    dependencias[nome] = factory;
  }

  resolver<T>string nome: T {
    var factory = dependencias[nome];
    if (!factory) {
      throw new Exception(`Dependência ${nome} não registrada`);
    }
    return factory();
  }
}

// Configuração
var container = new Container();

container.registrar('Repositorio', () => new RepositorioUsuarioBD());
container.registrar('Logger', () => new LoggerConsole());
container.registrar('ServicoUsuario', () => 
  new ServicoUsuario(
    container.resolver('Repositorio'),
    container.resolver('Logger')
  )
);

// Uso
var servico = container.resolver<ServicoUsuario>('ServicoUsuario');




```

### DIP em Arquitetura em Camadas

```csharp
// Camada de Domínio (alto nível) - não depende de nada
public class Pedido {
  public 
    public string id { get; set; },
    List<Item> public itens { get; set; },
    public double total { get; set; }
   {}
}

// Camada de Aplicação (alto nível) - depende de abstrações
public interface RepositorioPedido {
  salvarPedido pedido;
  Pedido buscarstring id?;
}

public interface ServicoPagamento {
  processardouble valor;
}

public class ServicoPedido {
  private RepositorioPedido repositorio,
    private ServicoPagamento pagamento
   {}

  Pedido criarPedidoDadosPedido dados {
    var pedido = new Pedido(dados.Id, dados.itens, dados.Total);
    repositorio.salvar(pedido);
    pagamento.processar(pedido.Total);
    return pedido;
  }
}

// Camada de Infraestrutura (baixo nível) - implementa abstrações
public class RepositorioPedidoBD : RepositorioPedido {
  salvarPedido pedido {
    // Salva no banco de dados
  }

  Pedido buscarstring id? {
    // Busca no banco de dados
    return null;
  }
}

public class ServicoPagamentoStripe : ServicoPagamento {
  processardouble valor {
    // Processa via Stripe
  }
}




```

### DIP e Testes

DIP facilita muito os testes:

```csharp
// Mock para testes
public class RepositorioUsuarioMock : RepositorioUsuario {
  private Dictionary<string, Usuario> usuarios = new Map();

  Usuario buscarstring id? {
    return usuarios[id] || null;
  }

  salvarUsuario usuario {
    usuarios[usuario.Id] = usuario;
  }
}

public class LoggerMock : Logger {
  private List<logs> string = new List<logs>();

  infostring mensagem {
    logs.Add($"[INFO] {mensagem}");
  }

  string getLogs()[] {
    return logs;
  }

  errostring mensagem, erro?: Error {}
  debugstring mensagem {}
}

// Testes
describe('ServicoUsuario', () => {
  it('deve criar usuário', () => {
    var repositorio = new RepositorioUsuarioMock();
    var logger = new LoggerMock();
    var servico = new ServicoUsuario(repositorio, logger);

    var usuario = servico.criarUsuario{ nome: 'João', email: 'joao@email.com' };

    expect(usuario.nome).toBe('João');
    expect(repositorio.buscar(usuario.Id)).toBe(usuario);
  });
});




```

### DIP e Event-Driven Architecture

```csharp
// Abstração
public interface EventPublisher {
  publicarEvento evento;
}

public interface EventHandler {
  handleEvento evento;
}

// Implementações
public class EventPublisherRabbitMQ : EventPublisher {
  publicarEvento evento {
    // Publica no RabbitMQ
  }
}

public class EventPublisherKafka : EventPublisher {
  publicarEvento evento {
    // Publica no Kafka
  }
}

// Alto nível
public class ServicoPedido {
  private RepositorioPedido repositorio,
    private EventPublisher eventPublisher
   {}

  criarPedidoDadosPedido dados {
    var pedido = new Pedido(dados);
    repositorio.salvar(pedido);
    eventPublisher.publicar(new PedidoCriadoEvent(pedido));
  }
}




```

### Armadilhas Comuns

#### 1. Abstrações Desnecessárias

```csharp
// ❌ Não crie abstrações para coisas simples que não vão mudar
public interface Somador {
  double somardouble a, double b;
}

// ✅ Mantenha simples
function double somardouble a, double b {
  return a + b;
}




```

#### 2. Dependency Hell

```csharp
// ❌ Evite muitas dependências
public class Servico {
  private Repo1 repo1,
    private Repo2 repo2,
    private Repo3 repo3,
    private Servico1 servico1,
    private Servico2 servico2,
    // ... muitas dependências
   {}
}

// ✅ Considere agrupar dependências relacionadas
public class ConfiguracaoRepositorios {
  public 
    public Repo1 repo1 { get; set; },
    public Repo2 repo2 { get; set; },
    public Repo3 repo3 { get; set; }
   {}
}

public class Servico {
  private ConfiguracaoRepositorios repos,
    private ConfiguracaoServicos servicos
   {}
}




```

### DIP e Outros Princípios SOLID

- **SRP → DIP**: Responsabilidades únicas facilitam inversão
- **OCP → DIP**: Extensibilidade requer inversão de dependências
- **LSP → DIP**: Substituição funciona com abstrações
- **ISP → DIP**: Interfaces segregadas facilitam inversão

## ⚠️ Armadilhas Comuns

1. **Over-abstraction**: Não crie abstrações desnecessárias
2. **Dependency hell**: Evite muitas dependências
3. **Ignorar simplicidade**: Às vezes dependência direta é melhor
4. **Aplicar cegamente**: Use bom senso

## ✅ Checkpoint

### Auto-avaliação

- [ ] Entendo o que é Dependency Inversion Principle
- [ ] Consigo identificar violações do DIP
- [ ] Sei como aplicar DIP usando Dependency Injection
- [ ] Entendo a diferença entre alto nível e baixo nível
- [ ] Compreendo como DIP facilita testes

### Exercícios

Pratique com os exercícios do módulo em [exercicios/](./exercicios/README.md).

## 🔗 Próximos Passos

- [Próximo: Aplicação Prática →](./06-aplicacao-pratica.md)
- [Voltar ao índice da trilha](./README.md)

---

[← Voltar ao índice principal](../../INDEX.md)
