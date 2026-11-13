using Juegoteca.Biblioteca.Interfaces;
using Juegoteca.Biblioteca.Modelos;

namespace Juegoteca.Biblioteca.Servicios;


/// Orquesta usuarios, catálogo y exportación. Recibe sus dependencias por constructor.

public class ServicioVideojuegos {
    public IGestorVideojuegos Gestor { get; }
    public IExportador Exportador { get; }
    public List<Usuario> Usuarios { get; }

    public ServicioVideojuegos(IGestorVideojuegos gestor, IExportador exportador) {
        Gestor = gestor ?? throw new ArgumentNullException(nameof(gestor));
        Exportador = exportador ?? throw new ArgumentNullException(nameof(exportador));
        Usuarios = new List<Usuario>();
    }

    public Usuario RegistrarUsuario(string nombre) {
        if (string.IsNullOrWhiteSpace(nombre)) {
            throw new ArgumentException("El nombre del usuario no puede estar vacío.", nameof(nombre));
        }

        var usuario = new Usuario(nombre.Trim());
        Usuarios.Add(usuario);
        return usuario;
    }

    /// Busca ignorando mayúsculas y minúsculas; null si no existe.</summary>
    public Usuario? BuscarUsuario(string nombre) {
        return Usuarios.FirstOrDefault(u =>
            string.Equals(u.Nombre, nombre?.Trim(), StringComparison.OrdinalIgnoreCase));
    }

    public bool EliminarUsuario(string nombre) {
        var usuario = BuscarUsuario(nombre);
        return usuario is not null && Usuarios.Remove(usuario);
    }

    public int ContarUsuarios() => Usuarios.Count;

    /// <summary>"quick" usa QuickSort, "merge" usa MergeSort; otro valor lanza ArgumentException.</summary>
    public void OrdenarCatalogo(string algoritmo) {
        switch (algoritmo?.Trim().ToLowerInvariant()) {
            case "quick":
                Gestor.QuickSort(Gestor.JuegosDisponibles);
                Console.WriteLine("Catálogo ordenado con QuickSort.");
                break;

            case "merge":
                var ordenado = Gestor.MergeSort(Gestor.JuegosDisponibles);
                Gestor.JuegosDisponibles.Clear();
                Gestor.JuegosDisponibles.AddRange(ordenado);
                Console.WriteLine("Catálogo ordenado con MergeSort.");
                break;

            default:
                throw new ArgumentException(
                    $"Algoritmo desconocido: '{algoritmo}'. Use \"quick\" o \"merge\".", nameof(algoritmo));
        }
    }

    
    /// Ordena la colección por HorasJuego (de menor a mayor) y devuelve el total de horas.
    /// El algoritmo es opcional: por defecto "quick".

    public int OrdenarColeccionYCalcularHoras(Usuario usuario, string coleccion, string algoritmo = "quick") {
        ArgumentNullException.ThrowIfNull(usuario);

        var juegos = usuario.ObtenerColeccion(coleccion);

        switch (algoritmo?.Trim().ToLowerInvariant()) {
            case "quick":
                Gestor.QuickSort(juegos);
                break;

            case "merge":
                var ordenados = Gestor.MergeSort(juegos);
                juegos.Clear();
                juegos.AddRange(ordenados);
                break;

            default:
                throw new ArgumentException(
                    $"Algoritmo desconocido: '{algoritmo}'. Use \"quick\" o \"merge\".", nameof(algoritmo));
        }

        return juegos.Sum(j => j.HorasJuego);
    }

    public void ExportarColecciones(Usuario usuario, string ruta) {
        Exportador.ExportarATxt(usuario, ruta);
    }
}
