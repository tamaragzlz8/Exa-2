
using Juegoteca.Biblioteca.Modelos;

namespace Juegoteca.Biblioteca.Gestores;

public class BusquedaBinaria
{
    public static int BuscarPorTitulo(List<Videojuego> juegosDisponibles, string titulo)
    {
        if (juegosDisponibles == null || juegosDisponibles.Count == 0)
        {
            return -1;
        }

        // Creamos la copia ordenada para aplicar la búsqueda binaria de forma segura
        var copia = juegosDisponibles.OrderBy(j => j.Titulo, StringComparer.OrdinalIgnoreCase).ToList();
        int izq = 0;
        int der = copia.Count - 1;

        while (izq <= der)
        {
            int medio = izq + (der - izq) / 2;
            int comparacion = string.Compare(copia[medio].Titulo, titulo, StringComparison.OrdinalIgnoreCase);

            if (comparacion == 0)
            {
                // Retornamos el índice correspondiente en la lista original
                return juegosDisponibles.IndexOf(copia[medio]);
            }
            if (comparacion < 0)
            {
                izq = medio + 1;
            }
            else
            {
                der = medio - 1;
            }
        }

        return -1; // No encontrado
    }
}