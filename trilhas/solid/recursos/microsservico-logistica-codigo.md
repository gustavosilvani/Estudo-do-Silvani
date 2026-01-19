# 🚚 Microsserviço de Logística - Código Completo

Este documento contém o código completo de um microsserviço de logística implementado em C# aplicando todos os princípios SOLID. Cada seção do código está comentada indicando qual princípio está sendo aplicado.

## 📋 Estrutura do Projeto

```
MicrosservicoLogistica/
├── Domain/              # Entidades de domínio
├── Services/            # Serviços de aplicação (SRP)
├── Strategies/          # Estratégias de cálculo (OCP)
├── Repositories/        # Abstrações de persistência (DIP, ISP)
├── Notifications/       # Sistema de notificações (OCP, LSP, ISP)
└── Program.cs           # Composição e ponto de entrada
```

---

## 📦 Domain - Entidades de Domínio

```csharp
// ============================================
// DOMAIN - Entidades de Domínio
// ============================================

namespace MicrosservicoLogistica.Domain
{
    // Enum para status de entrega
    public enum StatusEntrega
    {
        Pendente,
        Preparando,
        EmTransito,
        Entregue,
        Cancelado
    }

    // Entidade de endereço
    public class Endereco
    {
        public string Cep { get; set; }
        public string Rua { get; set; }
        public string Numero { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string Pais { get; set; }
    }

    // Entidade de item do pedido
    public class ItemPedido
    {
        public string ProdutoId { get; set; }
        public string Nome { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
    }

    // Entidade principal de pedido
    public class Pedido
    {
        public string Id { get; set; }
        public string ClienteId { get; set; }
        public List<ItemPedido> Itens { get; set; }
        public Endereco EnderecoEntrega { get; set; }
        public StatusEntrega Status { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataEntrega { get; set; }
        public string CodigoRastreamento { get; set; }
        public decimal ValorFrete { get; set; }

        public Pedido()
        {
            Itens = new List<ItemPedido>();
            Status = StatusEntrega.Pendente;
            DataCriacao = DateTime.Now;
        }
    }
}
```

---

## 🎯 Services - Serviços de Aplicação (SRP)

### SRP: Cada serviço tem uma única responsabilidade

```csharp
// ============================================
// SERVICES - Serviços de Aplicação
// SRP: Cada serviço tem UMA única responsabilidade
// ============================================

namespace MicrosservicoLogistica.Services
{
    // ============================================
    // SRP: RastreamentoService - Responsabilidade única: Rastreamento de pedidos
    // ============================================
    public class RastreamentoService
    {
        private readonly IRepositorioPedido _repositorio;
        private readonly INotificacaoService _notificacaoService;

        // DIP: Depende de abstrações (interfaces), não de implementações concretas
        public RastreamentoService(
            IRepositorioPedido repositorio,
            INotificacaoService notificacaoService)
        {
            _repositorio = repositorio;
            _notificacaoService = notificacaoService;
        }

        // Responsabilidade única: Criar rastreamento de pedido
        public string CriarRastreamento(string pedidoId)
        {
            var pedido = _repositorio.BuscarPorId(pedidoId);
            if (pedido == null)
                throw new Exception("Pedido não encontrado");

            // Gera código de rastreamento único
            pedido.CodigoRastreamento = $"TRK{pedidoId.Substring(0, 8).ToUpper()}{DateTime.Now:yyyyMMdd}";
            pedido.Status = StatusEntrega.Preparando;

            _repositorio.Atualizar(pedido);

            // Delega notificação para serviço especializado
            _notificacaoService.NotificarCriacaoRastreamento(pedido);

            return pedido.CodigoRastreamento;
        }

        // Responsabilidade única: Atualizar status de entrega
        public void AtualizarStatus(string codigoRastreamento, StatusEntrega novoStatus)
        {
            var pedido = _repositorio.BuscarPorCodigoRastreamento(codigoRastreamento);
            if (pedido == null)
                throw new Exception("Pedido não encontrado");

            var statusAnterior = pedido.Status;
            pedido.Status = novoStatus;

            if (novoStatus == StatusEntrega.Entregue)
                pedido.DataEntrega = DateTime.Now;

            _repositorio.Atualizar(pedido);

            // Delega notificação para serviço especializado
            _notificacaoService.NotificarAtualizacaoStatus(pedido, statusAnterior, novoStatus);
        }

        // Responsabilidade única: Consultar rastreamento
        public Pedido ConsultarRastreamento(string codigoRastreamento)
        {
            return _repositorio.BuscarPorCodigoRastreamento(codigoRastreamento);
        }
    }

    // ============================================
    // SRP: CalculadoraFrete - Responsabilidade única: Cálculo de frete
    // ============================================
    public class CalculadoraFrete
    {
        private readonly IEstrategiaFrete _estrategiaFrete;

        // DIP: Depende de abstração IEstrategiaFrete, não de implementação concreta
        // OCP: Permite adicionar novas estratégias sem modificar esta classe
        public CalculadoraFrete(IEstrategiaFrete estrategiaFrete)
        {
            _estrategiaFrete = estrategiaFrete;
        }

        // Responsabilidade única: Calcular valor do frete
        public decimal CalcularFrete(Pedido pedido)
        {
            if (pedido == null || pedido.Itens == null || pedido.Itens.Count == 0)
                throw new ArgumentException("Pedido inválido para cálculo de frete");

            // Delega cálculo para estratégia específica (OCP)
            return _estrategiaFrete.Calcular(pedido);
        }
    }

    // ============================================
    // SRP: EstoqueService - Responsabilidade única: Gestão de estoque
    // ============================================
    public class EstoqueService
    {
        private readonly ILeitorEstoque _leitorEstoque;
        private readonly IEscritorEstoque _escritorEstoque;

        // DIP + ISP: Depende de interfaces segregadas
        // ISP: Não força implementar métodos de leitura se só precisa escrever
        public EstoqueService(
            ILeitorEstoque leitorEstoque,
            IEscritorEstoque escritorEstoque)
        {
            _leitorEstoque = leitorEstoque;
            _escritorEstoque = escritorEstoque;
        }

        // Responsabilidade única: Verificar disponibilidade
        public bool VerificarDisponibilidade(string produtoId, int quantidade)
        {
            var estoqueAtual = _leitorEstoque.ObterQuantidade(produtoId);
            return estoqueAtual >= quantidade;
        }

        // Responsabilidade única: Reservar produtos
        public void ReservarProdutos(List<ItemPedido> itens)
        {
            foreach (var item in itens)
            {
                if (!VerificarDisponibilidade(item.ProdutoId, item.Quantidade))
                    throw new Exception($"Produto {item.ProdutoId} sem estoque suficiente");

                _escritorEstoque.Reservar(item.ProdutoId, item.Quantidade);
            }
        }

        // Responsabilidade única: Liberar reserva
        public void LiberarReserva(List<ItemPedido> itens)
        {
            foreach (var item in itens)
            {
                _escritorEstoque.LiberarReserva(item.ProdutoId, item.Quantidade);
            }
        }
    }

    // ============================================
    // SRP: NotificacaoService - Responsabilidade única: Orquestração de notificações
    // ============================================
    public class NotificacaoService
    {
        private readonly List<INotificador> _notificadores;

        // DIP: Depende de lista de abstrações INotificador
        // OCP: Pode adicionar novos notificadores sem modificar esta classe
        public NotificacaoService(List<INotificador> notificadores)
        {
            _notificadores = notificadores ?? new List<INotificador>();
        }

        // Responsabilidade única: Notificar criação de rastreamento
        public void NotificarCriacaoRastreamento(Pedido pedido)
        {
            var mensagem = $"Seu pedido {pedido.Id} foi criado. Código de rastreamento: {pedido.CodigoRastreamento}";

            // LSP: Qualquer implementação de INotificador pode ser usada
            foreach (var notificador in _notificadores)
            {
                notificador.Enviar(pedido.ClienteId, mensagem);
            }
        }

        // Responsabilidade única: Notificar atualização de status
        public void NotificarAtualizacaoStatus(
            Pedido pedido,
            StatusEntrega statusAnterior,
            StatusEntrega novoStatus)
        {
            var mensagem = $"Status do pedido {pedido.Id} alterado de {statusAnterior} para {novoStatus}";

            // LSP: Qualquer implementação de INotificador pode ser usada
            foreach (var notificador in _notificadores)
            {
                notificador.Enviar(pedido.ClienteId, mensagem);
            }
        }
    }
}
```

---

## 🎨 Strategies - Estratégias de Cálculo (OCP)

### OCP: Aberto para extensão, fechado para modificação

```csharp
// ============================================
// STRATEGIES - Estratégias de Cálculo de Frete
// OCP: Aberto para extensão (novas estratégias), fechado para modificação
// ============================================

namespace MicrosservicoLogistica.Strategies
{
    // ============================================
    // OCP: Interface que permite adicionar novas estratégias sem modificar código existente
    // ============================================
    public interface IEstrategiaFrete
    {
        decimal Calcular(Pedido pedido);
        string TipoFrete { get; }
    }

    // ============================================
    // OCP: Estratégia de frete padrão
    // Pode adicionar novas estratégias sem modificar CalculadoraFrete
    // ============================================
    public class FretePadrao : IEstrategiaFrete
    {
        public string TipoFrete => "Padrão";

        public decimal Calcular(Pedido pedido)
        {
            // Cálculo base: R$ 10,00 + R$ 2,00 por item
            decimal valorBase = 10.00m;
            decimal valorPorItem = 2.00m;
            decimal totalItens = pedido.Itens.Sum(item => item.Quantidade);

            return valorBase + (valorPorItem * totalItens);
        }
    }

    // ============================================
    // OCP: Estratégia de frete expresso
    // Nova estratégia adicionada sem modificar código existente
    // ============================================
    public class FreteExpresso : IEstrategiaFrete
    {
        public string TipoFrete => "Expresso";

        public decimal Calcular(Pedido pedido)
        {
            // Cálculo expresso: R$ 25,00 + R$ 5,00 por item (mais caro, mais rápido)
            decimal valorBase = 25.00m;
            decimal valorPorItem = 5.00m;
            decimal totalItens = pedido.Itens.Sum(item => item.Quantidade);

            return valorBase + (valorPorItem * totalItens);
        }
    }

    // ============================================
    // OCP: Estratégia de frete econômico
    // Nova estratégia adicionada sem modificar código existente
    // ============================================
    public class FreteEconomico : IEstrategiaFrete
    {
        public string TipoFrete => "Econômico";

        public decimal Calcular(Pedido pedido)
        {
            // Cálculo econômico: R$ 5,00 + R$ 1,00 por item (mais barato, mais lento)
            decimal valorBase = 5.00m;
            decimal valorPorItem = 1.00m;
            decimal totalItens = pedido.Itens.Sum(item => item.Quantidade);

            return valorBase + (valorPorItem * totalItens);
        }
    }

    // ============================================
    // OCP: Exemplo de como adicionar nova estratégia sem modificar código existente
    // ============================================
    public class FreteGratis : IEstrategiaFrete
    {
        public string TipoFrete => "Grátis";

        public decimal Calcular(Pedido pedido)
        {
            // Frete grátis para pedidos acima de R$ 200,00
            decimal valorTotal = pedido.Itens.Sum(item => item.PrecoUnitario * item.Quantidade);
            
            if (valorTotal >= 200.00m)
                return 0.00m;

            // Se não atingir o valor, usa cálculo padrão
            return new FretePadrao().Calcular(pedido);
        }
    }
}
```

---

## 💾 Repositories - Abstrações de Persistência (DIP, ISP)

### DIP: Dependências invertidas | ISP: Interfaces segregadas

```csharp
// ============================================
// REPOSITORIES - Abstrações de Persistência
// DIP: Dependências invertidas (serviços dependem de abstrações)
// ISP: Interfaces segregadas (separar leitura e escrita)
// ============================================

namespace MicrosservicoLogistica.Repositories
{
    // ============================================
    // DIP: Interface de repositório de pedidos
    // Serviços dependem desta abstração, não de implementação concreta
    // ============================================
    public interface IRepositorioPedido
    {
        Pedido BuscarPorId(string id);
        Pedido BuscarPorCodigoRastreamento(string codigoRastreamento);
        void Salvar(Pedido pedido);
        void Atualizar(Pedido pedido);
        List<Pedido> ListarPorCliente(string clienteId);
    }

    // ============================================
    // ISP: Interface segregada para LEITURA de estoque
    // Clientes que só precisam ler não são forçados a implementar escrita
    // ============================================
    public interface ILeitorEstoque
    {
        int ObterQuantidade(string produtoId);
        bool VerificarDisponibilidade(string produtoId, int quantidade);
        Dictionary<string, int> ObterEstoqueCompleto();
    }

    // ============================================
    // ISP: Interface segregada para ESCRITA de estoque
    // Clientes que só precisam escrever não são forçados a implementar leitura
    // ============================================
    public interface IEscritorEstoque
    {
        void Reservar(string produtoId, int quantidade);
        void LiberarReserva(string produtoId, int quantidade);
        void AtualizarQuantidade(string produtoId, int quantidade);
    }

    // ============================================
    // Implementação concreta do repositório (poderia ser SQL Server, MongoDB, etc.)
    // DIP: Esta implementação depende da interface, não o contrário
    // ============================================
    public class RepositorioPedidoBD : IRepositorioPedido
    {
        // Simulação de banco de dados em memória para exemplo
        private readonly Dictionary<string, Pedido> _pedidos = new Dictionary<string, Pedido>();

        public Pedido BuscarPorId(string id)
        {
            _pedidos.TryGetValue(id, out var pedido);
            return pedido;
        }

        public Pedido BuscarPorCodigoRastreamento(string codigoRastreamento)
        {
            return _pedidos.Values.FirstOrDefault(p => p.CodigoRastreamento == codigoRastreamento);
        }

        public void Salvar(Pedido pedido)
        {
            _pedidos[pedido.Id] = pedido;
        }

        public void Atualizar(Pedido pedido)
        {
            if (_pedidos.ContainsKey(pedido.Id))
            {
                _pedidos[pedido.Id] = pedido;
            }
        }

        public List<Pedido> ListarPorCliente(string clienteId)
        {
            return _pedidos.Values.Where(p => p.ClienteId == clienteId).ToList();
        }
    }

    // ============================================
    // Implementação concreta de leitor de estoque
    // ISP: Implementa apenas ILeitorEstoque
    // ============================================
    public class LeitorEstoqueBD : ILeitorEstoque
    {
        // Simulação de banco de dados em memória
        private readonly Dictionary<string, int> _estoque = new Dictionary<string, int>
        {
            { "PROD001", 100 },
            { "PROD002", 50 },
            { "PROD003", 200 }
        };

        public int ObterQuantidade(string produtoId)
        {
            _estoque.TryGetValue(produtoId, out var quantidade);
            return quantidade;
        }

        public bool VerificarDisponibilidade(string produtoId, int quantidade)
        {
            return ObterQuantidade(produtoId) >= quantidade;
        }

        public Dictionary<string, int> ObterEstoqueCompleto()
        {
            return new Dictionary<string, int>(_estoque);
        }
    }

    // ============================================
    // Implementação concreta de escritor de estoque
    // ISP: Implementa apenas IEscritorEstoque
    // ============================================
    public class EscritorEstoqueBD : IEscritorEstoque
    {
        private readonly Dictionary<string, int> _estoque = new Dictionary<string, int>
        {
            { "PROD001", 100 },
            { "PROD002", 50 },
            { "PROD003", 200 }
        };

        private readonly Dictionary<string, int> _reservas = new Dictionary<string, int>();

        public void Reservar(string produtoId, int quantidade)
        {
            var estoqueAtual = _estoque.ContainsKey(produtoId) ? _estoque[produtoId] : 0;
            
            if (estoqueAtual < quantidade)
                throw new Exception($"Estoque insuficiente para produto {produtoId}");

            _estoque[produtoId] = estoqueAtual - quantidade;
            
            if (_reservas.ContainsKey(produtoId))
                _reservas[produtoId] += quantidade;
            else
                _reservas[produtoId] = quantidade;
        }

        public void LiberarReserva(string produtoId, int quantidade)
        {
            if (_reservas.ContainsKey(produtoId))
            {
                _reservas[produtoId] = Math.Max(0, _reservas[produtoId] - quantidade);
                _estoque[produtoId] = (_estoque.ContainsKey(produtoId) ? _estoque[produtoId] : 0) + quantidade;
            }
        }

        public void AtualizarQuantidade(string produtoId, int quantidade)
        {
            _estoque[produtoId] = quantidade;
        }
    }
}
```

---

## 📢 Notifications - Sistema de Notificações (OCP, LSP, ISP)

### OCP: Extensível | LSP: Substituição | ISP: Interface simples

```csharp
// ============================================
// NOTIFICATIONS - Sistema de Notificações
// OCP: Pode adicionar novos canais sem modificar código existente
// LSP: Todas as implementações são substituíveis
// ISP: Interface simples, sem métodos desnecessários
// ============================================

namespace MicrosservicoLogistica.Notifications
{
    // ============================================
    // ISP: Interface segregada e simples
    // Apenas o método necessário, sem configurações complexas
    // ============================================
    public interface INotificador
    {
        void Enviar(string destinatario, string mensagem);
        string Canal { get; }
    }

    // ============================================
    // LSP: NotificadorEmail pode ser substituído por qualquer INotificador
    // OCP: Nova implementação adicionada sem modificar código existente
    // ============================================
    public class NotificadorEmail : INotificador
    {
        public string Canal => "Email";

        public void Enviar(string destinatario, string mensagem)
        {
            // Simulação de envio de email
            Console.WriteLine($"[EMAIL] Para: {destinatario}");
            Console.WriteLine($"[EMAIL] Mensagem: {mensagem}");
            Console.WriteLine($"[EMAIL] Enviado com sucesso!");
        }
    }

    // ============================================
    // LSP: NotificadorSMS pode ser substituído por qualquer INotificador
    // OCP: Nova implementação adicionada sem modificar código existente
    // ============================================
    public class NotificadorSMS : INotificador
    {
        public string Canal => "SMS";

        public void Enviar(string destinatario, string mensagem)
        {
            // Simulação de envio de SMS
            Console.WriteLine($"[SMS] Para: {destinatario}");
            Console.WriteLine($"[SMS] Mensagem: {mensagem}");
            Console.WriteLine($"[SMS] Enviado com sucesso!");
        }
    }

    // ============================================
    // LSP: NotificadorPush pode ser substituído por qualquer INotificador
    // OCP: Nova implementação adicionada sem modificar código existente
    // ============================================
    public class NotificadorPush : INotificador
    {
        public string Canal => "Push";

        public void Enviar(string destinatario, string mensagem)
        {
            // Simulação de notificação push
            Console.WriteLine($"[PUSH] Para: {destinatario}");
            Console.WriteLine($"[PUSH] Mensagem: {mensagem}");
            Console.WriteLine($"[PUSH] Enviado com sucesso!");
        }
    }

    // ============================================
    // OCP: Exemplo de como adicionar novo canal sem modificar código existente
    // LSP: NotificadorWhatsApp pode substituir qualquer INotificador
    // ============================================
    public class NotificadorWhatsApp : INotificador
    {
        public string Canal => "WhatsApp";

        public void Enviar(string destinatario, string mensagem)
        {
            // Simulação de envio via WhatsApp
            Console.WriteLine($"[WHATSAPP] Para: {destinatario}");
            Console.WriteLine($"[WHATSAPP] Mensagem: {mensagem}");
            Console.WriteLine($"[WHATSAPP] Enviado com sucesso!");
        }
    }
}
```

---

## 🚀 Program.cs - Composição e Ponto de Entrada

```csharp
// ============================================
// PROGRAM - Composição e Ponto de Entrada
// DIP: Inversão de dependências através de injeção manual
// ============================================

using MicrosservicoLogistica.Domain;
using MicrosservicoLogistica.Services;
using MicrosservicoLogistica.Strategies;
using MicrosservicoLogistica.Repositories;
using MicrosservicoLogistica.Notifications;

namespace MicrosservicoLogistica
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Microsserviço de Logística - Exemplo SOLID ===\n");

            // ============================================
            // DIP: Composição de dependências
            // Alto nível controla quais implementações usar
            // ============================================

            // Repositórios (DIP)
            IRepositorioPedido repositorioPedido = new RepositorioPedidoBD();
            ILeitorEstoque leitorEstoque = new LeitorEstoqueBD();
            IEscritorEstoque escritorEstoque = new EscritorEstoqueBD();

            // Notificadores (OCP, LSP, ISP)
            var notificadores = new List<INotificador>
            {
                new NotificadorEmail(),
                new NotificadorSMS(),
                new NotificadorPush()
            };

            // Serviços (SRP, DIP)
            var notificacaoService = new NotificacaoService(notificadores);
            var rastreamentoService = new RastreamentoService(repositorioPedido, notificacaoService);
            var estoqueService = new EstoqueService(leitorEstoque, escritorEstoque);

            // Estratégia de frete (OCP)
            IEstrategiaFrete estrategiaFrete = new FretePadrao();
            var calculadoraFrete = new CalculadoraFrete(estrategiaFrete);

            // ============================================
            // Exemplo de uso: Criar pedido e rastreamento
            // ============================================

            // Criar pedido de exemplo
            var pedido = new Pedido
            {
                Id = "PED001",
                ClienteId = "CLI001",
                EnderecoEntrega = new Endereco
                {
                    Cep = "01310-100",
                    Rua = "Avenida Paulista",
                    Numero = "1000",
                    Cidade = "São Paulo",
                    Estado = "SP",
                    Pais = "Brasil"
                },
                Itens = new List<ItemPedido>
                {
                    new ItemPedido { ProdutoId = "PROD001", Nome = "Notebook", Quantidade = 1, PrecoUnitario = 2500.00m },
                    new ItemPedido { ProdutoId = "PROD002", Nome = "Mouse", Quantidade = 2, PrecoUnitario = 50.00m }
                }
            };

            // Verificar estoque (SRP)
            Console.WriteLine("1. Verificando estoque...");
            foreach (var item in pedido.Itens)
            {
                var disponivel = estoqueService.VerificarDisponibilidade(item.ProdutoId, item.Quantidade);
                Console.WriteLine($"   Produto {item.Nome}: {(disponivel ? "Disponível" : "Indisponível")}");
            }

            // Reservar produtos (SRP)
            Console.WriteLine("\n2. Reservando produtos...");
            estoqueService.ReservarProdutos(pedido.Itens);
            Console.WriteLine("   Produtos reservados com sucesso!");

            // Calcular frete (OCP)
            Console.WriteLine("\n3. Calculando frete...");
            pedido.ValorFrete = calculadoraFrete.CalcularFrete(pedido);
            Console.WriteLine($"   Frete calculado: R$ {pedido.ValorFrete:F2}");

            // Salvar pedido
            repositorioPedido.Salvar(pedido);

            // Criar rastreamento (SRP)
            Console.WriteLine("\n4. Criando rastreamento...");
            var codigoRastreamento = rastreamentoService.CriarRastreamento(pedido.Id);
            Console.WriteLine($"   Código de rastreamento: {codigoRastreamento}");

            // Consultar rastreamento (SRP)
            Console.WriteLine("\n5. Consultando rastreamento...");
            var pedidoRastreado = rastreamentoService.ConsultarRastreamento(codigoRastreamento);
            Console.WriteLine($"   Status: {pedidoRastreado.Status}");
            Console.WriteLine($"   Cliente: {pedidoRastreado.ClienteId}");

            // Atualizar status (SRP)
            Console.WriteLine("\n6. Atualizando status para 'Em Trânsito'...");
            rastreamentoService.AtualizarStatus(codigoRastreamento, StatusEntrega.EmTransito);

            Console.WriteLine("\n7. Atualizando status para 'Entregue'...");
            rastreamentoService.AtualizarStatus(codigoRastreamento, StatusEntrega.Entregue);

            // ============================================
            // Exemplo de extensibilidade (OCP)
            // Trocar estratégia de frete sem modificar código
            // ============================================

            Console.WriteLine("\n=== Exemplo de Extensibilidade (OCP) ===");
            
            // Usar frete expresso
            IEstrategiaFrete freteExpresso = new FreteExpresso();
            var calculadoraExpresso = new CalculadoraFrete(freteExpresso);
            var freteExpressoValor = calculadoraExpresso.CalcularFrete(pedido);
            Console.WriteLine($"Frete Expresso: R$ {freteExpressoValor:F2}");

            // Usar frete econômico
            IEstrategiaFrete freteEconomico = new FreteEconomico();
            var calculadoraEconomico = new CalculadoraFrete(freteEconomico);
            var freteEconomicoValor = calculadoraEconomico.CalcularFrete(pedido);
            Console.WriteLine($"Frete Econômico: R$ {freteEconomicoValor:F2}");

            // Usar frete grátis
            IEstrategiaFrete freteGratis = new FreteGratis();
            var calculadoraGratis = new CalculadoraFrete(freteGratis);
            var freteGratisValor = calculadoraGratis.CalcularFrete(pedido);
            Console.WriteLine($"Frete Grátis: R$ {freteGratisValor:F2}");

            Console.WriteLine("\n=== Fim do Exemplo ===");
        }
    }
}
```

---

## 📝 Resumo dos Princípios SOLID Aplicados

### ✅ SRP (Single Responsibility Principle)
- `RastreamentoService`: Apenas rastreamento
- `CalculadoraFrete`: Apenas cálculo de frete
- `EstoqueService`: Apenas gestão de estoque
- `NotificacaoService`: Apenas orquestração de notificações

### ✅ OCP (Open/Closed Principle)
- `IEstrategiaFrete`: Permite adicionar novos tipos de frete sem modificar `CalculadoraFrete`
- `INotificador`: Permite adicionar novos canais sem modificar `NotificacaoService`

### ✅ LSP (Liskov Substitution Principle)
- Todas as implementações de `INotificador` são substituíveis
- Todas as implementações de `IEstrategiaFrete` são substituíveis

### ✅ ISP (Interface Segregation Principle)
- `ILeitorEstoque` e `IEscritorEstoque` separados
- `INotificador` simples, sem métodos desnecessários

### ✅ DIP (Dependency Inversion Principle)
- Serviços dependem de interfaces, não de implementações concretas
- Alto nível controla quais implementações usar

---

## 🎯 Benefícios da Aplicação SOLID

1. **Testabilidade**: Cada componente pode ser testado isoladamente
2. **Manutenibilidade**: Mudanças são isoladas e não afetam outros componentes
3. **Extensibilidade**: Novas funcionalidades podem ser adicionadas sem modificar código existente
4. **Reutilização**: Componentes podem ser reutilizados em diferentes contextos
5. **Flexibilidade**: Fácil trocar implementações (ex: trocar banco de dados, adicionar novo canal de notificação)

---

[← Voltar para recursos](../README.md) | [Ver documentação explicativa](./microsservico-logistica-documentacao.md)
