using System.Collections.Generic;
using System.Windows.Forms;
using Application;
using Domain;

namespace UI
{
    public partial class PermisosView : UserControl
    {
        private readonly PermisoApplicationService _permisoService;

        public PermisosView()
            : this(new PermisoApplicationService())
        {
        }

        public PermisosView(PermisoApplicationService permisoService)
        {
            _permisoService = permisoService;
            InitializeComponent();
            CargarArbol();
        }

        private void CargarArbol()
        {
            treeViewPermisos.Nodes.Clear();
            List<ComponentePermiso> componentes = _permisoService.ListarArbolCompleto();

            foreach (ComponentePermiso componente in componentes)
            {
                AgregarNodo(componente, treeViewPermisos.Nodes);
            }

            treeViewPermisos.ExpandAll();
        }

        private static void AgregarNodo(ComponentePermiso componente, TreeNodeCollection destino)
        {
            TreeNode nodo = new TreeNode(FormatearTexto(componente))
            {
                Tag = componente
            };

            destino.Add(nodo);

            foreach (ComponentePermiso hijo in componente.ObtenerHijos())
            {
                AgregarNodo(hijo, nodo.Nodes);
            }
        }

        private static string FormatearTexto(ComponentePermiso componente)
        {
            return componente.Tipo == TipoComponentePermiso.Familia
                ? $"Familia: {componente.Nombre}"
                : $"{componente.Codigo} - {componente.Nombre}";
        }
    }
}
