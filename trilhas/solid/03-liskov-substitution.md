# 03 - Liskov Substitution Principle (LSP)

## 📘 Nível Básico

### O que é o Liskov Substitution Principle?

O **Liskov Substitution Principle (LSP)** ou **Princípio de Substituição de Liskov** afirma que:

> **Objetos de uma superclasse devem ser substituíveis por objetos de suas subclasses sem quebrar a aplicação.**

Em outras palavras, se você tem uma referência a uma classe base, deve ser capaz de substituí-la por qualquer uma de suas subclasses sem que o programa pare de funcionar corretamente.

### Definição Formal

Barbara Liskov definiu o princípio em 1987 no artigo "Data Abstraction and Hierarchy":

> Se para cada objeto o1 do tipo S há um objeto o2 do tipo T tal que para todos os programas P definidos em termos de T, o comportamento de P não muda quando o1 é substituído por o2, então S é um subtipo de T.

### Contexto Histórico

O Princípio de Substituição de Liskov foi introduzido por **Barbara Liskov** em 1987, durante uma conferência sobre programação orientada a objetos. Liskov, que recebeu o Prêmio Turing em 2008, estabeleceu este princípio como fundamental para garantir que classes derivadas possam substituir suas classes base sem quebrar o comportamento do programa. Este princípio é essencial para o polimorfismo confiável e é um dos pilares do design orientado a objetos.

### Exemplo Básico: Violação do LSP

```csharp
// ❌ VIOLAÇÃO: Retangulo não pode ser substituído por Quadrado

public class Retangulo {
  protected double largura;
  protected double altura;

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
  // Quadrado força largura e altura a serem iguais
  setLarguradouble largura {
    largura = largura;
    altura = largura; // ⚠️ Comportamento inesperado!
  }

  setAlturadouble altura {
    altura = altura;
    largura = altura; // ⚠️ Comportamento inesperado!
  }
}

// Problema: Código que espera Retangulo quebra com Quadrado
function testarRetanguloRetangulo retangulo {
  retangulo.setLargura(5);
  retangulo.setAltura(4);
  
  // Esperamos área = 20, mas com Quadrado será 16!
  Console.WriteLine(retangulo.calcularArea()); // Quebra expectativa
}




```

**Problemas:**
- `Quadrado` não pode ser substituído por `Retangulo` sem quebrar comportamento esperado
- Código cliente que depende de `Retangulo` não funciona corretamente com `Quadrado`
- Violação do contrato estabelecido pela classe base

### Exemplo Básico: Aplicando LSP

```csharp
// ✅ CORRETO: Ambas as classes podem ser usadas de forma intercambiável

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

  setLarguradouble largura {
    largura = largura;
  }

  setAlturadouble altura {
    altura = altura;
  }
}

public class Quadrado : Forma {
  private double lado {}

  double calcularArea() {
    return lado * lado;
  }

  setLadodouble lado {
    lado = lado;
  }
}

// Ambas podem ser usadas onde Forma é esperado
function double calcularAreaTotalList formas<Forma> {
  return formas.Sum(forma => forma.CalcularArea());
}

// Funciona com qualquer implementação de Forma
var retangulo = new Retangulo(5, 4);
var quadrado = new Quadrado(4);
calcularAreaTotal([retangulo, quadrado]); // ✅ Funciona corretamente




```

**Benefícios:**
- Qualquer implementação de `Forma` pode ser usada
- Comportamento esperado é mantido
- Código cliente não precisa conhecer implementações específicas

## 📗 Nível Intermediário

### Contratos e Pré/Pós-condições

LSP está relacionado ao conceito de **Design by Contract**:

- **Pré-condições**: Condições que devem ser verdadeiras antes de chamar um método
- **Pós-condições**: Condições que devem ser verdadeiras após chamar um método
- **Invariantes**: Condições que sempre devem ser verdadeiras

**Regra de LSP:**
- Subclasses podem **enfraquecer pré-condições** (aceitar mais casos)
- Subclasses podem **fortalecer pós-condições** (garantir mais)
- Subclasses devem **manter invariantes** da classe base

### Exemplo: Pré e Pós-condições

```csharp
// Classe base
public class ContaBancaria {
  protected double saldo = 0;

  // Pré-condição: valor > 0
  // Pós-condição: saldo aumenta
  depositardouble valor {
    if (valor <= 0) {
      throw new Exception('Valor deve ser positivo');
    }
    saldo += valor;
  }

  // Pré-condição: valor > 0 e valor <= saldo
  // Pós-condição: saldo diminui
  sacardouble valor {
    if (valor <= 0) {
      throw new Exception('Valor deve ser positivo');
    }
    if (valor > saldo) {
      throw new Exception('Saldo insuficiente');
    }
    saldo -= valor;
  }
}

// ✅ CORRETO: Mantém ou enfraquece pré-condições
public class ContaPoupanca : ContaBancaria {
  // Pode enfraquecer pré-condição (aceitar valores menores)
  sacardouble valor {
    if (valor <= 0) {
      throw new Exception('Valor deve ser positivo');
    }
    // Permite saque mesmo com saldo menor (com taxa)
    if (valor > saldo) {
      var taxa = (valor - saldo) * 0.1;
      saldo -= (valor + taxa);
    } else {
      saldo -= valor;
    }
  }
}

// ❌ ERRADO: Fortalece pré-condição (aceita menos casos)
public class ContaRestrita : ContaBancaria {
  sacardouble valor {
    if (valor <= 0) {
      throw new Exception('Valor deve ser positivo');
    }
    if (valor > saldo) {
      throw new Exception('Saldo insuficiente');
    }
    // ❌ Adiciona nova pré-condição
    if (valor > 1000) {
      throw new Exception('Valor máximo de saque é 1000');
    }
    saldo -= valor;
  }
}




```

### Casos de Uso Comuns

#### 1. Coleções e Iteradores

```csharp
// ✅ CORRETO: Todas as coleções podem ser substituídas
public interface Colecao<T> {
  adicionarT item;
  removerT item;
  double tamanho();
}

public class Lista<T> : Colecao<T> {
  private List<itens> T = new List<itens>();

  adicionarT item {
    itens.Add(item);
  }

  removerT item {
    var index = itens.indexOf(item);
    if (index > -1) {
      itens.splice(index, 1);
    }
  }

  double tamanho() {
    return itens.length;
  }
}

public class Conjunto<T> : Colecao<T> {
  private HashHashSet<T> itens = new Set();

  adicionarT item {
    itens.add(item);
  }

  removerT item {
    itens.delete(item);
  }

  double tamanho() {
    return itens.size;
  }
}

// Funciona com qualquer implementação
function processarColecao<T>Colecao colecao<T> {
  // Código funciona independente da implementação
}




```

#### 2. Repositórios

```csharp
public interface Repositorio<T> {
  T buscarstring id?;
  salvarT entidade;
  deletarstring id;
}

public class RepositorioMemoria<T> : Repositorio<T> {
  private Dictionary<string, T> dados = new Map();

  T buscarstring id? {
    return dados[id] || null;
  }

  salvarT entidade {
    dados['id'] = entidade;
  }

  deletarstring id {
    dados.delete(id);
  }
}

public class RepositorioBancoDados<T> : Repositorio<T> {
  T buscarstring id? {
    // Implementação com banco de dados
    return null;
  }

  salvarT entidade {
    // Implementação com banco de dados
  }

  deletarstring id {
    // Implementação com banco de dados
  }
}

// Qualquer repositório pode ser usado
public class Servico<T> {
  private Repositorio repositorio<T> {}

  processarstring id {
    var entidade = repositorio.buscar(id);
    // Funciona com qualquer repositório
  }
}




```

### Benefícios Práticos

1. **Polimorfismo confiável**: Subclasses podem ser usadas onde a classe base é esperada
2. **Testabilidade**: Fácil criar mocks e stubs que seguem o mesmo contrato
3. **Extensibilidade**: Novas subclasses podem ser adicionadas sem quebrar código existente
4. **Manutenibilidade**: Código cliente não precisa conhecer implementações específicas

## 📕 Nível Avançado

### LSP e Exceções

Subclasses não devem lançar exceções que a classe base não lança:

```csharp
// Classe base
public class LeitorArquivo {
  string lerstring caminho {
    // Pode lançar FileNotFoundError
    return 'conteudo';
  }
}

// ❌ ERRADO: Lança exceção não esperada
public class LeitorArquivoSeguro : LeitorArquivo {
  string lerstring caminho {
    if (!caminho) {
      throw new Exception('Caminho inválido'); // Nova exceção!
    }
    return super.ler(caminho);
  }
}

// ✅ CORRETO: Não adiciona novas exceções
public class LeitorArquivoSeguro : LeitorArquivo {
  string lerstring caminho {
    if (!caminho) {
      return ''; // Retorna valor padrão ao invés de lançar exceção
    }
    return super.ler(caminho);
  }
}




```

### LSP e Valores de Retorno

Subclasses podem retornar tipos mais específicos (covariância):

```csharp
// ✅ CORRETO: Retorno mais específico é permitido
public class Animal {
  Animal void criar() {
    return new Animal();
  }
}

public class Cachorro : Animal {
  Cachorro void criar() { // Tipo mais específico
    return new Cachorro();
  }
}

// ❌ ERRADO: Retorno menos específico quebra LSP
public class Animal {
  Cachorro void criar() {
    return new Cachorro();
  }
}

public class Gato : Animal {
  Animal void criar() { // Tipo menos específico - quebra contrato
    return new Gato();
  }
}




```

### LSP e Parâmetros

Parâmetros devem aceitar tipos mais gerais (contravariância):

```csharp
// ✅ CORRETO: Aceita tipo mais geral
public class ProcessadorAnimal {
  processarAnimal animal {
    // Processa qualquer animal
  }
}

public class ProcessadorCachorro : ProcessadorAnimal {
  processarAnimal animal { // Aceita tipo mais geral
    if (animal instanceof Cachorro) {
      // Processa cachorro
    }
  }
}




```

### LSP e História

LSP também se aplica ao **histórico de mudanças**:

```csharp
// ❌ ERRADO: Muda comportamento histórico
public class Contador {
  private double valor = 0;

  void incrementar() {
    valor++;
  }

  double obterValor() {
    return valor;
  }
}

public class ContadorLimitado : Contador {
  private double limite = 10;

  void incrementar() {
    // ❌ Quebra: não incrementa além do limite
    if (obterValor() < limite) {
      super.incrementar();
    }
  }
}

// Código cliente espera que sempre incremente
function testarContadorContador contador {
  var valorInicial = contador.obterValor();
  contador.incrementar();
  var valorFinal = contador.obterValor();
  
  // Espera: valorFinal > valorInicial
  // Mas com ContadorLimitado pode não ser verdade!
}




```

### LSP e Outros Princípios SOLID

- **LSP → OCP**: Substituição correta permite extensão sem modificação
- **LSP → ISP**: Interfaces que podem ser substituídas são mais específicas
- **LSP → DIP**: Dependências em abstrações garantem substituição

### Testes e LSP

LSP facilita testes:

```csharp
// Interface
public interface RepositorioUsuario {
  Usuario buscarstring id?;
}

// Implementação real
public class RepositorioUsuarioBD : RepositorioUsuario {
  Usuario buscarstring id? {
    // Busca no banco de dados
    return null;
  }
}

// Mock para testes (segue LSP)
public class RepositorioUsuarioMock : RepositorioUsuario {
  private Dictionary<string, Usuario> usuarios = new Map();

  Usuario buscarstring id? {
    return usuarios[id] || null;
  }
}

// Testes podem usar mock sem modificar código
function testarServicoRepositorioUsuario repositorio {
  var servico = new ServicoUsuario(repositorio);
  // Testes funcionam com qualquer implementação
}




```

## ⚠️ Armadilhas Comuns

1. **Herança "é um" vs "tem um"**: Nem sempre herança é a solução
2. **Violar contratos**: Subclasses devem manter contratos da classe base
3. **Lançar exceções inesperadas**: Não adicione novas exceções
4. **Mudar comportamento esperado**: Comportamento deve ser consistente

## ✅ Checkpoint

### Auto-avaliação

- [ ] Entendo o que é Liskov Substitution Principle
- [ ] Consigo identificar violações do LSP
- [ ] Entendo pré-condições, pós-condições e invariantes
- [ ] Sei como garantir que subclasses sejam substituíveis
- [ ] Compreendo a relação entre LSP e polimorfismo

### Exercícios

Pratique com os exercícios do módulo em [exercicios/](./exercicios/README.md).

## 🔗 Próximos Passos

- [Próximo: Interface Segregation Principle →](./04-interface-segregation.md)
- [Voltar ao índice da trilha](./README.md)

---

[← Voltar ao índice principal](../../INDEX.md)
