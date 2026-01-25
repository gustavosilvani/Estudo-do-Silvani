# 01 - Orientação a Objetos em C#

## 📖 O que é Orientação a Objetos?

Programação Orientada a Objetos (OOP) é um paradigma que organiza código em torno de "objetos" que contêm dados (propriedades) e comportamentos (métodos).

## 🎯 Os 4 Pilares da OOP

### 1. **Encapsulamento**
Esconder detalhes internos e expor apenas o necessário.

### 2. **Herança**
Criar classes baseadas em outras, reutilizando código.

### 3. **Polimorfismo**
Objetos de classes diferentes responderem à mesma interface.

### 4. **Abstração**
Focar no essencial, escondendo complexidade.

## 📦 Classes e Objetos

### Definindo uma Classe

```csharp
public class Pessoa {
    // Campos (dados privados)
    private string _nome;
    private int _idade;

    // Propriedades (acesso controlado)
    public string Nome {
        get { return _nome; }
        set { _nome = value; }
    }

    // Propriedade auto-implementada (forma curta)
    public int Idade { get; set; }

    // Construtor
    public Pessoa(string nome, int idade) {
        Nome = nome;
        Idade = idade;
    }

    // Método
    public void ApresentarSe() {
        Console.WriteLine($"Olá, sou {Nome} e tenho {Idade} anos");
    }
}
```

### Criando e Usando Objetos

```csharp
// Criar objeto
Pessoa pessoa1 = new Pessoa("João", 30);

// Usar propriedades
Console.WriteLine(pessoa1.Nome);  // João
pessoa1.Idade = 31;

// Chamar método
pessoa1.ApresentarSe();  // Olá, sou João e tenho 31 anos

// Sintaxe moderna (C# 9+)
Pessoa pessoa2 = new("Maria", 25);
```

## 🔐 Encapsulamento

### Modificadores de Acesso

```csharp
public class ContaBancaria {
    // public - Acessível de qualquer lugar
    public string Titular { get; set; }

    // private - Apenas dentro da classe
    private decimal _saldo;

    // protected - Classe e classes derivadas
    protected string _senha;

    // internal - Mesmo assembly
    internal string NumeroConta { get; set; }

    // protected internal - Combinação
    protected internal DateTime DataCriacao { get; set; }

    // Propriedade com validação
    public decimal Saldo {
        get { return _saldo; }
        private set { _saldo = value; }  // Setter privado
    }

    public void Depositar(decimal valor) {
        if (valor > 0) {
            _saldo += valor;
        }
    }

    public bool Sacar(decimal valor) {
        if (valor > 0 && valor <= _saldo) {
            _saldo -= valor;
            return true;
        }
        return false;
    }
}
```

### Propriedades Somente Leitura

```csharp
public class Produto {
    // Somente leitura (init-only property)
    public string Nome { get; init; }
    public decimal Preco { get; init; }

    // Readonly field
    private readonly DateTime _dataCriacao;

    public Produto(string nome, decimal preco) {
        Nome = nome;
        Preco = preco;
        _dataCriacao = DateTime.Now;
    }
}

// Uso
var produto = new Produto("Notebook", 3500) {
    Nome = "Notebook Gamer"  // OK no init
};

// produto.Nome = "Outro";  // ERRO após criação
```

## 🧬 Herança

### Classe Base e Derivada

```csharp
// Classe base
public class Animal {
    public string Nome { get; set; }
    public int Idade { get; set; }

    public virtual void FazerSom() {
        Console.WriteLine("Algum som...");
    }

    public void Dormir() {
        Console.WriteLine($"{Nome} está dormindo");
    }
}

// Classe derivada
public class Cachorro : Animal {
    public string Raca { get; set; }

    // Override - sobrescrever método
    public override void FazerSom() {
        Console.WriteLine("Au au!");
    }

    // Novo método
    public void Latir() {
        Console.WriteLine("Latindo...");
    }
}

public class Gato : Animal {
    public override void FazerSom() {
        Console.WriteLine("Miau!");
    }
}
```

### Usando Herança

```csharp
Cachorro cachorro = new Cachorro {
    Nome = "Rex",
    Idade = 3,
    Raca = "Labrador"
};

cachorro.FazerSom();  // Au au!
cachorro.Dormir();     // Rex está dormindo
cachorro.Latir();      // Latindo...

Gato gato = new Gato {
    Nome = "Mimi",
    Idade = 2
};

gato.FazerSom();  // Miau!
```

### Palavra-chave base

```csharp
public class Funcionario {
    public string Nome { get; set; }
    public decimal SalarioBase { get; set; }

    public virtual decimal CalcularSalario() {
        return SalarioBase;
    }
}

public class Gerente : Funcionario {
    public decimal Bonus { get; set; }

    public override decimal CalcularSalario() {
        // Chamar método da classe base
        return base.CalcularSalario() + Bonus;
    }
}
```

## 🎭 Polimorfismo

### Polimorfismo em Ação

```csharp
public class FormaGeometrica {
    public virtual double CalcularArea() {
        return 0;
    }
}

public class Circulo : FormaGeometrica {
    public double Raio { get; set; }

    public override double CalcularArea() {
        return Math.PI * Raio * Raio;
    }
}

public class Retangulo : FormaGeometrica {
    public double Largura { get; set; }
    public double Altura { get; set; }

    public override double CalcularArea() {
        return Largura * Altura;
    }
}

// Usando polimorfismo
List<FormaGeometrica> formas = new List<FormaGeometrica> {
    new Circulo { Raio = 5 },
    new Retangulo { Largura = 4, Altura = 6 },
    new Circulo { Raio = 3 }
};

foreach (var forma in formas) {
    Console.WriteLine($"Área: {forma.CalcularArea():F2}");
}
```

## 🎨 Abstração

### Classes Abstratas

```csharp
public abstract class Veiculo {
    public string Marca { get; set; }
    public string Modelo { get; set; }

    // Método abstrato (sem implementação)
    public abstract void Acelerar();

    // Método concreto
    public void Buzinar() {
        Console.WriteLine("Beep beep!");
    }
}

public class Carro : Veiculo {
    // Obrigado a implementar
    public override void Acelerar() {
        Console.WriteLine("Carro acelerando...");
    }
}

public class Moto : Veiculo {
    public override void Acelerar() {
        Console.WriteLine("Moto acelerando rápido!");
    }
}

// Uso
// Veiculo v = new Veiculo();  // ERRO - não pode instanciar classe abstrata
Veiculo carro = new Carro { Marca = "Ford", Modelo = "Fiesta" };
carro.Acelerar();  // Carro acelerando...
```

### Interfaces

```csharp
public interface IRepositorio<T> {
    void Adicionar(T item);
    void Remover(int id);
    T BuscarPorId(int id);
    List<T> ListarTodos();
}

public class RepositorioProduto : IRepositorio<Produto> {
    private List<Produto> _produtos = new List<Produto>();

    public void Adicionar(Produto item) {
        _produtos.Add(item);
    }

    public void Remover(int id) {
        var produto = _produtos.FirstOrDefault(p => p.Id == id);
        if (produto != null) {
            _produtos.Remove(produto);
        }
    }

    public Produto BuscarPorId(int id) {
        return _produtos.FirstOrDefault(p => p.Id == id);
    }

    public List<Produto> ListarTodos() {
        return _produtos;
    }
}
```

## 🏗️ Construtores

### Tipos de Construtores

```csharp
public class Cliente {
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Telefone { get; set; }

    // Construtor padrão
    public Cliente() {
        Console.WriteLine("Cliente criado");
    }

    // Construtor com parâmetros
    public Cliente(string nome) {
        Nome = nome;
    }

    // Construtor completo
    public Cliente(string nome, string email, string telefone) {
        Nome = nome;
        Email = email;
        Telefone = telefone;
    }

    // Construtor chamando outro (chaining)
    public Cliente(string nome, string email) : this(nome, email, null) {
    }
}
```

## 🎯 Exercício Prático

### Sistema de Biblioteca

```csharp
// Classe base
public abstract class ItemBiblioteca {
    public string Titulo { get; set; }
    public string Codigo { get; set; }
    public bool Disponivel { get; set; } = true;

    public abstract string ObterInformacoes();

    public void Emprestar() {
        if (Disponivel) {
            Disponivel = false;
            Console.WriteLine($"{Titulo} emprestado com sucesso");
        } else {
            Console.WriteLine($"{Titulo} não disponível");
        }
    }

    public void Devolver() {
        Disponivel = true;
        Console.WriteLine($"{Titulo} devolvido");
    }
}

// Classes derivadas
public class Livro : ItemBiblioteca {
    public string Autor { get; set; }
    public int NumeroPaginas { get; set; }

    public override string ObterInformacoes() {
        return $"Livro: {Titulo} - Autor: {Autor} - Páginas: {NumeroPaginas}";
    }
}

public class Revista : ItemBiblioteca {
    public int Edicao { get; set; }
    public string Mes { get; set; }

    public override string ObterInformacoes() {
        return $"Revista: {Titulo} - Edição: {Edicao} - Mês: {Mes}";
    }
}

// Uso
var livro = new Livro {
    Titulo = "Clean Code",
    Autor = "Robert Martin",
    NumeroPaginas = 464,
    Codigo = "L001"
};

var revista = new Revista {
    Titulo = "Tech Magazine",
    Edicao = 123,
    Mes = "Janeiro",
    Codigo = "R001"
};

Console.WriteLine(livro.ObterInformacoes());
livro.Emprestar();
livro.Devolver();
```

## ✅ Verificação

Para prosseguir, certifique-se de:

- [ ] Criou pelo menos 3 classes próprias
- [ ] Implementou encapsulamento com propriedades
- [ ] Usou herança entre classes
- [ ] Implementou polimorfismo
- [ ] Criou pelo menos 1 classe abstrata
- [ ] Implementou 1 interface

## 🎯 Próximos Passos

No próximo módulo, vamos aprender sobre **Collections e LINQ** - manipulação avançada de dados!

---

[← Voltar: Introdução](./00-introducao.md) | [Próximo: Collections e LINQ →](./02-collections-linq.md)