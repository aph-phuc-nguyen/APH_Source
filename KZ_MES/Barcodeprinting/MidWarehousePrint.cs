using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Barcodeprinting
{
    public partial class MidWarehousePrint : Form
    {

        public MidWarehousePrint(DataTable dt, string path)
        {
            InitializeComponent();
            Dictionary<string, string> dic = new Dictionary<string, string>();
            FastReportHelper.LoadFastReport(panel1, path, dic, dt, "Table");
        }
           
    }
}
