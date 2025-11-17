using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestorBibliotecaDigital_DiseñoSoft11_2025.domain;
using GestorBibliotecaDigital_DiseñoSoft11_2025.repositories;

namespace GestorBibliotecaDigital_DiseñoSoft11_2025.services
{
    public class BibliotecaService : IBibliotecaService
    {
        private readonly IRepositorioLibros _repoLibros;
        private readonly IDictionary<string, Usuario> _usuarios = new Dictionary<string, Usuario> ();
        private readonly List<Prestamo> _prestamos = new List<Prestamo>();

        private readonly IDictionary<TipoUsuario, int> _limitesPorTipo = new Dictionary<TipoUsuario, int> 
        { 
            { TipoUsuario.Estudiante, 3},
            { TipoUsuario.Docente, 5 },
            { TipoUsuario.Externo, 2 },
        };

        public BibliotecaService(IRepositorioLibros repoLibros)
        {
            _repoLibros = repoLibros ?? throw new ArgumentNullException(nameof(repoLibros));
        }

        public void RegistrarLibro(Libro libro) => _repoLibros.Agregar(libro);

        public void RegistrarUsuario(Usuario usuario)
        {
            if (!_usuarios.ContainsKey(usuario.Id))
            {
                _usuarios.Add(usuario.Id, usuario);
                Console.WriteLine("Usuario registrado!, " + usuario.Nombre + " (" + usuario.Id + ")");
            } else
            {
                Console.WriteLine("EL Usuario ya existe!");
            }
        }

        public IEnumerable<Libro> BuscarLibrosPorTitulo(string titulo) => _repoLibros.BuscarPorTitulo(titulo);

        public bool PrestarLibro(string isbn, string usuarioId)
        {
            var libro = _repoLibros.ObtenerPorISBN(isbn);
            if (libro == null || !libro.Disponible) return false;
            if (!_usuarios.TryGetValue(usuarioId, out var usuario)) return false;

            var regla = FabricaReglasPrestamo.ObtenerRegla(usuario.Tipo);

            // validar límite
            var librosPrestados = _prestamos.Count(p => p.Usuario.Id == usuarioId && !p.EstaDevuelto);
            if (librosPrestados >= regla.LimiteLibros)
            {
                Console.WriteLine($"Ya llegaste al limite de libros permitdos para prestamos ${regla.LimiteLibros}");
                return false;
            };

            // crear préstamo
            libro.Prestar();
            var prestamo = new Prestamo(Guid.NewGuid().ToString(), libro, usuario, DateTime.Now, DateTime.Now.AddDays(regla.DiasPrestamo));
            _prestamos.Add(prestamo);
            return true;
        }

        public bool DevolverLibro(string isbn, string usuarioId)
        {
            var prestamo = _prestamos.FirstOrDefault(p => p.Libro.ISBN == isbn && p.Usuario.Id == usuarioId && !p.EstaDevuelto);
            if (prestamo == null) return false;
            prestamo.RegistrarDevolucion(DateTime.Now);
            prestamo.Libro.Devolver();
            return true;
        }
        public IEnumerable<Prestamo> ListarPrestamosUsuario(string usuarioId)
        {
            return _prestamos.Where(p => p.Usuario.Id == usuarioId);
        }

        public IEnumerable<Prestamo> ListarPrestamos()
        {
            return _prestamos;
        }

    }
}
