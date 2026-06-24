using Abstractions;

namespace Domain
{
    public class Idioma : IEntity
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public bool Activo { get; set; }

        public override string ToString()
        {
            return string.IsNullOrWhiteSpace(Nombre) ? Codigo : Nombre;
        }
    }
}
