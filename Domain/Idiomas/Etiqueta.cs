using Abstractions;

namespace Domain
{
    public class Etiqueta : IEntity
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Descripcion { get; set; }

        public override string ToString()
        {
            return Key ?? string.Empty;
        }
    }
}
