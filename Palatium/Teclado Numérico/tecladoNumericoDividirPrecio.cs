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
    public partial class tecladoNumericoDividirPrecio : Form
    {
        string sSaldo;
        VentanasMensajes.frmMensajeOK ok = new VentanasMensajes.frmMensajeOK();

        public tecladoNumericoDividirPrecio(string sSaldo)
        {
            this.sSaldo = sSaldo; 
            InitializeComponent();

        }

        #region FUNCIONES DEL USUARIO
        //FUNCION PARA CONCATENAR
        private void concatenarValores(string sValor)
        {
            try
            {
                if ((txt_valor.Text == "0") && (sValor == "0"))
                {
                    goto fin;
                }

                else if ((txt_valor.Text == "0") && (sValor != "0"))
                {
                    txt_valor.Clear();
                }

                if (txt_valor.Text.Trim().Contains('.') == true)
                {
                    int longi = txt_valor.Text.Trim().Length;
                    int band = 0, cont = 0;

                    for (int i = 0; i < longi; i++)
                    {
                        if (band == 1)
                            cont++;

                        if (txt_valor.Text.Substring(i, 1) == ".")
                            band = 1;
                    }

                    if (cont < 2)
                    {
                        txt_valor.Text = txt_valor.Text + sValor;
                    }
                }

                else
                {
                    txt_valor.Text = txt_valor.Text + sValor;
                }

                txt_valor.Focus();
                txt_valor.SelectionStart = txt_valor.Text.Trim().Length;
            }

            catch (Exception)
            {
                ok.LblMensaje.Text = "Ocurrió un problema al concatenar los valores.";
                ok.ShowInTaskbar = false;
                ok.ShowDialog();
            }
        fin: { }
        }

        #endregion

        private void button4_Click(object sender, EventArgs e)
        {
            string str;
            int loc;

            if (txt_valor.Text.Length > 0)
            {
                str = txt_valor.Text.Substring(txt_valor.Text.Length - 1);
                loc = txt_valor.Text.Length;
                txt_valor.Text = txt_valor.Text.Remove(loc - 1, 1);
            }

            txt_valor.Focus();
            txt_valor.SelectionStart = txt_valor.Text.Trim().Length;
        }

        private void tecladoNumericoDividirPrecio_Load(object sender, EventArgs e)
        {

        }

        private void btingresar_Click(object sender, EventArgs e)
        {
            DividirPrecio dividirPrecio = new DividirPrecio(Convert.ToInt32(txt_valor.Text),sSaldo);
            dividirPrecio.ShowInTaskbar = false;
            dividirPrecio.ShowDialog();
            this.Close();
        }

        private void btcancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tecladoNumericoDividirPrecio_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void txt_valor_KeyPress(object sender, KeyPressEventArgs e)
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
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            concatenarValores(btn1.Text);
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            concatenarValores(btn2.Text);
        }

        private void btn3_Click(object sender, EventArgs e)
        {
            concatenarValores(btn3.Text);
        }

        private void btn4_Click(object sender, EventArgs e)
        {
            concatenarValores(btn4.Text);
        }

        private void btn5_Click(object sender, EventArgs e)
        {
            concatenarValores(btn5.Text);
        }

        private void btn6_Click(object sender, EventArgs e)
        {
            concatenarValores(btn6.Text);
        }

        private void btn7_Click(object sender, EventArgs e)
        {
            concatenarValores(btn7.Text);
        }

        private void btn8_Click(object sender, EventArgs e)
        {
            concatenarValores(btn8.Text);
        }

        private void btn9_Click(object sender, EventArgs e)
        {
            concatenarValores(btn9.Text);
        }

        private void btn0_Click(object sender, EventArgs e)
        {
            concatenarValores(btn0.Text);
        }
    }
}
