using Juegoteca.Biblioteca.Interfaces;
using Juegoteca.Biblioteca.Modelos;

namespace Juegoteca.Biblioteca.Servicios;

public class ExportadorTxt : IExportador
{
    public void ExportarATxt(Usuario usuario,string rutaArchivo)
    {
        ArgumentNullException.ThrowIfNull(usuario);

        if (string.IsNullOrWhiteSpace(rutaArchivo))
        {
            throw new ArgumentException("La ruta del archivo no puede estar vacía.",nameof(rutaArchivo));
        }

        var texto = new StringBuilder();
        texto.AppendLine($"Jugador: {usuario.Nombre}");
        texto.AppendLine($"Fecha: {DateTime.Now:yyyy-MM-dd HH:mm}");
        texto.AppendLine();

        foreach (var (nombre, juegos) in usuario.Colecciones)
        {
            texto.AppendLine($"Colección: {nombre}");

            foreach (var juego in juegos)
            {
                texto.AppendLine($"  - {juego}");
            }

            texto.AppendLine($"  Total de horas: {juegos.Sum(j => j.HorasJuego)} h");
            texto.AppendLine();
        }

        string? carpeta = Path.GetDirectoryName(rutaArchivo);
        if (!string.IsNullOrEmpty(carpeta))
        {
            Directory.CreateDirectory(carpeta);
        }
        File.WriteAllText(rutaArchivo,texto.ToString());
    }
}
