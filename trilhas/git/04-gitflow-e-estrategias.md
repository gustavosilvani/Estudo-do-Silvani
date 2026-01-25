# 04 - GitFlow e Estratégias de Branching

## 📖 O que é GitFlow?

GitFlow é uma estratégia de branching criada por Vincent Driessen, amplamente adotada em projetos profissionais.

## 🌳 Estrutura do GitFlow

```
main (produção)    ●────────●────────●
                    \      / \      /
release              ●────●   ●────●
                    /          \
develop           ●───●───●───●───●
                   \   \     /   /
feature/A           ●───●───●   /
feature/B                ●───●──
```

### Branches Principais

#### 1. `main` (ou `master`)
- **Código em produção**
- Sempre estável e deployável
- Apenas merges de `release` e `hotfix`
- Cada merge = nova versão em produção

#### 2. `develop`
- **Código em desenvolvimento**
- Integração de todas as features
- Base para criar `release` branches

### Branches de Suporte

#### 3. `feature/*`
- **Novas funcionalidades**
- Criadas a partir de `develop`
- Mergeadas de volta para `develop`
- Nomenclatura: `feature/nome-da-funcionalidade`

#### 4. `release/*`
- **Preparação para produção**
- Criadas a partir de `develop`
- Apenas bugfixes e ajustes finais
- Merge para `main` E `develop`
- Nomenclatura: `release/v1.2.0`

#### 5. `hotfix/*`
- **Correções urgentes em produção**
- Criadas a partir de `main`
- Merge para `main` E `develop`
- Nomenclatura: `hotfix/corrigir-bug-critico`

## 🎯 Fluxo Completo do GitFlow

### 1. Desenvolver Nova Feature

```bash
# Iniciar feature
git checkout develop
git pull
git checkout -b feature/adicionar-carrinho

# Desenvolver
echo "código do carrinho" > carrinho.cs
git add carrinho.cs
git commit -m "feat: implementar carrinho de compras"

echo "testes do carrinho" > carrinho-tests.cs
git add carrinho-tests.cs
git commit -m "test: adicionar testes do carrinho"

# Finalizar feature
git checkout develop
git merge feature/adicionar-carrinho
git push
git branch -d feature/adicionar-carrinho
```

### 2. Criar Release

```bash
# Iniciar release
git checkout develop
git pull
git checkout -b release/v1.2.0

# Ajustes finais
echo "1.2.0" > version.txt
git add version.txt
git commit -m "chore: atualizar versão para 1.2.0"

# Bugfixes encontrados em QA
git add fix.cs
git commit -m "fix: corrigir bug encontrado em QA"

# Finalizar release
# 1. Merge para main
git checkout main
git merge release/v1.2.0
git tag -a v1.2.0 -m "Release 1.2.0"
git push origin main --tags

# 2. Merge de volta para develop
git checkout develop
git merge release/v1.2.0
git push

# 3. Deletar branch de release
git branch -d release/v1.2.0
```

### 3. Hotfix Urgente

```bash
# Bug crítico em produção!

# Iniciar hotfix
git checkout main
git pull
git checkout -b hotfix/corrigir-pagamento

# Corrigir bug
git add pagamento.cs
git commit -m "fix: corrigir erro crítico no pagamento"

# Finalizar hotfix
# 1. Merge para main
git checkout main
git merge hotfix/corrigir-pagamento
git tag -a v1.2.1 -m "Hotfix 1.2.1 - Correção pagamento"
git push origin main --tags

# 2. Merge para develop
git checkout develop
git merge hotfix/corrigir-pagamento
git push

# 3. Deletar branch de hotfix
git branch -d hotfix/corrigir-pagamento
```

## 🔧 Git Flow Extension

### Instalação

```bash
# macOS
brew install git-flow

# Linux (Ubuntu/Debian)
sudo apt-get install git-flow

# Windows
# Incluído no Git for Windows
```

### Inicializar GitFlow

```bash
git flow init

# Perguntas (pode aceitar padrões):
# Branch name for production releases: [main]
# Branch name for "next release" development: [develop]
# Feature branches? [feature/]
# Release branches? [release/]
# Hotfix branches? [hotfix/]
# Support branches? [support/]
# Version tag prefix? []
```

### Comandos GitFlow

#### Features

```bash
# Iniciar feature
git flow feature start adicionar-login

# Publicar feature (push para remoto)
git flow feature publish adicionar-login

# Finalizar feature (merge para develop)
git flow feature finish adicionar-login
```

#### Releases

```bash
# Iniciar release
git flow release start 1.2.0

# Publicar release
git flow release publish 1.2.0

# Finalizar release (merge para main e develop)
git flow release finish 1.2.0
```

#### Hotfixes

```bash
# Iniciar hotfix
git flow hotfix start corrigir-bug

# Finalizar hotfix (merge para main e develop)
git flow hotfix finish corrigir-bug
```

## 📋 Estratégias Alternativas

### 1. GitHub Flow (Mais Simples)

```
main  ●───●───●───●───●
       \   \ / \ /   /
feature  ●──● ●─● ●──
```

**Regras:**
- Apenas `main` como branch principal
- Features criadas de `main`
- Deploy contínuo de `main`
- Pull Requests obrigatórios

```bash
# Workflow
git checkout main
git pull
git checkout -b feature/nova-funcionalidade
# ... desenvolver ...
git push -u origin feature/nova-funcionalidade
# Criar PR no GitHub
# Após aprovação e merge, deletar branch
```

### 2. Trunk-Based Development

```
main  ●───●───●───●───●───●
       \  /\  /\  /\  /
short   ●─  ●─  ●─  ●─
```

**Características:**
- Commits diretos ou branches muito curtas (<1 dia)
- Integração contínua agressiva
- Feature flags para features incompletas

### 3. Release Flow (Microsoft)

```
main    ●───────────●───────────●
         \         / \         /
release   ●───────●   ●───────●
```

**Características:**
- `main` sempre deployável
- `release/*` para versões específicas
- Hotfixes aplicados na release e cherry-picked para main

## 🎯 Comparação de Estratégias

| Aspecto | GitFlow | GitHub Flow | Trunk-Based |
|---------|---------|-------------|-------------|
| **Complexidade** | Alta | Baixa | Média |
| **Branches** | 5 tipos | 2 tipos | 1-2 tipos |
| **Deploy** | Releases planejados | Contínuo | Contínuo |
| **Ideal para** | Releases programados | Deploys frequentes | CI/CD avançado |
| **Aprendizado** | Mais difícil | Mais fácil | Médio |

## 🎯 Boas Práticas

### 1. Nomenclatura Consistente

```bash
# ✅ BOM
feature/adicionar-autenticacao
feature/sistema-notificacoes
bugfix/corrigir-validacao-email
hotfix/erro-critico-pagamento
release/v2.1.0

# ❌ RUIM
nova-feature
correção
branch1
teste
```

### 2. Commits Semânticos

```bash
# Features
feat: adicionar sistema de login
feat(api): criar endpoint de usuários

# Bugfixes
fix: corrigir validação de CPF
fix(pagamento): resolver erro de timeout

# Hotfixes
hotfix: corrigir erro crítico no checkout

# Release
release: preparar versão 1.2.0
chore(version): atualizar para 1.2.0
```

### 3. Pull Requests Detalhados

```markdown
## Descrição
Implementação do sistema de autenticação com JWT

## Tipo de Mudança
- [ ] Bugfix
- [x] Nova feature
- [ ] Breaking change
- [ ] Documentação

## Checklist
- [x] Testes passando
- [x] Code review solicitado
- [x] Documentação atualizada
- [x] Sem console.log/debugs
- [x] Seguiu padrões do projeto

## Como Testar
1. Executar `dotnet test`
2. Iniciar aplicação
3. Fazer login com usuário teste
4. Verificar token JWT no retorno
```

## 🎯 Exercício Prático Completo

### Simular Projeto Real com GitFlow

```bash
# Setup
git init meu-ecommerce
cd meu-ecommerce

# Configurar GitFlow
git flow init  # Aceitar padrões

# Estrutura inicial
echo "# E-commerce" > README.md
git add README.md
git commit -m "chore: projeto inicial"
git push -u origin develop

# Feature 1: Produtos
git flow feature start produtos
echo "public class Produto {}" > Produto.cs
git add Produto.cs
git commit -m "feat: adicionar entidade Produto"
git flow feature finish produtos

# Feature 2: Carrinho
git flow feature start carrinho
echo "public class Carrinho {}" > Carrinho.cs
git add Carrinho.cs
git commit -m "feat: adicionar sistema de carrinho"
git flow feature finish carrinho

# Preparar Release
git flow release start 1.0.0
echo "1.0.0" > version.txt
git add version.txt
git commit -m "chore: versão 1.0.0"
git flow release finish 1.0.0

# Simular Hotfix
git flow hotfix start bug-critico
echo "fix aplicado" >> Produto.cs
git add Produto.cs
git commit -m "hotfix: corrigir bug crítico em Produto"
git flow hotfix finish bug-critico

# Ver resultado
git log --oneline --graph --all --decorate
```

## ✅ Verificação

Para prosseguir, certifique-se de:

- [ ] Entendeu diferença entre GitFlow, GitHub Flow e Trunk-Based
- [ ] Criou branches de feature, release e hotfix
- [ ] Usou nomenclatura adequada
- [ ] Instalou e testou git-flow extension
- [ ] Simulou workflow completo de release
- [ ] Criou tags de versão apropriadas

## 🎯 Conclusão da Trilha Git

**Parabéns! Você completou a trilha de Git!**

Agora você domina:
- ✅ Comandos básicos
- ✅ Branches e merges
- ✅ Colaboração em equipe
- ✅ Estratégias profissionais (GitFlow)

**Próximo passo:** Aplicar Git em projetos reais das próximas trilhas!

---

[← Voltar: Colaboração em Equipe](./03-colaboracao-equipe.md) | [Voltar à Trilha](./README.md)