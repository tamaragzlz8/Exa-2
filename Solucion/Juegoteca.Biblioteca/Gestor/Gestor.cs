using Juegoteca.Biblioteca.Interfaces;
using Juegoteca.Biblioteca.Modelos;
//Correcto
namespace Juegoteca.Biblioteca.Gestores;

public class GestorVideojuegos : IGestorVideojuegos
{
    public List<Videojuego> JuegosDisponibles { get; }

    public GestorVideojuegos()
    {
        JuegosDisponibles = new List<Videojuego>();
    }

    public void AgregarJuego(Videojuego juego)
    {
        ArgumentNullException.ThrowIfNull(juego);
        if (!JuegosDisponibles.Any(j => j.Titulo.Equals(juego.Titulo,StringComparison.OrdinalIgnoreCase)))
        {
            JuegosDisponibles.Add(juego);
        }
    }

    public bool EliminarJuego(Videojuego juego)
    {
        return JuegosDisponibles.Remove(juego);
    }

    public bool EliminarPorIndice(int indice)
    {
        if (indice >= 0 && indice < JuegosDisponibles.Count)
        {
            JuegosDisponibles.RemoveAt(indice);
            return true;
        }
        return false;
    }

    public List<Videojuego> BuscarPorTitulo(string texto)
    {
        return JuegosDisponibles
            .Where(j => j.Titulo.Contains(texto,StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    public void LimpiarCatalogo()
    {
        JuegosDisponibles.Clear();
    }
    public void QuickSort(List<Videojuego> lista)
    {
        var ordenamiento = new Juegoteca.Biblioteca.Gestores.QuickSort();
        ordenamiento.OrdenarQuickSort(lista);
    }

    public List<Videojuego> MergeSort(List<Videojuego> lista)
    {
        var ordenamiento = new Juegoteca.Biblioteca.Gestores.MergeSort();
        return ordenamiento.OrdenarMergeSort(lista); // Ajusta al nombre exacto del método en tu clase MergeSort
    }

    public int BusquedaBinaria(string titulo)
    {
        return Juegoteca.Biblioteca.Gestores.BusquedaBinaria.BuscarPorTitulo(JuegosDisponibles, titulo);
    }

}