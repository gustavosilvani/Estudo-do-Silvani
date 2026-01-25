# 03 - Comentários

## 📖 Quando (Não) Comentar

> "O código deveria ser auto-explicativo. Comentários são, no melhor dos casos, um mal necessário." - Robert C. Martin

## 🎯 Regra de Ouro

**Código bom > Comentários**

Prefira melhorar o código a adicionar comentários explicativos.

## ❌ Comentários Ruins

### 1. Comentários Óbvios
```csharp
// ❌ Não adiciona valor
// Incrementa i
i++;

// Cria um cliente
Cliente cliente = new Cliente();

// ✅ Código auto-explicativo (sem comentário)
quantidadeTentativasLogin++;
var novoCliente = CriarClientePadrao();
```

### 2. Comentários Desatualizados
```csharp
// ❌ Mentira - código mudou
// Retorna lista de clientes ativos
public Dictionary<int, Cliente> BuscarTodos() {  // Agora retorna Dictionary!
    return _clientes;
}

// ✅ Sem comentário enganoso
public Dictionary<int, Cliente> BuscarClientesAtivos() {
    return _clientes.Where(c => c.Ativo).ToDictionary(c => c.Id);
}
```

### 3. Comentários Redundantes
```csharp
// ❌ Código já é claro
public class Cliente {
    // Nome do cliente
    public string Nome { get; set; }
    
    // Email do cliente
    public string Email { get; set; }
    
    // Idade do cliente
    public int Idade { get; set; }
}
```

## ✅ Comentários Bons

### 1. Explicar "Porquê"
```csharp
// ✅ Explica decisão de negócio
// Aguarda 3 segundos antes de retentar para evitar sobrecarga no servidor
await Task.Delay(3000);

// ✅ Explica workaround
// Bug na API externa: retorna null em vez de lista vazia
var items = response.Items ?? new List<Item>();
```

### 2. Avisos Importantes
```csharp
// ✅ Alerta sobre consequências
// ATENÇÃO: Esta operação é IRREVERSÍVEL e deleta TODOS os dados do cliente
public void DeletarClientePermanentemente(int clienteId) {
    // ...
}

// ✅ TODO com contexto
// TODO: Implementar cache Redis quando > 10k usuários (Issue #234)
public List<Usuario> BuscarTodos() {
    return _repository.GetAll();
}
```

### 3. Documentação de API Pública
```csharp
/// <summary>
/// Calcula o desconto aplicável ao cliente baseado em seu histórico de compras.
/// </summary>
/// <param name="clienteId">ID do cliente</param>
/// <param name="valorCompra">Valor total da compra em reais</param>
/// <returns>Percentual de desconto entre 0 e 100</returns>
/// <exception cref="ClienteNaoEncontradoException">Cliente não existe</exception>
public decimal CalcularDesconto(int clienteId, decimal valorCompra) {
    // Implementação...
}
```

## 🔄 Refatoração: Comentário → Código

```csharp
// ❌ ANTES - Comentário necessário
public void ProcessarPedido(Pedido pedido) {
    // Verifica se cliente é VIP e tem mais de 5 pedidos nos últimos 30 dias
    if (pedido.Cliente.Tipo == "VIP" && pedido.Cliente.Pedidos.Count(p => p.Data > DateTime.Now.AddDays(-30)) > 5) {
        // Aplica desconto especial
        pedido.DescontoPercentual = 20;
    }
}

// ✅ DEPOIS - Código auto-explicativo
public void ProcessarPedido(Pedido pedido) {
    if (ClienteQualificaParaDescontoEspecial(pedido.Cliente)) {
        AplicarDescontoEspecial(pedido);
    }
}

private bool ClienteQualificaParaDescontoEspecial(Cliente cliente) {
    return cliente.EhVIP && cliente.TemMaisDe5PedidosUltimos30Dias();
}

private void AplicarDescontoEspecial(Pedido pedido) {
    const decimal DESCONTO_CLIENTE_VIP_FREQUENTE = 20m;
    pedido.DescontoPercentual = DESCONTO_CLIENTE_VIP_FREQUENTE;
}
```

## 🎯 Exercício Prático

```csharp
// ❌ CÓDIGO COM COMENTÁRIOS RUINS - REFATORE
public class UserManager {
    // Lista de usuários
    private List<User> users;
    
    // Construtor
    public UserManager() {
        users = new List<User>();
    }
    
    // Adiciona usuário
    public void Add(User u) {
        // Verifica se usuário não é nulo
        if (u != null) {
            // Verifica se email é válido (deve conter @)
            if (u.Email.Contains("@")) {
                // Adiciona na lista
                users.Add(u);
            }
        }
    }
    
    // Busca usuário por ID
    // Retorna null se não encontrar
    public User Get(int id) {
        // Loop pelos usuários
        foreach (var u in users) {
            // Compara ID
            if (u.Id == id) {
                // Retorna usuário encontrado
                return u;
            }
        }
        // Não encontrou
        return null;
    }
}

// ✅ SOLUÇÃO - CÓDIGO LIMPO SEM COMENTÁRIOS DESNECESSÁRIOS
public class GerenciadorUsuarios {
    private readonly List<Usuario> _usuarios;

    public GerenciadorUsuarios() {
        _usuarios = new List<Usuario>();
    }

    public void AdicionarUsuario(Usuario usuario) {
        ValidarUsuario(usuario);
        _usuarios.Add(usuario);
    }

    private void ValidarUsuario(Usuario usuario) {
        if (usuario == null) {
            throw new ArgumentNullException(nameof(usuario));
        }

        if (!EmailEhValido(usuario.Email)) {
            throw new ValidationException("Email inválido");
        }
    }

    private bool EmailEhValido(string email) {
        return !string.IsNullOrEmpty(email) && email.Contains("@");
    }

    public Usuario BuscarPorId(int id) {
        return _usuarios.FirstOrDefault(u => u.Id == id);
    }
}
```

## ✅ Checklist de Comentários

Antes de adicionar comentário, pergunte:
- [ ] Posso melhorar o nome da função/variável?
- [ ] Posso extrair função com nome descritivo?
- [ ] Posso usar constante nomeada?
- [ ] Este comentário vai ficar desatualizado?
- [ ] Estou explicando "o quê" (ruim) ou "porquê" (bom)?

---

[← Voltar: Funções](./02-funcoes.md) | [Próximo: Formatação →](./04-formatacao.md)