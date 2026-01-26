namespace Ecommerce.Domain.Interfaces;

using Ecommerce.Domain.Entities;

/// <summary>
/// Repository Pattern - Abstração de persistência
/// </summary>
public interface IClienteRepository
{
    Task<Cliente?> BuscarPorIdAsync(Guid id);
    Task<Cliente?> BuscarPorEmailAsync(string email);
    Task<List<Cliente>> BuscarTodosAsync();
    Task SalvarAsync(Cliente cliente);
}

public interface IPedidoRepository
{
    Task<Pedido?> BuscarPorIdAsync(Guid id);
    Task<List<Pedido>> BuscarPorClienteAsync(Guid clienteId);
    Task SalvarAsync(Pedido pedido);
}

/// <summary>
/// Unit of Work Pattern - Consistência transacional
/// </summary>
public interface IUnitOfWork
{
    IClienteRepository Clientes { get; }
    IPedidoRepository Pedidos { get; }
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}