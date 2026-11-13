using Juegoteca.Biblioteca.Interfaces;
using Juegoteca.Biblioteca.Modelos;
//Correcto
namespace Juegoteca.Biblioteca.Gestores;

public class GestorVideojuegos 
{
    public List<Videojuego> JuegosDisponibles { get; }

    public GestorVideojuegos()
    {
        JuegosDisponibles = new List<Videojuego>();
    }

    public void AgregarJuego(Videojuego juego)
    {
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
   
}