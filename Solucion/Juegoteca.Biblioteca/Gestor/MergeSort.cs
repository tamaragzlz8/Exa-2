using Juegoteca.Biblioteca.Modelos;

namespace Juegoteca.Biblioteca.Gestores;

public class MergeSort
{

    public List<Videojuego> OrdenarMergeSort(List<Videojuego> lista)
    {
        ArgumentNullException.ThrowIfNull(lista);
        if (lista.Count <= 1) return new List<Videojuego>(lista);

        int medio = lista.Count / 2;
        var izq = OrdenarMergeSort(lista.GetRange(0, medio));
        var der = OrdenarMergeSort(lista.GetRange(medio, lista.Count - medio));

        return Unir(izq, der);
    }

    private List<Videojuego> Unir(List<Videojuego> izq, List<Videojuego> der)
    {
        var resultado = new List<Videojuego>();
        int i = 0, j = 0;

        while (i < izq.Count && j < der.Count)
        {
            if (izq[i].HorasJuego <= der[j].HorasJuego)
            {
                resultado.Add(izq[i]);
                i++;
            }
            else
            {
                resultado.Add(der[j]);
                j++;
            }
        }

        while (i < izq.Count) resultado.Add(izq[i++]);
        while (j < der.Count) resultado.Add(der[j++]);

        return resultado;
    }
}
