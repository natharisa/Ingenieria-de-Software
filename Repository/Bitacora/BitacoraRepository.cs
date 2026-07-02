using DAL;
using Domain;
using System.Collections.Generic;
using System;
using System.Web.Script.Serialization;

namespace Repository
{
    public class BitacoraRepository
    {
        private readonly BitacoraDataMapper _bitacoraDataMapper;
        private readonly AuditoriaRepository _auditoriaRepository;
        private readonly JavaScriptSerializer _serializer;

        public BitacoraRepository()
            : this(new BitacoraDataMapper(), new AuditoriaRepository())
        {
        }

        public BitacoraRepository(BitacoraDataMapper bitacoraDataMapper)
            : this(bitacoraDataMapper, new AuditoriaRepository())
        {
        }

        public BitacoraRepository(BitacoraDataMapper bitacoraDataMapper, AuditoriaRepository auditoriaRepository)
        {
            _bitacoraDataMapper = bitacoraDataMapper;
            _auditoriaRepository = auditoriaRepository;
            _serializer = new JavaScriptSerializer();
        }

        public bool Registrar(IBitacoraEvento bitacora)
        {
            if (bitacora == null)
            {
                return false;
            }

            BitacoraRegistro registro = BitacoraRegistroMapper.Mapear(bitacora);
            bool registrado = _bitacoraDataMapper.Insertar(registro) > 0;

            if (registrado)
            {
                RegistrarAuditoria(registro);
            }

            return registrado;
        }

        public List<BitacoraRegistro> Listar()
        {
            return _bitacoraDataMapper.Listar();
        }

        private void RegistrarAuditoria(BitacoraRegistro registro)
        {
            AuditoriaMemento estadoNuevo = registro.SaveToMemento();
            List<AuditoriaCambio> cambios = new List<AuditoriaCambio>();

            foreach (KeyValuePair<string, object> item in estadoNuevo.Estado)
            {
                cambios.Add(new AuditoriaCambio
                {
                    Campo = item.Key,
                    ValorAnterior = null,
                    ValorNuevo = item.Value
                });
            }

            _auditoriaRepository.Registrar(new AuditoriaRegistro
            {
                Entidad = estadoNuevo.Entidad,
                IdEntidad = estadoNuevo.IdEntidad,
                Accion = "CREATE",
                IdUsuarioActor = registro.IdUsuario,
                IdentificadorUsuarioActor = registro.IdentificadorUsuario,
                FechaEvento = DateTime.Now,
                EstadoAnteriorJson = _serializer.Serialize(new Dictionary<string, object>()),
                EstadoNuevoJson = _serializer.Serialize(estadoNuevo.Estado),
                CambiosJson = _serializer.Serialize(cambios)
            });
        }
    }
}
