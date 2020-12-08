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

namespace ProductionTargets
{
    public partial class MessageForm : MaterialForm
    {
        public MessageForm()
        {
            InitializeComponent();
        }



        public MessageForm(DataTable dataTable)
        {
            InitializeComponent();
            dataGridView1.DataSource = dataTable.DefaultView;
            dataGridView1.Update();
        }

    }
}
