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

        public List<Traduccion> ListarTraducciones()
        {
            return _translationRepository.ListarTraducciones();
        }
    }
}
