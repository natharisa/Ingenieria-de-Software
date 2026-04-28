using System;
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
            if (usuario == null)
            {
                return -1;
            }

            SqlParameter idUsuarioNuevo = new SqlParameter("@id_usuario_nuevo", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            List<SqlParameter> parametros = new List<SqlParameter>
            {
                _databaseContext.CrearParametro("@nombre_usuario", usuario.Username),
                _databaseContext.CrearParametro("@email", usuario.Email),
                _databaseContext.CrearParametro("@password_hash", usuario.Password),
                idUsuarioNuevo
            };

            try
            {
                _databaseContext.Abrir();
                DataTable tabla = _databaseContext.Leer("sp_Usuario_Registrar", parametros);

                if (tabla.Rows.Count == 0)
                {
                    return -1;
                }

                usuario.Id = Convert.ToInt32(tabla.Rows[0]["id_usuario"]);
                return usuario.Id;
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

        public Usuario ObtenerPorCredenciales(string identificador, string passwordHash)
        {
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                _databaseContext.CrearParametro("@identificador", identificador),
                _databaseContext.CrearParametro("@password_hash", passwordHash)
            };

            try
            {
                _databaseContext.Abrir();
                DataTable tabla = _databaseContext.Leer("sp_Usuario_Login", parametros);

                if (tabla.Rows.Count == 0)
                {
                    return null;
                }

                return MapearUsuario(tabla.Rows[0]);
            }
            finally
            {
                _databaseContext.Cerrar();
            }
        }

        public bool ExistePorNombreUsuario(string username)
        {
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                _databaseContext.CrearParametro("@nombre_usuario", username)
            };

            const string sql = @"
                SELECT TOP (1) id_usuario
                FROM dbo.Usuario
                WHERE nombre_usuario = @nombre_usuario";

            try
            {
                _databaseContext.Abrir();
                DataTable tabla = _databaseContext.LeerTexto(sql, parametros);
                return tabla.Rows.Count > 0;
            }
            finally
            {
                _databaseContext.Cerrar();
            }
        }

        public int Editar(Usuario usuario)
        {
            return -1;
        }

        public int Borrar(Usuario usuario)
        {
            return -1;
        }

        public List<Usuario> Listar()
        {
            return new List<Usuario>();
        }

        private static Usuario MapearUsuario(DataRow registro)
        {
            return new Usuario
            {
                Id = Convert.ToInt32(registro["id_usuario"]),
                Username = registro["nombre_usuario"].ToString(),
                Email = registro["email"].ToString(),
                Idioma = registro["id_idioma"].ToString()
            };
        }
    }
}
