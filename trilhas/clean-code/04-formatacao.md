# 04 - Formatação

## 📖 A Importância da Formatação

Código bem formatado é mais fácil de ler, entender e manter. Formatação é comunicação.

## 📏 Tamanho de Arquivo

```csharp
// ✅ BOM - Arquivo pequeno e focado
// Usuario.cs (50 linhas)
public class Usuario {
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    
    public void ValidarEmail() {
        if (!Email.Contains("@")) {
            throw new ValidationException("Email inválido");
        }
    }
}

// ❌ RUIM - Arquivo gigante (1000+ linhas)
// Sistema.cs
public class Sistema {
    // 200 linhas de propriedades
    // 300 linhas de validações
    // 500 linhas de lógica de negócio
}
```

**Regra:** Classes devem ter menos de 200-300 linhas.

## 📐 Espaçamento Vertical

### Linhas em Branco com Propósito

```csharp
// ✅ BOM - Agrupamento lógico
public class PedidoService {
    private readonly IRepositorioPedido _repositorio;
    private readonly IEmailService _emailService;
    private readonly ILogger _logger;

    public PedidoService(
        IRepositorioPedido repositorio,
        IEmailService emailService,
        ILogger logger) {
        _repositorio = repositorio;
        _emailService = emailService;
        _logger = logger;
    }

    public void ProcessarPedido(Pedido pedido) {
        ValidarPedido(pedido);
        
        CalcularTotais(pedido);
        AplicarDescontos(pedido);
        
        SalvarPedido(pedido);
        NotificarCliente(pedido);
    }

    private void ValidarPedido(Pedido pedido) {
        // Implementação...
    }
}

// ❌ RUIM - Sem agrupamento
public class PedidoService {
    private readonly IRepositorioPedido _repositorio;
    private readonly IEmailService _emailService;
    public PedidoService(IRepositorioPedido repositorio, IEmailService emailService) {
        _repositorio = repositorio;
        _emailService = emailService;
    }
    public void ProcessarPedido(Pedido pedido) {
        ValidarPedido(pedido);
        CalcularTotais(pedido);
        SalvarPedido(pedido);
    }
    private void ValidarPedido(Pedido pedido) {
    }
}
```

## 🔤 Indentação

```csharp
// ✅ BOM - 4 espaços (padrão C#)
public class Cliente {
    public void ProcessarPedido() {
        if (EstaAtivo) {
            foreach (var item in Itens) {
                ProcessarItem(item);
            }
        }
    }
}

// Configure seu editor:
// Visual Studio / VS Code: 4 espaços para C#
```

## 📊 Densidade

```csharp
// ❌ RUIM - Muito denso
public class Calculadora{
    private decimal valor;
    public Calculadora(decimal v){valor=v;}
    public decimal Calcular(decimal x,decimal y){
        var r=x+y;
        if(r>100)r=100;
        return r*valor;
    }
}

// ✅ BOM - Espaçamento adequado
public class Calculadora {
    private decimal _valorBase;

    public Calculadora(decimal valorBase) {
        _valorBase = valorBase;
    }

    public decimal Calcular(decimal x, decimal y) {
        var resultado = x + y;
        
        if (resultado > 100) {
            resultado = 100;
        }
        
        return resultado * _valorBase;
    }
}
```

## 🎯 Ordem dos Membros

```csharp
// ✅ BOM - Ordem consistente
public class Cliente {
    // 1. Campos privados
    private readonly IRepositorio _repositorio;
    private readonly ILogger _logger;
    
    // 2. Constantes
    private const int IDADE_MINIMA = 18;
    
    // 3. Propriedades públicas
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    
    // 4. Construtor
    public Cliente(IRepositorio repositorio, ILogger logger) {
        _repositorio = repositorio;
        _logger = logger;
    }
    
    // 5. Métodos públicos
    public void Salvar() {
        ValidarDados();
        _repositorio.Salvar(this);
    }
    
    public bool EhMaiorDeIdade() {
        return Idade >= IDADE_MINIMA;
    }
    
    // 6. Métodos privados
    private void ValidarDados() {
        if (string.IsNullOrEmpty(Nome)) {
            throw new ValidationException("Nome obrigatório");
        }
    }
}
```

## 📝 Alinhamento

```csharp
// ✅ BOM - Declarações claras
var nome = "João";
var email = "joao@email.com";
var idade = 30;
var ativo = true;

// ❌ RUIM - Alinhamento artificial
var nome   = "João";
var email  = "joao@email.com";
var idade  = 30;
var ativo  = true;

// ✅ BOM - Objetos multi-linha
var cliente = new Cliente {
    Nome = "João Silva",
    Email = "joao@email.com",
    Telefone = "(11) 99999-9999",
    Endereco = new Endereco {
        Rua = "Rua Principal",
        Numero = 123,
        Cidade = "São Paulo"
    }
};
```

## 🔨 Convenções C#

### Nomenclatura
```csharp
// Classes e Interfaces: PascalCase
public class PedidoService { }
public interface IRepositorio { }

// Métodos e Propriedades: PascalCase
public string Nome { get; set; }
public void ProcessarPedido() { }

// Parâmetros e variáveis locais: camelCase
public void Calcular(int valorTotal, decimal desconto) {
    var resultado = valorTotal - desconto;
}

// Campos privados: _camelCase
private readonly ILogger _logger;
private int _contador;

// Constantes: SCREAMING_SNAKE_CASE ou PascalCase
private const int MAX_TENTATIVAS = 3;
private const int MaxTentativas = 3;
```

### Chaves
```csharp
// ✅ BOM - Chaves em nova linha (Allman style - padrão C#)
if (condicao) {
    Executar();
}

public class Exemplo {
    public void Metodo() {
        // código
    }
}

// ❌ RUIM para C# - K&R style (mais comum em Java/JS)
if (condicao) {
    Executar();
}
```

## 🎯 EditorConfig

Crie `.editorconfig` na raiz do projeto:

```ini
# EditorConfig para C#
root = true

[*.cs]
indent_style = space
indent_size = 4
end_of_line = crlf
charset = utf-8
trim_trailing_whitespace = true
insert_final_newline = true

# Nomenclatura C#
dotnet_naming_rule.private_fields_underscored.symbols = private_fields
dotnet_naming_rule.private_fields_underscored.style = underscored
dotnet_naming_rule.private_fields_underscored.severity = warning

dotnet_naming_symbols.private_fields.applicable_kinds = field
dotnet_naming_symbols.private_fields.applicable_accessibilities = private

dotnet_naming_style.underscored.capitalization = camel_case
dotnet_naming_style.underscored.required_prefix = _

# Organização de usings
dotnet_sort_system_directives_first = true
dotnet_separate_import_directive_groups = false
```

## 🔧 Ferramentas

### Visual Studio
- **Format Document**: Ctrl+K, Ctrl+D
- **Format Selection**: Ctrl+K, Ctrl+F

### VS Code
- **Format Document**: Shift+Alt+F
- **Format on Save**: Configurar em settings.json

```json
{
    "editor.formatOnSave": true,
    "editor.tabSize": 4,
    "editor.insertSpaces": true,
    "[csharp]": {
        "editor.defaultFormatter": "ms-dotnettools.csharp"
    }
}
```

### StyleCop
```xml
<PackageReference Include="StyleCop.Analyzers" Version="1.1.118" />
```

## 🎯 Exercício Prático

```csharp
// ❌ CÓDIGO MAL FORMATADO - REFATORE
public class usr{private string n;private int a;public usr(string nome,int idade){n=nome;a=idade;}
public void sv(){if(a>=18){Console.WriteLine("Maior");} else{Console.WriteLine("Menor");}}
public bool val(){return !string.IsNullOrEmpty(n)&&a>0;}
}

// ✅ CÓDIGO BEM FORMATADO
public class Usuario {
    private readonly string _nome;
    private readonly int _idade;

    public Usuario(string nome, int idade) {
        _nome = nome;
        _idade = idade;
    }

    public void ExibirCategoria() {
        if (_idade >= 18) {
            Console.WriteLine("Maior de idade");
        } else {
            Console.WriteLine("Menor de idade");
        }
    }

    public bool EhValido() {
        return !string.IsNullOrEmpty(_nome) && _idade > 0;
    }
}
```

## ✅ Checklist de Formatação

- [ ] Arquivos com menos de 300 linhas?
- [ ] Indentação consistente (4 espaços)?
- [ ] Linhas em branco separam conceitos?
- [ ] Ordem dos membros consistente?
- [ ] Chaves seguem padrão Allman?
- [ ] Nomenclatura segue convenções C#?
- [ ] EditorConfig configurado?
- [ ] Formatação automática ativada?

---

[← Voltar: Comentários](./03-comentarios.md) | [Próximo: Objetos e Estruturas →](./05-objetos-e-estruturas.md)