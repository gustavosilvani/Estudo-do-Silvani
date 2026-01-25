# 02 - Branches e Merges

## 📖 O que são Branches?

Branches (ramificações) permitem trabalhar em features separadas sem afetar o código principal. É como criar uma cópia paralela do seu código.

## 🌳 Conceito de Branches

```
main    ●───●───●───●───●
             \         /
feature       ●───●───●
```

**Cenário real:**
- `main`: Código em produção
- `feature/login`: Desenvolvendo autenticação
- `bugfix/payment`: Corrigindo bug no pagamento

## 🎯 Comandos de Branch

### Listar Branches

```bash
# Listar branches locais
git branch

# Listar todas (locais + remotas)
git branch -a

# Listar com últimos commits
git branch -v
```

### Criar Branch

```bash
# Criar branch
git branch nome-da-branch

# Criar e trocar para a branch
git checkout -b nome-da-branch

# Forma moderna (Git 2.23+)
git switch -c nome-da-branch
```

### Trocar de Branch

```bash
# Forma tradicional
git checkout nome-da-branch

# Forma moderna
git switch nome-da-branch

# Voltar para branch anterior
git switch -
```

### Deletar Branch

```bash
# Deletar branch já mergeada
git branch -d nome-da-branch

# Forçar deleção (não mergeada)
git branch -D nome-da-branch
```

## 🔀 Merge - Unindo Branches

### Tipos de Merge

#### 1. Fast-Forward Merge

```
Antes:
main    ●───●
             \
feature       ●───●

Depois:
main    ●───●───●───●
```

```bash
# Na branch main
git checkout main
git merge feature

# Resultado: Fast-forward
```

#### 2. Three-Way Merge

```
Antes:
main    ●───●───●
             \
feature       ●───●

Depois:
main    ●───●───●───●  (merge commit)
             \     /
feature       ●───●
```

```bash
git checkout main
git merge feature -m "Merge feature into main"
```

### Estratégias de Merge

```bash
# Merge normal (com commit de merge)
git merge feature

# Merge com fast-forward apenas se possível
git merge --ff-only feature

# Merge sem fast-forward (sempre cria commit)
git merge --no-ff feature

# Merge com squash (combina commits)
git merge --squash feature
```

## ⚔️ Conflitos de Merge

### O que são Conflitos?

Quando duas branches modificam as mesmas linhas de código:

```
main:    "versão A"
feature: "versão B"
Git:     "Não sei qual escolher!"
```

### Resolvendo Conflitos

**1. Git identifica conflito:**
```bash
git merge feature
# Auto-merging arquivo.txt
# CONFLICT (content): Merge conflict in arquivo.txt
```

**2. Arquivo com conflito:**
```csharp
public class Usuario {
<<<<<<< HEAD
    public string Nome { get; set; }  // versão main
=======
    public string NomeCompleto { get; set; }  // versão feature
>>>>>>> feature
}
```

**3. Resolver manualmente:**
```csharp
// Escolher uma versão ou combinar
public class Usuario {
    public string NomeCompleto { get; set; }
}
```

**4. Finalizar merge:**
```bash
git add arquivo.txt
git commit -m "fix: resolver conflito de merge"
```

## 🎯 Fluxo Completo de Trabalho

### Exemplo: Desenvolver Nova Feature

```bash
# 1. Criar branch para feature
git checkout main
git pull  # Garantir que está atualizado
git checkout -b feature/adicionar-login

# 2. Desenvolver feature
echo "código do login" > login.cs
git add login.cs
git commit -m "feat: implementar sistema de login"

# 3. Mais commits na feature
echo "testes do login" > login-tests.cs
git add login-tests.cs
git commit -m "test: adicionar testes de login"

# 4. Voltar para main e fazer merge
git checkout main
git merge feature/adicionar-login

# 5. Deletar branch (opcional)
git branch -d feature/adicionar-login
```

### Exemplo: Resolver Conflito

```bash
# Situação: main e feature modificaram mesmo arquivo

# 1. Tentar merge
git checkout main
git merge feature/pagamento

# 2. Git reporta conflito
# Auto-merging pagamento.cs
# CONFLICT (content): Merge conflict in pagamento.cs

# 3. Abrir arquivo e resolver
code pagamento.cs  # ou seu editor

# 4. Marcar como resolvido
git add pagamento.cs

# 5. Finalizar merge
git commit -m "merge: integrar feature de pagamento"
```

## 🔧 Comandos Úteis

### Ver Diferenças Entre Branches

```bash
# Ver commits diferentes
git log main..feature

# Ver arquivos diferentes
git diff main feature

# Ver mudanças em arquivo específico
git diff main feature -- arquivo.txt
```

### Abortar Merge

```bash
# Se conflito é muito complexo
git merge --abort

# Volta ao estado anterior ao merge
```

### Cherry-Pick (Pegar Commit Específico)

```bash
# Aplicar commit específico de outra branch
git cherry-pick abc1234
```

## 🎯 Boas Práticas

### Nomenclatura de Branches

```bash
# ✅ BOM
git checkout -b feature/adicionar-carrinho
git checkout -b bugfix/corrigir-login
git checkout -b hotfix/erro-pagamento
git checkout -b release/v1.2.0

# ❌ RUIM
git checkout -b minha-branch
git checkout -b teste
git checkout -b branch1
```

### Convenções

```
feature/   # Nova funcionalidade
bugfix/    # Correção de bug
hotfix/    # Correção urgente em produção
release/   # Preparação de release
docs/      # Documentação
refactor/  # Refatoração
```

### Fluxo Recomendado

```bash
# 1. Sempre atualizar main antes de criar branch
git checkout main
git pull
git checkout -b feature/nova-funcionalidade

# 2. Commits frequentes
git add .
git commit -m "feat: ..."

# 3. Manter branch atualizada
git checkout main
git pull
git checkout feature/nova-funcionalidade
git merge main

# 4. Merge quando pronto
git checkout main
git merge feature/nova-funcionalidade

# 5. Limpar branch antiga
git branch -d feature/nova-funcionalidade
```

## 🎯 Exercício Prático

### Criar e Mergear Múltiplas Branches

```bash
# Setup
mkdir blog-project
cd blog-project
git init
echo "# Blog" > README.md
git add README.md
git commit -m "chore: projeto inicial"

# Feature 1: Posts
git checkout -b feature/posts
echo "public class Post {}" > Post.cs
git add Post.cs
git commit -m "feat: adicionar classe Post"
git checkout main
git merge feature/posts

# Feature 2: Comentários
git checkout -b feature/comments
echo "public class Comment {}" > Comment.cs
git add Comment.cs
git commit -m "feat: adicionar classe Comment"
git checkout main
git merge feature/comments

# Verificar histórico
git log --oneline --graph --all
```

### Exercício: Simular e Resolver Conflito

```bash
# 1. Criar arquivo na main
git checkout main
echo "Versão inicial" > arquivo.txt
git add arquivo.txt
git commit -m "feat: adicionar arquivo"

# 2. Modificar em branch1
git checkout -b branch1
echo "Modificação branch1" > arquivo.txt
git add arquivo.txt
git commit -m "feat: modificar arquivo (branch1)"

# 3. Modificar em branch2
git checkout main
git checkout -b branch2
echo "Modificação branch2" > arquivo.txt
git add arquivo.txt
git commit -m "feat: modificar arquivo (branch2)"

# 4. Merge branch1 (OK)
git checkout main
git merge branch1

# 5. Merge branch2 (CONFLITO!)
git merge branch2
# CONFLICT! Resolver manualmente

# 6. Resolver conflito
echo "Versão final combinada" > arquivo.txt
git add arquivo.txt
git commit -m "merge: resolver conflito"

# 7. Ver resultado
git log --oneline --graph --all
```

## ✅ Verificação

Para prosseguir, certifique-se de:

- [ ] Criou pelo menos 3 branches
- [ ] Fez merge de branches com sucesso
- [ ] Resolveu pelo menos 1 conflito
- [ ] Deletou branches após merge
- [ ] Usou nomenclatura adequada
- [ ] Entendeu diferença entre fast-forward e three-way merge

## 🎯 Próximos Passos

No próximo módulo, vamos aprender sobre **Colaboração em Equipe** - trabalhando com repositórios remotos!

---

[← Voltar: Comandos Básicos](./01-comandos-basicos.md) | [Próximo: Colaboração em Equipe →](./03-colaboracao-equipe.md)