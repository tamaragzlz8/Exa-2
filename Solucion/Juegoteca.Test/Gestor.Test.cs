using Juegoteca.Biblioteca.Gestores;
using Juegoteca.Biblioteca.Modelos;
using Juegoteca.Tests.Datos;

namespace Juegoteca.Tests;

public class GestorVideojuegosTests
{
    private readonly GestorVideojuegos _gestor;

    // xUnit crea una instancia nueva de la clase por cada prueba:
    // el constructor deja el gestor con los 8 juegos de ejemplo.
    public GestorVideojuegosTests()
    {
        _gestor = DatosPrueba.GestorPoblado();
    }

    // 1. Constructor
    [Fact]
    public void Constructor_GestorNuevo_JuegosDisponiblesVaciaYNoNull()
    {
        // Act
        var gestor = new GestorVideojuegos();

        // Assert
        Assert.NotNull(gestor.JuegosDisponibles);
        Assert.Empty(gestor.JuegosDisponibles);
    }

    [Fact]
    public void Constructor_DosGestores_NoComparteLaMismaLista()
    {
        // Arrange
        var gestorA = new GestorVideojuegos();
        var gestorB = new GestorVideojuegos();

        // Act
        gestorA.AgregarJuego(new Videojuego("Celeste","Maddy Makes Games",8));

        // Assert
        Assert.NotSame(gestorA.JuegosDisponibles,gestorB.JuegosDisponibles);
        Assert.Empty(gestorB.JuegosDisponibles);
    }

    // 2. AgregarJuego
    [Fact]
    public void AgregarJuego_JuegoNuevo_CatalogoPasaDe8A9()
    {
        // Arrange
        var nuevo = new Videojuego("Undertale","Toby Fox",6);

        // Act
        _gestor.AgregarJuego(nuevo);

        // Assert
        Assert.Equal(9,_gestor.JuegosDisponibles.Count);
    }

    [Fact]
    public void AgregarJuego_MismoJuegoOtraVez_SigueEn9()
    {
        // Arrange
        var nuevo = new Videojuego("Undertale","Toby Fox",6);
        _gestor.AgregarJuego(nuevo);

        // Act
        _gestor.AgregarJuego(nuevo);

        // Assert
        Assert.Equal(9,_gestor.JuegosDisponibles.Count);
    }

    [Fact]
    public void AgregarJuego_JuegoNull_LanzaArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _gestor.AgregarJuego(null!));
    }

    // 3. EliminarJuego / EliminarPorIndice
    [Fact]
    public void EliminarJuego_JuegoExistente_RetornaTrueYQuedan7()
    {
        // Arrange
        var juego = _gestor.JuegosDisponibles[0];

        // Act
        bool resultado = _gestor.EliminarJuego(juego);

        // Assert
        Assert.True(resultado);
        Assert.Equal(7,_gestor.JuegosDisponibles.Count);
    }

    [Fact]
    public void EliminarJuego_JuegoInexistente_RetornaFalseYSiguen8()
    {
        // Arrange
        var inexistente = new Videojuego("Undertale","Toby Fox",6);

        // Act
        bool resultado = _gestor.EliminarJuego(inexistente);

        // Assert
        Assert.False(resultado);
        Assert.Equal(8,_gestor.JuegosDisponibles.Count);
    }

    [Fact]
    public void EliminarPorIndice_IndiceValido_RetornaTrueYQuedan7()
    {
        // Act
        bool resultado = _gestor.EliminarPorIndice(0);

        // Assert
        Assert.True(resultado);
        Assert.Equal(7,_gestor.JuegosDisponibles.Count);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(8)]
    [InlineData(100)]
    public void EliminarPorIndice_IndiceFueraDeRango_RetornaFalseYSiguen8(int indice)
    {
        // Act
        bool resultado = _gestor.EliminarPorIndice(indice);

        // Assert
        Assert.False(resultado);
        Assert.Equal(8,_gestor.JuegosDisponibles.Count);
    }

    // 4. BuscarPorTitulo
    [Fact]
    public void BuscarPorTitulo_TextoEnMayusculasParcial_EncuentraHollowKnight()
    {
        // Act
        var resultado = _gestor.BuscarPorTitulo("KNIGHT");

        // Assert
        Assert.Single(resultado);
        Assert.Equal("Hollow Knight",resultado[0].Titulo);
    }

    [Fact]
    public void BuscarPorTitulo_TextoSinCoincidencias_RetornaListaVacia()
    {
        // Act
        var resultado = _gestor.BuscarPorTitulo("xyz");

        // Assert
        Assert.NotNull(resultado);
        Assert.Empty(resultado);
    }

    // 5. LimpiarCatalogo
    [Fact]
    public void LimpiarCatalogo_CatalogoPoblado_JuegosDisponiblesQuedaVacia()
    {
        // Act
        _gestor.LimpiarCatalogo();

        // Assert
        Assert.Empty(_gestor.JuegosDisponibles);
    }

    // 6. Catálogo vacío
    [Fact]
    public void BuscarYEliminar_CatalogoVacio_NoLanzanExcepcion()
    {
        // Arrange
        var gestor = new GestorVideojuegos();
        var juego = new Videojuego("Celeste","Maddy Makes Games",8);

        // Act
        var excepcion = Record.Exception(() =>
        {
            gestor.BuscarPorTitulo("celeste");
            gestor.EliminarJuego(juego);
            gestor.EliminarPorIndice(0);
        });

        // Assert
        Assert.Null(excepcion);
    }
}