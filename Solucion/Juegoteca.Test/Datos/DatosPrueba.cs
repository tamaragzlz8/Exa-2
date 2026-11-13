using Juegoteca.Biblioteca.Gestores;
using Juegoteca.Biblioteca.Modelos;

namespace Juegoteca.Tests.Datos;

/// Datos para poblar las pruebas. Cada método regresa objetos NUEVOS,
/// así ninguna prueba ve los cambios de otra.

public static class DatosPrueba {
    /// <summary>Lista nueva con los 8 juegos de ejemplo.</summary>
    public static List<Videojuego> Catalogo() {
        return new List<Videojuego>
        {
            new Videojuego("Celeste", "Maddy Makes Games", 8),
            new Videojuego("Hollow Knight", "Team Cherry", 40),
            new Videojuego("Minecraft", "Mojang Studios", 100),
            new Videojuego("Tetris", "Alexey Pajitnov", 5),
            new Videojuego("Portal 2", "Valve", 10),
            new Videojuego("Stardew Valley", "ConcernedApe", 60),
            new Videojuego("Hades", "Supergiant", 25),
            new Videojuego("Zelda: BOTW", "Nintendo", 120)
        };
    }

    /// <summary>GestorVideojuegos nuevo con los 8 juegos de ejemplo.</summary>
    public static GestorVideojuegos GestorPoblado() {
        var gestor = new GestorVideojuegos();

        foreach (var juego in Catalogo()) {
            gestor.AgregarJuego(juego);
        }

        return gestor;
    }

    /// <summary>Usuario "Ana" con la colección "Pendientes" que contiene "Celeste".</summary>
    public static Usuario UsuarioConColecciones() {
        var usuario = new Usuario("Ana");
        usuario.CrearColeccion("Pendientes");
        usuario.AgregarJuegoAColeccion("Pendientes", Catalogo()[0]); // Celeste
        return usuario;
    }
}
