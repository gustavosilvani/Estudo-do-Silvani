namespace Ecommerce.Application.Services;

using Ecommerce.Application.DTOs;
using Ecommerce.Domain.Common;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Domain.ValueObjects;

/// <summary>
/// Application Service - Orquestra casos de uso
/// SOLID: Single Responsibility (apenas orquestração)
/// </summary>
public interface IClienteService
{
    Task<Result<ClienteDto>> CriarClienteAsync(CriarClienteDto dto);
    Task<Result<ClienteDto>> BuscarPorIdAsync(Guid id);
    Task<Result<List<ClienteDto>>> BuscarTodosAsync();
}

public class ClienteService : IClienteService
{
    private readonly IUnitOfWork _unitOfWork;

    public ClienteService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ClienteDto>> CriarClienteAsync(CriarClienteDto dto)
    {
        // Validação
        if (string.IsNullOrWhiteSpace(dto.Nome))
        {
            return Result.Fail<ClienteDto>("Nome é obrigatório");
        }

        // Verificar se email já existe
        var clienteExistente = await _unitOfWork.Clientes.BuscarPorEmailAsync(dto.Email);
        if (clienteExistente != null)
        {
            return Result.Fail<ClienteDto>("Email já cadastrado");
        }

        // Criar entidade
        var email = new Email(dto.Email);
        var cliente = new Cliente(dto.Nome, email);

        // Salvar
        await _unitOfWork.Clientes.SalvarAsync(cliente);
        await _unitOfWork.SaveChangesAsync();

        // Retornar DTO
        return Result.Ok(new ClienteDto
        {
            Id = cliente.Id,
            Nome = cliente.Nome,
            Email = cliente.Email.Valor,
            DataCriacao = cliente.DataCriacao
        });
    }

    public async Task<Result<ClienteDto>> BuscarPorIdAsync(Guid id)
    {
        var cliente = await _unitOfWork.Clientes.BuscarPorIdAsync(id);
        if (cliente == null)
        {
            return Result.Fail<ClienteDto>("Cliente não encontrado");
        }

        return Result.Ok(new ClienteDto
        {
            Id = cliente.Id,
            Nome = cliente.Nome,
            Email = cliente.Email.Valor,
            DataCriacao = cliente.DataCriacao
        });
    }

    public async Task<Result<List<ClienteDto>>> BuscarTodosAsync()
    {
        var clientes = await _unitOfWork.Clientes.BuscarTodosAsync();
        var dtos = clientes.Select(c => new ClienteDto
        {
            Id = c.Id,
            Nome = c.Nome,
            Email = c.Email.Valor,
            DataCriacao = c.DataCriacao
        }).ToList();

        return Result.Ok(dtos);
    }
}