using Abstractions;

namespace Domain
{
    public class Traduccion : IEntity
    {
        public int Id { get; set; }
        public int EtiquetaId { get; set; }
        public int IdiomaId { get; set; }
        public string Texto { get; set; }
        public string EtiquetaKey { get; set; }
        public string IdiomaCodigo { get; set; }
    }
}
