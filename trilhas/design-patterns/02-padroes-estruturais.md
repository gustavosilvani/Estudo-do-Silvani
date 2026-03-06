# 02 - Padrões Estruturais

Padrões **estruturais** tratam da **composição de classes e objetos**, facilitando o uso de estruturas flexíveis e reutilizáveis.

## Adapter

Converte a **interface de uma classe** em outra interface que o cliente espera. Permite que classes com interfaces incompatíveis trabalhem juntas.

### Quando usar

- Integrar bibliotecas ou APIs externas cuja interface não combina com o que seu código espera.

### Exemplo em C#

```csharp
// Serviço externo (interface incompatível)
public class ServicoLegado
{
    public string ObterDadosXml() => "<usuario><nome>João</nome></usuario>";
}

// Interface que sua aplicação usa
public interface IUsuarioService
{
    Usuario ObterUsuario();
}

// Adapter
public class ServicoLegadoAdapter : IUsuarioService
{
    private readonly ServicoLegado _legado;

    public ServicoLegadoAdapter(ServicoLegado legado) => _legado = legado;

    public Usuario ObterUsuario()
    {
        var xml = _legado.ObterDadosXml();
        // Converte XML para Usuario
        return ParseUsuario(xml);
    }
}
```

---

## Decorator

**Estende o comportamento** de um objeto dinamicamente, envolvendo-o em um objeto “decorador” que adiciona responsabilidades sem alterar a classe original.

### Quando usar

- Adicionar responsabilidades individuais (logging, cache, compressão) de forma combinável, sem subclasse fixa.

### Exemplo em C#

```csharp
public interface INotificador
{
    void Enviar(string mensagem);
}

public class NotificadorEmail : INotificador
{
    public void Enviar(string mensagem) => Console.WriteLine($"Email: {mensagem}");
}

public class NotificadorComLog : INotificador
{
    private readonly INotificador _inner;

    public NotificadorComLog(INotificador inner) => _inner = inner;

    public void Enviar(string mensagem)
    {
        Console.WriteLine($"[Log] Enviando: {mensagem}");
        _inner.Enviar(mensagem);
    }
}

// Uso: new NotificadorComLog(new NotificadorEmail())
```

---

## Facade

Oferece uma **interface simplificada** para um conjunto de interfaces de um subsistema, reduzindo a complexidade percebida pelo cliente.

### Quando usar

- Esconder a complexidade de várias classes ou APIs atrás de uma única interface de alto nível.

### Exemplo em C#

```csharp
public class SubsistemaA { public void OperacaoA() { } }
public class SubsistemaB { public void OperacaoB() { } }

public class FacadeOrquestrador
{
    private readonly SubsistemaA _a;
    private readonly SubsistemaB _b;

    public FacadeOrquestrador(SubsistemaA a, SubsistemaB b)
    {
        _a = a;
        _b = b;
    }

    public void ExecutarProcessoCompleto()
    {
        _a.OperacaoA();
        _b.OperacaoB();
    }
}
```

---

**Próximo**: [03 - Padrões Comportamentais](./03-padroes-comportamentais.md)
