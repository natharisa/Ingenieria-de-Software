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

        public CodigoRegistroUsuario Insertar(Usuario usuario)
        {
            if (usuario == null)
            {
                return CodigoRegistroUsuario.DatosInvalidos;
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
                    return CodigoRegistroUsuario.ErrorBaseDatos;
                }

                if (tabla.Columns.Contains("codigo_resultado"))
                {
                    return MapearResultadoRegistro(tabla.Rows[0], usuario);
                }

                usuario.Id = Convert.ToInt32(tabla.Rows[0]["id_usuario"]);
                usuario.IntentosLoginFallidos = tabla.Columns.Contains("intentos_login_fallidos")
                    ? Convert.ToInt32(tabla.Rows[0]["intentos_login_fallidos"])
                    : 0;
                return CodigoRegistroUsuario.Creado;
            }
            catch (SqlException ex)
            {
                return MapearErrorSqlRegistro(ex);
            }
            catch
            {
                return CodigoRegistroUsuario.ErrorBaseDatos;
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

        public Usuario ObtenerActivoPorIdentificador(string identificador)
        {
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                _databaseContext.CrearParametro("@identificador", identificador)
            };

            const string sql = @"
                SELECT TOP (1)
                    id_usuario,
                    id_idioma,
                    nombre_usuario,
                    email,
                    estado_usuario,
                    intentos_login_fallidos
                FROM dbo.Usuario
                WHERE (nombre_usuario = @identificador OR email = @identificador)
                  AND estado_usuario = 'ACTIVO'";

            try
            {
                _databaseContext.Abrir();
                DataTable tabla = _databaseContext.LeerTexto(sql, parametros);

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

        public int RegistrarLoginFallidoPorIdentificador(string identificador)
        {
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                _databaseContext.CrearParametro("@identificador", identificador)
            };

            const string sql = @"
                UPDATE dbo.Usuario
                SET intentos_login_fallidos = ISNULL(intentos_login_fallidos, 0) + 1,
                    estado_usuario = CASE
                        WHEN ISNULL(intentos_login_fallidos, 0) + 1 >= 3 THEN 'INACTIVO'
                        ELSE estado_usuario
                    END
                OUTPUT INSERTED.intentos_login_fallidos
                WHERE (nombre_usuario = @identificador OR email = @identificador)
                  AND estado_usuario = 'ACTIVO'";

            try
            {
                _databaseContext.Abrir();
                DataTable tabla = _databaseContext.LeerTexto(sql, parametros);

                if (tabla.Rows.Count == 0)
                {
                    return -1;
                }

                return Convert.ToInt32(tabla.Rows[0]["intentos_login_fallidos"]);
            }
            finally
            {
                _databaseContext.Cerrar();
            }
        }

        public void ReiniciarIntentosLoginFallidos(int idUsuario)
        {
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                _databaseContext.CrearParametro("@id_usuario", idUsuario)
            };

            const string sql = @"
                UPDATE dbo.Usuario
                SET intentos_login_fallidos = 0
                WHERE id_usuario = @id_usuario";

            try
            {
                _databaseContext.Abrir();
                _databaseContext.Escribir(sql, parametros);
            }
            finally
            {
                _databaseContext.Cerrar();
            }
        }

        public int Editar(Usuario usuario)
        {
            if (usuario == null || usuario.Id == 0)
            {
                return -1;
            }

            List<SqlParameter> parametros = new List<SqlParameter>
            {
                _databaseContext.CrearParametro("@id_usuario", usuario.Id),
                _databaseContext.CrearParametro("@nombre_usuario", usuario.Username),
                _databaseContext.CrearParametro("@email", usuario.Email),
                _databaseContext.CrearParametro("@estado_usuario", usuario.Estado),
                _databaseContext.CrearParametro("@password_hash", string.IsNullOrWhiteSpace(usuario.Password) ? null : usuario.Password)
            };

            const string sql = @"
                UPDATE dbo.Usuario
                SET nombre_usuario = @nombre_usuario,
                    email = @email,
                    estado_usuario = @estado_usuario,
                    password_hash = COALESCE(@password_hash, password_hash)
                WHERE id_usuario = @id_usuario";

            try
            {
                _databaseContext.Abrir();
                return _databaseContext.Escribir(sql, parametros);
            }
            finally
            {
                _databaseContext.Cerrar();
            }
        }

        public int Borrar(Usuario usuario)
        {
            if (usuario == null || usuario.Id == 0)
            {
                return -1;
            }

            List<SqlParameter> parametros = new List<SqlParameter>
            {
                _databaseContext.CrearParametro("@id_usuario", usuario.Id)
            };

            const string sql = @"
                UPDATE dbo.Usuario
                SET estado_usuario = 'INACTIVO'
                WHERE id_usuario = @id_usuario";

            try
            {
                _databaseContext.Abrir();
                return _databaseContext.Escribir(sql, parametros);
            }
            finally
            {
                _databaseContext.Cerrar();
            }
        }

        public List<Usuario> Listar()
        {
            const string sql = @"
                SELECT
                    u.id_usuario,
                    u.id_idioma,
                    u.nombre_usuario,
                    u.email,
                    u.estado_usuario,
                    u.intentos_login_fallidos
                FROM dbo.Usuario u
                ORDER BY u.nombre_usuario";

            try
            {
                _databaseContext.Abrir();
                DataTable tabla = _databaseContext.LeerTexto(sql);
                List<Usuario> usuarios = new List<Usuario>();

                foreach (DataRow registro in tabla.Rows)
                {
                    usuarios.Add(MapearUsuario(registro));
                }

                return usuarios;
            }
            finally
            {
                _databaseContext.Cerrar();
            }
        }

        private static Usuario MapearUsuario(DataRow registro)
        {
            return new Usuario
            {
                Id = Convert.ToInt32(registro["id_usuario"]),
                Username = registro["nombre_usuario"].ToString(),
                Email = registro["email"].ToString(),
                Idioma = registro["id_idioma"].ToString(),
                Estado = registro.Table.Columns.Contains("estado_usuario")
                    ? registro["estado_usuario"].ToString()
                    : null,
                IntentosLoginFallidos = registro.Table.Columns.Contains("intentos_login_fallidos")
                    ? Convert.ToInt32(registro["intentos_login_fallidos"])
                    : 0
            };
        }

        private static CodigoRegistroUsuario MapearResultadoRegistro(DataRow registro, Usuario usuario)
        {
            string codigoResultado = registro["codigo_resultado"].ToString();

            switch (codigoResultado)
            {
                case "OK":
                    usuario.Id = Convert.ToInt32(registro["id_usuario"]);
                    usuario.IntentosLoginFallidos = registro.Table.Columns.Contains("intentos_login_fallidos")
                        ? Convert.ToInt32(registro["intentos_login_fallidos"])
                        : 0;
                    return CodigoRegistroUsuario.Creado;

                case "USUARIO_EXISTENTE":
                    return CodigoRegistroUsuario.UsuarioExistente;

                case "EMAIL_EXISTENTE":
                    return CodigoRegistroUsuario.EmailExistente;

                case "IDIOMA_DEFAULT_INEXISTENTE":
                    return CodigoRegistroUsuario.IdiomaDefaultInexistente;

                default:
                    return CodigoRegistroUsuario.ErrorBaseDatos;
            }
        }

        private static CodigoRegistroUsuario MapearErrorSqlRegistro(SqlException ex)
        {
            if (ex.Number == 2601 || ex.Number == 2627)
            {
                string mensaje = ex.Message ?? string.Empty;

                if (mensaje.IndexOf("email", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return CodigoRegistroUsuario.EmailExistente;
                }

                return CodigoRegistroUsuario.UsuarioExistente;
            }

            return CodigoRegistroUsuario.ErrorBaseDatos;
        }
    }
}
