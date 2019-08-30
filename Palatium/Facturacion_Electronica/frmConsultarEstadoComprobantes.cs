using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Palatium.Facturacion_Electronica
{
    public partial class frmConsultarEstadoComprobantes : Form
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        VentanasMensajes.frmMensajeCatch catchMensaje = new VentanasMensajes.frmMensajeCatch();
        VentanasMensajes.frmMensajeOK ok = new VentanasMensajes.frmMensajeOK();
        VentanasMensajes.frmMensajeSiNo SiNo = new VentanasMensajes.frmMensajeSiNo();

        Clases_Factura_Electronica.ClaseConsultarXML consultar = new Clases_Factura_Electronica.ClaseConsultarXML();

        string sSql;
        string sFechaInicial;
        string sFechaFinal;
        string sIdTipoAmbiente;
        string sIdTipoEmision;
        string sEstadoAutorizacion;
        string sErrorAutorizacion;
        string sNumeroComprobante;

        int iNumeroRegistros;

        DataTable dtConsulta;

        bool bRespuesta;

        XmlDocument xmlAut;

        public frmConsultarEstadoComprobantes()
        {
            InitializeComponent();
        }

        #region FUNCIONES DEL USUARIO

        //FUNCION PARA SABER SI HAY CONEXION A INTERNET
        private bool conexionInternet()
        {
            try
            {
                IPHostEntry host = Dns.GetHostEntry("www.google.com");
                return true;
            }

            catch (Exception ex)
            {
                return false;
            }
        }

        ///FUNCION PARA LIMPIAR
        private void limpiar()
        {
            LLenarComboEmpresa();
            LLenarComboLocalidad();
            LLenarComboVendedor();
            LLenarComboMoneda();

            chkSeleccionar.Checked = false;
            chkSeleccionar.Text = "Seleccionar todos los registros";

            txtFechaInicial.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtFechaFinal.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }

        //LLENAR EL COMBO DE EMPRESA
        private void LLenarComboEmpresa()
        {
            try
            {
                dtConsulta = new DataTable();
                dtConsulta.Clear();

                sSql = "";
                sSql += "select idempresa, case when nombrecomercial in ('', null) then" + Environment.NewLine;
                sSql += "razonsocial else nombrecomercial end nombre_comercial, *" + Environment.NewLine;
                sSql += "from sis_empresa" + Environment.NewLine;
                sSql += "where idempresa = " + Program.iIdEmpresa;

                cmbEmpresa.llenar(dtConsulta, sSql);

                if (cmbEmpresa.Items.Count >= 1)
                {
                    cmbEmpresa.SelectedIndex = 1;
                }
            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowInTaskbar = false;
                catchMensaje.ShowDialog();
            }
        }

        //LLENAR EL COMBO DE LOCALIDADES
        private void LLenarComboLocalidad()
        {
            try
            {
                dtConsulta = new DataTable();
                dtConsulta.Clear();

                sSql = "";
                sSql = sSql + "select id_localidad,nombre_localidad" + Environment.NewLine;
                sSql = sSql + "from tp_vw_localidades" + Environment.NewLine;
                sSql = sSql + "where id_localidad = " + Program.iIdLocalidad;

                cmbLocalidad.llenar(dtConsulta, sSql);

                if (cmbLocalidad.Items.Count >= 1)
                {
                    cmbLocalidad.SelectedIndex = 1;
                }
            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowInTaskbar = false;
                catchMensaje.ShowDialog();
            }
        }

        //LLENAR EL COMBO DE VENDEDORES
        private void LLenarComboVendedor()
        {
            try
            {
                dtConsulta = new DataTable();
                dtConsulta.Clear();

                sSql = "";
                sSql = sSql + "select id_vendedor, descripcion" + Environment.NewLine;
                sSql = sSql + "from cv403_vendedores";

                cmbVendedor.llenar(dtConsulta, sSql);

                if (cmbVendedor.Items.Count >= 1)
                {
                    cmbVendedor.SelectedIndex = 1;
                }
            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowInTaskbar = false;
                catchMensaje.ShowDialog();
            }
        }

        //LLENAR EL COMBO DE MONEDAS
        private void LLenarComboMoneda()
        {
            try
            {
                dtConsulta = new DataTable();
                dtConsulta.Clear();

                sSql = "";
                sSql = sSql + "select * from tp_vw_moneda";

                cmbMoneda.llenar(dtConsulta, sSql);

                if (cmbMoneda.Items.Count >= 1)
                {
                    cmbMoneda.SelectedIndex = 1;
                }
            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowInTaskbar = false;
                catchMensaje.ShowDialog();
            }
        }

        //FUNCION PARA CONSULTAR EL TIPO DE AMBIENTE CONFIGURADO EN EL SISTEMA
        private void consultarTipoAmbiente()
        {
            try
            {
                sSql = "";
                sSql = sSql + "select TA.codigo" + Environment.NewLine;
                sSql = sSql + "from sis_empresa E,cel_tipo_ambiente TA" + Environment.NewLine;
                sSql = sSql + "where E.id_tipo_ambiente = TA.id_tipo_ambiente" + Environment.NewLine;
                sSql = sSql + "and E.estado = 'A'" + Environment.NewLine;
                sSql = sSql + "and TA.estado = 'A'" + Environment.NewLine;
                sSql = sSql + "order By TA.codigo";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        sIdTipoAmbiente = dtConsulta.Rows[0].ItemArray[0].ToString();
                    }

                    else
                    {
                        ok.LblMensaje.Text = "No se encuentra información de configuración del Tipo de Ambiente";
                        ok.ShowDialog();
                    }
                }

                else
                {
                    catchMensaje.LblMensaje.Text = sSql;
                    catchMensaje.ShowDialog();
                }
            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
            }
        }

        //FUNCION PARA CONSULTAR EL TIPO DE EMISION CONFIGURADO EN EL SISTEMA
        private void consultarTipoEmision()
        {
            try
            {
                sSql = "";
                sSql = sSql + "select TE.codigo" + Environment.NewLine;
                sSql = sSql + "from sis_empresa E,cel_tipo_emision TE" + Environment.NewLine;
                sSql = sSql + "where E.id_tipo_emision = TE.id_tipo_emision" + Environment.NewLine;
                sSql = sSql + "and E.estado = 'A'" + Environment.NewLine;
                sSql = sSql + "and TE.estado = 'A'" + Environment.NewLine;
                sSql = sSql + "order By TE.codigo";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        sIdTipoEmision = dtConsulta.Rows[0].ItemArray[0].ToString();
                    }

                    else
                    {
                        ok.LblMensaje.Text = "No se encuentra información de configuración del Tipo de Emisión";
                        ok.ShowDialog();
                    }
                }

                else
                {
                    catchMensaje.LblMensaje.Text = sSql;
                    catchMensaje.ShowDialog();
                }
            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
            }
        }


        //FUNCION PARA LLENAR EL GRID
        private void llenarGrid()
        {
            try
            {
                sFechaInicial = Convert.ToDateTime(txtFechaInicial.Text.Trim()).ToString("yyyy/MM/dd");
                sFechaFinal = Convert.ToDateTime(txtFechaFinal.Text.Trim()).ToString("yyyy/MM/dd");

                dgvDatos.Rows.Clear();

                sSql = "";
                sSql = sSql + "select F.id_factura, VL.nombre_localidad, F.fecha_factura, VL.establecimiento, VL.punto_emision," + Environment.NewLine;
                sSql = sSql + "NF.numero_factura, ltrim((isnull(P.nombres, '') + ' ' + P.apellidos)) cliente, F.clave_acceso" + Environment.NewLine;
                sSql = sSql + "from cv403_facturas F, cv403_numeros_facturas NF, tp_personas P, tp_vw_localidades VL" + Environment.NewLine;
                sSql = sSql + "where NF.id_factura = F.id_factura" + Environment.NewLine;
                sSql = sSql + "and F.id_localidad = VL.id_localidad" + Environment.NewLine;
                sSql = sSql + "and F.id_persona = P.id_persona" + Environment.NewLine;
                sSql = sSql + "and VL.id_localidad = " + Program.iIdLocalidad + Environment.NewLine;
                sSql = sSql + "and F.estado = 'A'" + Environment.NewLine;
                sSql = sSql + "and NF.estado = 'A'" + Environment.NewLine;
                sSql = sSql + "and P.estado = 'A'" + Environment.NewLine;
                sSql = sSql + "and F.fecha_factura between '" + sFechaInicial + "'" + Environment.NewLine;
                sSql = sSql + "and '" + sFechaFinal + "'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {

                        for (int i = 0; i < dtConsulta.Rows.Count; i++)
                        {
                            sNumeroComprobante = dtConsulta.Rows[i].ItemArray[3].ToString() + "-" + dtConsulta.Rows[i].ItemArray[4].ToString() + "-" + dtConsulta.Rows[i].ItemArray[5].ToString().PadLeft(9, '0');

                            dgvDatos.Rows.Add(
                                                false,
                                                dtConsulta.Rows[i].ItemArray[0].ToString(),
                                                dtConsulta.Rows[i].ItemArray[1].ToString(),
                                                "FAC",
                                                Convert.ToDateTime(dtConsulta.Rows[i].ItemArray[2].ToString()).ToString("dd/MM/yyyy"),
                                                sNumeroComprobante,
                                                dtConsulta.Rows[i].ItemArray[6].ToString().Trim(),
                                                dtConsulta.Rows[i].ItemArray[7].ToString(),
                                                "",
                                                ""
                                );
                        }

                        lblCuentaRegistros.Text = dgvDatos.Rows.Count.ToString() + " Registros Encontrados.";
                    }

                    else
                    {
                        dgvDatos.Rows.Clear();
                        lblCuentaRegistros.Text = dgvDatos.Rows.Count.ToString() + " Registros Encontrados.";
                    }
                }

                else
                {
                    catchMensaje.LblMensaje.Text = sSql;
                    catchMensaje.ShowDialog();
                    lblCuentaRegistros.Text = dgvDatos.Rows.Count.ToString() + " Registros Encontrados.";
                }

            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
                lblCuentaRegistros.Text = dgvDatos.Rows.Count.ToString() + " Registros Encontrados.";
            }
        }


        //INSTRUCCIONES PARA CONSULTAR EL ESTADO DEL XML DEL SRI
        private bool consultarArchivoXML(string sClaveAcceso, int iFila_P)
        {
            try
            {
                sErrorAutorizacion = "";

                if (sClaveAcceso != "")
                {
                    RespuestaSRI respuesta = consultar.AutorizacionComprobante(out xmlAut, sClaveAcceso);
                    sEstadoAutorizacion = respuesta.Estado;
                    sErrorAutorizacion = respuesta.ErrorMensaje;

                    if (respuesta.Estado == "AUTORIZADO")
                    {
                        dgvDatos.Rows[iFila_P].Cells["colEstado"].Style.BackColor = Color.Lime;
                    }

                    else
                    {                        
                        dgvDatos.Rows[iFila_P].Cells["colEstado"].Style.BackColor = Color.Red;
                    }

                    return true;
                }

                else
                {
                    return false;
                }
            }

            catch (Exception ex)
            {
                //catchMensaje.LblMensaje.Text = ex.ToString();
                //catchMensaje.ShowDialog();
                return false;
            }
        }


        #endregion

        private void frmConsultarEstadoComprobantes_Load(object sender, EventArgs e)
        {
            consultarTipoAmbiente();
            consultarTipoEmision();
            limpiar();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            llenarGrid();
        }

        private void btnInicial_Click(object sender, EventArgs e)
        {
            Pedidos.frmCalendario calendario = new Pedidos.frmCalendario(txtFechaInicial.Text.Trim());
            calendario.ShowInTaskbar = false;
            calendario.ShowDialog();

            if (calendario.DialogResult == DialogResult.OK)
            {
                txtFechaInicial.Text = calendario.txtFecha.Text;

                if (Convert.ToDateTime(txtFechaInicial.Text) > Convert.ToDateTime(txtFechaFinal.Text))
                {
                    ok.LblMensaje.Text = "La fecha inicial no puede ser superior a la ficha final del rango.";
                    ok.ShowDialog();
                    txtFechaInicial.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtFechaFinal.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
            }
        }

        private void btnFinal_Click(object sender, EventArgs e)
        {
            Pedidos.frmCalendario calendario = new Pedidos.frmCalendario(txtFechaFinal.Text.Trim());
            calendario.ShowInTaskbar = false;
            calendario.ShowDialog();

            if (calendario.DialogResult == DialogResult.OK)
            {
                txtFechaFinal.Text = calendario.txtFecha.Text;

                if (Convert.ToDateTime(txtFechaInicial.Text) > Convert.ToDateTime(txtFechaFinal.Text))
                {
                    ok.LblMensaje.Text = "La fecha inicial no puede ser superior a la ficha final del rango.";
                    ok.ShowDialog();
                    txtFechaInicial.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtFechaFinal.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
            }
        }

        private async void btnSincronizar_Click(object sender, EventArgs e)
        {
            iNumeroRegistros = 0;

            Task<bool> task;
            bool bConsulta;

            for (int i = 0; i < dgvDatos.Rows.Count; i++)
            {
                if (Convert.ToBoolean(dgvDatos.Rows[i].Cells["colMarca"].Value) == true)
                {
                    iNumeroRegistros++;
                }
            }

            if (dgvDatos.Rows.Count == 0)
            {
                ok.LblMensaje.Text = "No existen comprobantes electrónicos para procesar.";
                ok.ShowDialog();
                goto fin;
            }

            else if (iNumeroRegistros == 0)
            {
                ok.LblMensaje.Text = "No hay registros seleccionados para procesar la información.";
                ok.ShowDialog();
                goto fin;
            }

            else
            {
                if (conexionInternet() == false)
                {
                    ok.LblMensaje.Text = "No hay una conexión a internet. Favor verifique la conectividad.";
                    ok.ShowDialog();
                    goto fin;
                }

                else
                {
                    SiNo.LblMensaje.Text = "¿Desea procesar los comprobantes electrónicos emitidos?";
                    SiNo.ShowDialog();

                    if (SiNo.DialogResult == DialogResult.OK)
                    {
                        this.Cursor = Cursors.WaitCursor;

                        //AQUI SE INICIA EL PROCESO DE SINCRONIZACION CON EL SRI
                        for (int i = 0; i < dgvDatos.Rows.Count; i++)
                        {
                            if (Convert.ToBoolean(dgvDatos.Rows[i].Cells["colMarca"].Value) == true)
                            {
                                task = new Task<bool>(() => consultarArchivoXML(dgvDatos.Rows[i].Cells["colClaveAcceso"].Value.ToString(), i));
                                task.Start();
                                dgvDatos.Rows[i].Cells["colEstado"].Value = "Consultando autorización SRI...";
                                bConsulta = await task;

                                if (bConsulta == true)
                                {
                                    dgvDatos.Rows[i].Cells["colEstado"].Value = sEstadoAutorizacion;
                                    dgvDatos.Rows[i].Cells["colMensaje"].Value = sErrorAutorizacion;
                                    dgvDatos.Rows[i].Cells["colEstado"].Style.BackColor = Color.Yellow;
                                }

                                else
                                {
                                    dgvDatos.Rows[i].Cells["colEstado"].Value = "Error consultando SRI.";
                                    dgvDatos.Rows[i].Cells["colEstado"].Style.BackColor = Color.Red;
                                    goto salir;
                                }
                            }

                        salir:
                            {
                                dgvDatos.Rows[i].Cells["colMarca"].Value = false;
                            }
                        }

                        this.Cursor = Cursors.Default;
                        chkSeleccionar.Checked = false;
                        chkSeleccionar.Text = "Seleccionar todos los registros";
                    }
                }
            }

        fin: { };
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            dgvDatos.Rows.Clear();
            lblCuentaRegistros.Text = dgvDatos.Rows.Count.ToString() + " Registros Encontrados.";
            limpiar();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void chkSeleccionar_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSeleccionar.Checked == true)
            {
                chkSeleccionar.Text = "Quitar selección de todos los registros";
                for (int i = 0; i < dgvDatos.Rows.Count; i++)
                {
                    dgvDatos.Rows[i].Cells["colMarca"].Value = true;
                }

            }

            else
            {
                chkSeleccionar.Text = "Seleccionar todos los registros";
                for (int i = 0; i < dgvDatos.Rows.Count; i++)
                {
                    dgvDatos.Rows[i].Cells["colMarca"].Value = false;
                }
            }
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            ok.LblMensaje.Text = "Módulo en desarrollo.";
            ok.ShowDialog();
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            ok.LblMensaje.Text = "Módulo en desarrollo.";
            ok.ShowDialog();
        }
    }
}
