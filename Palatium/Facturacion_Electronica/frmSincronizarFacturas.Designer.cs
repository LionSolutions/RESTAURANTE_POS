﻿namespace Palatium.Facturacion_Electronica
{
    partial class frmSincronizarFacturas
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
            this.dgvDatos = new System.Windows.Forms.DataGridView();
            this.colMarca = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colIdFactura = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLocalidad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTipo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFecha = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEstablecimiento = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPtoEmision = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNumeroComprobante = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCliente = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEstado = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMensaje = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnFinal = new System.Windows.Forms.Button();
            this.btnInicial = new System.Windows.Forms.Button();
            this.txtFechaFinal = new System.Windows.Forms.TextBox();
            this.txtFechaInicial = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbMoneda = new MisControles.ComboDatos();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbVendedor = new MisControles.ComboDatos();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbLocalidad = new MisControles.ComboDatos();
            this.cmbEmpresa = new MisControles.ComboDatos();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblCuentaRegistros = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.chkSeleccionar = new System.Windows.Forms.CheckBox();
            this.btnExcel = new System.Windows.Forms.Button();
            this.btnImprimir = new System.Windows.Forms.Button();
            this.btnLimpiar = new System.Windows.Forms.Button();
            this.btnSincronizar = new System.Windows.Forms.Button();
            this.btnSalir = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnCerrar = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDatos)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvDatos
            // 
            this.dgvDatos.AllowUserToAddRows = false;
            this.dgvDatos.AllowUserToDeleteRows = false;
            this.dgvDatos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDatos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMarca,
            this.colIdFactura,
            this.colLocalidad,
            this.colTipo,
            this.colFecha,
            this.colEstablecimiento,
            this.colPtoEmision,
            this.colNumeroComprobante,
            this.colCliente,
            this.colMail,
            this.colEstado,
            this.colMensaje});
            this.dgvDatos.Location = new System.Drawing.Point(12, 151);
            this.dgvDatos.Name = "dgvDatos";
            this.dgvDatos.Size = new System.Drawing.Size(1023, 320);
            this.dgvDatos.TabIndex = 0;
            // 
            // colMarca
            // 
            this.colMarca.Frozen = true;
            this.colMarca.HeaderText = "Marca";
            this.colMarca.Name = "colMarca";
            this.colMarca.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colMarca.Width = 50;
            // 
            // colIdFactura
            // 
            this.colIdFactura.Frozen = true;
            this.colIdFactura.HeaderText = "ID";
            this.colIdFactura.Name = "colIdFactura";
            this.colIdFactura.ReadOnly = true;
            this.colIdFactura.Visible = false;
            // 
            // colLocalidad
            // 
            this.colLocalidad.Frozen = true;
            this.colLocalidad.HeaderText = "Localidad";
            this.colLocalidad.Name = "colLocalidad";
            // 
            // colTipo
            // 
            this.colTipo.Frozen = true;
            this.colTipo.HeaderText = "Doc";
            this.colTipo.Name = "colTipo";
            this.colTipo.Width = 50;
            // 
            // colFecha
            // 
            this.colFecha.Frozen = true;
            this.colFecha.HeaderText = "Fecha";
            this.colFecha.Name = "colFecha";
            this.colFecha.Width = 75;
            // 
            // colEstablecimiento
            // 
            this.colEstablecimiento.Frozen = true;
            this.colEstablecimiento.HeaderText = "Est.";
            this.colEstablecimiento.Name = "colEstablecimiento";
            this.colEstablecimiento.Width = 50;
            // 
            // colPtoEmision
            // 
            this.colPtoEmision.Frozen = true;
            this.colPtoEmision.HeaderText = "Pto. Vta.";
            this.colPtoEmision.Name = "colPtoEmision";
            this.colPtoEmision.Width = 50;
            // 
            // colNumeroComprobante
            // 
            this.colNumeroComprobante.Frozen = true;
            this.colNumeroComprobante.HeaderText = "Comprobante";
            this.colNumeroComprobante.Name = "colNumeroComprobante";
            this.colNumeroComprobante.Width = 65;
            // 
            // colCliente
            // 
            this.colCliente.Frozen = true;
            this.colCliente.HeaderText = "Cliente";
            this.colCliente.Name = "colCliente";
            this.colCliente.Width = 170;
            // 
            // colMail
            // 
            this.colMail.Frozen = true;
            this.colMail.HeaderText = "CorreoElectrónico";
            this.colMail.Name = "colMail";
            this.colMail.Width = 140;
            // 
            // colEstado
            // 
            this.colEstado.Frozen = true;
            this.colEstado.HeaderText = "Estado";
            this.colEstado.Name = "colEstado";
            this.colEstado.Width = 140;
            // 
            // colMensaje
            // 
            this.colMensaje.Frozen = true;
            this.colMensaje.HeaderText = "Mensaje";
            this.colMensaje.Name = "colMensaje";
            this.colMensaje.Width = 150;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnOK);
            this.groupBox1.Controls.Add(this.btnFinal);
            this.groupBox1.Controls.Add(this.btnInicial);
            this.groupBox1.Controls.Add(this.txtFechaFinal);
            this.groupBox1.Controls.Add(this.txtFechaInicial);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.cmbMoneda);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.cmbVendedor);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.cmbLocalidad);
            this.groupBox1.Controls.Add(this.cmbEmpresa);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 47);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1023, 85);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // btnOK
            // 
            this.btnOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.Image = global::Palatium.Properties.Resources.ok4;
            this.btnOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOK.Location = new System.Drawing.Point(936, 31);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(81, 30);
            this.btnOK.TabIndex = 14;
            this.btnOK.Text = "Extraer";
            this.btnOK.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnFinal
            // 
            this.btnFinal.Location = new System.Drawing.Point(894, 37);
            this.btnFinal.Name = "btnFinal";
            this.btnFinal.Size = new System.Drawing.Size(21, 20);
            this.btnFinal.TabIndex = 13;
            this.btnFinal.Text = "...";
            this.btnFinal.UseVisualStyleBackColor = true;
            this.btnFinal.Click += new System.EventHandler(this.btnFinal_Click);
            // 
            // btnInicial
            // 
            this.btnInicial.Location = new System.Drawing.Point(755, 37);
            this.btnInicial.Name = "btnInicial";
            this.btnInicial.Size = new System.Drawing.Size(21, 20);
            this.btnInicial.TabIndex = 12;
            this.btnInicial.Text = "...";
            this.btnInicial.UseVisualStyleBackColor = true;
            this.btnInicial.Click += new System.EventHandler(this.btnInicial_Click);
            // 
            // txtFechaFinal
            // 
            this.txtFechaFinal.Location = new System.Drawing.Point(788, 37);
            this.txtFechaFinal.Name = "txtFechaFinal";
            this.txtFechaFinal.ReadOnly = true;
            this.txtFechaFinal.Size = new System.Drawing.Size(106, 20);
            this.txtFechaFinal.TabIndex = 11;
            // 
            // txtFechaInicial
            // 
            this.txtFechaInicial.Location = new System.Drawing.Point(661, 37);
            this.txtFechaInicial.Name = "txtFechaInicial";
            this.txtFechaInicial.ReadOnly = true;
            this.txtFechaInicial.Size = new System.Drawing.Size(94, 20);
            this.txtFechaInicial.TabIndex = 10;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label6.Location = new System.Drawing.Point(787, 17);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(78, 16);
            this.label6.TabIndex = 9;
            this.label6.Text = "Fecha Final";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label5.Location = new System.Drawing.Point(658, 17);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 16);
            this.label5.TabIndex = 8;
            this.label5.Text = "Fecha Inical";
            // 
            // cmbMoneda
            // 
            this.cmbMoneda.FormattingEnabled = true;
            this.cmbMoneda.Location = new System.Drawing.Point(540, 37);
            this.cmbMoneda.Name = "cmbMoneda";
            this.cmbMoneda.Size = new System.Drawing.Size(105, 21);
            this.cmbMoneda.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label4.Location = new System.Drawing.Point(537, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 16);
            this.label4.TabIndex = 6;
            this.label4.Text = "Moneda";
            // 
            // cmbVendedor
            // 
            this.cmbVendedor.FormattingEnabled = true;
            this.cmbVendedor.Location = new System.Drawing.Point(386, 37);
            this.cmbVendedor.Name = "cmbVendedor";
            this.cmbVendedor.Size = new System.Drawing.Size(126, 21);
            this.cmbVendedor.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label3.Location = new System.Drawing.Point(383, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 16);
            this.label3.TabIndex = 4;
            this.label3.Text = "Vendedor";
            // 
            // cmbLocalidad
            // 
            this.cmbLocalidad.FormattingEnabled = true;
            this.cmbLocalidad.Location = new System.Drawing.Point(205, 37);
            this.cmbLocalidad.Name = "cmbLocalidad";
            this.cmbLocalidad.Size = new System.Drawing.Size(158, 21);
            this.cmbLocalidad.TabIndex = 3;
            // 
            // cmbEmpresa
            // 
            this.cmbEmpresa.FormattingEnabled = true;
            this.cmbEmpresa.Location = new System.Drawing.Point(20, 37);
            this.cmbEmpresa.Name = "cmbEmpresa";
            this.cmbEmpresa.Size = new System.Drawing.Size(158, 21);
            this.cmbEmpresa.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label2.Location = new System.Drawing.Point(202, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "Localidad";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label1.Location = new System.Drawing.Point(17, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Empresa";
            // 
            // lblCuentaRegistros
            // 
            this.lblCuentaRegistros.AutoSize = true;
            this.lblCuentaRegistros.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCuentaRegistros.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblCuentaRegistros.Location = new System.Drawing.Point(137, 487);
            this.lblCuentaRegistros.Name = "lblCuentaRegistros";
            this.lblCuentaRegistros.Size = new System.Drawing.Size(178, 16);
            this.lblCuentaRegistros.TabIndex = 133;
            this.lblCuentaRegistros.Text = "0 Registros Encontrados";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label7.Location = new System.Drawing.Point(12, 487);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(119, 16);
            this.label7.TabIndex = 132;
            this.label7.Text = "Total Registros:";
            // 
            // chkSeleccionar
            // 
            this.chkSeleccionar.AutoSize = true;
            this.chkSeleccionar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSeleccionar.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.chkSeleccionar.Location = new System.Drawing.Point(12, 510);
            this.chkSeleccionar.Name = "chkSeleccionar";
            this.chkSeleccionar.Size = new System.Drawing.Size(212, 20);
            this.chkSeleccionar.TabIndex = 139;
            this.chkSeleccionar.Text = "Seleccionar todos los registros";
            this.chkSeleccionar.UseVisualStyleBackColor = true;
            this.chkSeleccionar.CheckedChanged += new System.EventHandler(this.chkSeleccionar_CheckedChanged);
            // 
            // btnExcel
            // 
            this.btnExcel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExcel.Image = global::Palatium.Properties.Resources.excel_png1;
            this.btnExcel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExcel.Location = new System.Drawing.Point(835, 487);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(94, 43);
            this.btnExcel.TabIndex = 138;
            this.btnExcel.Text = "Excel";
            this.btnExcel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExcel.UseVisualStyleBackColor = true;
            this.btnExcel.Click += new System.EventHandler(this.btnExcel_Click);
            // 
            // btnImprimir
            // 
            this.btnImprimir.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImprimir.Image = global::Palatium.Properties.Resources.impresora_png;
            this.btnImprimir.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnImprimir.Location = new System.Drawing.Point(733, 487);
            this.btnImprimir.Name = "btnImprimir";
            this.btnImprimir.Size = new System.Drawing.Size(96, 43);
            this.btnImprimir.TabIndex = 137;
            this.btnImprimir.Text = "Imprimir";
            this.btnImprimir.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnImprimir.UseVisualStyleBackColor = true;
            this.btnImprimir.Click += new System.EventHandler(this.btnImprimir_Click);
            // 
            // btnLimpiar
            // 
            this.btnLimpiar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLimpiar.Image = global::Palatium.Properties.Resources.limpiar_ico;
            this.btnLimpiar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLimpiar.Location = new System.Drawing.Point(631, 487);
            this.btnLimpiar.Name = "btnLimpiar";
            this.btnLimpiar.Size = new System.Drawing.Size(96, 43);
            this.btnLimpiar.TabIndex = 136;
            this.btnLimpiar.Text = "Limpiar";
            this.btnLimpiar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnLimpiar.UseVisualStyleBackColor = true;
            this.btnLimpiar.Click += new System.EventHandler(this.btnLimpiar_Click);
            // 
            // btnSincronizar
            // 
            this.btnSincronizar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSincronizar.Image = global::Palatium.Properties.Resources.sincronizar_png;
            this.btnSincronizar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSincronizar.Location = new System.Drawing.Point(380, 477);
            this.btnSincronizar.Name = "btnSincronizar";
            this.btnSincronizar.Size = new System.Drawing.Size(144, 62);
            this.btnSincronizar.TabIndex = 135;
            this.btnSincronizar.Text = "Sincronizar\r\nSRI";
            this.btnSincronizar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSincronizar.UseVisualStyleBackColor = true;
            this.btnSincronizar.Click += new System.EventHandler(this.btnSincronizar_Click);
            // 
            // btnSalir
            // 
            this.btnSalir.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSalir.Image = global::Palatium.Properties.Resources.salir_ico;
            this.btnSalir.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSalir.Location = new System.Drawing.Point(935, 487);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(94, 43);
            this.btnSalir.TabIndex = 134;
            this.btnSalir.Text = "Salir";
            this.btnSalir.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSalir.UseVisualStyleBackColor = true;
            this.btnSalir.Click += new System.EventHandler(this.btnSalir_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.panel1.Controls.Add(this.btnCerrar);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1047, 40);
            this.panel1.TabIndex = 140;
            // 
            // btnCerrar
            // 
            this.btnCerrar.BackColor = System.Drawing.Color.Red;
            this.btnCerrar.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCerrar.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnCerrar.Location = new System.Drawing.Point(1001, 0);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(46, 40);
            this.btnCerrar.TabIndex = 1;
            this.btnCerrar.Text = "X";
            this.btnCerrar.UseVisualStyleBackColor = false;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label8.Location = new System.Drawing.Point(337, 9);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(372, 20);
            this.label8.TabIndex = 1;
            this.label8.Text = "Sincronización de comprobantes electrónicos";
            // 
            // frmSincronizarFacturas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.Teal;
            this.ClientSize = new System.Drawing.Size(1047, 551);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.chkSeleccionar);
            this.Controls.Add(this.btnExcel);
            this.Controls.Add(this.btnImprimir);
            this.Controls.Add(this.btnLimpiar);
            this.Controls.Add(this.btnSincronizar);
            this.Controls.Add(this.btnSalir);
            this.Controls.Add(this.lblCuentaRegistros);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.dgvDatos);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSincronizarFacturas";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sincronización de comprobantes electrónicos";
            this.Load += new System.EventHandler(this.frmSincronizarFacturas_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmSincronizarFacturas_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDatos)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvDatos;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnFinal;
        private System.Windows.Forms.Button btnInicial;
        private System.Windows.Forms.TextBox txtFechaFinal;
        private System.Windows.Forms.TextBox txtFechaInicial;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private MisControles.ComboDatos cmbMoneda;
        private System.Windows.Forms.Label label4;
        private MisControles.ComboDatos cmbVendedor;
        private System.Windows.Forms.Label label3;
        private MisControles.ComboDatos cmbLocalidad;
        private MisControles.ComboDatos cmbEmpresa;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblCuentaRegistros;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnSalir;
        private System.Windows.Forms.Button btnSincronizar;
        private System.Windows.Forms.Button btnLimpiar;
        private System.Windows.Forms.Button btnImprimir;
        private System.Windows.Forms.Button btnExcel;
        private System.Windows.Forms.CheckBox chkSeleccionar;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colMarca;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIdFactura;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLocalidad;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTipo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFecha;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEstablecimiento;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPtoEmision;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNumeroComprobante;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCliente;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMail;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEstado;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMensaje;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnCerrar;
        private System.Windows.Forms.Label label8;
    }
}