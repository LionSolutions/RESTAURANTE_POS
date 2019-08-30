﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Palatium.Facturador
{
    public partial class frmEditarDatosClienteFactura : Form
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        VentanasMensajes.frmMensajeOK ok = new VentanasMensajes.frmMensajeOK();
        VentanasMensajes.frmMensajeCatch catchMensaje = new VentanasMensajes.frmMensajeCatch();

        Clases.ClaseValidarRUC validarRuc = new Clases.ClaseValidarRUC();
        ValidarCedula validarCedula = new ValidarCedula();

        Clases.ClaseFacturaTextBox factura = new Clases.ClaseFacturaTextBox();
        Clases.ClaseReporteFactura2 factura2 = new Clases.ClaseReporteFactura2();
        Clases.ClaseCrearImpresion imprimir = new Clases.ClaseCrearImpresion();
        Clases.ClaseNotaVenta notaVenta = new Clases.ClaseNotaVenta();
        Clases_Factura_Electronica.ClaseReporteFacturaElectronica facturaElectronica = new Clases_Factura_Electronica.ClaseReporteFacturaElectronica();

        string sSql;
        string sRetorno;
        string sIdOrden;
        string sTexto;
        string sCiudad;
        string sAutorizacion;
        string sNombreImpresora;        
        string sPuertoImpresora;
        string sIpImpresora;
        string sDescripcionImpresora;

        bool bRespuesta;

        DataTable dtConsulta;
        DataTable dtPago;
        DataTable dtImprimir;

        int iIdOrden;
        int iRetorno;
        int iIdPersona;
        int idTipoIdentificacion;
        int idTipoPersona;
        int iTercerDigito;
        int iIdFactura;
        int iCantidadImpresiones;
        int iCortarPapel;
        int iAbrirCajon;
        int iIdTipoComprobante;

        public frmEditarDatosClienteFactura()
        {
            InitializeComponent();
        }

        #region FUNCIONES DE CONTROL DE BOTONES

        //INGRESAR EL CURSOR AL BOTON
        private void ingresaBoton(Button btnProceso)
        {
            btnProceso.BackgroundImage = Properties.Resources.boton_cambio;
            btnProceso.BackgroundImageLayout = ImageLayout.Stretch;
            btnProceso.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btnProceso.FlatStyle = FlatStyle.Flat;
            btnProceso.ForeColor = Color.Black;

        }

        //SALIR EL CURSOR DEL BOTON
        private void salidaBoton(Button btnProceso)
        {
            btnProceso.BackgroundImage = Properties.Resources.boton;
            btnProceso.BackgroundImageLayout = ImageLayout.Stretch;
            btnProceso.ForeColor = Color.White;
        }

        #endregion


        #region FUNCIONES DEL USUARIO

        //FUNCION ACTIVA TECLADO
        private void activaTeclado()
        {
            //this.TecladoVirtual.SetShowTouchKeyboard(this.txtBuscar, DevComponents.DotNetBar.Keyboard.TouchKeyboardStyle.Floating);
        }

        //FUNCION PARA VERIFICAR SI YA ESTÁ EMITIDA UNA FACTURA EN UNA ORDEN
        private int validarPedido()
        {
            try
            {
                sSql = "";
                sSql = sSql + "select top 1 NCP.id_pedido, F.id_persona, TP.identificacion," + Environment.NewLine;
                sSql = sSql + conexion.GFun_St_esnulo() + "(F.autorizacion, 0) autorizacion, F.id_factura, F.idtipocomprobante" + Environment.NewLine;
                sSql = sSql + "from cv403_numero_cab_pedido NCP, cv403_facturas_pedidos FP," + Environment.NewLine;
                sSql = sSql + "cv403_facturas F, tp_personas TP" + Environment.NewLine;
                sSql = sSql + "where FP.id_pedido = NCP.id_pedido" + Environment.NewLine;
                sSql = sSql + "and FP.id_factura = F.id_factura" + Environment.NewLine;
                sSql = sSql + "and F.id_persona = TP.id_persona" + Environment.NewLine;
                sSql = sSql + "and FP.estado = 'A'" + Environment.NewLine;
                sSql = sSql + "and NCP.estado = 'A'" + Environment.NewLine;
                sSql = sSql + "and TP.estado = 'A'" + Environment.NewLine;
                sSql = sSql + "and F.estado = 'A'" + Environment.NewLine;
                sSql = sSql + "and NCP.numero_pedido = " + Convert.ToInt32(txtBuscar.Text.Trim()) + Environment.NewLine;
                sSql = sSql + "order by NCP.id_numero_cab_pedido desc";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        iIdOrden = Convert.ToInt32(dtConsulta.Rows[0].ItemArray[0].ToString());
                        iIdPersona = Convert.ToInt32(dtConsulta.Rows[0].ItemArray[1].ToString());
                        txtIdentificacion.Text = dtConsulta.Rows[0].ItemArray[2].ToString();
                        sAutorizacion = dtConsulta.Rows[0].ItemArray[3].ToString();
                        iIdFactura = Convert.ToInt32(dtConsulta.Rows[0].ItemArray[4].ToString());
                        iIdTipoComprobante = Convert.ToInt32(dtConsulta.Rows[0].ItemArray[5].ToString());
                        return 1;
                    }

                    else
                    {
                        return 0;
                    }
                }

                else
                {
                    return 0;
                }

            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowInTaskbar = false;
                catchMensaje.ShowDialog();
                return 0;
            }
        }

        //FUNCION PARA VALIDAR LA FACTURA
        private int validarFacturaNotaVenta(int iOp)
        {
            try
            {
                sSql = "";
                sSql += "select FP.id_pedido, F.id_persona, TP.identificacion," + Environment.NewLine;
                sSql += conexion.GFun_St_esnulo() + "(F.autorizacion, 0) autorizacion, F.id_factura, F.idtipocomprobante" + Environment.NewLine;
                sSql += "from cv403_numeros_facturas NF, cv403_facturas_pedidos FP," + Environment.NewLine;
                sSql += "cv403_facturas F, tp_personas TP" + Environment.NewLine;
                sSql += "where NF.id_factura = FP.id_factura" + Environment.NewLine;
                sSql += "and FP.id_factura = F.id_factura" + Environment.NewLine;
                sSql += "and F.id_persona = TP.id_persona" + Environment.NewLine;
                sSql += "and NF.estado = 'A'" + Environment.NewLine;
                sSql += "and FP.estado = 'A'" + Environment.NewLine;
                sSql += "and F.estado = 'A'" + Environment.NewLine;
                sSql += "and TP.estado = 'A'" + Environment.NewLine;
                sSql += "and NF.numero_factura = " + Convert.ToInt32(txtBuscar.Text.Trim()) + Environment.NewLine;
                sSql += "and F.idtipocomprobante = " + iOp;

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        iIdOrden = Convert.ToInt32(dtConsulta.Rows[0].ItemArray[0].ToString());
                        iIdPersona = Convert.ToInt32(dtConsulta.Rows[0].ItemArray[1].ToString());
                        txtIdentificacion.Text = dtConsulta.Rows[0].ItemArray[2].ToString();
                        sAutorizacion = dtConsulta.Rows[0].ItemArray[3].ToString();
                        iIdFactura = Convert.ToInt32(dtConsulta.Rows[0].ItemArray[4].ToString());
                        iIdTipoComprobante = Convert.ToInt32(dtConsulta.Rows[0].ItemArray[5].ToString());
                        return 1;
                    }

                    else
                    {
                        return 0;
                    }
                }

                else
                {
                    return 0;
                }

            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowInTaskbar = false;
                catchMensaje.ShowDialog();
                return 0;
            }
        }
        #endregion

        #region FUNCIONES PARA CARGAR DATOS DE LA FACTURA

        //FUNCION PARA CARGAR LA FACTURA EN UN TEXTBOX
        private void verFacturaTextBox()
        {
            try
            {

                if (llenarDataTable(1) == true)
                {
                    if (dtConsulta.Rows[0].ItemArray[63].ToString() == "1")
                    {
                        if (Program.iFacturacionElectronica == 1)
                        {
                            sRetorno = facturaElectronica.llenarFactura(dtConsulta, dtPago);
                        }

                        else
                        {
                            if (Program.iFormatoFactura == 1)
                            {
                                sRetorno = factura.llenarFactura(dtConsulta, sIdOrden, "Pagada", dtPago);
                            }

                            else if (Program.iFormatoFactura == 2)
                            {
                                sRetorno = factura2.llenarFactura(dtConsulta, sIdOrden, "Pagada", dtPago);
                                sRetorno = sRetorno + "".PadLeft(22, ' ') + "TOTAL:" + factura2.dTotal.ToString("N2").PadLeft(12, ' ');
                            }
                        }
                    }

                    else if (dtConsulta.Rows[0].ItemArray[63].ToString() == "2")
                    {
                        sRetorno = notaVenta.llenarNota(dtConsulta, sIdOrden, "Pagada");
                        sRetorno = sRetorno + "".PadLeft(22, ' ') + "TOTAL:" + notaVenta.dbTotal.ToString("N2").PadLeft(12, ' ');
                    }

                    if (sRetorno == "")
                    {
                        goto reversa;
                    }
                    else
                    {
                        sTexto = sTexto + Environment.NewLine;
                        sTexto = sTexto + sRetorno;
                    }

                    txtReporte.Text = sTexto;
                    sTexto = "";
                    goto fin;
                }

                else
                {
                    goto reversa;
                }
            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
                goto fin;
            }

        reversa:
            {
                ok.LblMensaje.Text = "Ocurrió un problema al realizar la consulta.";
                ok.ShowDialog();
            }

        fin: { }
        }

        //FUNCION PARA LLENAR LOS DATATABLES
        private bool llenarDataTable(int op)
        {
            try
            {
                //OPCION CERO   : PARA PRECUENTA
                //OPCION UNO    : PARA FACTURA

                if (op == 0)
                {
                    sSql = "";
                    sSql = sSql + "select * from pos_vw_det_pedido" + Environment.NewLine;
                    sSql = sSql + "where id_pedido = " + Convert.ToInt32(sIdOrden) + Environment.NewLine;
                    sSql = sSql + "and estado in ('A', 'N')" + Environment.NewLine;
                    sSql = sSql + "order by id_det_pedido";
                }

                else
                {
                    sSql = "";
                    sSql = sSql + "select * from pos_vw_factura" + Environment.NewLine;
                    sSql = sSql + "where id_pedido = " + Convert.ToInt32(sIdOrden) + Environment.NewLine;
                    sSql = sSql + "order by id_det_pedido";
                }

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        sSql = "";
                        sSql = sSql + "select descripcion, sum(valor) valor, cambio,  count(*) cuenta," + Environment.NewLine;
                        sSql = sSql + "sum(isnull(valor_recibido, valor)) valor_recibido" + Environment.NewLine;
                        sSql = sSql + "from pos_vw_pedido_forma_pago" + Environment.NewLine;
                        sSql = sSql + "where id_pedido = " + Convert.ToInt32(sIdOrden) + Environment.NewLine;
                        sSql = sSql + "group by descripcion, valor, cambio, valor_recibido" + Environment.NewLine;
                        sSql = sSql + "having count(*) >= 1";

                        dtPago = new DataTable();
                        dtPago.Clear();

                        bRespuesta = conexion.GFun_Lo_Busca_Registro(dtPago, sSql);

                        if (bRespuesta == true)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                        return false;
                }

                else
                {
                    return false;
                }
            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowInTaskbar = false;
                catchMensaje.ShowDialog();
                return false;
            }
        }


        //CONSULTAR DATOS EN LA BASE
        private void consultarRegistro()
        {
            try
            {
                sSql = "";
                sSql = sSql + "SELECT TP.id_persona, TP.identificacion, TP.nombres, TP.apellidos, TP.correo_electronico," + Environment.NewLine;
                sSql = sSql + "TD.direccion + ', ' + TD.calle_principal + ' ' + TD.numero_vivienda + ' ' + TD.calle_interseccion direccion_cliente," + Environment.NewLine;
                sSql = sSql + "TT.domicilio, TT.celular, TD.direccion, TP.codigo_alterno" + Environment.NewLine;
                sSql = sSql + "FROM dbo.tp_personas TP" + Environment.NewLine;
                sSql = sSql + "LEFT OUTER JOIN dbo.tp_direcciones TD ON TP.id_persona = TD.id_persona" + Environment.NewLine;
                sSql = sSql + "and TP.estado = 'A'" + Environment.NewLine;
                sSql = sSql + "and TD.estado = 'A'" + Environment.NewLine;
                sSql = sSql + "LEFT OUTER JOIN dbo.tp_telefonos TT ON TP.id_persona = TT.id_persona" + Environment.NewLine;
                sSql = sSql + "and TT.estado = 'A'" + Environment.NewLine;
                sSql = sSql + "WHERE TP.identificacion = '" + txtIdentificacion.Text.Trim() + "'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        iIdPersona = Convert.ToInt32(dtConsulta.Rows[0].ItemArray[0].ToString());
                        txtNombres.Text = dtConsulta.Rows[0].ItemArray[2].ToString();
                        txtApellidos.Text = dtConsulta.Rows[0].ItemArray[3].ToString();
                        txtMail.Text = dtConsulta.Rows[0].ItemArray[4].ToString();
                        txtDireccion.Text = dtConsulta.Rows[0].ItemArray[5].ToString();
                        sCiudad = dtConsulta.Rows[0].ItemArray[8].ToString();

                        if (dtConsulta.Rows[0].ItemArray[6].ToString() != "")
                        {
                            txtTelefono.Text = dtConsulta.Rows[0].ItemArray[6].ToString();
                        }

                        else if (dtConsulta.Rows[0].ItemArray[7].ToString() != "")
                        {
                            txtTelefono.Text = dtConsulta.Rows[0].ItemArray[7].ToString();
                        }

                        else
                        {
                            txtTelefono.Text = dtConsulta.Rows[0].ItemArray[9].ToString();
                        }

                        btnGuardar.Enabled = true;
                        btnGuardar.Focus();

                    }

                    else
                    {
                        frmNuevoCliente nuevoCliente = new frmNuevoCliente(txtIdentificacion.Text.Trim(), chkPasaporte.Checked);
                        nuevoCliente.ShowInTaskbar = false;
                        nuevoCliente.ShowDialog();

                        if (nuevoCliente.DialogResult == DialogResult.OK)
                        {
                            iIdPersona = nuevoCliente.iCodigo;
                            txtIdentificacion.Text = nuevoCliente.sIdentificacion;
                            txtNombres.Text = nuevoCliente.sNombre;
                            txtApellidos.Text = nuevoCliente.sApellido;
                            txtTelefono.Text = nuevoCliente.sTelefono;
                            txtDireccion.Text = nuevoCliente.sDireccion;
                            txtMail.Text = nuevoCliente.sMail;
                            sCiudad = nuevoCliente.sCiudad;
                            nuevoCliente.Close();
                            btnGuardar.Enabled = true;
                            btnGuardar.Focus();
                        }
                    }

                    btnEditar.Visible = true;
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


        //VERIFICAR SI LA CADENA ES UN NUMERO O UN STRING
        public bool esNumero(object Expression)
        {

            bool isNum;

            double retNum;

            isNum = Double.TryParse(Convert.ToString(Expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);

            return isNum;

        }


        //FUNCION PARA VALIDAR LA CEDULA O RUC
        private void validarIdentificacion()
        {
            try
            {
                if (txtIdentificacion.Text.Length >= 10)
                {
                    iTercerDigito = Convert.ToInt32(txtIdentificacion.Text.Substring(2, 1));
                }
                else
                {
                    goto mensaje;
                }

                if (txtIdentificacion.Text.Length == 10)
                {
                    if (validarCedula.validarCedulaConsulta(txtIdentificacion.Text.Trim()) == "SI")
                    {
                        //CONSULTAR EN LA BASE DE DATOS
                        consultarRegistro();
                        goto fin;
                    }

                    else
                    {
                        goto mensaje;
                    }
                }

                else if (txtIdentificacion.Text.Length == 13)
                {
                    if (iTercerDigito == 9)
                    {
                        if (validarRuc.validarRucPrivado(txtIdentificacion.Text.Trim()) == true)
                        {
                            //CONSULTAR EN LA BASE DE DATOS
                            consultarRegistro();
                            goto fin;
                        }

                        else
                        {
                            goto mensaje;
                        }

                    }

                    else if (iTercerDigito == 6)
                    {
                        if (validarRuc.validarRucPublico(txtIdentificacion.Text.Trim()) == true)
                        {
                            //CONSULTAR EN LA BASE DE DATOS
                            consultarRegistro();
                            goto fin;
                        }

                        else
                        {
                            goto mensaje;
                        }
                    }

                    else if ((iTercerDigito <= 5) || (iTercerDigito >= 0))
                    {
                        if (validarRuc.validarRucNatural(txtIdentificacion.Text.Trim()) == true)
                        {
                            //CONSULTAR EN LA BASE DE DATOS
                            consultarRegistro();
                            goto fin;
                        }

                        else
                        {
                            goto mensaje;
                        }
                    }

                    else
                    {
                        goto mensaje;
                    }
                }

                else
                {
                    goto mensaje;
                }
            }

            catch (Exception)
            {

            }

        mensaje:
            {
                ok.LblMensaje.Text = "El número de identificación ingresado es incorrecto.";
                ok.ShowDialog();
                btnGuardar.Enabled = false;
                txtIdentificacion.Clear();
                txtIdentificacion.Focus();
            }
        fin:
            { }
        }

        //FUNCION PARA LIMPIAR
        private void limpiar()
        {
            txtBuscar.Clear();
            txtIdentificacion.Clear();
            txtApellidos.Clear();
            txtNombres.Clear();
            txtDireccion.Clear();
            txtTelefono.Clear();
            txtMail.Clear();
            txtReporte.Clear();

            rbdTicket.Checked = true;
            chkEditar.Checked = false;
            btnGuardar.Enabled = false;
            btnImprimir.Enabled = false;

            grupoCliente.Enabled = false;

            btnMostrar.Visible = false;
            btnOcultar.Visible = false;
            this.Width = 684;
            centrarFormulario();

            iIdOrden = 0;
            iIdPersona = 0;

            txtBuscar.Focus();
        }

        //CAMBIAR TAMAÑO DE FORMULARIO Y CENTRAR
        private void centrarFormulario()
        {
            try
            {
                int boundWidth = Screen.PrimaryScreen.Bounds.Width;
                int boundHeight = Screen.PrimaryScreen.Bounds.Height;
                int x = boundWidth - this.Width;
                int y = boundHeight - this.Height;
                this.Location = new Point(x / 2, y / 2);
            }

            catch(Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
            }
        }


        //FUNCION PARA ACTUALIZAR EL CLIENTE EN LA FACTURA
        private void actualizarCliente()
        {
            try
            {
                //INICIAMOS UNA NUEVA TRANSACCION
                //=======================================================================================================
                if (!conexion.GFun_Lo_Maneja_Transaccion(Program.G_INICIA_TRANSACCION))
                {
                    ok.LblMensaje.Text = "Error al abrir transacción";
                    ok.ShowDialog();
                    goto fin;
                }

                //INSTRUCCION SQL PARA ACTUALIZAR EL CLIENTE EN LA TABLA CV403_CAB_PEDIDOS
                sSql = "";
                sSql += "update cv403_cab_pedidos set" + Environment.NewLine;
                sSql += "id_persona = " + iIdPersona + Environment.NewLine;
                sSql += "where id_pedido = " + iIdOrden + Environment.NewLine;
                sSql += "and estado = 'A'";

                //EJECUCION DE INSTRUCCION SQL
                if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                {
                    catchMensaje.LblMensaje.Text = sSql;
                    catchMensaje.ShowDialog();
                    goto reversa;
                }

                //INSTRUCCION SQL PARA ACTUALIZAR EL CLIENTE EN LA TABLA CV403_FACTURAS
                sSql = "";
                sSql += "update cv403_facturas set" + Environment.NewLine;
                sSql += "id_persona = " + iIdPersona + Environment.NewLine;
                sSql += "where id_factura = " + iIdFactura + Environment.NewLine;
                sSql += "and estado = 'A'";

                //EJECUCION DE INSTRUCCION SQL
                if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                {
                    catchMensaje.LblMensaje.Text = sSql;
                    catchMensaje.ShowDialog();
                    goto reversa;
                }

                conexion.GFun_Lo_Maneja_Transaccion(Program.G_TERMINA_TRANSACCION);
                ok.LblMensaje.Text = "Factura actualizada éxitosamente.";
                ok.ShowDialog();
                verFacturaTextBox();

                goto fin;
            }

            catch(Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
                goto reversa;
            }

            reversa:
            {
                conexion.GFun_Lo_Maneja_Transaccion(Program.G_REVERSA_TRANSACCION);
            }

            fin: { }
        }

        #endregion

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            if (txtBuscar.Text.Trim() == "")
            {
                if (rbdTicket.Checked == true)
                {
                    ok.LblMensaje.Text = "Favor ingrese el número de la orden.";
                }

                else if (rbdFactura.Checked == true)
                {
                    ok.LblMensaje.Text = "Favor ingrese el número de la factura.";
                }

                else if (rbdNotaVenta.Checked == true)
                {
                    ok.LblMensaje.Text = "Favor ingrese el número de la nota de venta.";
                }

                ok.ShowInTaskbar = false;
                ok.ShowDialog();
            }

            else
            {
                if (rbdTicket.Checked == true)
                {
                    iRetorno = validarPedido();
                }

                else if (rbdFactura.Checked == true)
                {
                    iRetorno = validarFacturaNotaVenta(1);
                }

                else if (rbdNotaVenta.Checked == true)
                {
                    iRetorno = validarFacturaNotaVenta(2);
                }

                sIdOrden = iIdOrden.ToString();

                if (iRetorno == 1)
                {
                    verFacturaTextBox();
                    consultarRegistro();

                    if (sAutorizacion == "0")
                    {
                        chkEditar.Enabled = true;
                        chkEditar.Checked = false;
                        btnGuardar.Enabled = true;
                        btnImprimir.Enabled = true;
                    }

                    else
                    {
                        chkEditar.Enabled = false;
                        chkEditar.Checked = false;
                        btnGuardar.Enabled = false;
                        btnImprimir.Enabled = true;
                        ok.LblMensaje.Text = "La factura ya se encuentra registrada en el SRI.";
                        ok.ShowDialog();
                    }

                    this.Width = 684;
                    btnOcultar.Visible = false;
                    btnMostrar.Visible = true;
                    centrarFormulario();
                }

                else
                {
                    ok.LblMensaje.Text = "No existen registros con los datos proporcionados";
                    ok.ShowDialog();
                    txtBuscar.Clear();

                    this.Width = 684;
                    btnOcultar.Visible = false;
                    btnMostrar.Visible = false;
                    this.StartPosition = FormStartPosition.CenterScreen;
                    
                    txtBuscar.Focus();
                }
            }
        }

        private void rbdTicket_CheckedChanged(object sender, EventArgs e)
        {
            if (rbdTicket.Checked == true)
            {
                grupoDatos.Text = "Búsqueda por Número de Ticket";
                txtBuscar.Clear();
                txtReporte.Clear();
                txtIdentificacion.Clear();
                txtApellidos.Clear();
                txtNombres.Clear();
                txtDireccion.Clear();
                txtTelefono.Clear();
                txtMail.Clear();
                txtBuscar.Focus();
            }
        }

        private void rbdFactura_CheckedChanged(object sender, EventArgs e)
        {
            if (rbdFactura.Checked == true)
            {
                grupoDatos.Text = "Búsqueda por Número de Factura";
                txtBuscar.Clear();
                txtReporte.Clear();
                txtIdentificacion.Clear();
                txtApellidos.Clear();
                txtNombres.Clear();
                txtDireccion.Clear();
                txtTelefono.Clear();
                txtMail.Clear();
                txtBuscar.Focus();
            }
        }

        private void txtBuscar_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (Char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (Char.IsSeparator(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }

            if (e.KeyChar == (char)Keys.Enter)
            {
                btnBuscar_Click(sender, e);
            }
        }

        private void frmEditarDatosClienteFactura_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void chkEditar_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEditar.Checked == true)
            {
                grupoCliente.Enabled = true;
                chkEditar.Text = "Inhabilitar edición";
                txtIdentificacion.Focus();
            }

            else
            {
                grupoCliente.Enabled = false;
                chkEditar.Text = "Habilitar Edición";
                txtBuscar.Focus();
            }
        }

        private void btnConsumidorFinal_Click(object sender, EventArgs e)
        {
            txtIdentificacion.Text = "9999999999999";
            txtApellidos.Text = "CONSUMIDOR FINAL";
            txtNombres.Text = "CONSUMIDOR FINAL";
            txtTelefono.Text = "9999999999";
            txtMail.Text = "dominio@dominio.com";
            txtDireccion.Text = "QUITO";
            iIdPersona = Program.iIdPersona;
            idTipoIdentificacion = 180;
            idTipoPersona = 2447;
            btnGuardar.Enabled = true;
            btnEditar.Visible = false;
            btnGuardar.Focus();
        }

        private void btnEditar_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmNuevoCliente nuevoCliente = new frmNuevoCliente(txtIdentificacion.Text.Trim(), chkPasaporte.Checked);
            nuevoCliente.ShowInTaskbar = false;
            nuevoCliente.ShowDialog();

            if (nuevoCliente.DialogResult == DialogResult.OK)
            {
                iIdPersona = nuevoCliente.iCodigo;
                txtIdentificacion.Text = nuevoCliente.sIdentificacion;
                consultarRegistro();
            }
        }

        private void chkPasaporte_CheckedChanged(object sender, EventArgs e)
        {
            txtIdentificacion.Focus();
        }

        private void txtIdentificacion_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (txtIdentificacion.Text != "")
                {
                    //AQUI INSTRUCCIONES PARA CONSULTAR Y VALIDAR LA CEDULA
                    if ((esNumero(txtIdentificacion.Text.Trim()) == true) && (chkPasaporte.Checked == false))
                    {
                        //INSTRUCCIONES PARA VALIDAR
                        validarIdentificacion();
                    }
                    else
                    {
                        //CONSULTAR EN LA BASE DE DATOS
                        consultarRegistro();
                    }
                }
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
           limpiar();
        }

        private void btnBuscarCliente_Click(object sender, EventArgs e)
        {
            frmControlDatosCliente controlClientes = new frmControlDatosCliente();
            controlClientes.ShowInTaskbar = false;
            controlClientes.ShowDialog();

            if (controlClientes.DialogResult == DialogResult.OK)
            {
                iIdPersona = controlClientes.iCodigo;
                txtIdentificacion.Text = controlClientes.sIdentificacion;
                consultarRegistro();
                btnGuardar.Focus();
                controlClientes.Close();
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if ((txtIdentificacion.Text == "") && (txtApellidos.Text == ""))
            {
                ok.LblMensaje.Text = "Favor ingrese los datos del cliente para la factura.";
                ok.ShowInTaskbar = false;
                ok.ShowDialog();
            }
            else
            {
                actualizarCliente();
            }
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            if (iIdTipoComprobante == 1)
            {
                Pedidos.frmVerFacturaTextBox factura = new Pedidos.frmVerFacturaTextBox(sIdOrden, Program.iVistaPreviaImpresiones);
                factura.ShowDialog();

                if (factura.DialogResult == DialogResult.OK)
                {
                    factura.Close();
                }
            }

            else if (iIdTipoComprobante == 2)
            {
                ReportesTextBox.frmVerNotaVenta notaVenta = new ReportesTextBox.frmVerNotaVenta(sIdOrden, 1);
                notaVenta.ShowDialog();

                if (notaVenta.DialogResult == DialogResult.OK)
                {
                    notaVenta.Close();
                }
            }
        }

        private void frmEditarDatosClienteFactura_Load(object sender, EventArgs e)
        {
            if (Program.iManejaNotaVenta == 1)
            {
                rbdNotaVenta.Visible = true;
            }

            else
            {
                rbdNotaVenta.Visible = false;
            }

            limpiar();
            this.ActiveControl = txtBuscar;
        }

        private void rbdNotaVenta_CheckedChanged(object sender, EventArgs e)
        {
            if (rbdNotaVenta.Checked == true)
            {
                grupoDatos.Text = "Búsqueda por Número de Nota de Venta";
                txtBuscar.Clear();
                txtReporte.Clear();
                txtIdentificacion.Clear();
                txtApellidos.Clear();
                txtNombres.Clear();
                txtDireccion.Clear();
                txtTelefono.Clear();
                txtMail.Clear();
                txtBuscar.Focus();
            }
        }

        private void btnOcultar_Click(object sender, EventArgs e)
        {
            this.Width = 684;
            btnOcultar.Visible = false;
            btnMostrar.Visible = true;
            centrarFormulario();
        }

        private void btnMostrar_Click(object sender, EventArgs e)
        {
            this.Width = 1054;
            btnOcultar.Visible = true;
            btnMostrar.Visible = false;
            centrarFormulario();
        }

        private void btnGuardar_MouseEnter(object sender, EventArgs e)
        {
            ingresaBoton(btnGuardar);
        }

        private void btnGuardar_MouseLeave(object sender, EventArgs e)
        {
            salidaBoton(btnGuardar);
        }

        private void btnLimpiar_MouseEnter(object sender, EventArgs e)
        {
            ingresaBoton(btnLimpiar);
        }

        private void btnLimpiar_MouseLeave(object sender, EventArgs e)
        {
            salidaBoton(btnLimpiar);
        }

        private void btnImprimir_MouseEnter(object sender, EventArgs e)
        {
            ingresaBoton(btnImprimir);
        }

        private void btnImprimir_MouseLeave(object sender, EventArgs e)
        {
            salidaBoton(btnImprimir);
        }

        private void btnSalir_MouseEnter(object sender, EventArgs e)
        {
            ingresaBoton(btnSalir);
        }

        private void btnSalir_MouseLeave(object sender, EventArgs e)
        {
            salidaBoton(btnSalir);
        }
    }
}
