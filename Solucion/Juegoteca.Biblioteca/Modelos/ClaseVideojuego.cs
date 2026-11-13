using System;
using Juegoteca.Biblioteca.Interfaces;
namespace Juegoteca.Biblioteca.Modelos {
    public class Videojuego : IVideojuego {
        public string Titulo { get; }
        public string Estudio { get; }
        public int HorasJuego { get; }

        public Videojuego(string titulo, string estudio, int horasJuego) {
            if (horasJuego <= 0) {
                throw new ArgumentException();
            }
            Titulo = titulo;
            Estudio = estudio;
            HorasJuego = horasJuego;
        }
        public override string ToString() {
            return $"{Titulo} - {Estudio} ({HorasJuego} h)";
        }
    }
}