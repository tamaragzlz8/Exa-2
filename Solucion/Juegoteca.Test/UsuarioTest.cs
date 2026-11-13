using Juegoteca.Biblioteca.Modelos;

namespace Juegoteca.Tests;

// La colección "Consola" evita que estas pruebas corran en paralelo con otras
// que también redirigen o escriben en Console.Out.
[Collection("Consola")]
public class UsuarioTests {
    private readonly Usuario _usuario;
    private readonly Videojuego _celeste;
    private readonly Videojuego _hades;

    public UsuarioTests() {
        _usuario = new Usuario("Ana");
        _celeste = new Videojuego("Celeste", "Maddy Makes Games", 8);
        _hades = new Videojuego("Hades", "Supergiant", 25);
    }

    private static string CapturarSalida(Action accion) {
        var original = Console.Out;
        var escritor = new StringWriter();
        Console.SetOut(escritor);

        try {
            accion();
        } finally {
            Console.SetOut(original);
        }

        return escritor.ToString();
    }

    // 1. Constructor
    [Fact]
    public void Constructor_NombreValido_AsignaNombreYColeccionesVacio() {
        // Arrange & Act
        var usuario = new Usuario("Zaa");

        // Assert
        Assert.Equal("Zaa", usuario.Nombre);
        Assert.NotNull(usuario.Colecciones);
        Assert.Empty(usuario.Colecciones);
    }

    // 2. CrearColeccion
    [Fact]
    public void CrearColeccion_NombreNuevo_AgregaClaveConListaVacia() {
        // Act
        _usuario.CrearColeccion("Pendientes");

        // Assert
        Assert.True(_usuario.Colecciones.ContainsKey("Pendientes"));
        Assert.Empty(_usuario.Colecciones["Pendientes"]);
    }

    [Fact]
    public void CrearColeccion_NombreRepetido_LanzaInvalidOperationException() {
        // Arrange
        _usuario.CrearColeccion("Pendientes");

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _usuario.CrearColeccion("Pendientes"));
    }

    // 3. AgregarJuegoAColeccion
    [Fact]
    public void AgregarJuegoAColeccion_JuegoAgregadoDosVeces_QuedaUnaSolaVez() {
        // Arrange
        _usuario.CrearColeccion("Pendientes");

        // Act
        _usuario.AgregarJuegoAColeccion("Pendientes", _celeste);
        _usuario.AgregarJuegoAColeccion("Pendientes", _celeste);

        // Assert
        Assert.Single(_usuario.Colecciones["Pendientes"]);
        Assert.Contains(_celeste, _usuario.Colecciones["Pendientes"]);
    }

    [Fact]
    public void AgregarJuegoAColeccion_ColeccionInexistente_LanzaKeyNotFoundException() {
        // Act & Assert
        Assert.Throws<KeyNotFoundException>(() => _usuario.AgregarJuegoAColeccion("NoExiste", _celeste));
    }

    [Fact]
    public void AgregarJuegoAColeccion_JuegoNull_LanzaArgumentNullException() {
        // Arrange
        _usuario.CrearColeccion("Pendientes");

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _usuario.AgregarJuegoAColeccion("Pendientes", null!));
    }

    // 4. ContarJuegos
    [Fact]
    public void ContarJuegos_DosColeccionesConUnJuegoCadaUna_RetornaDos() {
        // Arrange
        _usuario.CrearColeccion("Pendientes");
        _usuario.CrearColeccion("Favoritos");
        _usuario.AgregarJuegoAColeccion("Pendientes", _celeste);
        _usuario.AgregarJuegoAColeccion("Favoritos", _hades);

        // Act
        int total = _usuario.ContarJuegos();

        // Assert
        Assert.Equal(2, total);
    }

    [Fact]
    public void ContarJuegos_SinColecciones_RetornaCero() {
        // Act & Assert
        Assert.Equal(0, _usuario.ContarJuegos());
    }

    // 5. ObtenerColeccion / EliminarColeccion
    [Fact]
    public void ObtenerColeccion_ColeccionExistente_DevuelveSusJuegos() {
        // Arrange
        _usuario.CrearColeccion("Pendientes");
        _usuario.AgregarJuegoAColeccion("Pendientes", _celeste);

        // Act
        var juegos = _usuario.ObtenerColeccion("Pendientes");

        // Assert
        Assert.Single(juegos);
        Assert.Equal("Celeste", juegos[0].Titulo);
    }

    [Fact]
    public void ObtenerColeccion_ColeccionInexistente_LanzaKeyNotFoundException() {
        // Act & Assert
        Assert.Throws<KeyNotFoundException>(() => _usuario.ObtenerColeccion("NoExiste"));
    }

    [Fact]
    public void EliminarColeccion_ColeccionExistente_RetornaTrueLaPrimeraVezYFalseLaSegunda() {
        // Arrange
        _usuario.CrearColeccion("Pendientes");

        // Act
        bool primera = _usuario.EliminarColeccion("Pendientes");
        bool segunda = _usuario.EliminarColeccion("Pendientes");

        // Assert
        Assert.True(primera);
        Assert.False(segunda);
    }

    // 6. Colecciones independientes
    [Fact]
    public void CrearColeccion_EnUnUsuario_NoLaCreaEnOtroUsuario() {
        // Arrange
        var otro = new Usuario("Luis");

        // Act
        _usuario.CrearColeccion("Pendientes");

        // Assert
        Assert.False(otro.Colecciones.ContainsKey("Pendientes"));
        Assert.NotSame(_usuario.Colecciones, otro.Colecciones);
    }

    // MostrarColecciones
    [Fact]
    public void MostrarColecciones_ConColeccionesYJuegos_ImprimeNombresYJuegos() {
        // Arrange
        _usuario.CrearColeccion("Pendientes");
        _usuario.AgregarJuegoAColeccion("Pendientes", _hades);

        // Act
        string salida = CapturarSalida(() => _usuario.MostrarColecciones());

        // Assert
        Assert.Contains("Pendientes", salida);
        Assert.Contains("Hades - Supergiant (25 h)", salida);
    }

    [Fact]
    public void MostrarColecciones_SinColecciones_NoLanzaExcepcionEIndicaQueNoHay() {
        // Act
        string salida = "";
        var excepcion = Record.Exception(() => salida = CapturarSalida(() => _usuario.MostrarColecciones()));

        // Assert
        Assert.Null(excepcion);
        Assert.Contains("No hay colecciones", salida);
    }
}
