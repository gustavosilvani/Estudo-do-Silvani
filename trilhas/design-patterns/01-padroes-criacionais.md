# 01 - Padrões Criacionais

Padrões **criacionais** tratam da **criação de objetos**, encapsulando a lógica de instanciação e tornando o sistema independente de como os objetos são criados.

## Singleton

Garante que uma classe tenha **apenas uma instância** e oferece um ponto global de acesso a ela.

### Quando usar

- Logger, configuração global, conexão única com recurso externo (com cuidado em concorrência e testes).

### Exemplo em C#

```csharp
public sealed class ConfiguracaoSingleton
{
    private static readonly Lazy<ConfiguracaoSingleton> _instance =
        new Lazy<ConfiguracaoSingleton>(() => new ConfiguracaoSingleton());

    public static ConfiguracaoSingleton Instance => _instance.Value;

    private ConfiguracaoSingleton() { }

    public string ConnectionString { get; set; }
}
```

Em aplicações .NET, **evite Singleton estático** quando possível: use **Dependency Injection** com ciclo de vida `Singleton` no container (ex.: `AddSingleton<T>()`), o que mantém uma única instância mas facilita testes e substituição.

---

## Factory Method

Define uma **interface para criar um objeto**, mas deixa as subclasses decidirem qual classe instanciar.

### Quando usar

- A criação do objeto depende de lógica ou tipo que pode variar (por exemplo, por configuração ou tipo de usuário).

### Exemplo em C#

```csharp
public interface IDocumento
{
    string Gerar();
}

public class RelatorioPdf : IDocumento { /* ... */ }
public class RelatorioWord : IDocumento { /* ... */ }

public abstract class GeradorRelatorio
{
    protected abstract IDocumento CriarDocumento();

    public string GerarRelatorio()
    {
        var doc = CriarDocumento();
        return doc.Gerar();
    }
}

public class GeradorPdf : GeradorRelatorio
{
    protected override IDocumento CriarDocumento() => new RelatorioPdf();
}
```

---

## Abstract Factory

Fornece uma **interface para criar famílias de objetos relacionados** sem especificar classes concretas.

### Quando usar

- Múltiplos produtos relacionados (ex.: tema claro/escuro: botão, caixa de texto, label) que devem ser usados em conjunto.

### Exemplo em C#

```csharp
public interface IGUIFactory
{
    IBotao CriarBotao();
    ICaixaTexto CriarCaixaTexto();
}

public class TemaClaroFactory : IGUIFactory
{
    public IBotao CriarBotao() => new BotaoClaro();
    public ICaixaTexto CriarCaixaTexto() => new CaixaTextoClara();
}
```

---

## Builder

Separa a **construção de um objeto complexo** da sua representação, permitindo o mesmo processo de construção criar representações diferentes.

### Quando usar

- Objetos com muitos parâmetros opcionais, ou quando a construção em etapas deixa o código mais legível.

### Exemplo em C#

```csharp
public class PedidoBuilder
{
    private string _cliente;
    private readonly List<ItemPedido> _itens = new();
    private decimal _desconto;

    public PedidoBuilder ParaCliente(string cliente)
    {
        _cliente = cliente;
        return this;
    }

    public PedidoBuilder ComItem(string produto, int qtd, decimal preco)
    {
        _itens.Add(new ItemPedido(produto, qtd, preco));
        return this;
    }

    public PedidoBuilder ComDesconto(decimal valor)
    {
        _desconto = valor;
        return this;
    }

    public Pedido Build() => new Pedido(_cliente, _itens, _desconto);
}

// Uso:
var pedido = new PedidoBuilder()
    .ParaCliente("João")
    .ComItem("Livro", 2, 50m)
    .ComDesconto(10m)
    .Build();
```

---

**Próximo**: [02 - Padrões Estruturais](./02-padroes-estruturais.md)
