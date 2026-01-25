using TaskManagement.Domain.Common;
using TaskManagement.Domain.ValueObjects;
using Xunit;

namespace TaskManagement.Tests.Domain;

/// <summary>
/// Testes do Value Object Titulo
/// </summary>
public class TituloTests {
    [Fact]
    public void Criar_TituloValido_DeveCriarComSucesso() {
        // Arrange
        var valor = "Título válido";
        
        // Act
        var resultado = Titulo.Criar(valor);
        
        // Assert
        Assert.True(resultado.Success);
        Assert.Equal(valor, resultado.Value.Valor);
    }
    
    [Theory]
    [InlineData("ab")] // Menos de 3 caracteres
    public void Criar_TituloMuitoCurto_DeveFalhar(string tituloCurto) {
        // Act
        var resultado = Titulo.Criar(tituloCurto);
        
        // Assert
        Assert.True(resultado.IsFailure);
        Assert.Contains("mínimo 3 caracteres", resultado.Error);
    }
    
    [Fact]
    public void Criar_TituloMuitoLongo_DeveFalhar() {
        // Arrange
        var tituloLongo = new string('a', 101);
        
        // Act
        var resultado = Titulo.Criar(tituloLongo);
        
        // Assert
        Assert.True(resultado.IsFailure);
        Assert.Contains("máximo 100 caracteres", resultado.Error);
    }
    
    [Fact]
    public void Equals_TitulosIguais_DeveRetornarTrue() {
        // Arrange
        var titulo1 = Titulo.Criar("Mesmo título").Value;
        var titulo2 = Titulo.Criar("Mesmo título").Value;
        
        // Act & Assert
        Assert.Equal(titulo1, titulo2);
    }
}
