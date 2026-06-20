using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain
{
    public class FamiliaPermiso : ComponentePermiso
    {
        private readonly List<ComponentePermiso> _hijos = new List<ComponentePermiso>();

        public override TipoComponentePermiso Tipo
        {
            get { return TipoComponentePermiso.Familia; }
        }

        public override void Agregar(ComponentePermiso componente)
        {
            if (componente == null)
            {
                return;
            }

            if (_hijos.Any(h => h.Id == componente.Id && componente.Id != 0))
            {
                return;
            }

            _hijos.Add(componente);
        }

        public override void Quitar(ComponentePermiso componente)
        {
            if (componente == null)
            {
                return;
            }

            _hijos.RemoveAll(h => h.Id == componente.Id);
        }

        public override IReadOnlyList<ComponentePermiso> ObtenerHijos()
        {
            return _hijos.AsReadOnly();
        }

        public override bool TienePermiso(string codigoPermiso)
        {
            if (string.IsNullOrWhiteSpace(codigoPermiso))
            {
                return false;
            }

            return _hijos.Any(h => h.TienePermiso(codigoPermiso));
        }

        public bool PuedeAgregar(ComponentePermiso candidato)
        {
            if (candidato == null)
            {
                return false;
            }

            if (Id != 0 && candidato.Id == Id)
            {
                return false;
            }

            return !Contiene(candidato, Id);
        }

        private static bool Contiene(ComponentePermiso componente, int idBuscado)
        {
            if (componente == null || idBuscado == 0)
            {
                return false;
            }

            if (componente.Id == idBuscado)
            {
                return true;
            }

            return componente.ObtenerHijos().Any(h => Contiene(h, idBuscado));
        }
    }
}
