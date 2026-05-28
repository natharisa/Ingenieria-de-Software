using DAL;
using Domain;
using System.Collections.Generic;

namespace Repository
{
    public class BitacoraRepository
    {
        private readonly BitacoraDataMapper _bitacoraDataMapper;

        public BitacoraRepository()
            : this(new BitacoraDataMapper())
        {
        }

        public BitacoraRepository(BitacoraDataMapper bitacoraDataMapper)
        {
            _bitacoraDataMapper = bitacoraDataMapper;
        }

        public bool Registrar(IBitacoraEvento bitacora)
        {
            if (bitacora == null)
            {
                return false;
            }

            return _bitacoraDataMapper.Insertar(BitacoraRegistroMapper.Mapear(bitacora)) > 0;
        }

        public List<BitacoraRegistro> Listar()
        {
            return _bitacoraDataMapper.Listar();
        }
    }
}
