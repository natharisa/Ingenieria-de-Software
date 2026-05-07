namespace UI
{
    partial class BitacoraView
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblDescripcion = new System.Windows.Forms.Label();
            this.listViewBitacora = new System.Windows.Forms.ListView();
            this.columnFecha = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnUsuario = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnEvento = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location = new System.Drawing.Point(18, 18);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(117, 32);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Bitacora";
            // 
            // lblDescripcion
            // 
            this.lblDescripcion.AutoSize = true;
            this.lblDescripcion.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblDescripcion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.lblDescripcion.Location = new System.Drawing.Point(20, 58);
            this.lblDescripcion.Name = "lblDescripcion";
            this.lblDescripcion.Size = new System.Drawing.Size(320, 19);
            this.lblDescripcion.TabIndex = 1;
            this.lblDescripcion.Text = "Aca vas a poder consultar los eventos del sistema.";
            // 
            // listViewBitacora
            // 
            this.listViewBitacora.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnFecha,
            this.columnUsuario,
            this.columnEvento});
            this.listViewBitacora.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.listViewBitacora.FullRowSelect = true;
            this.listViewBitacora.GridLines = true;
            this.listViewBitacora.HideSelection = false;
            this.listViewBitacora.Location = new System.Drawing.Point(24, 97);
            this.listViewBitacora.Name = "listViewBitacora";
            this.listViewBitacora.Size = new System.Drawing.Size(826, 372);
            this.listViewBitacora.TabIndex = 2;
            this.listViewBitacora.UseCompatibleStateImageBehavior = false;
            this.listViewBitacora.View = System.Windows.Forms.View.Details;
            // 
            // columnFecha
            // 
            this.columnFecha.Text = "Fecha";
            this.columnFecha.Width = 180;
            // 
            // columnUsuario
            // 
            this.columnUsuario.Text = "Usuario";
            this.columnUsuario.Width = 220;
            // 
            // columnEvento
            // 
            this.columnEvento.Text = "Evento";
            this.columnEvento.Width = 400;
            // 
            // BitacoraView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.listViewBitacora);
            this.Controls.Add(this.lblDescripcion);
            this.Controls.Add(this.lblTitulo);
            this.Name = "BitacoraView";
            this.Size = new System.Drawing.Size(900, 520);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblDescripcion;
        private System.Windows.Forms.ListView listViewBitacora;
        private System.Windows.Forms.ColumnHeader columnFecha;
        private System.Windows.Forms.ColumnHeader columnUsuario;
        private System.Windows.Forms.ColumnHeader columnEvento;
    }
}
