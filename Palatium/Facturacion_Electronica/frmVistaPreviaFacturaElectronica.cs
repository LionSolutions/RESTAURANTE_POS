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
    public partial class frmVistaPreviaFacturaElectronica : Form
    {
        public frmVistaPreviaFacturaElectronica()
        {
            InitializeComponent();
        }

        private void frmVistaPreviaFacturaElectronica_Load(object sender, EventArgs e)
        {

            this.rptVisor.RefreshReport();
        }
    }
}
