using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Palatium.Empresa
{
    public partial class frmComandaClienteEmpresarial : Form
    {
        VentanasMensajes.frmMensajeNuevoSiNo NuevoSiNo = new VentanasMensajes.frmMensajeNuevoSiNo();
        VentanasMensajes.frmMensajeCatch catchMensaje = new VentanasMensajes.frmMensajeCatch();
        VentanasMensajes.frmMensajeOK ok = new VentanasMensajes.frmMensajeOK();
        VentanasMensajes.frmMensajeSiNo SiNo = new VentanasMensajes.frmMensajeSiNo();

         ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
         Button[,] boton = new Button[2, 4];
         Button[,] botonProductos = new Button[5, 5];
         
         
         string sSql;
         string sPagaIva_P;
         string sNombreProducto_P;
         string sNombreEmpresa;
         string sNombreEmpleado;
         string sFechaOrden;
         string sTabla;
         string sCampo;
         string sFechaConsulta;

         long iMaximo;

         bool bRespuesta;

         Button botonSeleccionadoCategoria;
         Button botonSeleccionadoProducto;

         DataTable dtConsulta;
         DataTable dtCategorias;
         DataTable dtProductos;

         int iPosXProductos;
         int iPosYProductos;
         int iCuentaAyudaProductos;
         int iCuentaCategorias;
         int iPosXCategorias;
         int iPosYCategorias;
         int iCuentaAyudaCategorias;
         int iCuentaProductos;
         int iIdPersona;
         int iIdPersonaEmpresa;
         int iIdOrigenOrden;
         int iIdPedido;
         int iCuentaDiaria;
         int iNumeroPedidoOrden;
         int iIdEventoCobro;
         int iIdCabDespachos;
         int iIdDespachoPedido;
         int iIdProducto_P;
         int iCgTipoDocumento = 2725;


         Decimal dPrecioUnitario_P;
         Decimal dCantidad_P;
         Decimal dIVA_P;
         Decimal dbIva_P;
         Decimal dbTotal_P;
         Decimal dTotalDebido;

         public frmComandaClienteEmpresarial(int iIdPersona_P, string sNombreEmpresa_P, string sNombreEmpleado_P, int iIdPersonaEmpresa_P, int iIdOrigenOrden_P)
         {
            this.iIdPersona = iIdPersona_P;
            this.sNombreEmpresa = sNombreEmpresa_P;
            this.sNombreEmpleado = sNombreEmpleado_P;
            this.iIdPersonaEmpresa = iIdPersonaEmpresa_P;
            this.iIdOrigenOrden = iIdOrigenOrden_P;
            InitializeComponent();
         }

        #region FUNCIONES EL USUARIO

        //FUNCION PARA EXTRAER EL NUMERO DE CUENTA
         private void extraerNumeroCuenta()
         {
             try
             {
                 sFechaConsulta = Program.sFechaSistema.ToString("yyyy/MM/dd");

                 sSql = "";
                 sSql += "select isnull(max(cuenta), 0) cuenta" + Environment.NewLine;
                 sSql += "from cv403_cab_pedidos" + Environment.NewLine;
                 sSql += "where fecha_pedido = '" + sFechaConsulta + "'";

                 dtConsulta = new DataTable();
                 dtConsulta.Clear();

                 bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                 if (bRespuesta == true)
                 {
                     iCuentaDiaria = Convert.ToInt32(dtConsulta.Rows[0][0].ToString()) + 1;
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

        //FUNCION PARA CARGAR LAS CATEGORIAS
         private void cargarCategorias()
         {
             try
             {
                 sSql = "";
                 sSql += "select P.id_Producto, NP.nombre as Nombre, P.paga_iva," + Environment.NewLine;
                 sSql += "P.subcategoria" + Environment.NewLine;
                 sSql += "from cv401_productos P INNER JOIN" + Environment.NewLine;
                 sSql += "cv401_nombre_productos NP ON P.id_Producto = NP.id_Producto" + Environment.NewLine;
                 sSql += "and P.estado ='A'" + Environment.NewLine;
                 sSql += "and NP.estado = 'A'" + Environment.NewLine;
                 sSql += "where P.nivel = 2" + Environment.NewLine;
                 sSql += "and P.maneja_almuerzos = 1" + Environment.NewLine;
                 sSql += "order by P.secuencia";

                 dtCategorias = new DataTable();
                 dtCategorias.Clear();
                 bRespuesta = conexion.GFun_Lo_Busca_Registro(dtCategorias, sSql);

                 if (bRespuesta == false)
                 {
                     catchMensaje.LblMensaje.Text = "ERROR EN LA INSTRUCCIÓN SQL:" + Environment.NewLine + sSql;
                     catchMensaje.ShowDialog();
                     return;
                 }

                 iCuentaCategorias = 0;

                 if (dtCategorias.Rows.Count > 0)
                 {
                     if (dtCategorias.Rows.Count > 8)
                     {
                         btnSiguiente.Enabled = true;
                         btnAnterior.Visible = true;
                         btnSiguiente.Visible = true;
                     }

                     else
                     {
                         btnSiguiente.Enabled = false;
                         btnAnterior.Visible = false;
                         btnSiguiente.Visible = false;
                     }

                     if (crearBotonesCategorias() == false)
                     { }

                 }

                 else
                 {
                     ok.LblMensaje.Text = "No se encuentras ítems de categorías en el sistema.";
                     ok.ShowDialog();
                 }

             }

             catch (Exception ex)
             {
                 catchMensaje.LblMensaje.Text = ex.Message;
                 catchMensaje.ShowDialog();
             }
         }

        //FUNCION PARA CREAR LOS BOTONES DE CATEGORIAS
         private bool crearBotonesCategorias()
         {
             try
             {
                 pnlCategorias.Controls.Clear();

                 iPosXCategorias = 0;
                 iPosYCategorias = 0;
                 iCuentaAyudaCategorias = 0;

                 for (int i = 0; i < 2; i++)
                 {
                     for (int j = 0; j < 4; j++)
                     {
                         boton[i, j] = new Button();
                         boton[i, j].Cursor = Cursors.Hand;
                         boton[i, j].Click += new EventHandler(boton_clic_categorias);
                         boton[i, j].Size = new Size(130, 71);
                         boton[i, j].Location = new Point(iPosXCategorias, iPosYCategorias);
                         boton[i, j].BackColor = Color.Lime;
                         boton[i, j].Font = new Font("Maiandra GD", 9.75f, FontStyle.Bold);
                         boton[i, j].Tag = dtCategorias.Rows[iCuentaCategorias]["id_producto"].ToString();
                         boton[i, j].Text = dtCategorias.Rows[iCuentaCategorias]["nombre"].ToString();
                         boton[i, j].AccessibleDescription = dtCategorias.Rows[iCuentaCategorias]["subcategoria"].ToString();
                         pnlCategorias.Controls.Add(boton[i, j]);

                         iCuentaCategorias++;
                         iCuentaAyudaCategorias++;

                         if (j + 1 == 4)
                         {
                             iPosXCategorias = 0;
                             iPosYCategorias += 71;
                         }

                         else
                         {
                             iPosXCategorias += 130;
                         }

                         if (dtCategorias.Rows.Count == iCuentaCategorias)
                         {
                             btnSiguiente.Enabled = false;
                             break;
                         }
                     }

                     if (dtCategorias.Rows.Count == iCuentaCategorias)
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

        //EVENTO CLIC DEL BOTON CATEGORIAS
         private void boton_clic_categorias(object sender, EventArgs e)
         {
             try
             {
                 Cursor = Cursors.WaitCursor;

                 botonSeleccionadoCategoria = sender as Button;

                 lblProductos.Text = botonSeleccionadoCategoria.Text.Trim().ToUpper();

                 if (Convert.ToInt32(botonSeleccionadoCategoria.AccessibleDescription) == 0)
                 {
                     cargarProductos(Convert.ToInt32(botonSeleccionadoCategoria.Tag), 3);
                 }

                 else
                 {
                     cargarProductos(Convert.ToInt32(botonSeleccionadoCategoria.Tag), 4);
                 }

                 Cursor = Cursors.Default;
             }

             catch (Exception ex)
             {
                 catchMensaje.LblMensaje.Text = ex.Message;
                 catchMensaje.ShowDialog();
             }
         }

        //FUNCION PARA CARGAR LOS PRODUCTOS
         private void cargarProductos(int iIdProducto_P, int iNivel_P)
         {
             try
             {
                 sSql = "";
                 sSql += "select P.id_Producto, NP.nombre as Nombre, P.paga_iva, PP.valor" + Environment.NewLine;
                 sSql += "from cv401_productos P INNER JOIN" + Environment.NewLine;
                 sSql += "cv401_nombre_productos NP ON P.id_Producto = NP.id_Producto" + Environment.NewLine;
                 sSql += "and P.estado ='A'" + Environment.NewLine;
                 sSql += "and NP.estado = 'A' INNER JOIN" + Environment.NewLine;
                 sSql += "cv403_precios_productos PP ON P.id_producto = PP.id_producto" + Environment.NewLine;
                 sSql += "and PP.estado = 'A'" + Environment.NewLine;
                 sSql += "where P.nivel = " + iNivel_P + Environment.NewLine;
                 sSql += "and PP.id_lista_precio = 4" + Environment.NewLine;
                 sSql += "and P.id_producto_padre = " + iIdProducto_P + Environment.NewLine;
                 sSql += "order by P.secuencia";

                 dtProductos = new DataTable();
                 dtProductos.Clear();
                 bRespuesta = conexion.GFun_Lo_Busca_Registro(dtProductos, sSql);

                 if (bRespuesta == false)
                 {
                     catchMensaje.LblMensaje.Text = "ERROR EN LA INSTRUCCIÓN SQL:" + Environment.NewLine + sSql;
                     catchMensaje.ShowDialog();
                     return;
                 }

                 iCuentaProductos = 0;

                 if (dtProductos.Rows.Count > 0)
                 {
                     if (dtProductos.Rows.Count > 25)
                     {
                         btnSiguienteProducto.Enabled = true;
                         btnSiguienteProducto.Visible = true;
                         btnAnteriorProducto.Visible = true;
                     }
                     else
                     {
                         btnSiguienteProducto.Enabled = false;
                         btnSiguienteProducto.Visible = false;
                         btnAnteriorProducto.Visible = false;
                     }
                     if (crearBotonesProductos() == false)
                     { }
                 }

                 else
                 {
                     ok.LblMensaje.Text = "No se encuentras ítems de categorías en el sistema.";
                     ok.ShowDialog();
                 }
             }

             catch (Exception ex)
             {
                 catchMensaje.LblMensaje.Text = ex.Message;
                 catchMensaje.ShowDialog();
             }
         }

        //FUNCION PARA CREAR LOS BOTONES DE PRODUCTOS
         private bool crearBotonesProductos()
         {
             try
             {
                 pnlProductos.Controls.Clear();

                 iPosXProductos = 0;
                 iPosYProductos = 0;
                 iCuentaAyudaProductos = 0;

                 for (int i = 0; i < 5; ++i)
                 {
                     for (int j = 0; j < 5; ++j)
                     {
                         botonProductos[i, j] = new Button();
                         botonProductos[i, j].Cursor = Cursors.Hand;
                         botonProductos[i, j].Click += new EventHandler(boton_clic_productos);
                         botonProductos[i, j].Size = new Size(130, 71);
                         botonProductos[i, j].Location = new Point(iPosXProductos, iPosYProductos);
                         botonProductos[i, j].BackColor = Color.FromArgb(255, 255, 128);
                         botonProductos[i, j].Font = new Font("Maiandra GD", 9.75f, FontStyle.Bold);
                         botonProductos[i, j].Tag = dtProductos.Rows[iCuentaProductos]["id_producto"].ToString();
                         botonProductos[i, j].Text = dtProductos.Rows[iCuentaProductos]["nombre"].ToString();
                         botonProductos[i, j].AccessibleDescription = dtProductos.Rows[iCuentaProductos]["paga_iva"].ToString();
                         botonProductos[i, j].AccessibleName = dtProductos.Rows[iCuentaProductos]["valor"].ToString();
                         pnlProductos.Controls.Add(botonProductos[i, j]);

                         iCuentaProductos++;
                         iCuentaAyudaProductos++;

                         if (j + 1 == 5)
                         {
                             iPosXProductos = 0;
                             iPosYProductos += 71;
                         }

                         else
                         {
                             iPosXProductos += 130;
                         }

                         if (dtProductos.Rows.Count == iCuentaProductos)
                         {
                             btnSiguienteProducto.Enabled = false;
                             break;
                         }
                     }

                     if (dtProductos.Rows.Count == iCuentaProductos)
                     {
                         btnSiguienteProducto.Enabled = false;
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

        //EVENTO CLIC DE LOS BOTONES DE PRODUCTOS
         private void boton_clic_productos(object sender, EventArgs e)
         {
             try
             {
                 Cursor = Cursors.WaitCursor;
                 botonSeleccionadoProducto = sender as Button;

                 int iExiste_R = 0;

                 Decimal num2 = 0;
                 Decimal num3;

                 Decimal dbCantidad_R;
                 Decimal dbValorUnitario_R;
                 Decimal dbSubtotal_R;
                 Decimal dbValorIVA_R;
                 Decimal dbTotal_R;


                 for (int i = 0; i < dgvPedido.Rows.Count; ++i)
                 {
                     if (dgvPedido.Rows[i].Cells["idProducto"].Value.ToString() == botonSeleccionadoProducto.Tag.ToString())
                     {
                         dbCantidad_R = Convert.ToDecimal(dgvPedido.Rows[i].Cells["cantidad"].Value);
                         dbCantidad_R += 1;
                         dgvPedido.Rows[i].Cells["cantidad"].Value = dbCantidad_R;
                         dbValorUnitario_R = Convert.ToDecimal(dgvPedido.Rows[i].Cells["valuni"].Value);
                         dbSubtotal_R = dbCantidad_R * dbValorUnitario_R;
                         dgvPedido.Rows[i].Cells["subtotal"].Value = dbSubtotal_R.ToString("N2");
                         dbValorIVA_R = dbSubtotal_R * Convert.ToDecimal(Program.iva);
                         dbTotal_R = dbSubtotal_R + dbValorIVA_R;
                         dgvPedido.Rows[i].Cells["valor"].Value = dbTotal_R.ToString("N2");
                         iExiste_R = 1;
                     }
                 }

                 if (iExiste_R == 0)
                 {
                     int i = dgvPedido.Rows.Add();

                     dgvPedido.Rows[i].Cells["cantidad"].Value = "1";
                     dgvPedido.Rows[i].Cells["producto"].Value = botonSeleccionadoProducto.Text.ToString().Trim();
                     sNombreProducto_P = botonSeleccionadoProducto.Text.ToString().Trim();
                     dgvPedido.Rows[i].Cells["idProducto"].Value = botonSeleccionadoProducto.Tag;
                     sPagaIva_P = botonSeleccionadoProducto.AccessibleDescription.ToString().Trim();
                     dgvPedido.Rows[i].Cells["pagaIva"].Value = sPagaIva_P;

                     if (sPagaIva_P == "1")
                     {
                         dgvPedido.Rows[i].DefaultCellStyle.ForeColor = Color.Blue;
                         dgvPedido.Rows[i].Cells["cantidad"].ToolTipText = sNombreProducto_P.Trim().ToUpper() + " PAGA IVA";
                         dgvPedido.Rows[i].Cells["producto"].ToolTipText = sNombreProducto_P.Trim().ToUpper() + " PAGA IVA";
                         dgvPedido.Rows[i].Cells["valor"].ToolTipText = sNombreProducto_P.Trim().ToUpper() + " PAGA IVA";
                     }
                     else
                     {
                         dgvPedido.Rows[i].DefaultCellStyle.ForeColor = Color.Purple;
                         dgvPedido.Rows[i].Cells["cantidad"].ToolTipText = sNombreProducto_P.Trim().ToUpper() + " NO PAGA IVA";
                         dgvPedido.Rows[i].Cells["producto"].ToolTipText = sNombreProducto_P.Trim().ToUpper() + " NO PAGA IVA";
                         dgvPedido.Rows[i].Cells["valor"].ToolTipText = sNombreProducto_P.Trim().ToUpper() + " NO PAGA IVA";
                     }

                     dbCantidad_R = 1;
                     dgvPedido.Rows[i].Cells["valuni"].Value = botonSeleccionadoProducto.AccessibleName;
                     dbValorUnitario_R = Convert.ToDecimal(botonSeleccionadoProducto.AccessibleName);
                     dbSubtotal_R = dbCantidad_R * dbValorUnitario_R;
                     dgvPedido.Rows[i].Cells["subtotal"].Value = dbSubtotal_R.ToString("N2");
                     dbValorIVA_R = dbSubtotal_R * Convert.ToDecimal(Program.iva);
                     dbTotal_R = dbSubtotal_R + dbValorIVA_R;
                     dgvPedido.Rows[i].Cells["valor"].Value = dbTotal_R.ToString("N2");
                 }

                 calcularTotales();
                 dgvPedido.ClearSelection();

                 Cursor = Cursors.Default;
             }

             catch (Exception ex)
             {
                 catchMensaje.LblMensaje.Text = ex.Message;
                 catchMensaje.ShowDialog();
             }
         }

        //FUNCION PARA CALCULAR TOTALES
         public void calcularTotales()
         {
             Decimal dSubtotalConIva = 0;
             Decimal dSubtotalCero = 0;
             dTotalDebido = 0;

             for (int i = 0; i < dgvPedido.Rows.Count; ++i)
             {
                 if (dgvPedido.Rows[i].Cells["pagaIva"].Value.ToString() == "0")
                 {
                     dSubtotalCero += Convert.ToDecimal(dgvPedido.Rows[i].Cells["cantidad"].Value.ToString()) * Convert.ToDecimal(dgvPedido.Rows[i].Cells["valuni"].Value.ToString());
                 }

                 else
                 {
                     dSubtotalConIva += Convert.ToDecimal(dgvPedido.Rows[i].Cells["cantidad"].Value.ToString()) * Convert.ToDecimal(dgvPedido.Rows[i].Cells["valuni"].Value.ToString());
                 }
             }

             //dTotalDebido = num1 + num2 - num3 - num4 + (num1 - num3) * Convert.ToDecimal(Program.iva) + num7;
             dTotalDebido = dSubtotalConIva + dSubtotalCero;
             lblTotal.Text = "$ " + dTotalDebido.ToString("N2");
         }

        #endregion
    }
}
