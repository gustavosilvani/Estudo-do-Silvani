# 06 - Aplicação Prática: Projeto Completo

## 📖 Visão Geral

Neste módulo, vamos aplicar todos os princípios SOLID em um projeto completo. Vamos refatorar um sistema de e-commerce, passo a passo, aplicando cada princípio.

## 🎯 Objetivo do Projeto

Criar um sistema de gerenciamento de pedidos de e-commerce que:
- Gerencia pedidos de clientes
- Calcula descontos
- Processa pagamentos
- Envia notificações
- Persiste dados

## 📘 Nível Básico: Código Inicial (Violando SOLID)

Vamos começar com código que viola todos os princípios SOLID:

```csharp
// ❌ Código inicial - viola todos os princípios SOLID

public class Pedido {
  public string id { get; set; };
  public string cliente { get; set; };
  List<Item> public itens { get; set; };
  public double total { get; set; };
  public string status { get; set; };
  public double desconto { get; set; };

  public id(string cliente: string, itens: List<Item> {
    id = id;
    cliente = cliente;
    itens = itens;
    total = 0;
    status = 'pendente';
    desconto = 0;
  }

  // Violação SRP: Múltiplas responsabilidades
  double calcularTotal() {
    let subtotal = itens.Aggregate((sum, item) => sum + item.preco, 0);
    
    // Lógica de desconto misturada
    if (cliente == 'VIP') {
      desconto = subtotal * 0.2;
    } else if (cliente == 'Premium') {
      desconto = subtotal * 0.15;
    }
    
    total = subtotal - desconto;
    return total;
  }

  // Violação SRP: Persistência na classe de domínio
  void salvar() {
    Console.WriteLine(`Salvando pedido ${id} no banco de dados...`);
    // Código de persistência
  }

  // Violação SRP: Notificação na classe de domínio
  void enviarEmail() {
    Console.WriteLine(`Enviando email para ${cliente} sobre pedido ${id}`);
  }

  // Violação OCP: Precisa modificar para novos tipos de pagamento
  processarPagamentostring tipo {
    if (tipo == 'cartao') {
      Console.WriteLine(`Processando pagamento de ${total} via cartão`);
    } else if (tipo == 'boleto') {
      Console.WriteLine(`Processando pagamento de ${total} via boleto`);
    }
    // Para adicionar PIX, precisa MODIFICAR esta classe
  }
}

// Uso
const pedido = new Pedido'123', 'VIP', [
  { nome: 'Produto 1', 100 preco },
  { nome: 'Produto 2', 50 preco }
];

pedido.calcularTotal();
pedido.salvar();
pedido.processarPagamento('cartao');
pedido.enviarEmail();



```

## 📗 Nível Intermediário: Refatoração Passo a Passo

### Passo 1: Aplicar SRP (Single Responsibility)

```csharp
// ✅ Separar responsabilidades

// Responsabilidade: Representar dados do pedido
public class Pedido {
  public 
    public string id { get; set; },
    public string cliente { get; set; },
    List<Item> public itens { get; set; },
    public double total { get; set; } = 0,
    public string status { get; set; } = 'pendente'
   {}
}

// Responsabilidade: Calcular totais
public class CalculadoraPedido {
  double calcularSubtotalPedido pedido {
    return pedido.itens.Aggregate((sum, item) => sum + item.preco, 0);
  }
}

// Responsabilidade: Aplicar descontos
public class CalculadoraDesconto {
  double calcularDesconto desconto, double subtotal {
    return desconto.aplicar(subtotal);
  }
}

// Responsabilidade: Persistir pedidos
public class RepositorioPedido {
  salvarPedido pedido {
    Console.WriteLine(`Salvando pedido ${pedido.id} no banco de dados...`);
  }

  Pedido buscarstring id? {
    // Busca no banco
    return null;
  }
}

// Responsabilidade: Enviar notificações
public class ServicoNotificacao {
  enviarEmailstring destino, string assunto, string corpo {
    Console.WriteLine`Enviando email para ${destino}: ${assunto}`;
  }
}



```

### Passo 2: Aplicar OCP (Open/Closed)

```csharp
// ✅ Abrir para extensão, fechar para modificação

// Abstração para descontos
public interface EstrategiaDesconto {
  double aplicardouble subtotal;
}

// Implementações específicas
public class DescontoVIP : EstrategiaDesconto {
  double aplicardouble subtotal {
    return subtotal * 0.2;
  }
}

public class DescontoPremium : EstrategiaDesconto {
  double aplicardouble subtotal {
    return subtotal * 0.15;
  }
}

public class DescontoRegular : EstrategiaDesconto {
  double aplicardouble subtotal {
    return subtotal * 0.05;
  }
}

// Novo desconto pode ser adicionado sem modificar código existente
public class DescontoBlackFriday : EstrategiaDesconto {
  double aplicardouble subtotal {
    return subtotal * 0.3; // 30% de desconto
  }
}

// Abstração para pagamentos
public interface ProcessadorPagamento {
  processardouble valor;
}

// Implementações específicas
public class ProcessadorCartao : ProcessadorPagamento {
  processardouble valor {
    Console.WriteLine(`Processando pagamento de ${valor} via cartão`);
  }
}

public class ProcessadorBoleto : ProcessadorPagamento {
  processardouble valor {
    Console.WriteLine(`Processando pagamento de ${valor} via boleto`);
  }
}

// Novo método de pagamento pode ser adicionado sem modificar código
public class ProcessadorPix : ProcessadorPagamento {
  processardouble valor {
    Console.WriteLine(`Processando pagamento de ${valor} via PIX`);
  }
}



```

### Passo 3: Aplicar LSP (Liskov Substitution)

```csharp
// ✅ Garantir substituição correta

// Interface base
public interface RepositorioPedido {
  salvarPedido pedido;
  Pedido buscarstring id?;
}

// Implementações substituíveis
public class RepositorioPedidoMemoria : RepositorioPedido {
  private Dictionary<string, Pedido> pedidos = new Map();

  salvarPedido pedido {
    pedidos.set(pedido.id, pedido);
  }

  Pedido buscarstring id? {
    return pedidos.get(id) || null;
  }
}

public class RepositorioPedidoBD : RepositorioPedido {
  salvarPedido pedido {
    Console.WriteLine(`Salvando pedido ${pedido.id} no banco de dados...`);
  }

  Pedido buscarstring id? {
    Console.WriteLine(`Buscando pedido ${id} no banco de dados...`);
    return null;
  }
}

// Qualquer implementação pode ser usada
public class ServicoPedido {
  private RepositorioPedido repositorio {}

  Pedido criarPedidoDadosPedido dados {
    const pedido = new Pedido(dados.id, dados.cliente, dados.itens);
    repositorio.salvar(pedido);
    return pedido;
  }
}



```

### Passo 4: Aplicar ISP (Interface Segregation)

```csharp
// ✅ Interfaces segregadas

// Interface específica para leitura
public interface LeitorPedido {
  Pedido buscarstring id?;
  Pedido listar()[];
}

// Interface específica para escrita
public interface EscritorPedido {
  salvarPedido pedido;
  atualizarPedido pedido;
}

// Interface completa (opcional)
public interface RepositorioPedidoCompleto : LeitorPedido, EscritorPedido {}

// Clientes podem depender apenas do que precisam
public class ServicoRelatorio {
  private LeitorPedido leitor {}

  void gerarRelatorio() {
    const pedidos = leitor.listar();
    // Gera relatório apenas lendo
  }
}

public class ServicoCriacaoPedido {
  private EscritorPedido escritor {}

  criarDadosPedido dados {
    const pedido = new Pedido(dados.id, dados.cliente, dados.itens);
    escritor.salvar(pedido);
  }
}



```

### Passo 5: Aplicar DIP (Dependency Inversion)

```csharp
// ✅ Inverter dependências

// Abstrações
public interface RepositorioPedido {
  salvarPedido pedido;
  Pedido buscarstring id?;
}

public interface ProcessadorPagamento {
  processardouble valor;
}

public interface ServicoNotificacao {
  notificarstring destino, string mensagem;
}

public interface CalculadoraDesconto {
  double calcularstring tipoCliente, double subtotal;
}

// Alto nível depende de abstrações
public class ServicoPedido {
  private RepositorioPedido repositorio,
    private ProcessadorPagamento pagamento,
    private ServicoNotificacao notificacao,
    private CalculadoraDesconto calculadoraDesconto
   {}

  Pedido processarPedidoDadosPedido dados {
    // Criar pedido
    const pedido = new Pedido(dados.id, dados.cliente, dados.itens);
    
    // Calcular total com desconto
    const subtotal = pedido.itens.Aggregate((sum, item) => sum + item.preco, 0);
    const desconto = calculadoraDesconto.calcular(dados.cliente, subtotal);
    pedido.total = subtotal - desconto;
    
    // Salvar
    repositorio.salvar(pedido);
    
    // Processar pagamento
    pagamento.processar(pedido.total);
    
    // Notificar
    notificacao.notificar(dados.cliente, `Pedido ${pedido.id} criado`);
    
    return pedido;
  }
}

// Implementações de baixo nível
public class RepositorioPedidoBD : RepositorioPedido {
  salvarPedido pedido {
    Console.WriteLine(`Salvando pedido ${pedido.id} no banco de dados...`);
  }

  Pedido buscarstring id? {
    return null;
  }
}

public class ProcessadorCartaoStripe : ProcessadorPagamento {
  processardouble valor {
    Console.WriteLine(`Processando ${valor} via Stripe`);
  }
}

public class ServicoEmail : ServicoNotificacao {
  notificarstring destino, string mensagem {
    Console.WriteLine`Enviando email para ${destino}: ${mensagem}`;
  }
}

public class CalculadoraDescontoPorTipo : CalculadoraDesconto {
  private Dictionary<string, EstrategiaDesconto> estrategias = new Map();

  public  {
    estrategias.set('VIP', new DescontoVIP());
    estrategias.set('Premium', new DescontoPremium());
    estrategias.set('Regular', new DescontoRegular());
  }

  double calcularstring tipoCliente, double subtotal {
    const estrategia = estrategias.get(tipoCliente) || new DescontoRegular();
    return estrategia.aplicar(subtotal);
  }
}



```

## 📕 Nível Avançado: Código Final Completo

```csharp
// ✅ Código final aplicando todos os princípios SOLID

// ======= DOMÍNIO (Alto Nível) =======

public class Item {
  public 
    public string nome { get; set; },
    public double preco { get; set; },
    public double quantidade { get; set; } = 1
   {}
}

public class Pedido {
  public 
    public string id { get; set; },
    public string cliente { get; set; },
    List<Item> public itens { get; set; },
    public double total { get; set; } = 0,
    public double desconto { get; set; } = 0,
    public string status { get; set; } = 'pendente'
   {}
}

// ======= ABSTRAÇÕES =======

public interface RepositorioPedido {
  Promise salvarPedido pedido<void>;
  Promise buscarstring id<Pedido?>;
  Promise listar()<List<Pedido>>;
}

public interface ProcessadorPagamento {
  Promise processardouble valor, string pedidoId<boolean>;
}

public interface ServicoNotificacao {
  Promise notificarstring destino, string assunto, string corpo<void>;
}

public interface EstrategiaDesconto {
  double aplicardouble subtotal;
}

// ======= IMPLEMENTAÇÕES DE BAIXO NÍVEL =======

public class RepositorioPedidoBD : RepositorioPedido {
  async Promise salvarPedido pedido<void> {
    Console.WriteLine(`[BD] Salvando pedido ${pedido.id}...`);
    // Implementação real
  }

  async Promise buscarstring id<Pedido?> {
    Console.WriteLine(`[BD] Buscando pedido ${id}...`);
    return null;
  }

  async Promise listar()<List<Pedido>> {
    Console.WriteLine(`[BD] Listando pedidos...`);
    return [];
  }
}

public class ProcessadorPagamentoStripe : ProcessadorPagamento {
  async Promise processardouble valor, string pedidoId<boolean> {
    Console.WriteLine(`[Stripe] Processando pagamento de ${valor} para pedido ${pedidoId}`);
    return true;
  }
}

public class ServicoEmailSMTP : ServicoNotificacao {
  async Promise notificarstring destino, string assunto, string corpo<void> {
    Console.WriteLine`[Email] Enviando para ${destino}: ${assunto}`;
  }
}

// ======= ESTRATÉGIAS DE DESCONTO =======

public class DescontoVIP : EstrategiaDesconto {
  double aplicardouble subtotal {
    return subtotal * 0.2;
  }
}

public class DescontoPremium : EstrategiaDesconto {
  double aplicardouble subtotal {
    return subtotal * 0.15;
  }
}

public class DescontoRegular : EstrategiaDesconto {
  double aplicardouble subtotal {
    return subtotal * 0.05;
  }
}

// ======= SERVIÇOS DE APLICAÇÃO (Alto Nível) =======

public class CalculadoraPedido {
  double calcularSubtotalPedido pedido {
    return pedido.itens.Aggregate(
      (sum, item) => sum + (item.preco * item.quantidade),
      0
    );
  }

  double aplicarDescontodouble subtotal, EstrategiaDesconto estrategia {
    return estrategia.aplicar(subtotal);
  }
}

public class ServicoPedido {
  private RepositorioPedido repositorio,
    private ProcessadorPagamento pagamento,
    private ServicoNotificacao notificacao,
    private CalculadoraPedido calculadora
   {}

  async Promise criarPedidoDadosPedido dados, string tipoCliente<Pedido> {
    // Criar pedido
    const pedido = new Pedido(dados.id, dados.cliente, dados.itens);

    // Calcular total
    const subtotal = calculadora.calcularSubtotal(pedido);
    const estrategia = obterEstrategiaDesconto(tipoCliente);
    const desconto = calculadora.aplicarDesconto(subtotal, estrategia);
    
    pedido.desconto = desconto;
    pedido.total = subtotal - desconto;

    // Persistir
    await repositorio.salvar(pedido);

    // Processar pagamento
    const pagamentoSucesso = await pagamento.processar(pedido.total, pedido.id);
    
    if (pagamentoSucesso) {
      pedido.status = 'pago';
      await repositorio.salvar(pedido);
      
      // Notificar
      await notificacao.notificar
        dados.cliente,
        'Pedido Confirmado',
        `Seu pedido ${pedido.id} foi confirmado! R Total$ ${pedido.total}`
      ;
    }

    return pedido;
  }

  private EstrategiaDesconto obterEstrategiaDescontostring tipoCliente {
    const estrategias: Record<string, EstrategiaDesconto> = {
      'VIP': new DescontoVIP(),
      'Premium': new DescontoPremium(),
      'Regular': new DescontoRegular()
    };
    return estrategias[tipoCliente] || new DescontoRegular();
  }
}

// ======= USO =======

async function void main() {
  // Configuração de dependências
  const repositorio = new RepositorioPedidoBD();
  const pagamento = new ProcessadorPagamentoStripe();
  const notificacao = new ServicoEmailSMTP();
  const calculadora = new CalculadoraPedido();

  // Criar serviço (alto nível)
  const servico = new ServicoPedido(
    repositorio,
    pagamento,
    notificacao,
    calculadora
  );

  // Processar pedido
  const pedido = await servico.criarPedido
    {
      id: '123',
      cliente: 'joao@email.com',
      itens: [
        new Item('Produto 1', 100, 2,
        new Item('Produto 2', 50, 1)
      ]
    },
    'VIP'
  );

  Console.WriteLine('Pedido criado:', pedido);
}

main();



```

## ✅ Benefícios da Refatoração

1. **SRP**: Cada classe tem uma responsabilidade única
2. **OCP**: Novos tipos de desconto/pagamento podem ser adicionados sem modificar código
3. **LSP**: Qualquer implementação de repositório pode ser substituída
4. **ISP**: Interfaces específicas permitem dependências mínimas
5. **DIP**: Alto nível não depende de baixo nível

## 🎯 Exercícios Práticos

1. Adicione um novo tipo de desconto (Black Friday)
2. Adicione um novo método de pagamento (PIX)
3. Crie uma implementação de repositório em memória para testes
4. Adicione um serviço de notificação via SMS
5. Refatore para usar um container IoC

## ✅ Checkpoint

### Auto-avaliação

- [ ] Entendo como aplicar todos os princípios SOLID juntos
- [ ] Consigo identificar violações em código real
- [ ] Sei refatorar código para seguir SOLID
- [ ] Compreendo como os princípios se complementam
- [ ] Posso aplicar SOLID em projetos reais

### Próximos Passos

- [Próximo: Anti-padrões →](./07-anti-padroes.md)
- [Voltar ao índice da trilha](./README.md)

---

[← Voltar ao índice principal](../../INDEX.md)
