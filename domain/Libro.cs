using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorBibliotecaDigital_DiseñoSoft11_2025.domain
{
    public class Libro
    {
        public string ISBN { get; }
        public string Titulo { get; }
        public string Autor { get; }
        public int Año { get; }
        public string Categoria { get; }
        public bool Disponible { get; private set; } = true;

        public Libro(string isbn, string titulo, string autor, int año, string categoria, bool disponible)
        {
            ISBN = isbn;
            Titulo = titulo;
            Autor = autor;
            Año = año;
            Categoria = categoria;
            Disponible = disponible;

        }

        public void Prestar()
        {
            if (!Disponible)
            {
                throw new InvalidOperationException("Libro no disponible");
            }
            
            Disponible = false;

        }

        public void Devolver()
        {
            Disponible = true;
        }
    }

}
