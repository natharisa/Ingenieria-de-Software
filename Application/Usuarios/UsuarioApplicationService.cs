using System.Collections.Generic;
using Domain;
using Repository;
using Services;

namespace Application
{
    public class UsuarioApplicationService
    {
        private readonly UsuarioRepository _usuarioRepository;
        private readonly BitacoraRepository _bitacoraRepository;
        private readonly PlainTextPasswordService _passwordService;
        private BitacoraFactory _bitacoraFactory;

        public UsuarioApplicationService()
            : this(new UsuarioRepository(), new BitacoraRepository(), new PlainTextPasswordService())
        {
        }

        public UsuarioApplicationService(
            UsuarioRepository usuarioRepository,
            BitacoraRepository bitacoraRepository,
            PlainTextPasswordService passwordService)
        {
            _usuarioRepository = usuarioRepository;
            _bitacoraRepository = bitacoraRepository;
            _passwordService = passwordService;
        }

        public CodigoRegistroUsuario CrearUsuario(Usuario nuevoUsuario)
        {
            if (nuevoUsuario == null ||
                string.IsNullOrWhiteSpace(nuevoUsuario.Username) ||
                string.IsNullOrWhiteSpace(nuevoUsuario.Email) ||
                string.IsNullOrWhiteSpace(nuevoUsuario.Password))
            {
                return CodigoRegistroUsuario.DatosInvalidos;
            }


            Usuario usuarioProtegido = new Usuario
            {
                Id = nuevoUsuario.Id,
                Username = nuevoUsuario.Username.Trim(),
                Email = nuevoUsuario.Email.Trim(),
                Password = _passwordService.Hash(nuevoUsuario.Password),
                Nombre = string.IsNullOrWhiteSpace(nuevoUsuario.Nombre) ? null : nuevoUsuario.Nombre.Trim(),
                Apellido = string.IsNullOrWhiteSpace(nuevoUsuario.Apellido) ? null : nuevoUsuario.Apellido.Trim(),
                Idioma = nuevoUsuario.Idioma,
                Estado = "ACTIVO"
            };

            CodigoRegistroUsuario resultado = _usuarioRepository.Crear(usuarioProtegido);
            this._bitacoraFactory = new RegistroFallidoBitacoraFactory();

            if (resultado == CodigoRegistroUsuario.UsuarioExistente)
            {
                RegistrarRegistroFallido(usuarioProtegido.Username, "Intento de registro con usuario existente.");
            }
            else if (resultado == CodigoRegistroUsuario.EmailExistente)
            {
                RegistrarRegistroFallido(usuarioProtegido.Email, "Intento de registro con email existente.");
            }

            return resultado;
        }

        public Usuario Login(string username, string password)
        {
            this._bitacoraFactory = new LoginFallidoBitacoraFactory();
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                RegistrarLoginFallido(username, "Intento de login con usuario o contrasena vacios.");
                return null;
            }

            string passwordProtegida = _passwordService.Hash(password);
            Usuario usuario = _usuarioRepository.ObtenerPorCredenciales(username.Trim(), passwordProtegida);

            if (usuario == null)
            {
                RegistrarLoginFallido(username, "Intento de login con credenciales invalidas.");
            }

            return usuario;
        }

        public bool ExisteUsuario(string username)
        {
            return _usuarioRepository.Existe(username);
        }

        public void Grabar(Usuario usuario)
        {
            _usuarioRepository.Guardar(usuario);
        }

        public bool ModificarUsuario(Usuario usuario)
        {
            if (usuario == null ||
                usuario.Id == 0 ||
                string.IsNullOrWhiteSpace(usuario.Username) ||
                string.IsNullOrWhiteSpace(usuario.Email))
            {
                return false;
            }

            Usuario usuarioNormalizado = new Usuario
            {
                Id = usuario.Id,
                Username = usuario.Username.Trim(),
                Email = usuario.Email.Trim(),
                Password = string.IsNullOrWhiteSpace(usuario.Password) ? null : _passwordService.Hash(usuario.Password),
                Nombre = string.IsNullOrWhiteSpace(usuario.Nombre) ? null : usuario.Nombre.Trim(),
                Apellido = string.IsNullOrWhiteSpace(usuario.Apellido) ? null : usuario.Apellido.Trim(),
                Idioma = usuario.Idioma,
                Estado = string.IsNullOrWhiteSpace(usuario.Estado) ? "ACTIVO" : usuario.Estado.Trim()
            };

            return _usuarioRepository.Modificar(usuarioNormalizado);
        }

        public void Borrar(Usuario usuario)
        {
            _usuarioRepository.Borrar(usuario);
        }

        public bool InhabilitarUsuario(Usuario usuario)
        {
            if (usuario == null || usuario.Id == 0)
            {
                return false;
            }

            return _usuarioRepository.Inhabilitar(usuario);
        }

        public List<Usuario> Listar()
        {
            return _usuarioRepository.Listar();
        }

        private void RegistrarLoginFallido(string username, string descripcion)
        {
            IBitacoraEvento evento = _bitacoraFactory.Crear(NormalizarIdentificador(username), descripcion);
            _bitacoraRepository.Registrar(evento);
        }

        private void RegistrarRegistroFallido(string identificador, string descripcion)
        {
            IBitacoraEvento evento = _bitacoraFactory.Crear(NormalizarIdentificador(identificador), descripcion);
            _bitacoraRepository.Registrar(evento);
        }

        private static string NormalizarIdentificador(string username)
        {
            return string.IsNullOrWhiteSpace(username) ? null : username.Trim();
        }
    }
}
