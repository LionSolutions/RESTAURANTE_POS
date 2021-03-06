﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics; 

namespace Palatium
{
    public partial class VerificarCuentaMesa : Form
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        VentanasMensajes.frmMensajeOK ok = new VentanasMensajes.frmMensajeOK();
        VentanasMensajes.frmMensajeCatch catchMensaje = new VentanasMensajes.frmMensajeCatch();

        Clases.ClasePrecuentaMesas precuenta = new Clases.ClasePrecuentaMesas();
        Clases.ClaseAbrirCajon abrir = new Clases.ClaseAbrirCajon();

        bool bRespuesta;

        Label[] etiqueta = new Label[50];

        TextBox[] lista = new TextBox[50];
        TextBox listaSelecionada;      
        
        Button bBoton;
        
        StreamWriter sw;
        Button[] botonImprimir = new Button[50];
        Button[] botonPagar = new Button[50];
        Button botonSeleccionado;
        Button botonDinamico;

        ToolTip ttMensaje = new ToolTip();        

        string texto = "";
        string sFechaActual;
        string sRetorno;
        string sSql;
        string sDescripcionOrigen;
        string sNombreMesero;

        int iIdPersona;
        int iBuscarIDOrden;
        int iIdPosMesa;
        int iIdPosSeccion;
        int iAnchoDePrecuenta = 30;
        int iAnchoDeDescripcion = 20;
        int iAnchoDePrecio = 8;
        int iControlarTamanoLetra;
        int iCoordenadaX = 0;
        int contadorDeCuentas = 1;
        int iContador = 0;
        int iIdOrigenOrden;
        int iNumeroPersonas;
        int iIdCajero;
        int iIdMesero;

        double dSubtotal = 0;
        double dIVA = 0;
        double dServicio = 0;
        double dDescuento = 0;
        string sCantidad;
        string sComentario;
        string sNombreProducto;
        double dbCantidad;
        double dbUnitario;
        double dbIva;
        double dbDescuento;
        double dbDescuentoItem;
        double dbServicio;
        double dbSubtotal;
        double subtotal1 = 0;
        double subtotal = 0;
        double iva = 0;
        double recargo = 0;
        double total = 0;

        DataTable dtConsulta;
        DataTable dtComanda;


        #region FUNCIONES DEL USUARIO

        private void llenarDatosPreCuenta(TextBox lista)
        {
            try
            {
                sSql = "";
                sSql += "select * from pos_vw_det_pedido" + Environment.NewLine;
                sSql += "where id_pedido = " + Convert.ToInt32(lista.Tag) + Environment.NewLine;
                sSql += "order by id_det_pedido";

                dtComanda = new DataTable();
                dtComanda.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtComanda, sSql);



                if (bRespuesta == true)
                {
                    if (dtComanda.Rows.Count > 0)
                    {
                        lista.Text = precuenta.llenarPrecuenta(dtComanda, Convert.ToInt32(lista.Tag));
                    }

                    else
                    {
                        lista.Text = "";
                    }
                }

                else
                {
                    catchMensaje.LblMensaje.Text = sSql;
                    catchMensaje.ShowDialog();
                    lista.Text = "";
                }
            }

            catch (Exception ex) 
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
                lista.Text = "";
            }
        }

        #endregion

        #region FUNCIONES DE CONTROL DE BOTONES

        //INGRESAR EL CURSOR AL BOTON
        private void ingresaBoton(Button btnProceso)
        {
            btnProceso.ForeColor = Color.Black;
            btnProceso.BackColor = Color.LawnGreen;
        }

        //SALIR EL CURSOR DEL BOTON
        private void salidaBoton(Button btnProceso)
        {
            btnProceso.ForeColor = Color.White;
            btnProceso.BackColor = Color.Navy;
        }

        //INGRESAR EL CURSOR AL BOTON DINAMICO
        private void ingresaBotonDinamico(Button btnProceso)
        {
            btnProceso.BackColor = Color.MediumBlue;
            btnProceso.ForeColor = Color.White;
        }

        //SALIR EL CURSOR DEL BOTON DINAMICO
        private void salidaBotonDinamico(Button btnProceso)
        {
            btnProceso.BackColor = Color.FromArgb(255, 192, 128);
            btnProceso.ForeColor = Color.Black;
        }

        #endregion   

        public VerificarCuentaMesa(string iIdMesa, Button boton)
        {
            this.iIdPosMesa = Convert.ToInt32(iIdMesa);
            this.bBoton = boton;
            InitializeComponent();
            llenarArreglo();
            //verificarOrdenesHijas();
        }

        private void llenarArreglo()
        {
            string sFechaActual1 = Program.sFechaSistema.ToString("yyyy-MM-dd");

            sSql = "";
            sSql += "select CP.id_pedido, CP.id_pos_mesa, M.descripcion," + Environment.NewLine;
            sSql += "C.descripcion, origen.descripcion," + Environment.NewLine;
            sSql += "CP.fecha_orden, CP.numero_personas," + Environment.NewLine;
            //sSql += "CP.comentarios, MS.descripcion" + Environment.NewLine;
            sSql += "CP.comentarios, MS.descripcion, seccion.descripcion" + Environment.NewLine;
            sSql += "from cv403_cab_pedidos as CP," + Environment.NewLine;
            sSql += "pos_mesa as M," + Environment.NewLine;
            sSql += "pos_cajero as C," + Environment.NewLine;
            sSql += "pos_origen_orden as origen," + Environment.NewLine;
            //sSql += "pos_mesero as MS" + Environment.NewLine;
            sSql += "pos_mesero as MS," + Environment.NewLine;
            sSql += "pos_seccion_mesa as seccion" + Environment.NewLine;
            sSql += "where (M.id_pos_mesa = CP.id_pos_mesa)" + Environment.NewLine;
            sSql += "and (seccion.id_pos_seccion_mesa = M.id_pos_seccion_mesa)" + Environment.NewLine;
            sSql += "and (MS.id_pos_mesero = CP.id_pos_mesero)" + Environment.NewLine;
            sSql += "and (C.id_pos_cajero = CP.id_pos_cajero)" + Environment.NewLine;
            sSql += "and (origen.id_pos_origen_orden = CP.id_pos_origen_orden)" + Environment.NewLine;
            sSql += "and CP.id_pos_mesa = " + iIdPosMesa + Environment.NewLine;
            sSql += "and CP.estado_orden in('Abierta', 'Pre-Cuenta')" + Environment.NewLine;
            sSql += "and CP.fecha_orden = '" + sFechaActual1 + "'" + Environment.NewLine;
            sSql += "and CP.id_localidad = " + Program.iIdLocalidad + Environment.NewLine;
            sSql += "and CP.estado = 'A'" + Environment.NewLine;
            sSql += "and M.estado = 'A'" + Environment.NewLine;
            sSql += "and C.estado = 'A'" + Environment.NewLine;
            sSql += "and seccion.estado = 'A'" + Environment.NewLine;
            sSql += "and origen.estado = 'A'" + Environment.NewLine;
            sSql += "and MS.estado = 'A'" + Environment.NewLine;
            sSql += "order by CP.id_pedido";

            DataTable dtConsulta = new DataTable();
            dtConsulta.Clear();
            bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);
           

            if (bRespuesta == true)
            {
                if (dtConsulta.Rows.Count > 0)
                {
                    Font fuente = new Font("Microsoft Sans Serif", 14);
                    //Microsoft Sans Serif; 18pt

                    for (int i = 0; i < dtConsulta.Rows.Count; i++)
                    {
                        int iIdPedido = Convert.ToInt32(dtConsulta.Rows[i][0].ToString());
                        subtotal1 = 0;
                        subtotal = 0;
                        lista[i] = new TextBox();
                        lista[i].Multiline = true;
                        //lista[i].ScrollToCaret();
                        lista[i].ScrollBars = ScrollBars.Vertical;
                        lista[i].Width = 317;   //292
                        lista[i].Height = 540;
                        lista[i].Left = iContador * 300;    //300
                        lista[i].Name = dtConsulta.Rows[i][4].ToString(); //Guardo el origen de la orden
                        lista[i].Tag = iIdPedido; //Guardo el id del pedido
                        lista[i].AccessibleDescription = dtConsulta.Rows[i][2].ToString(); //guardo la descripción de la mesa
                        lista[i].AccessibleDefaultActionDescription = dtConsulta.Rows[i][7].ToString();
                        lista[i].TabIndex = i;                        

                        sSql = "";
                        sSql += "select numero_pedido " + Environment.NewLine;
                        sSql += "from cv403_numero_cab_pedido" + Environment.NewLine;
                        sSql += "where id_pedido = " + iIdPedido + "";

                        DataTable dtConsulta1 = new DataTable();
                        dtConsulta1.Clear();
                        bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta1, sSql);
                        if (bRespuesta == true)
                        {
                            lista[i].AccessibleName = dtConsulta1.Rows[0][0].ToString();
                        }
                        else
                        {
                            ok.LblMensaje.Text = "Ocurrió un problema al recuperar el número de orden";
                            ok.ShowDialog();
                        }
                        
                        //AGREGAR ETIQUETA SOBRE LA CAJA DE TEXTO
                        etiqueta[i] = new Label();
                        etiqueta[i].Font = new Font("Consolas", 15, FontStyle.Bold);                        
                        etiqueta[i].Location = new Point(iCoordenadaX, 0);
                        etiqueta[i].Height = 40;
                        etiqueta[i].Width = 315;    //290
                        etiqueta[i].ForeColor = System.Drawing.SystemColors.ControlLightLight;
                        etiqueta[i].Text = "Clic en la comanda\npara editar la orden";
                        etiqueta[i].TextAlign = ContentAlignment.MiddleCenter;
                        panel1.Controls.Add(etiqueta[i]);

                        //PARA CARGAR LOS DATOS EN EL TEXTBOX
                        lista[i].Click += listaClic;
                        lista[i].Font = new Font("Consolas", 10);
                        lista[i].Location = new Point(iCoordenadaX, 50);
                        ttMensaje.SetToolTip(lista[i], "Clic aquí para editar la orden.");
                        this.Controls.Add(lista[i]);
                        llenarDatosPreCuenta(lista[i]);
                        panel1.Controls.Add(lista[i]);

                        //Cargar los botones de imprimir
                        botonImprimir[i] = new Button();
                        botonImprimir[i].Cursor = Cursors.Hand;
                        botonImprimir[i].Font = fuente;
                        botonImprimir[i].BackColor = Color.FromArgb(255, 192, 128);
                        botonImprimir[i].TextAlign = ContentAlignment.MiddleRight;
                        botonImprimir[i].ImageAlign = ContentAlignment.MiddleLeft;
                        botonImprimir[i].Image = Palatium.Properties.Resources.impresora_icono;
                        botonImprimir[i].Width = 137;
                        botonImprimir[i].Height = 55;
                        botonImprimir[i].Click += clickBotonImprimir;
                        botonImprimir[i].MouseEnter += imprimir_mouse_enter;
                        botonImprimir[i].MouseLeave += imprimir_mouse_leave;
                        botonImprimir[i].Left = iContador * 350;
                        botonImprimir[i].Text = "Imprimir";
                        botonImprimir[i].Location = new Point(iCoordenadaX, 597);
                        botonImprimir[i].Tag = iIdPedido;
                        botonImprimir[i].TabIndex = i;
                        ttMensaje.SetToolTip(botonImprimir[i], "Clic aquí para imprimir la comanda.");
                        panel1.Controls.Add(botonImprimir[i]);

                        //Cargar los botones de pagar
                        botonPagar[i] = new Button();
                        botonPagar[i].Cursor = Cursors.Hand;
                        botonPagar[i].Font = fuente;
                        botonPagar[i].BackColor = Color.FromArgb(255, 192, 128);
                        botonPagar[i].TextAlign = ContentAlignment.MiddleRight;
                        botonPagar[i].ImageAlign = ContentAlignment.MiddleLeft;
                        botonPagar[i].Image = Palatium.Properties.Resources.cobrar_icono;
                        botonPagar[i].Width = 137;
                        botonPagar[i].Height = 55;
                        botonPagar[i].Click += clickBotonPagos;
                        botonPagar[i].MouseEnter += pagos_mouse_enter;
                        botonPagar[i].MouseLeave += pagos_mouse_leave;
                        botonPagar[i].Left = iContador *350;
                        botonPagar[i].Text = "Cobrar";
                        botonPagar[i].Location = new Point(iCoordenadaX + 180, 597);    //iCoordenadaX + 155, 597
                        botonPagar[i].Tag = iIdPedido;
                        botonPagar[i].TabIndex = i;
                        ttMensaje.SetToolTip(botonPagar[i], "Clic aquí para realizar el cobro de la comanda.");
                        iCoordenadaX += 325;    //300
                        panel1.Controls.Add(botonPagar[i]);
                        iContador++;
                    }
                }
            }
        }

        //Evento click en Botón órdenes hijas
        private void clickBotonImprimir(Object sender, EventArgs o)
        {
            try
            {
                Button botonImprimir = sender as Button;

                //FUNCION PARA VERIFICAR SI LA COMANDA YA FUE COBRADA
                //=====================================================================================
                sSql = "";
                sSql += "select estado_orden" + Environment.NewLine;
                sSql += "from cv403_cab_pedidos" + Environment.NewLine;
                sSql += "where id_pedido = " + Convert.ToInt32(botonImprimir.Tag) + Environment.NewLine;
                sSql += "and estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == false)
                {
                    catchMensaje.LblMensaje.Text = "ERROR EN LA INSTRUCCIÓN:" + Environment.NewLine + sSql;
                    catchMensaje.ShowDialog();
                    return;
                }

                string sEstado_R = dtConsulta.Rows[0]["estado_orden"].ToString().Trim().ToUpper();

                if (sEstado_R == "PAGADA")
                {
                    ok.LblMensaje.Text = "La comanda ya ha sido cobrada.";
                    ok.ShowDialog();
                    this.Close();
                    return;
                }

                //=====================================================================================

                //INICIAMOS UNA NUEVA TRANSACCION
                if (!conexion.GFun_Lo_Maneja_Transaccion(Program.G_INICIA_TRANSACCION))
                {
                    ok.LblMensaje.Text = "Error al abrir transacción";
                    ok.ShowDialog();
                    return;
                }

                sSql = "";
                sSql += "update cv403_cab_pedidos set" + Environment.NewLine;
                sSql += "estado_orden = 'Pre-Cuenta' " + Environment.NewLine;
                sSql += "where id_pedido = " + Convert.ToInt32(botonImprimir.Tag);

                if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                {
                    catchMensaje.LblMensaje.Text = sSql;
                    catchMensaje.ShowDialog();
                    goto reversa;
                }

                conexion.GFun_Lo_Maneja_Transaccion(Program.G_TERMINA_TRANSACCION);
                Pedidos.frmVerPrecuentaTextBox precuenta = new Pedidos.frmVerPrecuentaTextBox(botonImprimir.Tag.ToString(), 1, "Pre-Cuenta");
                precuenta.ShowDialog();

                return;
            }
            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
                goto reversa;
            }

            reversa: { conexion.GFun_Lo_Maneja_Transaccion(Program.G_REVERSA_TRANSACCION); }
        }
        
        //Evento click en Botón órdenes hijas
        private void clickBotonPagos(Object sender, EventArgs o)
        {
            try
            {
                if (Program.iPuedeCobrar == 1)
                {
                    Button botonpagos = sender as Button;

                    //FUNCION PARA VERIFICAR SI LA COMANDA YA FUE COBRADA
                    //=====================================================================================
                    sSql = "";
                    sSql += "select estado_orden" + Environment.NewLine;
                    sSql += "from cv403_cab_pedidos" + Environment.NewLine;
                    sSql += "where id_pedido = " + Convert.ToInt32(botonpagos.Tag) + Environment.NewLine;
                    sSql += "and estado = 'A'";

                    dtConsulta = new DataTable();
                    dtConsulta.Clear();

                    bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                    if (bRespuesta == false)
                    {
                        catchMensaje.LblMensaje.Text = "ERROR EN LA INSTRUCCIÓN:" + Environment.NewLine + sSql;
                        catchMensaje.ShowDialog();
                        return;
                    }

                    string sEstado_R = dtConsulta.Rows[0]["estado_orden"].ToString().Trim().ToUpper();

                    if (sEstado_R == "PAGADA")
                    {
                        ok.LblMensaje.Text = "La comanda ya ha sido cobrada.";
                        ok.ShowDialog();
                        this.Close();
                        return;
                    }

                    //=====================================================================================

                    dSubtotal = 0;
                    dIVA = 0;
                    dServicio = 0;
                    dDescuento = 0;
                    Program.iBanderaCerrarVentana = 1;

                    sSql = "";
                    sSql += "select * from pos_vw_det_pedido" + Environment.NewLine;
                    sSql += "where id_pedido = " + Convert.ToInt32(botonpagos.Tag);

                    dtConsulta = new DataTable();
                    dtConsulta.Clear();

                    bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                    if (bRespuesta == true)
                    {
                        if (dtConsulta.Rows.Count > 0)
                        {
                            Program.iGeneraFactura = Convert.ToInt32(dtConsulta.Rows[0][51].ToString());

                            for (int j = 0; j < dtConsulta.Rows.Count; j++)
                            {
                                dbCantidad = Convert.ToDouble(dtConsulta.Rows[j][4].ToString());

                                dSubtotal = dSubtotal + (dbCantidad * (Convert.ToDouble(dtConsulta.Rows[j][5].ToString())));
                                dIVA = dIVA + (Convert.ToDouble(dtConsulta.Rows[j][10].ToString()) * dbCantidad);
                                dServicio = dServicio + (Convert.ToDouble(dtConsulta.Rows[j][11].ToString()) * dbCantidad);
                                dDescuento = dDescuento + (Convert.ToDouble(dtConsulta.Rows[j][7].ToString()) * dbCantidad);
                            }

                            Pedidos.frmCobros frmCobros = new Pedidos.frmCobros(botonpagos.Tag.ToString());
                            AddOwnedForm(frmCobros);
                            frmCobros.ShowDialog();

                            if (frmCobros.DialogResult == DialogResult.OK)
                            {
                                if (iContador == 1)
                                {
                                    DialogResult = DialogResult.OK;
                                    Close();
                                }
                                else
                                {
                                    iCoordenadaX = 0;
                                    iContador = 0;
                                    panel1.Controls.Clear();
                                    llenarArreglo();
                                }
                            }


                            ////ABRIMOS EL FORMULARIO DE PAGOS
                            //PagoTarjetas t;
                            //t = new PagoTarjetas(botonpagos.Tag.ToString(), (dSubtotal + dIVA + dServicio - dDescuento));

                            //AddOwnedForm(t);
                            //t.ShowInTaskbar = false;
                            //t.ShowDialog();

                            //if (t.DialogResult == DialogResult.OK)
                            //{
                            //    if (iContador == 1)
                            //    {
                            //        this.DialogResult = DialogResult.OK;
                            //        this.Close();
                            //    }

                            //    else
                            //    {
                            //        iCoordenadaX = 0;
                            //        iContador = 0;
                            //        panel1.Controls.Clear();
                            //        llenarArreglo();
                            //    }
                            //}
                        }
                    }

                    else
                    {
                        catchMensaje.LblMensaje.Text = sSql;
                        catchMensaje.ShowDialog();
                    }
                }

                else
                {
                    ok.LblMensaje.Text = "Su usuario no le permite realizar el cobro de la cuenta.";
                    ok.ShowDialog();
                }
            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                Program.iBanderaCerrarVentana = 0;
                catchMensaje.ShowDialog();
            }
        }

        //EVENTO DEL TIMER
        private void timerBlink_Tick(object sender, EventArgs e)
        {
            Random rand = new Random();
            int uno = rand.Next(0, 255);
            int dos = rand.Next(0, 255);
            int tres = rand.Next(0, 255);
            int cuatro = rand.Next(0, 255);
        }
        
        //EVENTOS DE LOS BOTONES PARA PASAR EL CURSOR
        private void imprimir_mouse_enter(object sender, EventArgs e)
        {
            botonDinamico = sender as Button;
            ingresaBotonDinamico(botonDinamico);
        }

        private void pagos_mouse_enter(object sender, EventArgs e)
        {
            botonDinamico = sender as Button;
            ingresaBotonDinamico(botonDinamico);
        }

        private void imprimir_mouse_leave(object sender, EventArgs e)
        {
            botonDinamico = sender as Button;
            salidaBotonDinamico(botonDinamico);
        }

        private void pagos_mouse_leave(object sender, EventArgs e)
        {
            botonDinamico = sender as Button;
            salidaBotonDinamico(botonDinamico);
        }

        private void listaClic(Object sender, EventArgs o)
        {
            try
            {
                listaSelecionada = sender as TextBox;

                //FUNCION PARA VERIFICAR SI LA COMANDA YA FUE COBRADA
                //=====================================================================================
                sSql = "";
                sSql += "select estado_orden" + Environment.NewLine;
                sSql += "from cv403_cab_pedidos" + Environment.NewLine;
                sSql += "where id_pedido = " + Convert.ToInt32(listaSelecionada.Tag) + Environment.NewLine;
                sSql += "and estado = 'A'";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == false)
                {
                    catchMensaje.LblMensaje.Text = "ERROR EN LA INSTRUCCIÓN:" + Environment.NewLine + sSql;
                    catchMensaje.ShowDialog();
                    return;
                }

                string sEstado_R = dtConsulta.Rows[0]["estado_orden"].ToString().Trim().ToUpper();

                if (sEstado_R == "PAGADA")
                {
                    ok.LblMensaje.Text = "La comanda ya ha sido cobrada.";
                    ok.ShowDialog();
                    this.Close();
                    return;
                }

                //=====================================================================================


                this.DialogResult = DialogResult.OK;
                this.Close();

                sSql = "";
                sSql += "select CP.id_pos_origen_orden, CP.id_pos_mesero," + Environment.NewLine;
                sSql += "OO.descripcion, OO.genera_factura, OO.id_persona," + Environment.NewLine;
                sSql += "OO.id_pos_modo_delivery, OO.presenta_opcion_delivery, OO.codigo," + Environment.NewLine;
                sSql += "CP.id_persona as id_persona_pedido, isnull(CP.id_pos_mesa, 0) id_pos_mesa," + Environment.NewLine;
                sSql += "isnull(CP.numero_personas, 0) numero_personas, CP.id_pos_cajero, MS.descripcion" + Environment.NewLine;
                sSql += "from cv403_cab_pedidos CP INNER JOIN" + Environment.NewLine;
                sSql += "pos_origen_orden OO ON OO.id_pos_origen_orden = CP.id_pos_origen_orden" + Environment.NewLine;
                sSql += "and OO.estado = 'A'" + Environment.NewLine;
                sSql += "and CP.estado = 'A' INNER JOIN" + Environment.NewLine;
                sSql += "pos_mesero MS ON MS.id_pos_mesero = CP.id_pos_mesero" + Environment.NewLine;
                sSql += "and MS.estado = 'A'" + Environment.NewLine;
                sSql += "where CP.id_pedido = " + Convert.ToInt32(listaSelecionada.Tag);

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        iIdOrigenOrden = Convert.ToInt32(dtConsulta.Rows[0][0].ToString());
                        sDescripcionOrigen = dtConsulta.Rows[0][2].ToString();
                        Program.iGeneraFactura = Convert.ToInt32(dtConsulta.Rows[0][3].ToString());
                        Program.iIdPosModoDelivery = Convert.ToInt32(dtConsulta.Rows[0][5].ToString());
                        Program.iPresentaOpcionDelivery = Convert.ToInt32(dtConsulta.Rows[0][6].ToString());
                        Program.sCodigoAsignadoOrigenOrden = dtConsulta.Rows[0][7].ToString();
                        iIdPersona = Convert.ToInt32(dtConsulta.Rows[0][8].ToString());
                        iIdPosMesa = Convert.ToInt32(dtConsulta.Rows[0][9].ToString());
                        iNumeroPersonas = Convert.ToInt32(dtConsulta.Rows[0][10].ToString());
                        iIdCajero = Convert.ToInt32(dtConsulta.Rows[0][11].ToString());
                        iIdMesero = Convert.ToInt32(dtConsulta.Rows[0][1].ToString());
                        sNombreMesero = dtConsulta.Rows[0][12].ToString();
                    }

                    Orden ord = new Orden(iIdOrigenOrden, sDescripcionOrigen, iNumeroPersonas, iIdPosMesa, Convert.ToInt32(listaSelecionada.Tag), "OK", iIdPersona, iIdCajero, iIdMesero, sNombreMesero);
                    ord.ShowDialog();

                    return;
                }

                else
                {
                    catchMensaje.LblMensaje.Text = sSql;
                    catchMensaje.ShowDialog();
                    return;
                }
            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
                return;
            }
        }

        private void VerificarCuentaMesa_Load(object sender, EventArgs e)
        {
            Clases.ClaseRedimension redimension = new Clases.ClaseRedimension();
            redimension.ResizeForm(this, Program.iLargoPantalla, Program.iAnchoPantalla);
        }

        private void VerificarCuentaMesa_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }

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

        private void btnNueva_MouseEnter(object sender, EventArgs e)
        {
            ingresaBoton(btnNueva);
        }

        private void btnNueva_MouseLeave(object sender, EventArgs e)
        {
            salidaBoton(btnNueva);
        }

        private void btnSalir_MouseEnter(object sender, EventArgs e)
        {
            ingresaBoton(btnSalir);
        }

        private void btnSalir_MouseLeave(object sender, EventArgs e)
        {
            salidaBoton(btnSalir);
        }

        private void btnNueva_Click(object sender, EventArgs e)
        {
            //NMesas numeroPersona = new NMesas(bBoton, 2, "Para la Mesa");
            NMesas numeroPersona = new NMesas(bBoton, 2, iIdPosMesa);
            //Program.sREFERENCIA_ORDEN = id_orden;

            if (Program.iBanderaNumeroMesa == 1)
            {
                numeroPersona.Text = "INGRESE EL NÚMERO DE PERSONAS - " + bBoton.Text.ToUpper();
            }

            else
            {
                numeroPersona.Text = "INGRESE EL NUMERO DE PERSONAS - MESA " + bBoton.Text.ToUpper();
            }

            numeroPersona.ShowDialog();

            if (numeroPersona.DialogResult == DialogResult.OK)
            {
                this.Close();
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }   
    }
}