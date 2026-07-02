using System.Collections.Generic;
using Domain;

namespace Application
{
    public class AuditoriaCaretaker
    {
        private readonly List<AuditoriaMemento> _mementos = new List<AuditoriaMemento>();

        public void AddMemento(AuditoriaMemento memento)
        {
            if (memento != null)
            {
                _mementos.Add(memento);
            }
        }

        public AuditoriaMemento GetMemento(int index)
        {
            if (index < 0 || index >= _mementos.Count)
            {
                return null;
            }

            return _mementos[index];
        }
    }
}
