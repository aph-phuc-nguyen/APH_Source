﻿using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace F_TMS_TierMeeting3_Main
{
    public partial class IntroForm : MaterialForm
    {
        public IntroForm()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Form frm = new MainForm();
            frm.Show();
            this.Hide();
        }
    }
}
