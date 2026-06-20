namespace UI
{
    public partial class RolesView : LocalizedUserControl
    {
        public RolesView()
        {
            InitializeComponent();
            ConfigurarTraducciones();
        }

        private void ConfigurarTraducciones()
        {
            lblTitulo.Tag = "ROLES_TITLE";
            lblDescripcion.Tag = "ROLES_DESCRIPTION";
            groupBoxRoles.Tag = "ROLES_STRUCTURE";
        }
    }
}
