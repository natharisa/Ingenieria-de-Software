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
        private readonly BitacoraFactory _loginFallidoFactory;

        public UsuarioApplicationService()
            : this(new UsuarioRepository(), new BitacoraRepository(), new PlainTextPasswordService(), new LoginFallidoBitacoraFactory())
        {
        }

        public UsuarioApplicationService(UsuarioRepository usuarioRepository, PlainTextPasswordService passwordService)
            : this(usuarioRepository, new BitacoraRepository(), passwordService, new LoginFallidoBitacoraFactory())
        {
        }

        public UsuarioApplicationService(
            UsuarioRepository usuarioRepository,
            BitacoraRepository bitacoraRepository,
            PlainTextPasswordService passwordService,
            BitacoraFactory loginFallidoFactory)
        {
            _usuarioRepository = usuarioRepository;
            _bitacoraRepository = bitacoraRepository;
            _passwordService = passwordService;
            _loginFallidoFactory = loginFallidoFactory;
        }

        public bool CrearUsuario(Usuario nuevoUsuario)
        {
            if (nuevoUsuario == null ||
                string.IsNullOrWhiteSpace(nuevoUsuario.Username) ||
                string.IsNullOrWhiteSpace(nuevoUsuario.Email) ||
                string.IsNullOrWhiteSpace(nuevoUsuario.Password) ||
                string.IsNullOrWhiteSpace(nuevoUsuario.Nombre) ||
                string.IsNullOrWhiteSpace(nuevoUsuario.Apellido))
            {
                return false;
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

            return _usuarioRepository.Crear(usuarioProtegido);
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

        private static string NormalizarIdentificador(string username)
        {
            return string.IsNullOrWhiteSpace(username) ? null : username.Trim();
        }
    }
}
