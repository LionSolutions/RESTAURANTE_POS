using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Palatium.Facturacion_Electronica
{
    public partial class frmGenerarXML : Form
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        VentanasMensajes.frmMensajeCatch catchMensaje = new VentanasMensajes.frmMensajeCatch();
        VentanasMensajes.frmMensajeOK ok = new VentanasMensajes.frmMensajeOK();
        Clases_Factura_Electronica.ClaseGenerarFacturaXml generar = new Clases_Factura_Electronica.ClaseGenerarFacturaXml();
        Clases_Factura_Electronica.ClaseGenerarRIDE ride = new Clases_Factura_Electronica.ClaseGenerarRIDE();

        DataTable dtConsulta;

        string sSql;
        string sAyuda;
        string sCodigo;
        string sNombre;
        string sUnidad;
        string sDirectorio;

        int iCol_Correlativo;
        int iCol_Codigo;
        int iCol_Descripcion;
        int iNumeroDecimales;
        int iIdPersona;
        int iIdFactura;

        bool bRespuesta;

        
        double dbVUnidad;
        double dbPocenDescuento;
        double dbValorDescuento;
        double dbCantidad;
        double dbValorTotal;
        double dbServicio;
        double dbSubtotalBruto;
        double dbSumaDescuento;
        double dbSumaServicio;
        double dbSubtotalNeto;
        double dbSumaIva;
        double dbTotal;


        public frmGenerarXML()
        {
            InitializeComponent();
        }

        #region FUNCIONES DEL USUARIO

        //FUNCION PARA RELLENAR LAS INSTRUCCIONES SQL
        private void llenarInstruccionesSQL()
        {
            sAyuda = generar.GSub_ActualizaPantalla("01");
            iCol_Correlativo = 4;
            iCol_Codigo = 0;
            iCol_Descripcion = 1;
            dbAyudaFacturas.Ver(sAyuda, "", iCol_Correlativo, iCol_Codigo, iCol_Descripcion);

            sAyuda = "";
            sAyuda += "select identificacion, apellidos + ' ' + nombres cliente, id_persona" + Environment.NewLine;
            sAyuda += "from tp_personas" + Environment.NewLine;
            sAyuda += "where estado = 'A'" + Environment.NewLine;
            sAyuda += "order by nombres";
            iCol_Correlativo = 2;
            iCol_Codigo = 0;
            iCol_Descripcion = 1;
            dbAyudaCliente.Ver(sAyuda, "", iCol_Correlativo, iCol_Codigo, iCol_Descripcion);
        }

        //LLENAR COMBO localidad
        private void llenarComboLocalidad()
        {
            try
            {
                sSql = "";
                sSql = "select id_localidad, nombre_localidad" + Environment.NewLine;
                sSql += "from tp_vw_localidades";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                cmbLocalidad.llenar(dtConsulta, sSql);

                if (cmbLocalidad.Items.Count > 0)
                {
                    cmbLocalidad.SelectedIndex = 1;
                }

                cmbLocalidad2.llenar(dtConsulta, sSql);

                if (cmbLocalidad2.Items.Count > 0)
                {
                    cmbLocalidad2.SelectedIndex = 1;
                }
            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
            }
        }

        //LLENAR COMBO Vendedor
        private void llenarComboVendedor()
        {
            try
            {
                sSql = "";
                sSql += "select id_vendedor, codigo" + Environment.NewLine;
                sSql += "from cv403_vendedores" + Environment.NewLine;
                sSql += "where estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                cmbVendedor.llenar(dtConsulta, sSql);

                if (cmbVendedor.Items.Count > 0)
                {
                    cmbVendedor.SelectedIndex = 1;
                }
            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
            }
        }

        //LLENAR COMBO moneda
        private void llenarComboMoneda()
        {
            try
            {
                sSql = "";
                sSql += "select correlativo, valor_texto" + Environment.NewLine;
                sSql += "from tp_codigos" + Environment.NewLine;
                sSql += "where tabla='SYS$00021'" + Environment.NewLine;
                sSql += "and estado='A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();
                
                cmbMonedaFactura.llenar(dtConsulta, sSql);

                if (cmbMonedaFactura.Items.Count > 0)
                {
                    cmbMonedaFactura.SelectedIndex = 1;
                }
            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
            }
        }

        //FUNCION PARA LLENAR EL GRID cuando no se haya seleccionado ningun cliente 
        private void llenarInformacion()
        {
            try
            {
                sSql = "";
                sSql += "Select F.id_persona, F.Direccion_Factura, F.Telefono_Factura, F.Ciudad_Factura, F.Fabricante,F.Referencia," + Environment.NewLine;
                sSql += "F.Comentarios, CO.valor_texto as Moneda, FORPAGO.descripcion as Foma_Pago, F.fecha_vcto, F.Comentarios," + Environment.NewLine;
                sSql += "F.Peso_Neto, F.Peso_Bruto, F.numero_exportacion, F.partida_arancelaria, F.idformulariossri, TIPOCO.descripcion as Formato," + Environment.NewLine;
                sSql += "isnull(F.autorizacion,'') autorizacion, VENDE.codigo as Vendedor, CODI.valor_texto as Tipo_cliente, CP.Porcentaje_Dscto," + Environment.NewLine;
                sSql += "CP.Porcentaje_IVA, PR.codigo,NP.nombre, UNIDAD.codigo Unidad, isnull(DP.comentario,'') Comentario, DP.precio_unitario," + Environment.NewLine;
                sSql += "DP.Cantidad,Case when DP.precio_unitario=0 then 0 else round(100*DP.valor_Dscto/DP.precio_unitario,2) end Pct_Dscto," + Environment.NewLine;
                sSql += "DP.valor_Dscto, DP.valor_ICE, DP.valor_IVA, DP.Comentario, DP.Id_Det_Pedido, F.fecha_factura, Case when PR.Expira = 1 Then 1 Else 0 End Expira," + Environment.NewLine;
                sSql += "F.id_persona, TP.identificacion, rtrim(TP.apellidos + ' ' + TP.nombres) cliente, F.id_localidad, DP.valor_otro, F.id_factura, TP.correo_electronico" + Environment.NewLine;
                sSql += "From cv403_facturas F, cv403_facturas_pedidos FP, cv403_cab_pedidos CP, cv403_det_pedidos DP, cv401_productos PR, cv401_nombre_productos NP," + Environment.NewLine;
                sSql += "tp_codigos UNIDAD, tp_codigos CO, tp_codigos CODI, cv403_vendedores VENDE, cv403_formas_pagos FORPAGO, vta_tipocomprobante TIPOCO," + Environment.NewLine;
                sSql += "tp_personas TP" + Environment.NewLine;
                sSql += "Where F.id_factura = " + dbAyudaFacturas.iId + Environment.NewLine;
                sSql += "And F.id_persona = TP.id_persona" + Environment.NewLine;
                sSql += "And F.id_factura = FP.id_factura" + Environment.NewLine;
                sSql += "And F.cg_moneda = CO.correlativo" + Environment.NewLine;
                sSql += "And CP.cg_tipo_cliente = CODI.correlativo" + Environment.NewLine;
                sSql += "And F.idtipocomprobante = TIPOCO.idtipocomprobante" + Environment.NewLine;
                sSql += "And F.id_forma_pago = FORPAGO.id_forma_pago" + Environment.NewLine;
                sSql += "And F.id_vendedor = VENDE.id_vendedor" + Environment.NewLine;
                sSql += "And FP.estado = 'A'" + Environment.NewLine;
                sSql += "And FP.Id_Pedido = CP.Id_Pedido" + Environment.NewLine;
                sSql += "And CP.Id_Pedido = DP.Id_Pedido" + Environment.NewLine;
                sSql += "And DP.estado = 'A'" + Environment.NewLine;
                sSql += "And DP.Cg_Unidad_Medida = UNIDAD.correlativo" + Environment.NewLine;
                sSql += "And DP.id_producto = PR.id_producto" + Environment.NewLine;
                sSql += "And PR.id_producto = NP.id_producto" + Environment.NewLine;
                sSql += "And NP.nombre_Interno = 1" + Environment.NewLine;
                sSql += "And NP.estado = 'A'" + Environment.NewLine;
                sSql += "order by DP.Id_Det_Pedido";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        txtDireccion.Text = dtConsulta.Rows[0].ItemArray[1].ToString();
                        txtTelefono.Text = dtConsulta.Rows[0].ItemArray[2].ToString();
                        txtCiudad.Text = dtConsulta.Rows[0].ItemArray[3].ToString();
                        txtFabricante.Text = dtConsulta.Rows[0].ItemArray[4].ToString();
                        txtRefOt.Text = dtConsulta.Rows[0].ItemArray[5].ToString();
                        txtObser.Text = dtConsulta.Rows[0].ItemArray[6].ToString();
                        cmbMonedaFactura.Text = dtConsulta.Rows[0].ItemArray[7].ToString();
                        cmbTipoPago.Text = dtConsulta.Rows[0].ItemArray[8].ToString();
                        txtFechaVcto.Text = dtConsulta.Rows[0].ItemArray[9].ToString();
                        txtPesoNeto.Text = dtConsulta.Rows[0].ItemArray[11].ToString();
                        txtPesoBruto.Text = dtConsulta.Rows[0].ItemArray[12].ToString();
                        txtNExportacion.Text = dtConsulta.Rows[0].ItemArray[13].ToString();
                        txtPartidaArancelaria.Text = dtConsulta.Rows[0].ItemArray[14].ToString();
                        cmbAutSri.Text = dtConsulta.Rows[0].ItemArray[15].ToString();
                        cmbFormato.Text = dtConsulta.Rows[0].ItemArray[16].ToString();
                        cmbVendedor.Text = dtConsulta.Rows[0].ItemArray[18].ToString();
                        cmbTipoCliente.Text = dtConsulta.Rows[0].ItemArray[19].ToString();
                        txtPorcientoDescuento.Text = dtConsulta.Rows[0].ItemArray[20].ToString();
                        txtFecha.Text = dtConsulta.Rows[0].ItemArray[34].ToString();
                        dbAyudaCliente.iId = Convert.ToInt32(dtConsulta.Rows[0].ItemArray[36].ToString());
                        dbAyudaCliente.txtIdentificacion.Text = dtConsulta.Rows[0].ItemArray[37].ToString();
                        dbAyudaCliente.txtDatos.Text = dtConsulta.Rows[0].ItemArray[38].ToString();
                        cmbLocalidad2.SelectedValue = Convert.ToInt32(dtConsulta.Rows[0].ItemArray[39].ToString());
                        txtMail.Text = dtConsulta.Rows[0].ItemArray[42].ToString();

                        iIdPersona = Convert.ToInt32(dtConsulta.Rows[0].ItemArray[0].ToString());
                        iIdFactura = Convert.ToInt32(dtConsulta.Rows[0].ItemArray[41].ToString());

                        dbSumaDescuento = 0;
                        dbSumaServicio = 0;
                        dbSubtotalBruto = 0;
                        dbSumaIva = 0;
                        dbTotal = 0;
                        dbSubtotalNeto = 0;

                        for (int i = 0; i < dtConsulta.Rows.Count; i++)
                        {
                            sCodigo = dtConsulta.Rows[i].ItemArray[22].ToString();

                            if (dtConsulta.Rows[i].ItemArray[25].ToString() == "")
                            {
                                sNombre = dtConsulta.Rows[i].ItemArray[23].ToString();
                            }

                            else
                            {
                                sNombre = dtConsulta.Rows[i].ItemArray[25].ToString();
                            }
                            
                            sUnidad = dtConsulta.Rows[i].ItemArray[24].ToString();
                            dbVUnidad = Convert.ToDouble(dtConsulta.Rows[i].ItemArray[26].ToString());
                            dbPocenDescuento = Convert.ToDouble(dtConsulta.Rows[i].ItemArray[28].ToString());
                            dbValorDescuento = Convert.ToDouble(dtConsulta.Rows[i].ItemArray[29].ToString());
                            dbCantidad = Convert.ToDouble(dtConsulta.Rows[i].ItemArray[27].ToString());
                            dbServicio = Convert.ToDouble(dtConsulta.Rows[i].ItemArray[40].ToString());

                            if (dbPocenDescuento == 100)
                            {
                                dbValorTotal = 0;
                            }

                            else
                            {
                                dbValorTotal = dbVUnidad * dbCantidad * (dbPocenDescuento / 100);
                            }

                            dbSubtotalBruto = dbSubtotalBruto + (dbCantidad * dbVUnidad);
                            dbSumaDescuento = dbSumaDescuento + (dbCantidad * dbValorDescuento);
                            dbSumaServicio = dbSumaServicio + (dbCantidad * dbServicio);
                            dbSumaIva = dbSumaIva + (dbCantidad * Convert.ToDouble(dtConsulta.Rows[i].ItemArray[31].ToString()));


                            dgvReimpresionFactura.Rows.Add(sCodigo, sNombre, sUnidad, dbCantidad.ToString("N2"), dbVUnidad.ToString("N2"),
                                                           dbPocenDescuento.ToString("N0"), dbValorDescuento.ToString("N2"),
                                                           dbServicio.ToString("N2"), dbValorTotal.ToString("N2"));
                        }

                        txtValorBruto.Text = dbSubtotalBruto.ToString("N2");
                        txtDescuento.Text = dbSumaDescuento.ToString("N2");
                        txtSubTotal.Text = (dbSubtotalBruto - dbSumaDescuento).ToString("N2");
                        txtIva.Text = dbSumaIva.ToString("N2");
                        txtServicio.Text = dbSumaServicio.ToString("N2");
                        txtTotalPagar.Text = (dbSubtotalBruto + dbSumaIva + dbSumaServicio - dbSumaDescuento).ToString("N2");

                        //para alinear los valores
                        dgvReimpresionFactura.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        dgvReimpresionFactura.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        dgvReimpresionFactura.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        dgvReimpresionFactura.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        dgvReimpresionFactura.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        dgvReimpresionFactura.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;


                        sSql = "";
                        sSql += "select idformulariossri, estabRetencion1, ptoEmiRetencion1, autRetencion1" + Environment.NewLine;
                        sSql += "from vta_formulariossri" + Environment.NewLine;
                        sSql += "where idformulariossri = 18";

                        dtConsulta = new DataTable();
                        dtConsulta.Clear();

                        bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                        if (bRespuesta == true)
                        {
                            if (dtConsulta.Rows.Count > 0)
                            {
                                txtNSerie1.Text = dtConsulta.Rows[0].ItemArray[1].ToString();
                                txtNSerie2.Text = dtConsulta.Rows[0].ItemArray[2].ToString();
                                txtNAut.Text = dtConsulta.Rows[0].ItemArray[3].ToString();
                            }
                        }

                        else
                        {
                            ok.LblMensaje.Text = "No se pudo cargar la información tributaria.";
                            ok.ShowDialog();
                        }
                    }

                    dgvReimpresionFactura.ClearSelection();
                }

                else
                {
                    ok.LblMensaje.Text = "Ocurrió un problema al extraer información de la factura seleccionada.";
                    ok.ShowDialog();
                }
            }

            catch(Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
            }
            
        }

        //LIPIAR LAS CAJAS DE TEXTO
        private void limpiarTodo()
        {
            dbAyudaFacturas.limpiar();
            dbAyudaCliente.limpiar();
            txtFecha.Text = "";
            txtNSerie1.Text = "";
            txtNSerie2.Text = "";
            txtCiudad.Text = "";
            txtDireccion.Text = "";
            txtTelefono.Text = "";
            txtPorcientoDescuento.Text = "";
            txtFechaVcto.Text = "";
            txtNAut.Text = "";

            txtValorBruto.Clear();
            txtDescuento.Clear();
            txtSubTotal.Clear();
            txtIva.Clear();
            txtServicio.Clear();
            txtTotalPagar.Clear();
            txtMail.Clear();

            iIdFactura = 0;
            iIdPersona = 0;

            cmbLocalidad.SelectedIndex = 1;
            cmbLocalidad2.SelectedIndex = 1;
            cmbMonedaFactura.SelectedIndex = 1;
            cmbVendedor.SelectedIndex = 1;
            dgvReimpresionFactura.Rows.Clear();
        }

        //CARGAR EL DIRECTORIO DONDE SE GUARDARAN LOS XML GENERADOS
        private bool buscarDirectorio()
        {
            try
            {
                sSql = "select codigo, nombres from cel_directorio where id_directorio = 1 and estado = 'A'";
                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        sDirectorio = dtConsulta.Rows[0].ItemArray[1].ToString();
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

        //FUNCION PARA ACTUALIZAR EL CORREO ELECTRONICO DEL CLIENTE
        private void actualizarCorreoElectronico()
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

                sSql = "";
                sSql += "update tp_personas set" + Environment.NewLine;
                sSql += "correo_electronico = '" + txtMail.Text.Trim() + "'" + Environment.NewLine;
                sSql += "where id_persona = " + iIdPersona + Environment.NewLine;
                sSql += "and estado = 'A'";


                //EJECUTA LA INSTRUCCION SQL
                if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                {
                    catchMensaje.LblMensaje.Text = sSql;
                    catchMensaje.ShowDialog();
                    goto reversa;
                }

                conexion.GFun_Lo_Maneja_Transaccion(Program.G_TERMINA_TRANSACCION);
                
                ok.LblMensaje.Text = "Correo electrónico actualizado éxitosamente";
                ok.ShowDialog();

                txtMail.ReadOnly = true;
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

            fin: {}

        }

        #endregion

        private void frmGenerarXML_Load(object sender, EventArgs e)
        {            
            llenarComboLocalidad();
            llenarComboVendedor();
            llenarComboMoneda();
            llenarInstruccionesSQL();
        }

        private void btnOKFactura_Click(object sender, EventArgs e)
        {
            dgvReimpresionFactura.Rows.Clear();

            if (dbAyudaFacturas.iId == 0)
            {
                ok.LblMensaje.Text = "Favor seleccione una factura para realizar la búsqueda de información.";
                ok.ShowDialog();
            }

            else
            {
                llenarInformacion();
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            limpiarTodo();
        }

        private void btnCerrarCateraCobrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnGenerarXML_Click(object sender, EventArgs e)
        {
            if (dbAyudaFacturas.iId == 0)
            {
                ok.LblMensaje.Text = "No ha seleccionado ninguna factura.";
                ok.ShowDialog();
            }

            else if (dgvReimpresionFactura.Rows.Count == 0)
            {
                ok.LblMensaje.Text = "No ha cargado los datos de la factura.";
                ok.ShowDialog();
            }

            else
            {
                if (rad2Decimales.Checked == true)
                {
                    iNumeroDecimales = 2;
                }

                else if (rad4Decimales.Checked == true)
                {
                    iNumeroDecimales = 4;
                }

                if (buscarDirectorio() == true)
                {
                    //xml.GSub_GenerarFacturaXML(iIdFactur, 0, "1", "1", @"C:", "FACTURA", 2, "elvis.geovanni@hotmail.com", "elvis.geovanni@hotmail.com");
                    generar.GSub_GenerarFacturaXML(dbAyudaFacturas.iId, 0, "1", "1", sDirectorio, "FACTURA", iNumeroDecimales, "elvis.geovanni@hotmail.com", "elvis.geovanni@hotmail.com");
                }
            }
        }

        private void btnFormatoRide_Click(object sender, EventArgs e)
        {
            try
            {
                if (dbAyudaFacturas.iId == 0)
                {
                    ok.LblMensaje.Text = "No ha seleccionado ninguna factura.";
                    ok.ShowDialog();
                }

                else
                {
                    dtConsulta = new DataTable();
                    dtConsulta.Clear();

                    this.Cursor = Cursors.WaitCursor;

                    bRespuesta = conexion.GFun_Lo_Genera_Ride(dtConsulta, dbAyudaFacturas.iId);

                    if (bRespuesta == true)
                    {
                        bRespuesta = ride.generarRide(dtConsulta, "", dbAyudaFacturas.iId);

                        if (bRespuesta == false)
                        {
                            ok.LblMensaje.Text = "Error al crear el reporte RIDE de la factura ";
                            ok.ShowDialog();
                        }
                    }

                    this.Cursor = Cursors.Default;
                }
            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
                this.Cursor = Cursors.Default;
            }
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            btnFormatoRide_Click(sender, e);
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (iIdPersona == 0)
            {
                ok.LblMensaje.Text = "No hay un cliente seleccionado para actualizar la información.";
                ok.ShowDialog();
                txtMail.ReadOnly = true;
            }

            else
            {
                txtMail.ReadOnly = false;
                txtMail.Focus();
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (iIdPersona == 0)
            {
                ok.LblMensaje.Text = "No hay un cliente seleccionado para actualizar la información.";
                ok.ShowDialog();
            }

            else if (txtMail.Text.Trim() == "")
            {
                ok.LblMensaje.Text = "Favor ingrese un correo electrónico.";
                ok.ShowDialog();
                txtMail.Focus();
            }

            else
            {
                //ACTUALIZAR CORREO ELECTRÓNICO
                actualizarCorreoElectronico();
            }
        }
    }
}
