using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Abstractions;
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

        public ResultadoOperacion<CodigoRegistroUsuario> Insertar(Usuario usuario)
        {
            if (usuario == null)
            {
                return ResultadoOperacion<CodigoRegistroUsuario>.FalloNegocio(
                    CodigoRegistroUsuario.DatosInvalidos,
                    "Los datos del usuario son invalidos.");
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
                    return ResultadoOperacion<CodigoRegistroUsuario>.ErrorTecnico(
                        CodigoRegistroUsuario.ErrorBaseDatos,
                        "El registro de usuario no devolvio resultado.");
                }

                if (tabla.Columns.Contains("codigo_resultado"))
                {
                    return MapearResultadoRegistro(tabla.Rows[0], usuario);
                }

                usuario.Id = Convert.ToInt32(tabla.Rows[0]["id_usuario"]);
                return ResultadoOperacion<CodigoRegistroUsuario>.Ok(
                    CodigoRegistroUsuario.Creado,
                    "Usuario registrado con exito.");
            }
            catch (SqlException ex)
            {
                return MapearErrorSqlRegistro(ex);
            }
            catch
            {
                return ResultadoOperacion<CodigoRegistroUsuario>.ErrorTecnico(
                    CodigoRegistroUsuario.ErrorBaseDatos,
                    "No se pudo registrar el usuario.");
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

        private static ResultadoOperacion<CodigoRegistroUsuario> MapearResultadoRegistro(DataRow registro, Usuario usuario)
        {
            string codigoResultado = registro["codigo_resultado"].ToString();
            string mensaje = registro.Table.Columns.Contains("mensaje") ? registro["mensaje"].ToString() : null;

            switch (codigoResultado)
            {
                case "OK":
                    usuario.Id = Convert.ToInt32(registro["id_usuario"]);
                    return ResultadoOperacion<CodigoRegistroUsuario>.Ok(
                        CodigoRegistroUsuario.Creado,
                        mensaje);

                case "USUARIO_EXISTENTE":
                    return ResultadoOperacion<CodigoRegistroUsuario>.FalloNegocio(
                        CodigoRegistroUsuario.UsuarioExistente,
                        mensaje);

                case "EMAIL_EXISTENTE":
                    return ResultadoOperacion<CodigoRegistroUsuario>.FalloNegocio(
                        CodigoRegistroUsuario.EmailExistente,
                        mensaje);

                case "IDIOMA_DEFAULT_INEXISTENTE":
                    return ResultadoOperacion<CodigoRegistroUsuario>.FalloNegocio(
                        CodigoRegistroUsuario.IdiomaDefaultInexistente,
                        mensaje);

                default:
                    return ResultadoOperacion<CodigoRegistroUsuario>.ErrorTecnico(
                        CodigoRegistroUsuario.ErrorBaseDatos,
                        mensaje);
            }
        }

        private static ResultadoOperacion<CodigoRegistroUsuario> MapearErrorSqlRegistro(SqlException ex)
        {
            if (ex.Number == 2601 || ex.Number == 2627)
            {
                string mensaje = ex.Message ?? string.Empty;

                if (mensaje.IndexOf("email", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return ResultadoOperacion<CodigoRegistroUsuario>.FalloNegocio(
                        CodigoRegistroUsuario.EmailExistente,
                        "Ya existe un usuario con ese email.");
                }

                return ResultadoOperacion<CodigoRegistroUsuario>.FalloNegocio(
                    CodigoRegistroUsuario.UsuarioExistente,
                    "Ya existe un usuario con ese nombre de usuario.");
            }

            return ResultadoOperacion<CodigoRegistroUsuario>.ErrorTecnico(
                CodigoRegistroUsuario.ErrorBaseDatos,
                "No se pudo registrar el usuario.");
        }
    }
}
