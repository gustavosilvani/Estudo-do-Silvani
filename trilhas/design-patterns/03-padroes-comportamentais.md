# 03 - Padrões Comportamentais

Padrões **comportamentais** tratam de **como objetos interagem e delegam responsabilidades**, definindo fluxos de comunicação e algoritmos.

## Strategy

Define uma **família de algoritmos**, encapsula cada um e os torna **intercambiáveis**. O padrão permite que o algoritmo varie independentemente dos clientes que o utilizam.

### Quando usar

- Várias formas de executar uma mesma tarefa (cálculo de frete, formatadores, regras de desconto). Muito alinhado ao **Open/Closed Principle**.

### Exemplo em C# (como no microsserviço de logística)

```csharp
public interface ICalculoFrete
{
    decimal Calcular(Pedido pedido);
    string TipoFrete { get; }
}

public class FretePadrao : ICalculoFrete
{
    public string TipoFrete => "Padrão";
    public decimal Calcular(Pedido pedido) => 10m + (pedido.PesoKg * 2m);
}

public class FreteExpresso : ICalculoFrete
{
    public string TipoFrete => "Expresso";
    public decimal Calcular(Pedido pedido) => 25m + (pedido.PesoKg * 5m);
}

public class CalculadoraFrete
{
    private readonly ICalculoFrete _estrategia;

    public CalculadoraFrete(ICalculoFrete estrategia) => _estrategia = estrategia;

    public decimal Calcular(Pedido pedido) => _estrategia.Calcular(pedido);
}
```

Novas estratégias são adicionadas criando novas classes, sem alterar `CalculadoraFrete` (OCP).

---

## Observer

Define uma dependência **um-para-muitos** entre objetos: quando um objeto muda de estado, todos os dependentes são notificados e atualizados.

### Quando usar

- Eventos de domínio, notificações, atualização de UI quando o modelo muda.

### Exemplo em C#

```csharp
public interface IObserver<T>
{
    void Atualizar(T dados);
}

public interface IObservable<T>
{
    void Inscrever(IObserver<T> observer);
    void Notificar(T dados);
}

public class Pedido : IObservable<Pedido>
{
    private readonly List<IObserver<Pedido>> _observers = new();
    public string Status { get; private set; }

    public void AtualizarStatus(string status)
    {
        Status = status;
        Notificar(this);
    }

    public void Inscrever(IObserver<Pedido> observer) => _observers.Add(observer);
    public void Notificar(Pedido dados)
    {
        foreach (var o in _observers) o.Atualizar(dados);
    }
}
```

Em .NET, `IObservable<T>` e `IObserver<T>` existem na BCL; eventos e `event` em C# são outra forma de observer.

---

## Command

Encapsula uma **solicitação como um objeto**, permitindo parametrizar clientes com diferentes requisições, enfileirar, fazer log ou desfazer operações.

### Quando usar

- Filas de comandos, undo/redo, operações assíncronas ou auditáveis.

### Exemplo em C#

```csharp
public interface IComando
{
    void Executar();
}

public class ComandoCriarPedido : IComando
{
    private readonly IPedidoRepository _repo;
    private readonly Pedido _pedido;

    public ComandoCriarPedido(IPedidoRepository repo, Pedido pedido)
    {
        _repo = repo;
        _pedido = pedido;
    }

    public void Executar() => _repo.Adicionar(_pedido);
}

// Uso: fila de comandos, undo, log
var comando = new ComandoCriarPedido(repo, pedido);
comando.Executar();
```

---

**Próximo**: [04 - Repository e Unit of Work](./04-repository-unit-of-work.md)
