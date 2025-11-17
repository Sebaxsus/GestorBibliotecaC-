using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestorBibliotecaDigital_DiseñoSoft11_2025.domain;
using GestorBibliotecaDigital_DiseñoSoft11_2025.repositories;
using GestorBibliotecaDigital_DiseñoSoft11_2025.services;
using static System.Console;

namespace GestorBibliotecaDigital_DiseñoSoft11_2025
{

    internal class Program
    {

        static void Main(string[] args)
        {
            
            // Inyección manual
            var repoLibros = new RepositorioLibrosMemoria();
            IBibliotecaService biblioteca = new BibliotecaService(repoLibros);

            // Datos de ejemplo
            var l1 = new Libro("978-111", "Aprendiendo C#", "Autor A", 2020, "Programacion", true);
            var l2 = new Libro("978-842", "Cien años de soledad", "Gabriel García Márquez", 1967, "Novela", true);
            var l3 = new Libro("978-030", "El senor de los anillos", "J. R. R. Tolkien", 1954, "Fantasia", true);
            var l4 = new Libro("978-842", "La sombra del viento", "Carlos Ruiz Zafón", 2001, "Misterio", true);
            biblioteca.RegistrarLibro(l1);
            biblioteca.RegistrarLibro(l2);
            biblioteca.RegistrarLibro(l3);
            biblioteca.RegistrarLibro(l4);

            bool flag = true;
            var cont = 1;
            List<Usuario> usuarios = new List<Usuario>();
            while (flag)
            {
                WriteLine("Porfavor registrese: ");
                Write("Nombre de usuario: ");
                var user = ReadLine()?.Trim();
                Usuario usuario;

                // Validando que halla escrito un nombre de usuario
                while (string.IsNullOrWhiteSpace(user))
                {
                    WriteLine("El nombre de usuario no puede ser vacio!");
                    Write("Digite su nombre de usuario: ");
                    user = ReadLine()?.Trim();
                }

                void crearUsuario()
                {
                    WriteLine("Tipo:\n1. Estudiante\n2. Docente\n3. Externo");
                    switch (int.Parse(ReadLine()))
                    {
                        case 1:
                            {
                                usuario = new Usuario("u" + cont, user, TipoUsuario.Estudiante);
                                break;
                            }
                        case 2:
                            {
                                usuario = new Usuario("d" + cont, user, TipoUsuario.Docente);
                                break;
                            }
                        case 3:
                            {
                                usuario = new Usuario("e" + cont, user, TipoUsuario.Externo);
                                break;
                            }
                        default:
                            {
                                WriteLine("Verifique la opcion seleccionada");
                                crearUsuario();
                                break;
                            }
                    }
                }

                crearUsuario();
                cont++;

                biblioteca.RegistrarUsuario(usuario);

                bool salirUsuario = false;
                while (!salirUsuario)
                {

                    MostrarMenu(usuario.Tipo);
                    var entrada = ReadLine();

                    if (!int.TryParse(entrada, out int opcion))
                    {
                        WriteLine("Opcion invalida, Intente de nuevo.");
                        continue;
                    }

                    if (usuario.Tipo != TipoUsuario.Docente)
                    {
                        switch (opcion)
                        {
                            case 1:
                                AccionRegistrarPrestamo(biblioteca, usuario);
                                break;
                            case 2:
                                AccionListarPrestamos(biblioteca, usuario);
                                break;
                            case 3:
                                AccionListarLibros(biblioteca);
                                break;
                            case 4:
                                AccionBuscarLibro(biblioteca);
                                break;
                            case 0:
                                break;
                            default:
                                WriteLine("Opcion no valida.");
                                break;
                        }
                    } else
                    {
                        switch (opcion)
                        {
                            case 1:
                                AccionRegistrarLibro(biblioteca);
                                break;
                            case 2:
                                AccionRegistrarPrestamo(biblioteca, usuario);
                                break;
                            case 3:
                                AccionProcesarPrestamo(biblioteca, usuario);
                                break;
                            case 4:
                                AccionListarPrestamos(biblioteca, usuario);
                                break;
                            case 5:
                                AccionListarLibros(biblioteca);
                                break;
                            case 6:
                                AccionBuscarLibro(biblioteca);
                                break;
                            case 0:
                                break;
                            default:
                                WriteLine("Opcion no valida.");
                                break;
                        }
                    }
                }

                WriteLine("Desea registrar otro usuario? (s/n)");
                var r = ReadLine();
                if (string.IsNullOrEmpty(r) || !r.Trim().ToLower().StartsWith("s"))
                {
                    break;
                }
            }

            WriteLine("Saliendo... presione una tecla.");
            ReadKey();

        }
        static void MostrarMenu(TipoUsuario tipo)
        {
            if (tipo == TipoUsuario.Docente)
            {
                WriteLine("\n=== Menú Docente ===");
                WriteLine("1. Registrar Libro.");
                WriteLine("2. Registrar un Prestamo.");
                WriteLine("3. Procesar un prestamo (Devolución).");
                WriteLine("4. Listar prestamos.");
                WriteLine("5. Listar Libros");
                WriteLine("6. Buscar libro.");
                WriteLine("0. Salir sesión.");
            }
            else
            {
                WriteLine("\n=== Menú Usuario ===");
                WriteLine("1. Registrar un Prestamo.");
                WriteLine("2. Listar prestamos.");
                WriteLine("3. Listar Libros");
                WriteLine("4. Buscar libro.");
                WriteLine("0. Salir sesión.");
            }

            Write("Selecciona una opción: ");
        }

        // ACCIONES auxiliares para mantener Main limpio
        static void AccionRegistrarLibro(IBibliotecaService servicio)
        {
            Write("ISBN: "); var isbn = ReadLine();
            Write("Titulo: "); var titulo = ReadLine();
            Write("Autor: "); var autor = ReadLine();
            Write("Año: "); var añoStr = ReadLine();
            int.TryParse(añoStr, out int año);
            Write("Categoria: "); var cat = ReadLine();

            var libro = new Libro(isbn, titulo, autor, año, cat, true);
            servicio.RegistrarLibro(libro);
            WriteLine("Libro registrado: " + libro.Titulo);
        }

        static void AccionRegistrarPrestamo(IBibliotecaService servicio, Usuario usuario)
        {
            Write("ISBN a prestar: ");
            var isbn = ReadLine();
            if (string.IsNullOrWhiteSpace(isbn)) { WriteLine("ISBN inválido."); return; }
            var exito = servicio.PrestarLibro(isbn.Trim(), usuario.Id);
            WriteLine(exito ? "Préstamo realizado" : "No se pudo prestar (no disponible o límite alcanzado).");
        }

        static void AccionProcesarPrestamo(IBibliotecaService servicio, Usuario usuario)
        {
            Write("ISBN a devolver: ");
            var isbn = ReadLine();
            if (string.IsNullOrWhiteSpace(isbn)) { WriteLine("ISBN inválido."); return; }
            var exito = servicio.DevolverLibro(isbn.Trim(), usuario.Id);
            WriteLine(exito ? "Devolución exitosa" : "No se encontró préstamo activo para ese ISBN y usuario.");
        }

        static void AccionListarPrestamos(IBibliotecaService servicio, Usuario usuario)
        {
            WriteLine($"ID: {usuario.Id} - Name: {usuario.Nombre} - Tipo: {usuario.Tipo}");
            var prestamos = servicio.ListarPrestamosUsuario(usuario.Id).ToList();
            if (!prestamos.Any()) { WriteLine("No tiene préstamos."); return; }
            foreach (var p in prestamos)
            {
                WriteLine($"ID: {p.Id} - Libro: {p.Libro.Titulo} - Prestado: {p.FechaPrestamo} - Devuelto: {(p.EstaDevuelto ? p.FechaDevolcion.ToString() : "No")}");
            }
        }

        static void AccionListarLibros(IBibliotecaService servicio)
        {
            var libros = servicio.BuscarLibrosPorTitulo(""); // o implementar ObtenerTodos en servicio si prefieres
            foreach (var l in libros)
            {
                WriteLine($"{l.ISBN} | {l.Titulo} | {l.Autor} | Disponible: {l.Disponible}");
            }
        }

        static void AccionBuscarLibro(IBibliotecaService servicio)
        {
            Write("Ingrese término de búsqueda (título): ");
            var termino = ReadLine();
            var resultados = servicio.BuscarLibrosPorTitulo(termino ?? "");
            foreach (var l in resultados)
            {
                WriteLine($"{l.ISBN} | {l.Titulo} | {l.Autor} | Disponible: {l.Disponible}");
            }
        }

    }
}
