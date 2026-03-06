# 05 - Aplicação Prática

Este módulo indica **onde cada padrão aparece** nos projetos deste repositório, para você estudar código real.

## Resumo por Projeto

### Microsserviço de Logística (`solucao-microsservico-logistica/`)

| Padrão        | Onde encontrar |
|---------------|----------------|
| **Strategy**  | `Strategies/` — `ICalculoFrete`, `FretePadrao`, `FreteExpresso`, `FreteEconomico`, `FreteGratis`; seleção da estratégia no fluxo do pedido |
| **Factory**   | Criação de estratégias ou de objetos de domínio conforme tipo de frete |
| **Repository**| Acesso a pedidos/entidades (se houver camada de persistência) |
| **Dependency Injection** | Registro de estratégias e serviços no `Program.cs` |

### Microsserviço de Tarefas (`microsservico-tarefas/`)

| Padrão           | Onde encontrar |
|------------------|----------------|
| **Repository**   | `TaskManagement.Infrastructure/Repositories/TarefaRepository.cs`, interface em `Domain/Interfaces/ITarefaRepository.cs` |
| **Unit of Work** | `TaskManagement.Infrastructure/Repositories/UnitOfWork.cs`, `Domain/Interfaces/IUnitOfWork.cs` |
| **Result Pattern** | `TaskManagement.Domain/Common/Result.cs` — retornos sem exceção |
| **Factory (Factory Method)** | Método estático de criação em entidades (ex.: `Tarefa.Criar`) |
| **HATEOAS**      | Links nos DTOs e no controller (padrão de API, não GoF) |

### Projeto Final E-commerce (`projeto-final-ecommerce/`)

| Padrão           | Onde encontrar |
|------------------|----------------|
| **Repository**   | Camada Infrastructure — repositórios por agregado/entidade |
| **Unit of Work** | Unit of Work na Infrastructure, usado nos serviços de aplicação |
| **Result Pattern** | Uso em serviços para retorno padronizado |
| **DDD**          | Domain com entidades, value objects; Application orquestrando com repositórios e UoW |

## Como estudar

1. **Strategy**: Abra uma estratégia de frete no microsserviço de logística e o ponto onde ela é escolhida e injetada.
2. **Repository + Unit of Work**: Abra `ITarefaRepository`, `TarefaRepository` e `UnitOfWork` no microsserviço de tarefas; depois veja o uso em `TarefaService`.
3. **Result**: Veja `Result.cs` e como os métodos do `TarefaService` retornam `Result<T>` em vez de lançar exceções.

## Checklist de revisão

- [ ] Identifiquei pelo menos um padrão **Strategy** no código
- [ ] Encontrei **Repository** e **Unit of Work** em um dos projetos
- [ ] Entendi como o **Result Pattern** é usado nas respostas de serviço
- [ ] Revisei a relação entre esses padrões e os princípios **SOLID** (especialmente OCP e DIP)

---

[← Voltar ao índice da trilha Design Patterns](./README.md) | [← Voltar ao índice principal](../../INDEX.md)
