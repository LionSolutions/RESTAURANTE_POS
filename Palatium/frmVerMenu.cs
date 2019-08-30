using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Palatium
{
    public partial class frmVerMenu : Form
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        Clases.ClaseCrearImpresion imprimir = new Clases.ClaseCrearImpresion();
        Clases.ClaseAbrirCajon abrir = new Clases.ClaseAbrirCajon();
        VentanasMensajes.frmMensajeOK ok = new VentanasMensajes.frmMensajeOK();
        VentanasMensajes.frmMensajeCatch catchMensaje = new VentanasMensajes.frmMensajeCatch();

        public string sEtiqueta;

        //VARIABLES DE CONFIGURACION DE LA IMPRESORA
        DataTable dtImprimir;
        bool bRespuesta;

        int iCortarPapel;
        int iAbrirCajon;
        int iCantidadImpresiones;

        string sSql;
        string sNombreImpresora;        
        string sPuertoImpresora;
        string sIpImpresora;
        string sDescripcionImpresora;

        public frmVerMenu()
        {
            //Thread t = new Thread(new ThreadStart(cargando));
            //t.Start();

            InitializeComponent();

            //for (int i = 0; i < 800; i++)
            //{
            //    Thread.Sleep(5);
            //}
            //t.Abort();
        }

        void cargando()
        {
            SplashScreen.frmSplashScreen frm = new SplashScreen.frmSplashScreen();
            Application.Run(frm);
        }

        #region FUNCIONES DEL USUARIO

        //EXTRAER LOS DATOS LAS IMPRESORAS
        private void consultarImpresoraTipoOrden()
        {
            try
            {
                sSql = "";
                sSql = sSql + "select I.path_url, I.numero_impresion, I.puerto_impresora," + Environment.NewLine;
                sSql = sSql + "I.ip_impresora, I.descripcion, I.cortar_papel, I.abrir_cajon" + Environment.NewLine;
                sSql = sSql + "from pos_impresora I, pos_formato_factura FF" + Environment.NewLine;
                sSql = sSql + "where FF.id_pos_impresora = I.id_pos_impresora" + Environment.NewLine;
                sSql = sSql + "and FF.estado = 'A'" + Environment.NewLine;
                sSql = sSql + "and I.estado = 'A'" + Environment.NewLine;
                sSql = sSql + "and FF.id_pos_formato_factura = " + Program.iFormatoFactura;

                dtImprimir = new DataTable();
                dtImprimir.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtImprimir, sSql);

                if (bRespuesta == true)
                {
                    if (dtImprimir.Rows.Count > 0)
                    {
                        sNombreImpresora = dtImprimir.Rows[0].ItemArray[0].ToString();
                        iCantidadImpresiones = Convert.ToInt32(dtImprimir.Rows[0].ItemArray[1].ToString());
                        sPuertoImpresora = dtImprimir.Rows[0].ItemArray[2].ToString();
                        sIpImpresora = dtImprimir.Rows[0].ItemArray[3].ToString();
                        sDescripcionImpresora = dtImprimir.Rows[0].ItemArray[4].ToString();
                        iCortarPapel = Convert.ToInt32(dtImprimir.Rows[0].ItemArray[5].ToString());
                        iAbrirCajon = Convert.ToInt32(dtImprimir.Rows[0].ItemArray[6].ToString());

                        //ENVIAR A IMPRIMIR
                        imprimir.iniciarImpresion();
                        imprimir.AbreCajon();
                        imprimir.imprimirReporte(sNombreImpresora);
                    }

                    else
                    {
                        ok.LblMensaje.Text = "No existe el registro de configuración de impresora. Comuníquese con el administrador.";
                        ok.ShowInTaskbar = false;
                        ok.ShowDialog();
                    }
                }

                else
                {
                    ok.LblMensaje.Text = "Ocurrió un problema al realizar la consulta.";
                    ok.ShowInTaskbar = false;
                    ok.ShowDialog();
                }
            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowInTaskbar = false;
                catchMensaje.ShowDialog();
            }
        }

        //CARGAR MENU PRINCIPAL
        private void cargarFormulario()
        {
            //Form1 menu = new Form1();
            Menú.frmMenuPos menu = new Menú.frmMenuPos();

            //menu.AutoSize = true;
            //menu.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            //menu.WindowState = FormWindowState.Maximized;
            //menu.StartPosition = FormStartPosition.CenterParent;
            //menu.Dock = DockStyle.Fill;

            menu.MdiParent = this;
            menu.Show();
        }

        public void cambiarEtiqueta(string sDato)
        {
            this.sEtiqueta = sDato;
            this.Text = sEtiqueta;
            this.Refresh();
        }

        #endregion

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }

        private void frmVerMenu_Load(object sender, EventArgs e)
        {
            MdiClient ctlMDI;
            // Loop through all of the form's controls looking
            // for the control of type MdiClient.
            foreach (Control ctl in this.Controls)
            {
                try
                {
                    // Attempt to cast the control to type MdiClient.
                    ctlMDI = (MdiClient)ctl;

                    // Set the BackColor of the MdiClient control.
                    ctlMDI.BackColor = this.BackColor;
                }
                catch (InvalidCastException exc)
                {
                    // Catch and ignore the error if casting failed.
                }
            }

            cargarFormulario();
            Program.iPuedeCobrar = 1;
        }

        private void frmVerMenu_KeyDown(object sender, KeyEventArgs e)
        {
            if (Program.iPermitirAbrirCajon == 1)
            {
                if (e.KeyCode == Keys.F7)
                {
                    if (Program.iPuedeCobrar == 1)
                    {
                        abrir.consultarImpresoraAbrirCajon();
                    }
                }
            }
        }
    }
}
