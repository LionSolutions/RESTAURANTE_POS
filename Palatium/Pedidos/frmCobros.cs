using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Palatium.Pedidos
{
    public partial class frmCobros : Form
    {
         ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
         VentanasMensajes.frmMensajeOK ok = new VentanasMensajes.frmMensajeOK();
         VentanasMensajes.frmMensajeCatch catchMensaje = new VentanasMensajes.frmMensajeCatch();
         Clases.ClaseValidarRUC validarRuc = new Clases.ClaseValidarRUC();
         Clases.ClaseAbrirCajon abrir = new Clases.ClaseAbrirCajon();
         ValidarCedula validarCedula = new ValidarCedula();

         Button[,] boton = new Button[5, 2];

         int iIdCaja = 30;
         int iCgEstadoDctoPorCobrar = 7461;
         int iIdTipoEmision = 0;
         int iIdTipoAmbiente = 0;

         Orden ord;
         Button bpagar;

         string sSql;
         string sCiudad;
         string sNumeroFactura;
         string sIdOrden;
         string sFechaCorta;
         string sTabla;
         string sCampo;
         string sFecha;
         string sFacturaRecuperada;
         string sMovimiento;
         string sSecuencial;
         string sNumeroOrden;
         long iMaximo;

         DataTable dtConsulta;
         DataTable dtFormasPago;
         DataTable dtComanda;
         DataTable dtAuxiliar;
         DataTable dtTarjetasT;
         DataTable dtAgrupado;
         DataTable dtAlmacenar;
         DataTable dtOriginal;

         bool bRespuesta;
         int iCuentaFormasPagos;
         int iCuentaAyudaFormasPagos;
         int iPosXFormasPagos;
         int iPosYFormasPagos;
         int iIdLocalidadImpresora;
         int iNumeroMovimientoCaja;
         int iIdPersona;
         int idTipoIdentificacion;
         int idTipoPersona;
         int iBanderaDomicilio;
         int iTercerDigito;
         int iIdDocumentoCobrar;
         int iCuenta;
         int iIdPago;
         int iIdDocumentoPagado;
         int iCgTipoDocumento;
         int iIdDocumentoPago;
         int iNumeroPago;
         int iEjecutarActualizacionIVA;
         int iEjecutarActualizacionTarjetas;
         int iIdPosMovimientoCaja;
         int iOpCambiarEstadoOrden;
         int iNumeroMovimiento;
         int iIdTipoComprobante;
         int iCerrarCuenta;
         int iNumeroPedido;
         int iIdFactura;
         int iManejaFE;
         int iBanderaGeneraFactura;
         int iIdListaMinorista_P;
         int iBanderaRemoverIvaBDD;
         int iBanderaRecargoBDD;
         int iBanderaRemoverIvaBoton;
         int iBanderaRecargoBoton;
         int iNumeroPedido_P;
         int iNumeroCuenta_P;
         int iIdFormaPago_1;
         int iIdFormaPago_2;
         int iIdFormaPago_3;

         Decimal dTotal;
         Decimal dSubtotal;
         Decimal dValor;
         Decimal dbSumaIva;
         Decimal dbRecalcularPrecioUnitario;
         Decimal dbRecalcularDescuento;
         Decimal dbRecalcularIva;
         Decimal dbValorGrid;
         Decimal dbValorRecuperado;
         Decimal dbPropina;
         Decimal dbCambio;
         Decimal dbIVAPorcentaje;
         Decimal dServicio;
         Decimal dbTotalAyuda;
         Decimal dbSubTotalRecargo;
         Decimal dbValorRecargo;
         Decimal dbPorcentajeRecargo;
         Decimal dbSubTotalNetoRecargo;
         Decimal dbIVARecargo;
         Decimal dbTotalRecargo;

         public frmCobros(string sIdOrden_P)
         {
             sIdOrden = sIdOrden_P;
            InitializeComponent();
         }

        #region FUNCIONES DEL USUARIO

        //FUNCION PARA EXTRAER LA LISTA MINORISTA
         private void extraerListaMinorista()
         {
            try
            {
                sSql = "";
                sSql += "select id_lista_precio from cv403_listas_precios" + Environment.NewLine;
                sSql += "where lista_minorista = 1" + Environment.NewLine;
                sSql += "and estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    if (Convert.ToInt32(dtConsulta.Rows[0]["id_lista_precio"].ToString()) > 0)
                    {
                        iIdListaMinorista_P = Convert.ToInt32(dtConsulta.Rows[0]["id_lista_precio"].ToString());
                    }

                    else
                    {
                        iIdListaMinorista_P = 4;
                    }

                    sSql = "";
                    sSql += "select DP.id_det_pedido, DP.id_producto, DP.precio_unitario, DP.valor_dscto," + Environment.NewLine;
                    sSql += "DP.valor_iva, DP.valor_otro, P.paga_iva, PP.valor, CP.porcentaje_iva" + Environment.NewLine;
                    sSql += "from cv403_cab_pedidos CP INNER JOIN" + Environment.NewLine;
                    sSql += "cv403_det_pedidos DP ON CP.id_pedido = DP.id_pedido" + Environment.NewLine;
                    sSql += "and CP.estado = 'A'" + Environment.NewLine;
                    sSql += "and DP.estado = 'A' INNER JOIN" + Environment.NewLine;
                    sSql += "cv401_productos P ON P.id_producto = DP.id_producto" + Environment.NewLine;
                    sSql += "and P.estado = 'A' INNER JOIN" + Environment.NewLine;
                    sSql += "cv403_precios_productos PP ON P.id_producto = PP.id_producto" + Environment.NewLine;
                    sSql += "and PP.estado = 'A'" + Environment.NewLine;
                    sSql += "where CP.id_pedido = " + sIdOrden + Environment.NewLine;
                    sSql += "and PP.id_lista_precio = " + iIdListaMinorista_P;

                    dtOriginal = new DataTable();
                    dtOriginal.Clear();

                    bRespuesta = conexion.GFun_Lo_Busca_Registro(dtOriginal, sSql);

                    if (bRespuesta == false)
                    {
                        catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                        catchMensaje.ShowDialog();
                    }
                }

                else
                {
                    catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                    catchMensaje.ShowDialog();
                }
            }
                 
            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.Message;
                catchMensaje.ShowDialog();
            }
        }

        //FUNCION PARA ACTUALIZAR EL IVA
         private bool actualizarIVA()
         {
             try
             {
                 for (int i = 0; i < dtComanda.Rows.Count; i++)
                 {
                     if (Convert.ToInt32(dtComanda.Rows[i]["paga_iva"].ToString()) == 1)
                     {
                         sSql = "";
                         sSql += "update cv403_det_pedidos set" + Environment.NewLine;
                         sSql += "valor_iva = " + Convert.ToDouble(dtComanda.Rows[i]["valor_iva"].ToString()) + Environment.NewLine;
                         sSql += "where id_det_pedido = " + Convert.ToInt32(dtComanda.Rows[i]["id_det_pedido"].ToString());

                         if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                         {
                             catchMensaje.LblMensaje.Text = "ERROR EN LA INSTRUCCIÓN:" + Environment.NewLine + sSql;
                             catchMensaje.ShowDialog();
                             return false;
                         }
                     }
                 }

                 return true;
             }
             catch (Exception ex)
             {
                 catchMensaje.LblMensaje.Text = ex.ToString();
                 catchMensaje.ShowDialog();
                 return false;
             }
         }

        //FUNCION PARA ACTUALIZAR EL IVA A CERO
         private bool actualizarIVACero()
         {
             try
             {
                 if (actualizarPreciosOriginales() == false)
                 {
                     return false;
                 }

                 sSql = "";
                 sSql += "update cv403_det_pedidos set" + Environment.NewLine;
                 sSql += "valor_iva = 0" + Environment.NewLine;
                 sSql += "where id_pedido = " + Convert.ToInt32(sIdOrden) + Environment.NewLine;
                 sSql += "and estado = 'A'";

                 if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                 {
                     catchMensaje.LblMensaje.Text = "ERROR EN LA INSTRUCCIÓN:" + Environment.NewLine + sSql;
                     catchMensaje.ShowDialog();
                     return false;
                 }

                 return true;
             }

             catch (Exception ex)
             {
                 catchMensaje.LblMensaje.Text = ex.ToString();
                 catchMensaje.ShowDialog();
                 return false;
             }
         }

        //FUNCION PARA ACTUALIZAR EL RECARGO DE TARJETAS
         private bool actualizarRecargoTarjetas()
         {
             try
             {
                 for (int i = 0; i < dtTarjetasT.Rows.Count; i++)
                 {
                     if (Convert.ToInt32(dtTarjetasT.Rows[i]["paga_iva"].ToString()) == 1)
                     {
                         sSql = "";
                         sSql += "update cv403_det_pedidos set" + Environment.NewLine;
                         sSql += "precio_unitario = " + Convert.ToDouble(dtTarjetasT.Rows[i]["precio_unitario"].ToString()) + "," + Environment.NewLine;
                         sSql += "valor_iva = " + Convert.ToDouble(dtTarjetasT.Rows[i]["valor_iva"].ToString()) + Environment.NewLine;
                         sSql += "where id_det_pedido = " + Convert.ToInt32(dtTarjetasT.Rows[i]["id_det_pedido"].ToString());
                         
                         if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                         {
                             catchMensaje.LblMensaje.Text = "ERROR EN LA INSTRUCCIÓN:" + Environment.NewLine + sSql;
                             catchMensaje.ShowDialog();
                             return false;
                         }
                     }
                 }

                 return true;
             }

             catch (Exception ex)
             {
                 catchMensaje.LblMensaje.Text = ex.ToString();
                 catchMensaje.ShowDialog();
                 return false;
             }
         }

        //FUNCION PARA REMOVER EL RECARGO TARJETAS
         private bool actualizarRemoverRecargoTarjetas()
         {
             try
             {
                 for (int i = 0; i < dtTarjetasT.Rows.Count; ++i)
                 {
                     sSql = "";
                     sSql += "select valor from cv403_precios_productos" + Environment.NewLine;
                     sSql += "where id_producto = " + dtTarjetasT.Rows[i]["id_producto"].ToString() + Environment.NewLine;
                     sSql += "and estado = 'A'" + Environment.NewLine;
                     sSql += "and id_lista_precio = " + iIdListaMinorista_P;

                     dtConsulta = new DataTable();
                     dtConsulta.Clear();

                     bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                     if (bRespuesta == false)
                     {
                         catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                         catchMensaje.ShowDialog();
                         return false;
                     }

                     Decimal dbValor_R = Convert.ToDecimal(dtConsulta.Rows[0]["valor"].ToString());
                     Decimal dbValorIVA_R;

                     if (Convert.ToInt32(dtTarjetasT.Rows[i]["paga_iva"].ToString()) == 0)
                     {
                         dbValorIVA_R = 0;
                     }

                     else
                     {
                         dbValorIVA_R = dbValor_R + Convert.ToDecimal(Program.iva);
                     }
	
                     sSql = "";
                     sSql += "update cv403_det_pedidos set" + Environment.NewLine;
                     sSql += "precio_unitario = " + dbValor_R + "," + Environment.NewLine;
                     sSql += "valor_iva = " + dbValorIVA_R + Environment.NewLine;
                     sSql += "where id_det_pedido = " + Convert.ToInt32(dtTarjetasT.Rows[i]["id_det_pedido"].ToString());

                     if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                     {
                         catchMensaje.LblMensaje.Text = "ERROR EN LA INSTRUCCIÓN:" + Environment.NewLine + sSql;
                         catchMensaje.ShowDialog();
                         return false;
                     }
                 }

                 return true;
             }
             catch (Exception ex)
             {
                 catchMensaje.LblMensaje.Text = ex.ToString();
                 catchMensaje.ShowDialog();
                 return false;
             }
         }

        //FUNCION PARA APLICAR EL RECARGO DE TARJETA
         private bool aplicaRecargoTarjetas()
         {
             try
             {
                 dtTarjetasT = new DataTable();
                 dtTarjetasT.Clear();
                 dtTarjetasT = dtComanda.Copy();

                 for (int i = 0; i < dtTarjetasT.Rows.Count; i++)
                 {
                     Decimal dbPrecioUnitario_R = Convert.ToDecimal(dtTarjetasT.Rows[i]["precio_unitario"].ToString());
                     Decimal dbValorRecargo_R = dbPrecioUnitario_R * Convert.ToDecimal(Program.dbPorcentajeRecargoTarjeta);
                     Decimal dbTotal_R = dbPrecioUnitario_R + dbValorRecargo_R;
                     Decimal dbValorIVA_R;

                     if (Convert.ToInt32(dtTarjetasT.Rows[i]["paga_iva"].ToString()) == 0)
                     {
                         dbValorIVA_R = 0;
                     }

                     else
                     {
                         dbValorIVA_R = dbTotal_R * Convert.ToDecimal(Program.iva);
                     }

                     dtTarjetasT.Rows[i]["precio_unitario"] = dbTotal_R.ToString();
                     dtTarjetasT.Rows[i]["valor_iva"] = dbValorIVA_R.ToString();
                 }

                 return true;
             }

             catch (Exception ex)
             {
                 catchMensaje.LblMensaje.Text = ex.ToString();
                 catchMensaje.ShowDialog();
                 return false;
             }
         }

        //FUNCION PARA OBTENER EL TOTAL
         private void obtenerTotal()
         {
             try
             {
                 sSql = "";
                 sSql += "select ltrim(str(sum(cantidad * (precio_unitario + valor_iva + valor_otro - valor_dscto)), 10, 2)) total" + Environment.NewLine;
                 sSql += "from pos_vw_det_pedido" + Environment.NewLine;
                 sSql += "where id_pedido = " + sIdOrden;

                 dtConsulta = new DataTable();
                 dtConsulta.Clear();

                 bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                 if (bRespuesta == true)
                 {
                     dTotal = Convert.ToDecimal(dtConsulta.Rows[0][0].ToString());
                     lblTotal.Text = "$ " + dTotal.ToString("N2");
                     dbTotalAyuda = Convert.ToDecimal(dtConsulta.Rows[0][0].ToString());
                 }
                 else
                 {
                     catchMensaje.LblMensaje.Text = "ERROR EN LA INSTRUCCIÓN:" + Environment.NewLine + sSql;
                     catchMensaje.ShowDialog();
                 }
             }

             catch (Exception ex)
             {
                 catchMensaje.LblMensaje.Text = ex.ToString();
                 catchMensaje.ShowDialog();
             }
         }

        //FUNCION PARA CARGAR LA INFORMACION DEL CLIENTE
         private void cargarInformacionCliente()
         {
             try
             {
                 sSql = "";
                 sSql += "select * from pos_vw_cargar_informacion_cliente" + Environment.NewLine;
                 sSql += "where id_pedido = " + sIdOrden;

                 dtConsulta = new DataTable();
                 dtConsulta.Clear();

                 bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                 if (bRespuesta == true)
                 {
                     iIdPersona = Convert.ToInt32(dtConsulta.Rows[0]["id_persona"].ToString());
                     iNumeroCuenta_P = Convert.ToInt32(dtConsulta.Rows[0]["cuenta"].ToString());
                     iNumeroPedido_P = Convert.ToInt32(dtConsulta.Rows[0]["numero_pedido"].ToString());

                     if (iIdPersona == Program.iIdPersona)
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
                         btnEditar.Visible = false;
                     }

                     else
                     {
                         txtIdentificacion.Text = dtConsulta.Rows[0]["identificacion"].ToString();
                         txtNombres.Text = dtConsulta.Rows[0]["nombres"].ToString();
                         txtApellidos.Text = dtConsulta.Rows[0]["apellidos"].ToString();
                         txtMail.Text = dtConsulta.Rows[0]["correo_electronico"].ToString();
                         txtDireccion.Text = dtConsulta.Rows[0]["direccion_completa"].ToString();
                         sCiudad = dtConsulta.Rows[0]["direccion"].ToString();

                         if (dtConsulta.Rows[0]["telefono_domicilio"].ToString() != "")
                         {
                             txtTelefono.Text = dtConsulta.Rows[0]["telefono_domicilio"].ToString();
                         }

                         else if (dtConsulta.Rows[0]["celular"].ToString() != "")
                         {
                             txtTelefono.Text = dtConsulta.Rows[0]["celular"].ToString();
                         }

                         else
                         {
                             txtTelefono.Text = "";
                         }
                     }
                 }

                 else
                 {
                     catchMensaje.LblMensaje.Text = "ERROR EN LA INSTRUCCIÓN:" + Environment.NewLine + sSql;
                     catchMensaje.ShowDialog();
                 }
             }
             catch (Exception ex)
             {
                 catchMensaje.LblMensaje.Text = ex.ToString();
                 catchMensaje.ShowDialog();
             }
         }

        //FUNCION PARA CARGAR TODAS LAS FORMAS DE PAGO
         private void cargarFormasPagos()
         {
             try
             {
                 sSql = "";
                 sSql += "select FC.id_pos_tipo_forma_cobro, FC.codigo, FC.descripcion," + Environment.NewLine;
                 sSql += "isnull(FC.imagen, '') imagen, MP.id_sri_forma_pago" + Environment.NewLine;
                 sSql += "from pos_tipo_forma_cobro FC INNER JOIN" + Environment.NewLine;
                 sSql += "pos_metodo_pago MP ON MP.id_pos_metodo_pago = FC.id_pos_metodo_pago" + Environment.NewLine;
                 sSql += "and FC.estado = 'A'" + Environment.NewLine;
                 sSql += "and MP.estado = 'A'";

                 dtFormasPago = new DataTable();
                 dtFormasPago.Clear();

                 bRespuesta = conexion.GFun_Lo_Busca_Registro(dtFormasPago, sSql);

                 if (!bRespuesta)
                 {
                     catchMensaje.LblMensaje.Text = "ERROR EN LA INSTRUCCIÓN SQL:" + Environment.NewLine + sSql;
                     catchMensaje.ShowDialog();
                 }

                 else
                 {
                     iCuentaFormasPagos = 0;

                     if (dtFormasPago.Rows.Count > 0)
                     {
                         if (dtFormasPago.Rows.Count > 8)
                         {
                             btnSiguiente.Enabled = true;
                         }

                         else
                         {
                             btnSiguiente.Enabled = false;
                         }

                         if (crearBotonesFormasPagos() == true)
                         { }

                     }

                     else
                     {
                         ok.LblMensaje.Text = "No se encuentras ítems de categorías en el sistema.";
                         ok.ShowDialog();
                     }
                 }
             }

             catch (Exception ex)
             {
                 catchMensaje.LblMensaje.Text = ex.Message;
                 catchMensaje.ShowDialog();
             }
         }

        //FUNCION PARA CARGAR FORMAS DE PAGO CON RECARGO
         private void cargarFormasPagosRecargo()
         {
             try
             {
                 sSql = "";
                 sSql += "select FC.id_pos_tipo_forma_cobro, FC.codigo, FC.descripcion," + Environment.NewLine;
                 sSql += "isnull(FC.imagen, '') imagen, MP.id_sri_forma_pago" + Environment.NewLine;
                 sSql += "from pos_tipo_forma_cobro FC INNER JOIN" + Environment.NewLine;
                 sSql += "pos_metodo_pago MP ON MP.id_pos_metodo_pago = FC.id_pos_metodo_pago" + Environment.NewLine;
                 sSql += "and FC.estado = 'A'" + Environment.NewLine;
                 sSql += "and MP.estado = 'A'" + Environment.NewLine;
                 sSql += "where MP.codigo in ('TC', 'TD')";

                 dtFormasPago = new DataTable();
                 dtFormasPago.Clear();

                 bRespuesta = conexion.GFun_Lo_Busca_Registro(dtFormasPago, sSql);

                 if (!bRespuesta)
                 {
                     catchMensaje.LblMensaje.Text = "ERROR EN LA INSTRUCCIÓN SQL:" + Environment.NewLine + sSql;
                     catchMensaje.ShowDialog();
                 }

                 else
                 {
                     iCuentaFormasPagos = 0;

                     if (dtFormasPago.Rows.Count > 0)
                     {
                         if (dtFormasPago.Rows.Count > 8)
                         {
                             btnSiguiente.Enabled = true;
                         }

                         else
                         {
                             btnSiguiente.Enabled = false;
                         }

                         if (crearBotonesFormasPagos() == true)
                         { }

                     }

                     else
                     {
                         ok.LblMensaje.Text = "No se encuentras ítems de categorías en el sistema.";
                         ok.ShowDialog();
                     }
                 }
             }

             catch (Exception ex)
             {
                 catchMensaje.LblMensaje.Text = ex.Message;
                 catchMensaje.ShowDialog();
             }
         }

        //FUNCION PARA CREAR LOS BOTONS DE TODAS LAS FORMAS DE PAGO
         private bool crearBotonesFormasPagos()
         {
             try
             {
                 pnlFormasCobros.Controls.Clear();
                 iPosXFormasPagos = 0;
                 iPosYFormasPagos = 0;
                 iCuentaAyudaFormasPagos = 0;

                 for (int i = 0; i < 5; ++i)
                 {
                     for (int j = 0; j < 2; ++j)
                     {
                         boton[i, j] = new Button();
                         boton[i, j].Cursor = Cursors.Hand;
                         boton[i, j].Click += new EventHandler(boton_clic);
                         boton[i, j].Size = new Size(153, 71);
                         boton[i, j].Location = new Point(iPosXFormasPagos, iPosYFormasPagos);
                         boton[i, j].BackColor = Color.Lime;
                         boton[i, j].Font = new Font("Maiandra GD", 9.75f, FontStyle.Bold);
                         boton[i, j].Tag = (object)dtFormasPago.Rows[iCuentaFormasPagos]["id_pos_tipo_forma_cobro"].ToString();
                         boton[i, j].Text = dtFormasPago.Rows[iCuentaFormasPagos]["descripcion"].ToString();
                         boton[i, j].AccessibleDescription = dtFormasPago.Rows[iCuentaFormasPagos]["id_sri_forma_pago"].ToString();
                         boton[i, j].TextAlign = ContentAlignment.MiddleCenter;

                         if (dtFormasPago.Rows[iCuentaFormasPagos]["imagen"].ToString().Trim() != "" && File.Exists(dtFormasPago.Rows[iCuentaFormasPagos]["imagen"].ToString().Trim()))
                         {
                             boton[i, j].TextAlign = ContentAlignment.MiddleRight;
                             boton[i, j].Image = Image.FromFile(dtFormasPago.Rows[iCuentaFormasPagos]["imagen"].ToString().Trim());
                             boton[i, j].ImageAlign = ContentAlignment.MiddleLeft;
                             boton[i, j].BackgroundImageLayout = ImageLayout.Stretch;
                         }

                         pnlFormasCobros.Controls.Add(boton[i, j]);
                         ++iCuentaFormasPagos;
                         ++iCuentaAyudaFormasPagos;

                         if (j + 1 == 2)
                         {
                             iPosXFormasPagos = 0;
                             iPosYFormasPagos += 71;
                         }

                         else
                         {
                             iPosXFormasPagos += 153;
                         }

                         if (dtFormasPago.Rows.Count == iCuentaFormasPagos)
                         {
                             btnSiguiente.Enabled = false;
                             break;
                         }
                     }

                     if (dtFormasPago.Rows.Count == iCuentaFormasPagos)
                     {
                         btnSiguiente.Enabled = false;
                         break;
                     }
                 }
                 return true;
             }

             catch (Exception ex)
             {
                 catchMensaje.LblMensaje.Text = ex.Message;
                 catchMensaje.ShowDialog();
                 return false;
             }
         }

        //EVENTO CLIC DE LAS FORMAS DE PAGO
         public void boton_clic(object sender, EventArgs e)
         {
             bpagar = sender as Button;

             if (Convert.ToDouble(dgvDetalleDeuda.Rows[1].Cells[1].Value) == 0.0)
             {
                 ok.LblMensaje.Text = "El saldo actual a pagar ya es de 0.00";
                 ok.ShowDialog();
             }

             else
             {
                 Efectivo efectivo = new Efectivo(bpagar.Tag.ToString(), dgvDetalleDeuda.Rows[1].Cells[1].Value.ToString(), "", bpagar.Text.ToString());
                 efectivo.ShowDialog();

                 if (efectivo.DialogResult == DialogResult.OK)
                 {
                     dbValorGrid = efectivo.dbValorGrid;
                     dbValorRecuperado = efectivo.dbValorIngresado;
                     dgvPagos.Rows.Add(efectivo.sIdPago, efectivo.sNombrePago, dbValorGrid.ToString("N2"), bpagar.AccessibleDescription);
                     dgvPagos.ClearSelection();
                     efectivo.Close();
                     recalcularValores();
                 }
             }
         }

        //FUNCION PARA RECALCULAR LOS VALORES
         private void recalcularValores()
         {
             try
             {
                 dgvDetalleDeuda.Rows[0].Cells[1].Value = (Convert.ToDecimal(dgvDetalleDeuda.Rows[0].Cells[1].Value) + dbValorRecuperado).ToString("N2");
                 dgvDetalleDeuda.Rows[1].Cells[1].Value = (dTotal - Convert.ToDecimal(dgvDetalleDeuda.Rows[0].Cells[1].Value)).ToString("N2");

                 if (Convert.ToDecimal(dgvDetalleDeuda.Rows[1].Cells[1].Value) < 0)
                 {
                     dgvDetalleDeuda.Rows[1].Cells[1].Value = "0.00";
                 }

                 dgvDetalleDeuda.Rows[2].Cells[1].Value = (Convert.ToDecimal(dgvDetalleDeuda.Rows[0].Cells[1].Value) - dTotal).ToString("N2");

                 if (Convert.ToDouble(dgvDetalleDeuda.Rows[2].Cells[1].Value) < 0)
                 {
                     dgvDetalleDeuda.Rows[2].Cells[1].Value = "0.00";
                 }

                 if (Convert.ToDouble(dgvDetalleDeuda.Rows[2].Cells[1].Value) <= 0)
                 {
                     return;
                 }

                 Program.dCambioPantalla = Convert.ToDouble(dgvDetalleDeuda.Rows[2].Cells[1].Value);
             }

             catch (Exception ex)
             {
             }
         }

        //FUNCION PARA EXTRAER LA INFORMACION PARA FACTURAR
         private void datosFactura()
         {
             try
             {
                 sSql = "";
                 sSql += "select L.id_localidad, L.establecimiento, L.punto_emision, " + Environment.NewLine;
                 sSql += "P.numero_factura, P.numeronotaentrega, P.numeromovimientocaja, P.id_localidad_impresora" + Environment.NewLine;
                 sSql += "from tp_localidades L, tp_localidades_impresoras P " + Environment.NewLine;
                 sSql += "where L.id_localidad = P.id_localidad" + Environment.NewLine;
                 sSql += "and L.id_localidad = " + (object)Program.iIdLocalidad + Environment.NewLine;
                 sSql += "and L.estado = 'A'" + Environment.NewLine;
                 sSql += "and P.estado = 'A'";

                 dtConsulta = new DataTable();
                 dtConsulta.Clear();

                 bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                 if (bRespuesta == true)
                 {
                     if (dtConsulta.Rows.Count == 0)
                     {
                         ok.LblMensaje.Text = "No se encuentran registros en la consulta.";
                         ok.ShowDialog();
                     }

                     else
                     {
                         txtfacturacion.Text = dtConsulta.Rows[0]["establecimiento"].ToString() + "-" + dtConsulta.Rows[0]["punto_emision"].ToString();

                         if (rdbFactura.Checked)
                         {
                             TxtNumeroFactura.Text = dtConsulta.Rows[0]["numero_factura"].ToString();
                         }

                         else if (rdbNotaVenta.Checked)
                         {
                             TxtNumeroFactura.Text = dtConsulta.Rows[0]["numeronotaentrega"].ToString();
                         }

                         iNumeroMovimientoCaja = Convert.ToInt32(dtConsulta.Rows[0]["numeromovimientocaja"].ToString());
                         iIdLocalidadImpresora = Convert.ToInt32(dtConsulta.Rows[0]["id_localidad_impresora"].ToString());
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

        //FUNCION PARA CONSULTAR DATOS DEL CLIENTE
         private void consultarRegistro()
         {
             try
             {
                 sSql = "";
                 sSql += "SELECT TP.id_persona, TP.identificacion, TP.nombres, TP.apellidos, TP.correo_electronico," + Environment.NewLine;
                 sSql += "TD.direccion + ', ' + TD.calle_principal + ' ' + TD.numero_vivienda + ' ' + TD.calle_interseccion," + Environment.NewLine;
                 sSql += conexion.GFun_St_esnulo() + "(TT.domicilio, TT.oficina) telefono_domicilio, TT.celular, TD.direccion" + Environment.NewLine;
                 sSql += "FROM tp_personas TP" + Environment.NewLine;
                 sSql += "LEFT OUTER JOIN tp_direcciones TD ON TP.id_persona = TD.id_persona" + Environment.NewLine;
                 sSql += "and TP.estado = 'A'" + Environment.NewLine;
                 sSql += "and TD.estado = 'A'" + Environment.NewLine;
                 sSql += "LEFT OUTER JOIN tp_telefonos TT ON TP.id_persona = TT.id_persona" + Environment.NewLine;
                 sSql += "and TT.estado = 'A'" + Environment.NewLine;
                 sSql += "WHERE  TP.identificacion = '" + txtIdentificacion.Text.Trim() + "'";

                 dtConsulta = new DataTable();
                 dtConsulta.Clear();

                 bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                 if (bRespuesta)
                 {
                     if (dtConsulta.Rows.Count > 0)
                     {
                         iIdPersona = Convert.ToInt32(dtConsulta.Rows[0][0].ToString());
                         txtNombres.Text = dtConsulta.Rows[0][2].ToString();
                         txtApellidos.Text = dtConsulta.Rows[0][3].ToString();
                         txtMail.Text = dtConsulta.Rows[0][4].ToString();
                         txtDireccion.Text = dtConsulta.Rows[0][5].ToString();
                         sCiudad = dtConsulta.Rows[0][8].ToString();

                         if (dtConsulta.Rows[0][6].ToString() != "")
                         {
                             txtTelefono.Text = dtConsulta.Rows[0][6].ToString();
                         }

                         else if (dtConsulta.Rows[0][7].ToString() != "")
                         {
                             txtTelefono.Text = dtConsulta.Rows[0][7].ToString();
                         }

                         else
                         {
                             txtTelefono.Text = "";
                         }

                         btnFacturar.Focus();
                     }

                     else
                     {
                         Facturador.frmNuevoCliente frmNuevoCliente = new Facturador.frmNuevoCliente(txtIdentificacion.Text.Trim(), chkPasaporte.Checked);
                         frmNuevoCliente.ShowDialog();

                         if (frmNuevoCliente.DialogResult == DialogResult.OK)
                         {
                             iIdPersona = frmNuevoCliente.iCodigo;
                             txtIdentificacion.Text = frmNuevoCliente.sIdentificacion;
                             txtNombres.Text = frmNuevoCliente.sNombre;
                             txtApellidos.Text = frmNuevoCliente.sApellido;
                             txtTelefono.Text = frmNuevoCliente.sTelefono;
                             txtDireccion.Text = frmNuevoCliente.sDireccion;
                             txtMail.Text = frmNuevoCliente.sMail;
                             sCiudad = frmNuevoCliente.sCiudad;
                             frmNuevoCliente.Close();
                             btnFacturar.Focus();
                         }
                     }

                     btnEditar.Visible = true;
                     goto continua;
                 }
             }
             catch (Exception ex)
             {
                 ok.LblMensaje.Text = "Ocurrió un problema al realizar la consulta.";
                 ok.ShowDialog();
                 txtIdentificacion.Clear();
                 txtIdentificacion.Focus();
             }            

         continua:
             iBanderaDomicilio = 1;
         }

        //FUNCION PARA VALIDAR LA IDENTIFICACION
         private void validarIdentificacion()
         {
             try
             {
                 if (txtIdentificacion.Text.Length >= 10)
                 {
                     iTercerDigito = Convert.ToInt32(txtIdentificacion.Text.Substring(2, 1));

                     if (txtIdentificacion.Text.Length == 10)
                     {
                         if (validarCedula.validarCedulaConsulta(txtIdentificacion.Text.Trim()) == "SI")
                         {
                             consultarRegistro();
                             return;
                         }
                     }

                     else if (txtIdentificacion.Text.Length == 13)
                     {
                         if (iTercerDigito == 9)
                         {
                             if (validarRuc.validarRucPrivado(txtIdentificacion.Text.Trim()))
                             {
                                 consultarRegistro();
                                 return;
                             }
                         }

                         else if (iTercerDigito == 6)
                         {
                             if (validarRuc.validarRucPublico(txtIdentificacion.Text.Trim()))
                             {
                                 consultarRegistro();
                                 return;
                             }
                         }

                         else if (iTercerDigito <= 5 || iTercerDigito >= 0)
                         {
                             if (validarRuc.validarRucNatural(txtIdentificacion.Text.Trim()))
                             {
                                 consultarRegistro();
                                 return;
                             }
                         }
                     }
                 }
             }

             catch (Exception ex)
             {
                 ok.LblMensaje.Text = "El número de identificación ingresado es incorrecto.";
                 ok.ShowDialog();
                 txtIdentificacion.Clear();
                 txtIdentificacion.Focus();
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

        //CREAR DATATABLE ´PARA EL GRID DE DEUDA
         private void llenarDetalleDeuda()
         {
             try
             {
                 dgvDetalleDeuda.Rows.Add("ABONO", "0.00");
                 dgvDetalleDeuda.Rows.Add("SALDO", "0.00");
                 dgvDetalleDeuda.Rows.Add("CAMBIO", "0.00");
                 dgvDetalleDeuda.Rows.Add("PROPINA", "0.00");
                 dgvDetalleDeuda.ClearSelection();
             }
             catch (Exception ex)
             {
             }
         }

        //LLENAR EL GRID DE PAGOS
         private void llenarGrid()
         {
             try
             {
                 sSql = "";
                 sSql += "select * from pos_vw_pedido_forma_pago" + Environment.NewLine;
                 sSql += "where id_pedido = " + Convert.ToInt32(sIdOrden);

                 dtConsulta = new DataTable();
                 dtConsulta.Clear();

                 bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                 if ((bRespuesta == false) || (dtConsulta.Rows.Count <= 0))
                 {
                     return;
                 }

                 Decimal num = new Decimal(0);

                 for (int i = 0; i < dtConsulta.Rows.Count; ++i)
                 {
                     dgvPagos.Rows.Add();
                     dgvPagos.Rows[i].Cells["id"].Value = dtConsulta.Rows[i]["id_pos_tipo_forma_cobro"].ToString();
                     dgvPagos.Rows[i].Cells["fpago"].Value = dtConsulta.Rows[i]["descripcion"].ToString();
                     dValor = Convert.ToDecimal(dtConsulta.Rows[i]["valor"].ToString());
                     dgvPagos.Rows[i].Cells["valor"].Value = dValor.ToString("N2");
                     dgvPagos.Rows[i].Cells["id_sri"].Value = dtConsulta.Rows[i]["id_sri_forma_pago"].ToString();
                     num += dValor;
                 }

                 dgvPagos.Columns[0].Visible = false;
             }

             catch (Exception ex)
             {
                 catchMensaje.LblMensaje.Text = ex.ToString();
                 catchMensaje.ShowDialog();
             }
         }

        //FUNCION PARA CARGAR LOS VALORES DE LA PRECUENTA
         private void valoresPrecuenta()
         {
             try
             {
                 dbSumaIva = 0;
                 sSql = "";
                 sSql += "select cantidad, precio_unitario, valor_dscto," + Environment.NewLine;
                 sSql += "valor_iva, valor_otro, nombre, paga_iva, id_det_pedido, id_producto" + Environment.NewLine;
                 sSql += "from pos_vw_det_pedido" + Environment.NewLine;
                 sSql += "where id_pedido = " + Convert.ToInt32(sIdOrden);

                 dtComanda = new DataTable();
                 dtComanda.Clear();
                 bRespuesta = conexion.GFun_Lo_Busca_Registro(dtComanda, sSql);

                 if (bRespuesta == true)
                 {
                     for (int i = 0; i < dtComanda.Rows.Count; ++i)
                     {
                         if (Convert.ToDouble(dtComanda.Rows[i]["valor_iva"].ToString()) == 0.0 && Convert.ToInt32(dtComanda.Rows[i]["paga_iva"].ToString()) == 1)
                         {
                             dbRecalcularPrecioUnitario = Convert.ToDecimal(dtComanda.Rows[i]["precio_unitario"].ToString());
                             dbRecalcularDescuento = Convert.ToDecimal(dtComanda.Rows[i]["valor_dscto"].ToString());
                             dbRecalcularIva = (dbRecalcularPrecioUnitario - dbRecalcularDescuento) * Convert.ToDecimal(Program.iva);
                             dtComanda.Rows[i]["valor_iva"] = dbRecalcularIva.ToString();
                         }
                         dbSumaIva += Convert.ToDecimal(dtComanda.Rows[i]["cantidad"].ToString()) * Convert.ToDecimal(dtComanda.Rows[i]["valor_iva"].ToString());
                     }
                 }

                 else
                 {
                     catchMensaje.LblMensaje.Text = "ERROR EN LA INSTRUCCIÓN:" + Environment.NewLine + sSql;
                     catchMensaje.ShowDialog();
                 }
             }

             catch (Exception ex)
             {
                 catchMensaje.LblMensaje.Text = ex.ToString();
                 catchMensaje.ShowDialog();
             }
         }

        //FUNCION PARA CAMBIAR LAS FORMAS DE PAGO
         private void cambiarFormasPagos(int iOp)
         {
             try
             {
                 if (Convert.ToDouble(dgvDetalleDeuda.Rows[1].Cells[1].Value) != 0.0 && iOp == 1)
                 {
                     ok.LblMensaje.Text = "No ha realizado el cobro completo de la comanda.";
                     ok.ShowDialog();
                     return;
                 }

                 string sFecha_P = Program.sFechaSistema.ToString("yyyy/MM/dd");

                 if (!conexion.GFun_Lo_Maneja_Transaccion(Program.G_INICIA_TRANSACCION))
                 {
                     ok.LblMensaje.Text = "Error al abrir transacción.";
                     ok.ShowDialog();
                     return;
                 }

                 sSql = "";
                 sSql += "select id_documento_pago" + Environment.NewLine;
                 sSql += "from pos_vw_pedido_forma_pago" + Environment.NewLine;
                 sSql += "where id_pedido = " + Convert.ToInt32(sIdOrden);

                 dtConsulta = new DataTable();
                 dtConsulta.Clear();

                 bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                 if (bRespuesta == false)
                 {
                     catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                     catchMensaje.ShowDialog();
                     goto reversa;
                 }

                 for (int i = 0; i < dtConsulta.Rows.Count; i++)
                 {
                     sSql = "";
                     sSql += "update pos_movimiento_caja set" + Environment.NewLine;
                     sSql += "estado = 'E'," + Environment.NewLine;
                     sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                     sSql += "usuario_anula = '" + Program.sDatosMaximo[0] + "'," + Environment.NewLine;
                     sSql += "terminal_anula = '" + Program.sDatosMaximo[1] + "'" + Environment.NewLine;
                     sSql += "where id_documento_pago = " + Convert.ToInt32(dtConsulta.Rows[i][0].ToString()) + Environment.NewLine;

                     if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                     {
                         catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                         catchMensaje.ShowDialog();
                         goto reversa;
                     }
                 }

                 if (iBanderaRecargoBoton == 1)
                 {
                     if (Program.iAplicaRecargoTarjeta == 1 && iBanderaRecargoBDD == 1)
                     {
                         if (actualizarPreciosRecargo() == false)
                         {
                             goto reversa;
                         }
                     }
                 }

                 else if (iBanderaRemoverIvaBoton == 1)
                 {
                     if (Program.iDescuentaIva == 1)
                     {
                         if (actualizarIVACero() == false)
                         {
                             goto reversa;
                         }
                     }
                 }

                 else if (Program.iDescuentaIva == 1 || Program.iAplicaRecargoTarjeta == 1)
                 {
                     if (actualizarPreciosOriginales() == false)
                     {
                         goto reversa;
                     }
                 }

                 sSql = "";
                 sSql += "select id_documento_cobrar" + Environment.NewLine;
                 sSql += "from cv403_dctos_por_cobrar" + Environment.NewLine;
                 sSql += "where id_pedido = " + sIdOrden + Environment.NewLine;
                 sSql += "and estado = 'A'";

                 dtConsulta = new DataTable();
                 dtConsulta.Clear();

                 bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                 if (bRespuesta == false)
                 {
                     catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                     catchMensaje.ShowDialog();
                     goto reversa;
                 }

                 iIdDocumentoCobrar = Convert.ToInt32(dtConsulta.Rows[0]["id_documento_cobrar"].ToString());
                 iCuenta = 0;

                 sSql = "";
                 sSql += "select count(*) cuenta" + Environment.NewLine;
                 sSql += "from  cv403_documentos_pagados" + Environment.NewLine;
                 sSql += "where id_documento_cobrar = " + iIdDocumentoCobrar + Environment.NewLine;
                 sSql += "and estado = 'A'";

                 dtConsulta = new DataTable();
                 dtConsulta.Clear();

                 bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                 if (bRespuesta == false)
                 {
                     catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                     catchMensaje.ShowDialog();
                     goto reversa;
                 }

                 iCuenta = Convert.ToInt32(dtConsulta.Rows[0][0].ToString());

                 if (iCuenta > 0)
                 {
                     sSql = "";
                     sSql += "select id_pago, id_documento_pagado" + Environment.NewLine;
                     sSql += "from  cv403_documentos_pagados" + Environment.NewLine;
                     sSql += "where id_documento_cobrar = " + iIdDocumentoCobrar + Environment.NewLine;
                     sSql += "and estado = 'A'";

                     dtConsulta = new DataTable();
                     dtConsulta.Clear();

                     bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                     if (bRespuesta == false)
                     {
                         catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                         catchMensaje.ShowDialog();
                         goto reversa;
                     }

                     if (dtConsulta.Rows.Count > 0)
                     {
                         iIdPago = Convert.ToInt32(dtConsulta.Rows[0][0].ToString());
                         iIdDocumentoPagado = Convert.ToInt32(dtConsulta.Rows[0][1].ToString());
                     }

                     sSql = "";
                     sSql += "update cv403_pagos set" + Environment.NewLine;
                     sSql += "estado = 'E'," + Environment.NewLine;
                     sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                     sSql += "usuario_anula = '" + Program.sDatosMaximo[0] + "'," + Environment.NewLine;
                     sSql += "terminal_anula = '" + Program.sDatosMaximo[1] + "'" + Environment.NewLine;
                     sSql += "where id_pago = " + iIdPago;

                     if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                     {
                         catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                         catchMensaje.ShowDialog();
                         goto reversa;
                     }

                     sSql = "";
                     sSql += "update cv403_documentos_pagos set" + Environment.NewLine;
                     sSql += "estado = 'E'," + Environment.NewLine;
                     sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                     sSql += "usuario_anula = '" + Program.sDatosMaximo[0] + "'," + Environment.NewLine;
                     sSql += "terminal_anula = '" + Program.sDatosMaximo[1] + "'" + Environment.NewLine;
                     sSql += "where id_pago = " + iIdPago;

                     if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                     {
                         catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                         catchMensaje.ShowDialog();
                         goto reversa;
                     }

                     sSql = "";
                     sSql += "update cv403_numeros_pagos set" + Environment.NewLine;
                     sSql += "estado = 'E'," + Environment.NewLine;
                     sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                     sSql += "usuario_anula = '" + Program.sDatosMaximo[0] + "'," + Environment.NewLine;
                     sSql += "terminal_anula = '" + Program.sDatosMaximo[1] + "'" + Environment.NewLine;
                     sSql += "where id_pago = " + iIdPago;

                     if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                     {
                         catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                         catchMensaje.ShowDialog();
                         goto reversa;
                     }

                     sSql = "";
                     sSql += "update cv403_documentos_pagados set" + Environment.NewLine;
                     sSql += "estado = 'E'," + Environment.NewLine;
                     sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                     sSql += "usuario_anula = '" + Program.sDatosMaximo[0] + "'," + Environment.NewLine;
                     sSql += "terminal_anula = '" + Program.sDatosMaximo[1] + "'" + Environment.NewLine;
                     sSql += "where id_documento_pagado = " + iIdDocumentoPagado;

                     if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                     {
                         catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                         catchMensaje.ShowDialog();
                         goto reversa;
                     }
                 }

                 sSql = "";
                 sSql += "insert into cv403_pagos (" + Environment.NewLine;
                 sSql += "idempresa, id_persona, fecha_pago, cg_moneda, valor," + Environment.NewLine;
                 sSql += "propina, cg_empresa, id_localidad, cg_cajero, fecha_ingreso," + Environment.NewLine;
                 sSql += "usuario_ingreso, terminal_ingreso, estado, " + Environment.NewLine;
                 sSql += "numero_replica_trigger, numero_control_replica,cambio) " + Environment.NewLine;
                 sSql += "values(" + Environment.NewLine;
                 sSql += Program.iIdEmpresa + ", " + Program.iIdPersona + ", '" + sFecha_P + "', " + Program.iMoneda + "," + Environment.NewLine;
                 sSql += dTotal + ", " + Convert.ToDouble(dgvDetalleDeuda.Rows[3].Cells[1].Value) + ", " + Program.iCgEmpresa + "," + Environment.NewLine;
                 sSql += Program.iIdLocalidad + ", 7799, GETDATE(), '" + Program.sDatosMaximo[0] + "'," + Environment.NewLine;
                 sSql += "'" + Program.sDatosMaximo[1] + "', 'A' , 1, 0, " + Convert.ToDouble(dgvDetalleDeuda.Rows[2].Cells[1].Value) + ")";
                 
                 if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                 {
                     catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                     catchMensaje.ShowDialog();
                     goto reversa;
                 }

                 dtConsulta = new DataTable();
                 dtConsulta.Clear();
                 sTabla = "cv403_pagos";
                 sCampo = "id_pago";

                 iMaximo = conexion.GFun_Ln_Saca_Maximo_ID(sTabla, sCampo, "", Program.sDatosMaximo);

                 if (iMaximo == -1)
                 {
                    ok.LblMensaje.Text = "No se pudo obtener el codigo de la tabla " + sTabla;
                    ok.ShowDialog();
                    goto reversa;
                 }

                 else
                 {
                     iIdPago = Convert.ToInt32(iMaximo);
                 }

                 sSql = "";
                 sSql += "select numero_pago" + Environment.NewLine;
                 sSql += "from tp_localidades_impresoras" + Environment.NewLine;
                 sSql += "where id_localidad = " + Program.iIdLocalidad + Environment.NewLine;
                 sSql += "and estado = 'A'";
                 
                 dtConsulta = new DataTable();
                 dtConsulta.Clear();

                 bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                 if (bRespuesta == false)
                 {
                     catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                     catchMensaje.ShowDialog();
                     goto reversa;
                 }

                 sSql = "";
                 sSql += "insert into cv403_numeros_pagos (" + Environment.NewLine;
                 sSql += "id_pago, serie, numero_pago, fecha_ingreso, usuario_ingreso," + Environment.NewLine;
                 sSql += "terminal_ingreso, estado, numero_replica_trigger, numero_control_replica)" + Environment.NewLine;
                 sSql += "values(" + Environment.NewLine;
                 sSql += iIdPago + ", 'A', " + iNumeroPago + ", GETDATE(), '" + Program.sDatosMaximo[0] + "'," + Environment.NewLine;
                 sSql += "'" + Program.sDatosMaximo[1] + "', 'A', 1, 0)";
                             
                 if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                 {
                     catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                     catchMensaje.ShowDialog();
                     goto reversa;
                 }

                 for (int i = 0; i < dgvPagos.Rows.Count; i++)
                 {
                     sSql = "";
                     sSql += "select cg_tipo_documento" + Environment.NewLine;
                     sSql += "from pos_tipo_forma_cobro " + Environment.NewLine;
                     sSql += "where id_pos_tipo_forma_cobro = " + dgvPagos.Rows[i].Cells[0].Value;
                     
                     dtConsulta = new DataTable();
                     dtConsulta.Clear();
                     bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                     if (bRespuesta == false)
                     {
                         catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                         catchMensaje.ShowDialog();
                         goto reversa;
                     }

                     iCgTipoDocumento = Convert.ToInt32(dtConsulta.Rows[0][0].ToString());
                     sSql = "";
                     sSql += "insert into cv403_documentos_pagos (" + Environment.NewLine;
                     sSql += "id_pago, cg_tipo_documento, numero_documento, fecha_vcto, " + Environment.NewLine;
                     sSql += "cg_moneda, cotizacion, valor, id_pos_tipo_forma_cobro," + Environment.NewLine;
                     sSql += "estado, fecha_ingreso, usuario_ingreso, terminal_ingreso," + Environment.NewLine;
                     sSql += "numero_replica_trigger, numero_control_replica, valor_recibido) " + Environment.NewLine;
                     sSql += "values(" + Environment.NewLine;
                     sSql += iIdPago + ", " + iCgTipoDocumento + ", 9999, '" + sFecha_P + "', " + Environment.NewLine;
                     sSql += Program.iMoneda + ", 1, " + Convert.ToDouble(dgvPagos.Rows[i].Cells[2].Value) + "," + Environment.NewLine;
                     sSql += Convert.ToInt32(dgvPagos.Rows[i].Cells[0].Value) + ", 'A', GETDATE()," + Environment.NewLine;
                     sSql += "'" + Program.sDatosMaximo[0] + "', '" + Program.sDatosMaximo[1] + "', 1, 0,";
                     
                     if (Convert.ToInt32(dgvPagos.Rows[i].Cells[0].Value) == 1)
                     {
                         sSql += (Convert.ToDouble(dgvPagos.Rows[i].Cells[2].Value) + Convert.ToDouble(dgvDetalleDeuda.Rows[2].Cells[1].Value));
                     }
                     
                     else
                     {
                         sSql += "null";
                     }
                     
                     sSql += ")";
                     
                     if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                     {
                         catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                         catchMensaje.ShowDialog();
                         goto reversa;
                     }

                     if (Convert.ToInt32(dgvPagos.Rows[i].Cells[0].Value) != 1 || Convert.ToInt32(dgvPagos.Rows[i].Cells[0].Value) != 11)
                     {
                         dtConsulta = new DataTable();
                         dtConsulta.Clear();
                         
                         sTabla = "cv403_documentos_pagos";
                         sCampo = "id_documento_pago";
                         
                         iMaximo = conexion.GFun_Ln_Saca_Maximo_ID(sTabla, sCampo, "", Program.sDatosMaximo);
                         
                         if (iMaximo == -1)
                         {
                             ok.LblMensaje.Text = "No se pudo obtener el codigo de la tabla " + sTabla;
                             ok.ShowDialog();
                             goto reversa;
                         }
                         
                         else
                         {
                             iIdDocumentoPago = Convert.ToInt32(iMaximo);
                         }
                     }
                 }

                 sSql = "";
                 sSql += "insert into cv403_documentos_pagados (" + Environment.NewLine;
                 sSql += "id_documento_cobrar, id_pago, valor, " + Environment.NewLine;
                 sSql += "estado, numero_replica_trigger,numero_control_replica," + Environment.NewLine;
                 sSql += "fecha_ingreso, usuario_ingreso, terminal_ingreso) " + Environment.NewLine;
                 sSql += "values (" + Environment.NewLine;
                 sSql += iIdDocumentoCobrar + ", " + iIdPago + ", " + dTotal + ", 'A', 1, 0," + Environment.NewLine;
                 sSql += "GETDATE(), '" + Program.sDatosMaximo[0] + "', '" + Program.sDatosMaximo[1] + "')";
                                 
                 if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                 {
                     catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                     catchMensaje.ShowDialog();
                     goto reversa;
                 }

                 sSql = "";
                 sSql += "update tp_localidades_impresoras set" + Environment.NewLine;
                 sSql += "numero_pago = numero_pago + 1" + Environment.NewLine;
                 sSql += "where id_localidad = " + Program.iIdLocalidad + Environment.NewLine;
                 
                 if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                 {
                     catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                     int num = (int)catchMensaje.ShowDialog();
                     goto reversa;
                 }

                 if (iOpCambiarEstadoOrden == 1)
                 {
                     sSql = "";
                     sSql += "update cv403_cab_pedidos set" + Environment.NewLine;
                     sSql += "estado_orden = 'Pagada'" + Environment.NewLine;
                     sSql += "where id_pedido = " + Convert.ToInt32(sIdOrden) + Environment.NewLine;
                     sSql += "and estado = 'A'";
                                          
                     if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                     {
                         catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                         catchMensaje.ShowDialog();
                         goto reversa;
                     }
                 }

                 if (iOp == 1)
                 {
                     if (!actualizarMovimientosCaja())
                     {
                         goto reversa;
                     }
                 }

                 sSql = "";
                 sSql += "update cv403_cab_pedidos set" + Environment.NewLine;
                 sSql += "recargo_tarjeta = " + (object)iBanderaRecargoBoton + "," + Environment.NewLine;
                 sSql += "remover_iva = " + (object)iBanderaRemoverIvaBoton + Environment.NewLine;
                 sSql += "where id_pedido = " + sIdOrden;

                 if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                 {
                     catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                     catchMensaje.ShowDialog();
                     goto reversa;
                 }
                 conexion.GFun_Lo_Maneja_Transaccion(Program.G_TERMINA_TRANSACCION);
                 ok.LblMensaje.Text = "Las formas de pago se han actualizado éxitosamente";
                 ok.ShowDialog();
                 this.DialogResult = DialogResult.OK;
                 return;
             }

             catch (Exception ex)
             {
                 catchMensaje.LblMensaje.Text = ex.ToString();
                 catchMensaje.ShowDialog();
             }

            reversa: { conexion.GFun_Lo_Maneja_Transaccion(Program.G_REVERSA_TRANSACCION); }
         }

        //FUNCION PARA ACTUALIZAR EL MOVIMIENTO DE CAJA
         private bool actualizarMovimientosCaja()
         {
             try
             {
                 sSql = "";
                 sSql += "select numeromovimientocaja" + Environment.NewLine;
                 sSql += "from tp_localidades_impresoras" + Environment.NewLine;
                 sSql += "where id_localidad = " + Program.iIdLocalidad + Environment.NewLine;
                 sSql += "and estado = 'A'";

                 dtConsulta = new DataTable();
                 dtConsulta.Clear();

                 bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                 if (bRespuesta == false)
                 {
                     catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                     catchMensaje.ShowDialog();
                     return false;
                 }

                 if (dtConsulta.Rows.Count > 0)
                 {
                     iNumeroMovimiento = Convert.ToInt32(dtConsulta.Rows[0][0].ToString());
                 }

                 sSql = "";
                 sSql += "select id_factura" + Environment.NewLine;
                 sSql += "from cv403_facturas_pedidos" + Environment.NewLine;
                 sSql += "where id_pedido = " + Convert.ToInt32(sIdOrden) + Environment.NewLine;
                 sSql += "and estado = 'A'";

                 dtConsulta = new DataTable();
                 dtConsulta.Clear();

                 bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                 if (bRespuesta == false)
                 {
                     catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                     catchMensaje.ShowDialog();
                     return false;
                 }

                 if (dtConsulta.Rows.Count > 0)
                 {
                     iIdFactura = Convert.ToInt32(dtConsulta.Rows[0][0].ToString());
                 }

                 sSql = "";
                 sSql += "select id_persona, numero_pedido, establecimiento," + Environment.NewLine;
                 sSql += "punto_emision, numero_factura, idtipocomprobante" + Environment.NewLine;
                 sSql += "from pos_vw_factura" + Environment.NewLine;
                 sSql += "where id_pedido = " + Convert.ToInt32(sIdOrden) + Environment.NewLine;
                 sSql += "order by id_det_pedido";

                 dtConsulta = new DataTable();
                 dtConsulta.Clear();

                 bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                 if (bRespuesta == false)
                 {
                     catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                     catchMensaje.ShowDialog();
                     return false;
                 }

                 iIdPersona = Convert.ToInt32(dtConsulta.Rows[0][0].ToString());
                 iNumeroPedido = Convert.ToInt32(dtConsulta.Rows[0][1].ToString());
                 sFacturaRecuperada = dtConsulta.Rows[0][2].ToString() + "-" + dtConsulta.Rows[0][3].ToString() + "-" + dtConsulta.Rows[0][4].ToString().PadLeft(9, '0');
                 iIdTipoComprobante = Convert.ToInt32(dtConsulta.Rows[0][5].ToString());

                 sSql = "";
                 sSql += "select descripcion, sum(valor) valor, cambio,  count(*) cuenta, " + Environment.NewLine;
                 sSql += "isnull(valor_recibido, valor) valor_recibido, id_documento_pago" + Environment.NewLine;
                 sSql += "from pos_vw_pedido_forma_pago " + Environment.NewLine;
                 sSql += "where id_pedido = " + Convert.ToInt32(sIdOrden) + Environment.NewLine;
                 sSql += "group by descripcion, valor, cambio, valor_recibido, " + Environment.NewLine;
                 sSql += "id_pago, id_documento_pago " + Environment.NewLine;
                 sSql += "having count(*) >= 1";

                 dtAuxiliar = new DataTable();
                 dtAuxiliar.Clear();

                 bRespuesta = conexion.GFun_Lo_Busca_Registro(dtAuxiliar, sSql);

                 if (bRespuesta == false)
                 {
                     catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                     catchMensaje.ShowDialog();
                     return false;
                 }

                 if (dtAuxiliar.Rows.Count == 0)
                 {
                     ok.LblMensaje.Text = "No existe formas de pagos realizados. Couníquese con el administrador del sistema.";
                     ok.ShowDialog();
                     return false;
                 }

                 if (iIdTipoComprobante == 1)
                 {
                     sFacturaRecuperada = "FACT. No. " + sFacturaRecuperada;
                 }

                 else
                 {
                     sFacturaRecuperada = "N. ENTREGA. No. " + sFacturaRecuperada;
                 }

                 sFecha = Program.sFechaSistema.ToString("yyyy/MM/dd");

                 for (int index = 0; index < dtAuxiliar.Rows.Count; ++index)
                 {
                     sSql = "";
                     sSql += "insert into pos_movimiento_caja (" + Environment.NewLine;
                     sSql += "tipo_movimiento, idempresa, id_localidad, " + Environment.NewLine;
                     sSql += "id_persona, id_cliente, id_caja, id_pos_cargo," + Environment.NewLine;
                     sSql += "fecha, hora, cg_moneda, valor, concepto, " + Environment.NewLine;
                     sSql += "documento_venta, id_documento_pago, id_pos_jornada, estado," + Environment.NewLine;
                     sSql += "fecha_ingreso, usuario_ingreso, terminal_ingreso) " + Environment.NewLine;
                     sSql += "values (" + Environment.NewLine;
                     sSql += "1, " + Program.iIdEmpresa + ", " + Program.iIdLocalidad + "," + Environment.NewLine;
                     sSql += Program.iIdPersonaMovimiento + ", " + iIdPersona + ", " + iIdCaja + ", 1, " + Environment.NewLine;
                     sSql += "'" + sFecha + "', GETDATE(), " + Program.iMoneda + ", " + Environment.NewLine;
                     sSql += Convert.ToDouble(dtAuxiliar.Rows[index][1].ToString()) + "," + Environment.NewLine;
                     sSql += "'COBRO No. CUENTA " + iNumeroPedido.ToString() + " (" + dtAuxiliar.Rows[index][0].ToString() + ")', '" + Environment.NewLine;
                     sSql += sFacturaRecuperada + "', " + Environment.NewLine;
                     sSql += Convert.ToInt32(dtAuxiliar.Rows[index][5].ToString()) + ", " + Program.iJORNADA + ", 'A', " + Environment.NewLine;
                     sSql += "GETDATE(), '" + Program.sDatosMaximo[0] + "', '" + Program.sDatosMaximo[1] + "')";

                     if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                     {
                         catchMensaje.LblMensaje.Text = sSql;
                         catchMensaje.ShowDialog();
                         return false;
                     }
                     
                     dtConsulta = new DataTable();
                     dtConsulta.Clear();

                     sTabla = "pos_movimiento_caja";
                     sCampo = "id_pos_movimiento_caja";

                     iMaximo = conexion.GFun_Ln_Saca_Maximo_ID(sCampo, sTabla, "", Program.sDatosMaximo);

                     if (iMaximo == -1)
                     {
                         ok.LblMensaje.Text = "No se pudo obtener el codigo de la tabla " + sTabla;
                         return false;
                     }

                     iIdPosMovimientoCaja = Convert.ToInt32(iMaximo);

                     sSql = "";
                     sSql += "insert into pos_numero_movimiento_caja (" + Environment.NewLine;
                     sSql += "id_pos_movimiento_caja, numero_movimiento_caja," + Environment.NewLine;
                     sSql += "estado, fecha_ingreso, usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                     sSql += "values (" + Environment.NewLine;
                     sSql += iIdPosMovimientoCaja + ", " + iNumeroMovimiento + ", 'A', GETDATE()," + Environment.NewLine;
                     sSql += "'" + Program.sDatosMaximo[0] + "', '" + Program.sDatosMaximo[1] + "')";

                     if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                     {
                         catchMensaje.LblMensaje.Text = sSql;
                         catchMensaje.ShowDialog();
                         return false;
                     }

                     iNumeroMovimiento++;
                 }

                 sSql = "";
                 sSql += "update tp_localidades_impresoras set" + Environment.NewLine;
                 sSql += "numeromovimientocaja = " + iNumeroMovimiento + Environment.NewLine;
                 sSql += "where id_localidad = " + Program.iIdLocalidad + Environment.NewLine;
                 sSql += "and estado = 'A'";

                 if (conexion.GFun_Lo_Ejecuta_SQL(sSql))
                 {
                     return true;
                 }

                 return true;
             }

             catch (Exception ex)
             {
                 catchMensaje.LblMensaje.Text = ex.ToString();
                 int num = (int)catchMensaje.ShowDialog();
                 return false;
             }
         }

        //FUNCION PARA INSERTAR LOS PAGOS EN LA NUEVA COMANDA
         private bool insertarPagoNuevoPrecuenta()
         {
             try
             {
                 sFechaCorta = Program.sFechaSistema.ToString("yyyy/MM/dd");

                 if (!conexion.GFun_Lo_Maneja_Transaccion(Program.G_INICIA_TRANSACCION))
                 {
                     ok.LblMensaje.Text = "Error al abrir transacción.";
                     int num = (int)ok.ShowDialog();
                     return false;
                 }

                 if (Program.iDescuentaIva == 1)
                 {
                     if (iEjecutarActualizacionIVA == 1)
                     {
                         if (!actualizarIVACero())
                         {
                             return false;
                         }
                     }

                     else if (!actualizarIVA())
                     {
                         return false;
                     }
                 }

                 if (Program.iAplicaRecargoTarjeta == 1)
                 {
                     if (iEjecutarActualizacionTarjetas == 1)
                     {
                         if (!actualizarRecargoTarjetas())
                         {
                             return false;
                         }
                     }

                     else if (!actualizarRemoverRecargoTarjetas())
                     {
                         catchMensaje.LblMensaje.Text = "Ocurrió un problema en remover el recargo de tarjetas.";
                         catchMensaje.ShowDialog();
                         return false;
                     }
                 }

                 sSql = "";
                 sSql += "select id_documento_cobrar" + Environment.NewLine;
                 sSql += "from cv403_dctos_por_cobrar" + Environment.NewLine;
                 sSql += "where id_pedido = " + sIdOrden + Environment.NewLine;
                 sSql += "and estado = 'A'";

                 dtConsulta = new DataTable();
                 dtConsulta.Clear();

                 bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                 if (bRespuesta == false)
                 {
                     catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                     catchMensaje.ShowDialog();
                     return false;
                 }

                 iIdDocumentoCobrar = Convert.ToInt32(dtConsulta.Rows[0]["id_documento_cobrar"].ToString());
                 iCuenta = 0;

                 sSql = "";
                 sSql += "select count(*) cuenta" + Environment.NewLine;
                 sSql += "from cv403_documentos_pagados" + Environment.NewLine;
                 sSql += "where id_documento_cobrar = " + iIdDocumentoCobrar + Environment.NewLine;
                 sSql += "and estado = 'A'";

                 dtConsulta = new DataTable();
                 dtConsulta.Clear();

                 bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                 if (bRespuesta == false)
                 {
                     catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                     catchMensaje.ShowDialog();
                     return false;
                 }

                 iCuenta = Convert.ToInt32(dtConsulta.Rows[0][0].ToString());

                 if (iCuenta > 0)
                 {
                     sSql = "";
                     sSql += "select id_pago, id_documento_pagado" + Environment.NewLine;
                     sSql += "from  cv403_documentos_pagados" + Environment.NewLine;
                     sSql += "where id_documento_cobrar = " + iIdDocumentoCobrar + Environment.NewLine;
                     sSql += "and estado = 'A'";

                     dtConsulta = new DataTable();
                     dtConsulta.Clear();

                     bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                     if (bRespuesta == false)
                     {
                         catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                         catchMensaje.ShowDialog();
                         return false;
                     }

                     iIdPago = Convert.ToInt32(dtConsulta.Rows[0][0].ToString());
                     iIdDocumentoPagado = Convert.ToInt32(dtConsulta.Rows[0][1].ToString());

                     sSql = "";
                     sSql += "update cv403_pagos set" + Environment.NewLine;
                     sSql += "estado = 'E'," + Environment.NewLine;
                     sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                     sSql += "usuario_anula = '" + Program.sDatosMaximo[0] + "'," + Environment.NewLine;
                     sSql += "terminal_anula = '" + Program.sDatosMaximo[1] + "'" + Environment.NewLine;
                     sSql += "where id_pago = " + iIdPago;

                     if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                     {
                         catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                         catchMensaje.ShowDialog();
                         return false;
                     }

                     sSql = "";
                     sSql += "update cv403_documentos_pagos set" + Environment.NewLine;
                     sSql += "estado = 'E'," + Environment.NewLine;
                     sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                     sSql += "usuario_anula = '" + Program.sDatosMaximo[0] + "'," + Environment.NewLine;
                     sSql += "terminal_anula = '" + Program.sDatosMaximo[1] + "'" + Environment.NewLine;
                     sSql += "where id_pago = " + iIdPago;

                     if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                     {
                         catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                         catchMensaje.ShowDialog();
                         return false;
                     }

                     sSql = "";
                     sSql += "update cv403_numeros_pagos set" + Environment.NewLine;
                     sSql += "estado = 'E'," + Environment.NewLine;
                     sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                     sSql += "usuario_anula = '" + Program.sDatosMaximo[0] + "'," + Environment.NewLine;
                     sSql += "terminal_anula = '" + Program.sDatosMaximo[1] + "'" + Environment.NewLine;
                     sSql += "where id_pago = " + iIdPago;

                     if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                     {
                         catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                         catchMensaje.ShowDialog();
                         return false;
                     }

                     sSql = "";
                     sSql += "update cv403_documentos_pagados set" + Environment.NewLine;
                     sSql += "estado = 'E'," + Environment.NewLine;
                     sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                     sSql += "usuario_anula = '" + Program.sDatosMaximo[0] + "'," + Environment.NewLine;
                     sSql += "terminal_anula = '" + Program.sDatosMaximo[1] + "'" + Environment.NewLine;
                     sSql += "where id_documento_pagado = " + iIdDocumentoPagado;

                     if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                     {
                         catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                         catchMensaje.ShowDialog();
                         return false;
                     }
                 }

                 dbCambio = Convert.ToDecimal(this.dgvDetalleDeuda.Rows[2].Cells[1].Value.ToString());
                 dbPropina = Convert.ToDecimal(this.dgvDetalleDeuda.Rows[3].Cells[1].Value.ToString());

                 for (int i = 0; i < dgvPagos.Rows.Count; ++i)
                 {
                     sSql = "";
                     sSql += "insert into cv403_pagos (" + Environment.NewLine;
                     sSql += "idempresa, id_persona, fecha_pago, cg_moneda, valor," + Environment.NewLine;
                     sSql += "propina, cg_empresa, id_localidad, cg_cajero, fecha_ingreso," + Environment.NewLine;
                     sSql += "usuario_ingreso, terminal_ingreso, estado, " + Environment.NewLine;
                     sSql += "numero_replica_trigger, numero_control_replica, cambio) " + Environment.NewLine;
                     sSql += "values(" + Environment.NewLine;
                     sSql += Program.iIdEmpresa + ", " + iIdPersona + ", '" + sFechaCorta + "', " + Program.iMoneda + "," + Environment.NewLine;
                     sSql += Convert.ToDecimal(dgvPagos.Rows[i].Cells["valor"].Value.ToString()) + ", " + dbPropina + ", " + Program.iCgEmpresa + "," + Environment.NewLine;
                     sSql += Program.iIdLocalidad + ", 7799, GETDATE(), '" + Program.sDatosMaximo[0] + "'," + Environment.NewLine;
                     sSql += "'" + Program.sDatosMaximo[1] + "', 'A' , 1, 0, " + dbCambio + ")";
                     
                     if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                     {
                         catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                         catchMensaje.ShowDialog();
                         return false;
                     }

                     dtConsulta = new DataTable();
                     dtConsulta.Clear();
                     sTabla = "cv403_pagos";
                     sCampo = "id_pago";
                     iMaximo = conexion.GFun_Ln_Saca_Maximo_ID(sTabla, sCampo, "", Program.sDatosMaximo);

                     if (iMaximo == -1)
                     {
                         ok.LblMensaje.Text = "No se pudo obtener el codigo de la tabla " + sTabla;
                         ok.ShowDialog();
                         return false;
                     }

                     iIdPago = Convert.ToInt32(iMaximo);

                     sSql = "";
                     sSql += "select numero_pago" + Environment.NewLine;
                     sSql += "from tp_localidades_impresoras" + Environment.NewLine;
                     sSql += "where id_localidad = " + Program.iIdLocalidad;

                     dtConsulta = new DataTable();
                     dtConsulta.Clear();

                     bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                     if (bRespuesta == false)
                     {
                         catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                         catchMensaje.ShowDialog();
                         return false;
                     }

                     iNumeroPago = Convert.ToInt32(dtConsulta.Rows[0][0].ToString());

                     sSql = "";
                     sSql += "insert into cv403_numeros_pagos (" + Environment.NewLine;
                     sSql += "id_pago, serie, numero_pago, fecha_ingreso, usuario_ingreso," + Environment.NewLine;
                     sSql += "terminal_ingreso, estado, numero_replica_trigger, numero_control_replica)" + Environment.NewLine;
                     sSql += "values(" + Environment.NewLine;
                     sSql += iIdPago + ", 'A', " + (object)iNumeroPago + ", GETDATE(), '" + Program.sDatosMaximo[0] + "'," + Environment.NewLine;
                     sSql += "'" + Program.sDatosMaximo[1] + "', 'A', 1, 0)";

                     if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                     {
                         catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                         catchMensaje.ShowDialog();
                         return false;
                     }

                     sSql = "";
                     sSql += "update tp_localidades_impresoras set" + Environment.NewLine;
                     sSql += "numero_pago = numero_pago + 1" + Environment.NewLine;
                     sSql += "where id_localidad = " + Program.iIdLocalidad;

                     if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                     {
                         catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                         catchMensaje.ShowDialog();
                         return false;
                     }

                     sSql = "";
                     sSql += "select cg_tipo_documento" + Environment.NewLine;
                     sSql += "from pos_tipo_forma_cobro " + Environment.NewLine;
                     sSql += "where id_pos_tipo_forma_cobro = " + dgvPagos.Rows[i].Cells[0].Value.ToString();

                     dtConsulta = new DataTable();
                     dtConsulta.Clear();

                     bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                     if (bRespuesta == false)
                     {
                         catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                         catchMensaje.ShowDialog();
                         return false;
                     }

                     iCgTipoDocumento = Convert.ToInt32(dtConsulta.Rows[0][0].ToString());

                     sSql = "";
                     sSql += "insert into cv403_documentos_pagos (" + Environment.NewLine;
                     sSql += "id_pago, cg_tipo_documento, numero_documento, fecha_vcto, " + Environment.NewLine;
                     sSql += "cg_moneda, cotizacion, valor, id_pos_tipo_forma_cobro," + Environment.NewLine;
                     sSql += "estado, fecha_ingreso, usuario_ingreso, terminal_ingreso," + Environment.NewLine;
                     sSql += "numero_replica_trigger, numero_control_replica, valor_recibido) " + Environment.NewLine;
                     sSql += "values(" + Environment.NewLine;
                     sSql += (object)iIdPago + ", " + (object)iCgTipoDocumento + ", 9999, '" + sFechaCorta + "', " + Environment.NewLine;
                     sSql += (object)Program.iMoneda + ", 1, " + (object)Convert.ToDecimal(dgvPagos.Rows[i].Cells[2].Value) + "," + Environment.NewLine;
                     sSql += (object)Convert.ToInt32(dgvPagos.Rows[i].Cells[0].Value) + ", 'A', GETDATE()," + Environment.NewLine;
                     sSql += "'" + Program.sDatosMaximo[0] + "', '" + Program.sDatosMaximo[1] + "', 1, 0,";

                     if (Convert.ToInt32(dgvPagos.Rows[i].Cells[0].Value) == 1)
                     {
                         sSql += (string)(object)(Convert.ToDecimal(dgvPagos.Rows[i].Cells[2].Value) + dbCambio);
                     }
                     else
                     {
                         sSql += "null";
                     }

                     sSql += ")";

                     if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                     {
                         catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                         catchMensaje.ShowDialog();
                         return false;
                     }

                     sSql = "";
                     sSql += "insert into cv403_documentos_pagados (" + Environment.NewLine;
                     sSql += "id_documento_cobrar, id_pago, valor," + Environment.NewLine;
                     sSql += "estado, numero_replica_trigger,numero_control_replica," + Environment.NewLine;
                     sSql += "fecha_ingreso, usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                     sSql += "values (" + Environment.NewLine;
                     sSql += iIdDocumentoCobrar + ", " + iIdPago + ", " + dTotal + ", 'A', 1, 0, " + Environment.NewLine;
                     sSql += "GETDATE(), '" + Program.sDatosMaximo[0] + "', '" + Program.sDatosMaximo[1] + "')";

                     if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                     {
                         catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                         catchMensaje.ShowDialog();
                         return false;
                     }
                 }

                 //FIN FOR
                 sSql = "";
                 sSql += "update cv403_cab_pedidos set" + Environment.NewLine;
                 sSql += "estado_orden = 'Pre-Cuenta'" + Environment.NewLine;
                 sSql += "where id_pedido = " + Convert.ToInt32(sIdOrden) + Environment.NewLine;
                 sSql += "and estado = 'A'" + Environment.NewLine;

                 if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                 {
                     catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                     catchMensaje.ShowDialog();
                     return false;
                 }

                 conexion.GFun_Lo_Maneja_Transaccion(2);
                 return true;
             }

             catch (Exception ex)
             {
                 catchMensaje.LblMensaje.Text = ex.ToString();
                 catchMensaje.ShowDialog();
                 return false;
             }
         }

        //FUNCION PARA CAMBIAR LAS FORMAS DE PAGO EN LA PRECUENTA
         private bool cambiarFormasPagosPrecuenta(int iOp)
         {
             try
             {
                 string sFecha_P = Program.sFechaSistema.ToString("yyyy/MM/dd");

                 if (!conexion.GFun_Lo_Maneja_Transaccion(Program.G_INICIA_TRANSACCION))
                 {
                     ok.LblMensaje.Text = "Error al abrir transacción.";
                     ok.ShowDialog();
                     return false;
                 }

                 sSql = "";
                 sSql += "select id_documento_pago" + Environment.NewLine;
                 sSql += "from pos_vw_pedido_forma_pago" + Environment.NewLine;
                 sSql += "where id_pedido = " + Convert.ToInt32(sIdOrden);

                 dtConsulta = new DataTable();
                 dtConsulta.Clear();

                 bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                 if (bRespuesta == false)
                 {
                     catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                     catchMensaje.ShowDialog();
                     return false;
                 }

                 for (int i = 0; i < dtConsulta.Rows.Count; i++)
                 {
                     sSql = "";
                     sSql += "update pos_movimiento_caja set" + Environment.NewLine;
                     sSql += "estado = 'E'," + Environment.NewLine;
                     sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                     sSql += "usuario_anula = '" + Program.sDatosMaximo[0] + "'," + Environment.NewLine;
                     sSql += "terminal_anula = '" + Program.sDatosMaximo[1] + "'" + Environment.NewLine;
                     sSql += "where id_documento_pago = " + Convert.ToInt32(dtConsulta.Rows[i][0].ToString()) + Environment.NewLine;

                     if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                     {
                         catchMensaje.LblMensaje.Text = sSql;
                         int num = (int)catchMensaje.ShowDialog();
                         goto reversa;
                     }
                 }

                 sSql = "";
                 sSql += "select id_documento_cobrar" + Environment.NewLine;
                 sSql += "from cv403_dctos_por_cobrar" + Environment.NewLine;
                 sSql += "where id_pedido = " + sIdOrden + Environment.NewLine;
                 sSql += "and estado = 'A'";

                 dtConsulta = new DataTable();
                 dtConsulta.Clear();

                 bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                 if (bRespuesta == false)
                 {
                     catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                     catchMensaje.ShowDialog();
                     return false;
                 }

                 iIdDocumentoCobrar = Convert.ToInt32(dtConsulta.Rows[0]["id_documento_cobrar"].ToString());
                 iCuenta = 0;

                 sSql = "";
                 sSql += "select count(*) cuenta" + Environment.NewLine;
                 sSql += "from cv403_documentos_pagados" + Environment.NewLine;
                 sSql += "where id_documento_cobrar = " + iIdDocumentoCobrar + Environment.NewLine;
                 sSql += "and estado = 'A'";

                 dtConsulta = new DataTable();
                 dtConsulta.Clear();

                 bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                 if (bRespuesta == false)
                 {
                     catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                     catchMensaje.ShowDialog();
                     return false;
                 }

                 iCuenta = Convert.ToInt32(dtConsulta.Rows[0][0].ToString());

                 if (iCuenta > 0)
                 {
                     sSql = "";
                     sSql += "select id_pago, id_documento_pagado" + Environment.NewLine;
                     sSql += "from  cv403_documentos_pagados" + Environment.NewLine;
                     sSql += "where id_documento_cobrar = " + iIdDocumentoCobrar + Environment.NewLine;
                     sSql += "and estado = 'A'";

                     dtConsulta = new DataTable();
                     dtConsulta.Clear();

                     bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                     if (bRespuesta == false)
                     {
                         catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                         catchMensaje.ShowDialog();
                         return false;
                     }

                     if (dtConsulta.Rows.Count > 0)
                     {
                         iIdPago = Convert.ToInt32(dtConsulta.Rows[0][0].ToString());
                         iIdDocumentoPagado = Convert.ToInt32(dtConsulta.Rows[0][1].ToString());
                     }

                     sSql = "";
                     sSql += "update cv403_pagos set" + Environment.NewLine;
                     sSql += "estado = 'E'," + Environment.NewLine;
                     sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                     sSql += "usuario_anula = '" + Program.sDatosMaximo[0] + "'," + Environment.NewLine;
                     sSql += "terminal_anula = '" + Program.sDatosMaximo[1] + "'" + Environment.NewLine;
                     sSql += "where id_pago = " + iIdPago;

                     if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                     {
                         catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                         catchMensaje.ShowDialog();
                         return false;
                     }

                     sSql = "";
                     sSql += "update cv403_documentos_pagos set" + Environment.NewLine;
                     sSql += "estado = 'E'," + Environment.NewLine;
                     sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                     sSql += "usuario_anula = '" + Program.sDatosMaximo[0] + "'," + Environment.NewLine;
                     sSql +="terminal_anula = '" + Program.sDatosMaximo[1] + "'" + Environment.NewLine;
                     sSql +="where id_pago = " + iIdPago;

                     if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                     {
                         catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                         catchMensaje.ShowDialog();
                         return false;
                     }

                     sSql = "";
                     sSql += "update cv403_numeros_pagos set" + Environment.NewLine;
                     sSql += "estado = 'E'," + Environment.NewLine;
                     sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                     sSql += "usuario_anula = '" + Program.sDatosMaximo[0] + "'," + Environment.NewLine;
                     sSql += "terminal_anula = '" + Program.sDatosMaximo[1] + "'" + Environment.NewLine;
                     sSql += "where id_pago = " + iIdPago;

                     if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                     {
                         catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                         catchMensaje.ShowDialog();
                         return false;
                     }

                     sSql = "";
                     sSql += "update cv403_documentos_pagados set" + Environment.NewLine;
                     sSql += "estado = 'E'," + Environment.NewLine;
                     sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                     sSql += "usuario_anula = '" + Program.sDatosMaximo[0] + "'," + Environment.NewLine;
                     sSql += "terminal_anula = '" + Program.sDatosMaximo[1] + "'" + Environment.NewLine;
                     sSql += "where id_documento_pagado = " + iIdDocumentoPagado;

                     if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                     {
                         catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                         catchMensaje.ShowDialog();
                         return false;
                     }
                 }

                 dbCambio = Convert.ToDecimal(dgvDetalleDeuda.Rows[2].Cells[1].Value.ToString());
                 dbPropina = Convert.ToDecimal(dgvDetalleDeuda.Rows[3].Cells[1].Value.ToString());

                 for (int i = 0; i < dgvPagos.Rows.Count; i++)
                 {
                     sSql = "";
                     sSql += "insert into cv403_pagos (" + Environment.NewLine;
                     sSql += "idempresa, id_persona, fecha_pago, cg_moneda, valor," + Environment.NewLine;
                     sSql += "propina, cg_empresa, id_localidad, cg_cajero, fecha_ingreso," + Environment.NewLine;
                     sSql += "usuario_ingreso, terminal_ingreso, estado, " + Environment.NewLine;
                     sSql += "numero_replica_trigger, numero_control_replica,cambio) " + Environment.NewLine;
                     sSql += "values(" + Environment.NewLine;
                     sSql += Program.iIdEmpresa + ", " + iIdPersona + ", '" + sFecha_P + "', " + Program.iMoneda + "," + Environment.NewLine;
                     sSql += Convert.ToDecimal(dgvPagos.Rows[i].Cells["valor"].Value.ToString()) + ", " + dbPropina + ", " + Program.iCgEmpresa + "," + Environment.NewLine;
                     sSql += Program.iIdLocalidad + ", 7799, GETDATE(), '" + Program.sDatosMaximo[0] + "'," + Environment.NewLine;
                     sSql += "'" + Program.sDatosMaximo[1] + "', 'A' , 1, 0, " + dbCambio + ")";
                     
                     if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                     {
                         catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                         catchMensaje.ShowDialog();
                         return false;
                     }

                     dtConsulta = new DataTable();
                     dtConsulta.Clear();
                     sTabla = "cv403_pagos";
                     sCampo = "id_pago";

                     iMaximo = conexion.GFun_Ln_Saca_Maximo_ID(sTabla, sCampo, "", Program.sDatosMaximo);

                     if (iMaximo == -1)
                     {
                         ok.LblMensaje.Text = "No se pudo obtener el codigo de la tabla " + sTabla;
                         ok.ShowDialog();
                         return false;
                     }

                     iIdPago = Convert.ToInt32(iMaximo);

                     sSql = "";
                     sSql += "select numero_pago" + Environment.NewLine;
                     sSql += "from tp_localidades_impresoras" + Environment.NewLine;
                     sSql += "where id_localidad = " + Program.iIdLocalidad;

                     dtConsulta = new DataTable();
                     dtConsulta.Clear();

                     bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                     if (bRespuesta == false)
                     {
                         catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                         catchMensaje.ShowDialog();
                         return false;
                     }

                     iNumeroPago = Convert.ToInt32(dtConsulta.Rows[0][0].ToString());

                     sSql = "";
                     sSql += "insert into cv403_numeros_pagos (" + Environment.NewLine;
                     sSql += "id_pago, serie, numero_pago, fecha_ingreso, usuario_ingreso," + Environment.NewLine;
                     sSql += "terminal_ingreso, estado, numero_replica_trigger, numero_control_replica)" + Environment.NewLine;
                     sSql += "values(" + Environment.NewLine;
                     sSql += iIdPago + ", 'A', " + iNumeroPago + ", GETDATE(), '" + Program.sDatosMaximo[0] + "'," + Environment.NewLine;
                     sSql += "'" + Program.sDatosMaximo[1] + "', 'A', 1, 0)";

                     if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                     {
                         catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                         catchMensaje.ShowDialog();
                         return false;
                     }

                     sSql = "";
                     sSql += "update tp_localidades_impresoras set" + Environment.NewLine;
                     sSql += "numero_pago = numero_pago + 1" + Environment.NewLine;
                     sSql += "where id_localidad = " + Program.iIdLocalidad;

                     if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                     {
                         catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                         catchMensaje.ShowDialog();
                         return false;
                     }

                     sSql = "";
                     sSql += "select cg_tipo_documento" + Environment.NewLine;
                     sSql += "from pos_tipo_forma_cobro " + Environment.NewLine;
                     sSql += "where id_pos_tipo_forma_cobro = " + dgvPagos.Rows[i].Cells[0].Value.ToString();

                     dtConsulta = new DataTable();
                     dtConsulta.Clear();

                     bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                     if (bRespuesta == false)
                     {
                         catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                         catchMensaje.ShowDialog();
                         return false;
                     }

                     iCgTipoDocumento = Convert.ToInt32(dtConsulta.Rows[0][0].ToString());

                     sSql = "";
                     sSql += "insert into cv403_documentos_pagos (" + Environment.NewLine;
                     sSql += "id_pago, cg_tipo_documento, numero_documento, fecha_vcto, " + Environment.NewLine;
                     sSql += "cg_moneda, cotizacion, valor, id_pos_tipo_forma_cobro," + Environment.NewLine;
                     sSql += "estado, fecha_ingreso, usuario_ingreso, terminal_ingreso," + Environment.NewLine;
                     sSql += "numero_replica_trigger, numero_control_replica, valor_recibido) " + Environment.NewLine;
                     sSql += "values(" + Environment.NewLine;
                     sSql += iIdPago + ", " + iCgTipoDocumento + ", 9999, '" + sFecha_P + "', " + Environment.NewLine;
                     sSql += Program.iMoneda + ", 1, " + Convert.ToDecimal(dgvPagos.Rows[i].Cells[2].Value) + "," + Environment.NewLine;
                     sSql += Convert.ToInt32(dgvPagos.Rows[i].Cells[0].Value) + ", 'A', GETDATE()," + Environment.NewLine;
                     sSql += "'" + Program.sDatosMaximo[0] + "', '" + Program.sDatosMaximo[1] + "', 1, 0,";

                     if (Convert.ToInt32(dgvPagos.Rows[i].Cells[0].Value) == 1)
                     {
                         sSql += Convert.ToDecimal(dgvPagos.Rows[i].Cells[2].Value) + dbCambio;
                     }

                     else
                     {
                         sSql += "null";
                     }

                     sSql += ")";

                     if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                     {
                         catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                         catchMensaje.ShowDialog();
                         return false;
                     }

                     sSql = "";
                     sSql += "insert into cv403_documentos_pagados (" + Environment.NewLine;
                     sSql += "id_documento_cobrar, id_pago, valor," + Environment.NewLine;
                     sSql += "estado, numero_replica_trigger,numero_control_replica," + Environment.NewLine;
                     sSql += "fecha_ingreso, usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                     sSql += "values (" + Environment.NewLine;
                     sSql += iIdDocumentoCobrar + ", " + iIdPago + ", " + dTotal + ", 'A', 1, 0, " + Environment.NewLine;
                     sSql += "GETDATE(), '" + Program.sDatosMaximo[0] + "', '" + Program.sDatosMaximo[1] + "')";

                     if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                     {
                         catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                         catchMensaje.ShowDialog();
                         return false;
                     }
                 }

                 if (iOpCambiarEstadoOrden == 1)
                 {
                     sSql = "";
                     sSql += "update cv403_cab_pedidos set" + Environment.NewLine;
                     sSql += "estado_orden = 'Pagada'" + Environment.NewLine;
                     sSql += "where id_pedido = " + Convert.ToInt32(sIdOrden) + Environment.NewLine;
                     sSql += "and estado = 'A'";

                     if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                     {
                         catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                         catchMensaje.ShowDialog();
                         goto reversa;
                     }
                 }

                 if (iOp == 1)
                 {
                     if (!actualizarMovimientosCaja())
                     {
                         goto reversa;
                     }
                 }

                 conexion.GFun_Lo_Maneja_Transaccion(Program.G_TERMINA_TRANSACCION);
                 return true;
             }

             catch (Exception ex)
             {
                 catchMensaje.LblMensaje.Text = ex.ToString();
                 catchMensaje.ShowDialog();
                 return false;
             }

             reversa: { conexion.GFun_Lo_Maneja_Transaccion(Program.G_REVERSA_TRANSACCION); return false; }
         }

        //FUNCION PARA CREAR LOS PAGOS FACTURA
         private void crearPagosFactura()
         {
             try
             {
                 Cursor = Cursors.WaitCursor;

                 if (!conexion.GFun_Lo_Maneja_Transaccion(Program.G_INICIA_TRANSACCION))
                 {
                     ok.LblMensaje.Text = "Error al abrir transacción.";
                     ok.ShowDialog();
                     return;
                 }

                 if (iBanderaRecargoBoton == 1)
                 {
                     if ((Program.iAplicaRecargoTarjeta == 1) && (iBanderaRecargoBDD == 1) && (actualizarPreciosRecargo() == false))
                     {
                         conexion.GFun_Lo_Maneja_Transaccion(Program.G_REVERSA_TRANSACCION);
                         return;
                     }
                 }

                 else if (iBanderaRemoverIvaBoton == 1)
                 {
                     if (Program.iDescuentaIva == 1 && !actualizarIVACero())
                     {
                         conexion.GFun_Lo_Maneja_Transaccion(Program.G_REVERSA_TRANSACCION);
                         return;
                     }
                 }

                 else if (((Program.iDescuentaIva == 1) || (Program.iAplicaRecargoTarjeta == 1)) && (actualizarPreciosOriginales() == false))
                 {
                     conexion.GFun_Lo_Maneja_Transaccion(Program.G_REVERSA_TRANSACCION);
                     return;
                 }

                 if (insertarPagos() == false)
                 {
                     conexion.GFun_Lo_Maneja_Transaccion((Program.G_REVERSA_TRANSACCION););
                 }

                 else
                 {
                     if (iBanderaGeneraFactura == 0)
                     {
                         sSql = sSql + "update cv403_cab_pedidos set" + Environment.NewLine;
                         sSql = sSql + "estado_orden = 'Pagada'," + Environment.NewLine;
                         sSql = sSql + "id_persona = " + iIdPersona + Environment.NewLine;
                         sSql = sSql + "where id_pedido = " + sIdOrden;

                         if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                         {
                             catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                             catchMensaje.ShowDialog();
                             conexion.GFun_Lo_Maneja_Transaccion(Program.G_REVERSA_TRANSACCION);
                         }
                     }
                     else
                     {
                         if (insertarFactura() == false)
                         {
                             conexion.GFun_Lo_Maneja_Transaccion(Program.G_REVERSA_TRANSACCION);
                             return;
                         }

                         if (!insertarMovimientosCaja())
                         {
                             conexion.GFun_Lo_Maneja_Transaccion(Program.G_REVERSA_TRANSACCION);
                             return;
                         }

                         sSql = "";
                         frmCobros frmCobros1 = this;
                         frmCobros1.sSql = frmCobros1.sSql + "update tp_localidades_impresoras set" + Environment.NewLine;

                         if (rdbFactura.Checked)
                         {
                             sSql += "numero_factura = " + (Convert.ToInt32(TxtNumeroFactura.Text) + 1) + Environment.NewLine;
                         }

                         else if (rdbNotaVenta.Checked)
                         {
                             sSql += "numeronotaentrega = " + (Convert.ToInt32(TxtNumeroFactura.Text) + 1) + Environment.NewLine;
                         }

                         sSql += "where id_localidad = " + Program.iIdLocalidad;
                         
                         if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                         {
                             catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                             catchMensaje.ShowDialog();
                             conexion.GFun_Lo_Maneja_Transaccion(Program.G_REVERSA_TRANSACCION);
                         }
                     }

                     conexion.GFun_Lo_Maneja_Transaccion(Program.G_TERMINA_TRANSACCION);

                     if (iBanderaGeneraFactura == 1)
                     {
                         crearReporte();
                     }
                     else
                     {
                         ok.LblMensaje.Text = "Se ha procedido a ingresar los datos de forma éxitosa.";
                         ok.ShowDialog();

                         if (ok.DialogResult == DialogResult.OK)
                         {
                             DialogResult = DialogResult.OK;
                             Close();

                             if (Program.iBanderaCerrarVentana == 0)
                             {
                                 ord.Close();
                             }

                             else
                             {
                                 Program.iBanderaCerrarVentana = 0;
                             }
                         }
                     }

                     Program.iSeleccionarNotaVenta = 0;
                     Cursor = Cursors.Default;
                 }
             }

             catch (Exception ex)
             {
             }
         }

        //FUNCION PARA INSERTAR LOS PAGOS
         private bool insertarPagos()
         {
             try
             {
                 sSql = "";
                 sSql += "select id_documento_cobrar" + Environment.NewLine;
                 sSql += "from cv403_dctos_por_cobrar" + Environment.NewLine;
                 sSql += "where id_pedido = " + sIdOrden + Environment.NewLine;
                 sSql += "and estado = 'A'";

                 dtConsulta = new DataTable();
                 dtConsulta.Clear();

                 bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                 if (bRespuesta == false)
                 {
                     catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                     catchMensaje.ShowDialog();
                     return false;
                 }

                 iIdDocumentoCobrar = Convert.ToInt32(dtConsulta.Rows[0]["id_documento_cobrar"].ToString());
                 iCuenta = 0;

                 sSql = "";
                 sSql += "select count(*) cuenta" + Environment.NewLine;
                 sSql += "from cv403_documentos_pagados" + Environment.NewLine;
                 sSql += "where id_documento_cobrar = " + iIdDocumentoCobrar + Environment.NewLine;
                 sSql += "and estado = 'A'";

                 dtConsulta = new DataTable();
                 dtConsulta.Clear();

                 bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                 if (bRespuesta == false)
                 {
                     catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                     catchMensaje.ShowDialog();
                     return false;
                 }

                 iCuenta = Convert.ToInt32(dtConsulta.Rows[0][0].ToString());

                 if (iCuenta > 0)
                 {
                     sSql = "";
                     sSql += "select id_pago, id_documento_pagado" + Environment.NewLine;
                     sSql += "from  cv403_documentos_pagados" + Environment.NewLine;
                     sSql += "where id_documento_cobrar = " + iIdDocumentoCobrar + Environment.NewLine;
                     sSql += "and estado = 'A'";

                     dtConsulta = new DataTable();
                     dtConsulta.Clear();

                     bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                     if (bRespuesta == false)
                     {
                         catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                         catchMensaje.ShowDialog();
                         return false;
                     }

                     if (dtConsulta.Rows.Count > 0)
                     {
                         iIdPago = Convert.ToInt32(dtConsulta.Rows[0][0].ToString());
                         iIdDocumentoPagado = Convert.ToInt32(dtConsulta.Rows[0][1].ToString());
                     }

                     sSql = "";
                     sSql += "update cv403_pagos set" + Environment.NewLine;
                     sSql += "estado = 'E'," + Environment.NewLine;
                     sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                     sSql += "usuario_anula = '" + Program.sDatosMaximo[0] + "'," + Environment.NewLine;
                     sSql += "terminal_anula = '" + Program.sDatosMaximo[1] + "'" + Environment.NewLine;
                     sSql += "where id_pago = " + iIdPago;

                     if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                     {
                         catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                         catchMensaje.ShowDialog();
                         return false;
                     }

                     sSql = "";
                     sSql += "update cv403_documentos_pagos set" + Environment.NewLine;
                     sSql += "estado = 'E'," + Environment.NewLine;
                     sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                     sSql += "usuario_anula = '" + Program.sDatosMaximo[0] + "'," + Environment.NewLine;
                     sSql += "terminal_anula = '" + Program.sDatosMaximo[1] + "'" + Environment.NewLine;
                     sSql += "where id_pago = " + iIdPago;

                     if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                     {
                         catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                         catchMensaje.ShowDialog();
                         return false;
                     }

                     sSql = "";
                     sSql += "update cv403_numeros_pagos set" + Environment.NewLine;
                     sSql += "estado = 'E'," + Environment.NewLine;
                     sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                     sSql += "usuario_anula = '" + Program.sDatosMaximo[0] + "'," + Environment.NewLine;
                     sSql += "terminal_anula = '" + Program.sDatosMaximo[1] + "'" + Environment.NewLine;
                     sSql += "where id_pago = " + iIdPago;

                     if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                     {
                         catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                         catchMensaje.ShowDialog();
                         return false;
                     }

                     sSql = "";
                     sSql += "update cv403_documentos_pagados set" + Environment.NewLine;
                     sSql += "estado = 'E'," + Environment.NewLine;
                     sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                     sSql += "usuario_anula = '" + Program.sDatosMaximo[0] + "'," + Environment.NewLine;
                     sSql += "terminal_anula = '" + Program.sDatosMaximo[1] + "'" + Environment.NewLine;
                     sSql += "where id_documento_pagado = " + iIdDocumentoPagado;

                     if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                     {
                         catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                         catchMensaje.ShowDialog();
                         return false;
                     }
                 }

                 dbCambio = Convert.ToDecimal(dgvDetalleDeuda.Rows[2].Cells[1].Value.ToString());
                 dbPropina = Convert.ToDecimal(dgvDetalleDeuda.Rows[3].Cells[1].Value.ToString());

                 for (int i = 0; i < dgvPagos.Rows.Count; i++)
                 {
                     sSql = "";
                     sSql += "insert into cv403_pagos (" + Environment.NewLine;
                     sSql += "idempresa, id_persona, fecha_pago, cg_moneda, valor," + Environment.NewLine;
                     sSql += "propina, cg_empresa, id_localidad, cg_cajero, fecha_ingreso," + Environment.NewLine;
                     sSql += "usuario_ingreso, terminal_ingreso, estado, " + Environment.NewLine;
                     sSql += "numero_replica_trigger, numero_control_replica,cambio) " + Environment.NewLine;
                     sSql += "values(" + Environment.NewLine;
                     sSql += Program.iIdEmpresa + ", " + iIdPersona + ", '" + sFechaCorta + "', " + Program.iMoneda + "," + Environment.NewLine;
                     sSql += Convert.ToDecimal(dgvPagos.Rows[i].Cells["valor"].Value.ToString()) + ", " + dbPropina + ", " + Program.iCgEmpresa + "," + Environment.NewLine;
                     sSql += Program.iIdLocalidad + ", 7799, GETDATE(), '" + Program.sDatosMaximo[0] + "'," + Environment.NewLine;
                     sSql += "'" + Program.sDatosMaximo[1] + "', 'A' , 1, 0, " + dbCambio + ")";

                     if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                     {
                         catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                         catchMensaje.ShowDialog();
                         return false;
                     }

                     dtConsulta = new DataTable();
                     dtConsulta.Clear();

                     sTabla = "cv403_pagos";
                     sCampo = "id_pago";

                     iMaximo = conexion.GFun_Ln_Saca_Maximo_ID(sTabla, sCampo, "", Program.sDatosMaximo);

                     if (iMaximo == -1)
                     {
                         ok.LblMensaje.Text = "No se pudo obtener el codigo de la tabla " + sTabla;
                         ok.ShowDialog();
                         return false;
                     }

                     iIdPago = Convert.ToInt32(iMaximo);

                     sSql = "";
                     sSql += "select numero_pago" + Environment.NewLine;
                     sSql += "from tp_localidades_impresoras" + Environment.NewLine;
                     sSql += "where id_localidad = " + Program.iIdLocalidad;

                     dtConsulta = new DataTable();
                     dtConsulta.Clear();

                     bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                     if (bRespuesta == false)
                     {
                         catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                         catchMensaje.ShowDialog();
                         return false;
                     }

                     iNumeroPago = Convert.ToInt32(dtConsulta.Rows[0][0].ToString());

                     sSql = "";
                     sSql += "insert into cv403_numeros_pagos (" + Environment.NewLine;
                     sSql += "id_pago, serie, numero_pago, fecha_ingreso, usuario_ingreso," + Environment.NewLine;
                     sSql += "terminal_ingreso, estado, numero_replica_trigger, numero_control_replica)" + Environment.NewLine;
                     sSql += "values(" + Environment.NewLine;
                     sSql += iIdPago + ", 'A', " + iNumeroPago + ", GETDATE(), '" + Program.sDatosMaximo[0] + "'," + Environment.NewLine;
                     sSql += "'" + Program.sDatosMaximo[1] + "', 'A', 1, 0)";

                     if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                     {
                         catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                         catchMensaje.ShowDialog();
                         return false;
                     }

                     sSql = "";
                     sSql += "update tp_localidades_impresoras set" + Environment.NewLine;
                     sSql += "numero_pago = numero_pago + 1" + Environment.NewLine;
                     sSql += "where id_localidad = " + Program.iIdLocalidad;

                     if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                     {
                         catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                         catchMensaje.ShowDialog();
                         return false;
                     }

                     sSql = "";
                     sSql += "select cg_tipo_documento" + Environment.NewLine;
                     sSql += "from pos_tipo_forma_cobro " + Environment.NewLine;
                     sSql += "where id_pos_tipo_forma_cobro = " + dgvPagos.Rows[i].Cells[0].Value.ToString();

                     dtConsulta = new DataTable();
                     dtConsulta.Clear();

                     bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                     if (bRespuesta == false)
                     {
                         catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                         catchMensaje.ShowDialog();
                         return false;
                     }

                     iCgTipoDocumento = Convert.ToInt32(dtConsulta.Rows[0][0].ToString());

                     sSql = "";
                     sSql += "insert into cv403_documentos_pagos (" + Environment.NewLine;
                     sSql += "id_pago, cg_tipo_documento, numero_documento, fecha_vcto, " + Environment.NewLine;
                     sSql += "cg_moneda, cotizacion, valor, id_pos_tipo_forma_cobro," + Environment.NewLine;
                     sSql += "estado, fecha_ingreso, usuario_ingreso, terminal_ingreso," + Environment.NewLine;
                     sSql += "numero_replica_trigger, numero_control_replica, valor_recibido) " + Environment.NewLine;
                     sSql += "values(" + Environment.NewLine;
                     sSql += iIdPago + ", " + iCgTipoDocumento + ", 9999, '" + sFechaCorta + "', " + Environment.NewLine;
                     sSql += Program.iMoneda + ", 1, " + Convert.ToDecimal(dgvPagos.Rows[i].Cells[2].Value) + "," + Environment.NewLine;
                     sSql += Convert.ToInt32(dgvPagos.Rows[i].Cells[0].Value) + ", 'A', GETDATE()," + Environment.NewLine;
                     sSql += "'" + Program.sDatosMaximo[0] + "', '" + Program.sDatosMaximo[1] + "', 1, 0,";

                     if (Convert.ToInt32(dgvPagos.Rows[i].Cells[0].Value) == 1)
                     {
                         sSql += (Convert.ToDecimal(dgvPagos.Rows[i].Cells[2].Value) + dbCambio);
                     }

                     else
                     {
                         sSql += "null";
                     }

                     sSql += ")";

                     if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                     {
                         catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                         catchMensaje.ShowDialog();
                         return false;
                     }

                     sSql = "";
                     sSql += "insert into cv403_documentos_pagados (" + Environment.NewLine;
                     sSql += "id_documento_cobrar, id_pago, valor," + Environment.NewLine;
                     sSql += "estado, numero_replica_trigger,numero_control_replica," + Environment.NewLine;
                     sSql += "fecha_ingreso, usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                     sSql += "values (" + Environment.NewLine;
                     sSql += iIdDocumentoCobrar + ", " + iIdPago + ", " + dTotal + ", 'A', 1, 0, " + Environment.NewLine;
                     sSql += "GETDATE(), '" + Program.sDatosMaximo[0] + "', '" + Program.sDatosMaximo[1] + "')";
                     
                     if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                     {
                         catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                         catchMensaje.ShowDialog();
                         return false;
                     }
                 }

                 sSql = "";
                 sSql += "update cv403_cab_pedidos set" + Environment.NewLine;
                 sSql += "estado_orden = 'Pre-Cuenta'" + Environment.NewLine;
                 sSql += "where id_pedido = " + Convert.ToInt32(sIdOrden) + Environment.NewLine;
                 sSql += "and estado = 'A'" + Environment.NewLine;

                 if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                 {
                     catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                     catchMensaje.ShowDialog();
                     return false;
                 }

                 return false;
                 
             }

             catch (Exception ex)
             {
                 catchMensaje.LblMensaje.Text = ex.ToString();
                 catchMensaje.ShowDialog();
                 return false;
             }
         }

        //FUNCION PARA INSERTAR LA FACTURA
         private bool insertarFactura()
         {
             try
             {
                 string sFecha_P = Program.sFechaSistema.ToString("yyyy/MM/dd");

                 int iFacturaElectronica_P;

                 if (rdbFactura.Checked)
                 {
                     iIdTipoComprobante = 1;
                 }

                 else if (rdbNotaVenta.Checked)
                 {
                     iIdTipoComprobante = Program.iComprobanteNotaEntrega;
                 }

                 if (Program.iFacturacionElectronica != 0)
                 {
                     iFacturaElectronica_P = 1;
                 }

                 else
                 {
                     iFacturaElectronica_P = 0;
                 }

                 llenarDataTable();

                 sSql = "";
                 sSql += "insert into cv403_facturas (idempresa, id_persona, cg_empresa, idtipocomprobante," + Environment.NewLine;
                 sSql += "id_localidad, idformulariossri, id_vendedor, id_forma_pago, id_forma_pago2, id_forma_pago3," + Environment.NewLine;
                 sSql += "fecha_factura, fecha_vcto, cg_moneda, valor, cg_estado_factura, editable, fecha_ingreso, " + Environment.NewLine;
                 sSql += "usuario_ingreso, terminal_ingreso, estado, numero_replica_trigger, numero_control_replica, " + Environment.NewLine;
                 sSql += "Direccion_Factura,Telefono_Factura,Ciudad_Factura, correo_electronico, servicio," + Environment.NewLine;
                 sSql += "facturaelectronica, id_tipo_emision, id_tipo_ambiente)" + Environment.NewLine;
                 sSql += "values(" + Environment.NewLine;
                 sSql += Program.iIdEmpresa + ", " + iIdPersona + ", " + Program.iCgEmpresa + "," + Environment.NewLine;
                 sSql += iIdTipoComprobante + "," + Program.iIdLocalidad + ", " + Program.iIdFormularioSri + ", " + Program.iIdVendedor + ", " + iIdFormaPago_1 + ", " + Environment.NewLine;
                 
                 if (iIdFormaPago_2 == 0)
                 {
                     sSql += "null, ";
                 }
                 
                 else
                 {
                     sSql += iIdFormaPago_2 + ", ";
                 }

                 if (iIdFormaPago_3 == 0)
                 {
                     sSql += "null, ";
                 }
                 else
                 {
                     sSql += iIdFormaPago_3 + ", ";
                 }

                 sSql += "'" + sFecha_P + "', '" + sFecha_P + "', " + Program.iMoneda + ", " + dTotal + ", 0, 0, GETDATE()," + Environment.NewLine;
                 sSql += "'" + Program.sDatosMaximo[0] + "', '" + Program.sDatosMaximo[1] + "', 'A', 1, 0," + Environment.NewLine;
                 sSql += "'" + txtDireccion.Text.Trim() + "', '" + txtTelefono.Text + "', '" + sCiudad + "'," + Environment.NewLine;
                 sSql += "'" + txtMail.Text.Trim() + "', " + dServicio + ", " + iFacturaElectronica_P + ", " + iIdTipoEmision + ", " + iIdTipoAmbiente + ")";
                 
                 if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                 {
                     catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                     catchMensaje.ShowDialog();
                     return false;
                 }

                 dtConsulta = new DataTable();
                 dtConsulta.Clear();

                 sTabla = "cv403_facturas";
                 sCampo = "id_factura";

                 iMaximo = conexion.GFun_Ln_Saca_Maximo_ID(sTabla, sCampo, "", Program.sDatosMaximo);

                 if (iMaximo == -1)
                 {
                     ok.LblMensaje.Text = "No se pudo obtener el codigo de la tabla " + sTabla;
                     ok.ShowDialog();
                     return false;
                 }

                 iIdFactura = Convert.ToInt32(iMaximo);

                 sSql = "";
                 sSql += "insert into cv403_numeros_facturas (id_factura, idtipocomprobante, numero_factura," + Environment.NewLine;
                 sSql += "fecha_ingreso, usuario_ingreso, terminal_ingreso, estado, numero_replica_trigger," + Environment.NewLine;
                 sSql += "numero_control_replica) " + Environment.NewLine;
                 sSql += "values (" + Environment.NewLine;
                 sSql += iIdFactura + ", " + iIdTipoComprobante + ", " + Convert.ToInt32(TxtNumeroFactura.Text.Trim()) + ", GETDATE()," + Environment.NewLine;
                 sSql += "'" + Program.sDatosMaximo[0] + "', '" + Program.sDatosMaximo[1] + "', 'A', 1, 0 )";
                 
                 if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                 {
                     catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                     catchMensaje.ShowDialog();
                     return false;
                 }

                 sSql = "";
                 sSql += "insert into cv403_facturas_pedidos (" + Environment.NewLine;
                 sSql += "id_factura, id_pedido, fecha_ingreso, usuario_ingreso, terminal_ingreso," + Environment.NewLine;
                 sSql += "estado, numero_replica_trigger, numero_control_replica) " + Environment.NewLine;
                 sSql += "values (" + Environment.NewLine;                 
                 sSql += iIdFactura + ", " + sIdOrden + ", GETDATE()," + Environment.NewLine;
                 sSql += "'" + Program.sDatosMaximo[0] + "', '" + Program.sDatosMaximo[1] + "', 'A', 1, 0 )";

                 if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                 {
                     catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                     catchMensaje.ShowDialog();
                     return false;
                 }

                 sSql = "";
                 sSql += "update cv403_dctos_por_cobrar set" + Environment.NewLine;
                 sSql += "id_factura = " + iIdFactura + "," + Environment.NewLine;
                 sSql += "cg_estado_dcto = " + iCgEstadoDctoPorCobrar + "," + Environment.NewLine;
                 sSql += "numero_documento = " + Convert.ToInt32(TxtNumeroFactura.Text.Trim()) + Environment.NewLine;
                 sSql += "where id_pedido = " + sIdOrden;
                 
                 if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                 {
                     catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                     catchMensaje.ShowDialog();
                     return false;
                 }

                 sSql = "";
                 sSql += "update cv403_cab_pedidos set" + Environment.NewLine;
                 sSql += "estado_orden = 'Pagada'," + Environment.NewLine;
                 sSql += "id_persona = " + iIdPersona + "," + Environment.NewLine;
                 sSql += "fecha_cierre_orden = GETDATE()," + Environment.NewLine;
                 sSql += "recargo_tarjeta = " + iBanderaRecargoBoton + "," + Environment.NewLine;
                 sSql += "remover_iva = " + iBanderaRemoverIvaBoton + Environment.NewLine;
                 sSql += "where id_pedido = " + sIdOrden;
                 
                 if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                 {
                     catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                     catchMensaje.ShowDialog();
                     return false;
                 }

                 if (rdbNotaVenta.Checked)
                 {
                     sSql = "";
                     sSql += "update cv403_numero_cab_pedido set" + Environment.NewLine;
                     sSql += "idtipocomprobante = " + Program.iComprobanteNotaEntrega + Environment.NewLine;
                     sSql += "where id_pedido = " + sIdOrden;

                     if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                     {
                         catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                         catchMensaje.ShowDialog();
                         return false;
                     }
                 }
                 return true;
             }
             catch (Exception ex)
             {
                 return false;
             }
         }

        //FUNCION PARA INSERTAR LOS MOVIMIENTOS DE CAJA
         private bool insertarMovimientosCaja()
         {
             try
             {
                 sFecha = Program.sFechaSistema.ToString("yyyy/MM/dd");
                 sSecuencial = TxtNumeroFactura.Text.Trim().PadLeft(9, '0');

                 sSql = "";
                 sSql += "select descripcion, sum(valor), cambio,  count(*) cuenta, " + Environment.NewLine;
                 sSql += "sum(isnull(valor_recibido, valor)) valor_recibido, id_documento_pago" + Environment.NewLine;
                 sSql += "from pos_vw_pedido_forma_pago " + Environment.NewLine;
                 sSql += "where id_pedido = " + Convert.ToInt32(sIdOrden) + Environment.NewLine;
                 sSql += "group by descripcion, valor, cambio, valor_recibido, " + Environment.NewLine;
                 sSql += "id_pago, id_documento_pago";

                 dtAuxiliar = new DataTable();
                 dtAuxiliar.Clear();

                 bRespuesta = conexion.GFun_Lo_Busca_Registro(dtAuxiliar, sSql);

                 if (bRespuesta == false)
                 {
                     catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                     catchMensaje.ShowDialog();
                     return false;
                 }

                 if (dtAuxiliar.Rows.Count == 0)
                 {
                     ok.LblMensaje.Text = "No existe formas de pagos realizados. Comuníquese con el administrador del sistema.";
                     ok.ShowDialog();
                     return false;
                 }

                 if (rdbFactura.Checked)
                 {
                     sMovimiento = ("FACT. No. " + txtfacturacion.Text.Trim() + "-" + sSecuencial).Trim();
                 }

                 else if (rdbNotaVenta.Checked)
                 {
                     sMovimiento = ("N. ENTREGA. No. " + txtfacturacion.Text.Trim() + "-" + sSecuencial).Trim();
                 }

                 for (int i = 0; i < dtAuxiliar.Rows.Count; ++i)
                 {
                     sSql = "";
                     sSql += "insert into pos_movimiento_caja (" + Environment.NewLine;
                     sSql += "tipo_movimiento, idempresa, id_localidad, id_persona, id_cliente," + Environment.NewLine;
                     sSql += "id_caja, id_pos_cargo, fecha, hora, cg_moneda, valor, concepto," + Environment.NewLine;
                     sSql += "documento_venta, id_documento_pago, id_pos_jornada, estado," + Environment.NewLine;
                     sSql += "fecha_ingreso, usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                     sSql += "values (" + Environment.NewLine;
                     sSql += "1, " + Program.iIdEmpresa + ", " + Program.iIdLocalidad + Environment.NewLine;
                     sSql += ", " + Program.iIdPersonaMovimiento + ", " + iIdPersona + ", " + iIdCaja + ", 1," + Environment.NewLine;
                     sSql += "'" + sFecha + "', GETDATE(), " + Program.iMoneda + ", " + Environment.NewLine;
                     sSql += Convert.ToDecimal(dtAuxiliar.Rows[i][1].ToString()) + "," + Environment.NewLine;
                     sSql += "'COBRO No. CUENTA " + iNumeroPedido_P.ToString() + " (" + dtAuxiliar.Rows[i][0].ToString() + ")'," + Environment.NewLine;
                     sSql += "'" + sMovimiento.Trim() + "', " + Convert.ToInt32(dtAuxiliar.Rows[i][5].ToString()) + ", " + Program.iJORNADA + "," + Environment.NewLine;
                     sSql += "'A', GETDATE(), '" + Program.sDatosMaximo[0] + "', '" + Program.sDatosMaximo[1] + "')";
                     
                     if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                     {
                         catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                         catchMensaje.ShowDialog();
                         return false;
                     }

                     dtConsulta = new DataTable();
                     dtConsulta.Clear();

                     sTabla = "pos_movimiento_caja";
                     sCampo = "id_pos_movimiento_caja";

                     iMaximo= conexion.GFun_Ln_Saca_Maximo_ID(sTabla, sCampo, "", Program.sDatosMaximo);

                     if (iMaximo == -1)
                     {
                         ok.LblMensaje.Text = "No se pudo obtener el codigo de la tabla " + sTabla;
                         int num2 = (int)ok.ShowDialog();
                         return false;
                     }

                     iIdPosMovimientoCaja = Convert.ToInt32(iMaximo);

                     sSql = "";
                     sSql += "select numeromovimientocaja" + Environment.NewLine;
                     sSql += "from tp_localidades_impresoras" + Environment.NewLine;
                     sSql += "where id_localidad = " + Program.iIdLocalidad;
                     
                     dtConsulta = new DataTable();
                     dtConsulta.Clear();
                     bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                     if (bRespuesta == false)
                     {
                         catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                         catchMensaje.ShowDialog();
                         return false;
                     }

                     iNumeroMovimientoCaja = Convert.ToInt32(dtConsulta.Rows[0][0].ToString());

                     sSql = "";
                     sSql += "insert into pos_numero_movimiento_caja (" + Environment.NewLine;
                     sSql += "id_pos_movimiento_caja, numero_movimiento_caja, estado," + Environment.NewLine;
                     sSql += "fecha_ingreso, usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                     sSql += "values (" + Environment.NewLine;
                     sSql += iIdPosMovimientoCaja + ", " + iNumeroMovimientoCaja + ", 'A', GETDATE()," + Environment.NewLine;
                     sSql += "'" + Program.sDatosMaximo[0] + "', '" + Program.sDatosMaximo[1] + "')";

                     if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                     {
                         catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                         catchMensaje.ShowDialog();
                         return false;
                     }

                     sSql = "";
                     sSql += "update tp_localidades_impresoras set" + Environment.NewLine;
                     sSql += "numeromovimientocaja = numeromovimientocaja + 1" + Environment.NewLine;
                     sSql += "where id_localidad = " + Program.iIdLocalidad;

                     if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                     {
                         catchMensaje.LblMensaje.Text = "ERROR EN LA SIGUIENTE INSTRUCCIÓN:" + Environment.NewLine + sSql;
                         catchMensaje.ShowDialog();
                         return false;
                     }
                 }

                 return true;                 
             }

             catch (Exception ex)
             {
                 return false;
             }
         }

         private void crearDataTable()
         {
             try
             {
                 dtAlmacenar = new DataTable();
                 dtAlmacenar.Columns.Add("id_forma_pago");
                 dtAlmacenar.Columns.Add("valor");
             }
             catch (Exception ex)
             {
                 catchMensaje.LblMensaje.Text = ex.ToString();
                 catchMensaje.ShowDialog();
             }
         }


        #endregion
    }
}
