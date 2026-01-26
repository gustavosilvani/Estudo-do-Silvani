namespace Ecommerce.Tests.Domain;

using Ecommerce.Domain.Entities;
using Ecommerce.Domain.ValueObjects;
using Xunit;

/// <summary>
/// Testes Unitários - Domain Layer
/// SOLID: Testabilidade através de DIP
/// </summary>
public class ClienteTests
{
    [Fact]
    public void CriarCliente_ComDadosValidos_DeveCriarComSucesso()
    {
        // Arrange
        var email = new Email("joao@email.com");

        // Act
        var cliente = new Cliente("João Silva", email);

        // Assert
        Assert.NotEqual(Guid.Empty, cliente.Id);
        Assert.Equal("João Silva", cliente.Nome);
        Assert.Equal("joao@email.com", cliente.Email.Valor);
    }

    [Fact]
    public void AlterarNome_ComNomeValido_DeveAlterar()
    {
        // Arrange
        var cliente = new Cliente("João", new Email("joao@email.com"));

        // Act
        cliente.AlterarNome("João Silva");

        // Assert
        Assert.Equal("João Silva", cliente.Nome);
    }

    [Fact]
    public void AlterarNome_ComNomeVazio_DeveLancarExcecao()
    {
        // Arrange
        var cliente = new Cliente("João", new Email("joao@email.com"));

        // Act & Assert
        Assert.Throws<ArgumentException>(() => cliente.AlterarNome(""));
    }
}

public class PedidoTests
{
    [Fact]
    public void CriarPedido_DeveIniciarComStatusRascunho()
    {
        // Arrange & Act
        var pedido = new Pedido(Guid.NewGuid());

        // Assert
        Assert.Equal(StatusPedido.Rascunho, pedido.Status);
        Assert.Empty(pedido.Itens);
    }

    [Fact]
    public void AdicionarItem_ComDadosValidos_DeveAdicionar()
    {
        // Arrange
        var pedido = new Pedido(Guid.NewGuid());
        var preco = new Dinheiro(100m);

        // Act
        pedido.AdicionarItem(Guid.NewGuid(), 2, preco);

        // Assert
        Assert.Single(pedido.Itens);
        Assert.Equal(200m, pedido.Total.Valor);
    }

    [Fact]
    public void FinalizarPedido_ComItens_DeveFinalizar()
    {
        // Arrange
        var pedido = new Pedido(Guid.NewGuid());
        pedido.AdicionarItem(Guid.NewGuid(), 1, new Dinheiro(100m));

        // Act
        pedido.Finalizar();

        // Assert
        Assert.Equal(StatusPedido.Finalizado, pedido.Status);
        Assert.NotNull(pedido.DataFinalizacao);
    }

    [Fact]
    public void FinalizarPedido_SemItens_DeveLancarExcecao()
    {
        // Arrange
        var pedido = new Pedido(Guid.NewGuid());

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => pedido.Finalizar());
    }
}