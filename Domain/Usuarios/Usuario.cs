using Abstractions;

namespace Domain
{
    public class Usuario : IEntity
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Idioma { get; set; }

        public override string ToString()
        {
            string nombreCompleto = $"{Nombre} {Apellido}".Trim();

            if (!string.IsNullOrWhiteSpace(nombreCompleto))
            {
                return nombreCompleto;
            }

            if (!string.IsNullOrWhiteSpace(Username))
            {
                return Username;
            }

            return Email ?? string.Empty;
        }
    }
}
