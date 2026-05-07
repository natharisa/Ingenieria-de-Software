namespace Abstractions
{
    public class ResultadoOperacion<TCodigo>
    {
        private ResultadoOperacion(ResultadoEstado estado, TCodigo codigo, string mensaje)
        {
            Estado = estado;
            Codigo = codigo;
            Mensaje = mensaje;
        }

        public ResultadoEstado Estado { get; private set; }
        public TCodigo Codigo { get; private set; }
        public string Mensaje { get; private set; }
        public bool Exitoso
        {
            get { return Estado == ResultadoEstado.Exitoso; }
        }

        public static ResultadoOperacion<TCodigo> Ok(TCodigo codigo, string mensaje = null)
        {
            return new ResultadoOperacion<TCodigo>(ResultadoEstado.Exitoso, codigo, mensaje);
        }

        public static ResultadoOperacion<TCodigo> FalloNegocio(TCodigo codigo, string mensaje = null)
        {
            return new ResultadoOperacion<TCodigo>(ResultadoEstado.FalloNegocio, codigo, mensaje);
        }

        public static ResultadoOperacion<TCodigo> ErrorTecnico(TCodigo codigo, string mensaje = null)
        {
            return new ResultadoOperacion<TCodigo>(ResultadoEstado.ErrorTecnico, codigo, mensaje);
        }
    }
}
