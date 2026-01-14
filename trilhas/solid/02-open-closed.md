# 02 - Open/Closed Principle (OCP)

## 📘 Nível Básico

### O que é o Open/Closed Principle?

O **Open/Closed Principle (OCP)** ou **Princípio Aberto/Fechado** afirma que:

> **Entidades de software devem estar abertas para extensão, mas fechadas para modificação.**

Em outras palavras, você deve ser capaz de adicionar novas funcionalidades sem modificar código existente.

### Definição Formal

Bertrand Meyer, que cunhou o termo em 1988 no livro "Object-Oriented Software Construction", definiu que módulos devem ser:
- **Abertos para extensão**: Novas funcionalidades podem ser adicionadas
- **Fechados para modificação**: Código existente não precisa ser alterado

### Contexto Histórico

O Princípio Aberto/Fechado foi introduzido por **Bertrand Meyer** em 1988, como parte de sua obra "Object-Oriented Software Construction". Meyer também foi pioneiro na ideia de **Programação por Contrato**, estabelecendo bases para design de software mais robusto e extensível. O OCP é um dos princípios fundamentais que influenciou profundamente o desenvolvimento de software orientado a objetos moderno.

### Exemplo Básico: Violação do OCP

```csharp
// ❌ VIOLAÇÃO: Precisamos modificar a classe para adicionar novos tipos
public class CalculadoraDesconto
{
    public double CalcularDesconto(double preco, string tipoCliente) {
        if (tipoCliente == "VIP")
        {
            return preco * 0.2; // 20% de desconto
        }
        else if (tipoCliente == "Premium")
        {
            return preco * 0.15; // 15% de desconto
        }
        else if (tipoCliente == "Regular")
        {
            return preco * 0.05; // 5% de desconto
        }
        return 0;
    }
}

// Problema: Para adicionar um novo tipo ex: 'Gold', precisamos MODIFICAR a classe



```

**Problemas:**
- Cada novo tipo requer modificação da classe
- Risco de quebrar código existente
- Violação do princípio "fechado para modificação"

### Exemplo Básico: Aplicando OCP

```csharp
// ✅ CORRETO: Aberto para extensão, fechado para modificação

// Interface/Contrato
public interface IEstrategiaDesconto
{
    double Calcular(double preco);
}

// Implementações específicas
public class DescontoVIP : IEstrategiaDesconto
{
    public double Calcular(double preco) {
        return preco * 0.2;
    }
}

public class DescontoPremium : IEstrategiaDesconto
{
    public double Calcular(double preco) {
        return preco * 0.15;
    }
}

public class DescontoRegular : IEstrategiaDesconto
{
    public double Calcular(double preco) {
        return preco * 0.05;
    }
}

// Classe fechada para modificação
public class CalculadoraDesconto
{
    public double CalcularDesconto(double preco, IEstrategiaDesconto estrategia) {
        return estrategia.Calcular(preco);
    }
}

// Para adicionar novo tipo, apenas criamos nova classe (EXTENSÃO, não MODIFICAÇÃO)
public class DescontoGold : IEstrategiaDesconto
{
    public double Calcular(double preco) {
        return preco * 0.25;
    }
}



```

**Benefícios:**
- Novos tipos podem ser adicionados sem modificar código existente
- Código existente permanece intacto
- Menor risco de introduzir bugs

## 📗 Nível Intermediário

### Mecanismos de Extensão

Existem várias formas de aplicar OCP:

#### 1. Herança (Menos Preferível)

```csharp
// Classe base fechada para modificação
abstract public class Forma {
  abstract double calcularArea();
}

// Extensões sem modificar a classe base
public class Retangulo : Forma {
  private double largura, private double altura {
    super();
  }
  
  double calcularArea() {
    return largura * altura;
  }
}

public class Circulo : Forma {
  private double raio {
    super();
  }
  
  double calcularArea() {
    return Math.PI * raio * raio;
  }
}



```

#### 2. Composição e Interfaces (Preferível)

```csharp
// Interface define contrato
public interface ProcessadorPagamento {
  processardouble valor;
}

// Implementações específicas
public class ProcessadorCartao : ProcessadorPagamento {
  processardouble valor {
    Console.WriteLine(`Processando ${valor} via cartão`);
  }
}

public class ProcessadorBoleto : ProcessadorPagamento {
  processardouble valor {
    Console.WriteLine(`Processando ${valor} via boleto`);
  }
}

// Classe que usa processadores (fechada para modificação)
public class ServicoPagamento {
  processarPagamentodouble valor, ProcessadorPagamento processador {
    processador.processar(valor);
  }
}

// Nova forma de pagamento: apenas criar nova classe
public class ProcessadorPix : ProcessadorPagamento {
  processardouble valor {
    Console.WriteLine(`Processando ${valor} via PIX`);
  }
}



```

### Casos de Uso Comuns

#### 1. Filtros e Validações

```csharp
// ❌ Antes
public class FiltroProdutos {
  Produto filtrarPorCategoriaList produtos<Produto>, List<categoria> string {
    return produtos.Where(p => p.categoria == categoria);
  }
  
  Produto filtrarPorPrecoList produtos<Produto>, List<precoMax> double {
    return produtos.Where(p => p.preco <= precoMax);
  }
  
  // Para adicionar novo filtro, precisamos modificar a classe
}

// ✅ Depois
public interface Filtro {
  Produto aplicarList produtos<Produto>[];
}

public class FiltroCategoria : Filtro {
  private string categoria {}
  
  Produto aplicarList produtos<Produto>[] {
    return produtos.Where(p => p.categoria == categoria);
  }
}

public class FiltroPreco : Filtro {
  private double precoMax {}
  
  Produto aplicarList produtos<Produto>[] {
    return produtos.Where(p => p.preco <= precoMax);
  }
}

// Novos filtros podem ser adicionados sem modificar código existente
public class FiltroEstoque : Filtro {
  Produto aplicarList produtos<Produto>[] {
    return produtos.Where(p => p.estoque > 0);
  }
}



```

#### 2. Processadores e Transformadores

```csharp
public interface TransformadorTexto {
  string transformarstring texto;
}

public class Maiusculas : TransformadorTexto {
  string transformarstring texto {
    return texto.toUpperCase();
  }
}

public class Minusculas : TransformadorTexto {
  string transformarstring texto {
    return texto.toLowerCase();
  }
}

public class RemoverEspacos : TransformadorTexto {
  string transformarstring texto {
    return texto.replace(/\s/g, '');
  }
}

// Pipeline de transformações (fechado para modificação)
public class ProcessadorTexto {
  string processarstring texto, List transformadores<TransformadorTexto> {
    return transformadores.Aggregate(
      (resultado, transformador) => transformador.transformar(resultado),
      texto
    );
  }
}



```

### Benefícios Práticos

1. **Estabilidade**: Código existente não muda, reduzindo bugs
2. **Extensibilidade**: Fácil adicionar novas funcionalidades
3. **Testabilidade**: Código existente já testado não precisa ser retestado
4. **Colaboração**: Diferentes desenvolvedores podem trabalhar em extensões sem conflitos

### Quando Aplicar OCP

Aplique OCP quando:

- Você sabe que novos tipos/variantes serão adicionados
- Modificar código existente é arriscado ou caro
- Você quer manter código estável e testado intacto
- Múltiplas pessoas podem precisar adicionar funcionalidades

## 📕 Nível Avançado

### OCP e Strategy Pattern

OCP é frequentemente implementado usando o **Strategy Pattern**:

```csharp
// Context (usa estratégia)
public class Contexto {
  private Estrategia estrategia {}
  
  void executar() {
    estrategia.executar();
  }
  
  // Pode trocar estratégia em runtime
  setEstrategiaEstrategia estrategia {
    estrategia = estrategia;
  }
}

// Strategy interface
public interface Estrategia {
  executar();
}

// Concrete strategies (extensões)
public class EstrategiaA : Estrategia {
  void executar() {
    Console.WriteLine('Executando estratégia A');
  }
}

public class EstrategiaB : Estrategia {
  void executar() {
    Console.WriteLine('Executando estratégia B');
  }
}



```

### OCP e Template Method Pattern

```csharp
// Template method (fechado para modificação)
abstract public class ProcessadorDocumento {
  // Método template - define o algoritmo
  void processar() {
    validar();
    transformar();
    salvar();
  }
  
  // Hook methods (abertos para extensão)
  protected abstract validar();
  protected abstract transformar();
  protected abstract salvar();
}

// Extensões sem modificar o template
public class ProcessadorPDF : ProcessadorDocumento {
  protected void validar() {
    Console.WriteLine('Validando PDF');
  }
  
  protected void transformar() {
    Console.WriteLine('Transformando PDF');
  }
  
  protected void salvar() {
    Console.WriteLine('Salvando PDF');
  }
}



```

### OCP em Arquitetura

OCP também se aplica a nível arquitetural:

```csharp
// Camada de aplicação (fechada para modificação)
public class ServicoPedido {
  private RepositorioPedido repositorio,
    private Notificador notificador
   {}
  
  criarPedidoDadosPedido dados {
    const pedido = new Pedido(dados);
    repositorio.salvar(pedido);
    notificador.notificar(pedido);
  }
}

// Extensões podem ser feitas via plugins/módulos
public interface PluginPedido {
  antesDeSalvarPedido pedido;
  depoisDeSalvarPedido pedido;
}

// Sistema pode registrar plugins sem modificar ServicoPedido



```

### OCP e Dependency Injection

Dependency Injection facilita OCP:

```csharp
// Classe fechada para modificação
public class ServicoEmail {
  private ProvedorEmail provedor {}
  
  enviarstring destino, string mensagem {
    provedor.enviar(destino, mensagem);
  }
}

// Diferentes implementações podem ser injetadas
public interface ProvedorEmail {
  enviarstring destino, string mensagem;
}

public class ProvedorSMTP : ProvedorEmail {
  enviarstring destino, string mensagem {
    // Implementação SMTP
  }
}

public class ProvedorSendGrid : ProvedorEmail {
  enviarstring destino, string mensagem {
    // Implementação SendGrid
  }
}



```

### Armadilhas Comuns

#### 1. Over-abstraction

```csharp
// ❌ Não crie abstrações para coisas que nunca vão mudar
public interface Calculadora {
  double somardouble a, double b;
}

// ✅ Se é simples e não vai mudar, mantenha simples
function double somardouble a, double b {
  return a + b;
}



```

#### 2. Premature Abstraction

Não crie abstrações "por precaução". Aplique OCP quando:
- Você tem evidência de que extensões serão necessárias
- Você já precisa de múltiplas implementações
- Modificações frequentes estão causando problemas

#### 3. Confundir Extensão com Modificação

```csharp
// ❌ Isso ainda é modificação
public class Calculadora {
  double calculardouble a, double b, string operacao {
    if (operacao == 'soma') return a + b;
    if (operacao == 'subtracao') return a - b;
    // Adicionar nova operação = MODIFICAÇÃO
  }
}

// ✅ Isso é extensão
public interface Operacao {
  double executardouble a, double b;
}

public class Soma : Operacao {
  double executardouble a, double b {
    return a + b;
  }
}



```

### OCP e Outros Princípios SOLID

- **SRP → OCP**: Responsabilidades únicas facilitam extensão sem modificação
- **OCP → LSP**: Extensões devem ser substituíveis
- **OCP → DIP**: Depender de abstrações permite extensão

## ⚠️ Armadilhas Comuns

1. **Criar abstrações desnecessárias**: Não abstraia coisas que não vão mudar
2. **Aplicar prematuramente**: Espere evidência de necessidade de extensão
3. **Confundir com herança**: Composição geralmente é melhor que herança
4. **Ignorar complexidade**: Abstrações têm custo, use com sabedoria

## ✅ Checkpoint

### Auto-avaliação

- [ ] Entendo o que é Open/Closed Principle
- [ ] Consigo identificar violações do OCP
- [ ] Sei como aplicar OCP usando interfaces e composição
- [ ] Entendo a diferença entre extensão e modificação
- [ ] Compreendo quando aplicar OCP e quando não aplicar

### Exercícios

Pratique com os exercícios do módulo em [exercicios/](./exercicios/README.md).

## 🔗 Próximos Passos

- [Próximo: Liskov Substitution Principle →](./03-liskov-substitution.md)
- [Voltar ao índice da trilha](./README.md)

---

[← Voltar ao índice principal](../../INDEX.md)
