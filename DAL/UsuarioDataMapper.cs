using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Domain;

namespace DAL
{
    public class UsuarioDataMapper
    {
        private readonly DatabaseContext _databaseContext;

        public UsuarioDataMapper()
            : this(new DatabaseContext())
        {
        }

        public UsuarioDataMapper(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public int Insertar(Usuario usuario)
        {
            _databaseContext.Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                _databaseContext.CrearParametro("@NOMBRE", usuario.Nombre)
            };
            int resultado = _databaseContext.Escribir("INSERTAR_CLIENTE", parametros);
            _databaseContext.Cerrar();
            return resultado;
        }

        public int Editar(Usuario usuario)
        {
            _databaseContext.Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                _databaseContext.CrearParametro("@ID", usuario.Id),
                _databaseContext.CrearParametro("@NOMBRE", usuario.Nombre)
            };
            int resultado = _databaseContext.Escribir("EDITAR_CLIENTE", parametros);
            _databaseContext.Cerrar();
            return resultado;
        }

        public int Borrar(Usuario usuario)
        {
            _databaseContext.Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                _databaseContext.CrearParametro("@ID", usuario.Id)
            };
            int resultado = _databaseContext.Escribir("BORRAR_CLIENTE", parametros);
            _databaseContext.Cerrar();
            return resultado;
        }

        public List<Usuario> Listar()
        {
            List<Usuario> usuarios = new List<Usuario>();

            _databaseContext.Abrir();
            DataTable tabla = _databaseContext.Leer("LISTAR_CLIENTE");
            _databaseContext.Cerrar();

            foreach (DataRow registro in tabla.Rows)
            {
                Usuario usuario = new Usuario
                {
                    Id = int.Parse(registro["ID"].ToString()),
                    Nombre = registro["NOMBRE"].ToString()
                };

                usuarios.Add(usuario);
            }

            return usuarios;
        }
    }
}
