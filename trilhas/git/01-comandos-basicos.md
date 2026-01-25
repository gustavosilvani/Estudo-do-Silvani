# 01 - Comandos Básicos do Git

## 📖 Comandos Essenciais

Neste módulo, você aprenderá os comandos Git que usará todos os dias como desenvolvedor profissional.

## 🎯 Os 7 Comandos Fundamentais

### 1. `git init` - Inicializar Repositório

```bash
# Criar novo repositório
mkdir meu-projeto
cd meu-projeto
git init

# Resultado: Initialized empty Git repository in /caminho/meu-projeto/.git/
```

**O que acontece:**
- Cria pasta oculta `.git/` com estrutura do repositório
- Transforma pasta comum em repositório Git

### 2. `git status` - Verificar Estado

```bash
git status

# Mostra:
# - Arquivos modificados
# - Arquivos na staging area
# - Branch atual
# - Commits pendentes
```

**Exemplo de saída:**
```
On branch main
Changes not staged for commit:
  modified:   README.md

Untracked files:
  novo-arquivo.txt
```

### 3. `git add` - Adicionar à Staging Area

```bash
# Adicionar arquivo específico
git add arquivo.txt

# Adicionar múltiplos arquivos
git add arquivo1.txt arquivo2.txt

# Adicionar todos os arquivos
git add .

# Adicionar por extensão
git add *.cs

# Adicionar arquivos de uma pasta
git add src/
```

**Estados dos arquivos:**
```
Untracked → git add → Staged → git commit → Committed
    ↓                    ↓
Modified ←───────────────┘
```

### 4. `git commit` - Salvar Mudanças

```bash
# Commit com mensagem inline
git commit -m "feat: adicionar funcionalidade de login"

# Commit abrindo editor
git commit

# Commit pulando staging (add + commit)
git commit -a -m "fix: corrigir bug no cálculo"

# Commit com descrição detalhada
git commit -m "feat: implementar carrinho de compras" -m "- Adicionar itens
- Remover itens
- Calcular total"
```

**Boas práticas de mensagens:**
```bash
# ✅ BOM
git commit -m "feat: adicionar validação de email"
git commit -m "fix: corrigir erro de divisão por zero"
git commit -m "docs: atualizar README com instruções"

# ❌ RUIM
git commit -m "mudanças"
git commit -m "atualizações"
git commit -m "commit"
```

### 5. `git log` - Histórico de Commits

```bash
# Log completo
git log

# Log resumido (uma linha por commit)
git log --oneline

# Log com gráfico
git log --oneline --graph --all

# Log de um arquivo específico
git log -- arquivo.txt

# Log com diff
git log -p

# Últimos N commits
git log -3
```

**Exemplo de saída:**
```
commit abc1234 (HEAD -> main)
Author: João Silva <joao@email.com>
Date:   Sat Jan 25 10:30:00 2025 -0300

    feat: adicionar funcionalidade X
```

### 6. `git diff` - Ver Diferenças

```bash
# Diferenças não staged
git diff

# Diferenças staged
git diff --staged

# Diferença entre commits
git diff abc1234 def5678

# Diferença de arquivo específico
git diff arquivo.txt

# Estatísticas
git diff --stat
```

### 7. `git rm` - Remover Arquivos

```bash
# Remover arquivo do Git e do disco
git rm arquivo.txt

# Remover apenas do Git (manter no disco)
git rm --cached arquivo.txt

# Remover pasta
git rm -r pasta/

# Remover forçado
git rm -f arquivo.txt
```

## 🎯 Fluxo Completo de Trabalho

```bash
# 1. Criar/Modificar arquivos
echo "# Meu Projeto" > README.md

# 2. Verificar status
git status

# 3. Adicionar à staging area
git add README.md

# 4. Verificar o que será commitado
git status

# 5. Fazer commit
git commit -m "docs: adicionar README inicial"

# 6. Verificar histórico
git log --oneline
```

## 📋 Convenção de Commits

### Prefixos Padrão (Conventional Commits)

```bash
feat:     # Nova funcionalidade
fix:      # Correção de bug
docs:     # Documentação
style:    # Formatação (não afeta código)
refactor: # Refatoração
test:     # Adicionar/modificar testes
chore:    # Tarefas de manutenção
perf:     # Melhoria de performance
```

### Exemplos Práticos

```bash
git commit -m "feat: adicionar autenticação JWT"
git commit -m "fix: corrigir validação de CPF"
git commit -m "docs: atualizar guia de instalação"
git commit -m "refactor: simplificar lógica de cálculo"
git commit -m "test: adicionar testes para UserService"
git commit -m "chore: atualizar dependências do projeto"
```

## 🔧 Comandos Úteis Adicionais

### Desfazer Mudanças

```bash
# Descartar mudanças em arquivo (antes de add)
git checkout -- arquivo.txt

# Remover arquivo da staging area
git reset HEAD arquivo.txt

# Desfazer último commit (mantém mudanças)
git reset --soft HEAD~1

# Desfazer último commit (descarta mudanças)
git reset --hard HEAD~1
```

### Modificar Último Commit

```bash
# Adicionar arquivo esquecido ao último commit
git add arquivo-esquecido.txt
git commit --amend --no-edit

# Modificar mensagem do último commit
git commit --amend -m "nova mensagem"
```

### Ignorar Arquivos (.gitignore)

```bash
# Criar .gitignore
echo "bin/" > .gitignore
echo "obj/" >> .gitignore
echo "*.log" >> .gitignore

# Exemplo de .gitignore para C#
cat > .gitignore << EOF
# Build results
bin/
obj/
*.dll
*.exe

# User-specific files
*.suo
*.user

# Visual Studio
.vs/
EOF

git add .gitignore
git commit -m "chore: adicionar .gitignore"
```

## 🎯 Exercício Prático

### Criar um Projeto Completo

```bash
# 1. Criar projeto
mkdir meu-blog
cd meu-blog
git init

# 2. Criar estrutura
mkdir src tests docs
echo "# Meu Blog" > README.md

# 3. Adicionar .gitignore
cat > .gitignore << EOF
bin/
obj/
*.log
EOF

# 4. Primeiro commit
git add .
git status
git commit -m "chore: estrutura inicial do projeto"

# 5. Adicionar código
echo "public class Post {}" > src/Post.cs
git add src/Post.cs
git commit -m "feat: adicionar classe Post"

# 6. Adicionar testes
echo "public class PostTests {}" > tests/PostTests.cs
git add tests/PostTests.cs
git commit -m "test: adicionar testes para Post"

# 7. Verificar histórico
git log --oneline --graph
```

### Exercício: Simular Erro e Corrigir

```bash
# 1. Fazer mudança errada
echo "código errado" >> src/Post.cs
git add src/Post.cs
git commit -m "feat: adicionar funcionalidade"

# 2. Perceber erro
cat src/Post.cs

# 3. Desfazer último commit
git reset --hard HEAD~1

# 4. Verificar que voltou ao estado anterior
cat src/Post.cs

# 5. Fazer correção certa
echo "código correto" >> src/Post.cs
git add src/Post.cs
git commit -m "feat: adicionar funcionalidade corretamente"
```

## 📊 Tabela de Referência Rápida

| Comando | Descrição | Exemplo |
|---------|-----------|---------|
| `git init` | Inicializar repositório | `git init` |
| `git status` | Ver estado atual | `git status` |
| `git add` | Adicionar à staging | `git add arquivo.txt` |
| `git commit` | Salvar mudanças | `git commit -m "mensagem"` |
| `git log` | Ver histórico | `git log --oneline` |
| `git diff` | Ver diferenças | `git diff` |
| `git rm` | Remover arquivo | `git rm arquivo.txt` |
| `git reset` | Desfazer mudanças | `git reset HEAD arquivo.txt` |

## ✅ Verificação

Para prosseguir, certifique-se de:

- [ ] Criou repositório com `git init`
- [ ] Fez pelo menos 3 commits
- [ ] Usou `git status` e `git log`
- [ ] Criou e configurou `.gitignore`
- [ ] Desfez mudanças com sucesso
- [ ] Usou conventional commits

## 🎯 Próximos Passos

No próximo módulo, vamos aprender sobre **Branches e Merges** - trabalhando com múltiplas linhas de desenvolvimento!

---

[← Voltar: Introdução](./00-introducao.md) | [Próximo: Branches e Merges →](./02-branches-e-merges.md)