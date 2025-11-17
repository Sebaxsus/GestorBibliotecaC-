using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorBibliotecaDigital_DiseñoSoft11_2025.domain
{
    public class Prestamo
    {
        public string Id { get; }
        public Libro Libro { get; }
        public Usuario Usuario { get; }
        public DateTime FechaPrestamo { get; }
        public DateTime FechaDevolucionEsperada { get; }
        public DateTime? FechaDevolcion { get; private set; }

        public Prestamo(string id, Libro libro, Usuario usuario, DateTime fechaPrestamo, DateTime fechaDevolucionEsperada)
        {
            Id = id;
            Libro = libro;
            Usuario = usuario;
            FechaPrestamo = fechaPrestamo;
            FechaDevolucionEsperada = fechaDevolucionEsperada;
            FechaDevolcion = null;
        }

        public void RegistrarDevolucion(DateTime fecha)
        {
            FechaDevolcion = fecha;
        }

        public int DiasRetraso()
        {
            if (FechaDevolcion.HasValue) return 0;
            var diff = (FechaDevolcion.Value.Date - FechaDevolucionEsperada.Date).Days;
            return diff > 0 ? diff : 1;
        }

        public decimal CalcularMulta(decimal taricaPorDia)
        {
            return DiasRetraso() * taricaPorDia;
        }

        public bool EstaDevuelto => FechaDevolcion.HasValue;
    }
}
