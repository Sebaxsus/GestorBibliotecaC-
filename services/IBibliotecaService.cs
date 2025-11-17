using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestorBibliotecaDigital_DiseñoSoft11_2025.domain;

namespace GestorBibliotecaDigital_DiseñoSoft11_2025.services
{
    internal interface IBibliotecaService
    {
        void RegistrarLibro(Libro libro);
        void RegistrarUsuario(Usuario usuario);
        IEnumerable<Libro> BuscarLibrosPorTitulo(string titulo);
        bool PrestarLibro(string isbn, string usuarioId);
        bool DevolverLibro(string isbn, string usuarioId);

        IEnumerable<Prestamo> ListarPrestamosUsuario(string usuarioId);
        IEnumerable<Prestamo> ListarPrestamos();
    }
}
