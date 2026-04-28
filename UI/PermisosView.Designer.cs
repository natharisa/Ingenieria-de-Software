namespace UI
{
    partial class PermisosView
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
            this.groupBoxPermisos = new System.Windows.Forms.GroupBox();
            this.listBoxPermisos = new System.Windows.Forms.ListBox();
            this.groupBoxPermisos.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location = new System.Drawing.Point(18, 18);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(124, 32);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Permisos";
            // 
            // lblDescripcion
            // 
            this.lblDescripcion.AutoSize = true;
            this.lblDescripcion.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblDescripcion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.lblDescripcion.Location = new System.Drawing.Point(20, 58);
            this.lblDescripcion.Name = "lblDescripcion";
            this.lblDescripcion.Size = new System.Drawing.Size(307, 19);
            this.lblDescripcion.TabIndex = 1;
            this.lblDescripcion.Text = "Pantalla base para administrar permisos futuros.";
            // 
            // groupBoxPermisos
            // 
            this.groupBoxPermisos.Controls.Add(this.listBoxPermisos);
            this.groupBoxPermisos.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.groupBoxPermisos.Location = new System.Drawing.Point(24, 98);
            this.groupBoxPermisos.Name = "groupBoxPermisos";
            this.groupBoxPermisos.Size = new System.Drawing.Size(380, 253);
            this.groupBoxPermisos.TabIndex = 2;
            this.groupBoxPermisos.TabStop = false;
            this.groupBoxPermisos.Text = "Permisos disponibles";
            // 
            // listBoxPermisos
            // 
            this.listBoxPermisos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxPermisos.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.listBoxPermisos.FormattingEnabled = true;
            this.listBoxPermisos.ItemHeight = 17;
            this.listBoxPermisos.Items.AddRange(new object[] {
            "Ver bitacora",
            "Gestionar permisos",
            "Asignar roles"});
            this.listBoxPermisos.Location = new System.Drawing.Point(3, 21);
            this.listBoxPermisos.Name = "listBoxPermisos";
            this.listBoxPermisos.Size = new System.Drawing.Size(374, 229);
            this.listBoxPermisos.TabIndex = 0;
            // 
            // PermisosView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.groupBoxPermisos);
            this.Controls.Add(this.lblDescripcion);
            this.Controls.Add(this.lblTitulo);
            this.Name = "PermisosView";
            this.Size = new System.Drawing.Size(900, 520);
            this.groupBoxPermisos.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblDescripcion;
        private System.Windows.Forms.GroupBox groupBoxPermisos;
        private System.Windows.Forms.ListBox listBoxPermisos;
    }
}
