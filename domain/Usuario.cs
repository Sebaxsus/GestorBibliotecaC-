using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorBibliotecaDigital_DiseñoSoft11_2025.domain
{
    public class Usuario
    {
        public string Id { get; }
        public string Nombre { get; }
        public TipoUsuario Tipo { get; }

        public Usuario(string id, string nombre, TipoUsuario tipo)
        {
            Id = id;
            Nombre = nombre;
            Tipo = tipo;
        }
    }
}
