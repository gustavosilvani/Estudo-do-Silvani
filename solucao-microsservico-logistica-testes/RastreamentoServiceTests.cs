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

        var service = new RastreamentoService(mockRepositorio.Object, mockNotificacao.Object);

        // Act
        var codigo = service.CriarRastreamento("PED001");

        // Assert
        Assert.NotNull(codigo);
        Assert.True(codigo.StartsWith("TRK"));
        Assert.True(codigo.Length > 10);
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
}