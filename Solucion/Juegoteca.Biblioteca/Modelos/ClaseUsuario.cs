using System;
using System.Collections.Generic;

namespace Juegoteca.Biblioteca.Modelos {
    public class Usuario {
        string Nombre { get; }
        public Dictionary<string, List<Videojuego>> Colecciones { get; }

        public Usuario(string nombre) {
            Nombre = nombre;
            Colecciones = new Dictionary<string, List<Videojuego>>(StringComparer.OrdinalIgnoreCase);

        }


        public void CrearColeccion(string nombre) {
            if (Colecciones.ContainsKey(nombre)) {
                throw new InvalidOperationException();
            }
            Colecciones[nombre] = new List<Videojuego>();

        }

        public void AgregarJuegoAColeccion(string nombreColeccion, Videojuego juego) {
            if (Colecciones.ContainsKey(nombreColeccion)) {
                throw new KeyNotFoundException();
            }
            var lista = Colecciones[nombreColeccion];
            // Evita agregar el mismo juego dos veces a la misma colección (comparando por título, por ejemplo)
            if (!lista.Exists(j => j.Titulo.Equals(juego.Titulo, StringComparison.OrdinalIgnoreCase))) {
                lista.Add(juego);
            }
        }

        public bool EliminarColeccion(string nombre) {
            return Colecciones.Remove(nombre);
        }

        public List<Videojuego> ObtenerColeccion(string nombre) {
            if (!Colecciones.ContainsKey(nombre)) {
                throw new KeyNotFoundException($"La colección '{nombre}' no existe.");
            }
            return Colecciones[nombre];
        }

        public int ContarJuegos() {
            int total = 0;
            foreach (var col in Colecciones.Values) {
                total += col.Count;
            }
            return total;
        }

        public void MostrarColecciones() {
            if (Colecciones.Count == 0) {
                Console.WriteLine("El usuario no tiene colecciones creadas.");
                return;
            }

            foreach (var par in Colecciones) {
                Console.WriteLine($"Colección: {par.Key} ({par.Value.Count} juegos)");
                foreach (var juego in par.Value) {
                    Console.WriteLine($"  - {juego.ToString()}");
                }
            }
        }
    }
}
