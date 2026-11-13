using Juegoteca.Biblioteca.Modelos;
using Juegoteca.Biblioteca.Servicios;
using Juegoteca.Tests.Datos;

namespace Juegoteca.Tests;

public class ExportadorTxtTests : IDisposable {
    private readonly ExportadorTxt _exportador;
    private readonly string _ruta;

    public ExportadorTxtTests() {
        _exportador = new ExportadorTxt();
        // Ruta única en la carpeta temporal del sistema para cada prueba.
        _ruta = Path.Combine(Path.GetTempPath(), $"juegoteca_{Guid.NewGuid():N}.txt");
    }

    // 6. Limpieza: xUnit llama a Dispose al terminar cada prueba.
    public void Dispose() {
        if (File.Exists(_ruta)) {
            File.Delete(_ruta);
        }
    }

    // 1. Crea el archivo
    [Fact]
    public void ExportarATxt_UsuarioConColecciones_CreaElArchivoEnLaRuta() {
        // Arrange
        var usuario = DatosPrueba.UsuarioConColecciones();

        // Act
        _exportador.ExportarATxt(usuario, _ruta);

        // Assert
        Assert.True(File.Exists(_ruta));
    }

    // 2. Contenido
    [Fact]
    public void ExportarATxt_UsuarioConColecciones_ContieneNombreColeccionJuegoYTotalDeHoras() {
        // Arrange
        var usuario = DatosPrueba.UsuarioConColecciones();

        // Act
        _exportador.ExportarATxt(usuario, _ruta);
        string texto = File.ReadAllText(_ruta);

        // Assert
        Assert.Contains("Ana", texto);
        Assert.Contains("Pendientes", texto);
        Assert.Contains("Celeste", texto);
        Assert.Contains("Total de horas: 8", texto);
    }

    // 3. Usuario sin colecciones
    [Fact]
    public void ExportarATxt_UsuarioSinColecciones_CreaArchivoSoloConEncabezadoSinExcepcion() {
        // Arrange
        var usuario = new Usuario("Luis");

        // Act
        var excepcion = Record.Exception(() => _exportador.ExportarATxt(usuario, _ruta));
        string texto = File.ReadAllText(_ruta);

        // Assert
        Assert.Null(excepcion);
        Assert.Contains("Luis", texto);
        Assert.DoesNotContain("Colección:", texto);
    }

    // 4. Sobrescritura
    [Fact]
    public void ExportarATxt_DosVecesALaMismaRuta_DejaSoloElContenidoDeLaUltima() {
        // Arrange
        var primero = DatosPrueba.UsuarioConColecciones(); // Ana
        var segundo = new Usuario("Luis");

        // Act
        _exportador.ExportarATxt(primero, _ruta);
        _exportador.ExportarATxt(segundo, _ruta);
        string texto = File.ReadAllText(_ruta);

        // Assert
        Assert.Contains("Luis", texto);
        Assert.DoesNotContain("Ana", texto);
        Assert.DoesNotContain("Pendientes", texto);
    }

    // Casos de error
    [Fact]
    public void ExportarATxt_UsuarioNull_LanzaArgumentNullException() {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _exportador.ExportarATxt(null!, _ruta));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void ExportarATxt_RutaVacia_LanzaArgumentException(string rutaInvalida) {
        // Arrange
        var usuario = DatosPrueba.UsuarioConColecciones();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _exportador.ExportarATxt(usuario, rutaInvalida));
    }

    // 6. Limpieza
    [Fact]
    public void Dispose_ArchivoTemporalCreado_LoElimina() {
        // Arrange
        _exportador.ExportarATxt(DatosPrueba.UsuarioConColecciones(), _ruta);
        Assert.True(File.Exists(_ruta));

        // Act
        Dispose();

        // Assert
        Assert.False(File.Exists(_ruta));
    }
}
