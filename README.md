## SRP (Single Responsibility):

`Libro`, `Usuario`, `Prestamo` solo contienen datos y lógica pequeña propia.

`RepositorioLibrosEnMemoria` solo gestiona persistencia.

`BibliotecaService` contiene la lógica del negocio (préstamo/devolución) y delega acceso a repositorios.

## DIP (Dependency Inversion):

`BibliotecaService` depende de la abstracción `IRepositorioLibros`.

En `Program/Main` inyectamos la implementación concreta `RepositorioLibrosEnMemoria` (inyección manual).

## OCP (Open/Closed):

`IReglaPrestamo` define la estructura y cada Regla `ReglaEstudiante.cs, ReglaDocente.cs y ReglaExterno.cs` La utiliza pero no la modifican.

| Principio SOLID | Clase/Archivo donde se aplica | Ejemplo concreto | Justificación |
| :---: | :---: | :---: | :---: |
| SRP | `Libro.cs` | La clase solo gestiona su propio estado: Prestar(), Devolver() y sus atributos. . | La clase solo tiene una razón válida para cambiar: reglas internas del comportamiento de un libro. No contiene lógica de préstamos ni de usuarios. |
| | `Usuario.cs` | Contiene únicamente datos del usuario (Id, Nombre, Tipo). | Cambia solo si se modifican los datos del usuario. No carga lógica de negocio. |
| | `RepositorioLibrosMemoria.cs` | Solo se encarga del almacenamiento y consulta de libros en memoria. | Si cambia la forma de persistencia, solo cambia el repositorio, no el dominio ni los servicios. |
| | `BibliotecaService.cs` | Contiene solo lógica de negocio (prestamos y devoluciones). | Si cambian reglas de negocio, solo se modifica esta clase, no el dominio ni los repositorios. |
| OCP | `IReglaPrestamo.cs` | Interfaz define: `int LimiteLibros`, `int DiasPrestamo` | Se puede extender con nuevas reglas sin modificar el código existente. |
| | `ReglaEstudiante.cs` <br/> `ReglaDocente.cs` <br/> `ReglaExterno.cs` | Cada clase implementa distintas políticas de préstamo. | Para añadir un nuevo tipo (p. ej. “Administrador”), solo creas una nueva clase que implemente IReglaPrestamo → NO se modifica el servicio.|
| | `FrabricaReglasPrestamo.cs` | Devuelve la regla según el tipo. | Permite ampliar el sistema sin tocar `BibliotecaService`. |
| | `BibliotecaService.cs` | `var regla = FabricaReglasPrestamo.ObtenerRegla(usuario.Tipo);` | El servicio no sabe los detalles de las reglas → queda abierto a extensión y cerrado a modificación. |
| DIP | `IBibliotecaService.cs` | El servicio depende de una abstracción. | Permite sustituir implementaciones sin tocar la capa de negocio. |
| | `IRepositorioLibros.cs` | El repositorio es una interfaz. | El servicio usa solo la interfaz, no la implementación concreta. |
| | `Program.cs` | `var repoLibros = new RepositorioLibrosMemoria(); IBibliotecaService servicio = new BibliotecaService(repoLibros);` | Se cumple DIP porque la creación de dependencias está fuera del servicio. |
| | `FabricaReglasPrestamo.cs` | Devuelve una instancia de una regla basada en la abstracción `IReglaPrestamo`. | `BibliotecaService` depende de la interfaz, no de las clases concretas de reglas. |


```mermaid
classDiagram
    class Usuario {
        -string Id
        -string Nombre
        -TipoUsuario Tipo
    }

    class TipoUsuario {
        <<enumeration>>
        Estudiante
        Docente
        Externo
    }

    class Libro {
        -string ISBN
        -string Titulo
        -string Autor
        -int Año
        -string Categoria
        -bool Disponible
        +Prestar()
        +Devolver()
    }

    class Prestamo {
        -string Id
        -Libro Libro
        -Usuario Usuario
        -DateTime FechaPrestamo
        -DateTime FechaDevEsp
        -DateTime FechaDevolucion
        +RegistrarDevolucion()
        +DiasRetraso()
        +CalcularMulta()
    }

    class IRepositorioLibros {
        <<interface>>
        +Agregar(Libro)
        +ObtenerPorISBN(string)
        +BuscarPorTitulo(string)
        +BuscarPorAutor(string)
        +BuscarPorCategoria(string)
        +ObtenerTodos()
    }

    class RepositorioLibrosMemoria {
        +Agregar(Libro)
        +ObtenerPorISBN(string)
        +BuscarPorTitulo(string)
        +BuscarPorAutor(string)
        +BuscarPorCategoria(string)
        +ObtenerTodos()
    }

    class IBibliotecaService {
        <<interface>>
        +RegistrarLibro(Libro)
        +RegistrarUsuario(Usuario)
        +PrestarLibro(string, string)
        +DevolverLibro(string)
    }

    class BibliotecaService {
        -IRepositorioLibros repoLibros
        -Dictionary~string,Usuario~ usuarios
        -List~Prestamo~ prestamos
        +RegistrarLibro(Libro)
        +RegistrarUsuario(Usuario)
        +PrestarLibro(string, string)
        +DevolverLibro(string)
    }

    class IReglaPrestamo {
        <<interface>>
        +get LimiteLibros
        +get DiasPrestamo
    }

    class ReglaEstudiante {
        +LimiteLibros
        +DiasPrestamo
    }

    class ReglaDocente {
        +LimiteLibros
        +DiasPrestamo
    }

    class ReglaExterno {
        +LimiteLibros
        +DiasPrestamo
    }

    class FabricaReglasPrestamo {
        +ObtenerRegla(tipoUsuario) IReglaPrestamo
    }

    %% Relaciones
    Usuario --> TipoUsuario
    Prestamo --> Usuario
    Prestamo --> Libro

    IRepositorioLibros <|.. RepositorioLibrosMemoria
    IBibliotecaService <|.. BibliotecaService

    IReglaPrestamo <|.. ReglaEstudiante
    IReglaPrestamo <|.. ReglaDocente
    IReglaPrestamo <|.. ReglaExterno

    BibliotecaService --> IRepositorioLibros
    BibliotecaService --> IReglaPrestamo : usa via fábrica
    FabricaReglasPrestamo --> IReglaPrestamo
```

```mermaid
flowchart TD
    actorUsuario([Usuario])

    subgraph SistemaBiblioteca [Sistema de Gestión de Biblioteca Digital]
        CU1[Registrar Libro]
        CU2[Registrar Usuario]
        CU3[Prestar Libro]
        CU4[Devolver Libro]
        CU5[Consultar Libros]
        CU6[Buscar Libro]
    end

    actorUsuario --> CU2
    actorUsuario --> CU3
    actorUsuario --> CU4
    actorUsuario --> CU5
    actorUsuario --> CU6

    %% Solo personal autorizado
    actorUsuario --> CU1
```

```mermaid
flowchart LR
    subgraph domain [📦 domain]
        D1[Libro]
        D2[Usuario]
        D3[Prestamo]
        D4[TipoUsuario]
    end

    subgraph services[📦 services]
        S1[IBibliotecaService]
        S2[BibliotecaService]
        S3[IReglaPrestamo]
        S4[Reglas Prestamo]
        S5[FabricaReglasPrestamo]
    end

    subgraph repositories[📦 repositories]
        R1[IRepositorioLibros]
        R2[RepositorioLibrosMemoria]
    end

    Main[Main.cs] --> services
    services --> repositories
    services --> domain
    repositories --> domain

```