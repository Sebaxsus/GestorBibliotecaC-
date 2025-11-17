using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorBibliotecaDigital_DiseñoSoft11_2025.services
{
    public class ReglaExterno : IReglaPrestamo
    {
        public int LimiteLibros => 2;
        public int DiasPrestamo => 5;
    }
}
