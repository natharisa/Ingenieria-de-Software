using System.Collections.Generic;
using Domain;
using Repository;

namespace Application
{
    public class IdiomaApplicationService
    {
        private readonly LanguageRepository _languageRepository;
        private readonly TranslationRepository _translationRepository;

        public IdiomaApplicationService()
            : this(new LanguageRepository(), new TranslationRepository())
        {
        }

        public IdiomaApplicationService(
            LanguageRepository languageRepository,
            TranslationRepository translationRepository)
        {
            _languageRepository = languageRepository;
            _translationRepository = translationRepository;
        }

        public List<Idioma> ListarIdiomas(bool soloActivos)
        {
            return _languageRepository.Listar(soloActivos);
        }

        public bool GuardarIdioma(Idioma idioma, int? idUsuarioResponsable, string motivo)
        {
            if (idioma == null ||
                string.IsNullOrWhiteSpace(idioma.Codigo) ||
                string.IsNullOrWhiteSpace(idioma.Nombre))
            {
                return false;
            }

            idioma.Codigo = idioma.Codigo.Trim();
            idioma.Nombre = idioma.Nombre.Trim();

            if (idioma.Id == 0)
            {
                return _languageRepository.Crear(idioma, idUsuarioResponsable) > 0;
            }

            return _languageRepository.Actualizar(idioma, idUsuarioResponsable, motivo);
        }

        public List<Etiqueta> ListarEtiquetas()
        {
            return _translationRepository.ListarEtiquetas();
        }

        public bool CrearEtiqueta(Etiqueta etiqueta)
        {
            if (etiqueta == null || string.IsNullOrWhiteSpace(etiqueta.Key))
            {
                return false;
            }

            etiqueta.Key = etiqueta.Key.Trim();
            etiqueta.Descripcion = string.IsNullOrWhiteSpace(etiqueta.Descripcion)
                ? null
                : etiqueta.Descripcion.Trim();

            return _translationRepository.CrearEtiqueta(etiqueta) > 0;
        }

        public bool GuardarTraduccion(Traduccion traduccion)
        {
            if (traduccion == null ||
                traduccion.EtiquetaId == 0 ||
                traduccion.IdiomaId == 0 ||
                string.IsNullOrWhiteSpace(traduccion.Texto))
            {
                return false;
            }

            traduccion.Texto = traduccion.Texto.Trim();
            return _translationRepository.GuardarTraduccion(traduccion);
        }

        public bool GuardarTraduccionDetectada(string key, string descripcion, int idiomaId, string texto)
        {
            if (string.IsNullOrWhiteSpace(key) ||
                idiomaId == 0 ||
                string.IsNullOrWhiteSpace(texto))
            {
                return false;
            }

            key = key.Trim();
            texto = texto.Trim();
            descripcion = string.IsNullOrWhiteSpace(descripcion) ? null : descripcion.Trim();

            Etiqueta etiqueta = null;
            foreach (Etiqueta item in _translationRepository.ListarEtiquetas())
            {
                if (string.Equals(item.Key, key, System.StringComparison.OrdinalIgnoreCase))
                {
                    etiqueta = item;
                    break;
                }
            }

            if (etiqueta == null)
            {
                etiqueta = new Etiqueta
                {
                    Key = key,
                    Descripcion = descripcion
                };

                if (_translationRepository.CrearEtiqueta(etiqueta) <= 0)
                {
                    return false;
                }
            }

            return _translationRepository.GuardarTraduccion(new Traduccion
            {
                EtiquetaId = etiqueta.Id,
                IdiomaId = idiomaId,
                Texto = texto
            });
        }

        public List<Traduccion> ListarTraducciones()
        {
            return _translationRepository.ListarTraducciones();
        }

        public Traduccion ObtenerTraduccion(int etiquetaId, int idiomaId)
        {
            if (etiquetaId == 0 || idiomaId == 0)
            {
                return null;
            }

            return _translationRepository.ObtenerTraduccion(etiquetaId, idiomaId);
        }
    }
}
