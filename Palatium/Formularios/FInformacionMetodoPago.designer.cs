namespace Palatium.Formularios
{
    partial class FInformacionMetodoPago
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabCon_MetodoPago = new System.Windows.Forms.TabControl();
            this.tabPag_MetodoPago = new System.Windows.Forms.TabPage();
            this.Grb_listReMetodoPago = new System.Windows.Forms.GroupBox();
            this.btnBuscarMetodoPago = new System.Windows.Forms.Button();
            this.txtBuscar = new System.Windows.Forms.TextBox();
            this.dgvDatos = new System.Windows.Forms.DataGridView();
            this.Grb_opcioMetodoPago = new System.Windows.Forms.GroupBox();
            this.btnCerrarMetodoPago = new System.Windows.Forms.Button();
            this.btnLimpiarMetodoPago = new System.Windows.Forms.Button();
            this.btnAnularMetodoPago = new System.Windows.Forms.Button();
            this.btnNuevoMetodoPago = new System.Windows.Forms.Button();
            this.Grb_DatoMetodoPago = new System.Windows.Forms.GroupBox();
            this.cmbEstado = new System.Windows.Forms.ComboBox();
            this.lblEstaMetodoPago = new System.Windows.Forms.Label();
            this.lblDescrMetodoPago = new System.Windows.Forms.Label();
            this.txtDescripcion = new System.Windows.Forms.TextBox();
            this.lblCodigoMetodoPago = new System.Windows.Forms.Label();
            this.txtCodigo = new System.Windows.Forms.TextBox();
            this.tabCon_MetodoPago.SuspendLayout();
            this.tabPag_MetodoPago.SuspendLayout();
            this.Grb_listReMetodoPago.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDatos)).BeginInit();
            this.Grb_opcioMetodoPago.SuspendLayout();
            this.Grb_DatoMetodoPago.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabCon_MetodoPago
            // 
            this.tabCon_MetodoPago.Controls.Add(this.tabPag_MetodoPago);
            this.tabCon_MetodoPago.Location = new System.Drawing.Point(-3, -1);
            this.tabCon_MetodoPago.Name = "tabCon_MetodoPago";
            this.tabCon_MetodoPago.SelectedIndex = 0;
            this.tabCon_MetodoPago.Size = new System.Drawing.Size(840, 333);
            this.tabCon_MetodoPago.TabIndex = 2;
            // 
            // tabPag_MetodoPago
            // 
            this.tabPag_MetodoPago.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.tabPag_MetodoPago.Controls.Add(this.Grb_listReMetodoPago);
            this.tabPag_MetodoPago.Controls.Add(this.Grb_opcioMetodoPago);
            this.tabPag_MetodoPago.Controls.Add(this.Grb_DatoMetodoPago);
            this.tabPag_MetodoPago.Location = new System.Drawing.Point(4, 22);
            this.tabPag_MetodoPago.Name = "tabPag_MetodoPago";
            this.tabPag_MetodoPago.Padding = new System.Windows.Forms.Padding(3);
            this.tabPag_MetodoPago.Size = new System.Drawing.Size(832, 307);
            this.tabPag_MetodoPago.TabIndex = 0;
            this.tabPag_MetodoPago.Text = "Método de Pago";
            // 
            // Grb_listReMetodoPago
            // 
            this.Grb_listReMetodoPago.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.Grb_listReMetodoPago.Controls.Add(this.btnBuscarMetodoPago);
            this.Grb_listReMetodoPago.Controls.Add(this.txtBuscar);
            this.Grb_listReMetodoPago.Controls.Add(this.dgvDatos);
            this.Grb_listReMetodoPago.Location = new System.Drawing.Point(409, 19);
            this.Grb_listReMetodoPago.Name = "Grb_listReMetodoPago";
            this.Grb_listReMetodoPago.Size = new System.Drawing.Size(415, 276);
            this.Grb_listReMetodoPago.TabIndex = 5;
            this.Grb_listReMetodoPago.TabStop = false;
            this.Grb_listReMetodoPago.Text = "Lista de Registros";
            // 
            // btnBuscarMetodoPago
            // 
            this.btnBuscarMetodoPago.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnBuscarMetodoPago.ForeColor = System.Drawing.Color.White;
            this.btnBuscarMetodoPago.Location = new System.Drawing.Point(240, 24);
            this.btnBuscarMetodoPago.Name = "btnBuscarMetodoPago";
            this.btnBuscarMetodoPago.Size = new System.Drawing.Size(88, 26);
            this.btnBuscarMetodoPago.TabIndex = 4;
            this.btnBuscarMetodoPago.Text = "Buscar";
            this.btnBuscarMetodoPago.UseVisualStyleBackColor = false;
            this.btnBuscarMetodoPago.Click += new System.EventHandler(this.btnBuscarMetodoPago_Click);
            // 
            // txtBuscar
            // 
            this.txtBuscar.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtBuscar.Location = new System.Drawing.Point(18, 28);
            this.txtBuscar.MaxLength = 20;
            this.txtBuscar.Name = "txtBuscar";
            this.txtBuscar.Size = new System.Drawing.Size(216, 20);
            this.txtBuscar.TabIndex = 3;
            // 
            // dgvDatos
            // 
            this.dgvDatos.AllowUserToAddRows = false;
            this.dgvDatos.AllowUserToDeleteRows = false;
            this.dgvDatos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDatos.Location = new System.Drawing.Point(18, 61);
            this.dgvDatos.Name = "dgvDatos";
            this.dgvDatos.ReadOnly = true;
            this.dgvDatos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDatos.Size = new System.Drawing.Size(377, 203);
            this.dgvDatos.TabIndex = 0;
            this.dgvDatos.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDatos_CellDoubleClick);
            // 
            // Grb_opcioMetodoPago
            // 
            this.Grb_opcioMetodoPago.Controls.Add(this.btnCerrarMetodoPago);
            this.Grb_opcioMetodoPago.Controls.Add(this.btnLimpiarMetodoPago);
            this.Grb_opcioMetodoPago.Controls.Add(this.btnAnularMetodoPago);
            this.Grb_opcioMetodoPago.Controls.Add(this.btnNuevoMetodoPago);
            this.Grb_opcioMetodoPago.Location = new System.Drawing.Point(17, 206);
            this.Grb_opcioMetodoPago.Name = "Grb_opcioMetodoPago";
            this.Grb_opcioMetodoPago.Size = new System.Drawing.Size(386, 89);
            this.Grb_opcioMetodoPago.TabIndex = 4;
            this.Grb_opcioMetodoPago.TabStop = false;
            this.Grb_opcioMetodoPago.Text = "Opciones";
            // 
            // btnCerrarMetodoPago
            // 
            this.btnCerrarMetodoPago.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnCerrarMetodoPago.ForeColor = System.Drawing.Color.White;
            this.btnCerrarMetodoPago.Location = new System.Drawing.Point(275, 19);
            this.btnCerrarMetodoPago.Name = "btnCerrarMetodoPago";
            this.btnCerrarMetodoPago.Size = new System.Drawing.Size(70, 39);
            this.btnCerrarMetodoPago.TabIndex = 3;
            this.btnCerrarMetodoPago.Text = "Cerrar";
            this.btnCerrarMetodoPago.UseVisualStyleBackColor = false;
            this.btnCerrarMetodoPago.Click += new System.EventHandler(this.btnCerrarMetodoPago_Click);
            // 
            // btnLimpiarMetodoPago
            // 
            this.btnLimpiarMetodoPago.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnLimpiarMetodoPago.ForeColor = System.Drawing.Color.White;
            this.btnLimpiarMetodoPago.Location = new System.Drawing.Point(199, 19);
            this.btnLimpiarMetodoPago.Name = "btnLimpiarMetodoPago";
            this.btnLimpiarMetodoPago.Size = new System.Drawing.Size(70, 39);
            this.btnLimpiarMetodoPago.TabIndex = 2;
            this.btnLimpiarMetodoPago.Text = "Limpiar";
            this.btnLimpiarMetodoPago.UseVisualStyleBackColor = false;
            this.btnLimpiarMetodoPago.Click += new System.EventHandler(this.btnLimpiarMetodoPago_Click);
            // 
            // btnAnularMetodoPago
            // 
            this.btnAnularMetodoPago.BackColor = System.Drawing.Color.Red;
            this.btnAnularMetodoPago.ForeColor = System.Drawing.Color.White;
            this.btnAnularMetodoPago.Location = new System.Drawing.Point(123, 19);
            this.btnAnularMetodoPago.Name = "btnAnularMetodoPago";
            this.btnAnularMetodoPago.Size = new System.Drawing.Size(70, 39);
            this.btnAnularMetodoPago.TabIndex = 1;
            this.btnAnularMetodoPago.Text = "Anular";
            this.btnAnularMetodoPago.UseVisualStyleBackColor = false;
            this.btnAnularMetodoPago.Click += new System.EventHandler(this.btnAnularMetodoPago_Click);
            // 
            // btnNuevoMetodoPago
            // 
            this.btnNuevoMetodoPago.BackColor = System.Drawing.Color.Blue;
            this.btnNuevoMetodoPago.ForeColor = System.Drawing.Color.White;
            this.btnNuevoMetodoPago.Location = new System.Drawing.Point(47, 19);
            this.btnNuevoMetodoPago.Name = "btnNuevoMetodoPago";
            this.btnNuevoMetodoPago.Size = new System.Drawing.Size(70, 39);
            this.btnNuevoMetodoPago.TabIndex = 0;
            this.btnNuevoMetodoPago.Text = "Nuevo";
            this.btnNuevoMetodoPago.UseVisualStyleBackColor = false;
            this.btnNuevoMetodoPago.Click += new System.EventHandler(this.btnNuevoMetodoPago_Click);
            // 
            // Grb_DatoMetodoPago
            // 
            this.Grb_DatoMetodoPago.Controls.Add(this.cmbEstado);
            this.Grb_DatoMetodoPago.Controls.Add(this.lblEstaMetodoPago);
            this.Grb_DatoMetodoPago.Controls.Add(this.lblDescrMetodoPago);
            this.Grb_DatoMetodoPago.Controls.Add(this.txtDescripcion);
            this.Grb_DatoMetodoPago.Controls.Add(this.lblCodigoMetodoPago);
            this.Grb_DatoMetodoPago.Controls.Add(this.txtCodigo);
            this.Grb_DatoMetodoPago.Enabled = false;
            this.Grb_DatoMetodoPago.Location = new System.Drawing.Point(17, 19);
            this.Grb_DatoMetodoPago.Name = "Grb_DatoMetodoPago";
            this.Grb_DatoMetodoPago.Size = new System.Drawing.Size(386, 171);
            this.Grb_DatoMetodoPago.TabIndex = 3;
            this.Grb_DatoMetodoPago.TabStop = false;
            this.Grb_DatoMetodoPago.Text = "Datos del Registro";
            // 
            // cmbEstado
            // 
            this.cmbEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEstado.Enabled = false;
            this.cmbEstado.FormattingEnabled = true;
            this.cmbEstado.Items.AddRange(new object[] {
            "ACTIVO",
            "INACTIVO"});
            this.cmbEstado.Location = new System.Drawing.Point(101, 120);
            this.cmbEstado.Name = "cmbEstado";
            this.cmbEstado.Size = new System.Drawing.Size(107, 21);
            this.cmbEstado.TabIndex = 10;
            // 
            // lblEstaMetodoPago
            // 
            this.lblEstaMetodoPago.AutoSize = true;
            this.lblEstaMetodoPago.BackColor = System.Drawing.Color.Transparent;
            this.lblEstaMetodoPago.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEstaMetodoPago.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblEstaMetodoPago.Location = new System.Drawing.Point(17, 121);
            this.lblEstaMetodoPago.Name = "lblEstaMetodoPago";
            this.lblEstaMetodoPago.Size = new System.Drawing.Size(48, 15);
            this.lblEstaMetodoPago.TabIndex = 7;
            this.lblEstaMetodoPago.Text = "Estado:";
            // 
            // lblDescrMetodoPago
            // 
            this.lblDescrMetodoPago.AutoSize = true;
            this.lblDescrMetodoPago.BackColor = System.Drawing.Color.Transparent;
            this.lblDescrMetodoPago.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescrMetodoPago.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblDescrMetodoPago.Location = new System.Drawing.Point(15, 67);
            this.lblDescrMetodoPago.Name = "lblDescrMetodoPago";
            this.lblDescrMetodoPago.Size = new System.Drawing.Size(75, 15);
            this.lblDescrMetodoPago.TabIndex = 5;
            this.lblDescrMetodoPago.Text = "Descripción:";
            // 
            // txtDescripcion
            // 
            this.txtDescripcion.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtDescripcion.Location = new System.Drawing.Point(101, 66);
            this.txtDescripcion.MaxLength = 20;
            this.txtDescripcion.Multiline = true;
            this.txtDescripcion.Name = "txtDescripcion";
            this.txtDescripcion.Size = new System.Drawing.Size(267, 48);
            this.txtDescripcion.TabIndex = 4;
            // 
            // lblCodigoMetodoPago
            // 
            this.lblCodigoMetodoPago.AutoSize = true;
            this.lblCodigoMetodoPago.BackColor = System.Drawing.Color.Transparent;
            this.lblCodigoMetodoPago.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCodigoMetodoPago.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblCodigoMetodoPago.Location = new System.Drawing.Point(16, 42);
            this.lblCodigoMetodoPago.Name = "lblCodigoMetodoPago";
            this.lblCodigoMetodoPago.Size = new System.Drawing.Size(49, 15);
            this.lblCodigoMetodoPago.TabIndex = 3;
            this.lblCodigoMetodoPago.Text = "Código:";
            // 
            // txtCodigo
            // 
            this.txtCodigo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtCodigo.Location = new System.Drawing.Point(101, 40);
            this.txtCodigo.MaxLength = 20;
            this.txtCodigo.Name = "txtCodigo";
            this.txtCodigo.Size = new System.Drawing.Size(107, 20);
            this.txtCodigo.TabIndex = 2;
            this.txtCodigo.Leave += new System.EventHandler(this.txtCodigoCajero_Leave);
            // 
            // FInformacionMetodoPago
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.ClientSize = new System.Drawing.Size(839, 338);
            this.Controls.Add(this.tabCon_MetodoPago);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FInformacionMetodoPago";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Módulo de Configuración de Métodos de Pago";
            this.Load += new System.EventHandler(this.FInformacionMetodoPago_Load);
            this.tabCon_MetodoPago.ResumeLayout(false);
            this.tabPag_MetodoPago.ResumeLayout(false);
            this.Grb_listReMetodoPago.ResumeLayout(false);
            this.Grb_listReMetodoPago.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDatos)).EndInit();
            this.Grb_opcioMetodoPago.ResumeLayout(false);
            this.Grb_DatoMetodoPago.ResumeLayout(false);
            this.Grb_DatoMetodoPago.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabCon_MetodoPago;
        private System.Windows.Forms.TabPage tabPag_MetodoPago;
        private System.Windows.Forms.GroupBox Grb_listReMetodoPago;
        private System.Windows.Forms.Button btnBuscarMetodoPago;
        private System.Windows.Forms.TextBox txtBuscar;
        private System.Windows.Forms.DataGridView dgvDatos;
        private System.Windows.Forms.GroupBox Grb_opcioMetodoPago;
        private System.Windows.Forms.Button btnCerrarMetodoPago;
        private System.Windows.Forms.Button btnLimpiarMetodoPago;
        private System.Windows.Forms.Button btnAnularMetodoPago;
        private System.Windows.Forms.Button btnNuevoMetodoPago;
        private System.Windows.Forms.GroupBox Grb_DatoMetodoPago;
        private System.Windows.Forms.ComboBox cmbEstado;
        private System.Windows.Forms.Label lblEstaMetodoPago;
        private System.Windows.Forms.Label lblDescrMetodoPago;
        private System.Windows.Forms.TextBox txtDescripcion;
        private System.Windows.Forms.Label lblCodigoMetodoPago;
        private System.Windows.Forms.TextBox txtCodigo;
    }
}