namespace Ecommerce.Api.Controllers;

using Ecommerce.Application.DTOs;
using Ecommerce.Application.Services;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Controller de Clientes
/// SOLID: Single Responsibility (apenas HTTP)
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ClientesController : ControllerBase
{
    private readonly IClienteService _clienteService;

    public ClientesController(IClienteService clienteService)
    {
        _clienteService = clienteService;
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarClienteDto dto)
    {
        var resultado = await _clienteService.CriarClienteAsync(dto);
        
        if (resultado.IsFailure)
        {
            return BadRequest(resultado.ErrorMessage);
        }

        return CreatedAtAction(nameof(BuscarPorId), new { id = resultado.Value.Id }, resultado.Value);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> BuscarPorId(Guid id)
    {
        var resultado = await _clienteService.BuscarPorIdAsync(id);
        
        if (resultado.IsFailure)
        {
            return NotFound(resultado.ErrorMessage);
        }

        return Ok(resultado.Value);
    }

    [HttpGet]
    public async Task<IActionResult> BuscarTodos()
    {
        var resultado = await _clienteService.BuscarTodosAsync();
        return Ok(resultado.Value);
    }
}