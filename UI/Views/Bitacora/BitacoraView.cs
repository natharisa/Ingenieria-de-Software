using System;
using System.Collections.Generic;
using Application;
using Domain;
using System.Windows.Forms;

namespace UI
{
    public partial class BitacoraView : UserControl
    {
        private readonly BitacoraApplicationService _bitacoraService;

        public BitacoraView()
            : this(new BitacoraApplicationService())
        {
        }

        public BitacoraView(BitacoraApplicationService bitacoraService)
        {
            _bitacoraService = bitacoraService;
            InitializeComponent();
            CargarBitacora();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            CargarBitacora();
        }

        private void CargarBitacora()
        {
            listViewBitacora.BeginUpdate();
            listViewBitacora.Items.Clear();

            List<BitacoraRegistro> registros = _bitacoraService.Listar();

            foreach (BitacoraRegistro registro in registros)
            {
                ListViewItem item = new ListViewItem(registro.Id.ToString());
                item.SubItems.Add(registro.Fecha.ToString("dd/MM/yyyy HH:mm:ss"));
                item.SubItems.Add(registro.IdUsuario.HasValue ? registro.IdUsuario.Value.ToString() : string.Empty);
                item.SubItems.Add(registro.IdentificadorUsuario ?? string.Empty);
                item.SubItems.Add(registro.Modulo ?? string.Empty);
                item.SubItems.Add(registro.Accion ?? string.Empty);
                item.SubItems.Add(registro.Nivel ?? string.Empty);
                item.SubItems.Add(registro.Descripcion ?? string.Empty);
                item.SubItems.Add(registro.Equipo ?? string.Empty);

                listViewBitacora.Items.Add(item);
            }

            listViewBitacora.EndUpdate();
            lblEstado.Text = registros.Count == 0
                ? "No hay eventos registrados."
                : string.Format("{0} evento(s) registrados.", registros.Count);
        }
    }
}
