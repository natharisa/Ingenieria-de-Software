using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DAO
    {
        //De esta forma nos enseño Parkinson (revisar jeje)

        #region Variables

        public SqlConnection conexion;
        public SqlTransaction transaccion;

        #endregion

        #region Conexion

        public void Abrir()
        {
            conexion= new SqlConnection();
            conexion.ConnectionString = "Aquí la base de datos";
            conexion.Open();

        }

        public void Cerrar()
        {
            conexion.Close();
            conexion = null;
        }

        #endregion

        #region Transaccion

        public void IniciarTx()
        {
            transaccion = conexion.BeginTransaction();
        }

        public void Confirmar()
        {
            transaccion.Commit();
            transaccion = null;
        }

        public void Deshacer()
        {
            transaccion.Rollback();
            transaccion = null;
        }

        #endregion

        #region CrearComando,Leer,Escribir

        private SqlCommand CrearComando(string sql, List<SqlParameter> parametros = null)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.StoredProcedure;

            if(transaccion != null )
            {
                cmd.Transaction = transaccion;

            }
            else
            {
                cmd.Parameters.AddRange(parametros.ToArray());

            }
            return cmd;
        }
        
        public int Escribir(string sql, List<SqlParameter> parametros = null)
        {
            SqlCommand cmd = CrearComando(sql,parametros);
            int fila = 0;
            try
            {
                fila = cmd.ExecuteNonQuery();
            }
            catch
            {
                fila = -1;
            }
            return fila;
        }

        public DataTable Leer(string sql, List<SqlParameter> parametros = null)
        {
            SqlDataAdapter adaptador = new SqlDataAdapter();
            DataTable tabla = new DataTable();
            adaptador.SelectCommand=CrearComando(sql,parametros);

            adaptador.Fill(tabla);

            return tabla;
        }

        #endregion

        #region Parametros


        public SqlParameter CrearParametro(string nombre, string valor)
        {
            SqlParameter p = new SqlParameter();
            p.ParameterName = nombre;
            p.Value = valor;
            p.DbType = DbType.String;
            return p;

        }
        public SqlParameter CrearParametro(string nombre, int valor)
        {
            SqlParameter p = new SqlParameter();
            p.ParameterName = nombre;
            p.Value = valor;
            p.DbType = DbType.Int32;
            return p;

        }


        #endregion

    }
}
