# ✅ Boas Práticas Gerais

Boas práticas aplicáveis a todas as trilhas de estudo.

## 📝 Código

### Nomenclatura

- Use nomes descritivos e significativos
- Evite abreviações desnecessárias
- Use convenções consistentes (camelCase, PascalCase, etc.)
- Nomes devem revelar intenção

### Funções e Métodos

- Mantenha funções pequenas e focadas
- Uma função deve fazer uma coisa
- Evite efeitos colaterais
- Prefira funções puras quando possível

### Classes

- Classes devem ser pequenas e coesas
- Uma classe deve ter uma única responsabilidade
- Evite classes "God Object" (que fazem tudo)
- Use composição ao invés de herança quando apropriado

## 🏗️ Arquitetura

### Separação de Responsabilidades

- Separe lógica de negócio de apresentação
- Separe lógica de negócio de acesso a dados
- Use camadas apropriadas (Domain, Application, Infrastructure)

### Dependências

- Dependa de abstrações, não de implementações
- Use injeção de dependência
- Evite acoplamento forte entre módulos
- Mantenha dependências mínimas

## 🧪 Testes

### Estrutura

- Escreva testes antes ou junto com o código (TDD)
- Mantenha testes simples e legíveis
- Um teste deve verificar uma coisa
- Use nomes descritivos para testes

### Cobertura

- Teste casos felizes (happy path)
- Teste casos de erro
- Teste casos extremos (edge cases)
- Mantenha cobertura adequada, mas não obsessiva

## 📚 Documentação

### Código

- Código deve ser auto-documentado
- Use comentários apenas quando necessário
- Comentários devem explicar "por quê", não "o quê"
- Mantenha comentários atualizados

### Documentação Técnica

- Documente decisões arquiteturais importantes
- Mantenha README atualizado
- Documente APIs públicas
- Use exemplos práticos

## 🔄 Refatoração

### Quando Refatorar

- Quando código está difícil de entender
- Quando há duplicação
- Quando há violação de princípios
- Quando adicionar funcionalidade é difícil

### Como Refatorar

- Refatore em pequenos passos
- Mantenha testes passando
- Faça uma mudança por vez
- Teste após cada mudança

## 🎯 Princípios Gerais

### KISS (Keep It Simple, Stupid)

- Prefira soluções simples
- Evite complexidade desnecessária
- Não otimize prematuramente

### DRY (Don't Repeat Yourself)

- Elimine duplicação de código
- Extraia código comum
- Mas não force abstrações desnecessárias

### YAGNI (You Aren't Gonna Need It)

- Não adicione funcionalidade "por precaução"
- Implemente apenas o que é necessário agora
- Confie na refatoração futura

### SOLID

- Siga os princípios SOLID
- Aplique consistentemente
- Use como guia, não como dogma

## 🔍 Code Review

### O que Procurar

- Legibilidade e clareza
- Aderência a padrões estabelecidos
- Possíveis bugs ou problemas
- Oportunidades de melhoria

### Feedback

- Seja construtivo e respeitoso
- Explique o "por quê"
- Sugira alternativas quando apropriado
- Reconheça boas práticas

## 🛠️ Ferramentas

### Linters e Formatters

- Use linters para manter qualidade
- Configure formatters para consistência
- Integre no processo de desenvolvimento
- Mantenha configurações atualizadas

### Versionamento

- Faça commits frequentes e pequenos
- Use mensagens de commit descritivas
- Mantenha histórico limpo
- Use branches para features

---

[← Voltar para o README principal](../README.md)
