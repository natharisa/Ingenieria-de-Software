using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Application;
using Domain;
using Services;

namespace UI
{
    public partial class UsuariosView : LocalizedUserControl
    {
        private readonly UsuarioApplicationService _usuarioService;
        private List<Usuario> _usuarios;
        private Usuario _usuarioSeleccionado;
        private bool _preparandoFormulario;

        public UsuariosView()
            : this(new UsuarioApplicationService())
        {
        }

        public UsuariosView(UsuarioApplicationService usuarioService)
        {
            _usuarioService = usuarioService;
            InitializeComponent();
            ConfigurarTraducciones();
            ConfigurarGrilla();
            CargarUsuarios();
            PrepararNuevoUsuario();
        }

        private void ConfigurarTraducciones()
        {
            lblTitulo.Tag = "USERS_TITLE";
            lblDescripcion.Tag = "USERS_DESCRIPTION";
            groupBoxDetalle.Tag = "USERS_DETAIL";
            lblUsuario.Tag = "FIELD_USER";
            lblEmail.Tag = "FIELD_EMAIL";
            lblNombre.Tag = "FIELD_NAME";
            lblApellido.Tag = "FIELD_LASTNAME";
            lblPassword.Tag = "FIELD_NEW_PASSWORD";
            lblEstado.Tag = "FIELD_STATUS";
            btnNuevo.Tag = "BTN_NEW";
            btnInhabilitar.Tag = "BTN_DISABLE";
            columnId.Tag = "GRID_ID";
            columnUsuario.Tag = "GRID_USER";
            columnEmail.Tag = "GRID_EMAIL";
            columnEstado.Tag = "GRID_STATUS";
        }

        private void ConfigurarGrilla()
        {
            dgvUsuarios.AutoGenerateColumns = false;
            cmbEstado.Items.AddRange(new object[] { "ACTIVO", "INACTIVO" });
        }

        private void CargarUsuarios()
        {
            _usuarios = _usuarioService.Listar();
            dgvUsuarios.DataSource = null;
            dgvUsuarios.DataSource = _usuarios;
        }

        private void dgvUsuarios_SelectionChanged(object sender, EventArgs e)
        {
            if (_preparandoFormulario)
            {
                return;
            }

            if (dgvUsuarios.CurrentRow == null)
            {
                return;
            }

            Usuario usuario = dgvUsuarios.CurrentRow.DataBoundItem as Usuario;
            if (usuario == null)
            {
                return;
            }

            _usuarioSeleccionado = usuario;
            txtUsuario.Text = usuario.Username;
            txtEmail.Text = usuario.Email;
            txtNombre.Text = usuario.Nombre;
            txtApellido.Text = usuario.Apellido;
            txtPassword.Text = string.Empty;
            cmbEstado.SelectedItem = string.IsNullOrWhiteSpace(usuario.Estado) ? "ACTIVO" : usuario.Estado;
            lblModo.Tag = "USERS_EDIT_MODE";
            lblModo.Text = LanguageManager.Instance.Translate("USERS_EDIT_MODE");
            btnGuardar.Tag = "BTN_SAVE";
            btnGuardar.Text = LanguageManager.Instance.Translate("BTN_SAVE");
            btnInhabilitar.Enabled = usuario.Id > 0 && usuario.Estado != "INACTIVO";
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            PrepararNuevoUsuario();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (_usuarioSeleccionado == null)
            {
                CrearUsuario();
                return;
            }

            ModificarUsuario();
        }

        private void btnInhabilitar_Click(object sender, EventArgs e)
        {
            if (_usuarioSeleccionado == null)
            {
                MessageBox.Show("Selecciona un usuario para inhabilitar.");
                return;
            }

            DialogResult confirmacion = MessageBox.Show(
                "El usuario no podra iniciar sesion mientras este inactivo. Deseas continuar?",
                "Inhabilitar usuario",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmacion != DialogResult.Yes)
            {
                return;
            }

            bool resultado = _usuarioService.InhabilitarUsuario(_usuarioSeleccionado);
            MessageBox.Show(resultado ? "Usuario inhabilitado correctamente." : "No se pudo inhabilitar el usuario.");
            CargarUsuarios();
            PrepararNuevoUsuario();
        }

        private void CrearUsuario()
        {
            if (!ValidarCamposAlta())
            {
                return;
            }

            Usuario usuario = CrearUsuarioDesdeFormulario();
            CodigoRegistroUsuario resultado = _usuarioService.CrearUsuario(usuario);

            if (resultado == CodigoRegistroUsuario.Creado)
            {
                MessageBox.Show("Usuario creado correctamente.");
                CargarUsuarios();
                PrepararNuevoUsuario();
                return;
            }

            MessageBox.Show(ObtenerMensajeRegistro(resultado));
        }

        private void ModificarUsuario()
        {
            if (!ValidarCamposModificacion())
            {
                return;
            }

            Usuario usuario = CrearUsuarioDesdeFormulario();
            usuario.Id = _usuarioSeleccionado.Id;

            bool resultado = _usuarioService.ModificarUsuario(usuario);
            MessageBox.Show(resultado ? "Usuario modificado correctamente." : "No se pudo modificar el usuario.");
            CargarUsuarios();
            PrepararNuevoUsuario();
        }

        private Usuario CrearUsuarioDesdeFormulario()
        {
            return new Usuario
            {
                Username = txtUsuario.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                Password = txtPassword.Text,
                Nombre = txtNombre.Text.Trim(),
                Apellido = txtApellido.Text.Trim(),
                Estado = cmbEstado.SelectedItem == null ? "ACTIVO" : cmbEstado.SelectedItem.ToString()
            };
        }

        private bool ValidarCamposAlta()
        {
            if (string.IsNullOrWhiteSpace(txtPassword.Text) ||
                string.IsNullOrWhiteSpace(txtUsuario.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Completa usuario, email y contrasena para crear.");
                return false;
            }

            return ValidarCamposModificacion();
        }

        private bool ValidarCamposModificacion()
        {
            if (string.IsNullOrWhiteSpace(txtUsuario.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Completa usuario y email.");
                return false;
            }

            if (!_usuarioService.EsEmailValido(txtEmail.Text))
            {
                MessageBox.Show("Ingresa un email valido.");
                return false;
            }

            return true;
        }

        private void PrepararNuevoUsuario()
        {
            _preparandoFormulario = true;
            _usuarioSeleccionado = null;
            dgvUsuarios.ClearSelection();
            txtUsuario.Clear();
            txtEmail.Clear();
            txtNombre.Clear();
            txtApellido.Clear();
            txtPassword.Clear();
            cmbEstado.SelectedItem = "ACTIVO";
            lblModo.Tag = "USERS_CREATE_MODE";
            lblModo.Text = LanguageManager.Instance.Translate("USERS_CREATE_MODE");
            btnGuardar.Tag = "BTN_CREATE";
            btnGuardar.Text = LanguageManager.Instance.Translate("BTN_CREATE");
            btnInhabilitar.Enabled = false;
            txtUsuario.Focus();
            _preparandoFormulario = false;
        }

        private static string ObtenerMensajeRegistro(CodigoRegistroUsuario resultado)
        {
            switch (resultado)
            {
                case CodigoRegistroUsuario.DatosInvalidos:
                    return "Completa todos los campos obligatorios.";

                case CodigoRegistroUsuario.EmailInvalido:
                    return "Ingresa un email valido.";

                case CodigoRegistroUsuario.UsuarioExistente:
                    return "Ya existe un usuario con ese nombre.";

                case CodigoRegistroUsuario.EmailExistente:
                    return "Ya existe un usuario con ese email.";

                case CodigoRegistroUsuario.IdiomaDefaultInexistente:
                    return "No se pudo crear el usuario porque falta el idioma default.";

                default:
                    return "Ocurrio un error tecnico al crear el usuario.";
            }
        }
    }
}
