using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain;
using Repository;

namespace Application
{
    public class PermisoApplicationService
    {
        private readonly PermisoRepository _permisoRepository;

        public PermisoApplicationService()
            : this(new PermisoRepository())
        {
        }

        public PermisoApplicationService(PermisoRepository permisoRepository)
        {
            _permisoRepository = permisoRepository;
        }

        public List<ComponentePermiso> ListarAsignadosPorUsuario(int idUsuario)
        {
            return _permisoRepository.ListarAsignadosPorUsuario(idUsuario);
        }

        public List<ComponentePermiso> ListarArbolCompleto()
        {
            return _permisoRepository.ListarArbolCompleto();
        }

        public List<ComponentePermiso> ListarComponentes()
        {
            return _permisoRepository.ListarComponentes();
        }

        public List<ComponentePermiso> ListarFamilias()
        {
            return _permisoRepository.ListarFamilias();
        }

        public List<int> ListarIdsComponentesAsignadosPorUsuario(int idUsuario)
        {
            return _permisoRepository.ListarIdsComponentesAsignadosPorUsuario(idUsuario);
        }

        public bool CrearFamilia(string codigo, string nombre, string descripcion)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                return false;
            }

            string codigoNormalizado = NormalizarCodigo(string.IsNullOrWhiteSpace(codigo) ? nombre : codigo);
            return !string.IsNullOrWhiteSpace(codigoNormalizado) &&
                   _permisoRepository.CrearFamilia(codigoNormalizado, nombre.Trim(), descripcion);
        }

        public string AgregarRelacion(int idPadre, int idHijo)
        {
            if (idPadre == 0 || idHijo == 0)
            {
                return "DATOS_INVALIDOS";
            }

            return _permisoRepository.AgregarRelacion(idPadre, idHijo);
        }

        public bool QuitarRelacion(int idPadre, int idHijo)
        {
            return idPadre != 0 && idHijo != 0 && _permisoRepository.QuitarRelacion(idPadre, idHijo);
        }

        public bool GuardarComponentesUsuario(int idUsuario, List<int> idsComponentes)
        {
            return idUsuario != 0 && _permisoRepository.GuardarComponentesUsuario(idUsuario, idsComponentes);
        }

        public bool PuedeAgregarRelacion(int idPadre, int idHijo)
        {
            return _permisoRepository.PuedeAgregarRelacion(idPadre, idHijo);
        }

        private static string NormalizarCodigo(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
            {
                return string.Empty;
            }

            StringBuilder builder = new StringBuilder();

            foreach (char caracter in texto.Trim().ToUpperInvariant())
            {
                if (char.IsLetterOrDigit(caracter))
                {
                    builder.Append(caracter);
                }
                else if (builder.Length > 0 && builder[builder.Length - 1] != '_')
                {
                    builder.Append('_');
                }
            }

            return builder.ToString().Trim('_');
        }
    }
}
