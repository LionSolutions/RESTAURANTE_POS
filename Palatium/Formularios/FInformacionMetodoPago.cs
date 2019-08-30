﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;
using System.Net;
using System.Security.Util;
using ConexionBD;

namespace Palatium.Formularios
{
    public partial class FInformacionMetodoPago : Form
    {
        //VARIABLES, INSTANCIAS
        VentanasMensajes.frmMensajeCatch catchMensaje = new VentanasMensajes.frmMensajeCatch();
        VentanasMensajes.frmMensajeOK ok = new VentanasMensajes.frmMensajeOK();
        VentanasMensajes.frmMensajeSiNo SiNo = new VentanasMensajes.frmMensajeSiNo();

        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        bool modificar = false;
        bool bRespuesta;

        DataTable dtConsulta;

        string sEstado;
        string sSql;

        int iIdRegistro;

        public FInformacionMetodoPago()
        {
            InitializeComponent();
        }

        private void FInformacionMetodoPago_Load(object sender, EventArgs e)
        {
            limpiar();
        }

        #region FUNCIONES DEL USUARIO

        //LIPIAR LAS CAJAS DE TEXTO
        private void limpiarTodo()
        {
            txtBuscar.Clear();
            txtCodigo.Enabled = true;
            txtCodigo.Clear();
            txtDescripcion.Clear();
            cmbEstado.Text = "ACTIVO";
            llenarGrid(0);
        }

        //FUNCION COLUMNAS GRID
        private void columnasGrid(bool ok)
        {
            dgvDatos.Columns[0].Visible = ok;
            dgvDatos.Columns[1].Width = 75;
            dgvDatos.Columns[2].Width = 150;
            dgvDatos.Columns[3].Width = 75;
        }

        //FUNCION PARA LLENAR EL GRID
        private void llenarGrid(int iOp)
        {
            try
            {
                sSql = "";
                sSql += "select id_pos_metodo_pago, codigo CÓDIGO, descripcion DESCRIPCIÓN," + Environment.NewLine;
                sSql += "case estado when 'A' then 'ACTIVO' else 'INACTIVO' end ESTADO" + Environment.NewLine;
                sSql += "from pos_metodo_pago" + Environment.NewLine;
                sSql += "where estado in ('A', 'N')" + Environment.NewLine;
                
                if (iOp == 1)
                {
                    sSql += "and (codigo like '%" + txtBuscar.Text.Trim() + "%'" + Environment.NewLine;
                    sSql += "or descripcion like '%" + txtBuscar.Text.Trim() + "%')" + Environment.NewLine;
                }

                sSql += "order by id_pos_metodo_pago";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    dgvDatos.DataSource = dtConsulta;
                    columnasGrid(false);
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

        //FUNCION PARA INSERTAR UN REGISTRO
        private void insertarRegistro()
        {
            try
            {
                //AQUI INICIA PROCESO DE ELIMINAR
                if (!conexion.GFun_Lo_Maneja_Transaccion(Program.G_INICIA_TRANSACCION))
                {
                    ok.LblMensaje.Text = "Error al abrir transacción.";
                    ok.ShowDialog();
                    goto fin;
                }

                sSql = "";
                sSql += "insert into pos_metodo_pago (" + Environment.NewLine;
                sSql += "codigo, descripcion, estado, fecha_ingreso, usuario_ingreso, terminal_ingreso)" + Environment.NewLine;
                sSql += "values (" + Environment.NewLine;
                sSql += "'" + txtCodigo.Text.ToString().Trim() + "', '" + txtDescripcion.Text.ToString().Trim() + "', 'A'," + Environment.NewLine;
                sSql += "GETDATE(), '" + Program.sDatosMaximo[0] + "', '" + Program.sDatosMaximo[1] + "')";

                if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                {
                    catchMensaje.LblMensaje.Text = sSql;
                    catchMensaje.ShowDialog();
                    goto reversa;
                }

                conexion.GFun_Lo_Maneja_Transaccion(Program.G_TERMINA_TRANSACCION);
                ok.LblMensaje.Text = "Registro agregado éxitosamente.";
                ok.ShowDialog();
                limpiar();
                goto fin;
            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
                goto reversa;
            }

        reversa: { conexion.GFun_Lo_Maneja_Transaccion(Program.G_REVERSA_TRANSACCION); }

        fin: { }
        }


        //FUNCION PARA ACTUALIZAR UN REGISTRO
        private void actualizarRegistro()
        {
            try
            {
                //AQUI INICIA PROCESO DE ELIMINAR
                if (!conexion.GFun_Lo_Maneja_Transaccion(Program.G_INICIA_TRANSACCION))
                {
                    ok.LblMensaje.Text = "Error al abrir transacción.";
                    ok.ShowDialog();
                    goto fin;
                }


                sSql = "";
                sSql += "update pos_metodo_pago set" + Environment.NewLine;
                sSql += "descripcion = '" + txtDescripcion.Text.Trim().ToUpper() + "'," + Environment.NewLine;
                sSql += "estado = '" + sEstado + "'" + Environment.NewLine;
                sSql += "where id_pos_metodo_pago = " + iIdRegistro + Environment.NewLine;
                sSql += "and estado in ('A', 'N')";


                if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                {
                    catchMensaje.LblMensaje.Text = sSql;
                    catchMensaje.ShowDialog();
                    goto reversa;
                }

                conexion.GFun_Lo_Maneja_Transaccion(Program.G_TERMINA_TRANSACCION);
                ok.LblMensaje.Text = "Registro actualizado éxitosamente.";
                ok.ShowDialog();
                limpiar();
                goto fin;
            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
                goto reversa;
            }

        reversa: { conexion.GFun_Lo_Maneja_Transaccion(Program.G_REVERSA_TRANSACCION); }

        fin: { }
        }

        //FUNCION PARA ELIMINAR UN REGISTRO
        private void eliminarRegistro()
        {
            try
            {
                //AQUI INICIA PROCESO DE ELIMINAR
                if (!conexion.GFun_Lo_Maneja_Transaccion(Program.G_INICIA_TRANSACCION))
                {
                    ok.LblMensaje.Text = "Error al abrir transacción.";
                    ok.ShowDialog();
                    goto fin;
                }

                sSql = "";
                sSql += "update pos_metodo_pago set" + Environment.NewLine;
                sSql += "estado = 'E'," + Environment.NewLine;
                sSql += "fecha_anula = GETDATE()," + Environment.NewLine;
                sSql += "usuario_anula = '" + Program.sDatosMaximo[0] + "'," + Environment.NewLine;
                sSql += "terminal_anula = '" + Program.sDatosMaximo[1] + "'" + Environment.NewLine;
                sSql += "where id_pos_metodo_pago = " + iIdRegistro;


                if (!conexion.GFun_Lo_Ejecuta_SQL(sSql))
                {
                    catchMensaje.LblMensaje.Text = sSql;
                    catchMensaje.ShowDialog();
                    goto reversa;
                }

                conexion.GFun_Lo_Maneja_Transaccion(Program.G_TERMINA_TRANSACCION);
                ok.LblMensaje.Text = "Registro eliminado éxitosamente.";
                ok.ShowDialog();
                limpiar();
                goto fin;
            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
                goto reversa;
            }

        reversa: { conexion.GFun_Lo_Maneja_Transaccion(Program.G_REVERSA_TRANSACCION); }

        fin: { }
        }

        //FUNCION PARA LIMPIAR
        private void limpiar()
        {
            txtBuscar.Clear();
            txtCodigo.Clear();
            txtDescripcion.Clear();
            cmbEstado.Text = "ACTIVO";
            cmbEstado.Enabled = false;
            llenarGrid(0);
        }

        #endregion

        private void Txt_Codigo_LostFocus(object sender, EventArgs e)
        {
            try
            {
                sSql = "";
                sSql += "select id_pos_metodo_pago, descripcion," + Environment.NewLine;
                sSql += "case estado when 'A' then 'ACTIVO' else 'INACTIVO' end estado" + Environment.NewLine;
                sSql += "from pos_metodo_pago" + Environment.NewLine;
                sSql += "where codigo = '" + txtCodigo.Text + "'" + Environment.NewLine;
                sSql += "and estado in ('A', 'N')";

                dtConsulta = new DataTable();
                dtConsulta.Clear();

                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    if (dtConsulta.Rows.Count > 0)
                    {
                        iIdRegistro = Convert.ToInt32(dtConsulta.Rows[0][0].ToString());
                        txtDescripcion.Text = dtConsulta.Rows[0][1].ToString().ToUpper();
                        cmbEstado.Text = dtConsulta.Rows[0][2].ToString().ToUpper();
                        btnAnularMetodoPago.Enabled = true;
                        btnNuevoMetodoPago.Text = "Actualizar";
                        txtCodigo.Enabled = false;
                        cmbEstado.Enabled = true;
                        txtDescripcion.Focus();
                    }

                    else
                    {
                        iIdRegistro = 0;
                        txtDescripcion.Clear();
                        cmbEstado.Text = "ACTIVO";
                        btnAnularMetodoPago.Enabled = false;
                        cmbEstado.Enabled = false;
                        btnNuevoMetodoPago.Text = "Guardar";
                        txtDescripcion.Focus();
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

        private void txtCodigoCajero_Leave(object sender, EventArgs e)
        {
            txtCodigo.LostFocus += new EventHandler(Txt_Codigo_LostFocus);
        }

        private void btnCerrarMetodoPago_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLimpiarMetodoPago_Click(object sender, EventArgs e)
        {
            Grb_DatoMetodoPago.Enabled = false;
            btnNuevoMetodoPago.Text = "Nuevo";
            limpiarTodo();
        }

        private void btnBuscarMetodoPago_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtBuscar.Text.Trim() == "")
                {
                    llenarGrid(0);
                }

                else
                {
                    llenarGrid(1);
                }
            }

            catch (Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
            }
        }

        private void btnNuevoMetodoPago_Click(object sender, EventArgs e)
        {
            //SI EL BOTON ESTA EN OPCION NUEVO
            if (btnNuevoMetodoPago.Text == "Nuevo")
            {
                limpiarTodo();
                Grb_DatoMetodoPago.Enabled = true;
                btnNuevoMetodoPago.Text = "Guardar";
                cmbEstado.Text = "ACTIVO";
                cmbEstado.Enabled = false;
                txtCodigo.Focus();
            }

            else 
            {
                if ((txtCodigo.Text == "") && (txtDescripcion.Text == ""))
                {
                    ok.LblMensaje.Text = "Debe rellenar todos los campos obligatorios.";
                    ok.ShowDialog();
                    txtCodigo.Focus();
                }

                else if (txtCodigo.Text == "")
                {
                    ok.LblMensaje.Text = "Favor ingrese el código del Método de pago.";
                    ok.ShowDialog();
                    txtCodigo.Focus();
                }

                else if (txtDescripcion.Text == "")
                {
                    ok.LblMensaje.Text = "Favor ingrese la descripción del Método de pago.";
                    ok.ShowDialog();
                    txtDescripcion.Focus();
                }

                else
                {
                    if (cmbEstado.Text == "ACTIVO")
                    {
                        sEstado = "A";
                    }

                    else
                    {
                        sEstado = "N";
                    }

                    if (btnNuevoMetodoPago.Text == "Guardar")
                    {
                        insertarRegistro();
                    }

                    else if (btnNuevoMetodoPago.Text == "Actualizar")
                    {
                        actualizarRegistro();
                    }
                }            
            }
        }

        private void btnAnularMetodoPago_Click(object sender, EventArgs e)
        {
            SiNo.LblMensaje.Text = "¿Está seguro que desea dar de baja el registro?";
            SiNo.ShowDialog();

            if (SiNo.DialogResult == DialogResult.OK)
            {
                eliminarRegistro();
            }
        }

        private void dgvDatos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                Grb_DatoMetodoPago.Enabled = true;
                txtCodigo.Enabled = false;
                btnNuevoMetodoPago.Text = "Actualizar";
                cmbEstado.Enabled = true;

                iIdRegistro = Convert.ToInt32(dgvDatos.CurrentRow.Cells[0].Value.ToString());
                txtCodigo.Text = dgvDatos.CurrentRow.Cells[1].Value.ToString();
                txtDescripcion.Text = dgvDatos.CurrentRow.Cells[2].Value.ToString();
                cmbEstado.Text = dgvDatos.CurrentRow.Cells[3].Value.ToString();

                txtDescripcion.Focus();
            }

            catch(Exception ex)
            {
                catchMensaje.LblMensaje.Text = ex.ToString();
                catchMensaje.ShowDialog();
            }
        }
    }
}
