namespace Ecommerce.Application.Services;

using Ecommerce.Application.DTOs;
using Ecommerce.Domain.Common;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.ValueObjects;

/// <summary>
/// Application Service para Pedidos
/// SOLID: Single Responsibility, Dependency Inversion
/// </summary>
public interface IPedidoService
{
    Task<Result<PedidoDto>> CriarPedidoAsync(CriarPedidoDto dto);
    Task<Result<PedidoDto>> BuscarPorIdAsync(Guid id);
    Task<Result<PedidoDto>> FinalizarPedidoAsync(Guid pedidoId);
}

public class PedidoService : IPedidoService
{
    private readonly IUnitOfWork _unitOfWork;

    public PedidoService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PedidoDto>> CriarPedidoAsync(CriarPedidoDto dto)
    {
        // Validar cliente existe
        var cliente = await _unitOfWork.Clientes.BuscarPorIdAsync(dto.ClienteId);
        if (cliente == null)
        {
            return Result.Fail<PedidoDto>("Cliente não encontrado");
        }

        // Validar itens
        if (!dto.Itens.Any())
        {
            return Result.Fail<PedidoDto>("Pedido deve ter pelo menos um item");
        }

        // Criar pedido
        var pedido = new Pedido(dto.ClienteId);

        // Adicionar itens
        foreach (var itemDto in dto.Itens)
        {
            var preco = new Dinheiro(itemDto.PrecoUnitario);
            pedido.AdicionarItem(itemDto.ProdutoId, itemDto.Quantidade, preco);
        }

        // Salvar
        await _unitOfWork.Pedidos.SalvarAsync(pedido);
        await _unitOfWork.SaveChangesAsync();

        // Retornar DTO
        return Result.Ok(MapToDto(pedido));
    }

    public async Task<Result<PedidoDto>> BuscarPorIdAsync(Guid id)
    {
        var pedido = await _unitOfWork.Pedidos.BuscarPorIdAsync(id);
        if (pedido == null)
        {
            return Result.Fail<PedidoDto>("Pedido não encontrado");
        }

        return Result.Ok(MapToDto(pedido));
    }

    public async Task<Result<PedidoDto>> FinalizarPedidoAsync(Guid pedidoId)
    {
        var pedido = await _unitOfWork.Pedidos.BuscarPorIdAsync(pedidoId);
        if (pedido == null)
        {
            return Result.Fail<PedidoDto>("Pedido não encontrado");
        }

        try
        {
            pedido.Finalizar();
            await _unitOfWork.Pedidos.SalvarAsync(pedido);
            await _unitOfWork.SaveChangesAsync();

            return Result.Ok(MapToDto(pedido));
        }
        catch (Exception ex)
        {
            return Result.Fail<PedidoDto>(ex.Message);
        }
    }

    private static PedidoDto MapToDto(Pedido pedido)
    {
        return new PedidoDto
        {
            Id = pedido.Id,
            ClienteId = pedido.ClienteId,
            Status = pedido.Status.ToString(),
            Total = pedido.Total.Valor,
            DataCriacao = pedido.DataCriacao,
            Itens = pedido.Itens.Select(i => new ItemPedidoDto
            {
                Id = i.Id,
                ProdutoId = i.ProdutoId,
                Quantidade = i.Quantidade,
                PrecoUnitario = i.PrecoUnitario.Valor,
                Subtotal = i.Subtotal.Valor
            }).ToList()
        };
    }
}