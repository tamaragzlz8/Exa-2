using Juegoteca.Biblioteca.Gestores;
using Juegoteca.Biblioteca.Interfaces;
using Juegoteca.Biblioteca.Modelos;
using Juegoteca.Biblioteca.Servicios;
using Juegoteca.Tests.Datos;

namespace Juegoteca.Tests;

// OrdenarCatalogo escribe en la consola: se agrupa con las demás pruebas que usan Console.
[Collection("Consola")]
public class ServicioVideojuegosTests {
    private readonly GestorVideojuegos _gestor;
    private readonly ExportadorPrueba _exportador;
    private readonly ServicioVideojuegos _servicio;

    public ServicioVideojuegosTests() {
        _gestor = DatosPrueba.GestorPoblado();
        _exportador = new ExportadorPrueba();
        _servicio = new ServicioVideojuegos(_gestor, _exportador);
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
    public void Constructor_DependenciasValidas_AsignaGestorYExportadorYUsuariosVacia() {
        // Assert
        Assert.Same(_gestor, _servicio.Gestor);
        Assert.Same(_exportador, _servicio.Exportador);
        Assert.NotNull(_servicio.Usuarios);
        Assert.Empty(_servicio.Usuarios);
    }

    [Fact]
    public void Constructor_GestorNull_LanzaArgumentNullException() {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new ServicioVideojuegos(null!, _exportador));
    }

    [Fact]
    public void Constructor_ExportadorNull_LanzaArgumentNullException() {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new ServicioVideojuegos(_gestor, null!));
    }

    // 2. RegistrarUsuario / ContarUsuarios
    [Fact]
    public void RegistrarUsuario_NombreValido_RetornaUsuarioLoAgregaYContarPasaDe0A1() {
        // Arrange
        Assert.Equal(0, _servicio.ContarUsuarios());

        // Act
        var usuario = _servicio.RegistrarUsuario("Ana");

        // Assert
        Assert.Equal("Ana", usuario.Nombre);
        Assert.Contains(usuario, _servicio.Usuarios);
        Assert.Equal(1, _servicio.ContarUsuarios());
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void RegistrarUsuario_NombreVacio_LanzaArgumentException(string nombre) {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => _servicio.RegistrarUsuario(nombre));
    }

    // 3. BuscarUsuario
    [Fact]
    public void BuscarUsuario_NombreEnMayusculas_EncuentraAlUsuario() {
        // Arrange
        var registrado = _servicio.RegistrarUsuario("Ana");

        // Act
        var encontrado = _servicio.BuscarUsuario("ANA");

        // Assert
        Assert.Same(registrado, encontrado);
    }

    [Fact]
    public void BuscarUsuario_NombreNoRegistrado_RetornaNull() {
        // Act & Assert
        Assert.Null(_servicio.BuscarUsuario("Nadie"));
    }

    // 4. EliminarUsuario
    [Fact]
    public void EliminarUsuario_UsuarioExistente_RetornaTrueYConteoBajaA0() {
        // Arrange
        _servicio.RegistrarUsuario("Ana");

        // Act
        bool resultado = _servicio.EliminarUsuario("Ana");

        // Assert
        Assert.True(resultado);
        Assert.Equal(0, _servicio.ContarUsuarios());
    }

    [Fact]
    public void EliminarUsuario_UsuarioInexistente_RetornaFalse() {
        // Act & Assert
        Assert.False(_servicio.EliminarUsuario("Nadie"));
    }

    // 5. OrdenarCatalogo
    [Theory]
    [InlineData("quick")]
    [InlineData("merge")]
    public void OrdenarCatalogo_AlgoritmoValido_TetrisQuedaPrimeroYMuestraMensaje(string algoritmo) {
        // Act
        string salida = CapturarSalida(() => _servicio.OrdenarCatalogo(algoritmo));

        // Assert
        Assert.Equal("Tetris", _gestor.JuegosDisponibles[0].Titulo);
        Assert.Equal(8, _gestor.JuegosDisponibles.Count);
        Assert.Contains("ordenado", salida);
    }

    [Theory]
    [InlineData("burbuja")]
    [InlineData("")]
    public void OrdenarCatalogo_AlgoritmoDesconocido_LanzaArgumentException(string algoritmo) {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => _servicio.OrdenarCatalogo(algoritmo));
    }

    // 6. OrdenarColeccionYCalcularHoras
    private Usuario UsuarioConHadesYCeleste() {
        var usuario = _servicio.RegistrarUsuario("Ana");
        usuario.CrearColeccion("Pendientes");
        usuario.AgregarJuegoAColeccion("Pendientes", _gestor.BuscarPorTitulo("Hades")[0]);
        usuario.AgregarJuegoAColeccion("Pendientes", _gestor.BuscarPorTitulo("Celeste")[0]);
        return usuario;
    }

    [Fact]
    public void OrdenarColeccionYCalcularHoras_HadesYCeleste_Retorna33YCelesteQuedaPrimero() {
        // Arrange
        var usuario = UsuarioConHadesYCeleste();

        // Act
        int total = _servicio.OrdenarColeccionYCalcularHoras(usuario, "Pendientes");

        // Assert
        Assert.Equal(33, total);
        Assert.Equal("Celeste", usuario.ObtenerColeccion("Pendientes")[0].Titulo);
    }

    [Fact]
    public void OrdenarColeccionYCalcularHoras_ConMergeSort_Retorna33YCelesteQuedaPrimero() {
        // Arrange
        var usuario = UsuarioConHadesYCeleste();

        // Act
        int total = _servicio.OrdenarColeccionYCalcularHoras(usuario, "Pendientes", "merge");

        // Assert
        Assert.Equal(33, total);
        Assert.Equal("Celeste", usuario.ObtenerColeccion("Pendientes")[0].Titulo);
    }

    [Fact]
    public void OrdenarColeccionYCalcularHoras_ColeccionInexistente_LanzaKeyNotFoundException() {
        // Arrange
        var usuario = _servicio.RegistrarUsuario("Ana");

        // Act & Assert
        Assert.Throws<KeyNotFoundException>(() => _servicio.OrdenarColeccionYCalcularHoras(usuario, "NoExiste"));
    }

    [Fact]
    public void OrdenarColeccionYCalcularHoras_UsuarioNull_LanzaArgumentNullException() {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _servicio.OrdenarColeccionYCalcularHoras(null!, "Pendientes"));
    }

    // 7. ExportarColecciones (usa ExportadorPrueba, no toca el disco)
    [Fact]
    public void ExportarColecciones_ConExportadorPrueba_LlamaUnaVezConMismoUsuarioYRuta() {
        // Arrange
        var usuario = DatosPrueba.UsuarioConColecciones();
        string ruta = "salida.txt";

        // Act
        _servicio.ExportarColecciones(usuario, ruta);

        // Assert
        Assert.Equal(1, _exportador.VecesLlamado);
        Assert.Same(usuario, _exportador.UltimoUsuario);
        Assert.Equal(ruta, _exportador.UltimaRuta);
    }
}
