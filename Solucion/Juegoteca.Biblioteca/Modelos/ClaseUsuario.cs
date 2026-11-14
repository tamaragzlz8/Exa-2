using System;
using System.Collections.Generic;

namespace Juegoteca.Biblioteca.Modelos {
    public class Usuario {
        public string Nombre { get; set; }
        public Dictionary<string, List<Videojuego>> Colecciones { get; }

        public Usuario(string nombre) {
            Nombre = nombre;
            Colecciones = new Dictionary<string, List<Videojuego>>(StringComparer.OrdinalIgnoreCase);

        }


        public void CrearColeccion(string nombre) {
            
            if (Colecciones.ContainsKey(nombre)) {
                throw new InvalidOperationException("La colección ya existe");
            }
            //Agregarla al diccionario con una nueva lista
            Colecciones.Add(nombre, new List<Videojuego>());

        }

        public void AgregarJuegoAColeccion(string nombreColeccion, Videojuego juego) {
            ArgumentNullException.ThrowIfNull(juego);
            if (!Colecciones.ContainsKey(nombreColeccion)) {
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
                Console.WriteLine("No hay colecciones registradas");
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
