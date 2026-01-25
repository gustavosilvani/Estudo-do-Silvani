# 🚚 Microsserviço de Logística - Solução C# Completa

Este é um projeto completo de microsserviço de logística implementado em C# aplicando todos os princípios SOLID. O projeto demonstra como construir software manutenível, extensível e testável.

## 📋 Funcionalidades

- ✅ **Rastreamento de Pedidos**: Criar, consultar e atualizar status de entregas
- ✅ **Cálculo de Frete**: Múltiplas estratégias (padrão, expresso, econômico, grátis)
- ✅ **Gestão de Estoque**: Verificar disponibilidade e reservar produtos
- ✅ **Notificações**: Suporte a Email, SMS, Push e WhatsApp
- ✅ **Persistência**: Abstração para diferentes tipos de banco de dados

## 🎯 Princípios SOLID Aplicados

### SRP - Single Responsibility Principle
Cada classe tem uma única responsabilidade:
- `RastreamentoService`: Apenas rastreamento de pedidos
- `CalculadoraFrete`: Apenas cálculo de frete
- `EstoqueService`: Apenas gestão de estoque
- `NotificacaoService`: Apenas orquestração de notificações

### OCP - Open/Closed Principle
Aberto para extensão, fechado para modificação:
- `IEstrategiaFrete`: Adicionar novos tipos de frete sem modificar código existente
- `INotificador`: Adicionar novos canais de notificação sem modificar código

### LSP - Liskov Substitution Principle
Substituição de objetos sem quebrar o comportamento:
- Todas as implementações de `IEstrategiaFrete` são substituíveis
- Todas as implementações de `INotificador` são substituíveis

### ISP - Interface Segregation Principle
Interfaces segregadas por responsabilidade:
- `ILeitorEstoque` e `IEscritorEstoque` separados
- `INotificador` simples, sem métodos desnecessários

### DIP - Dependency Inversion Principle
Dependências invertidas:
- Serviços dependem de interfaces, não de implementações concretas
- Alto nível controla quais implementações usar

## 🏗️ Arquitetura

```
MicrosservicoLogistica/
├── Domain/              # Entidades de domínio
│   ├── Pedido.cs
│   ├── ItemPedido.cs
│   ├── Endereco.cs
│   └── StatusEntrega.cs
├── Services/            # Serviços de aplicação (SRP)
│   ├── RastreamentoService.cs
│   ├── CalculadoraFrete.cs
│   ├── EstoqueService.cs
│   ├── NotificacaoService.cs
│   └── INotificacaoService.cs
├── Strategies/          # Estratégias de cálculo (OCP)
│   ├── IEstrategiaFrete.cs
│   ├── FretePadrao.cs
│   ├── FreteExpresso.cs
│   ├── FreteEconomico.cs
│   └── FreteGratis.cs
├── Repositories/        # Abstrações de persistência (DIP, ISP)
│   ├── IRepositorioPedido.cs
│   ├── ILeitorEstoque.cs
│   ├── IEscritorEstoque.cs
│   ├── RepositorioPedidoBD.cs
│   ├── LeitorEstoqueBD.cs
│   └── EscritorEstoqueBD.cs
├── Notifications/       # Sistema de notificações (OCP, LSP, ISP)
│   ├── INotificador.cs
│   ├── NotificadorEmail.cs
│   ├── NotificadorSMS.cs
│   ├── NotificadorPush.cs
│   └── NotificadorWhatsApp.cs
├── Program.cs           # Ponto de entrada e demonstração
├── MicrosservicoLogistica.csproj
└── README.md
```

## 🚀 Como Executar

### Pré-requisitos
- .NET 8.0 ou superior

### Passos
1. **Clone/baixe** este projeto
2. **Navegue** até a pasta do projeto:
   ```bash
   cd solucao-microsservico-logistica
   ```
3. **Execute** o projeto:
   ```bash
   dotnet run
   ```

### O que acontece na execução:
1. Cria um pedido de exemplo com 2 itens
2. Verifica disponibilidade no estoque
3. Reserva os produtos
4. Calcula o frete usando estratégia padrão
5. Cria rastreamento e envia notificações
6. Atualiza status para "Em Trânsito"
7. Atualiza status para "Entregue"
8. Demonstra diferentes estratégias de frete
9. Mostra como adicionar novo canal de notificação

## 🧪 Testabilidade

Cada componente pode ser testado isoladamente graças aos princípios SOLID:

```csharp
// Exemplo de teste unitário
[Test]
public void CriarRastreamento_DeveGerarCodigoUnico()
{
    // Arrange
    var mockRepositorio = new Mock<IRepositorioPedido>();
    var mockNotificacao = new Mock<INotificacaoService>();
    var pedido = new Pedido { Id = "PED001" };

    mockRepositorio.Setup(r => r.BuscarPorId("PED001")).Returns(pedido);

    var service = new RastreamentoService(mockRepositorio.Object, mockNotificacao.Object);

    // Act
    var codigo = service.CriarRastreamento("PED001");

    // Assert
    Assert.IsNotNull(codigo);
    Assert.IsTrue(codigo.StartsWith("TRK"));
}
```

## 🔧 Extensibilidade

### Adicionar Novo Tipo de Frete

```csharp
// 1. Criar nova estratégia
public class FreteInternacional : IEstrategiaFrete
{
    public string TipoFrete => "Internacional";
    public decimal Calcular(Pedido pedido) => /* lógica específica */;
}

// 2. Usar sem modificar código existente
var freteInternacional = new FreteInternacional();
var calculadora = new CalculadoraFrete(freteInternacional);
```

### Adicionar Novo Canal de Notificação

```csharp
// 1. Criar novo notificador
public class NotificadorTelegram : INotificador
{
    public string Canal => "Telegram";
    public void Enviar(string destinatario, string mensagem) => /* lógica */;
}

// 2. Adicionar à lista sem modificar NotificacaoService
notificadores.Add(new NotificadorTelegram());
```

## 📊 Benefícios Demonstrados

| Aspecto | Antes (Sem SOLID) | Depois (Com SOLID) |
|---------|-------------------|-------------------|
| **Manutenibilidade** | Mudanças afetam múltiplas áreas | Mudanças isoladas |
| **Extensibilidade** | Precisa modificar código existente | Adiciona sem modificar |
| **Testabilidade** | Testes complexos | Testes unitários isolados |
| **Flexibilidade** | Difícil trocar implementações | Fácil trocar implementações |
| **Reutilização** | Código acoplado | Componentes reutilizáveis |

## 📚 Documentação Relacionada

Este projeto é acompanhado de documentação completa:

- **[Código Completo com Comentários](../trilhas/solid/recursos/microsservico-logistica-codigo.md)** - Código fonte comentado explicando cada princípio
- **[Documentação Explicativa](../trilhas/solid/recursos/microsservico-logistica-documentacao.md)** - Análise detalhada da aplicação dos princípios SOLID

## 🎓 Uso Educacional

Este projeto serve como exemplo prático para:

- **Aprender SOLID**: Cada princípio aplicado de forma concreta
- **Padrões de Design**: Strategy, Repository, Dependency Injection
- **Arquitetura de Software**: Camadas, responsabilidades, dependências
- **Boas Práticas**: Código limpo, testável e manutenível
- **Microsserviços**: Separação de responsabilidades, interfaces claras

## 📝 Licença

Este projeto é parte da Escola de Estudo e está disponível para fins educacionais.

---

**🚀 Execute `dotnet run` e veja os princípios SOLID em ação!**