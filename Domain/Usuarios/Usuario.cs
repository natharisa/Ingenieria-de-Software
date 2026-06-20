using Abstractions;
using System.Collections.Generic;
using System.Linq;

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
        public int? IdiomaPreferidoId { get; set; }
        public string Estado { get; set; }
        public int IntentosLoginFallidos { get; set; }
        public List<ComponentePermiso> ComponentesPermiso { get; set; } = new List<ComponentePermiso>();

        public bool TienePermiso(string codigoPermiso)
        {
            if (ComponentesPermiso == null || string.IsNullOrWhiteSpace(codigoPermiso))
            {
                return false;
            }

            return ComponentesPermiso.Any(c => c.TienePermiso(codigoPermiso));
        }

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
