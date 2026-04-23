using System.Collections.Generic;
using Domain;
using Repository;
using Services;

namespace Application
{
    public class UsuarioApplicationService
    {
        private readonly UsuarioRepository _usuarioRepository;
        private readonly PlainTextPasswordService _passwordService;

        public UsuarioApplicationService()
            : this(new UsuarioRepository(), new PlainTextPasswordService())
        {
        }

        public UsuarioApplicationService(UsuarioRepository usuarioRepository, PlainTextPasswordService passwordService)
        {
            _usuarioRepository = usuarioRepository;
            _passwordService = passwordService;
        }

        public bool CrearUsuario(Usuario nuevoUsuario)
        {
            if (nuevoUsuario == null ||
                string.IsNullOrWhiteSpace(nuevoUsuario.Username) ||
                string.IsNullOrWhiteSpace(nuevoUsuario.Password) ||
                string.IsNullOrWhiteSpace(nuevoUsuario.Nombre) ||
                string.IsNullOrWhiteSpace(nuevoUsuario.Apellido))
            {
                return false;
            }

            if (_usuarioRepository.Existe(nuevoUsuario.Username))
            {
                return false;
            }

            Usuario usuarioProtegido = new Usuario
            {
                Id = nuevoUsuario.Id,
                Username = nuevoUsuario.Username.Trim(),
                Password = _passwordService.Protect(nuevoUsuario.Password),
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
                return null;
            }

            string passwordProtegida = _passwordService.Protect(password);
            return _usuarioRepository.ObtenerPorCredenciales(username.Trim(), passwordProtegida);
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
    }
}
