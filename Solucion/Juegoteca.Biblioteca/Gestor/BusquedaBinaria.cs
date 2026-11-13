
public int BusquedaBinaria(string titulo) {
    var copia = JuegosDisponibles.OrderBy(j => j.Titulo, StringComparer.OrdinalIgnoreCase).ToList();
    int izq = 0;
    int der = copia.Count - 1;

    while (izq <= der) {
        int medio = izq + (der - izq) / 2;
        int comparacion = string.Compare(copia[medio].Titulo, titulo, StringComparison.OrdinalIgnoreCase);

        if (comparacion == 0) {
            return JuegosDisponibles.IndexOf(copia[medio]);
        }
        if (comparacion < 0) {
            izq = medio + 1;
        } else {
            der = medio - 1;
        }
    }
    return -1;
}
