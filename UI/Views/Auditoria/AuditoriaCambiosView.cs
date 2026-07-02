using System;
using System.Collections.Generic;
using System.Drawing;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using Application;
using Domain;
using Services;

namespace UI
{
    public class AuditoriaCambiosView : LocalizedUserControl
    {
        private readonly AuditoriaApplicationService _auditoriaService;
        private readonly UsuarioApplicationService _usuarioService;
        private readonly JavaScriptSerializer _serializer = new JavaScriptSerializer();

        private Label lblTitulo;
        private Label lblDescripcion;
        private Label lblUsuarioAuditado;
        private ComboBox cmbUsuarios;
        private Button btnActualizar;
        private ListView listViewCambios;
        private ColumnHeader columnId;
        private ColumnHeader columnFecha;
        private ColumnHeader columnEntidad;
        private ColumnHeader columnEntidadId;
        private ColumnHeader columnActor;
        private ColumnHeader columnCampo;
        private ColumnHeader columnValorAnterior;
        private ColumnHeader columnValorNuevo;
        private Label lblEstado;
        private Label lblEstadoAnterior;
        private Label lblEstadoNuevo;
        private TextBox txtEstadoAnterior;
        private TextBox txtEstadoNuevo;

        private List<Usuario> _usuarios = new List<Usuario>();
        private List<AuditoriaRegistro> _registros = new List<AuditoriaRegistro>();
        private int _cantidadCambios;

        public AuditoriaCambiosView()
            : this(new AuditoriaApplicationService(), new UsuarioApplicationService())
        {
        }

        public AuditoriaCambiosView(AuditoriaApplicationService auditoriaService, UsuarioApplicationService usuarioService)
        {
            _auditoriaService = auditoriaService;
            _usuarioService = usuarioService;
            ConstruirInterfaz();
            ConfigurarTraducciones();
            CargarUsuarios();
        }

        private void ConstruirInterfaz()
        {
            BackColor = Color.White;
            Size = new Size(900, 520);

            lblTitulo = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                Location = new Point(18, 18),
                Text = "Auditoria de cambios"
            };

            lblDescripcion = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(108, 117, 125),
                Location = new Point(20, 58),
                Text = "Historial de cambios registrados sobre entidades auditadas."
            };

            lblUsuarioAuditado = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 9F),
                Location = new Point(24, 101),
                Text = "Usuario auditado"
            };

            cmbUsuarios = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9F),
                Location = new Point(138, 97),
                Size = new Size(260, 23)
            };
            cmbUsuarios.SelectedIndexChanged += cmbUsuarios_SelectedIndexChanged;

            btnActualizar = new Button
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                BackColor = Color.FromArgb(0, 123, 255),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(752, 92),
                Size = new Size(124, 32),
                Text = "Actualizar",
                UseVisualStyleBackColor = false
            };
            btnActualizar.FlatAppearance.BorderSize = 0;
            btnActualizar.Click += btnActualizar_Click;

            listViewCambios = new ListView
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom,
                Font = new Font("Segoe UI", 9F),
                FullRowSelect = true,
                GridLines = true,
                HideSelection = false,
                Location = new Point(24, 136),
                Size = new Size(852, 230),
                UseCompatibleStateImageBehavior = false,
                View = View.Details
            };
            listViewCambios.SelectedIndexChanged += listViewCambios_SelectedIndexChanged;

            columnId = new ColumnHeader { Text = "Id", Width = 55 };
            columnFecha = new ColumnHeader { Text = "Fecha", Width = 135 };
            columnEntidad = new ColumnHeader { Text = "Entidad", Width = 85 };
            columnEntidadId = new ColumnHeader { Text = "Id entidad", Width = 75 };
            columnActor = new ColumnHeader { Text = "Usuario", Width = 120 };
            columnCampo = new ColumnHeader { Text = "Campo", Width = 130 };
            columnValorAnterior = new ColumnHeader { Text = "Valor anterior", Width = 170 };
            columnValorNuevo = new ColumnHeader { Text = "Valor nuevo", Width = 170 };

            listViewCambios.Columns.AddRange(new[]
            {
                columnId,
                columnFecha,
                columnEntidad,
                columnEntidadId,
                columnActor,
                columnCampo,
                columnValorAnterior,
                columnValorNuevo
            });

            lblEstadoAnterior = new Label
            {
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left,
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold),
                Location = new Point(24, 378),
                Text = "Estado anterior"
            };

            lblEstadoNuevo = new Label
            {
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left,
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold),
                Location = new Point(456, 378),
                Text = "Estado nuevo"
            };

            txtEstadoAnterior = CrearTextBoxEstado(new Point(24, 398));
            txtEstadoNuevo = CrearTextBoxEstado(new Point(456, 398));

            lblEstado = new Label
            {
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left,
                AutoSize = true,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(108, 117, 125),
                Location = new Point(24, 494),
                Text = "No hay cambios registrados."
            };

            Controls.Add(lblTitulo);
            Controls.Add(lblDescripcion);
            Controls.Add(lblUsuarioAuditado);
            Controls.Add(cmbUsuarios);
            Controls.Add(btnActualizar);
            Controls.Add(listViewCambios);
            Controls.Add(lblEstadoAnterior);
            Controls.Add(lblEstadoNuevo);
            Controls.Add(txtEstadoAnterior);
            Controls.Add(txtEstadoNuevo);
            Controls.Add(lblEstado);
        }

        private static TextBox CrearTextBoxEstado(Point location)
        {
            return new TextBox
            {
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left,
                Font = new Font("Consolas", 8.5F),
                Location = location,
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                Size = new Size(420, 86)
            };
        }

        private void ConfigurarTraducciones()
        {
            lblTitulo.Tag = "CHANGE_AUDIT_TITLE";
            lblDescripcion.Tag = "CHANGE_AUDIT_DESCRIPTION";
            lblUsuarioAuditado.Tag = "CHANGE_AUDIT_USER";
            btnActualizar.Tag = "BTN_REFRESH";
            columnId.Tag = "GRID_ID";
            columnFecha.Tag = "GRID_DATE";
            columnEntidad.Tag = "GRID_ENTITY";
            columnEntidadId.Tag = "GRID_ENTITY_ID";
            columnActor.Tag = "GRID_USER";
            columnCampo.Tag = "GRID_FIELD";
            columnValorAnterior.Tag = "GRID_OLD_VALUE";
            columnValorNuevo.Tag = "GRID_NEW_VALUE";
            lblEstadoAnterior.Tag = "CHANGE_AUDIT_PREVIOUS_STATE";
            lblEstadoNuevo.Tag = "CHANGE_AUDIT_NEW_STATE";
        }

        protected override void ApplyTranslations()
        {
            base.ApplyTranslations();
            ActualizarEstado();
        }

        private void CargarUsuarios()
        {
            _usuarios = _usuarioService.Listar();
            _usuarios.Insert(0, new Usuario
            {
                Id = 0,
                Username = "Todas las entidades",
                Nombre = "Todas las entidades"
            });

            cmbUsuarios.DataSource = null;
            cmbUsuarios.DisplayMember = "Username";
            cmbUsuarios.ValueMember = "Id";
            cmbUsuarios.DataSource = _usuarios;

            if (_usuarios.Count > 0)
            {
                cmbUsuarios.SelectedIndex = 0;
                CargarAuditoria(_usuarios[0].Id);
            }
            else
            {
                LimpiarCambios();
            }
        }

        private void cmbUsuarios_SelectedIndexChanged(object sender, EventArgs e)
        {
            Usuario usuario = cmbUsuarios.SelectedItem as Usuario;
            if (usuario != null)
            {
                CargarAuditoria(usuario.Id);
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            Usuario usuario = cmbUsuarios.SelectedItem as Usuario;
            if (usuario != null)
            {
                CargarAuditoria(usuario.Id);
            }
        }

        private void CargarAuditoria(int usuarioId)
        {
            _registros = usuarioId == 0
                ? _auditoriaService.ListarTodos()
                : _auditoriaService.ListarHistorial("Usuario", usuarioId);
            listViewCambios.BeginUpdate();
            listViewCambios.Items.Clear();

            foreach (AuditoriaRegistro registro in _registros)
            {
                List<AuditoriaCambio> cambios = DeserializarCambios(registro.CambiosJson);

                foreach (AuditoriaCambio cambio in cambios)
                {
                    ListViewItem item = new ListViewItem(registro.Id.ToString());
                    item.SubItems.Add(registro.FechaEvento.ToString("dd/MM/yyyy HH:mm:ss"));
                    item.SubItems.Add(registro.Entidad ?? string.Empty);
                    item.SubItems.Add(registro.IdEntidad.ToString());
                    item.SubItems.Add(registro.IdentificadorUsuarioActor ?? string.Empty);
                    item.SubItems.Add(cambio.Campo ?? string.Empty);
                    item.SubItems.Add(FormatearValor(cambio.ValorAnterior));
                    item.SubItems.Add(FormatearValor(cambio.ValorNuevo));
                    item.Tag = registro;
                    listViewCambios.Items.Add(item);
                }
            }

            listViewCambios.EndUpdate();
            _cantidadCambios = listViewCambios.Items.Count;
            LimpiarDetalle();
            ActualizarEstado();
        }

        private void listViewCambios_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewCambios.SelectedItems.Count == 0)
            {
                LimpiarDetalle();
                return;
            }

            AuditoriaRegistro registro = listViewCambios.SelectedItems[0].Tag as AuditoriaRegistro;
            if (registro == null)
            {
                LimpiarDetalle();
                return;
            }

            txtEstadoAnterior.Text = registro.EstadoAnteriorJson ?? string.Empty;
            txtEstadoNuevo.Text = registro.EstadoNuevoJson ?? string.Empty;
        }

        private List<AuditoriaCambio> DeserializarCambios(string cambiosJson)
        {
            if (string.IsNullOrWhiteSpace(cambiosJson))
            {
                return new List<AuditoriaCambio>();
            }

            try
            {
                return _serializer.Deserialize<List<AuditoriaCambio>>(cambiosJson) ?? new List<AuditoriaCambio>();
            }
            catch
            {
                return new List<AuditoriaCambio>();
            }
        }

        private static string FormatearValor(object valor)
        {
            return valor == null ? string.Empty : valor.ToString();
        }

        private void LimpiarCambios()
        {
            _registros = new List<AuditoriaRegistro>();
            listViewCambios.Items.Clear();
            _cantidadCambios = 0;
            LimpiarDetalle();
            ActualizarEstado();
        }

        private void LimpiarDetalle()
        {
            txtEstadoAnterior.Clear();
            txtEstadoNuevo.Clear();
        }

        private void ActualizarEstado()
        {
            lblEstado.Text = _cantidadCambios == 0
                ? LanguageManager.Instance.Translate("CHANGE_AUDIT_EMPTY")
                : string.Format(LanguageManager.Instance.Translate("CHANGE_AUDIT_COUNT"), _cantidadCambios);
        }
    }
}
