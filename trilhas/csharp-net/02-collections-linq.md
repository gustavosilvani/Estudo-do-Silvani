# 02 - Collections e LINQ

## 📖 Collections em C#

Collections são estruturas de dados para armazenar e manipular grupos de objetos.

## 📦 Tipos de Collections

### 1. **List<T>** - Lista dinâmica

```csharp
// Criar lista
List<string> nomes = new List<string>();

// Adicionar elementos
nomes.Add("João");
nomes.Add("Maria");
nomes.AddRange(new[] { "Pedro", "Ana" });

// Acessar elementos
string primeiro = nomes[0];  // João
string ultimo = nomes[nomes.Count - 1];  // Ana

// Remover elementos
nomes.Remove("João");
nomes.RemoveAt(0);
nomes.Clear();

// Verificar existência
bool existe = nomes.Contains("Maria");

// Inicialização inline
List<int> numeros = new List<int> { 1, 2, 3, 4, 5 };
```

### 2. **Dictionary<TKey, TValue>** - Chave-Valor

```csharp
// Criar dicionário
Dictionary<string, int> idades = new Dictionary<string, int>();

// Adicionar
idades.Add("João", 30);
idades["Maria"] = 25;  // Adiciona ou atualiza

// Acessar
int idadeJoao = idades["João"];

// Verificar chave
if (idades.ContainsKey("Pedro")) {
    Console.WriteLine(idades["Pedro"]);
}

// Tentar obter valor
if (idades.TryGetValue("Ana", out int idadeAna)) {
    Console.WriteLine($"Ana tem {idadeAna} anos");
}

// Iterar
foreach (var kvp in idades) {
    Console.WriteLine($"{kvp.Key}: {kvp.Value}");
}

// Inicialização
var produtos = new Dictionary<int, string> {
    { 1, "Notebook" },
    { 2, "Mouse" },
    { 3, "Teclado" }
};
```

### 3. **HashSet<T>** - Conjunto único

```csharp
// Criar conjunto (sem duplicatas)
HashSet<string> emails = new HashSet<string>();

// Adicionar
emails.Add("joao@email.com");
emails.Add("maria@email.com");
emails.Add("joao@email.com");  // Ignorado - já existe

// Operações de conjunto
HashSet<int> A = new HashSet<int> { 1, 2, 3, 4 };
HashSet<int> B = new HashSet<int> { 3, 4, 5, 6 };

// União
A.UnionWith(B);  // { 1, 2, 3, 4, 5, 6 }

// Interseção
A.IntersectWith(B);  // { 3, 4 }

// Diferença
A.ExceptWith(B);  // { 1, 2 }
```

### 4. **Queue<T>** - Fila (FIFO)

```csharp
Queue<string> fila = new Queue<string>();

// Adicionar no final
fila.Enqueue("Primeiro");
fila.Enqueue("Segundo");
fila.Enqueue("Terceiro");

// Remover do início
string atendido = fila.Dequeue();  // "Primeiro"

// Ver próximo sem remover
string proximo = fila.Peek();  // "Segundo"

// Verificar quantidade
int quantidade = fila.Count;
```

### 5. **Stack<T>** - Pilha (LIFO)

```csharp
Stack<string> pilha = new Stack<string>();

// Adicionar no topo
pilha.Push("Base");
pilha.Push("Meio");
pilha.Push("Topo");

// Remover do topo
string removido = pilha.Pop();  // "Topo"

// Ver topo sem remover
string topo = pilha.Peek();  // "Meio"
```

## 🔍 LINQ - Language Integrated Query

LINQ permite consultar coleções de forma expressiva e poderosa.

### Operadores Básicos

```csharp
List<int> numeros = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

// Where - Filtrar
var pares = numeros.Where(n => n % 2 == 0);
// Result: { 2, 4, 6, 8, 10 }

// Select - Projetar/Transformar
var quadrados = numeros.Select(n => n * n);
// Result: { 1, 4, 9, 16, 25, 36, 49, 64, 81, 100 }

// OrderBy - Ordenar
var ordenados = numeros.OrderByDescending(n => n);
// Result: { 10, 9, 8, 7, 6, 5, 4, 3, 2, 1 }

// First / FirstOrDefault
int primeiro = numeros.First();  // 1
int primeiroMaiorQue5 = numeros.First(n => n > 5);  // 6
int? inexistente = numeros.FirstOrDefault(n => n > 100);  // null

// Any / All
bool temPares = numeros.Any(n => n % 2 == 0);  // true
bool todosMaiorQueZero = numeros.All(n => n > 0);  // true

// Count
int quantidade = numeros.Count();
int quantidadePares = numeros.Count(n => n % 2 == 0);  // 5

// Sum, Average, Min, Max
int soma = numeros.Sum();  // 55
double media = numeros.Average();  // 5.5
int minimo = numeros.Min();  // 1
int maximo = numeros.Max();  // 10

// Take / Skip
var primeiros3 = numeros.Take(3);  // { 1, 2, 3 }
var pular3 = numeros.Skip(3);  // { 4, 5, 6, 7, 8, 9, 10 }

// Distinct - Remover duplicatas
var comDuplicatas = new List<int> { 1, 2, 2, 3, 3, 3 };
var unicos = comDuplicatas.Distinct();  // { 1, 2, 3 }
```

### Consultas Complexas

```csharp
public class Produto {
    public int Id { get; set; }
    public string Nome { get; set; }
    public decimal Preco { get; set; }
    public string Categoria { get; set; }
    public int EstoqueAtual { get; set; }
}

List<Produto> produtos = new List<Produto> {
    new Produto { Id = 1, Nome = "Notebook", Preco = 3500, Categoria = "Informática", EstoqueAtual = 10 },
    new Produto { Id = 2, Nome = "Mouse", Preco = 50, Categoria = "Informática", EstoqueAtual = 50 },
    new Produto { Id = 3, Nome = "Cadeira", Preco = 800, Categoria = "Móveis", EstoqueAtual = 5 },
    new Produto { Id = 4, Nome = "Mesa", Preco = 1200, Categoria = "Móveis", EstoqueAtual = 3 },
    new Produto { Id = 5, Nome = "Teclado", Preco = 150, Categoria = "Informática", EstoqueAtual = 30 }
};

// Produtos de informática com preço < 200
var produtosBaratos = produtos
    .Where(p => p.Categoria == "Informática" && p.Preco < 200)
    .OrderBy(p => p.Preco);

// Agrupar por categoria
var porCategoria = produtos
    .GroupBy(p => p.Categoria)
    .Select(g => new {
        Categoria = g.Key,
        Quantidade = g.Count(),
        PrecoMedio = g.Average(p => p.Preco)
    });

foreach (var grupo in porCategoria) {
    Console.WriteLine($"{grupo.Categoria}: {grupo.Quantidade} produtos, média R$ {grupo.PrecoMedio:F2}");
}

// Join com outra lista
List<Pedido> pedidos = new List<Pedido> {
    new Pedido { ProdutoId = 1, Quantidade = 2 },
    new Pedido { ProdutoId = 2, Quantidade = 5 }
};

var pedidosComDetalhes = pedidos
    .Join(produtos,
          pedido => pedido.ProdutoId,
          produto => produto.Id,
          (pedido, produto) => new {
              Produto = produto.Nome,
              Quantidade = pedido.Quantidade,
              Total = pedido.Quantidade * produto.Preco
          });
```

### Sintaxe de Consulta (Query Syntax)

```csharp
// Method syntax (mais comum)
var resultado1 = produtos
    .Where(p => p.Preco > 100)
    .OrderBy(p => p.Nome)
    .Select(p => p.Nome);

// Query syntax (estilo SQL)
var resultado2 = from p in produtos
                 where p.Preco > 100
                 orderby p.Nome
                 select p.Nome;

// Ambos produzem o mesmo resultado
```

## 🎯 Exercício Prático

### Sistema de Gerenciamento de Alunos

```csharp
public class Aluno {
    public int Id { get; set; }
    public string Nome { get; set; }
    public int Idade { get; set; }
    public List<double> Notas { get; set; } = new List<double>();
}

// Criar alunos
List<Aluno> alunos = new List<Aluno> {
    new Aluno { Id = 1, Nome = "João", Idade = 20, Notas = new List<double> { 8.5, 9.0, 7.5 } },
    new Aluno { Id = 2, Nome = "Maria", Idade = 22, Notas = new List<double> { 9.5, 9.0, 10 } },
    new Aluno { Id = 3, Nome = "Pedro", Idade = 19, Notas = new List<double> { 6.0, 7.0, 6.5 } },
    new Aluno { Id = 4, Nome = "Ana", Idade = 21, Notas = new List<double> { 9.0, 8.5, 9.5 } }
};

// 1. Alunos aprovados (média >= 7)
var aprovados = alunos
    .Where(a => a.Notas.Average() >= 7)
    .Select(a => new {
        a.Nome,
        Media = a.Notas.Average()
    })
    .OrderByDescending(a => a.Media);

Console.WriteLine("Alunos Aprovados:");
foreach (var aluno in aprovados) {
    Console.WriteLine($"{aluno.Nome}: {aluno.Media:F2}");
}

// 2. Maior nota de cada aluno
var maioresNotas = alunos
    .Select(a => new {
        a.Nome,
        MaiorNota = a.Notas.Max()
    });

// 3. Alunos maiores de 20 anos ordenados por nome
var maioresDe20 = alunos
    .Where(a => a.Idade > 20)
    .OrderBy(a => a.Nome);

// 4. Estatísticas gerais
double mediaGeral = alunos.SelectMany(a => a.Notas).Average();
double maiorNotaGeral = alunos.SelectMany(a => a.Notas).Max();
int totalAlunos = alunos.Count();

Console.WriteLine($"\nEstatísticas:");
Console.WriteLine($"Total de alunos: {totalAlunos}");
Console.WriteLine($"Média geral: {mediaGeral:F2}");
Console.WriteLine($"Maior nota: {maiorNotaGeral:F2}");
```

### Exercício: E-commerce

```csharp
// Implementar sistema que:
// 1. Liste produtos por categoria
// 2. Encontre produtos em promoção (preco < 100)
// 3. Calcule valor total do estoque
// 4. Encontre produto mais caro de cada categoria
// 5. Liste produtos com estoque baixo (< 10 unidades)
```

## ✅ Verificação

Para prosseguir, certifique-se de:

- [ ] Usou List, Dictionary, HashSet
- [ ] Implementou consultas LINQ básicas
- [ ] Usou Where, Select, OrderBy
- [ ] Calculou agregações (Sum, Average, etc.)
- [ ] Agrupou dados com GroupBy
- [ ] Completou exercício de gerenciamento de alunos

## 🎯 Próximos Passos

No próximo módulo, vamos aprender sobre **Programação Assíncrona** - async/await e Tasks!

---

[← Voltar: Orientação a Objetos](./01-orientacao-objetos.md) | [Próximo: Programação Assíncrona →](./03-programacao-assincrona.md)