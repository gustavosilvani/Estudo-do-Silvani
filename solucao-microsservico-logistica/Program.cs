// ============================================
// PROGRAM - Composição e Ponto de Entrada
// DIP: Inversão de dependências através de injeção manual
// ============================================

using MicrosservicoLogistica.Domain;
using MicrosservicoLogistica.Services;
using MicrosservicoLogistica.Strategies;
using MicrosservicoLogistica.Repositories;
using MicrosservicoLogistica.Notifications;

namespace MicrosservicoLogistica;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== Microsserviço de Logística - Demonstração SOLID ===\n");

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
        // Demonstração: Criar pedido e rastreamento
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
                new ItemPedido
                {
                    ProdutoId = "PROD001",
                    Nome = "Notebook",
                    Quantidade = 1,
                    PrecoUnitario = 2500.00m
                },
                new ItemPedido
                {
                    ProdutoId = "PROD002",
                    Nome = "Mouse",
                    Quantidade = 2,
                    PrecoUnitario = 50.00m
                }
            }
        };

        Console.WriteLine("=== Pedido Criado ===");
        Console.WriteLine($"ID: {pedido.Id}");
        Console.WriteLine($"Cliente: {pedido.ClienteId}");
        Console.WriteLine($"Itens: {pedido.Itens.Count}");
        Console.WriteLine();

        // Verificar estoque (SRP)
        Console.WriteLine("1. Verificando estoque...");
        foreach (var item in pedido.Itens)
        {
            var disponivel = estoqueService.VerificarDisponibilidade(item.ProdutoId, item.Quantidade);
            Console.WriteLine($"   Produto {item.Nome}: {(disponivel ? "✅ Disponível" : "❌ Indisponível")}");
        }
        Console.WriteLine();

        // Reservar produtos (SRP)
        Console.WriteLine("2. Reservando produtos...");
        estoqueService.ReservarProdutos(pedido.Itens);
        Console.WriteLine("   ✅ Produtos reservados com sucesso!");
        Console.WriteLine();

        // Calcular frete (OCP)
        Console.WriteLine("3. Calculando frete...");
        pedido.ValorFrete = calculadoraFrete.CalcularFrete(pedido);
        Console.WriteLine($"   ✅ Frete calculado: R$ {pedido.ValorFrete:F2}");
        Console.WriteLine();

        // Salvar pedido
        repositorioPedido.Salvar(pedido);
        Console.WriteLine("4. Pedido salvo no repositório");
        Console.WriteLine();

        // Criar rastreamento (SRP)
        Console.WriteLine("5. Criando rastreamento...");
        var codigoRastreamento = rastreamentoService.CriarRastreamento(pedido.Id);
        Console.WriteLine($"   ✅ Código de rastreamento: {codigoRastreamento}");
        Console.WriteLine();

        // Consultar rastreamento (SRP)
        Console.WriteLine("6. Consultando rastreamento...");
        var pedidoRastreado = rastreamentoService.ConsultarRastreamento(codigoRastreamento);
        Console.WriteLine($"   Status: {pedidoRastreado.Status}");
        Console.WriteLine($"   Cliente: {pedidoRastreado.ClienteId}");
        Console.WriteLine();

        // Atualizar status (SRP)
        Console.WriteLine("7. Atualizando status para 'Em Trânsito'...");
        rastreamentoService.AtualizarStatus(codigoRastreamento, StatusEntrega.EmTransito);
        Console.WriteLine("   ✅ Status atualizado");
        Console.WriteLine();

        Console.WriteLine("8. Atualizando status para 'Entregue'...");
        rastreamentoService.AtualizarStatus(codigoRastreamento, StatusEntrega.Entregue);
        Console.WriteLine("   ✅ Pedido entregue!");
        Console.WriteLine();

        // ============================================
        // Demonstração de Extensibilidade (OCP)
        // Trocar estratégia de frete sem modificar código
        // ============================================

        Console.WriteLine("=== Demonstração de Extensibilidade (OCP) ===");

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
        Console.WriteLine();

        // ============================================
        // Demonstração de Novos Canais de Notificação (OCP)
        // ============================================

        Console.WriteLine("=== Adicionando Novo Canal de Notificação (OCP) ===");
        notificadores.Add(new NotificadorWhatsApp());

        Console.WriteLine("Novo canal WhatsApp adicionado sem modificar NotificacaoService!");
        Console.WriteLine("Agora o sistema suporta: Email, SMS, Push e WhatsApp");
        Console.WriteLine();

        Console.WriteLine("=== Fim da Demonstração ===");
        Console.WriteLine();
        Console.WriteLine("🎯 Esta demonstração mostra como os princípios SOLID permitem:");
        Console.WriteLine("   - Código extensível (OCP)");
        Console.WriteLine("   - Responsabilidades claras (SRP)");
        Console.WriteLine("   - Componentes substituíveis (LSP)");
        Console.WriteLine("   - Interfaces segregadas (ISP)");
        Console.WriteLine("   - Dependências invertidas (DIP)");
        Console.WriteLine();
        Console.WriteLine("Pressione qualquer tecla para sair...");
        Console.ReadKey();
    }
}