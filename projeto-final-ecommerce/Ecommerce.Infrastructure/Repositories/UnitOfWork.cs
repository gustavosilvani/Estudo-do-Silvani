namespace Ecommerce.Infrastructure.Repositories;

using Ecommerce.Domain.Interfaces;
using Ecommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;

/// <summary>
/// Unit of Work Implementation - Consistência transacional
/// SOLID: Single Responsibility, Dependency Inversion
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly EcommerceDbContext _context;
    private IDbContextTransaction? _transaction;
    private IClienteRepository? _clientes;
    private IPedidoRepository? _pedidos;

    public UnitOfWork(EcommerceDbContext context)
    {
        _context = context;
    }

    public IClienteRepository Clientes
    {
        get
        {
            _clientes ??= new ClienteRepository(_context);
            return _clientes;
        }
    }

    public IPedidoRepository Pedidos
    {
        get
        {
            _pedidos ??= new PedidoRepository(_context);
            return _pedidos;
        }
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }
}