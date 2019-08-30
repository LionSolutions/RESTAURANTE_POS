using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Palatium
{
    public partial class MotivoCancelación : Form
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        VentanasMensajes.frmMensajeOK ok = new VentanasMensajes.frmMensajeOK();
        VentanasMensajes.frmMensajeCatch catchMensaje = new VentanasMensajes.frmMensajeCatch();

        string sIdOrden;
        string sSql;
        string sFecha;
        DataTable dtConsulta;
        bool bRespuesta = false;
        Double DSumaDetalleOrden;


        public MotivoCancelación(string sIdOrden)
        {
            this.sIdOrden = sIdOrden;
            InitializeComponent();            
        }

        #region FUNCIONES DEL USUARIO

        //FUNCION ACTIVA TECLADO
        private void activaTeclado()
        {
            this.TecladoVirtual.SetShowTouchKeyboard(this.txtMotivo, DevComponents.DotNetBar.Keyboard.TouchKeyboardStyle.Floating);
        }

        //EXTRAER EL TOTAL DE LA ORDEN PARA ALMACENAR EN LA BASE DE DATOS
        private void sumarTotalOrden()
        {
            try
            {
                sSql = "select sum(DP.cantidad * DP.precio_unitario * (" + Convert.ToDouble(Program.iva + Program.servicio + 1) + ")) total from cv403_det_pedidos as DP, cv403_cab_pedidos as CP " +
                       "where (CP.id_pedido = DP.id_pedido) and CP.id_pedido = " + Convert.ToInt32(sIdOrden) + " and CP.estado = 'A' and DP.estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();
                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    DSumaDetalleOrden = Convert.ToDouble(dtConsulta.Rows[0].ItemArray[0].ToString());
                }

            }
            catch (Exception)
            {
                ok.LblMensaje.Text = "Ocurrió un problema al consultar el registro.";
                ok.ShowInTaskbar = false;
                ok.ShowDialog();
                this.Close();
            }
        }


        //FUNCION PARA INSERTAR EN LA BASE DE DATOS
        private void insertarRegistro()
        {
            try
            {
                //INICIAMOS UNA NUEVA TRANSACCION
                //=======================================================================================================
                //=======================================================================================================
                if (!conexion.GFun_Lo_Maneja_Transaccion(Program.G_INICIA_TRANSACCION))
                {
                    ok.LblMensaje.Text = "Error al abrir transacción";
                    ok.ShowInTaskbar = false;
                    ok.ShowDialog();
                    goto fin;
                }

                else
                {
                    //INSERTAMOS INFORMACION EN LA TABLA DE CANCELACIONES
                    //=========================================================================================================
                    //=========================================================================================================

                    sSql = "insert into pos_cancelacion (id_pedido, motivo_cancelacion, estado, " +
                           "fecha_ingreso, usuario_ingreso, terminal_ingreso) " +
                           "values (" + Convert.ToInt32(sIdOrden)  + ",'" + txtMotivo.Text + "', 'A', GETDATE()," +
                           "'" + Program.sDatosMaximo[0] + "', '" + Program.sDatosMaximo[1] + "')";

                    if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                    {
                        catchMensaje.LblMensaje.Text = sSql;
                        catchMensaje.ShowDialog();
                        goto reversa;
                    }
                    //=========================================================================================================

                    //ACTUALIZAMOS EL ESTADO EN LA TABLA CV403_CAB_PEDIDOS
                    //=========================================================================================================
                    //=========================================================================================================

                    sSql = "update cv403_cab_pedidos set " +
                           "fecha_cierre_orden = '" +  sFecha+ "', estado_orden = 'Cancelada', " +
                           "valor_cancelado = " + DSumaDetalleOrden + ", " +
                           "estado = 'N', fecha_anula = GETDATE(), " +
                           "usuario_anula = '" + Program.sDatosMaximo[0] + "', " +
                           "terminal_anula = '" + Program.sDatosMaximo[1] + "' " +
                           "where id_pedido = " + Convert.ToInt32(sIdOrden);

                    if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                    {
                        catchMensaje.LblMensaje.Text = sSql;
                        catchMensaje.ShowInTaskbar = false;
                        catchMensaje.ShowDialog();
                        goto reversa;
                    }
                    //=========================================================================================================


                    //SI NO HUBO INCONVENIENTES REALIZA EL COMMIT PARA ALMACENAR LA INFORMACIÓN
                    //=========================================================================================================
                    //=========================================================================================================

                    conexion.GFun_Lo_Maneja_Transaccion(Program.G_TERMINA_TRANSACCION);
                    ok.LblMensaje.Text = "La orden ha sido cancelada éxitosamente.";
                    ok.ShowInTaskbar = false;
                    ok.ShowDialog();
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                    goto fin;
                }
            }
            catch (Exception)
            {
                goto reversa;
            }

            //ACCEDER A HACER EL ROLLBACK
             //=======================================================================================================
            reversa:
                {
                    conexion.GFun_Lo_Maneja_Transaccion(Program.G_REVERSA_TRANSACCION);
                    ok.LblMensaje.Text = "Ocurrió un problema en la transacción. No se guardarán los cambios.";
                    ok.ShowInTaskbar = false;    
                    ok.ShowDialog();
                }

            //=======================================================================================================
            fin:
                {

                }
        }


        #endregion

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (txtMotivo.Text == "")
            {
                ok.LblMensaje.Text = "Debe ingresar un motivo de la cancelación del pedido.";
                ok.ShowInTaskbar = false;
                ok.ShowDialog();
                txtMotivo.Focus();
            }
            else
            {
                insertarRegistro();
            }
        }

        private void MotivoCancelación_Load(object sender, EventArgs e)
        {
            sumarTotalOrden();

            if (Program.iActivaTeclado == 1)
            {
                activaTeclado();
                this.ActiveControl = label1;
            }

            else
            {
                this.ActiveControl = txtMotivo;
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MotivoCancelación_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void txtMotivo_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

    }
}
