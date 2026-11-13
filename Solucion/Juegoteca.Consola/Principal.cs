
using System;
using Juegoteca.Biblioteca.Gestores;
using Juegoteca.Biblioteca.Modelos;
using Juegoteca.Biblioteca.Servicios;

namespace Juegoteca.Consola {
    class Program {
        static void Main(string[] args) {
            // 1. Inicialización
            var gestor = new GestorVideojuegos();
            var exportador = new ExportadorTxt();
            var servicio = new ServicioVideojuegos(gestor, exportador);

            // Añadir los 8 juegos de ejemplo requeridos[cite: 16]
            gestor.AgregarJuego(new Videojuego("Celeste", "Maddy Makes Games", 8));
            gestor.AgregarJuego(new Videojuego("Hollow Knight", "Team Cherry", 40));
            gestor.AgregarJuego(new Videojuego("Minecraft", "Mojang", 100));
            gestor.AgregarJuego(new Videojuego("Tetris", "Alexey Pajitnov", 5));
            gestor.AgregarJuego(new Videojuego("Portal 2", "Valve", 10));
            gestor.AgregarJuego(new Videojuego("Stardew Valley", "ConcernedApe", 60));
            gestor.AgregarJuego(new Videojuego("Hades", "Supergiant", 25));
            gestor.AgregarJuego(new Videojuego("Zelda: BOTW", "Nintendo", 120));

            Console.WriteLine("Bienvenido al Sistema de Gestión de Videojuegos");

            // 2. Registro de usuario
            Console.WriteLine("\nREGISTRO DE USUARIO");
            Console.Write("Ingrese su nombre de usuario: ");
            string nombreUsuario = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(nombreUsuario)) {
                nombreUsuario = "Invitado";
            }

            Usuario usuarioActual = servicio.RegistrarUsuario(nombreUsuario);
            Console.WriteLine($"Usuario '{usuarioActual.Nombre}' registrado. ¡Bienvenido/va, {usuarioActual.Nombre}!");

            // 3. Creación de colección inicial[cite: 16]
            Console.Write("Nombre de su primera colección: ");
            string nombreColeccionInicial = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(nombreColeccionInicial)) {
                nombreColeccionInicial = "Pendientes";
            }

            try {
                usuarioActual.CrearColeccion(nombreColeccionInicial);
                Console.WriteLine($"Colección '{nombreColeccionInicial}' creada exitosamente.");
            } catch (Exception ex) {
                Console.WriteLine($"Aviso: {ex.Message}");
                nombreColeccionInicial = "Pendientes";
                if (!usuarioActual.Colecciones.ContainsKey(nombreColeccionInicial)) {
                    usuarioActual.CrearColeccion(nombreColeccionInicial);
                }
            }

            string coleccionActual = nombreColeccionInicial;

            // 4. Menú principal (bucle hasta salir)
            bool salir = false;
            while (!salir) {
                var juegosCol = usuarioActual.ObtenerColeccion(coleccionActual);
                Console.WriteLine("\n----------------------------------------");
                Console.WriteLine($"MENÚ PRINCIPAL");
                Console.WriteLine($"Colección actual: '{coleccionActual}' ({juegosCol.Count} juegos)");
                Console.WriteLine("1. Mostrar juegos disponibles");
                Console.WriteLine("2. Ordenar colección (QuickSort o MergeSort)");
                Console.WriteLine("3. Búsqueda binaria en catálogo");
                Console.WriteLine("4. Crear nueva colección");
                Console.WriteLine("5. Cambiar de colección actual");
                Console.WriteLine("6. Ver mis colecciones");
                Console.WriteLine("7. Exportar mis colecciones a .txt");
                Console.WriteLine("8. Salir");
                Console.Write("Seleccione una opción: ");

                string input = Console.ReadLine();
                if (!int.TryParse(input, out int opcion)) [cite: 19]
                {
                    Console.WriteLine("Entrada inválida. Por favor, ingrese un número del 1 al 8.");
                    continue;
                }

                switch (opcion) { 
                    case 1:
                        // Mostrar juegos del catálogo que se pueden incluir en la colección actual[cite: 17]
                        Console.WriteLine("\n--- CATÁLOGO DE JUEGOS DISPONIBLES ---");
                        if (gestor.JuegosDisponibles.Count == 0) {
                            Console.WriteLine("No hay juegos en el catálogo.");
                        } else {
                            for (int i = 0; i < gestor.JuegosDisponibles.Count; i++) {
                                Console.WriteLine($"{i + 1}. {gestor.JuegosDisponibles[i].ToString()}");
                            }

                            Console.Write("\n¿Desea agregar algún juego de aquí a su colección actual? (s/n): ");
                            if (Console.ReadLine()?.Trim().Equals("s", StringComparison.OrdinalIgnoreCase) == true) {
                                Console.Write("Ingrese el número del juego a agregar (0 para cancelar): ");
                                if (int.TryParse(Console.ReadLine(), out int idxJuego) && idxJuego > 0 && idxJuego <= gestor.JuegosDisponibles.Count) {
                                    var juegoSeleccionado = gestor.JuegosDisponibles[idxJuego - 1];
                                    try {
                                        usuarioActual.AgregarJuegoAColeccion(coleccionActual, juegoSeleccionado);
                                        Console.WriteLine($"¡Juego '{juegoSeleccionado.Titulo}' agregado a '{coleccionActual}'!");
                                    } catch (Exception ex) {
                                        Console.WriteLine($"Error: {ex.Message}");
                                    }
                                }
                            }
                        }
                        break;

                    case 2:
                        // Ordenar colección (QuickSort o MergeSort) y mostrar el total de horas[cite: 17]
                        Console.WriteLine("Elija el algoritmo de ordenamiento:");
                        Console.WriteLine("1. QuickSort");
                        Console.WriteLine("2. MergeSort");
                        Console.Write("Seleccione una opción de algoritmo: ");
                        string algOpt = Console.ReadLine();

                        try {
                            if (algOpt == "1") {
                                servicio.OrdenarCatalogo("quick");
                            } else if (algOpt == "2") {
                                servicio.OrdenarCatalogo("merge");
                            } else {
                                Console.WriteLine("Algoritmo no reconocido. Usando orden por defecto.");
                            }

                            int totalHoras = servicio.OrdenarColeccionYCalcularHoras(usuarioActual, coleccionActual);
                            Console.WriteLine($"Colección '{coleccionActual}' ordenada correctamente.");
                            Console.WriteLine($"Total de horas de juego en '{coleccionActual}': {totalHoras} h");
                        } catch (Exception ex) {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                        break;

                    case 3:
                        // Búsqueda binaria en catálogo[cite: 17]
                        Console.Write("Ingrese el título del videojuego a buscar: ");
                        string tituloBusqueda = Console.ReadLine();
                        int indiceEncontrado = gestor.BusquedaBinaria(tituloBusqueda);

                        if (indiceEncontrado >= 0) {
                            var juegoEncontrado = gestor.JuegosDisponibles[indiceEncontrado];
                            Console.WriteLine($"¡Encontrado! [{indiceEncontrado + 1}] {juegoEncontrado.ToString()}");
                            Console.Write($"¿Desea agregar '{juegoEncontrado.Titulo}' a la colección '{coleccionActual}'? (s/n): ");
                            if (Console.ReadLine()?.Trim().Equals("s", StringComparison.OrdinalIgnoreCase) == true) {
                                try {
                                    usuarioActual.AgregarJuegoAColeccion(coleccionActual, juegoEncontrado);
                                    Console.WriteLine("Juego agregado exitosamente a la colección.");
                                } catch (Exception ex) {
                                    Console.WriteLine($"Error: {ex.Message}");
                                }
                            }
                        } else {
                            Console.WriteLine("No se encontraron coincidencias con ese título en el catálogo."); [cite: 17]
                        }
                        break;

                    case 4:
                        // Crear nueva colección[cite: 17]
                        Console.Write("Ingrese el nombre de la nueva colección: ");
                        string nuevaCol = Console.ReadLine();
                        try {
                            usuarioActual.CrearColeccion(nuevaCol);
                            Console.WriteLine($"Colección '{nuevaCol}' creada exitosamente.");
                        } catch (InvalidOperationException ex) {
                            Console.WriteLine($"Error: {ex.Message}"); [cite: 17, 19]
                        }
                        break;

                    case 5:
                        // Cambiar de colección actual[cite: 18]
                        Console.WriteLine("Colecciones disponibles de su usuario:");
                        foreach (var colName in usuarioActual.Colecciones.Keys) {
                            Console.WriteLine($"- {colName}");
                        }
                        Console.Write("Ingrese el nombre de la colección que será la actual: ");
                        string cambiarA = Console.ReadLine();
                        if (usuarioActual.Colecciones.ContainsKey(cambiarA)) {
                            coleccionActual = cambiarA;
                            Console.WriteLine($"Colección actual cambiada a: '{coleccionActual}'");
                        } else {
                            Console.WriteLine("La colección especificada no existe.");
                        }
                        break;

                    case 6:
                        // Ver mis colecciones[cite: 18]
                        Console.WriteLine("\n--- SUS COLECCIONES ---");
                        usuarioActual.MostrarColecciones();
                        break;

                    case 7:
                        // Exportar a .txt[cite: 18]
                        Console.Write("Ingrese la ruta del archivo .txt (ej. colecciones.txt): ");
                        string ruta = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(ruta)) {
                            ruta = "colecciones.txt";
                        }
                        try {
                            servicio.ExportarColecciones(usuarioActual, ruta);
                            Console.WriteLine($"Colecciones exportadas exitosamente a '{ruta}'.");
                        } catch (Exception ex) {
                            Console.WriteLine($"Error al exportar: {ex.Message}");
                        }
                        break;

                    case 8:
                        salir = true;
                        Console.WriteLine("¡Gracias por usar el Sistema de Gestión de Videojuegos!");
                        break;

                    default:
                        Console.WriteLine("Opción no válida. Por favor, seleccione un número entre 1 y 8.");
                        break;
                }
            }
        }
    }
}