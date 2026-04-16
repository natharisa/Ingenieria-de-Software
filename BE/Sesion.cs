using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public sealed class Sesion
    {
        private static Sesion _instance;
        private static readonly object _lock = new object();

        // Propiedad para almacenar los datos del usuario (tu clase Usuario de BE)
        public Usuario Usuario { get; set; }
        public DateTime FechaInicio { get; set; }

        // Constructor privado
        private Sesion() { }

        // Punto de acceso global
        //Esto es para que no haya otra instancia 
        public static Sesion GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new Sesion();
                    }
                }
            }
            return _instance;
        }

        //Cerrar sesión
        public void Logout()
        {
            Usuario= null;
        }


    }
}
