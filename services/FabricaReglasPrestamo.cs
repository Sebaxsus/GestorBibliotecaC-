using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestorBibliotecaDigital_DiseñoSoft11_2025.domain;

namespace GestorBibliotecaDigital_DiseñoSoft11_2025.services
{
    public static class FabricaReglasPrestamo
    {
        public static IReglaPrestamo ObtenerRegla(TipoUsuario tipo) 
        {
            IReglaPrestamo regla;
            switch (tipo)
            {
                case TipoUsuario.Estudiante:
                    {
                        regla = new ReglaEstudiante();
                        break;
                    }
                case TipoUsuario.Docente:
                    {
                        regla = new ReglaDocente();
                        break;
                    }
                case TipoUsuario.Externo:
                    {
                        regla = new ReglaExterno();
                        break;
                    }
                default:
                    {
                        throw new Exception("Tipo de usuario no soportado");
                        break;
                    }
            }
            return regla;
        }
    }
}
