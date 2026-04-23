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
        private readonly UsuarioBLL bllUsuario = new UsuarioBLL();

        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            var usuarioValildado = bllUsuario.Login(txtUser.Text, txtPass.Text);

            if (usuarioValildado != null)
            {
                Sesion.GetInstance().Usuario = usuarioValildado;
                MessageBox.Show($"¡Bienvenido {Sesion.GetInstance().Usuario.ToString()}!");
                this.DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                if (bllUsuario.ExisteUsuario(txtUser.Text))
                {
                    MessageBox.Show("La contraseña es incorrecta");
                }
                else
                {
                    MessageBox.Show("El usuario no se encuentra registrado.");
                }
            }
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            using (var registro = new Registro())
            {
                if (registro.ShowDialog(this) == DialogResult.OK)
                {
                    txtUser.Text = registro.UsuarioRegistrado;
                    txtPass.Clear();
                    txtPass.Focus();
                }
            }
        }
    }
}
