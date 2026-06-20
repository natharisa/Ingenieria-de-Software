using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Application;
using Domain;

namespace UI
{
    public partial class RolesView : UserControl
    {
        private readonly PermisoApplicationService _permisoService;
        private readonly AutorizacionApplicationService _autorizacionService;
        private TextBox txtCodigoRol;
        private TextBox txtNombreRol;
        private TextBox txtDescripcionRol;
        private Label lblFamiliaSeleccionada;
        private ComboBox cmbComponenteHijo;
        private Button btnCrearRol;
        private Button btnAgregarHijo;
        private Button btnQuitarHijo;

        public RolesView()
            : this(new PermisoApplicationService())
        {
        }

        public RolesView(PermisoApplicationService permisoService)
        {
            _permisoService = permisoService;
            _autorizacionService = new AutorizacionApplicationService();
            InitializeComponent();
            ConfigurarAdministracion();
            CargarArbol();
            CargarCombos();
            ActualizarAccionesSeleccionadas();
        }

        private void CargarArbol()
        {
            treeViewRoles.Nodes.Clear();
            List<ComponentePermiso> componentes = _permisoService.ListarArbolCompleto();

            foreach (ComponentePermiso componente in componentes)
            {
                AgregarNodo(componente, treeViewRoles.Nodes);
            }

            treeViewRoles.AfterSelect -= treeViewRoles_AfterSelect;
            treeViewRoles.AfterSelect += treeViewRoles_AfterSelect;
            treeViewRoles.ExpandAll();
        }

        private void ConfigurarAdministracion()
        {
            groupBoxRoles.Location = new Point(24, 98);
            groupBoxRoles.Size = new Size(500, 390);

            GroupBox groupBoxEdicion = new GroupBox
            {
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                Location = new Point(548, 98),
                Name = "groupBoxEdicionRoles",
                Size = new Size(328, 390),
                Text = "Administracion de roles"
            };

            Label lblCodigo = CrearLabel("Codigo", 18, 30);
            txtCodigoRol = CrearTextBox(18, 48);

            Label lblNombre = CrearLabel("Nombre", 18, 80);
            txtNombreRol = CrearTextBox(18, 98);

            Label lblDescripcion = CrearLabel("Descripcion", 18, 130);
            txtDescripcionRol = CrearTextBox(18, 148);

            btnCrearRol = CrearBoton("Crear rol", 18, 181);
            btnCrearRol.Click += btnCrearRol_Click;

            Label lblRolPadre = CrearLabel("Familia seleccionada en el arbol", 18, 222);
            lblFamiliaSeleccionada = new Label
            {
                AutoSize = false,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 9F),
                Location = new Point(18, 240),
                Size = new Size(280, 25),
                TextAlign = ContentAlignment.MiddleLeft
            };

            Label lblHijo = CrearLabel("Permiso o familia a agregar", 18, 272);
            cmbComponenteHijo = CrearCombo(18, 290);

            btnAgregarHijo = CrearBoton("Agregar", 18, 326);
            btnAgregarHijo.Click += btnAgregarHijo_Click;

            btnQuitarHijo = CrearBoton("Quitar seleccionado", 128, 326);
            btnQuitarHijo.Size = new Size(170, 31);
            btnQuitarHijo.Click += btnQuitarHijo_Click;

            groupBoxEdicion.Controls.Add(lblCodigo);
            groupBoxEdicion.Controls.Add(txtCodigoRol);
            groupBoxEdicion.Controls.Add(lblNombre);
            groupBoxEdicion.Controls.Add(txtNombreRol);
            groupBoxEdicion.Controls.Add(lblDescripcion);
            groupBoxEdicion.Controls.Add(txtDescripcionRol);
            groupBoxEdicion.Controls.Add(btnCrearRol);
            groupBoxEdicion.Controls.Add(lblRolPadre);
            groupBoxEdicion.Controls.Add(lblFamiliaSeleccionada);
            groupBoxEdicion.Controls.Add(lblHijo);
            groupBoxEdicion.Controls.Add(cmbComponenteHijo);
            groupBoxEdicion.Controls.Add(btnAgregarHijo);
            groupBoxEdicion.Controls.Add(btnQuitarHijo);
            Controls.Add(groupBoxEdicion);

            btnCrearRol.Visible = _autorizacionService.TienePermiso(PermisosSistema.RolCrear);
            btnAgregarHijo.Visible = _autorizacionService.TienePermiso(PermisosSistema.RolEditar);
            btnQuitarHijo.Visible = _autorizacionService.TienePermiso(PermisosSistema.RolEditar);
        }

        private void CargarCombos()
        {
            List<ComponentePermiso> componentes = _permisoService.ListarComponentes();

            cmbComponenteHijo.DataSource = null;
            cmbComponenteHijo.DataSource = componentes;
        }

        private void btnCrearRol_Click(object sender, System.EventArgs e)
        {
            if (!_autorizacionService.TienePermiso(PermisosSistema.RolCrear))
            {
                MessageBox.Show("No tenes permisos para crear roles.");
                return;
            }

            bool creado = _permisoService.CrearFamilia(
                txtCodigoRol.Text,
                txtNombreRol.Text,
                txtDescripcionRol.Text);

            MessageBox.Show(creado ? "Rol creado correctamente." : "No se pudo crear el rol.");

            if (creado)
            {
                txtCodigoRol.Clear();
                txtNombreRol.Clear();
                txtDescripcionRol.Clear();
                CargarArbol();
                CargarCombos();
                ActualizarAccionesSeleccionadas();
            }
        }

        private void btnAgregarHijo_Click(object sender, System.EventArgs e)
        {
            if (!_autorizacionService.TienePermiso(PermisosSistema.RolEditar))
            {
                MessageBox.Show("No tenes permisos para modificar roles.");
                return;
            }

            ComponentePermiso padre = ObtenerFamiliaSeleccionada();
            ComponentePermiso hijo = cmbComponenteHijo.SelectedItem as ComponentePermiso;

            if (padre == null || hijo == null)
            {
                MessageBox.Show("Selecciona una familia en el arbol y un componente para agregar.");
                return;
            }

            string resultado = _permisoService.AgregarRelacion(padre.Id, hijo.Id);
            MessageBox.Show(ObtenerMensajeRelacion(resultado));
            CargarArbol();
            CargarCombos();
            ActualizarAccionesSeleccionadas();
        }

        private void btnQuitarHijo_Click(object sender, System.EventArgs e)
        {
            if (!_autorizacionService.TienePermiso(PermisosSistema.RolEditar))
            {
                MessageBox.Show("No tenes permisos para modificar roles.");
                return;
            }

            if (treeViewRoles.SelectedNode == null || treeViewRoles.SelectedNode.Parent == null)
            {
                MessageBox.Show("Selecciona un componente hijo dentro del arbol.");
                return;
            }

            ComponentePermiso padre = treeViewRoles.SelectedNode.Parent.Tag as ComponentePermiso;
            ComponentePermiso hijo = treeViewRoles.SelectedNode.Tag as ComponentePermiso;

            if (padre == null || hijo == null)
            {
                MessageBox.Show("No se pudo identificar la relacion seleccionada.");
                return;
            }

            bool quitado = _permisoService.QuitarRelacion(padre.Id, hijo.Id);
            MessageBox.Show(quitado ? "Relacion quitada correctamente." : "No se pudo quitar la relacion.");
            CargarArbol();
            CargarCombos();
            ActualizarAccionesSeleccionadas();
        }

        private void treeViewRoles_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ActualizarAccionesSeleccionadas();
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

        private ComponentePermiso ObtenerFamiliaSeleccionada()
        {
            if (treeViewRoles.SelectedNode == null)
            {
                return null;
            }

            ComponentePermiso componente = treeViewRoles.SelectedNode.Tag as ComponentePermiso;

            if (componente != null && componente.Tipo == TipoComponentePermiso.Familia)
            {
                return componente;
            }

            return null;
        }

        private void ActualizarAccionesSeleccionadas()
        {
            ComponentePermiso familiaSeleccionada = ObtenerFamiliaSeleccionada();
            lblFamiliaSeleccionada.Text = familiaSeleccionada == null
                ? "Selecciona una familia en el arbol"
                : familiaSeleccionada.Nombre;

            btnAgregarHijo.Enabled = familiaSeleccionada != null &&
                                     _autorizacionService.TienePermiso(PermisosSistema.RolEditar);

            if (treeViewRoles.SelectedNode != null && treeViewRoles.SelectedNode.Parent != null)
            {
                ComponentePermiso hijo = treeViewRoles.SelectedNode.Tag as ComponentePermiso;
                ComponentePermiso padre = treeViewRoles.SelectedNode.Parent.Tag as ComponentePermiso;
                btnQuitarHijo.Text = hijo == null || padre == null
                    ? "Quitar seleccionado"
                    : $"Quitar de {padre.Nombre}";
                btnQuitarHijo.Enabled = _autorizacionService.TienePermiso(PermisosSistema.RolEditar);
                return;
            }

            btnQuitarHijo.Text = "Quitar seleccionado";
            btnQuitarHijo.Enabled = false;
        }

        private static Label CrearLabel(string texto, int x, int y)
        {
            return new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 9F),
                Location = new Point(x, y),
                Text = texto
            };
        }

        private static TextBox CrearTextBox(int x, int y)
        {
            return new TextBox
            {
                Font = new Font("Segoe UI", 10F),
                Location = new Point(x, y),
                Size = new Size(280, 25)
            };
        }

        private static ComboBox CrearCombo(int x, int y)
        {
            return new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10F),
                Location = new Point(x, y),
                Size = new Size(280, 25)
            };
        }

        private static Button CrearBoton(string texto, int x, int y)
        {
            return new Button
            {
                BackColor = Color.FromArgb(13, 110, 253),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(x, y),
                Size = new Size(100, 31),
                Text = texto,
                UseVisualStyleBackColor = false
            };
        }

        private static string ObtenerMensajeRelacion(string resultado)
        {
            switch (resultado)
            {
                case "OK":
                    return "Componente agregado correctamente.";

                case "AUTO_REFERENCIA":
                    return "Un rol no puede agregarse a si mismo.";

                case "PADRE_INVALIDO":
                    return "El padre debe ser una familia activa.";

                case "HIJO_INVALIDO":
                    return "El componente hijo no existe o esta inactivo.";

                case "CICLO_DETECTADO":
                    return "No se puede agregar porque generaria una relacion circular.";

                default:
                    return "No se pudo agregar el componente.";
            }
        }
    }
}
