namespace F_EAP_MachineLink_BalanceKanBan
{
    partial class F_EAP_Oven_RealParam
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title2 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(F_EAP_Oven_RealParam));
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.textOvenDayOut = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textOvenKwh = new System.Windows.Forms.TextBox();
            this.textOvenStatus = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textOvenSpeed = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.textOvenLAct1 = new System.Windows.Forms.TextBox();
            this.textOvenUSet3 = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.textOvenUSet1 = new System.Windows.Forms.TextBox();
            this.textOvenUSet2 = new System.Windows.Forms.TextBox();
            this.textOvenLAct2 = new System.Windows.Forms.TextBox();
            this.textOvenLSet1 = new System.Windows.Forms.TextBox();
            this.textOvenLSet2 = new System.Windows.Forms.TextBox();
            this.textOvenLSet3 = new System.Windows.Forms.TextBox();
            this.textOvenUAct1 = new System.Windows.Forms.TextBox();
            this.textOvenUAct2 = new System.Windows.Forms.TextBox();
            this.textOvenUAct3 = new System.Windows.Forms.TextBox();
            this.textOvenLAct3 = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.textOvenID = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.chart2 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.tableLayoutPanel8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart2)).BeginInit();
            this.SuspendLayout();
            // 
            // chart1
            // 
            chartArea1.AxisX.ArrowStyle = System.Windows.Forms.DataVisualization.Charting.AxisArrowStyle.Lines;
            chartArea1.AxisX.MajorGrid.Enabled = false;
            chartArea1.AxisX.MinorTickMark.TickMarkStyle = System.Windows.Forms.DataVisualization.Charting.TickMarkStyle.InsideArea;
            chartArea1.AxisX.Title = "时间段";
            chartArea1.AxisY.ArrowStyle = System.Windows.Forms.DataVisualization.Charting.AxisArrowStyle.Lines;
            chartArea1.AxisY.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea1.AxisY.Title = "产量(双)";
            chartArea1.CursorX.AutoScroll = false;
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            this.tableLayoutPanel1.SetColumnSpan(this.chart1, 2);
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            legend2.Name = "Legend1";
            this.chart1.Legends.Add(legend2);
            this.chart1.Location = new System.Drawing.Point(1, 355);
            this.chart1.Margin = new System.Windows.Forms.Padding(0);
            this.chart1.Name = "chart1";
            this.chart1.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Chocolate;
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chart1.Series.Add(series2);
            this.chart1.Size = new System.Drawing.Size(596, 437);
            this.chart1.TabIndex = 13;
            this.chart1.Text = "chart1";
            title2.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            title2.ForeColor = System.Drawing.Color.MediumBlue;
            title2.Name = "Title1";
            title2.Text = "当日各时段产量";
            this.chart1.Titles.Add(title2);
            // 
            // textOvenDayOut
            // 
            this.textOvenDayOut.BackColor = System.Drawing.Color.Gainsboro;
            this.textOvenDayOut.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textOvenDayOut.ForeColor = System.Drawing.SystemColors.WindowText;
            this.textOvenDayOut.Location = new System.Drawing.Point(153, 228);
            this.textOvenDayOut.Name = "textOvenDayOut";
            this.textOvenDayOut.ReadOnly = true;
            this.textOvenDayOut.Size = new System.Drawing.Size(164, 29);
            this.textOvenDayOut.TabIndex = 12;
            this.textOvenDayOut.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(43, 229);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(107, 25);
            this.label6.TabIndex = 11;
            this.label6.Text = "当日产量：";
            // 
            // textOvenKwh
            // 
            this.textOvenKwh.BackColor = System.Drawing.Color.Gainsboro;
            this.textOvenKwh.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textOvenKwh.ForeColor = System.Drawing.SystemColors.WindowText;
            this.textOvenKwh.Location = new System.Drawing.Point(153, 124);
            this.textOvenKwh.Name = "textOvenKwh";
            this.textOvenKwh.ReadOnly = true;
            this.textOvenKwh.Size = new System.Drawing.Size(164, 29);
            this.textOvenKwh.TabIndex = 12;
            this.textOvenKwh.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textOvenStatus
            // 
            this.textOvenStatus.BackColor = System.Drawing.Color.Gainsboro;
            this.textOvenStatus.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textOvenStatus.ForeColor = System.Drawing.SystemColors.WindowText;
            this.textOvenStatus.Location = new System.Drawing.Point(153, 175);
            this.textOvenStatus.Name = "textOvenStatus";
            this.textOvenStatus.ReadOnly = true;
            this.textOvenStatus.Size = new System.Drawing.Size(164, 29);
            this.textOvenStatus.TabIndex = 12;
            this.textOvenStatus.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(83, 124);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 25);
            this.label4.TabIndex = 11;
            this.label4.Text = "功率：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(83, 175);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 25);
            this.label5.TabIndex = 11;
            this.label5.Text = "状态：";
            // 
            // textOvenSpeed
            // 
            this.textOvenSpeed.BackColor = System.Drawing.Color.Gainsboro;
            this.textOvenSpeed.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textOvenSpeed.ForeColor = System.Drawing.SystemColors.WindowText;
            this.textOvenSpeed.Location = new System.Drawing.Point(153, 75);
            this.textOvenSpeed.Name = "textOvenSpeed";
            this.textOvenSpeed.ReadOnly = true;
            this.textOvenSpeed.Size = new System.Drawing.Size(164, 29);
            this.textOvenSpeed.TabIndex = 1;
            this.textOvenSpeed.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(83, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 25);
            this.label3.TabIndex = 11;
            this.label3.Text = "速度：";
            // 
            // textBox2
            // 
            this.textBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox2.BackColor = System.Drawing.SystemColors.Control;
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox2.ForeColor = System.Drawing.Color.DarkOrange;
            this.textBox2.Location = new System.Drawing.Point(183, 254);
            this.textBox2.Margin = new System.Windows.Forms.Padding(0);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(26, 23);
            this.textBox2.TabIndex = 10;
            this.textBox2.Text = "●";
            this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(211, 256);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 16);
            this.label2.TabIndex = 9;
            this.label2.Text = "实际温度";
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox1.BackColor = System.Drawing.SystemColors.Control;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox1.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.textBox1.Location = new System.Drawing.Point(49, 253);
            this.textBox1.Margin = new System.Windows.Forms.Padding(0);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(26, 23);
            this.textBox1.TabIndex = 8;
            this.textBox1.Text = "●";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(77, 255);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 16);
            this.label1.TabIndex = 7;
            this.label1.Text = "设定温度";
            // 
            // tableLayoutPanel8
            // 
            this.tableLayoutPanel8.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel8.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel8.ColumnCount = 7;
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 6F));
            this.tableLayoutPanel8.Controls.Add(this.textOvenLAct1, 0, 4);
            this.tableLayoutPanel8.Controls.Add(this.textOvenUSet3, 4, 0);
            this.tableLayoutPanel8.Controls.Add(this.pictureBox1, 0, 2);
            this.tableLayoutPanel8.Controls.Add(this.textOvenUSet1, 0, 0);
            this.tableLayoutPanel8.Controls.Add(this.textOvenUSet2, 2, 0);
            this.tableLayoutPanel8.Controls.Add(this.textOvenLAct2, 1, 4);
            this.tableLayoutPanel8.Controls.Add(this.textOvenLSet1, 0, 3);
            this.tableLayoutPanel8.Controls.Add(this.textOvenLSet2, 2, 3);
            this.tableLayoutPanel8.Controls.Add(this.textOvenLSet3, 4, 3);
            this.tableLayoutPanel8.Controls.Add(this.textOvenUAct1, 0, 1);
            this.tableLayoutPanel8.Controls.Add(this.textOvenUAct2, 2, 1);
            this.tableLayoutPanel8.Controls.Add(this.textOvenUAct3, 4, 1);
            this.tableLayoutPanel8.Controls.Add(this.textOvenLAct3, 4, 4);
            this.tableLayoutPanel8.Location = new System.Drawing.Point(38, 21);
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.RowCount = 5;
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel8.Size = new System.Drawing.Size(772, 214);
            this.tableLayoutPanel8.TabIndex = 6;
            // 
            // textOvenLAct1
            // 
            this.textOvenLAct1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.textOvenLAct1.BackColor = System.Drawing.Color.DarkOrange;
            this.textOvenLAct1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tableLayoutPanel8.SetColumnSpan(this.textOvenLAct1, 2);
            this.textOvenLAct1.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textOvenLAct1.ForeColor = System.Drawing.Color.Black;
            this.textOvenLAct1.Location = new System.Drawing.Point(105, 189);
            this.textOvenLAct1.Margin = new System.Windows.Forms.Padding(0);
            this.textOvenLAct1.Name = "textOvenLAct1";
            this.textOvenLAct1.Size = new System.Drawing.Size(66, 22);
            this.textOvenLAct1.TabIndex = 20;
            this.textOvenLAct1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textOvenUSet3
            // 
            this.textOvenUSet3.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.textOvenUSet3.BackColor = System.Drawing.SystemColors.ControlDark;
            this.textOvenUSet3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tableLayoutPanel8.SetColumnSpan(this.textOvenUSet3, 2);
            this.textOvenUSet3.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textOvenUSet3.ForeColor = System.Drawing.Color.Black;
            this.textOvenUSet3.Location = new System.Drawing.Point(520, 2);
            this.textOvenUSet3.Margin = new System.Windows.Forms.Padding(0);
            this.textOvenUSet3.Name = "textOvenUSet3";
            this.textOvenUSet3.Size = new System.Drawing.Size(65, 23);
            this.textOvenUSet3.TabIndex = 12;
            this.textOvenUSet3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel8.SetColumnSpan(this.pictureBox1, 7);
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.ErrorImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.ErrorImage")));
            this.pictureBox1.Image = global::F_EAP_MachineLink_BalanceKanBan.Properties.Resources.oven;
            this.pictureBox1.Location = new System.Drawing.Point(0, 50);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(772, 114);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // textOvenUSet1
            // 
            this.textOvenUSet1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.textOvenUSet1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.textOvenUSet1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tableLayoutPanel8.SetColumnSpan(this.textOvenUSet1, 2);
            this.textOvenUSet1.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textOvenUSet1.ForeColor = System.Drawing.Color.Black;
            this.textOvenUSet1.Location = new System.Drawing.Point(104, 2);
            this.textOvenUSet1.Margin = new System.Windows.Forms.Padding(0);
            this.textOvenUSet1.Name = "textOvenUSet1";
            this.textOvenUSet1.Size = new System.Drawing.Size(67, 23);
            this.textOvenUSet1.TabIndex = 5;
            this.textOvenUSet1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textOvenUSet2
            // 
            this.textOvenUSet2.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.textOvenUSet2.BackColor = System.Drawing.SystemColors.ControlDark;
            this.textOvenUSet2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tableLayoutPanel8.SetColumnSpan(this.textOvenUSet2, 2);
            this.textOvenUSet2.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textOvenUSet2.ForeColor = System.Drawing.Color.Black;
            this.textOvenUSet2.Location = new System.Drawing.Point(298, 2);
            this.textOvenUSet2.Margin = new System.Windows.Forms.Padding(0);
            this.textOvenUSet2.Name = "textOvenUSet2";
            this.textOvenUSet2.Size = new System.Drawing.Size(64, 23);
            this.textOvenUSet2.TabIndex = 7;
            this.textOvenUSet2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textOvenLAct2
            // 
            this.textOvenLAct2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.textOvenLAct2.BackColor = System.Drawing.Color.DarkOrange;
            this.textOvenLAct2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tableLayoutPanel8.SetColumnSpan(this.textOvenLAct2, 2);
            this.textOvenLAct2.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textOvenLAct2.ForeColor = System.Drawing.Color.Black;
            this.textOvenLAct2.Location = new System.Drawing.Point(299, 189);
            this.textOvenLAct2.Margin = new System.Windows.Forms.Padding(0);
            this.textOvenLAct2.Name = "textOvenLAct2";
            this.textOvenLAct2.Size = new System.Drawing.Size(62, 23);
            this.textOvenLAct2.TabIndex = 6;
            this.textOvenLAct2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textOvenLSet1
            // 
            this.textOvenLSet1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.textOvenLSet1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.textOvenLSet1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tableLayoutPanel8.SetColumnSpan(this.textOvenLSet1, 2);
            this.textOvenLSet1.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textOvenLSet1.ForeColor = System.Drawing.Color.Black;
            this.textOvenLSet1.Location = new System.Drawing.Point(105, 164);
            this.textOvenLSet1.Margin = new System.Windows.Forms.Padding(0);
            this.textOvenLSet1.Name = "textOvenLSet1";
            this.textOvenLSet1.Size = new System.Drawing.Size(66, 23);
            this.textOvenLSet1.TabIndex = 17;
            this.textOvenLSet1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textOvenLSet2
            // 
            this.textOvenLSet2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.textOvenLSet2.BackColor = System.Drawing.SystemColors.ControlDark;
            this.textOvenLSet2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tableLayoutPanel8.SetColumnSpan(this.textOvenLSet2, 2);
            this.textOvenLSet2.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textOvenLSet2.ForeColor = System.Drawing.Color.Black;
            this.textOvenLSet2.Location = new System.Drawing.Point(299, 164);
            this.textOvenLSet2.Margin = new System.Windows.Forms.Padding(0);
            this.textOvenLSet2.Name = "textOvenLSet2";
            this.textOvenLSet2.Size = new System.Drawing.Size(62, 23);
            this.textOvenLSet2.TabIndex = 18;
            this.textOvenLSet2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textOvenLSet3
            // 
            this.textOvenLSet3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.textOvenLSet3.BackColor = System.Drawing.SystemColors.ControlDark;
            this.textOvenLSet3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tableLayoutPanel8.SetColumnSpan(this.textOvenLSet3, 2);
            this.textOvenLSet3.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textOvenLSet3.ForeColor = System.Drawing.Color.Black;
            this.textOvenLSet3.Location = new System.Drawing.Point(518, 164);
            this.textOvenLSet3.Margin = new System.Windows.Forms.Padding(0);
            this.textOvenLSet3.Name = "textOvenLSet3";
            this.textOvenLSet3.Size = new System.Drawing.Size(70, 23);
            this.textOvenLSet3.TabIndex = 19;
            this.textOvenLSet3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textOvenUAct1
            // 
            this.textOvenUAct1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.textOvenUAct1.BackColor = System.Drawing.Color.DarkOrange;
            this.textOvenUAct1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tableLayoutPanel8.SetColumnSpan(this.textOvenUAct1, 2);
            this.textOvenUAct1.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textOvenUAct1.ForeColor = System.Drawing.Color.Black;
            this.textOvenUAct1.Location = new System.Drawing.Point(104, 28);
            this.textOvenUAct1.Margin = new System.Windows.Forms.Padding(0);
            this.textOvenUAct1.Name = "textOvenUAct1";
            this.textOvenUAct1.Size = new System.Drawing.Size(68, 22);
            this.textOvenUAct1.TabIndex = 16;
            this.textOvenUAct1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textOvenUAct2
            // 
            this.textOvenUAct2.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.textOvenUAct2.BackColor = System.Drawing.Color.DarkOrange;
            this.textOvenUAct2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tableLayoutPanel8.SetColumnSpan(this.textOvenUAct2, 2);
            this.textOvenUAct2.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textOvenUAct2.ForeColor = System.Drawing.Color.Black;
            this.textOvenUAct2.Location = new System.Drawing.Point(299, 27);
            this.textOvenUAct2.Margin = new System.Windows.Forms.Padding(0);
            this.textOvenUAct2.Name = "textOvenUAct2";
            this.textOvenUAct2.Size = new System.Drawing.Size(62, 23);
            this.textOvenUAct2.TabIndex = 11;
            this.textOvenUAct2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textOvenUAct3
            // 
            this.textOvenUAct3.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.textOvenUAct3.BackColor = System.Drawing.Color.DarkOrange;
            this.textOvenUAct3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tableLayoutPanel8.SetColumnSpan(this.textOvenUAct3, 2);
            this.textOvenUAct3.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textOvenUAct3.ForeColor = System.Drawing.Color.Black;
            this.textOvenUAct3.Location = new System.Drawing.Point(521, 27);
            this.textOvenUAct3.Margin = new System.Windows.Forms.Padding(0);
            this.textOvenUAct3.Name = "textOvenUAct3";
            this.textOvenUAct3.Size = new System.Drawing.Size(64, 23);
            this.textOvenUAct3.TabIndex = 13;
            this.textOvenUAct3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textOvenLAct3
            // 
            this.textOvenLAct3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.textOvenLAct3.BackColor = System.Drawing.Color.DarkOrange;
            this.textOvenLAct3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tableLayoutPanel8.SetColumnSpan(this.textOvenLAct3, 2);
            this.textOvenLAct3.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textOvenLAct3.ForeColor = System.Drawing.Color.Black;
            this.textOvenLAct3.Location = new System.Drawing.Point(518, 189);
            this.textOvenLAct3.Margin = new System.Windows.Forms.Padding(0);
            this.textOvenLAct3.Name = "textOvenLAct3";
            this.textOvenLAct3.Size = new System.Drawing.Size(69, 23);
            this.textOvenLAct3.TabIndex = 15;
            this.textOvenLAct3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 20000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Location = new System.Drawing.Point(0, 62);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1329, 820);
            this.panel1.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55F));
            this.tableLayoutPanel1.Controls.Add(this.label13, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label12, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.label11, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label8, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.chart1, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.panel3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.chart2, 2, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1329, 820);
            this.tableLayoutPanel1.TabIndex = 15;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.Color.Gray;
            this.tableLayoutPanel1.SetColumnSpan(this.label13, 3);
            this.label13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label13.Location = new System.Drawing.Point(1, 793);
            this.label13.Margin = new System.Windows.Forms.Padding(0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(1327, 26);
            this.label13.TabIndex = 20;
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.LightGray;
            this.label12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label12.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label12.Location = new System.Drawing.Point(598, 324);
            this.label12.Margin = new System.Windows.Forms.Padding(0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(730, 30);
            this.label12.TabIndex = 19;
            this.label12.Text = "  ◆温度曲线";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.LightGray;
            this.tableLayoutPanel1.SetColumnSpan(this.label11, 2);
            this.label11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label11.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label11.Location = new System.Drawing.Point(1, 324);
            this.label11.Margin = new System.Windows.Forms.Padding(0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(596, 30);
            this.label11.TabIndex = 18;
            this.label11.Text = "  ◆产量统计";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.LightGray;
            this.tableLayoutPanel1.SetColumnSpan(this.label8, 3);
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(1, 1);
            this.label8.Margin = new System.Windows.Forms.Padding(0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(1327, 30);
            this.label8.TabIndex = 17;
            this.label8.Text = "  ◆设备实时信息";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.textOvenID);
            this.panel3.Controls.Add(this.label14);
            this.panel3.Controls.Add(this.textOvenSpeed);
            this.panel3.Controls.Add(this.label9);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.textOvenStatus);
            this.panel3.Controls.Add(this.textOvenDayOut);
            this.panel3.Controls.Add(this.textOvenKwh);
            this.panel3.Controls.Add(this.label7);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.label10);
            this.panel3.Controls.Add(this.label6);
            this.panel3.Controls.Add(this.label5);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(1, 32);
            this.panel3.Margin = new System.Windows.Forms.Padding(0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(397, 291);
            this.panel3.TabIndex = 16;
            // 
            // textOvenID
            // 
            this.textOvenID.BackColor = System.Drawing.Color.Gainsboro;
            this.textOvenID.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textOvenID.ForeColor = System.Drawing.SystemColors.WindowText;
            this.textOvenID.Location = new System.Drawing.Point(153, 27);
            this.textOvenID.Name = "textOvenID";
            this.textOvenID.ReadOnly = true;
            this.textOvenID.Size = new System.Drawing.Size(164, 29);
            this.textOvenID.TabIndex = 26;
            this.textOvenID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label14.Location = new System.Drawing.Point(63, 30);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(89, 25);
            this.label14.TabIndex = 27;
            this.label14.Text = "设备ID：";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(323, 127);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(43, 25);
            this.label9.TabIndex = 11;
            this.label9.Text = "KW";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(323, 78);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 25);
            this.label7.TabIndex = 11;
            this.label7.Text = "mm/s";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.Location = new System.Drawing.Point(323, 231);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(31, 25);
            this.label10.TabIndex = 11;
            this.label10.Text = "双";
            // 
            // panel2
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.panel2, 2);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.textBox2);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.textBox1);
            this.panel2.Controls.Add(this.tableLayoutPanel8);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(399, 32);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(929, 291);
            this.panel2.TabIndex = 15;
            // 
            // chart2
            // 
            chartArea2.AxisX.ArrowStyle = System.Windows.Forms.DataVisualization.Charting.AxisArrowStyle.Lines;
            chartArea2.AxisX.MajorGrid.Enabled = false;
            chartArea2.AxisX.Title = "时间";
            chartArea2.AxisY.ArrowStyle = System.Windows.Forms.DataVisualization.Charting.AxisArrowStyle.Lines;
            chartArea2.AxisY.MajorGrid.Enabled = false;
            chartArea2.AxisY.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea2.AxisY.Title = "温度(℃)";
            chartArea2.Name = "ChartArea1";
            this.chart2.ChartAreas.Add(chartArea2);
            this.chart2.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.chart2.Legends.Add(legend1);
            this.chart2.Location = new System.Drawing.Point(598, 355);
            this.chart2.Margin = new System.Windows.Forms.Padding(0);
            this.chart2.Name = "chart2";
            this.chart2.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Bright;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart2.Series.Add(series1);
            this.chart2.Size = new System.Drawing.Size(730, 437);
            this.chart2.TabIndex = 21;
            this.chart2.Text = "chart2";
            title1.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            title1.ForeColor = System.Drawing.Color.MediumBlue;
            title1.Name = "Title1";
            title1.Text = "最近1小时温度曲线";
            this.chart2.Titles.Add(title1);
            // 
            // F_EAP_Oven_RealParam
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1329, 883);
            this.Controls.Add(this.panel1);
            this.Name = "F_EAP_Oven_RealParam";
            this.Text = "烘箱概况";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.F_EAP_Oven_RealParam_FormClosing);
            this.Load += new System.EventHandler(this.F_EAP_Oven_RealParam_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.tableLayoutPanel8.ResumeLayout(false);
            this.tableLayoutPanel8.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
        private System.Windows.Forms.TextBox textOvenLAct2;
        private System.Windows.Forms.TextBox textOvenUSet1;
        private System.Windows.Forms.TextBox textOvenUSet2;
        private System.Windows.Forms.TextBox textOvenUAct3;
        private System.Windows.Forms.TextBox textOvenUSet3;
        private System.Windows.Forms.TextBox textOvenUAct2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textOvenKwh;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textOvenSpeed;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textOvenDayOut;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textOvenStatus;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TextBox textOvenLAct3;
        private System.Windows.Forms.TextBox textOvenUAct1;
        private System.Windows.Forms.TextBox textOvenLSet1;
        private System.Windows.Forms.TextBox textOvenLSet2;
        private System.Windows.Forms.TextBox textOvenLSet3;
        private System.Windows.Forms.TextBox textOvenLAct1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart2;
        private System.Windows.Forms.TextBox textOvenID;
        private System.Windows.Forms.Label label14;
    }
}