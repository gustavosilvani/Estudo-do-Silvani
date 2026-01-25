# 🎯 Git - Controle de Versão

## 📖 Sobre

Esta trilha ensina controle de versão com Git, ferramenta essencial para qualquer desenvolvedor profissional. Aprenda desde comandos básicos até estratégias avançadas de colaboração em equipe.

## 🎯 Objetivos de Aprendizado

Ao final desta trilha, você será capaz de:

- Dominar comandos básicos do Git (init, add, commit, push, pull)
- Trabalhar com branches e merges
- Resolver conflitos de merge
- Colaborar efetivamente em equipe
- Implementar GitFlow em projetos reais
- Usar conventional commits e pull requests

## 📋 Pré-requisitos

- Conhecimento básico de linha de comando
- Conta no GitHub/GitLab/Bitbucket
- Editor de código (VS Code, etc.)

## 📚 Módulos

### 📘 Nível Básico
1. **[00 - Introdução ao Git](./00-introducao.md)** - O que é Git e por que usar
2. **[01 - Comandos Básicos](./01-comandos-basicos.md)** - init, add, commit, status, log
3. **[02 - Branches e Merges](./02-branches-e-merges.md)** - Ramificações e fusões

### 📗 Nível Intermediário
4. **[03 - Colaboração em Equipe](./03-colaboracao-equipe.md)** - Push, pull, fetch, clone
5. **[04 - GitFlow e Estratégias](./04-gitflow-e-estrategias.md)** - Workflows avançados

## 🎓 Progressão Sugerida

Siga os módulos em ordem. Cada conceito constrói sobre o anterior, e os exercícios práticos consolidam o aprendizado.

**Tempo estimado**: 8-10 horas

## ✅ Checklist de Progresso

- [ ] Módulo 00 - Introdução ao Git
- [ ] Módulo 01 - Comandos Básicos
- [ ] Módulo 02 - Branches e Merges
- [ ] Módulo 03 - Colaboração em Equipe
- [ ] Módulo 04 - GitFlow e Estratégias

## 💻 Ambiente de Desenvolvimento

### Instalação do Git

**Windows:**
```bash
# Via Chocolatey
choco install git

# Ou baixar do site oficial
# https://git-scm.com/download/win
```

**macOS:**
```bash
# Via Homebrew
brew install git

# Ou via Xcode Command Line Tools
xcode-select --install
```

**Linux:**
```bash
# Ubuntu/Debian
sudo apt update && sudo apt install git

# CentOS/RHEL
sudo yum install git
```

### Configuração Inicial

```bash
# Configurar nome e email
git config --global user.name "Seu Nome"
git config --global user.email "seu.email@exemplo.com"

# Configurar editor padrão
git config --global core.editor "code --wait"

# Verificar configuração
git config --list
```

## 📝 Exercícios Práticos

Cada módulo inclui exercícios práticos que você pode fazer em um repositório real no GitHub.

### Criando um Repositório de Exercícios

```bash
# Criar pasta para exercícios
mkdir git-exercicios
cd git-exercicios

# Inicializar repositório
git init

# Criar arquivo inicial
echo "# Meus Exercícios de Git" > README.md
git add README.md
git commit -m "feat: adicionar README inicial"
```

## 🔧 Ferramentas Recomendadas

### GUI para Git
- **GitHub Desktop** - Interface amigável para Windows/macOS
- **GitKraken** - Cliente Git avançado
- **VS Code** - Integração nativa com Git
- **Sourcetree** - Cliente Git da Atlassian

### Extensões VS Code
- GitLens
- Git Graph
- Git History

## 🌟 Boas Práticas

### Commits
- Use mensagens descritivas em português ou inglês
- Siga conventional commits quando possível
- Commite mudanças pequenas e coesas

### Branches
- Use nomes descritivos em português ou inglês
- Delete branches após merge
- Mantenha branch main/master limpo

### Colaboração
- Sempre faça pull antes de push
- Resolva conflitos localmente quando possível
- Use pull requests para grandes mudanças

## 📚 Recursos Adicionais

### Documentação Oficial
- [Documentação Git](https://git-scm.com/doc)
- [Pro Git Book](https://git-scm.com/book/pt-br)

### Cursos Online
- [Git na Prática - Udemy](https://www.udemy.com/course/git-e-github-na-pratica/)
- [Git Complete - Coursera](https://www.coursera.org/learn/git)

### Comunidades
- [Stack Overflow - Git](https://stackoverflow.com/questions/tagged/git)
- [Reddit - r/git](https://www.reddit.com/r/git/)

## 🔗 Integração com Outras Trilhas

Git é fundamental para todas as outras trilhas:

- **C#/.NET**: Controle versão de projetos .NET
- **SOLID**: Versionar refatorações e melhorias
- **Testes**: Controlar evolução dos testes
- **DDD**: Versionar evolução do domínio
- **Projeto Final**: Colaboração em equipe

## 🚨 Dicas Importantes

### Evite Erros Comuns
- Não commite arquivos grandes (>100MB)
- Não commite senhas ou chaves API
- Sempre verifique `git status` antes de commit
- Use `.gitignore` apropriado para cada tecnologia

### Recuperação de Erros
```bash
# Desfazer último commit (mantém mudanças)
git reset --soft HEAD~1

# Desfazer último commit (remove mudanças)
git reset --hard HEAD~1

# Recuperar arquivo deletado
git checkout HEAD -- arquivo.txt
```

---

[← Voltar ao índice principal](../../INDEX.md)