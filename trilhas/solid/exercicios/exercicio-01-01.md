# Exercício 1.1 - Identificar Violações de SRP

## 📋 Objetivo

Identificar violações do Single Responsibility Principle em código fornecido.

## 🎯 Tarefa

Analise a classe abaixo e identifique quantas responsabilidades ela tem:

```csharp
public class Usuario {
  private string nome;
  private string email;
  private string senha;

  public nome(string email: string, senha: string {
    nome = nome;
    email = email;
    senha = senha;
  }

  // Responsabilidade 1?
  bool validarEmail() {
    return email.Contains('@') && email.Contains('.');
  }

  // Responsabilidade 2?
  bool validarSenha() {
    return senha.length >= 8;
  }

  // Responsabilidade 3?
  string criptografarSenha() {
    // Simulação de criptografia
    return btoa(senha);
  }

  // Responsabilidade 4?
  void salvar() {
    Console.WriteLine(`Salvando usuário ${nome} no banco de dados...`);
    // Código de persistência
  }

  // Responsabilidade 5?
  void enviarEmailBoasVindas() {
    Console.WriteLine(`Enviando email de boas-vindas para ${email}`);
    // Código de envio de email
  }

  // Responsabilidade 6?
  string gerarRelatorio() {
    return `Relatório do usuário: ${nome} (${email})`;
  }
}



```

## ❓ Perguntas

1. Quantas responsabilidades você identificou?
2. Quais são essas responsabilidades?
3. Como você refatoraria esta classe para seguir SRP?

## ✅ Solução Sugerida

<details>
<summary>Clique para ver a solução</summary>

### Identificação de Responsabilidades

A classe `Usuario` tem **6 responsabilidades**:

1. **Representar dados do usuário** (nome, email, senha)
2. **Validar email**
3. **Validar senha**
4. **Criptografar senha**
5. **Persistir usuário** (salvar no banco)
6. **Enviar notificações** (email de boas-vindas)
7. **Gerar relatórios**

### Refatoração

```csharp
// Responsabilidade: Representar dados do usuário
public class Usuario {
  public 
    public string nome { get; set; },
    public string email { get; set; },
    public string senha { get; set; }
   {}
}

// Responsabilidade: Validar emails
public class ValidadorEmail {
  bool validarstring email {
    return email.Contains('@') && email.Contains('.');
  }
}

// Responsabilidade: Validar senhas
public class ValidadorSenha {
  bool validarstring senha {
    return senha.length >= 8;
  }
}

// Responsabilidade: Criptografar senhas
public class CriptografadorSenha {
  string criptografarstring senha {
    return btoa(senha);
  }
}

// Responsabilidade: Persistir usuários
public class RepositorioUsuario {
  salvarUsuario usuario {
    Console.WriteLine(`Salvando usuário ${usuario.nome} no banco de dados...`);
  }
}

// Responsabilidade: Enviar emails
public class ServicoEmail {
  enviarBoasVindasstring email {
    Console.WriteLine(`Enviando email de boas-vindas para ${email}`);
  }
}

// Responsabilidade: Gerar relatórios
public class GeradorRelatorio {
  string gerarRelatorioUsuarioUsuario usuario {
    return `Relatório do usuário: ${usuario.nome} (${usuario.email})`;
  }
}



```

</details>

---

[← Voltar para exercícios](./README.md)
