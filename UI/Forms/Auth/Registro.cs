using System;
using System.Windows.Forms;
using Abstractions;
using Application;
using Domain;

namespace UI
{
    public partial class Registro : Form
    {
        private readonly UsuarioApplicationService _usuarioService;

        public string UsuarioRegistrado { get; private set; }

        public Registro(UsuarioApplicationService usuarioService)
        {
            _usuarioService = usuarioService;
            InitializeComponent();
        }

        private void btnCrearCuenta_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUser.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
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
                Email = txtEmail.Text.Trim(),
                Password = txtPass.Text,
                Nombre = txtNombre.Text.Trim(),
                Apellido = txtApellido.Text.Trim()
            };

            ResultadoOperacion<CodigoRegistroUsuario> resultado = _usuarioService.CrearUsuario(nuevo);

            if (resultado.Exitoso)
            {
                UsuarioRegistrado = nuevo.Username;
                MessageBox.Show("Usuario registrado con exito.");
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show(ObtenerMensajeRegistro(resultado));
            }
        }

        private static string ObtenerMensajeRegistro(ResultadoOperacion<CodigoRegistroUsuario> resultado)
        {
            switch (resultado.Codigo)
            {
                case CodigoRegistroUsuario.DatosInvalidos:
                    return "Completa todos los campos para registrarte.";

                case CodigoRegistroUsuario.UsuarioExistente:
                    return "Ya existe un usuario con ese nombre.";

                case CodigoRegistroUsuario.EmailExistente:
                    return "Ya existe un usuario con ese email.";

                case CodigoRegistroUsuario.IdiomaDefaultInexistente:
                    return "No se pudo registrar el usuario porque falta el idioma default.";

                default:
                    return resultado.Estado == ResultadoEstado.ErrorTecnico
                        ? "Ocurrio un error tecnico al registrar el usuario."
                        : "No se pudo registrar el usuario.";
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
