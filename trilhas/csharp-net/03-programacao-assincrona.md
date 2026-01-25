# 03 - Programação Assíncrona

## 📖 O que é Programação Assíncrona?

Programação assíncrona permite executar operações demoradas sem bloquear a thread principal, melhorando responsividade e performance.

## 🎯 Conceitos Fundamentais

### Operações Síncronas vs Assíncronas

```csharp
// ❌ SÍNCRONO - Bloqueia execução
public string BaixarDados() {
    Thread.Sleep(3000);  // Simula operação demorada
    return "Dados baixados";
}

void Executar() {
    Console.WriteLine("Iniciando...");
    string dados = BaixarDados();  // Bloqueia por 3 segundos
    Console.WriteLine(dados);
    Console.WriteLine("Finalizado");
}

// ✅ ASSÍNCRONO - Não bloqueia
public async Task<string> BaixarDadosAsync() {
    await Task.Delay(3000);  // Não bloqueia
    return "Dados baixados";
}

async Task ExecutarAsync() {
    Console.WriteLine("Iniciando...");
    string dados = await BaixarDadosAsync();  // Não bloqueia
    Console.WriteLine(dados);
    Console.WriteLine("Finalizado");
}
```

## 🔄 async e await

### Sintaxe Básica

```csharp
// Método assíncrono retorna Task ou Task<T>
public async Task ProcessarAsync() {
    await Task.Delay(1000);
    Console.WriteLine("Processado");
}

public async Task<int> CalcularAsync() {
    await Task.Delay(500);
    return 42;
}

// Usando
await ProcessarAsync();
int resultado = await CalcularAsync();
```

### Exemplos Práticos

```csharp
public class ServicoCliente {
    // Operação de I/O assíncrona
    public async Task<Cliente> BuscarClienteAsync(int id) {
        // Simula chamada ao banco de dados
        await Task.Delay(100);
        return new Cliente { Id = id, Nome = "João" };
    }

    // Múltiplas operações assíncronas em sequência
    public async Task<Pedido> ProcessarPedidoAsync(int clienteId, int produtoId) {
        var cliente = await BuscarClienteAsync(clienteId);
        var produto = await BuscarProdutoAsync(produtoId);
        var pedido = await CriarPedidoAsync(cliente, produto);
        return pedido;
    }

    // Operações paralelas
    public async Task<(Cliente, List<Produto>)> BuscarDadosParalelosAsync(int clienteId) {
        // Executa ambas ao mesmo tempo
        Task<Cliente> taskCliente = BuscarClienteAsync(clienteId);
        Task<List<Produto>> taskProdutos = BuscarProdutosAsync();

        // Aguarda ambas terminarem
        await Task.WhenAll(taskCliente, taskProdutos);

        return (taskCliente.Result, taskProdutos.Result);
    }
}
```

## 📦 Task e Task<T>

### Criando Tasks

```csharp
// Task sem retorno
Task tarefa1 = Task.Run(() => {
    Thread.Sleep(1000);
    Console.WriteLine("Tarefa 1 concluída");
});

// Task com retorno
Task<int> tarefa2 = Task.Run(() => {
    Thread.Sleep(500);
    return 42;
});

// Aguardar tasks
await tarefa1;
int resultado = await tarefa2;
```

### Operações com Múltiplas Tasks

```csharp
// WhenAll - Aguardar todas
Task<int> t1 = Task.Run(async () => { await Task.Delay(1000); return 10; });
Task<int> t2 = Task.Run(async () => { await Task.Delay(500); return 20; });
Task<int> t3 = Task.Run(async () => { await Task.Delay(1500); return 30; });

int[] resultados = await Task.WhenAll(t1, t2, t3);
// resultados = [10, 20, 30]
// Tempo total: ~1.5 segundos (não 3 segundos!)

// WhenAny - Primeira a completar
Task<string> primeira = await Task.WhenAny(
    BaixarDe("servidor1"),
    BaixarDe("servidor2"),
    BaixarDe("servidor3")
);
string dados = await primeira;
```

## 🌐 Chamadas HTTP Assíncronas

```csharp
public class ApiService {
    private readonly HttpClient _httpClient;

    public ApiService() {
        _httpClient = new HttpClient {
            BaseAddress = new Uri("https://api.exemplo.com")
        };
    }

    // GET assíncrono
    public async Task<string> BuscarDadosAsync(int id) {
        var response = await _httpClient.GetAsync($"/api/dados/{id}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    // POST assíncrono
    public async Task<Cliente> CriarClienteAsync(Cliente cliente) {
        var json = JsonSerializer.Serialize(cliente);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        var response = await _httpClient.PostAsync("/api/clientes", content);
        response.EnsureSuccessStatusCode();
        
        var responseJson = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Cliente>(responseJson);
    }
}
```

## 🎯 Tratamento de Erros

```csharp
public async Task<string> BuscarDadosComTratamentoAsync() {
    try {
        var response = await _httpClient.GetAsync("/api/dados");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
    catch (HttpRequestException ex) {
        Console.WriteLine($"Erro HTTP: {ex.Message}");
        return null;
    }
    catch (TaskCanceledException ex) {
        Console.WriteLine($"Timeout: {ex.Message}");
        return null;
    }
    catch (Exception ex) {
        Console.WriteLine($"Erro: {ex.Message}");
        throw;
    }
}
```

## ⏱️ Timeouts e Cancelamento

```csharp
// CancellationToken
public async Task ProcessarComCancelamentoAsync(CancellationToken cancellationToken) {
    for (int i = 0; i < 10; i++) {
        // Verificar se foi cancelado
        cancellationToken.ThrowIfCancellationRequested();
        
        await Task.Delay(1000, cancellationToken);
        Console.WriteLine($"Progresso: {i + 1}/10");
    }
}

// Uso
var cts = new CancellationTokenSource();
cts.CancelAfter(TimeSpan.FromSeconds(5));  // Cancela após 5s

try {
    await ProcessarComCancelamentoAsync(cts.Token);
}
catch (OperationCanceledException) {
    Console.WriteLine("Operação cancelada!");
}

// Timeout em HttpClient
var httpClient = new HttpClient {
    Timeout = TimeSpan.FromSeconds(30)
};
```

## 🎯 Exercício Prático

```csharp
// Sistema de download paralelo
public class DownloadManager {
    private readonly HttpClient _httpClient = new HttpClient();

    public async Task<List<string>> BaixarMultiplosArquivosAsync(List<string> urls) {
        var tasks = urls.Select(url => BaixarArquivoAsync(url)).ToList();
        var resultados = await Task.WhenAll(tasks);
        return resultados.ToList();
    }

    private async Task<string> BaixarArquivoAsync(string url) {
        Console.WriteLine($"Iniciando download: {url}");
        var conteudo = await _httpClient.GetStringAsync(url);
        Console.WriteLine($"Concluído: {url}");
        return conteudo;
    }

    // Com progresso
    public async Task BaixarComProgressoAsync(string url, IProgress<int> progresso) {
        var response = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
        var total = response.Content.Headers.ContentLength ?? -1L;
        var buffer = new byte[8192];
        long totalBaixado = 0;

        using var stream = await response.Content.ReadAsStreamAsync();
        int bytesLidos;

        while ((bytesLidos = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0) {
            totalBaixado += bytesLidos;
            var percentual = total > 0 ? (int)((totalBaixado * 100) / total) : 0;
            progresso?.Report(percentual);
        }
    }
}
```

## ✅ Verificação

- [ ] Entendeu diferença entre síncrono e assíncrono
- [ ] Usou async/await corretamente
- [ ] Trabalhou com múltiplas Tasks
- [ ] Implementou operações paralelas
- [ ] Tratou erros em código assíncrono
- [ ] Usou CancellationToken

---

[← Voltar: Collections e LINQ](./02-collections-linq.md) | [Próximo: ASP.NET Core Básico →](./04-aspnet-core-basico.md)