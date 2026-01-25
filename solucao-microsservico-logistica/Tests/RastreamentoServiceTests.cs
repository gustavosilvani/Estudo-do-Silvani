using Xunit;
using Moq;
using MicrosservicoLogistica.Domain;
using MicrosservicoLogistica.Services;
using MicrosservicoLogistica.Repositories;

namespace MicrosservicoLogistica.Tests;

public class RastreamentoServiceTests
{
    [Fact]
    public void CriarRastreamento_DeveGerarCodigoUnico()
    {
        // Arrange
        var pedido = new Pedido { Id = "PED001" };
        var mockRepositorio = new Mock<IRepositorioPedido>();
        var mockNotificacao = new Mock<INotificacaoService>();

        mockRepositorio.Setup(r => r.BuscarPorId("PED001")).Returns(pedido);
        mockRepositorio.Setup(r => r.Atualizar(It.IsAny<Pedido>())).Verifiable();

        var service = new RastreamentoService(mockRepositorio.Object, mockNotificacao.Object);

        // Act
        var codigo = service.CriarRastreamento("PED001");

        // Assert
        Assert.NotNull(codigo);
        Assert.True(codigo.StartsWith("TRK"));
        Assert.True(codigo.Length > 10); // Deve ter mais que apenas o prefixo
        mockRepositorio.Verify(r => r.Atualizar(It.Is<Pedido>(p => p.CodigoRastreamento == codigo)), Times.Once);
    }

    [Fact]
    public void CriarRastreamento_DeveDefinirStatusPreparando()
    {
        // Arrange
        var pedido = new Pedido { Id = "PED001", Status = StatusEntrega.Pendente };
        var mockRepositorio = new Mock<IRepositorioPedido>();
        var mockNotificacao = new Mock<INotificacaoService>();

        mockRepositorio.Setup(r => r.BuscarPorId("PED001")).Returns(pedido);

        var service = new RastreamentoService(mockRepositorio.Object, mockNotificacao.Object);

        // Act
        service.CriarRastreamento("PED001");

        // Assert
        Assert.Equal(StatusEntrega.Preparando, pedido.Status);
    }

    [Fact]
    public void CriarRastreamento_DeveNotificarCriacao()
    {
        // Arrange
        var pedido = new Pedido { Id = "PED001" };
        var mockRepositorio = new Mock<IRepositorioPedido>();
        var mockNotificacao = new Mock<INotificacaoService>();

        mockRepositorio.Setup(r => r.BuscarPorId("PED001")).Returns(pedido);

        var service = new RastreamentoService(mockRepositorio.Object, mockNotificacao.Object);

        // Act
        service.CriarRastreamento("PED001");

        // Assert
        mockNotificacao.Verify(n => n.NotificarCriacaoRastreamento(pedido), Times.Once);
    }

    [Fact]
    public void CriarRastreamento_PedidoNaoEncontrado_DeveLancarExcecao()
    {
        // Arrange
        var mockRepositorio = new Mock<IRepositorioPedido>();
        var mockNotificacao = new Mock<INotificacaoService>();

        mockRepositorio.Setup(r => r.BuscarPorId("PED001")).Returns((Pedido)null);

        var service = new RastreamentoService(mockRepositorio.Object, mockNotificacao.Object);

        // Act & Assert
        var exception = Assert.Throws<Exception>(() => service.CriarRastreamento("PED001"));
        Assert.Equal("Pedido não encontrado", exception.Message);
    }

    [Fact]
    public void AtualizarStatus_DeveAlterarStatusENotificar()
    {
        // Arrange
        var pedido = new Pedido
        {
            Id = "PED001",
            CodigoRastreamento = "TRKPED001001",
            Status = StatusEntrega.Preparando
        };

        var mockRepositorio = new Mock<IRepositorioPedido>();
        var mockNotificacao = new Mock<INotificacaoService>();

        mockRepositorio.Setup(r => r.BuscarPorCodigoRastreamento("TRKPED001001")).Returns(pedido);

        var service = new RastreamentoService(mockRepositorio.Object, mockNotificacao.Object);

        // Act
        service.AtualizarStatus("TRKPED001001", StatusEntrega.EmTransito);

        // Assert
        Assert.Equal(StatusEntrega.EmTransito, pedido.Status);
        mockRepositorio.Verify(r => r.Atualizar(pedido), Times.Once);
        mockNotificacao.Verify(n => n.NotificarAtualizacaoStatus(
            pedido, StatusEntrega.Preparando, StatusEntrega.EmTransito), Times.Once);
    }

    [Fact]
    public void AtualizarStatus_ParaEntregue_DeveDefinirDataEntrega()
    {
        // Arrange
        var pedido = new Pedido { Id = "PED001", CodigoRastreamento = "TRKPED001001" };
        var mockRepositorio = new Mock<IRepositorioPedido>();
        var mockNotificacao = new Mock<INotificacaoService>();

        mockRepositorio.Setup(r => r.BuscarPorCodigoRastreamento("TRKPED001001")).Returns(pedido);

        var service = new RastreamentoService(mockRepositorio.Object, mockNotificacao.Object);
        var antes = DateTime.Now;

        // Act
        service.AtualizarStatus("TRKPED001001", StatusEntrega.Entregue);

        // Assert
        Assert.Equal(StatusEntrega.Entregue, pedido.Status);
        Assert.NotNull(pedido.DataEntrega);
        Assert.True(pedido.DataEntrega >= antes);
    }

    [Fact]
    public void ConsultarRastreamento_DeveRetornarPedidoCorreto()
    {
        // Arrange
        var pedidoEsperado = new Pedido { Id = "PED001", CodigoRastreamento = "TRKPED001001" };
        var mockRepositorio = new Mock<IRepositorioPedido>();
        var mockNotificacao = new Mock<INotificacaoService>();

        mockRepositorio.Setup(r => r.BuscarPorCodigoRastreamento("TRKPED001001")).Returns(pedidoEsperado);

        var service = new RastreamentoService(mockRepositorio.Object, mockNotificacao.Object);

        // Act
        var pedido = service.ConsultarRastreamento("TRKPED001001");

        // Assert
        Assert.Equal(pedidoEsperado, pedido);
        Assert.Equal("PED001", pedido.Id);
    }
}