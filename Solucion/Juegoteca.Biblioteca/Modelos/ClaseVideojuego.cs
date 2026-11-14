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
        public override bool Equals(object obj)
        {
            if (obj is Videojuego otro)
            {
                return string.Equals(Titulo, otro.Titulo, StringComparison.OrdinalIgnoreCase) &&
                       string.Equals(Estudio, otro.Estudio, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Titulo?.ToLowerInvariant(), Estudio?.ToLowerInvariant());
        }
    }
}