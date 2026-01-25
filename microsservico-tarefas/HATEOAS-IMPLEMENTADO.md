# 🎯 Microsserviço com HATEOAS - Nível 3 REST

## 🏆 **RICHARDSON MATURITY MODEL - NÍVEL 3**

Este microsserviço agora implementa o **nível máximo de maturidade REST** com **HATEOAS** (Hypermedia as the Engine of Application State).

---

## 📊 **Evolução dos Níveis**

### **Antes: Nível 2** ✅
```json
{
  "id": "123",
  "titulo": "Implementar feature",
  "status": "Pendente"
}
```
✅ Recursos bem definidos  
✅ Verbos HTTP corretos  
✅ Status codes apropriados  
❌ Cliente precisa conhecer todas as URLs

### **Agora: Nível 3** 🏆
```json
{
  "id": "123",
  "titulo": "Implementar feature",
  "status": "Pendente",
  "_links": {
    "self": { "href": "/api/v1/tarefas/123", "method": "GET" },
    "iniciar": { "href": "/api/v1/tarefas/123/iniciar", "method": "PATCH" },
    "cancelar": { "href": "/api/v1/tarefas/123/cancelar", "method": "PATCH" },
    "comentarios": { "href": "/api/v1/tarefas/123/comentarios", "method": "POST" },
    "delete": { "href": "/api/v1/tarefas/123", "method": "DELETE" },
    "todas-tarefas": { "href": "/api/v1/tarefas", "method": "GET" }
  }
}
```
✅ **Links dinâmicos baseados no estado**  
✅ **Cliente descobre ações disponíveis**  
✅ **Autodescritível**  
✅ **Desacoplamento total**

---

## 🎯 **Links Dinâmicos por Estado**

### **Tarefa Pendente:**
```json
"_links": {
  "self": { "href": "/api/v1/tarefas/123", "method": "GET" },
  "iniciar": { "href": "/api/v1/tarefas/123/iniciar", "method": "PATCH" },
  "cancelar": { "href": "/api/v1/tarefas/123/cancelar", "method": "PATCH" },
  "delete": { "href": "/api/v1/tarefas/123", "method": "DELETE" }
}
```

### **Tarefa Em Andamento:**
```json
"_links": {
  "self": { "href": "/api/v1/tarefas/123", "method": "GET" },
  "concluir": { "href": "/api/v1/tarefas/123/concluir", "method": "PATCH" },
  "cancelar": { "href": "/api/v1/tarefas/123/cancelar", "method": "PATCH" }
}
```

### **Tarefa Concluída:**
```json
"_links": {
  "self": { "href": "/api/v1/tarefas/123", "method": "GET" },
  "delete": { "href": "/api/v1/tarefas/123", "method": "DELETE" },
  "todas-tarefas": { "href": "/api/v1/tarefas", "method": "GET" }
}
```

---

## 🚀 **Como Usar**

### **1. Ponto de Entrada (API Root)**
```bash
curl http://localhost:5000/api/v1
```

**Resposta:**
```json
{
  "message": "Task Management API v1 - HATEOAS (Nível 3 REST)",
  "versao": "1.0.0",
  "maturidade": "Richardson Level 3 - HATEOAS",
  "_links": {
    "self": { "href": "/api/v1", "method": "GET" },
    "tarefas": { "href": "/api/v1/tarefas", "method": "GET" },
    "criarTarefa": { "href": "/api/v1/tarefas", "method": "POST" },
    "documentacao": { "href": "/", "method": "GET" }
  }
}
```

### **2. Listar Tarefas**
```bash
curl http://localhost:5000/api/v1/tarefas
```

**Resposta:**
```json
{
  "total": 2,
  "items": [
    {
      "id": "abc-123",
      "titulo": "Tarefa 1",
      "status": "Pendente",
      "_links": {
        "self": { "href": "/api/v1/tarefas/abc-123", "method": "GET" },
        "iniciar": { "href": "/api/v1/tarefas/abc-123/iniciar", "method": "PATCH" }
      }
    }
  ],
  "_links": {
    "self": { "href": "/api/v1/tarefas", "method": "GET" },
    "criar": { "href": "/api/v1/tarefas", "method": "POST" }
  }
}
```

### **3. Criar Tarefa**
```bash
curl -X POST http://localhost:5000/api/v1/tarefas \
  -H "Content-Type: application/json" \
  -d '{
    "titulo": "Nova tarefa",
    "descricao": "Descrição",
    "prioridade": 2
  }'
```

**Resposta (201 Created):**
```json
{
  "id": "new-123",
  "titulo": "Nova tarefa",
  "status": "Pendente",
  "_links": {
    "self": { "href": "/api/v1/tarefas/new-123", "method": "GET" },
    "iniciar": { "href": "/api/v1/tarefas/new-123/iniciar", "method": "PATCH" },
    "cancelar": { "href": "/api/v1/tarefas/new-123/cancelar", "method": "PATCH" },
    "delete": { "href": "/api/v1/tarefas/new-123", "method": "DELETE" }
  }
}
```

### **4. Seguir Links (HATEOAS em Ação)**
```bash
# 1. Cliente busca tarefa
curl http://localhost:5000/api/v1/tarefas/abc-123

# 2. Vê que pode "iniciar" nos links
# 3. Usa o link fornecido
curl -X PATCH http://localhost:5000/api/v1/tarefas/abc-123/iniciar

# 4. Agora links mudaram! Pode "concluir"
curl -X PATCH http://localhost:5000/api/v1/tarefas/abc-123/concluir
```

---

## 💡 **Benefícios HATEOAS**

### **1. Desacoplamento**
```javascript
// ❌ Cliente acoplado (Nível 2)
const url = `/api/tarefas/${id}/concluir`;

// ✅ Cliente desacoplado (Nível 3)
const url = tarefa._links.concluir.href;
```

### **2. Evoluibilidade**
```
Servidor muda URL: /tarefas → /tasks
Cliente não quebra! Usa links da resposta.
```

### **3. Descoberta de Funcionalidades**
```javascript
// Cliente verifica se ação está disponível
if (tarefa._links.concluir) {
  // Mostrar botão "Concluir"
  mostrarBotao(tarefa._links.concluir);
}
```

### **4. Autodocumentação**
```
Cliente não precisa ler documentação.
Links dizem o que é possível fazer.
```

---

## 📐 **Arquitetura HATEOAS**

```
┌─────────────────────────────────────────┐
│          Cliente (Frontend)             │
│  - Não conhece URLs hardcoded           │
│  - Segue links da resposta              │
│  - Adapta UI baseado em links           │
└────────────┬────────────────────────────┘
             │
             │ HTTP + JSON com _links
             │
┌────────────▼────────────────────────────┐
│         Controller (HATEOAS)            │
│  - Adiciona links nas respostas         │
│  - Links baseados no estado             │
│  - Usa IUrlHelper para gerar URLs       │
└────────────┬────────────────────────────┘
             │
┌────────────▼────────────────────────────┐
│      HateoasLinkBuilder                 │
│  - Lógica de links por estado           │
│  - Reutilizável                         │
│  - Extensível                           │
└─────────────────────────────────────────┘
```

---

## 🎯 **Exemplo Real de Uso**

### **Frontend Inteligente:**
```javascript
// Buscar tarefa
const tarefa = await fetch('/api/v1/tarefas/123').then(r => r.json());

// UI se adapta aos links disponíveis
const botoes = [];

if (tarefa._links.iniciar) {
  botoes.push({
    label: 'Iniciar',
    url: tarefa._links.iniciar.href,
    method: tarefa._links.iniciar.method
  });
}

if (tarefa._links.concluir) {
  botoes.push({
    label: 'Concluir',
    url: tarefa._links.concluir.href,
    method: tarefa._links.concluir.method
  });
}

// Renderizar apenas botões disponíveis
renderizarBotoes(botoes);
```

---

## 🏆 **Comparação de Maturidade**

| Aspecto | Nível 0 | Nível 1 | Nível 2 | Nível 3 |
|---------|---------|---------|---------|---------|
| **Múltiplos Recursos** | ❌ | ✅ | ✅ | ✅ |
| **HTTP Verbs** | ❌ | ❌ | ✅ | ✅ |
| **Status Codes** | ❌ | ❌ | ✅ | ✅ |
| **HATEOAS** | ❌ | ❌ | ❌ | ✅ |
| **Desacoplamento** | Baixo | Médio | Alto | **Máximo** |
| **Evoluibilidade** | Baixa | Média | Alta | **Máxima** |
| **Complexidade** | Baixa | Média | Média | Alta |

---

## 📚 **Padrões Implementados**

✅ **Richardson Maturity Model - Level 3**  
✅ **HATEOAS (Hypermedia as the Engine of Application State)**  
✅ **HAL (Hypertext Application Language)** - Formato de links  
✅ **Self-descriptive messages**  
✅ **Uniform Interface**  
✅ **Stateless**  
✅ **Resource-based**  

---

## 🎉 **Resultado Final**

### **Antes:**
- API RESTful nível 2 (boa)
- Cliente precisa conhecer todas as URLs
- Mudanças podem quebrar clientes

### **Agora:**
- API RESTful nível 3 (excelente)
- Cliente descobre URLs dinamicamente
- Servidor pode evoluir sem quebrar clientes
- **Maturidade REST Máxima** 🏆

---

## 🚀 **Teste Agora!**

```bash
# Acesse o ponto de entrada
curl http://localhost:5000/api/v1

# Siga os links
curl http://localhost:5000/api/v1/tarefas

# Crie uma tarefa e veja os links
curl -X POST http://localhost:5000/api/v1/tarefas \
  -H "Content-Type: application/json" \
  -d '{"titulo":"Teste HATEOAS","descricao":"Testando nível 3","prioridade":2}'
```

---

**📖 Documentação Swagger:** http://localhost:5000  
**🎯 API Root (Entry Point):** http://localhost:5000/api/v1

---

**🏆 Agora temos um microsserviço com o NÍVEL MÁXIMO de maturidade REST!**
