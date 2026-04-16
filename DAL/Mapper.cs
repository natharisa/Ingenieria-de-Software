using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public abstract class Mapper<T>
    {
        internal DAO dao;
        public abstract int Insertar (T objeto);
        public abstract int Borrar (T objeto);
        public abstract int Editar (T objeto);
        public abstract List<T> Listar();


    }
}
