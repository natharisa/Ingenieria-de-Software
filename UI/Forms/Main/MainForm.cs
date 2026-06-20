using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Domain;
using Services;

namespace UI
{
    public partial class MainForm : Form, IObserverLanguage
    {
        private bool _cargandoIdiomas;

        public MainForm()
        {
            InitializeComponent();
            ConfigurarTraducciones();
            Load += MainForm_Load;
            FormClosed += MainForm_FormClosed;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Usuario usuario = Sesion.ObtenerInstancia().ObtenerUsuario();
            LanguageManager.Instance.Attach(this);
            LanguageManager.Instance.Initialize(usuario);
            CargarSelectorIdiomas();
            ActualizarUsuario();

            ShowScreen(new BitacoraView());
        }

        public void OnLanguageChanged(Idioma idioma)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => OnLanguageChanged(idioma)));
                return;
            }

            TranslationApplier.ApplyMenu(menuStrip1);
            TranslationApplier.Apply(topPanel);
            Text = LanguageManager.Instance.Translate("MAIN_TITLE");
            ActualizarUsuario();
            SeleccionarIdiomaActual();
        }

        private void ConfigurarTraducciones()
        {
            bitacoraToolStripMenuItem.Tag = "MENU_AUDIT";
            usuariosToolStripMenuItem.Tag = "MENU_USERS";
            permisosToolStripMenuItem.Tag = "MENU_PERMISSIONS";
            rolesToolStripMenuItem.Tag = "MENU_ROLES";
            idiomasToolStripMenuItem.Tag = "MENU_LANGUAGES";
            salirToolStripMenuItem.Tag = "MENU_LOGOUT";
            lblIdioma.Tag = "LANGUAGE_SELECTOR";
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

        private void idiomasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowScreen(new IdiomasView());
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

        public void RefrescarIdiomasDisponibles()
        {
            CargarSelectorIdiomas();
        }

        private void CargarSelectorIdiomas()
        {
            _cargandoIdiomas = true;
            List<Idioma> idiomas = LanguageManager.Instance.ListarIdiomasActivos();
            cmbIdiomas.DataSource = null;
            cmbIdiomas.DisplayMember = "Nombre";
            cmbIdiomas.ValueMember = "Id";
            cmbIdiomas.DataSource = idiomas;
            _cargandoIdiomas = false;
            SeleccionarIdiomaActual();
        }

        private void SeleccionarIdiomaActual()
        {
            if (_cargandoIdiomas || LanguageManager.Instance.CurrentLanguage == null)
            {
                return;
            }

            cmbIdiomas.SelectedValue = LanguageManager.Instance.CurrentLanguage.Id;
        }

        private void cmbIdiomas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_cargandoIdiomas)
            {
                return;
            }

            Idioma idioma = cmbIdiomas.SelectedItem as Idioma;
            Usuario usuario = Sesion.ObtenerInstancia().ObtenerUsuario();
            LanguageManager.Instance.ChangeLanguage(idioma, usuario);
        }

        private void ActualizarUsuario()
        {
            Usuario usuario = Sesion.ObtenerInstancia().ObtenerUsuario();
            lblUsuario.Text = usuario != null
                ? string.Format(LanguageManager.Instance.Translate("MAIN_USER"), usuario)
                : LanguageManager.Instance.Translate("MAIN_NO_SESSION");
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            LanguageManager.Instance.Detach(this);
        }
    }
}
