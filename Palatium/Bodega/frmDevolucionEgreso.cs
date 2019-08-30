﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Palatium.Bodega
{
    public partial class frmDevolucionEgreso : Form
    {
        ConexionBD.ConexionBD conexion = new ConexionBD.ConexionBD();
        string sSql;
        bool bRespuesta;
        DataTable dtConsulta = new DataTable();
        VentanasMensajes.frmMensajeOK ok = new VentanasMensajes.frmMensajeOK();

        public frmDevolucionEgreso()
        {
            InitializeComponent();
        }

        private void btnAnular_Click(object sender, EventArgs e)
        {

        }

        private void btnGrabar_Click(object sender, EventArgs e)
        {

        }

        private void dgvDetalleVenta_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Función para cargar el combo de oficina
        private void cargarComboOficina()
        {
            try
            {
                sSql = @"select id_localidad, nombre_localidad from tp_vw_localidades
                            where codigo in (00004,00005,00006) ";
                DataTable dtConsulta = new DataTable();
                dtConsulta.Clear();
                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    cmbOficina.llenar(dtConsulta, sSql);
                }
                else
                {
                    ok.LblMensaje.Text = "Ocurrió un problema al cargaar el combo de oficina";
                    ok.ShowDialog();
                }

            }
            catch (Exception)
            {
                ok.LblMensaje.Text = "Ocurrió un problema al cargaar el combo de oficina";
                ok.ShowDialog();
            }
        }

        //Función para cargar el combo de bodega
        private void cargarComboBodega()
        {
            try
            {
                sSql = @"select id_bodega, descripcion from cv402_bodegas
                            where categoria =1 ";

                DataTable dtConsulta = new DataTable();
                dtConsulta.Clear();
                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    cmbBodega.llenar(dtConsulta, sSql);
                }
                else
                {
                    ok.LblMensaje.Text = "Ocurrió un problema al cargaar el combo de oficina";
                    ok.ShowDialog();
                }

            }
            catch (Exception)
            {
                ok.LblMensaje.Text = "Ocurrió un problema al cargaar el combo de oficina";
                ok.ShowDialog();
            }
        }

        private void cmbOficina_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbOficina.SelectedIndex > 0)
            {
                sSql = @"select id_bodega from tp_vw_localidades where id_localidad = " + cmbOficina.SelectedValue;
                DataTable dtConsulta = new DataTable();
                dtConsulta.Clear();
                bRespuesta = conexion.GFun_Lo_Busca_Registro(dtConsulta, sSql);

                if (bRespuesta == true)
                {
                    cmbBodega.SelectedValue = dtConsulta.Rows[0].ItemArray[0];
                }
            }
        }



        //Fin de la clase
    }
}
