using Juegoteca.Biblioteca.Modelos;

namespace Juegoteca.Biblioteca.Gestores;

public class QuickSort
{
    public void OrdenarQuickSort(List<Videojuego> lista)
    {
        ArgumentNullException.ThrowIfNull(lista);
        if (lista.Count <= 1) return;
        QuickSortRec(lista, 0, lista.Count - 1);
    }

    private void QuickSortRec(List<Videojuego> lista, int izq, int der)
    {
        if (izq < der)
        {
            int pivote = Particionar(lista, izq, der);
            QuickSortRec(lista, izq, pivote - 1);
            QuickSortRec(lista, pivote + 1, der);
        }
    }

    private int Particionar(List<Videojuego> lista, int izq, int der)
    {
        int pivoteHoras = lista[der].HorasJuego;
        int i = izq - 1;

        for (int j = izq; j < der; j++)
        {
            if (lista[j].HorasJuego <= pivoteHoras)
            {
                i++;
                var temp = lista[i];
                lista[i] = lista[j];
                lista[j] = temp;
            }
        }
        var tempPivote = lista[i + 1];
        lista[i + 1] = lista[der];
        lista[der] = tempPivote;
        return i + 1;
    }
}