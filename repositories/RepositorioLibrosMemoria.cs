using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestorBibliotecaDigital_DiseñoSoft11_2025.domain;

namespace GestorBibliotecaDigital_DiseñoSoft11_2025.repositories
{
    public class RepositorioLibrosMemoria : IRepositorioLibros
    {
        // el "_" Hacer referencia a una propiedad de solo lectura
        private readonly List<Libro> _libros = new List<Libro>();

        public void Agregar(Libro libro) => _libros.Add(libro);

        public Libro ObtenerPorISBN(string isbn) => _libros.FirstOrDefault(libro => libro.ISBN == isbn);

        public IEnumerable<Libro> BuscarPorTitulo(string titulo) => _libros.Where(libro => libro.Titulo.Contains(titulo));

        public IEnumerable<Libro> BuscarPorAutor(string autor) => _libros.Where(libro => libro.Autor.Contains(autor));

        public IEnumerable<Libro> BuscarPorCategoria(string categoria) => _libros.Where(libro => libro.Categoria.Equals(categoria, StringComparison.OrdinalIgnoreCase));

        public IEnumerable<Libro> ObtenerTodos() => _libros.AsReadOnly();
    }
}
