using Juegoteca.Biblioteca.Interfaces;
using Juegoteca.Biblioteca.Modelos;

namespace Juegoteca.Tests.Datos;


/// Exportador falso: implementa IExportador sin escribir archivos.
/// Sirve para probar ServicioVideojuegos sin tocar el disco.

public class ExportadorPrueba : IExportador {
    public int VecesLlamado { get; private set; }
    public Usuario? UltimoUsuario { get; private set; }
    public string? UltimaRuta { get; private set; }

    public void ExportarATxt(Usuario usuario, string rutaArchivo) {
        VecesLlamado++;
        UltimoUsuario = usuario;
        UltimaRuta = rutaArchivo;
    }
}