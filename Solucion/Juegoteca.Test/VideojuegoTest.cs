using Juegoteca.Biblioteca.Interfaces;
using Juegoteca.Biblioteca.Modelos;

namespace Juegoteca.Tests;

public class VideojuegoTests {
    [Fact]
    public void Constructor_ValoresValidos_AsignaPropiedades() {
        // Arrange & Act
        var juego = new Videojuego("Hades", "Supergiant", 25);

        // Assert
        Assert.Equal("Hades", juego.Titulo);
        Assert.Equal("Supergiant", juego.Estudio);
        Assert.Equal(25, juego.HorasJuego);
    }

    [Fact]
    public void ToString_JuegoValido_DevuelveFormatoEsperado() {
        // Arrange
        var juego = new Videojuego("Hades", "Supergiant", 25);

        // Act
        string texto = juego.ToString();

        // Assert
        Assert.Equal("Hades - Supergiant (25 h)", texto);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void Constructor_HorasInvalidas_LanzaArgumentException(int horas) {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentException>(() => new Videojuego("Hades", "Supergiant", horas));
    }

    [Fact]
    public void Videojuego_UsadoComoIVideojuego_ExponeLasMismasPropiedades() {
        // Arrange
        IVideojuego juego = new Videojuego("Celeste", "Maddy Makes Games", 8);

        // Act & Assert
        Assert.Equal("Celeste", juego.Titulo);
        Assert.Equal("Maddy Makes Games", juego.Estudio);
        Assert.Equal(8, juego.HorasJuego);
    }

    [Fact]
    public void Equals_MismoTituloYEstudioConOtraCapitalizacion_RetornaTrue() {
        // Arrange
        var a = new Videojuego("Hades", "Supergiant", 25);
        var b = new Videojuego("HADES", "supergiant", 25);

        // Act & Assert
        Assert.Equal(a, b);
        Assert.Equal(a.GetHashCode(), b.GetHashCode());
    }

    [Fact]
    public void Equals_TituloDistinto_RetornaFalse() {
        // Arrange
        var a = new Videojuego("Hades", "Supergiant", 25);
        var b = new Videojuego("Celeste", "Maddy Makes Games", 8);

        // Act & Assert
        Assert.NotEqual(a, b);
    }
}
