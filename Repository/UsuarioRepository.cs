using System.Collections.Generic;
using System.Linq;
using DAL;
using Domain;

namespace Repository
{
    public class UsuarioRepository
    {
        private static readonly List<Usuario> UsuariosEnMemoria = new List<Usuario>();
        private readonly UsuarioDataMapper _usuarioDataMapper;

        public UsuarioRepository()
            : this(new UsuarioDataMapper())
        {
        }

        public UsuarioRepository(UsuarioDataMapper usuarioDataMapper)
        {
            _usuarioDataMapper = usuarioDataMapper;
        }

        public bool Crear(Usuario usuario)
        {
            if (usuario == null || Existe(usuario.Username))
            {
                return false;
            }

            UsuariosEnMemoria.Add(Clonar(usuario));
            return true;
        }

        public Usuario ObtenerPorCredenciales(string username, string password)
        {
            Usuario usuario = UsuariosEnMemoria.FirstOrDefault(
                u => u.Username == username && u.Password == password);

            return Clonar(usuario);
        }

        public bool Existe(string username)
        {
            return UsuariosEnMemoria.Any(u => u.Username == username);
        }

        public void Guardar(Usuario usuario)
        {
            if (usuario == null)
            {
                return;
            }

            if (usuario.Id == 0)
            {
                _usuarioDataMapper.Insertar(usuario);
            }
            else
            {
                _usuarioDataMapper.Editar(usuario);
            }
        }

        public void Borrar(Usuario usuario)
        {
            if (usuario == null)
            {
                return;
            }

            _usuarioDataMapper.Borrar(usuario);
        }

        public List<Usuario> Listar()
        {
            return _usuarioDataMapper.Listar();
        }

        private static Usuario Clonar(Usuario usuario)
        {
            if (usuario == null)
            {
                return null;
            }

            return new Usuario
            {
                Id = usuario.Id,
                Username = usuario.Username,
                Password = usuario.Password,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Idioma = usuario.Idioma
            };
        }
    }
}
