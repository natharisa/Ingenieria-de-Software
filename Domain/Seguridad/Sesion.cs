using System;

namespace Domain
{
    public sealed class Sesion
    {
        private static Sesion _instance;
        private static readonly object _lock = new object();

        public Usuario Usuario { get; set; }
        public DateTime FechaInicio { get; set; }

        private Sesion()
        {
        }

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

        public void Logout()
        {
            Usuario = null;
        }
    }
}
