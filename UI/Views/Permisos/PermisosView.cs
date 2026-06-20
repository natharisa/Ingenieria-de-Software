namespace UI
{
    public partial class PermisosView : LocalizedUserControl
    {
        public PermisosView()
        {
            InitializeComponent();
            ConfigurarTraducciones();
        }

        private void ConfigurarTraducciones()
        {
            lblTitulo.Tag = "PERMISSIONS_TITLE";
            lblDescripcion.Tag = "PERMISSIONS_DESCRIPTION";
            groupBoxPermisos.Tag = "PERMISSIONS_AVAILABLE";
        }
    }
}
