namespace Ecommerce.Infrastructure.Repositories;

using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Repository Implementation - SOLID: Dependency Inversion
/// </summary>
public class ClienteRepository : IClienteRepository
{
    private readonly EcommerceDbContext _context;

    public ClienteRepository(EcommerceDbContext context)
    {
        _context = context;
    }

    public async Task<Cliente?> BuscarPorIdAsync(Guid id)
    {
        return await _context.Clientes
            .Include(c => c.Pedidos)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Cliente?> BuscarPorEmailAsync(string email)
    {
        return await _context.Clientes
            .FirstOrDefaultAsync(c => c.Email.Valor == email);
    }

    public async Task<List<Cliente>> BuscarTodosAsync()
    {
        return await _context.Clientes.ToListAsync();
    }

    public async Task SalvarAsync(Cliente cliente)
    {
        var existe = await _context.Clientes.AnyAsync(c => c.Id == cliente.Id);
        if (existe)
        {
            _context.Clientes.Update(cliente);
        }
        else
        {
            await _context.Clientes.AddAsync(cliente);
        }
    }
}

public class PedidoRepository : IPedidoRepository
{
    private readonly EcommerceDbContext _context;

    public PedidoRepository(EcommerceDbContext context)
    {
        _context = context;
    }

    public async Task<Pedido?> BuscarPorIdAsync(Guid id)
    {
        return await _context.Pedidos
            .Include(p => p.Itens)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<List<Pedido>> BuscarPorClienteAsync(Guid clienteId)
    {
        return await _context.Pedidos
            .Include(p => p.Itens)
            .Where(p => p.ClienteId == clienteId)
            .ToListAsync();
    }

    public async Task SalvarAsync(Pedido pedido)
    {
        var existe = await _context.Pedidos.AnyAsync(p => p.Id == pedido.Id);
        if (existe)
        {
            _context.Pedidos.Update(pedido);
        }
        else
        {
            await _context.Pedidos.AddAsync(pedido);
        }
    }
}