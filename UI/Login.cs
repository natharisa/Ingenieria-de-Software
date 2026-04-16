using BE;
using BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        UsuarioBLL bllUsuario = new UsuarioBLL(); //revisar
        private void btnLogin_Click(object sender, EventArgs e)
        {

            var usuarioValildado = bllUsuario.Login(txtUser.Text, txtPass.Text);

            if(usuarioValildado != null )
            {
                //Guardamos en el singleton
                Sesion.GetInstance().Usuario = usuarioValildado;

                MessageBox.Show($"¡Bienvenido {Sesion.GetInstance().Usuario.ToString()}!");

                //Envia al formulario de login como terminó la operación
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                if(bllUsuario.ExisteUsuario(txtUser.Text))
                {

                 MessageBox.Show("La contraseña es incorrecta");
                }
                else
                {

                MessageBox.Show("El usuario no se encuentra registrado.");
                }
            }



            //var usuarioValidado = bllUsuario.Login(txtUser.Text, txtPass.Text);

            //if (usuarioValidado != null)
            //{
            //    // Seteamos el Singleton
            //    Sesion.GetInstance().Usuario = usuarioValidado;
            //    Sesion.GetInstance().FechaInicio = DateTime.Now;

            //    this.DialogResult = DialogResult.OK; // Cerramos login y abrimos Main
            //}
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            Usuario nuevo = new Usuario
            {
                Username = txtUser.Text,
                Password = txtPass.Text,
                Nombre = Nombretxt.Text,
                Apellido = Apellidotxt.Text
            

            };

            if(bllUsuario.CrearUsuario(nuevo))
            {
                MessageBox.Show("¡Usuario registrado con éxito!");
                
            }
            
        }
    }
}
