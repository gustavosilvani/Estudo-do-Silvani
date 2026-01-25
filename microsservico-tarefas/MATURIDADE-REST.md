# 🎯 Maturidade REST - Richardson Maturity Model

## 📊 Modelo de Maturidade (Richardson)

### **Nível 0: The Swamp of POX (Plain Old XML)**
- Um único endpoint
- Um único método HTTP (geralmente POST)
- RPC style

**Exemplo:**
```
POST /api
{
  "method": "getTarefa",
  "id": 123
}
```

❌ **Não é REST**

---

### **Nível 1: Recursos**
- Múltiplos endpoints baseados em recursos
- Ainda usa apenas POST
- Recursos identificados por URI

**Exemplo:**
```
POST /api/tarefas/123
POST /api/usuarios/456
```

⚠️ **REST Básico**

---

### **Nível 2: Verbos HTTP**
- Usa métodos HTTP corretos (GET, POST, PUT, PATCH, DELETE)
- Status codes apropriados (200, 201, 404, etc.)
- Recursos com URIs padronizadas

**Exemplo:**
```
GET    /api/tarefas         -> 200 OK
POST   /api/tarefas         -> 201 Created
GET    /api/tarefas/123     -> 200 OK ou 404 Not Found
PUT    /api/tarefas/123     -> 204 No Content
DELETE /api/tarefas/123     -> 204 No Content
```

✅ **REST Pragmático** (maioria das APIs)

---

### **Nível 3: HATEOAS (Hypermedia)**
- Inclui links de navegação nas respostas
- Cliente descobre ações disponíveis através dos links
- Autodescritível
- Desacoplamento total

**Exemplo:**
```json
{
  "id": "123",
  "titulo": "Tarefa exemplo",
  "status": "Pendente",
  "_links": {
    "self": { "href": "/api/tarefas/123" },
    "concluir": { "href": "/api/tarefas/123/concluir", "method": "PATCH" },
    "cancelar": { "href": "/api/tarefas/123/cancelar", "method": "PATCH" },
    "comentarios": { "href": "/api/tarefas/123/comentarios", "method": "POST" }
  }
}
```

🏆 **REST Maduro** (RESTful completo)

---

## 🎯 Implementação no Microsserviço

**Status Atual:** Nível 2 ✅  
**Objetivo:** Nível 3 (HATEOAS) 🎯

### **Benefícios do Nível 3:**

1. **Autodescritível**: Cliente sabe o que pode fazer
2. **Evoluibilidade**: Mudanças no servidor não quebram cliente
3. **Desacoplamento**: Cliente não precisa conhecer URLs
4. **Descoberta**: Ações disponíveis baseadas no estado

---

## 📚 Referências

- [Richardson Maturity Model](https://martinfowler.com/articles/richardsonMaturityModel.html)
- REST in Practice (O'Reilly)
- RESTful Web APIs (O'Reilly)
