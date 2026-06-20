using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Application;
using Domain;
using Services;

namespace UI
{
    public class IdiomasView : LocalizedUserControl
    {
        private readonly IdiomaApplicationService _idiomaService;
        private readonly Label lblTitulo = new Label();
        private readonly Label lblDescripcion = new Label();
        private readonly DataGridView dgvIdiomas = new DataGridView();
        private readonly DataGridView dgvTraducciones = new DataGridView();
        private readonly GroupBox groupIdioma = new GroupBox();
        private readonly GroupBox groupEtiqueta = new GroupBox();
        private readonly GroupBox groupTraduccion = new GroupBox();
        private readonly TextBox txtCodigo = new TextBox();
        private readonly TextBox txtNombre = new TextBox();
        private readonly CheckBox chkActivo = new CheckBox();
        private readonly Button btnNuevoIdioma = new Button();
        private readonly Button btnGuardarIdioma = new Button();
        private readonly TextBox txtKey = new TextBox();
        private readonly TextBox txtDescripcionEtiqueta = new TextBox();
        private readonly Button btnCrearEtiqueta = new Button();
        private readonly ComboBox cmbEtiquetas = new ComboBox();
        private readonly ComboBox cmbIdiomas = new ComboBox();
        private readonly TextBox txtTraduccion = new TextBox();
        private readonly Button btnGuardarTraduccion = new Button();

        private List<Idioma> _idiomas = new List<Idioma>();
        private List<Etiqueta> _etiquetas = new List<Etiqueta>();
        private Idioma _idiomaSeleccionado;

        public IdiomasView()
            : this(new IdiomaApplicationService())
        {
        }

        public IdiomasView(IdiomaApplicationService idiomaService)
        {
            _idiomaService = idiomaService;
            ConstruirVista();
            ConfigurarTraducciones();
            CargarDatos();
            PrepararNuevoIdioma();
        }

        private void ConstruirVista()
        {
            BackColor = Color.White;
            Size = new Size(900, 520);

            lblTitulo.SetBounds(18, 18, 300, 36);
            lblTitulo.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblDescripcion.SetBounds(20, 58, 650, 24);
            lblDescripcion.Font = new Font("Segoe UI", 10F);
            lblDescripcion.ForeColor = Color.FromArgb(108, 117, 125);

            dgvIdiomas.SetBounds(24, 98, 385, 250);
            dgvIdiomas.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            dgvIdiomas.AutoGenerateColumns = false;
            dgvIdiomas.AllowUserToAddRows = false;
            dgvIdiomas.AllowUserToDeleteRows = false;
            dgvIdiomas.ReadOnly = true;
            dgvIdiomas.MultiSelect = false;
            dgvIdiomas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvIdiomas.RowHeadersVisible = false;
            dgvIdiomas.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Id", HeaderText = "Id", Width = 55, Tag = "GRID_ID" });
            dgvIdiomas.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Codigo", HeaderText = "Codigo", Width = 90, Tag = "LANGUAGE_CODE" });
            dgvIdiomas.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Nombre", HeaderText = "Nombre", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, Tag = "FIELD_NAME" });
            dgvIdiomas.Columns.Add(new DataGridViewCheckBoxColumn { DataPropertyName = "Activo", HeaderText = "Activo", Width = 70, Tag = "LANGUAGE_ACTIVE" });
            dgvIdiomas.SelectionChanged += dgvIdiomas_SelectionChanged;

            groupIdioma.SetBounds(430, 98, 430, 150);
            groupIdioma.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupIdioma.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);

            Label lblCodigo = CrearLabel("Codigo", 16, 30, "LANGUAGE_CODE");
            Label lblNombre = CrearLabel("Nombre", 150, 30, "FIELD_NAME");
            txtCodigo.SetBounds(16, 50, 120, 25);
            txtNombre.SetBounds(150, 50, 250, 25);
            chkActivo.SetBounds(16, 84, 120, 25);
            btnNuevoIdioma.SetBounds(230, 104, 82, 30);
            btnGuardarIdioma.SetBounds(318, 104, 82, 30);
            btnNuevoIdioma.Click += btnNuevoIdioma_Click;
            btnGuardarIdioma.Click += btnGuardarIdioma_Click;
            groupIdioma.Controls.AddRange(new Control[] { lblCodigo, lblNombre, txtCodigo, txtNombre, chkActivo, btnNuevoIdioma, btnGuardarIdioma });

            groupEtiqueta.SetBounds(430, 258, 430, 112);
            groupEtiqueta.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupEtiqueta.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            Label lblKey = CrearLabel("Key", 16, 28, "LABEL_KEY");
            Label lblDescripcionEtiqueta = CrearLabel("Descripcion", 150, 28, "GRID_DESCRIPTION");
            txtKey.SetBounds(16, 48, 120, 25);
            txtDescripcionEtiqueta.SetBounds(150, 48, 250, 25);
            btnCrearEtiqueta.SetBounds(275, 78, 125, 28);
            btnCrearEtiqueta.Click += btnCrearEtiqueta_Click;
            groupEtiqueta.Controls.AddRange(new Control[] { lblKey, lblDescripcionEtiqueta, txtKey, txtDescripcionEtiqueta, btnCrearEtiqueta });

            groupTraduccion.SetBounds(24, 360, 836, 124);
            groupTraduccion.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            groupTraduccion.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            cmbEtiquetas.SetBounds(16, 44, 210, 25);
            cmbIdiomas.SetBounds(236, 44, 170, 25);
            txtTraduccion.SetBounds(416, 44, 275, 25);
            btnGuardarTraduccion.SetBounds(700, 42, 120, 29);
            btnGuardarTraduccion.Click += btnGuardarTraduccion_Click;
            groupTraduccion.Controls.AddRange(new Control[]
            {
                CrearLabel("Etiqueta", 16, 24, "LABEL_TAG"),
                CrearLabel("Idioma", 236, 24, "LANGUAGE_SELECTOR"),
                CrearLabel("Traduccion", 416, 24, "TRANSLATION_TEXT"),
                cmbEtiquetas,
                cmbIdiomas,
                txtTraduccion,
                btnGuardarTraduccion
            });

            dgvTraducciones.SetBounds(24, 490, 836, 0);
            Controls.AddRange(new Control[] { lblTitulo, lblDescripcion, dgvIdiomas, groupIdioma, groupEtiqueta, groupTraduccion });
        }

        private Label CrearLabel(string texto, int x, int y, string tag)
        {
            return new Label
            {
                Text = texto,
                Tag = tag,
                Location = new Point(x, y),
                Size = new Size(125, 18),
                Font = new Font("Segoe UI", 9F)
            };
        }

        private void ConfigurarTraducciones()
        {
            lblTitulo.Tag = "LANGUAGES_TITLE";
            lblDescripcion.Tag = "LANGUAGES_DESCRIPTION";
            groupIdioma.Tag = "LANGUAGE_DETAIL";
            groupEtiqueta.Tag = "LABEL_DETAIL";
            groupTraduccion.Tag = "TRANSLATION_DETAIL";
            chkActivo.Tag = "LANGUAGE_ACTIVE";
            btnNuevoIdioma.Tag = "BTN_NEW";
            btnGuardarIdioma.Tag = "BTN_SAVE";
            btnCrearEtiqueta.Tag = "BTN_CREATE_LABEL";
            btnGuardarTraduccion.Tag = "BTN_SAVE";
        }

        private void CargarDatos()
        {
            _idiomas = _idiomaService.ListarIdiomas(false);
            _etiquetas = _idiomaService.ListarEtiquetas();

            dgvIdiomas.DataSource = null;
            dgvIdiomas.DataSource = _idiomas;

            cmbIdiomas.DataSource = null;
            cmbIdiomas.DisplayMember = "Nombre";
            cmbIdiomas.ValueMember = "Id";
            cmbIdiomas.DataSource = new List<Idioma>(_idiomas);

            cmbEtiquetas.DataSource = null;
            cmbEtiquetas.DisplayMember = "Key";
            cmbEtiquetas.ValueMember = "Id";
            cmbEtiquetas.DataSource = new List<Etiqueta>(_etiquetas);
        }

        private void PrepararNuevoIdioma()
        {
            _idiomaSeleccionado = null;
            txtCodigo.Clear();
            txtNombre.Clear();
            chkActivo.Checked = true;
        }

        private void dgvIdiomas_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvIdiomas.CurrentRow == null)
            {
                return;
            }

            _idiomaSeleccionado = dgvIdiomas.CurrentRow.DataBoundItem as Idioma;
            if (_idiomaSeleccionado == null)
            {
                return;
            }

            txtCodigo.Text = _idiomaSeleccionado.Codigo;
            txtNombre.Text = _idiomaSeleccionado.Nombre;
            chkActivo.Checked = _idiomaSeleccionado.Activo;
        }

        private void btnNuevoIdioma_Click(object sender, EventArgs e)
        {
            PrepararNuevoIdioma();
        }

        private void btnGuardarIdioma_Click(object sender, EventArgs e)
        {
            Idioma idioma = _idiomaSeleccionado ?? new Idioma();
            idioma.Codigo = txtCodigo.Text;
            idioma.Nombre = txtNombre.Text;
            idioma.Activo = chkActivo.Checked;

            Usuario usuario = Sesion.ObtenerInstancia().ObtenerUsuario();
            bool resultado = _idiomaService.GuardarIdioma(
                idioma,
                usuario == null ? (int?)null : usuario.Id,
                "Cambio de activacion desde administracion de idiomas.");

            MessageBox.Show(resultado
                ? LanguageManager.Instance.Translate("LANGUAGE_SAVED")
                : LanguageManager.Instance.Translate("SAVE_ERROR"));

            CargarDatos();
            CargarSelectorPrincipalSiCorresponde();
        }

        private void btnCrearEtiqueta_Click(object sender, EventArgs e)
        {
            bool resultado = _idiomaService.CrearEtiqueta(new Etiqueta
            {
                Key = txtKey.Text,
                Descripcion = txtDescripcionEtiqueta.Text
            });

            MessageBox.Show(resultado
                ? LanguageManager.Instance.Translate("LABEL_SAVED")
                : LanguageManager.Instance.Translate("SAVE_ERROR"));

            txtKey.Clear();
            txtDescripcionEtiqueta.Clear();
            CargarDatos();
        }

        private void btnGuardarTraduccion_Click(object sender, EventArgs e)
        {
            Etiqueta etiqueta = cmbEtiquetas.SelectedItem as Etiqueta;
            Idioma idioma = cmbIdiomas.SelectedItem as Idioma;

            bool resultado = _idiomaService.GuardarTraduccion(new Traduccion
            {
                EtiquetaId = etiqueta == null ? 0 : etiqueta.Id,
                IdiomaId = idioma == null ? 0 : idioma.Id,
                Texto = txtTraduccion.Text
            });

            MessageBox.Show(resultado
                ? LanguageManager.Instance.Translate("TRANSLATION_SAVED")
                : LanguageManager.Instance.Translate("SAVE_ERROR"));

            if (resultado && idioma != null && LanguageManager.Instance.CurrentLanguage != null &&
                idioma.Id == LanguageManager.Instance.CurrentLanguage.Id)
            {
                LanguageManager.Instance.Notify();
            }

            txtTraduccion.Clear();
        }

        private void CargarSelectorPrincipalSiCorresponde()
        {
            Form form = FindForm();
            MainForm mainForm = form as MainForm;
            if (mainForm != null)
            {
                mainForm.RefrescarIdiomasDisponibles();
            }
        }
    }
}
