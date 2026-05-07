using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Domain;

namespace DAL
{
    public class BitacoraDataMapper
    {
        private readonly DatabaseContext _databaseContext;

        public BitacoraDataMapper()
            : this(new DatabaseContext())
        {
        }

        public BitacoraDataMapper(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public int Insertar(BitacoraRegistro bitacora)
        {
            if (bitacora == null)
            {
                return -1;
            }

            List<SqlParameter> parametros = new List<SqlParameter>
            {
                CrearParametro("@id_usuario", bitacora.IdUsuario),
                CrearParametro("@identificador_usuario", bitacora.IdentificadorUsuario),
                CrearParametro("@modulo", bitacora.Modulo),
                CrearParametro("@accion", bitacora.Accion),
                CrearParametro("@nivel", bitacora.Nivel),
                CrearParametro("@descripcion", bitacora.Descripcion),
                CrearParametro("@equipo", bitacora.Equipo),
                CrearParametro("@fecha_evento", bitacora.Fecha)
            };

            try
            {
                _databaseContext.Abrir();
                DataTable tabla = _databaseContext.Leer("sp_Bitacora_Registrar", parametros);

                if (tabla.Rows.Count == 0)
                {
                    return -1;
                }

                bitacora.Id = Convert.ToInt32(tabla.Rows[0]["id_bitacora"]);
                return bitacora.Id;
            }
            catch
            {
                return -1;
            }
            finally
            {
                _databaseContext.Cerrar();
            }
        }

        private static SqlParameter CrearParametro(string nombre, string valor)
        {
            return new SqlParameter
            {
                ParameterName = nombre,
                Value = string.IsNullOrWhiteSpace(valor) ? (object)DBNull.Value : valor,
                DbType = DbType.String
            };
        }

        private static SqlParameter CrearParametro(string nombre, int? valor)
        {
            return new SqlParameter
            {
                ParameterName = nombre,
                Value = valor.HasValue ? (object)valor.Value : DBNull.Value,
                DbType = DbType.Int32
            };
        }

        private static SqlParameter CrearParametro(string nombre, DateTime valor)
        {
            return new SqlParameter
            {
                ParameterName = nombre,
                Value = valor,
                DbType = DbType.DateTime
            };
        }
    }
}
