using BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class MpUsuario: Mapper<Usuario>
    {
        public override int Borrar(Usuario objeto)
        {
            dao = new DAO();
            dao.Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(dao.CrearParametro("Id aqui",objeto.Id));
            int res = dao.Escribir("stores procedure borrar", parametros);
            dao.Cerrar();
            return res;
        }

        public override int Editar(Usuario objeto)
        {
            dao = new DAO();
            dao.Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(dao.CrearParametro("@ID", objeto.Id));
            parametros.Add(dao.CrearParametro("@NOMBRE", objeto.Nombre));
            int res = dao.Escribir("EDITAR_CLIENTE", parametros);
            dao.Cerrar();
            return res;
        }

        public override int Insertar(Usuario objeto)
        {
            dao = new DAO();
            dao.Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(dao.CrearParametro("@NOMBRE", objeto.Nombre));
            int res = dao.Escribir("INSERTAR_CLIENTE", parametros);
            dao.Cerrar();
            return res;
        }

        public override List<Usuario> Listar()
        {
            List<Usuario> clientes = new List<Usuario>();
            dao = new DAO();
            dao.Abrir();
            DataTable tabla = dao.Leer("LISTAR_CLIENTE");
            dao.Cerrar();

            foreach (DataRow registro in tabla.Rows)
            {
                Usuario usuario = new Usuario();
                usuario.Id = int.Parse(registro["ID"].ToString());
                usuario.Nombre = registro["NOMBRE"].ToString();
                clientes.Add(usuario);

            }

            return clientes;
        }
    }
}
