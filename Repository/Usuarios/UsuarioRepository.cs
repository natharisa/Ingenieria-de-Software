using System.Collections.Generic;
using DAL;
using Domain;

namespace Repository
{
    public class UsuarioRepository
    {
        private readonly UsuarioDataMapper _usuarioDataMapper;

        public UsuarioRepository()
            : this(new UsuarioDataMapper())
        {
        }

        public UsuarioRepository(UsuarioDataMapper usuarioDataMapper)
        {
            _usuarioDataMapper = usuarioDataMapper;
        }

        public CodigoRegistroUsuario Crear(Usuario usuario)
        {
            if (usuario == null)
            {
                return CodigoRegistroUsuario.DatosInvalidos;
            }

            return _usuarioDataMapper.Insertar(usuario);
        }

        public Usuario ObtenerPorCredenciales(string username, string password)
        {
            return _usuarioDataMapper.ObtenerPorCredenciales(username, password);
        }

        public bool Existe(string username)
        {
            return _usuarioDataMapper.ExistePorNombreUsuario(username);
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

    }
}
