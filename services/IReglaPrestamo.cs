using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorBibliotecaDigital_DiseñoSoft11_2025.services
{
    public interface IReglaPrestamo
    {
        int LimiteLibros { get; }
        int DiasPrestamo { get; }
    }
}
