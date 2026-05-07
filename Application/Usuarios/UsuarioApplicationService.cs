using System.Collections.Generic;
using Abstractions;
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
        private readonly BitacoraFactory _loginFallidoFactory;
        private readonly BitacoraFactory _registroFallidoFactory;

        public UsuarioApplicationService()
            : this(new UsuarioRepository(), new BitacoraRepository(), new PlainTextPasswordService(), new LoginFallidoBitacoraFactory(), new RegistroFallidoBitacoraFactory())
        {
        }

        public UsuarioApplicationService(UsuarioRepository usuarioRepository, PlainTextPasswordService passwordService)
            : this(usuarioRepository, new BitacoraRepository(), passwordService, new LoginFallidoBitacoraFactory(), new RegistroFallidoBitacoraFactory())
        {
        }

        public UsuarioApplicationService(
            UsuarioRepository usuarioRepository,
            BitacoraRepository bitacoraRepository,
            PlainTextPasswordService passwordService,
            BitacoraFactory loginFallidoFactory,
            BitacoraFactory registroFallidoFactory)
        {
            _usuarioRepository = usuarioRepository;
            _bitacoraRepository = bitacoraRepository;
            _passwordService = passwordService;
            _loginFallidoFactory = loginFallidoFactory;
            _registroFallidoFactory = registroFallidoFactory;
        }

        public ResultadoOperacion<CodigoRegistroUsuario> CrearUsuario(Usuario nuevoUsuario)
        {
            if (nuevoUsuario == null ||
                string.IsNullOrWhiteSpace(nuevoUsuario.Username) ||
                string.IsNullOrWhiteSpace(nuevoUsuario.Email) ||
                string.IsNullOrWhiteSpace(nuevoUsuario.Password) ||
                string.IsNullOrWhiteSpace(nuevoUsuario.Nombre) ||
                string.IsNullOrWhiteSpace(nuevoUsuario.Apellido))
            {
                return ResultadoOperacion<CodigoRegistroUsuario>.FalloNegocio(
                    CodigoRegistroUsuario.DatosInvalidos,
                    "Completa todos los campos para registrarte.");
            }

            Usuario usuarioProtegido = new Usuario
            {
                Id = nuevoUsuario.Id,
                Username = nuevoUsuario.Username.Trim(),
                Email = nuevoUsuario.Email.Trim(),
                Password = _passwordService.Hash(nuevoUsuario.Password),
                Nombre = nuevoUsuario.Nombre.Trim(),
                Apellido = nuevoUsuario.Apellido.Trim(),
                Idioma = nuevoUsuario.Idioma
            };

            ResultadoOperacion<CodigoRegistroUsuario> resultado = _usuarioRepository.Crear(usuarioProtegido);

            if (resultado.Codigo == CodigoRegistroUsuario.UsuarioExistente)
            {
                RegistrarRegistroFallido(usuarioProtegido.Username, "Intento de registro con usuario existente.");
            }
            else if (resultado.Codigo == CodigoRegistroUsuario.EmailExistente)
            {
                RegistrarRegistroFallido(usuarioProtegido.Email, "Intento de registro con email existente.");
            }

            return resultado;
        }

        public Usuario Login(string username, string password)
        {
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

        public void Borrar(Usuario usuario)
        {
            _usuarioRepository.Borrar(usuario);
        }

        public List<Usuario> Listar()
        {
            return _usuarioRepository.Listar();
        }

        private void RegistrarLoginFallido(string username, string descripcion)
        {
            IBitacoraEvento evento = _loginFallidoFactory.Crear(NormalizarIdentificador(username), descripcion);
            _bitacoraRepository.Registrar(evento);
        }

        private void RegistrarRegistroFallido(string identificador, string descripcion)
        {
            IBitacoraEvento evento = _registroFallidoFactory.Crear(NormalizarIdentificador(identificador), descripcion);
            _bitacoraRepository.Registrar(evento);
        }

        private static string NormalizarIdentificador(string username)
        {
            return string.IsNullOrWhiteSpace(username) ? null : username.Trim();
        }
    }
}
