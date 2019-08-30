﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Xml.Linq;
using System.Threading;
using System.Net;
using System.Threading.Tasks;

namespace Palatium.Facturacion_Electronica
{
    public partial class frmSincronizarFacturas : Form
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        VentanasMensajes.frmMensajeCatch catchMensaje = new VentanasMensajes.frmMensajeCatch();
        VentanasMensajes.frmMensajeOK ok = new VentanasMensajes.frmMensajeOK();
        VentanasMensajes.frmMensajeSiNo SiNo = new VentanasMensajes.frmMensajeSiNo();

        Clases_Factura_Electronica.ClaseGenerarFacturaXml generar = new Clases_Factura_Electronica.ClaseGenerarFacturaXml();
        Clases_Factura_Electronica.ClaseFirmarXML firmar = new Clases_Factura_Electronica.ClaseFirmarXML();
        Clases_Factura_Electronica.ClaseEnviarXML enviar = new Clases_Factura_Electronica.ClaseEnviarXML();
        Clases_Factura_Electronica.ClaseConsultarXML consultar = new Clases_Factura_Electronica.ClaseConsultarXML();
        Clases_Factura_Electronica.ClaseEnviarMail correo = new Clases_Factura_Electronica.ClaseEnviarMail();
        Clases_Factura_Electronica.ClaseGenerarRIDE ride = new Clases_Factura_Electronica.ClaseGenerarRIDE();

        DataTable dtConsulta;

        XmlDocument xmlAut;
        XDocument xml;
        XElement autorizacion;

        int iNumeroRegistros;
        int iErrorGenerados;
        int iErrorFirmados;
        int iErrorEnviarSRI;
        int iErrorConsultarXMLSRI;
        int iErrorEnviarMail;
        int iEnviadosExitosamente;

        string sSql;
        string sFechaInicial;
        string sFechaFinal;
        string sDirGenerados;
        string sDirFirmados;
        string sDirAutorizados;
        string sDirNoAutorizados;

        string sArchivoEnviar;
        string sClaveAcceso;
        string filename;
        string miXMl;
        string sVersion = "1.0";
        string sUTF = "utf-8";
        string sStandAlone = "yes";

        string sCodigoDocumento;

        string sIdTipoAmbiente;
        string sIdTipoEmision;

        string sEstadoAutorizacion;

        bool bRespuesta;

        //VARIABLES PARA EL FIRMADO DEL XML
        string sArchivoFirmar;
        string sNumeroDocumento;

        string sJar;
        string sCertificado;
        string sPassCertificado;
        string sXmlPathOut;
        string sFileOut;
        string sCodigoError = "";
        string sDescripcionError = "";
        string[] sCertificado_digital = new string[5];

        //VARIABLES PARA CARGAR LOS PARAMETROS DE ENVIO DEL MAIL
        string P_St_correo_server_smtp;
        string P_St_from;
        string P_St_fromname;
        string P_St_correo_que_envia;
        string P_St_correo_con_copia;
        string P_St_correo_consumidor_final;
        string P_St_correo_ambiente_prueba;
        string P_St_correo_palabra_clave;
        long P_Ln_correo_puerto_smtp;
        int P_In_maneja_SSL;
        string P_St_telefono_empresa;
        string P_St_nombre_comercial;
        string sMensajeEnviar;
        string sTipoComprobante;
        string sMensajeRetorno;
        string sCorreoCliente;
        string sAsuntoMail;
        string srutaXML;
        string srutaRIDE;
        string sRutaAdjuntos;

        bool bRespuestaEnvioMail;


        public frmSincronizarFacturas(string sCodigoDocumento)
        {
            this.sCodigoDocumento = sCodigoDocumento;
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

            catch(Exception ex)
            {
                return false;
            }
        }

        //FUNCION PARA CARGAR LOS PARAMETROS DE ENVIO DEL CORREO CON LOS ARCHIVOS ADJUNTOS
        public void traerparametrosMail()
        {
            try
            {
                P_St_correo_que_envia = "";
                P_Ln_correo_puerto_smtp = 0;

                sSql = "";
                sSql = sSql + "select correo_que_envia,correo_con_copia," + Environment.NewLine;
                sSql = sSql + "correo_consumidor_final,correo_ambiente_prueba,correo_palabra_clave," + Environment.NewLine;
                sSql = sSql + "correo_smtp,correo_puerto, maneja_SSL" + Environment.NewLine;
                sSql = sSql + "from cel_parametro" + Environment.NewLine;
                sSql = sSql + "where estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        P_St_correo_que_envia = conexion.GFun_Va_Valor_Defecto(dtConsulta.Rows[0][0].ToString(), "");
                        P_St_from = P_St_correo_que_envia;
                        P_St_correo_con_copia = conexion.GFun_Va_Valor_Defecto(dtConsulta.Rows[0][1].ToString(), "");
                        P_St_correo_consumidor_final = conexion.GFun_Va_Valor_Defecto(dtConsulta.Rows[0][2].ToString(), "");
                        P_St_correo_ambiente_prueba = conexion.GFun_Va_Valor_Defecto(dtConsulta.Rows[0][3].ToString(), "");
                        P_St_correo_palabra_clave = conexion.GFun_Va_Valor_Defecto(dtConsulta.Rows[0][4].ToString(), "");
                        P_St_correo_server_smtp = conexion.GFun_Va_Valor_Defecto(dtConsulta.Rows[0][5].ToString(), "");
                        P_Ln_correo_puerto_smtp = Convert.ToInt64(conexion.GFun_Va_Valor_Defecto(dtConsulta.Rows[0][6].ToString(), "0"));
                        P_In_maneja_SSL = Convert.ToInt32(conexion.GFun_Va_Valor_Defecto(dtConsulta.Rows[0][7].ToString(), "0"));
                        //P_St_fromname = G_St_Nombre_Empresa;

                    }
                }

                else
                {
                    catchMensaje.LblMensaje.Text = sSql;
                    catchMensaje.ShowDialog();
                    goto fin;
                }

                //==================================================================================================
                sSql = "";
                sSql = sSql + "select telefono, nombrecomercial" + Environment.NewLine;
                sSql = sSql + "from sis_empresa" + Environment.NewLine;
                sSql = sSql + "where idempresa = " + Program.iIdEmpresa;

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        P_St_telefono_empresa = conexion.GFun_Va_Valor_Defecto(dtConsulta.Rows[0][0].ToString(), "");
                        P_St_nombre_comercial = conexion.GFun_Va_Valor_Defecto(dtConsulta.Rows[0][1].ToString(), "");
                    }
                }

                else
                {
                    catchMensaje.LblMensaje.Text = sSql;
                    catchMensaje.ShowDialog();
                    goto fin;
                }
            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
            }

        fin: { }

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
                        sIdTipoAmbiente = dtConsulta.Rows[0][0].ToString();
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
                        sIdTipoEmision = dtConsulta.Rows[0][0].ToString();
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

        //FUNCION PARA CARGAR LOS PARAMETROS PARA EL FIRMADO DE LOS XML
        private void datosCertificado()
        {
            try
            {
                firmar.Gsub_trae_parametros_certificado(sCertificado_digital);
                sCertificado = sCertificado_digital[0];
                sPassCertificado = sCertificado_digital[1];
            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
            }
        }

        //CARGAR EL DIRECTORIO DONDE SE GUARDARAN LOS XML GENERADOS
        private bool buscarDirectorio()
        {
            try
            {
                sSql = "";
                sSql = sSql + "select codigo, nombres" + Environment.NewLine;
                sSql = sSql + "from cel_directorio" + Environment.NewLine;
                sSql = sSql + "where id_tipo_comprobante = 1" + Environment.NewLine;
                sSql = sSql + "and estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        sDirGenerados = dtConsulta.Rows[0][1].ToString();
                        sDirFirmados = dtConsulta.Rows[1][1].ToString();
                        sDirAutorizados = dtConsulta.Rows[2][1].ToString();
                        sDirNoAutorizados = dtConsulta.Rows[3][1].ToString();
                        return true;
                    }

                    else
                    {
                        ok.LblMensaje.Text = "No existe una configuracion de directorio para guardar los xml genereados.";
                        ok.ShowDialog();
                        return false;
                    }
                }

                else
                {
                    catchMensaje.LblMensaje.Text = sSql;
                    catchMensaje.ShowDialog();
                    return false;
                }
            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
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

        //FUNCION PARA LLENAR EL GRID
        private void llenarGrid()
        {
            try
            {
                sFechaInicial = Convert.ToDateTime(txtFechaInicial.Text.Trim()).ToString("yyyy/MM/dd");
                sFechaFinal = Convert.ToDateTime(txtFechaFinal.Text.Trim()).ToString("yyyy/MM/dd");

                dgvDatos.Rows.Clear();
                /*  ACTUALIZACION NUEVA
                    AUTOR: ELVIS GUAIGUA
                    FECHA: 2018/09/18
                */

                sSql = "";
                sSql += "select F.id_factura, VL.nombre_localidad, F.fecha_factura, VL.establecimiento, VL.punto_emision," + Environment.NewLine;
                sSql += "NF.numero_factura, ltrim((isnull(P.nombres, '') + ' ' + P.apellidos)) cliente," + Environment.NewLine;
                sSql += "isnull(P.correo_electronico, '') correo_electronico" + Environment.NewLine;
                sSql += "from cv403_facturas F, cv403_numeros_facturas NF, tp_personas P, tp_vw_localidades VL," + Environment.NewLine;
                sSql += "cv403_facturas_pedidos FP, pos_origen_orden O, cv403_cab_pedidos CP" + Environment.NewLine;
                sSql += "where NF.id_factura = F.id_factura" + Environment.NewLine;
                sSql += "and F.id_localidad = VL.id_localidad" + Environment.NewLine;
                sSql += "and F.id_persona = P.id_persona" + Environment.NewLine;
                sSql += "and FP.id_factura = F.id_factura" + Environment.NewLine;
                sSql += "and FP.id_pedido = CP.id_pedido" + Environment.NewLine;
                sSql += "and CP.id_pos_origen_orden = O.id_pos_origen_orden" + Environment.NewLine;
                sSql += "and VL.id_localidad = " + Program.iIdLocalidad + Environment.NewLine;
                sSql += "and O.repartidor_externo = 0" + Environment.NewLine;
                sSql += "and F.estado = 'A'" + Environment.NewLine;
                sSql += "and NF.estado = 'A'" + Environment.NewLine;
                sSql += "and P.estado = 'A'" + Environment.NewLine;
                sSql += "and FP.estado = 'A'" + Environment.NewLine;
                sSql += "and CP.estado = 'A'" + Environment.NewLine;
                sSql += "and O.estado = 'A'" + Environment.NewLine;
                sSql += "and F.fecha_factura between '" + sFechaInicial + "'" + Environment.NewLine;
                sSql += "and '" + sFechaFinal + "'" + Environment.NewLine;
                sSql += "and F.facturaelectronica = 1" + Environment.NewLine;
                sSql += "group by F.id_factura, VL.nombre_localidad, F.fecha_factura, VL.establecimiento," + Environment.NewLine;
                sSql += "VL.punto_emision, NF.numero_factura, P.nombres, P.apellidos, P.correo_electronico";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {

                        for (int i = 0; i < dtConsulta.Rows.Count; i++)
                        {
                            dgvDatos.Rows.Add(
                                                false,
                                                dtConsulta.Rows[i][0].ToString(),
                                                dtConsulta.Rows[i][1].ToString(),
                                                "FAC",
                                                Convert.ToDateTime(dtConsulta.Rows[i][2].ToString()).ToString("dd/MM/yyyy"),
                                                dtConsulta.Rows[i][3].ToString(),
                                                dtConsulta.Rows[i][4].ToString(),
                                                dtConsulta.Rows[i][5].ToString(),
                                                dtConsulta.Rows[i][6].ToString().Trim(),
                                                dtConsulta.Rows[i][7].ToString(),
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

        //FUNCION PARA CREAR UN INFORME DE FACTURAS ENVIADAS Y RECHAZADAS
        private void enviarInformeFacturas()
        {
            try
            {
                sMensajeEnviar = "";
                sMensajeEnviar = sMensajeEnviar + "INFORME DE ENVÍO DE FACTURAS ELECTRÓNICAS" + Environment.NewLine + Environment.NewLine;
                sMensajeEnviar = sMensajeEnviar + "TOTAL DE FACTURAS PARA PROCESAR: " + iNumeroRegistros.ToString() + Environment.NewLine + Environment.NewLine;
                sMensajeEnviar = sMensajeEnviar + "DETALLE EXITOSO DEL PROCESO DE SINCRONIZACIÓN" + Environment.NewLine;
                sMensajeEnviar = sMensajeEnviar + "- Total de comprobantes electrónicos enviados éxitosamente: " + iEnviadosExitosamente.ToString() + Environment.NewLine + Environment.NewLine;
                sMensajeEnviar = sMensajeEnviar + "DETALLE DE ERRORES DE GENERACION DE FACTURAS ELECTRÓNICAS:" + Environment.NewLine;
                sMensajeEnviar = sMensajeEnviar + "- Documentos al generar el XML: " + iErrorGenerados.ToString() + Environment.NewLine;
                sMensajeEnviar = sMensajeEnviar + "- Documentos al firmar el XML: " + iErrorFirmados.ToString() + Environment.NewLine;
                sMensajeEnviar = sMensajeEnviar + "- Documentos al enviar el XML al SRI: " + iErrorEnviarSRI.ToString() + Environment.NewLine;
                sMensajeEnviar = sMensajeEnviar + "- Documentos al consultar el XML en el SRI: " + iErrorConsultarXMLSRI.ToString() + Environment.NewLine;
                sMensajeEnviar = sMensajeEnviar + "- Documentos al enviar al correo los comprobantes electrónicos: " + iErrorEnviarMail.ToString() + Environment.NewLine;
                sMensajeEnviar = sMensajeEnviar + Environment.NewLine;
                sMensajeEnviar = sMensajeEnviar + "Atentamente, " + Environment.NewLine;
                sMensajeEnviar = sMensajeEnviar + Environment.NewLine;
                sMensajeEnviar = sMensajeEnviar + P_St_nombre_comercial;
                sMensajeEnviar = sMensajeEnviar + Environment.NewLine + Environment.NewLine;

                //ARMANDO LOS PARAMETROS NECESARIOS PARA ENVIAR EL MAIL
                sCorreoCliente = "elvis.geovanni@hotmail.com";
                sAsuntoMail = P_St_nombre_comercial + " - REPORTE DE SINCRONIZACIÓN DE COMPROBANTES ELECTRÓNICOS";
                sRutaAdjuntos = "";

                bRespuestaEnvioMail = correo.enviarCorreo(P_St_correo_server_smtp, Convert.ToInt32(P_Ln_correo_puerto_smtp), P_St_from,
                                    P_St_correo_palabra_clave, P_St_fromname, sCorreoCliente,
                                    P_St_correo_con_copia, P_St_correo_consumidor_final, sAsuntoMail,
                                    sRutaAdjuntos, sMensajeEnviar, P_In_maneja_SSL);

                if (bRespuestaEnvioMail == true)
                {
                    ok.LblMensaje.Text = "Se ha procesado los comprobantes electrónicos." + Environment.NewLine + "Se ha enviado un informe al correo del remitente.";
                    ok.ShowDialog();
                }

                else
                {
                    ok.LblMensaje.Text = "Se ha procesado los comprobantes electrónicos." + Environment.NewLine + "Ha ocurrido un problema al enviar el informe al correo del remitente.";
                    ok.ShowDialog();
                }

            }

            catch(Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
            }
        }

        #endregion

        #region FUNCIONES PARA EL PROCESO DE SINCRONIZACION DE FACTURAS ELECTRONICAS

        //FUNCION PARA GENERAR EL XML
        private bool generarXML(long iIdFactura_P)
        {
            try
            {
                generar.GSub_GenerarFacturaXML(iIdFactura_P, 1, sIdTipoAmbiente, sIdTipoEmision, sDirGenerados, "FACTURA", 2, "elvis.geovanni@hotmail.com", "elvis.geovanni@hotmail.com");
                return true;
            }

            catch (Exception ex)
            {
                //catchMensaje.LblMensaje.Text = ex.ToString();
                //catchMensaje.ShowDialog();
                return false;
            }
        }
        
        //INSTRUCCIONES PARA FIRMAR EL DOCUMENTO XML
        private bool firmarArchivoXML(string sNumeroDocumento_P)
        {
            try
            {
                sArchivoFirmar = sDirGenerados + @"\" + sNumeroDocumento_P + ".xml";

                sJar = @"c:\SRI.jar";

                sXmlPathOut = sDirFirmados + @"\";

                sFileOut = sNumeroDocumento_P + ".xml";

                sCodigoError = firmar.GSub_FirmarXML(sJar, sCertificado, sPassCertificado, sArchivoFirmar, sXmlPathOut, sFileOut, sCodigoError, sDescripcionError);
                
                if (sCodigoError == "00")
                {
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

        //INSTRUCCIONES PARA FIRMAR EL DOCUMENTO XML
        private bool enviarArchivoXML(string sNumeroDocumento_P, int iFila_P)
        {
            try
            {
                sArchivoEnviar = sDirFirmados + @"\" + sNumeroDocumento_P + ".xml";

                RespuestaSRI respuesta = enviar.EnvioComprobante(sArchivoEnviar);

                dgvDatos.Rows[iFila_P].Cells["colEstado"].Value = respuesta.Estado;

                if (respuesta.Estado == "RECIBIDA")
                {
                    dgvDatos.Rows[iFila_P].Cells["colEstado"].Style.BackColor = Color.Cyan;
                }

                else
                {
                    dgvDatos.Rows[iFila_P].Cells["colEstado"].Style.BackColor = Color.Red;
                }

                return true;
            }

            catch (Exception ex)
            {
                //catchMensaje.LblMensaje.Text = ex.ToString();
                //catchMensaje.ShowDialog();
                return false;
            }
        }

        //INSTRUCCIONES PARA CONSULTAR EL ESTADO DEL XML DEL SRI
        private bool consultarArchivoXML(string sNumeroDocumento_P, long iIdFactura_P, int iFila_P)
        {
            try
            {
                sClaveAcceso = consultarClaveAcceso(iIdFactura_P);

                if (sClaveAcceso != "")
                {
                    RespuestaSRI respuesta = consultar.AutorizacionComprobante(out xmlAut, sClaveAcceso);

                    //dgvDatos.Rows[iFila_P].Cells["colEstado"].Value = respuesta.Estado;
                    sEstadoAutorizacion = respuesta.Estado;

                    //Genera y guarda el XML autorizado
                    filename = Path.GetFileNameWithoutExtension(sNumeroDocumento_P) + ".xml";

                    if (respuesta.Estado == "AUTORIZADO")
                    {
                        dgvDatos.Rows[iFila_P].Cells["colEstado"].Style.BackColor = Color.Lime;
                        filename = Path.Combine(sDirAutorizados, filename);
                    }

                    else
                    {
                        dgvDatos.Rows[iFila_P].Cells["colMensaje"].Value = respuesta.ErrorMensaje;
                        dgvDatos.Rows[iFila_P].Cells["colEstado"].Style.BackColor = Color.Red;
                        filename = Path.Combine(sDirNoAutorizados, filename);
                    }
                    

                    xmlAutorizado(respuesta, filename, sNumeroDocumento_P);
                    actualizarDatos(respuesta.FechaAutorizacion.Substring(0,10) + " " + respuesta.FechaAutorizacion.Substring(11, 8), iIdFactura_P, respuesta.NumeroAutorizacion);

                    //ENVIAR A FUNCION PARA CREAR EL PDF
                    filename = Path.GetFileNameWithoutExtension(sNumeroDocumento_P) + ".pdf";
                    filename = Path.Combine(sDirAutorizados, filename);
                    crearRide(filename, iIdFactura_P, sNumeroDocumento);

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

        //CREAR RIDE
        private void crearRide(string filename, long iIdFactura_P, string sNumeroDocumento)
        {
            try
            {
                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Genera_Ride(dtConsulta, iIdFactura_P);

                if (bRespuesta == true)
                {
                    bRespuesta = ride.generarRide(dtConsulta, filename, iIdFactura_P);

                    if (bRespuesta == false)
                    {
                        ok.LblMensaje.Text = "Error al crear el reporte RIDE de la factura " + sNumeroDocumento;
                        ok.ShowDialog();
                    }
                }
            }

            catch(Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
            }
        }

        //CONSTRUIR XML AUTORIZADO
        private void xmlAutorizado(RespuestaSRI sri, string filename, string sNumeroDocumento_P)
        {
            try
            {
                sArchivoFirmar = sDirFirmados + @"\" + sNumeroDocumento_P + ".xml";

                miXMl = File.ReadAllText(sArchivoFirmar);
                //Declaramos el documento y su definición
                xml = new XDocument(
                    new XDeclaration(sVersion, sUTF, sStandAlone));

                autorizacion = new XElement("autorizacion");
                autorizacion.Add(new XElement("estado", sri.Estado));
                autorizacion.Add(new XElement("numeroAutorizacion", sri.NumeroAutorizacion));
                autorizacion.Add(new XElement("fechaAutorizacion", sri.FechaAutorizacion));
                autorizacion.Add(new XElement("ambiente", sri.Ambiente));
                autorizacion.Add(new XElement("comprobante", new XCData(miXMl)));
                autorizacion.Add(new XElement("mensajes", sri.ErrorMensaje));
                xml.Add(autorizacion);

                //PROBAR COMO GUARDA
                xml.Save(filename);
            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowInTaskbar = false;
                catchMensaje.ShowDialog();
            }
        }

        //ACTUALIZAR EN LA BASE DE DATOS
        private bool actualizarDatos(string sFecha_P, long iIdFactura_P, string sNumeroAutorizacion_P)
        {
            try
            {
                if (sFecha_P != "")
                {
                    //SE INICIA UNA TRANSACCION
                    if (!conexion.GFun_Lo_Maneja_Transaccion(Program.G_INICIA_TRANSACCION))
                    {
                        ok.LblMensaje.Text = "Error al abrir transacción.";
                        ok.ShowInTaskbar = false;
                        ok.ShowDialog();
                    }

                    //UPDATE PARA FACTURAS
                    if (sCodigoDocumento == "01")
                    {
                        sSql = "";
                        sSql = sSql + "update cv403_facturas set" + Environment.NewLine;
                        sSql = sSql + "autorizacion = '" + sNumeroAutorizacion_P + "'," + Environment.NewLine;
                        sSql = sSql + "fecha_autorizacion = '" + sFecha_P + "'" + Environment.NewLine;
                        sSql = sSql + "where id_factura = " + iIdFactura_P;

                        //EJECUTAR LA INSTRUCCIÓN SQL
                        if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                        {
                            catchMensaje.LblMensaje.Text = "No se pudo grabar la autorización de la factura.";
                            catchMensaje.ShowInTaskbar = false;
                            catchMensaje.ShowDialog();
                            goto reversa;
                        }
                    }

                    //UPDATE PARA COMPROBANTES DE RETENCION
                    else if (sCodigoDocumento == "07")
                    {
                        sSql = "";
                        sSql = sSql + "update cv405_cab_comprobantes_retencion set" + Environment.NewLine;
                        sSql = sSql + "autorizacion = '" + sNumeroAutorizacion_P + "'," + Environment.NewLine;
                        sSql = sSql + "fecha_autorizacion = '" + sFecha_P + "'" + Environment.NewLine;
                        sSql = sSql + "where id_cab_comprobante_retencion = " + iIdFactura_P;

                        //EJECUTAR LA INSTRUCCIÓN SQL
                        if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                        {
                            catchMensaje.LblMensaje.Text = "No se pudo grabar la autorización del comprobante de retención";
                            catchMensaje.ShowInTaskbar = false;
                            catchMensaje.ShowDialog();
                            goto reversa;
                        }
                    }

                    //UPDATE PARA NOTAS DE CREDITO
                    else if (sCodigoDocumento == "04")
                    {
                        sSql = "";
                        sSql = sSql + "update cv403_notas_credito set" + Environment.NewLine;
                        sSql = sSql + "autorizacion = '" + sNumeroAutorizacion_P + "'," + Environment.NewLine;
                        sSql = sSql + "fecha_autorizacion = '" + sFecha_P + "'" + Environment.NewLine;
                        sSql = sSql + "where id_nota_credito = " + iIdFactura_P;

                        //EJECUTAR LA INSTRUCCIÓN SQL
                        if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                        {
                            catchMensaje.LblMensaje.Text = "No se pudo grabar la autorización de la nota de crédito " + iIdFactura_P;
                            catchMensaje.ShowInTaskbar = false;
                            catchMensaje.ShowDialog();
                            goto reversa;
                        }
                    }

                    //UPDATE PARA GUIAS DE REMISION
                    else if (sCodigoDocumento == "06")
                    {
                        sSql = "";
                        sSql = sSql + "update cv403_guias_remision set" + Environment.NewLine;
                        sSql = sSql + "autorizacion = '" + sNumeroAutorizacion_P + "'," + Environment.NewLine;
                        sSql = sSql + "fecha_autorizacion = '" + sFecha_P + "'" + Environment.NewLine;
                        sSql = sSql + "where id_guia_remision = " + iIdFactura_P;

                        //EJECUTAR LA INSTRUCCIÓN SQL
                        if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                        {
                            catchMensaje.LblMensaje.Text = "No se pudo grabar la Autorización de la guía de remisión.";
                            catchMensaje.ShowInTaskbar = false;
                            catchMensaje.ShowDialog();
                            goto reversa;
                        }
                    }

                    conexion.GFun_Lo_Maneja_Transaccion(Program.G_TERMINA_TRANSACCION);
                    return true;
                }

                else
                {
                    ok.LblMensaje.Text = "No ha ingresado el nombre del archivo autorizado";
                    ok.ShowDialog();
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

        reversa:
            {
                conexion.GFun_Lo_Maneja_Transaccion(Program.G_REVERSA_TRANSACCION);
                return false;
            }
        }

        //CONSULTAR LA CLAVE DE ACCESO DE CADA FACTURA DEL GRID
        private string consultarClaveAcceso(long iIdFactura_P)
        {
            try
            {
                sSql = "";
                sSql = sSql + "select " + conexion.GFun_St_esnulo() + "(clave_acceso, 'NINGUNA') clave_acceso " + Environment.NewLine;
                sSql = sSql + "from cv403_facturas " + Environment.NewLine;
                sSql = sSql + "where id_factura = " + iIdFactura_P;

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        return dtConsulta.Rows[0][0].ToString();
                    }

                    else
                    {
                        return "";
                    }
                }

                else
                {
                    catchMensaje.LblMensaje.Text = sSql;
                    catchMensaje.ShowDialog();
                    return "";
                }
            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
                return "";
            }
        }
        
        //FUNCION PARA ENVIAR AL CORREO ELECTRONICO
        private bool enviarMail(int iFila_P, string sNumeroDocumento_P)
        {
            try
            {
                sCorreoCliente = dgvDatos.Rows[iFila_P].Cells["colMail"].Value.ToString();
                sAsuntoMail = P_St_nombre_comercial + ", Envio de Comprobante electrónico " + sNumeroDocumento_P;
                srutaXML =  sDirAutorizados + @"\" + sNumeroDocumento_P + ".xml";
                srutaRIDE = sDirAutorizados + @"\" + sNumeroDocumento_P + ".pdf";
                sRutaAdjuntos = "";
                
                if (srutaRIDE != "")
                {
                    sRutaAdjuntos = sRutaAdjuntos + srutaXML + "|" + srutaRIDE;
                }
                else
                {
                    sRutaAdjuntos = sRutaAdjuntos + srutaXML;
                }

                sMensajeRetorno = crearMensajeEnvio(iFila_P, sNumeroDocumento_P);
                bRespuestaEnvioMail = correo.enviarCorreo(P_St_correo_server_smtp, Convert.ToInt32(P_Ln_correo_puerto_smtp), P_St_from,
                                    P_St_correo_palabra_clave, P_St_fromname, sCorreoCliente,
                                    P_St_correo_con_copia, P_St_correo_consumidor_final, sAsuntoMail,
                                    sRutaAdjuntos, sMensajeRetorno, P_In_maneja_SSL);

                if (bRespuestaEnvioMail == true)
                {
                    return true;
                }

                else
                {
                    return false;
                }
            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
                return false;
            }
        }

        //FUNCION PARA CREAR EL CUERPO DEL MENSAJE
        private string crearMensajeEnvio(int iFila_P, string sNumeroDocumento_P)
        {
            try
            {
                sMensajeEnviar = "";
                sCodigoDocumento = dgvDatos.Rows[iFila_P].Cells["colTipo"].Value.ToString();

                if (sCodigoDocumento == "FAC")
                {
                    sTipoComprobante = "FACTURA";
                }

                sMensajeEnviar = sMensajeEnviar + "Estimado Cliente " + dgvDatos.Rows[iFila_P].Cells["colCliente"].Value.ToString() + ":"+ Environment.NewLine;
                sMensajeEnviar = sMensajeEnviar + Environment.NewLine;
                sMensajeEnviar = sMensajeEnviar + P_St_nombre_comercial + " informa que en cumplimiento con la Resolución No.NAC-DGERCGC17-00000430. ";
                sMensajeEnviar = sMensajeEnviar + "emitida por el SRI, adjunto a este correo se encuentra su " + sTipoComprobante + " electrónica No. " + sNumeroDocumento_P;
                sMensajeEnviar = sMensajeEnviar + " en formato XML, así como su interpretación en formato PDF. " + Environment.NewLine;
                sMensajeEnviar = sMensajeEnviar + Environment.NewLine;

                sMensajeEnviar = sMensajeEnviar + "Favor no responder a este correo, cualquier consulta realice a nuestra oficina, teléfono " + P_St_telefono_empresa + "." + Environment.NewLine;
                sMensajeEnviar = sMensajeEnviar + Environment.NewLine;
                sMensajeEnviar = sMensajeEnviar  + "Mayor información sobre facturación electrónica en: http://www.sri.gob.ec/web/guest/comprobantes-electronicos1 " + Environment.NewLine;
                sMensajeEnviar = sMensajeEnviar + Environment.NewLine;
                sMensajeEnviar = sMensajeEnviar + "Atentamente, " + Environment.NewLine;
                sMensajeEnviar = sMensajeEnviar  + Environment.NewLine;
                sMensajeEnviar = sMensajeEnviar + P_St_nombre_comercial;
                sMensajeEnviar = sMensajeEnviar + Environment.NewLine + Environment.NewLine;

                return sMensajeEnviar;
            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
                return "";
            }
        }

        #endregion

        private void frmSincronizarFacturas_Load(object sender, EventArgs e)
        {
            buscarDirectorio();
            datosCertificado();
            consultarTipoAmbiente();
            consultarTipoEmision();
            traerparametrosMail();
            limpiar();
        }

        private void frmSincronizarFacturas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
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

        private void btnOK_Click(object sender, EventArgs e)
        {
            llenarGrid();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {            
            dgvDatos.Rows.Clear();
            lblCuentaRegistros.Text = dgvDatos.Rows.Count.ToString() + " Registros Encontrados.";
            limpiar();
        }

        private async void btnSincronizar_Click(object sender, EventArgs e)
        {
            iNumeroRegistros = 0;
            iErrorGenerados = 0;
            iErrorFirmados = 0;
            iErrorEnviarSRI = 0;
            iErrorConsultarXMLSRI = 0;
            iErrorEnviarMail = 0;
            iEnviadosExitosamente = 0;

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
                                sNumeroDocumento = "F" + dgvDatos.Rows[i].Cells["colEstablecimiento"].Value.ToString() +
                                               dgvDatos.Rows[i].Cells["colPtoEmision"].Value.ToString() +
                                               dgvDatos.Rows[i].Cells["colNumeroComprobante"].Value.ToString().PadLeft(9, '0');



                                //USANDO TASK ASYNC AWAIT

                                // PASO 1.- INVOCAR A FUNCION PARA PRIMERO GENERAR EL XML
                                task = new Task<bool>(() => generarXML(Convert.ToInt64(dgvDatos.Rows[i].Cells["colIdFactura"].Value)));
                                task.Start();
                                dgvDatos.Rows[i].Cells["colEstado"].Value = "Generando XML...";
                                bConsulta = await task;

                                if (bConsulta == true)
                                {
                                    dgvDatos.Rows[i].Cells["colEstado"].Value = "XML Generado.";
                                    dgvDatos.Rows[i].Cells["colEstado"].Style.BackColor = Color.Yellow;
                                }

                                else
                                {
                                    dgvDatos.Rows[i].Cells["colEstado"].Value = "Error generar XML.";
                                    dgvDatos.Rows[i].Cells["colEstado"].Style.BackColor = Color.Red;
                                    iErrorGenerados++;
                                    goto salir;
                                }



                                // PASO 2.- INVOCAR A FUNCION PARA FIRMAR EL XML
                                task = new Task<bool>(() => firmarArchivoXML(sNumeroDocumento));
                                task.Start();
                                dgvDatos.Rows[i].Cells["colEstado"].Value = "Firmando XML...";
                                bConsulta = await task;

                                if (bConsulta == true)
                                {
                                    dgvDatos.Rows[i].Cells["colEstado"].Value = "XML Firmado.";
                                    dgvDatos.Rows[i].Cells["colEstado"].Style.BackColor = Color.Yellow;

                                }

                                else
                                {
                                    dgvDatos.Rows[i].Cells["colEstado"].Value = "Error firmar XML.";
                                    dgvDatos.Rows[i].Cells["colEstado"].Style.BackColor = Color.Red;
                                    iErrorFirmados++;
                                    goto salir;
                                }

                                Thread.Sleep(3000);

                                // PASO 3.- INVOCAR A FUNCION PARA ENVIAR EL XML AL SRI
                                bConsulta = false;
                                int iNumeroIntentos = 1;

                                while ((bConsulta == false) && (iNumeroIntentos <= 3))
                                {
                                    task = new Task<bool>(() => enviarArchivoXML(sNumeroDocumento, i));
                                    task.Start();
                                    dgvDatos.Rows[i].Cells["colEstado"].Value = "Enviando al SRI...";
                                    bConsulta = await task;

                                    if (bConsulta == true)
                                    {
                                        dgvDatos.Rows[i].Cells["colEstado"].Value = "XML Enviado.";
                                        dgvDatos.Rows[i].Cells["colEstado"].Style.BackColor = Color.Yellow;
                                    }

                                    else
                                    {
                                        if (iNumeroIntentos < 3)
                                        {
                                            dgvDatos.Rows[i].Cells["colEstado"].Value = "Reintentando enviar a SRI.";
                                            dgvDatos.Rows[i].Cells["colEstado"].Style.BackColor = Color.Red;
                                        }

                                        else
                                        {
                                            dgvDatos.Rows[i].Cells["colEstado"].Value = "Error al enviar XML.";
                                            dgvDatos.Rows[i].Cells["colEstado"].Style.BackColor = Color.Red;
                                            iErrorEnviarSRI++;
                                            goto salir;
                                        }
                                    }

                                    iNumeroIntentos++;
                                }

                                
                                

                                // PASO 4.- INVOCAR A FUNCION PARA CONSULTAR LA AUTORIZACION DEL SRI
                                task = new Task<bool>(() => consultarArchivoXML(sNumeroDocumento, Convert.ToInt64(dgvDatos.Rows[i].Cells["colIdFactura"].Value), i));
                                task.Start();
                                dgvDatos.Rows[i].Cells["colEstado"].Value = "Consultando autorización SRI...";
                                bConsulta = await task;

                                if (bConsulta == true)
                                {
                                    dgvDatos.Rows[i].Cells["colEstado"].Value = sEstadoAutorizacion;

                                    if (sEstadoAutorizacion != "AUTORIZADO")
                                    {
                                        dgvDatos.Rows[i].Cells["colEstado"].Style.BackColor = Color.Yellow;
                                        goto salir;
                                    }
                                }

                                else
                                {
                                    dgvDatos.Rows[i].Cells["colEstado"].Value = "Error consultando SRI.";
                                    dgvDatos.Rows[i].Cells["colEstado"].Style.BackColor = Color.Red;
                                    iErrorConsultarXMLSRI++;
                                    goto salir;
                                }


                                //PASO 5.- ENVIAR AL CORREO ELECTRONICO DEL CLIENTE
                                task = new Task<bool>(() =>enviarMail(i, sNumeroDocumento));
                                task.Start();
                                dgvDatos.Rows[i].Cells["colEstado"].Value = "Enviando al correo...";
                                bConsulta = await task;

                                if (bConsulta == true)
                                {
                                    dgvDatos.Rows[i].Cells["colEstado"].Value = "Correo enviado.";
                                    dgvDatos.Rows[i].Cells["colEstado"].Style.BackColor = Color.Yellow;
                                }

                                else
                                {
                                    dgvDatos.Rows[i].Cells["colEstado"].Value = "Error al enviar al correo.";
                                    dgvDatos.Rows[i].Cells["colEstado"].Style.BackColor = Color.Red;
                                    iErrorEnviarMail++;
                                    goto salir;
                                }                                

                                iEnviadosExitosamente++;
                            }

                        salir:
                            {
                                dgvDatos.Rows[i].Cells["colMarca"].Value = false;
                            }
                        }

                        enviarInformeFacturas();

                        this.Cursor = Cursors.Default;
                        chkSeleccionar.Checked = false;
                        chkSeleccionar.Text = "Seleccionar todos los registros";
                    }
                }
            }

            fin:{};
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

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}