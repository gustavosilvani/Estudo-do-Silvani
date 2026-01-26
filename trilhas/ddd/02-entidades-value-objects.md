# 02 - Entidades e Value Objects

## 📘 Nível Básico

### Entidades vs Value Objects

No DDD, existem dois tipos principais de objetos de domínio:

- **Entidades**: Têm identidade única
- **Value Objects**: São definidos apenas por seus valores

## 🆔 Entidades (Entities)

### Características

- **Identidade única**: Dois objetos com mesmo ID são o mesmo objeto
- **Mutáveis**: Podem mudar ao longo do tempo
- **Persistem**: Mantêm identidade mesmo após mudanças
- **Comparação por ID**: `cliente1.Id == cliente2.Id` → mesmo cliente

### Exemplo

```csharp
public class Cliente {
    public ClienteId Id { get; }  // Identidade única
    public string Nome { get; private set; }
    public Email Email { get; private set; }
    
    public Cliente(ClienteId id, string nome, Email email) {
        Id = id;
        Nome = nome;
        Email = email;
    }
    
    public void AlterarNome(string novoNome) {
        // Cliente muda, mas ID permanece
        Nome = novoNome;
    }
    
    // Comparação por identidade
    public override bool Equals(object? obj) {
        return obj is Cliente outro && Id.Equals(outro.Id);
    }
    
    public override int GetHashCode() => Id.GetHashCode();
}

// Uso
var cliente1 = new Cliente(new ClienteId(1), "João", new Email("joao@email.com"));
var cliente2 = new Cliente(new ClienteId(1), "João Silva", new Email("joao.silva@email.com"));

// São o mesmo cliente (mesmo ID)
Assert.Equal(cliente1, cliente2);  // ✅ True
```

## 💎 Value Objects

### Características

- **Sem identidade**: Dois objetos com mesmo valor são iguais
- **Imutáveis**: Não mudam após criação
- **Comparação por valor**: `email1.Valor == email2.Valor` → iguais
- **Substituíveis**: Podem ser substituídos por novos objetos

### Exemplo

```csharp
public class Email {
    public string Valor { get; }
    
    public Email(string valor) {
        if (string.IsNullOrWhiteSpace(valor) || !valor.Contains("@")) {
            throw new EmailInvalidoException(valor);
        }
        Valor = valor.ToLowerInvariant();
    }
    
    // Comparação por valor
    public override bool Equals(object? obj) {
        return obj is Email outro && Valor == outro.Valor;
    }
    
    public override int GetHashCode() => Valor.GetHashCode();
    
    public override string ToString() => Valor;
}

// Uso
var email1 = new Email("Joao@Email.com");
var email2 = new Email("joao@email.com");

// São iguais (mesmo valor)
Assert.Equal(email1, email2);  // ✅ True
```

## 🎯 Quando Usar Cada Um?

### Use Entidade quando:
- Objeto precisa de identidade única
- Objeto muda ao longo do tempo
- Objeto precisa ser rastreado

```csharp
// ✅ Entidade - Cliente tem identidade
public class Cliente {
    public ClienteId Id { get; }  // Identidade única
    // ...
}
```

### Use Value Object quando:
- Objeto é definido apenas por seus valores
- Objeto é imutável
- Objeto pode ser substituído

```csharp
// ✅ Value Object - Email é apenas um valor
public class Email {
    public string Valor { get; }  // Sem ID
    // ...
}

// ✅ Value Object - Endereço é apenas um valor
public class Endereco {
    public string Rua { get; }
    public string Cidade { get; }
    public string CEP { get; }
    // ...
}
```

## 📚 Exemplos Práticos

### Value Objects Comuns

```csharp
// CPF
public class CPF {
    public string Valor { get; }
    
    public CPF(string valor) {
        if (!Validar(valor)) {
            throw new CPFInvalidoException(valor);
        }
        Valor = Limpar(valor);
    }
    
    private static bool Validar(string cpf) {
        // Lógica de validação
        return true;
    }
}

// Dinheiro
public class Dinheiro {
    public decimal Valor { get; }
    public string Moeda { get; }
    
    public Dinheiro(decimal valor, string moeda) {
        if (valor < 0) {
            throw new ValorNegativoException();
        }
        Valor = valor;
        Moeda = moeda;
    }
    
    public Dinheiro Somar(Dinheiro outro) {
        if (Moeda != outro.Moeda) {
            throw new MoedasDiferentesException();
        }
        return new Dinheiro(Valor + outro.Valor, Moeda);
    }
}

// Período
public class Periodo {
    public DateTime Inicio { get; }
    public DateTime Fim { get; }
    
    public Periodo(DateTime inicio, DateTime fim) {
        if (fim < inicio) {
            throw new PeriodoInvalidoException();
        }
        Inicio = inicio;
        Fim = fim;
    }
    
    public bool Contem(DateTime data) {
        return data >= Inicio && data <= Fim;
    }
}
```

### Entidades com Value Objects

```csharp
public class Cliente {
    public ClienteId Id { get; }
    public CPF Cpf { get; }  // Value Object
    public Email Email { get; }  // Value Object
    public Endereco Endereco { get; private set; }  // Value Object
    
    public Cliente(ClienteId id, CPF cpf, Email email) {
        Id = id;
        Cpf = cpf;
        Email = email;
    }
    
    public void AlterarEndereco(Endereco novoEndereco) {
        // Substitui o value object antigo pelo novo
        Endereco = novoEndereco;
    }
}
```

## 🔄 Imutabilidade em Value Objects

### Por que Imutáveis?

```csharp
// ❌ RUIM - Mutável
public class Email {
    public string Valor { get; set; }  // Pode mudar
    
    public void Alterar(string novo) {
        Valor = novo;  // Problema: referências antigas ficam desatualizadas
    }
}

// ✅ BOM - Imutável
public class Email {
    public string Valor { get; }  // Readonly
    
    // Não tem setter, não pode mudar
    // Para "mudar", cria novo objeto
}
```

### Substituição ao Invés de Mutação

```csharp
// ✅ BOM - Criar novo ao invés de mudar
var emailAntigo = new Email("joao@email.com");
var emailNovo = new Email("joao.silva@email.com");  // Novo objeto

cliente.AlterarEmail(emailNovo);  // Substitui o antigo
```

## ✅ Verificação

- [ ] Entendeu diferença entre Entidade e Value Object
- [ ] Sabe quando usar cada um
- [ ] Implementou Value Objects imutáveis
- [ ] Implementou Entidades com identidade
- [ ] Viu exemplos práticos
- [ ] Entendeu importância da imutabilidade

---

[← Voltar: Linguagem Ubíqua](./01-linguagem-ubiqua.md) | [Próximo: Aggregates e Repositories →](./03-aggregates-repositories.md)