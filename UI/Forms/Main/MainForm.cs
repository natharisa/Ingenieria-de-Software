using System;
using System.Drawing;
using System.Windows.Forms;
using Application;
using Domain;

namespace UI
{
    public partial class MainForm : Form
    {
        private readonly AutorizacionApplicationService _autorizacionService;

        public MainForm()
            : this(new AutorizacionApplicationService())
        {
        }

        public MainForm(AutorizacionApplicationService autorizacionService)
        {
            _autorizacionService = autorizacionService;
            InitializeComponent();
            Load += MainForm_Load;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Usuario usuario = Sesion.ObtenerInstancia().ObtenerUsuario();

            lblUsuario.Text = usuario != null
                ? $"Usuario: {usuario}"
                : "Usuario: sin sesion";

            ConfigurarMenuPorPermisos();
            MostrarPantallaInicial();
        }

        private void bitacoraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!ValidarPermiso(PermisosSistema.BitacoraVer))
            {
                return;
            }

            ShowScreen(new BitacoraView());
        }

        private void permisosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Los permisos se administran desde Roles.");
        }

        private void rolesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!ValidarPermiso(PermisosSistema.RolVer))
            {
                return;
            }

            ShowScreen(new RolesView());
        }

        private void usuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!ValidarPermiso(PermisosSistema.UsuarioVer))
            {
                return;
            }

            ShowScreen(new UsuariosView());
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Sesion.ObtenerInstancia().Logout();
            DialogResult = DialogResult.Retry;
            Close();
        }

        private void ShowScreen(UserControl screen)
        {
            contentPanel.Controls.Clear();
            screen.Dock = DockStyle.Fill;
            contentPanel.Controls.Add(screen);
        }

        private void ConfigurarMenuPorPermisos()
        {
            bitacoraToolStripMenuItem.Visible = _autorizacionService.TienePermiso(PermisosSistema.BitacoraVer);
            usuariosToolStripMenuItem.Visible = _autorizacionService.TienePermiso(PermisosSistema.UsuarioVer);
            permisosToolStripMenuItem.Visible = false;
            rolesToolStripMenuItem.Visible = _autorizacionService.TienePermiso(PermisosSistema.RolVer);
        }

        private void MostrarPantallaInicial()
        {
            if (bitacoraToolStripMenuItem.Visible)
            {
                ShowScreen(new BitacoraView());
                return;
            }

            if (usuariosToolStripMenuItem.Visible)
            {
                ShowScreen(new UsuariosView());
                return;
            }

            if (rolesToolStripMenuItem.Visible)
            {
                ShowScreen(new RolesView());
                return;
            }

            Label mensaje = new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 12F),
                TextAlign = ContentAlignment.MiddleCenter,
                Text = "No tenes permisos asignados para acceder a modulos del sistema."
            };

            contentPanel.Controls.Clear();
            contentPanel.Controls.Add(mensaje);
        }

        private bool ValidarPermiso(string codigoPermiso)
        {
            if (_autorizacionService.TienePermiso(codigoPermiso))
            {
                return true;
            }

            MessageBox.Show("No tenes permisos para acceder a esta funcionalidad.");
            return false;
        }
    }
}
