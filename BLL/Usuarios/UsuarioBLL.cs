using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class UsuarioBLL
    {
        private static List<Usuario> _usuarioMemoria = new List<Usuario>();

        public bool CrearUsuario( Usuario nuevoUsuario )
        {
            _usuarioMemoria.Add( nuevoUsuario );
            return true;
        }
        public Usuario Login(string user,string pass)
        {
            return _usuarioMemoria.FirstOrDefault(u => u.Username == user && u.Password == pass);
        
        }

        public bool ExisteUsuario(string nombreUsuario)
        {
            return _usuarioMemoria.Any(u => u.Username == nombreUsuario);
        }

        #region ABM

        MpUsuario mp = new MpUsuario();

        public void Grabar(Usuario usuario)
        {
            if(usuario.Id == 0)
            {
                mp.Insertar(usuario);
            }
            else
            {
                mp.Editar(usuario);
            }

        }

        public void Borrar(Usuario usuario)
        {
            mp.Borrar(usuario);
        }

        public List<Usuario> Listar()
        {
            return mp .Listar();
        }

        #endregion
    }
}
