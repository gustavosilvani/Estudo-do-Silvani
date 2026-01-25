# 🚚 Solução Microsserviço de Logística - Download e Execução

## 📦 Download

Baixe o arquivo **`solucao-microsservico-logistica.zip`** que contém a solução completa do microsserviço de logística implementado em C# aplicando todos os princípios SOLID.

## 📋 Conteúdo da Solução

```
solucao-microsservico-logistica/
├── Domain/              # Entidades de domínio
│   ├── Pedido.cs        # Entidade principal do pedido
│   ├── ItemPedido.cs    # Item individual do pedido
│   ├── Endereco.cs      # Endereço de entrega
│   └── StatusEntrega.cs # Enum de status
├── Services/            # Serviços de aplicação (SRP)
│   ├── RastreamentoService.cs    # Gestão de rastreamento
│   ├── CalculadoraFrete.cs       # Cálculo de frete
│   ├── EstoqueService.cs         # Gestão de estoque
│   ├── NotificacaoService.cs     # Orquestração de notificações
│   └── INotificacaoService.cs    # Interface do serviço
├── Strategies/          # Estratégias de cálculo (OCP)
│   ├── IEstrategiaFrete.cs       # Interface base
│   ├── FretePadrao.cs            # Frete padrão
│   ├── FreteExpresso.cs          # Frete expresso
│   ├── FreteEconomico.cs         # Frete econômico
│   └── FreteGratis.cs            # Frete grátis
├── Repositories/        # Abstrações de persistência (DIP, ISP)
│   ├── IRepositorioPedido.cs     # Interface repositório pedidos
│   ├── ILeitorEstoque.cs         # Interface leitura estoque
│   ├── IEscritorEstoque.cs       # Interface escrita estoque
│   ├── RepositorioPedidoBD.cs    # Implementação BD pedidos
│   ├── LeitorEstoqueBD.cs        # Implementação leitura estoque
│   └── EscritorEstoqueBD.cs      # Implementação escrita estoque
├── Notifications/       # Sistema de notificações (OCP, LSP, ISP)
│   ├── INotificador.cs           # Interface base
│   ├── NotificadorEmail.cs       # Notificação por email
│   ├── NotificadorSMS.cs         # Notificação por SMS
│   ├── NotificadorPush.cs        # Notificação push
│   └── NotificadorWhatsApp.cs    # Notificação WhatsApp
├── Program.cs           # Demonstração e ponto de entrada
├── MicrosservicoLogistica.csproj # Arquivo de projeto .NET
└── README.md            # Documentação detalhada
```

## 🚀 Como Executar

### Pré-requisitos
- **.NET 9.0** ou superior instalado
- Sistema operacional: Windows, Linux ou macOS

### Passos de Execução

1. **Extraia** o arquivo ZIP:
   ```
   unzip solucao-microsservico-logistica.zip
   # ou use o explorador de arquivos para extrair
   ```

2. **Navegue** até a pasta extraída:
   ```bash
   cd solucao-microsservico-logistica
   ```

3. **Execute** o projeto:
   ```bash
   dotnet run
   ```

### O que Acontece na Execução

O programa demonstra um fluxo completo de logística:

1. **Criação do Pedido** - Pedido com 2 itens (Notebook + Mouse)
2. **Verificação de Estoque** - Confirma disponibilidade
3. **Reserva de Produtos** - Reserva itens no estoque
4. **Cálculo de Frete** - Usa estratégia padrão (R$ 16,00)
5. **Criação de Rastreamento** - Gera código único e envia notificações
6. **Atualização de Status** - Muda para "Em Trânsito" e "Entregue"
7. **Demonstração de Extensibilidade** - Mostra diferentes tipos de frete
8. **Adição de Novo Canal** - Demonstra como adicionar WhatsApp

## 🎯 Demonstração dos Princípios SOLID

### SRP - Single Responsibility Principle
Cada serviço tem uma responsabilidade única:
- `RastreamentoService` → Apenas rastreamento
- `CalculadoraFrete` → Apenas cálculo de frete
- `EstoqueService` → Apenas gestão de estoque

### OCP - Open/Closed Principle
Extensível sem modificar código existente:
- **Frete**: Adicione `FreteInternacional` sem alterar `CalculadoraFrete`
- **Notificações**: Adicione `NotificadorTelegram` sem alterar `NotificacaoService`

### LSP - Liskov Substitution Principle
Substituição segura de implementações:
- Qualquer `IEstrategiaFrete` pode substituir outra
- Qualquer `INotificador` pode substituir outro

### ISP - Interface Segregation Principle
Interfaces segregadas por responsabilidade:
- `ILeitorEstoque` vs `IEscritorEstoque` (separados)
- `INotificador` simples e focado

### DIP - Dependency Inversion Principle
Dependências invertidas:
- Serviços dependem de interfaces, não implementações
- Alto nível controla quais implementações usar

## 🧪 Testabilidade

Cada componente pode ser testado isoladamente:

```csharp
// Exemplo de teste unitário
[Test]
public void CalcularFrete_ComFreteExpresso_DeveRetornarValorMaior()
{
    var estrategia = new FreteExpresso();
    var calculadora = new CalculadoraFrete(estrategia);
    var pedido = new Pedido { Itens = new List<ItemPedido> { ... } };

    var valor = calculadora.CalcularFrete(pedido);

    Assert.AreEqual(40.00m, valor);
}
```

## 🔧 Personalização

### Adicionar Novo Tipo de Frete

```csharp
public class FreteInternacional : IEstrategiaFrete
{
    public string TipoFrete => "Internacional";
    public decimal Calcular(Pedido pedido) => /* lógica específica */;
}
```

### Adicionar Novo Canal de Notificação

```csharp
public class NotificadorTelegram : INotificador
{
    public string Canal => "Telegram";
    public void Enviar(string destinatario, string mensagem) => /* lógica */;
}
```

## 📚 Recursos Educacionais

Esta solução acompanha documentação completa:

- **[Documentação Detalhada](../trilhas/solid/recursos/microsservico-logistica-documentacao.md)** - Explicação de cada princípio aplicado
- **[Código Comentado](../trilhas/solid/recursos/microsservico-logistica-codigo.md)** - Código fonte com explicações

## 🎓 Uso Educacional

- **Aprenda SOLID**: Veja princípios aplicados na prática
- **Estude Arquitetura**: Entenda organização de microsserviços
- **Pratique Padrões**: Repository, Strategy, Dependency Injection
- **Desenvolva Testes**: Componentes isolados e testáveis

## 📝 Notas Técnicas

- **Framework**: .NET 9.0 (compatível com .NET 8.0+)
- **Paradigma**: Programação Orientada a Objetos
- **Padrões**: Strategy, Repository, Dependency Injection
- **Persistência**: Simulada em memória (fácil trocar por SQL/NoSQL)
- **Notificações**: Simuladas no console (fácil trocar por APIs reais)

## 🚀 Próximos Passos

Após executar e entender o código, você pode:

1. **Adicionar testes unitários** com NUnit ou xUnit
2. **Implementar persistência real** (SQL Server, PostgreSQL, MongoDB)
3. **Integrar APIs externas** (correios, gateways de pagamento)
4. **Adicionar mais estratégias** de frete e notificações
5. **Implementar autenticação/autorização**
6. **Adicionar logging** estruturado
7. **Criar APIs REST** com ASP.NET Core

---

**🎯 Execute `dotnet run` e veja os princípios SOLID em ação!**

*Esta solução faz parte da Escola de Estudo - Trilha SOLID*