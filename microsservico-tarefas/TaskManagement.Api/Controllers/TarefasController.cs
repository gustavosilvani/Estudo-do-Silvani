using Microsoft.AspNetCore.Mvc;
using TaskManagement.Api.Models;
using TaskManagement.Application.DTOs;
using TaskManagement.Application.Services;

namespace TaskManagement.Api.Controllers;

/// <summary>
/// Controller REST API com HATEOAS (Nível 3 - Richardson Maturity Model)
/// Aplica: Clean Code, SOLID, Result Pattern, HATEOAS
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class TarefasController : ControllerBase {
    private readonly ITarefaService _service;
    private readonly ILogger<TarefasController> _logger;
    
    public TarefasController(
        ITarefaService service,
        ILogger<TarefasController> logger) {
        _service = service;
        _logger = logger;
    }
    
    /// <summary>
    /// Criar nova tarefa (HATEOAS)
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(TarefaHateoasDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Criar([FromBody] CriarTarefaDto dto) {
        var usuarioId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        
        _logger.LogInformation("Criando nova tarefa para usuário {UsuarioId}", usuarioId);
        
        var resultado = await _service.CriarAsync(dto, usuarioId);
        
        if (resultado.IsFailure) {
            _logger.LogWarning("Falha ao criar tarefa: {Erro}", resultado.Error);
            return BadRequest(new { 
                error = resultado.Error,
                _links = new {
                    todas = new { href = Url.Action("ListarTodas")!, method = "GET" }
                }
            });
        }
        
        var hateoasDto = MapToHateoas(resultado.Value);
        hateoasDto.AddTarefaLinks(Url);
        
        return CreatedAtAction(
            nameof(BuscarPorId),
            new { id = resultado.Value.Id },
            hateoasDto
        );
    }
    
    /// <summary>
    /// Buscar tarefa por ID (HATEOAS)
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(TarefaHateoasDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> BuscarPorId(Guid id) {
        var usuarioId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        
        var resultado = await _service.BuscarPorIdAsync(id, usuarioId);
        
        if (resultado.IsFailure) {
            return NotFound(new { 
                error = resultado.Error,
                _links = new {
                    todas = new { href = Url.Action("ListarTodas")!, method = "GET" },
                    criar = new { href = Url.Action("Criar")!, method = "POST" }
                }
            });
        }
        
        var hateoasDto = MapToHateoas(resultado.Value);
        hateoasDto.AddTarefaLinks(Url);
        
        return Ok(hateoasDto);
    }
    
    /// <summary>
    /// Listar todas tarefas (HATEOAS com links de coleção)
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(CollectionResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> ListarTodas() {
        var usuarioId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        
        var resultado = await _service.BuscarTodosPorUsuarioAsync(usuarioId);
        
        var hateaosList = resultado.Value.Select(MapToHateoas).ToList();
        hateaosList.AddCollectionLinks(Url);
        
        var response = new {
            total = hateaosList.Count,
            items = hateaosList,
            _links = new {
                self = new { href = Url.Action("ListarTodas")!, method = "GET" },
                criar = new { href = Url.Action("Criar")!, method = "POST" }
            }
        };
        
        return Ok(response);
    }
    
    /// <summary>
    /// Concluir tarefa
    /// </summary>
    [HttpPatch("{id}/concluir")]
    [ProducesResponseType(typeof(TarefaHateoasDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Concluir(Guid id) {
        var usuarioId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        
        var resultado = await _service.ConcluirAsync(id, usuarioId);
        
        if (resultado.IsFailure) {
            return BadRequest(new { 
                error = resultado.Error,
                _links = new {
                    tarefa = new { href = Url.Action("BuscarPorId", new { id })!, method = "GET" }
                }
            });
        }
        
        // Retornar tarefa atualizada com novos links HATEOAS
        var tarefaAtualizada = await _service.BuscarPorIdAsync(id, usuarioId);
        var hateoasDto = MapToHateoas(tarefaAtualizada.Value);
        hateoasDto.AddTarefaLinks(Url);
        
        return Ok(hateoasDto);
    }
    
    /// <summary>
    /// Cancelar tarefa
    /// </summary>
    [HttpPatch("{id}/cancelar")]
    [ProducesResponseType(typeof(TarefaHateoasDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Cancelar(Guid id) {
        var usuarioId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        
        var resultado = await _service.CancelarAsync(id, usuarioId);
        
        if (resultado.IsFailure) {
            return BadRequest(new { 
                error = resultado.Error,
                _links = new {
                    tarefa = new { href = Url.Action("BuscarPorId", new { id })!, method = "GET" }
                }
            });
        }
        
        var tarefaAtualizada = await _service.BuscarPorIdAsync(id, usuarioId);
        var hateoasDto = MapToHateoas(tarefaAtualizada.Value);
        hateoasDto.AddTarefaLinks(Url);
        
        return Ok(hateoasDto);
    }
    
    /// <summary>
    /// Iniciar execução da tarefa
    /// </summary>
    [HttpPatch("{id}/iniciar")]
    [ProducesResponseType(typeof(TarefaHateoasDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> IniciarExecucao(Guid id) {
        var usuarioId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        
        var resultado = await _service.IniciarExecucaoAsync(id, usuarioId);
        
        if (resultado.IsFailure) {
            return BadRequest(new { 
                error = resultado.Error,
                _links = new {
                    tarefa = new { href = Url.Action("BuscarPorId", new { id })!, method = "GET" }
                }
            });
        }
        
        var tarefaAtualizada = await _service.BuscarPorIdAsync(id, usuarioId);
        var hateoasDto = MapToHateoas(tarefaAtualizada.Value);
        hateoasDto.AddTarefaLinks(Url);
        
        return Ok(hateoasDto);
    }
    
    /// <summary>
    /// Adicionar comentário
    /// </summary>
    [HttpPost("{id}/comentarios")]
    [ProducesResponseType(typeof(TarefaHateoasDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AdicionarComentario(
        Guid id,
        [FromBody] AdicionarComentarioDto dto) {
        
        var usuarioId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        
        var resultado = await _service.AdicionarComentarioAsync(id, usuarioId, dto);
        
        if (resultado.IsFailure) {
            return BadRequest(new { 
                error = resultado.Error,
                _links = new {
                    tarefa = new { href = Url.Action("BuscarPorId", new { id })!, method = "GET" }
                }
            });
        }
        
        var tarefaAtualizada = await _service.BuscarPorIdAsync(id, usuarioId);
        var hateoasDto = MapToHateoas(tarefaAtualizada.Value);
        hateoasDto.AddTarefaLinks(Url);
        
        return Ok(hateoasDto);
    }
    
    /// <summary>
    /// Deletar tarefa
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Deletar(Guid id) {
        var usuarioId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        
        var resultado = await _service.RemoverAsync(id, usuarioId);
        
        if (resultado.IsFailure) {
            return NotFound(new { 
                error = resultado.Error,
                _links = new {
                    todas = new { href = Url.Action("ListarTodas")!, method = "GET" }
                }
            });
        }
        
        return NoContent();
    }
    
    /// <summary>
    /// API Root - Ponto de entrada com links (HATEOAS)
    /// </summary>
    [HttpGet("/api/v1")]
    [ApiExplorerSettings(IgnoreApi = false)]
    public IActionResult ApiRoot() {
        var response = new {
            message = "Task Management API v1 - HATEOAS (Nível 3 REST)",
            versao = "1.0.0",
            maturidade = "Richardson Level 3 - HATEOAS",
            _links = new {
                self = new { href = "/api/v1", method = "GET" },
                tarefas = new { href = Url.Action("ListarTodas", "Tarefas")!, method = "GET" },
                criarTarefa = new { href = Url.Action("Criar", "Tarefas")!, method = "POST" },
                documentacao = new { href = "/", method = "GET" }
            }
        };
        
        return Ok(response);
    }
    
    // Helper: Mapper para HATEOAS DTO
    private static TarefaHateoasDto MapToHateoas(TarefaDto tarefa) {
        return new TarefaHateoasDto {
            Id = tarefa.Id,
            Titulo = tarefa.Titulo,
            Descricao = tarefa.Descricao,
            Status = tarefa.Status,
            Prioridade = tarefa.Prioridade,
            DataCriacao = tarefa.DataCriacao,
            DataConclusao = tarefa.DataConclusao,
            DataVencimento = tarefa.DataVencimento,
            EstaVencida = tarefa.EstaVencida,
            TotalComentarios = tarefa.TotalComentarios
        };
    }
}

public class CollectionResponse {
    public int Total { get; set; }
    public List<TarefaHateoasDto> Items { get; set; } = new();
    public object Links { get; set; } = new();
}
