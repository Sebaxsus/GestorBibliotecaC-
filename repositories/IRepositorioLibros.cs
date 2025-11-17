using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestorBibliotecaDigital_DiseñoSoft11_2025.domain;

namespace GestorBibliotecaDigital_DiseñoSoft11_2025.repositories
{
    public interface IRepositorioLibros
    {
        void Agregar(Libro libro);
        Libro ObtenerPorISBN(string isbn);
        IEnumerable<Libro> BuscarPorTitulo(string titulo);
        IEnumerable<Libro> BuscarPorAutor(string autor);
        IEnumerable<Libro> BuscarPorCategoria(string categoria);
        IEnumerable<Libro> ObtenerTodos();
    }
}
