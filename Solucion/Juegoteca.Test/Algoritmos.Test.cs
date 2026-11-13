using Juegoteca.Biblioteca.Gestores;
using Juegoteca.Biblioteca.Modelos;
using Juegoteca.Tests.Datos;

namespace Juegoteca.Tests;

public class AlgoritmosTests
{
    private readonly GestorVideojuegos _gestor;

    public AlgoritmosTests()
    {
        _gestor = DatosPrueba.GestorPoblado();
    }

    private static bool EstaOrdenadaPorHoras(List<Videojuego> lista)
    {
        for (int i = 1; i < lista.Count; i++)
        {
            if (lista[i - 1].HorasJuego > lista[i].HorasJuego)
            {
                return false;
            }
        }

        return true;
    }

    private static List<Videojuego> ListaConHorasRepetidas() => new List<Videojuego>
    {
        new Videojuego("A", "Estudio", 10),
        new Videojuego("B", "Estudio", 5),
        new Videojuego("C", "Estudio", 10),
        new Videojuego("D", "Estudio", 5)
    };

    // 1. QuickSort
    [Fact]
    public void QuickSort_CatalogoPoblado_OrdenaLaMismaListaAscendente()
    {
        // Arrange
        var lista = _gestor.JuegosDisponibles;

        // Act
        _gestor.QuickSort(lista);

        // Assert
        Assert.Same(lista,_gestor.JuegosDisponibles);
        Assert.True(EstaOrdenadaPorHoras(lista));
        Assert.Equal("Tetris",lista[0].Titulo);
        Assert.Equal(5,lista[0].HorasJuego);
        Assert.Equal("Zelda: BOTW",lista[^1].Titulo);
        Assert.Equal(120,lista[^1].HorasJuego);
    }

    [Fact]
    public void QuickSort_ListaNull_LanzaArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _gestor.QuickSort(null!));
    }

    // 2. MergeSort
    [Fact]
    public void MergeSort_CatalogoPoblado_RetornaListaNuevaOrdenadaYNoModificaLaOriginal()
    {
        // Arrange
        var original = _gestor.JuegosDisponibles;
        var titulosAntes = original.Select(j => j.Titulo).ToList();

        // Act
        var ordenada = _gestor.MergeSort(original);

        // Assert
        Assert.NotSame(original,ordenada);
        Assert.True(EstaOrdenadaPorHoras(ordenada));
        Assert.Equal("Tetris",ordenada[0].Titulo);
        Assert.Equal("Zelda: BOTW",ordenada[^1].Titulo);
        Assert.Equal(titulosAntes,original.Select(j => j.Titulo).ToList());
    }

    [Fact]
    public void MergeSort_ListaNull_LanzaArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _gestor.MergeSort(null!));
    }

    // 3. Casos límite
    [Fact]
    public void Ordenamiento_ListaVacia_NoLanzaExcepcion()
    {
        // Arrange
        var vacia = new List<Videojuego>();

        // Act
        var excepcion = Record.Exception(() =>
        {
            _gestor.QuickSort(vacia);
            var resultado = _gestor.MergeSort(vacia);
            Assert.Empty(resultado);
        });

        // Assert
        Assert.Null(excepcion);
        Assert.Empty(vacia);
    }

    [Fact]
    public void Ordenamiento_ListaDeUnElemento_QuedaIgual()
    {
        // Arrange
        var lista = new List<Videojuego> { new Videojuego("Hades","Supergiant",25) };

        // Act
        _gestor.QuickSort(lista);
        var resultado = _gestor.MergeSort(lista);

        // Assert
        Assert.Single(lista);
        Assert.Single(resultado);
        Assert.Equal("Hades",resultado[0].Titulo);
    }

    [Fact]
    public void Ordenamiento_HorasRepetidas_ResultadoSigueOrdenado()
    {
        // Arrange
        var paraQuick = ListaConHorasRepetidas();
        var paraMerge = ListaConHorasRepetidas();

        // Act
        _gestor.QuickSort(paraQuick);
        var resultadoMerge = _gestor.MergeSort(paraMerge);

        // Assert
        Assert.Equal(4,paraQuick.Count);
        Assert.True(EstaOrdenadaPorHoras(paraQuick));
        Assert.Equal(4,resultadoMerge.Count);
        Assert.True(EstaOrdenadaPorHoras(resultadoMerge));
    }

    // 4. BusquedaBinaria: encontrado
    [Theory]
    [InlineData("Hades")]
    [InlineData("PORTAL 2")]
    public void BusquedaBinaria_TituloExistente_RetornaIndiceMayorOIgualACero(string titulo)
    {
        // Act
        int indice = _gestor.BusquedaBinaria(titulo);

        // Assert
        Assert.True(indice >= 0);
    }

    // 5. BusquedaBinaria: no encontrado
    [Fact]
    public void BusquedaBinaria_TituloInexistente_RetornaMenosUno()
    {
        // Act & Assert
        Assert.Equal(-1,_gestor.BusquedaBinaria("Juego que no existe"));
    }

    [Fact]
    public void BusquedaBinaria_CatalogoVacio_RetornaMenosUno()
    {
        // Arrange
        var gestorVacio = new GestorVideojuegos();

        // Act & Assert
        Assert.Equal(-1,gestorVacio.BusquedaBinaria("Hades"));
    }

    // 6. BusquedaBinaria no altera el catálogo
    [Fact]
    public void BusquedaBinaria_DespuesDeBuscar_CatalogoConservaSuOrdenOriginal()
    {
        // Arrange
        var titulosAntes = _gestor.JuegosDisponibles.Select(j => j.Titulo).ToList();

        // Act
        _gestor.BusquedaBinaria("Hades");

        // Assert
        var titulosDespues = _gestor.JuegosDisponibles.Select(j => j.Titulo).ToList();
        Assert.Equal(titulosAntes,titulosDespues);
    }
}
