using System;
using System.Windows.Forms;
using Domain;

namespace UI
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            Load += MainForm_Load;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Usuario usuario = Sesion.ObtenerInstancia().ObtenerUsuario();

            lblUsuario.Text = usuario != null
                ? $"Usuario: {usuario}"
                : "Usuario: sin sesion";

            ShowScreen(new BitacoraView());
        }

        private void bitacoraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowScreen(new BitacoraView());
        }

        private void permisosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowScreen(new PermisosView());
        }

        private void rolesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowScreen(new RolesView());
        }

        private void usuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
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
    }
}
