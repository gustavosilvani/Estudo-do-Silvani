# 03 - Colaboração em Equipe

## 📖 Trabalhando com Repositórios Remotos

Git não é apenas local - sua força está na colaboração. Vamos aprender a trabalhar em equipe usando GitHub, GitLab ou Bitbucket.

## 🌐 Conceito de Repositório Remoto

```
Local (Seu PC)          Remoto (GitHub)
    ●───●───●    ───push───>    ●───●───●
    ●───●───●    <──pull────    ●───●───●
```

## 🎯 Comandos de Repositório Remoto

### Conectar a Repositório Remoto

```bash
# Ver repositórios remotos
git remote -v

# Adicionar repositório remoto
git remote add origin https://github.com/usuario/projeto.git

# Verificar conexão
git remote show origin

# Renomear remoto
git remote rename origin upstream

# Remover remoto
git remote remove origin
```

### Clone - Copiar Repositório

```bash
# Clonar repositório
git clone https://github.com/usuario/projeto.git

# Clonar com nome diferente
git clone https://github.com/usuario/projeto.git meu-projeto

# Clonar branch específica
git clone -b develop https://github.com/usuario/projeto.git
```

## 📤 Push - Enviar Mudanças

### Sintaxe Básica

```bash
# Push primeira vez (criar branch remota)
git push -u origin main

# Push normal
git push

# Push branch específica
git push origin feature/login

# Push todas branches
git push --all

# Push com tags
git push --tags
```

### Push Forçado (Cuidado!)

```bash
# Force push (sobrescreve histórico remoto)
git push --force

# Force push mais seguro (não sobrescreve se houver commits novos)
git push --force-with-lease
```

## 📥 Pull - Receber Mudanças

### Sintaxe Básica

```bash
# Pull da branch atual
git pull

# Pull de branch específica
git pull origin main

# Pull com rebase
git pull --rebase

# Pull sem fazer merge automático
git fetch
git merge origin/main
```

### Fetch - Baixar sem Mergear

```bash
# Baixar mudanças sem aplicar
git fetch

# Baixar de repositório específico
git fetch origin

# Ver o que foi baixado
git log HEAD..origin/main

# Aplicar mudanças depois
git merge origin/main
```

## 🔀 Pull Request / Merge Request

### Fluxo Completo

```bash
# 1. Criar branch para feature
git checkout -b feature/nova-funcionalidade

# 2. Desenvolver e commitar
git add .
git commit -m "feat: implementar funcionalidade X"

# 3. Push da branch
git push -u origin feature/nova-funcionalidade

# 4. Criar Pull Request no GitHub/GitLab
# (via interface web)

# 5. Code review e aprovação

# 6. Merge no main via interface

# 7. Atualizar local
git checkout main
git pull

# 8. Deletar branch
git branch -d feature/nova-funcionalidade
git push origin --delete feature/nova-funcionalidade
```

## 👥 Fluxo de Trabalho em Equipe

### Cenário: Dois Desenvolvedores

**Dev A:**
```bash
# 1. Pegar código mais recente
git pull

# 2. Criar branch
git checkout -b feature/pagamento

# 3. Desenvolver
echo "código pagamento" > pagamento.cs
git add pagamento.cs
git commit -m "feat: adicionar sistema de pagamento"

# 4. Push
git push -u origin feature/pagamento
```

**Dev B (ao mesmo tempo):**
```bash
# 1. Pegar código mais recente
git pull

# 2. Criar branch
git checkout -b feature/notificacao

# 3. Desenvolver
echo "código notificação" > notificacao.cs
git add notificacao.cs
git commit -m "feat: adicionar sistema de notificação"

# 4. Push
git push -u origin feature/notificacao
```

**Tech Lead (merge):**
```bash
# Revisar PRs e mergear na ordem

# Atualizar local
git pull

# Ver todas branches remotas
git branch -r
```

## 🔄 Manter Branch Atualizada

### Atualizar Feature Branch

```bash
# Enquanto desenvolve feature, main avança

# Opção 1: Merge
git checkout feature/minha-feature
git merge main

# Opção 2: Rebase (mais limpo)
git checkout feature/minha-feature
git rebase main
```

### Resolver Conflitos Durante Pull

```bash
# Pull encontra conflito
git pull
# CONFLICT in arquivo.txt

# Resolver conflitos
code arquivo.txt

# Marcar como resolvido
git add arquivo.txt

# Continuar pull
git commit -m "merge: resolver conflitos"
```

## 🏷️ Tags - Versionamento

### Criar Tags

```bash
# Tag simples
git tag v1.0.0

# Tag anotada (recomendada)
git tag -a v1.0.0 -m "Release 1.0.0 - Nova funcionalidade X"

# Tag em commit específico
git tag -a v0.9.0 abc1234 -m "Release 0.9.0"
```

### Gerenciar Tags

```bash
# Listar tags
git tag

# Ver detalhes da tag
git show v1.0.0

# Push tags
git push origin v1.0.0
git push --tags  # todas tags

# Deletar tag local
git tag -d v1.0.0

# Deletar tag remota
git push origin --delete v1.0.0
```

## 🎯 Boas Práticas de Colaboração

### 1. Sempre Pull Antes de Push

```bash
# ✅ BOM
git pull
git push

# ❌ RUIM
git push  # Pode dar conflito se outros commitaram
```

### 2. Branches Curtas e Focadas

```bash
# ✅ BOM - Feature específica
git checkout -b feature/adicionar-botao-login

# ❌ RUIM - Feature genérica
git checkout -b melhorias
```

### 3. Commits Frequentes

```bash
# ✅ BOM - Commits pequenos e frequentes
git add login.cs
git commit -m "feat: adicionar validação de email"

git add login-tests.cs
git commit -m "test: adicionar testes de validação"

# ❌ RUIM - Commit gigante no final
git add .
git commit -m "tudo pronto"
```

### 4. Code Review

```markdown
# Checklist de PR

- [ ] Código segue padrões do projeto
- [ ] Testes passando
- [ ] Sem console.log / debugs
- [ ] Documentação atualizada
- [ ] Sem conflitos com main
```

## 🚨 Problemas Comuns e Soluções

### Problema 1: Push Rejeitado

```bash
# Erro: Updates were rejected

# Solução:
git pull --rebase
git push
```

### Problema 2: Branch Desatualizada

```bash
# Feature branch muito atrasada em relação a main

# Solução:
git checkout feature/minha-feature
git fetch origin
git rebase origin/main
git push --force-with-lease
```

### Problema 3: Commit no Branch Errado

```bash
# Fez commit na main ao invés de feature

# Solução:
# 1. Criar branch com commits
git branch feature/nova-funcionalidade

# 2. Voltar main ao estado anterior
git reset --hard origin/main

# 3. Continuar na branch correta
git checkout feature/nova-funcionalidade
```

## 🎯 Exercício Prático

### Simular Colaboração em Equipe

```bash
# Setup: Criar repositório no GitHub
# 1. Criar repositório "projeto-colaborativo"
# 2. Clonar localmente

git clone https://github.com/seu-usuario/projeto-colaborativo.git
cd projeto-colaborativo

# Dev 1: Feature A
git checkout -b feature/usuario
echo "public class Usuario {}" > Usuario.cs
git add Usuario.cs
git commit -m "feat: adicionar classe Usuario"
git push -u origin feature/usuario

# Dev 2 (simulado): Trabalhar em paralelo
git checkout main
git checkout -b feature/produto
echo "public class Produto {}" > Produto.cs
git add Produto.cs
git commit -m "feat: adicionar classe Produto"
git push -u origin feature/produto

# Mergear features (via PR no GitHub)
# Depois:
git checkout main
git pull

# Verificar histórico
git log --oneline --graph --all
```

### Exercício: Resolver Conflito Remoto

```bash
# Situação: Você e colega modificam mesmo arquivo

# 1. Criar arquivo
echo "linha 1" > config.txt
git add config.txt
git commit -m "feat: adicionar config"
git push

# 2. Dev A modifica
git checkout -b dev-a
echo "linha 2 - Dev A" >> config.txt
git add config.txt
git commit -m "feat: adicionar config A"
git push -u origin dev-a

# 3. Dev B modifica (simular em outra branch)
git checkout main
git checkout -b dev-b
echo "linha 2 - Dev B" >> config.txt
git add config.txt
git commit -m "feat: adicionar config B"
git push -u origin dev-b

# 4. Mergear dev-a no main (OK)
git checkout main
git merge dev-a
git push

# 5. Tentar mergear dev-b (CONFLITO!)
git merge dev-b
# Resolver conflito
code config.txt
git add config.txt
git commit -m "merge: resolver conflito entre dev-a e dev-b"
git push
```

## ✅ Verificação

Para prosseguir, certifique-se de:

- [ ] Conectou repositório local a remoto
- [ ] Fez push e pull com sucesso
- [ ] Criou pelo menos 1 Pull Request
- [ ] Trabalhou com múltiplas branches remotas
- [ ] Resolveu conflito em pull
- [ ] Criou tags de versão

## 🎯 Próximos Passos

No próximo módulo, vamos aprender sobre **GitFlow e Estratégias** - workflows profissionais para equipes!

---

[← Voltar: Branches e Merges](./02-branches-e-merges.md) | [Próximo: GitFlow e Estratégias →](./04-gitflow-e-estrategias.md)