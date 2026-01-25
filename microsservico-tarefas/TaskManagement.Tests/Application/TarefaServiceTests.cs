using Moq;
using TaskManagement.Application.DTOs;
using TaskManagement.Application.Services;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Domain.ValueObjects;
using Xunit;

namespace TaskManagement.Tests.Application;

/// <summary>
/// Testes do Serviço de Aplicação
/// Aplica: Mocking (Moq), Isolamento de Dependências
/// </summary>
public class TarefaServiceTests {
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<ITarefaRepository> _mockRepository;
    private readonly TarefaService _service;
    
    public TarefaServiceTests() {
        _mockRepository = new Mock<ITarefaRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockUnitOfWork.Setup(uow => uow.Tarefas).Returns(_mockRepository.Object);
        
        _service = new TarefaService(_mockUnitOfWork.Object);
    }
    
    [Fact]
    public async Task CriarAsync_DadosValidos_DeveCriarTarefaComSucesso() {
        // Arrange
        var dto = new CriarTarefaDto {
            Titulo = "Nova tarefa",
            Descricao = "Descrição da tarefa",
            Prioridade = Prioridade.Alta
        };
        var usuarioId = Guid.NewGuid();
        
        _mockUnitOfWork.Setup(u => u.CommitAsync()).ReturnsAsync(1);
        
        // Act
        var resultado = await _service.CriarAsync(dto, usuarioId);
        
        // Assert
        Assert.True(resultado.Success);
        Assert.NotNull(resultado.Value);
        Assert.Equal(dto.Titulo, resultado.Value.Titulo);
        
        _mockRepository.Verify(r => r.AdicionarAsync(It.IsAny<TarefaTask>()), Times.Once);
        _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Once);
    }
    
    [Fact]
    public async Task CriarAsync_TituloInvalido_DeveFalhar() {
        // Arrange
        var dto = new CriarTarefaDto {
            Titulo = "",  // Inválido
            Descricao = "Descrição válida",
            Prioridade = Prioridade.Media
        };
        var usuarioId = Guid.NewGuid();
        
        // Act
        var resultado = await _service.CriarAsync(dto, usuarioId);
        
        // Assert
        Assert.True(resultado.IsFailure);
        _mockRepository.Verify(r => r.AdicionarAsync(It.IsAny<TarefaTask>()), Times.Never);
    }
    
    [Fact]
    public async Task BuscarPorIdAsync_TarefaExiste_DeveRetornarTarefa() {
        // Arrange
        var usuarioId = Guid.NewGuid();
        var tarefa = CriarTarefaMock(Guid.NewGuid(), usuarioId);
        var tarefaId = tarefa.Id;  // Usar ID real da tarefa
        
        _mockRepository
            .Setup(r => r.BuscarPorIdAsync(tarefaId))
            .ReturnsAsync(tarefa);
        
        // Act
        var resultado = await _service.BuscarPorIdAsync(tarefaId, usuarioId);
        
        // Assert
        Assert.True(resultado.Success);
        Assert.Equal(tarefaId, resultado.Value.Id);
    }
    
    [Fact]
    public async Task BuscarPorIdAsync_TarefaNaoExiste_DeveFalhar() {
        // Arrange
        var tarefaId = Guid.NewGuid();
        var usuarioId = Guid.NewGuid();
        
        _mockRepository
            .Setup(r => r.BuscarPorIdAsync(tarefaId))
            .ReturnsAsync((TarefaTask?)null);
        
        // Act
        var resultado = await _service.BuscarPorIdAsync(tarefaId, usuarioId);
        
        // Assert
        Assert.True(resultado.IsFailure);
        Assert.Contains("não encontrada", resultado.Error);
    }
    
    [Fact]
    public async Task ConcluirAsync_TarefaValida_DeveConcluirComSucesso() {
        // Arrange
        var tarefaId = Guid.NewGuid();
        var usuarioId = Guid.NewGuid();
        var tarefa = CriarTarefaMock(tarefaId, usuarioId);
        
        _mockRepository
            .Setup(r => r.BuscarPorIdAsync(tarefaId))
            .ReturnsAsync(tarefa);
        
        _mockUnitOfWork.Setup(u => u.CommitAsync()).ReturnsAsync(1);
        
        // Act
        var resultado = await _service.ConcluirAsync(tarefaId, usuarioId);
        
        // Assert
        Assert.True(resultado.Success);
        Assert.Equal(StatusTarefa.Concluida, tarefa.Status);
        _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Once);
    }
    
    // Helper method (Clean Code)
    private TarefaTask CriarTarefaMock(Guid id, Guid usuarioId) {
        var tarefaResult = TarefaTask.Criar(
            "Tarefa mock",
            "Descrição mock",
            usuarioId,
            Prioridade.Media
        );
        
        return tarefaResult.Value;
    }
}
