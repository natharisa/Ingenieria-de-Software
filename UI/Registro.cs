using BE;
using BLL;
using System;
using System.Windows.Forms;

namespace UI
{
    public partial class Registro : Form
    {
        private readonly UsuarioBLL bllUsuario = new UsuarioBLL();

        public string UsuarioRegistrado { get; private set; }

        public Registro()
        {
            InitializeComponent();
        }

        private void btnCrearCuenta_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUser.Text) ||
                string.IsNullOrWhiteSpace(txtPass.Text) ||
                string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtApellido.Text))
            {
                MessageBox.Show("Completa todos los campos para registrarte.");
                return;
            }

            Usuario nuevo = new Usuario
            {
                Username = txtUser.Text.Trim(),
                Password = txtPass.Text,
                Nombre = txtNombre.Text.Trim(),
                Apellido = txtApellido.Text.Trim()
            };

            if (bllUsuario.CrearUsuario(nuevo))
            {
                UsuarioRegistrado = nuevo.Username;
                MessageBox.Show("Usuario registrado con exito.");
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("No se pudo registrar el usuario.");
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
