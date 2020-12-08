using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace F_JMS_QrCode_Print
{
    public partial class QrCodePrint : MaterialForm
    {

        public QrCodePrint(DataTable dt, string path)
        {
            InitializeComponent();
            Dictionary<string, string> dic = new Dictionary<string, string>();
            FastReportHelper.LoadFastReport(panel1, path, dic, dt, "Table");
        }
           
    }
}
