namespace Ecommerce.Api.Controllers;

using Ecommerce.Application.DTOs;
using Ecommerce.Application.Services;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Controller de Pedidos
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class PedidosController : ControllerBase
{
    private readonly IPedidoService _pedidoService;

    public PedidosController(IPedidoService pedidoService)
    {
        _pedidoService = pedidoService;
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarPedidoDto dto)
    {
        var resultado = await _pedidoService.CriarPedidoAsync(dto);
        
        if (resultado.IsFailure)
        {
            return BadRequest(resultado.ErrorMessage);
        }

        return CreatedAtAction(nameof(BuscarPorId), new { id = resultado.Value.Id }, resultado.Value);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> BuscarPorId(Guid id)
    {
        var resultado = await _pedidoService.BuscarPorIdAsync(id);
        
        if (resultado.IsFailure)
        {
            return NotFound(resultado.ErrorMessage);
        }

        return Ok(resultado.Value);
    }

    [HttpPost("{id}/finalizar")]
    public async Task<IActionResult> Finalizar(Guid id)
    {
        var resultado = await _pedidoService.FinalizarPedidoAsync(id);
        
        if (resultado.IsFailure)
        {
            return BadRequest(resultado.ErrorMessage);
        }

        return Ok(resultado.Value);
    }
}