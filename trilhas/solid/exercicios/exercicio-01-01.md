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

  public Usuario(string nome, string email, string senha) {
    this.nome = nome;
    this.email = email;
    this.senha = senha;
  }

  // Responsabilidade 1: Validação de email
  public bool ValidarEmail() {
    return email.Contains('@') && email.Contains('.');
  }

  // Responsabilidade 2: Validação de senha
  public bool ValidarSenha() {
    return senha.Length >= 8;
  }

  // Responsabilidade 3: Criptografia de senha
  public string CriptografarSenha() {
    // Simulação de criptografia usando Base64
    return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(senha));
  }

  // Responsabilidade 4: Persistência
  public void Salvar() {
    Console.WriteLine($"Salvando usuário {nome} no banco de dados...");
    // Código de persistência
  }

  // Responsabilidade 5: Notificação
  public void EnviarEmailBoasVindas() {
    Console.WriteLine($"Enviando email de boas-vindas para {email}");
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
  public string Nome { get; set; }
  public string Email { get; set; }
  public string Senha { get; set; }
}

// Responsabilidade: Validar emails
public class ValidadorEmail {
  public bool Validar(string email) {
    return email.Contains('@') && email.Contains('.');
  }
}

// Responsabilidade: Validar senhas
public class ValidadorSenha {
  public bool Validar(string senha) {
    return senha.Length >= 8;
  }
}

// Responsabilidade: Criptografar senhas
public class CriptografadorSenha {
  public string Criptografar(string senha) {
    return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(senha));
  }
}

// Responsabilidade: Persistir usuários
public class RepositorioUsuario {
  public void Salvar(Usuario usuario) {
    Console.WriteLine($"Salvando usuário {usuario.Nome} no banco de dados...");
  }
}

// Responsabilidade: Enviar emails
public class ServicoEmail {
  public void EnviarBoasVindas(string email) {
    Console.WriteLine($"Enviando email de boas-vindas para {email}");
  }
}

// Responsabilidade: Gerar relatórios
public class GeradorRelatorio {
  public string GerarRelatorio(Usuario usuario) {
    return $"Relatório do usuário: {usuario.Nome} ({usuario.Email})";
  }
}




```

</details>

---

[← Voltar para exercícios](./README.md)
