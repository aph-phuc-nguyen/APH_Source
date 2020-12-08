﻿namespace ProductionOutput
{
    partial class ProductionOutputForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProductionOutputForm));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.textTime = new System.Windows.Forms.TextBox();
            this.butSelectDept = new System.Windows.Forms.Button();
            this.butQuery = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.textLabel = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelOutQty = new System.Windows.Forms.Label();
            this.labelInQty = new System.Windows.Forms.Label();
            this.labelSeId = new System.Windows.Forms.Label();
            this.labelPo = new System.Windows.Forms.Label();
            this.labelArtName = new System.Windows.Forms.Label();
            this.labelArt = new System.Windows.Forms.Label();
            this.labelSize = new System.Windows.Forms.Label();
            this.labelDeptName = new System.Windows.Forms.Label();
            this.labelDeptNo = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label13 = new System.Windows.Forms.Label();
            this.textSeUnFinishQty = new System.Windows.Forms.TextBox();
            this.textSeFinishQty = new System.Windows.Forms.TextBox();
            this.textDayFinishQty = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.textHourQty = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.labelHour = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnImage = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.SE_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SIZE_NO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ART = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SCAN_DATE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PoPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.SizePanel = new System.Windows.Forms.FlowLayoutPanel();
            this.labelDetail = new System.Windows.Forms.Label();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Interval = 1200;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Location = new System.Drawing.Point(0, 68);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1366, 790);
            this.panel1.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 260F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 450F));
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel3, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.tabControl1, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.PoPanel1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.SizePanel, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.labelDetail, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.13028F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 85F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 49.86972F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1366, 790);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.SteelBlue;
            this.panel2.Controls.Add(this.textTime);
            this.panel2.Controls.Add(this.butSelectDept);
            this.panel2.Controls.Add(this.butQuery);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.textLabel);
            this.panel2.Controls.Add(this.label16);
            this.panel2.Controls.Add(this.label14);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.labelOutQty);
            this.panel2.Controls.Add(this.labelInQty);
            this.panel2.Controls.Add(this.labelSeId);
            this.panel2.Controls.Add(this.labelPo);
            this.panel2.Controls.Add(this.labelArtName);
            this.panel2.Controls.Add(this.labelArt);
            this.panel2.Controls.Add(this.labelSize);
            this.panel2.Controls.Add(this.labelDeptName);
            this.panel2.Controls.Add(this.labelDeptNo);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.tableLayoutPanel1.SetRowSpan(this.panel2, 3);
            this.panel2.Size = new System.Drawing.Size(260, 790);
            this.panel2.TabIndex = 0;
            // 
            // textTime
            // 
            this.textTime.BackColor = System.Drawing.Color.SteelBlue;
            this.textTime.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textTime.Font = new System.Drawing.Font("SimSun", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textTime.Location = new System.Drawing.Point(19, 867);
            this.textTime.Name = "textTime";
            this.textTime.Size = new System.Drawing.Size(227, 24);
            this.textTime.TabIndex = 5;
            this.textTime.Text = "2020/1/1 12:00:00";
            // 
            // butSelectDept
            // 
            this.butSelectDept.BackColor = System.Drawing.Color.MediumAquamarine;
            this.butSelectDept.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.butSelectDept.Location = new System.Drawing.Point(145, 41);
            this.butSelectDept.Name = "butSelectDept";
            this.butSelectDept.Size = new System.Drawing.Size(101, 35);
            this.butSelectDept.TabIndex = 4;
            this.butSelectDept.Text = "选择部门";
            this.butSelectDept.UseVisualStyleBackColor = false;
            this.butSelectDept.Click += new System.EventHandler(this.butSelectDept_Click);
            // 
            // butQuery
            // 
            this.butQuery.BackColor = System.Drawing.Color.MediumAquamarine;
            this.butQuery.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.butQuery.Location = new System.Drawing.Point(14, 743);
            this.butQuery.Name = "butQuery";
            this.butQuery.Size = new System.Drawing.Size(97, 47);
            this.butQuery.TabIndex = 3;
            this.butQuery.Text = "查询";
            this.butQuery.UseVisualStyleBackColor = false;
            this.butQuery.Click += new System.EventHandler(this.butQuery_Click);
            // 
            // button1
            // 
            this.button1.AutoSize = true;
            this.button1.BackColor = System.Drawing.Color.MediumAquamarine;
            this.button1.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button1.Location = new System.Drawing.Point(117, 743);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(123, 47);
            this.button1.TabIndex = 2;
            this.button1.Text = "PO资料刷新";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textLabel
            // 
            this.textLabel.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textLabel.Location = new System.Drawing.Point(14, 187);
            this.textLabel.Name = "textLabel";
            this.textLabel.Size = new System.Drawing.Size(229, 29);
            this.textLabel.TabIndex = 0;
            this.textLabel.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textLabel_KeyPress);
            this.textLabel.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.textLabel_PreviewKeyDown);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft YaHei", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label16.ForeColor = System.Drawing.Color.White;
            this.label16.Location = new System.Drawing.Point(12, 607);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(107, 25);
            this.label16.TabIndex = 0;
            this.label16.Text = "已扫描数量";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft YaHei", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label14.ForeColor = System.Drawing.Color.White;
            this.label14.Location = new System.Drawing.Point(12, 541);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(88, 25);
            this.label14.TabIndex = 0;
            this.label14.Text = "投入数量";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft YaHei", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(12, 411);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(69, 25);
            this.label8.TabIndex = 0;
            this.label8.Text = "订单号";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft YaHei", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(12, 344);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(39, 25);
            this.label7.TabIndex = 0;
            this.label7.Text = "PO";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft YaHei", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(12, 281);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(88, 25);
            this.label6.TabIndex = 0;
            this.label6.Text = "鞋型名称";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft YaHei", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(12, 218);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 25);
            this.label5.TabIndex = 0;
            this.label5.Text = "ART";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft YaHei", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(12, 473);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 25);
            this.label4.TabIndex = 0;
            this.label4.Text = "码数";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft YaHei", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(9, 158);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 25);
            this.label3.TabIndex = 0;
            this.label3.Text = "内盒标签";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft YaHei", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(9, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 25);
            this.label2.TabIndex = 0;
            this.label2.Text = "部门名称";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelOutQty
            // 
            this.labelOutQty.BackColor = System.Drawing.Color.Gainsboro;
            this.labelOutQty.Font = new System.Drawing.Font("Microsoft YaHei", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelOutQty.ForeColor = System.Drawing.Color.Black;
            this.labelOutQty.Location = new System.Drawing.Point(15, 637);
            this.labelOutQty.Name = "labelOutQty";
            this.labelOutQty.Size = new System.Drawing.Size(231, 27);
            this.labelOutQty.TabIndex = 0;
            this.labelOutQty.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelInQty
            // 
            this.labelInQty.BackColor = System.Drawing.Color.Gainsboro;
            this.labelInQty.Font = new System.Drawing.Font("Microsoft YaHei", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelInQty.ForeColor = System.Drawing.Color.Black;
            this.labelInQty.Location = new System.Drawing.Point(15, 571);
            this.labelInQty.Name = "labelInQty";
            this.labelInQty.Size = new System.Drawing.Size(231, 27);
            this.labelInQty.TabIndex = 0;
            this.labelInQty.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelSeId
            // 
            this.labelSeId.BackColor = System.Drawing.Color.Gainsboro;
            this.labelSeId.Font = new System.Drawing.Font("Microsoft YaHei", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelSeId.ForeColor = System.Drawing.Color.Black;
            this.labelSeId.Location = new System.Drawing.Point(15, 441);
            this.labelSeId.Name = "labelSeId";
            this.labelSeId.Size = new System.Drawing.Size(231, 27);
            this.labelSeId.TabIndex = 0;
            this.labelSeId.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelPo
            // 
            this.labelPo.BackColor = System.Drawing.Color.Gainsboro;
            this.labelPo.Font = new System.Drawing.Font("Microsoft YaHei", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelPo.ForeColor = System.Drawing.Color.Black;
            this.labelPo.Location = new System.Drawing.Point(15, 375);
            this.labelPo.Name = "labelPo";
            this.labelPo.Size = new System.Drawing.Size(231, 27);
            this.labelPo.TabIndex = 0;
            this.labelPo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelArtName
            // 
            this.labelArtName.BackColor = System.Drawing.Color.Gainsboro;
            this.labelArtName.Font = new System.Drawing.Font("Microsoft YaHei", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelArtName.ForeColor = System.Drawing.Color.Black;
            this.labelArtName.Location = new System.Drawing.Point(15, 312);
            this.labelArtName.Name = "labelArtName";
            this.labelArtName.Size = new System.Drawing.Size(231, 27);
            this.labelArtName.TabIndex = 0;
            this.labelArtName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelArt
            // 
            this.labelArt.BackColor = System.Drawing.Color.Gainsboro;
            this.labelArt.Font = new System.Drawing.Font("Microsoft YaHei", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelArt.ForeColor = System.Drawing.Color.Black;
            this.labelArt.Location = new System.Drawing.Point(15, 248);
            this.labelArt.Name = "labelArt";
            this.labelArt.Size = new System.Drawing.Size(231, 27);
            this.labelArt.TabIndex = 0;
            this.labelArt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelSize
            // 
            this.labelSize.BackColor = System.Drawing.Color.Gainsboro;
            this.labelSize.Font = new System.Drawing.Font("Microsoft YaHei", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelSize.ForeColor = System.Drawing.Color.Black;
            this.labelSize.Location = new System.Drawing.Point(15, 504);
            this.labelSize.Name = "labelSize";
            this.labelSize.Size = new System.Drawing.Size(231, 27);
            this.labelSize.TabIndex = 0;
            this.labelSize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelDeptName
            // 
            this.labelDeptName.BackColor = System.Drawing.Color.Gainsboro;
            this.labelDeptName.Font = new System.Drawing.Font("Microsoft YaHei", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelDeptName.ForeColor = System.Drawing.Color.Black;
            this.labelDeptName.Location = new System.Drawing.Point(12, 117);
            this.labelDeptName.Name = "labelDeptName";
            this.labelDeptName.Size = new System.Drawing.Size(231, 27);
            this.labelDeptName.TabIndex = 0;
            this.labelDeptName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelDeptNo
            // 
            this.labelDeptNo.BackColor = System.Drawing.Color.Gainsboro;
            this.labelDeptNo.Font = new System.Drawing.Font("Microsoft YaHei", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelDeptNo.ForeColor = System.Drawing.Color.Black;
            this.labelDeptNo.Location = new System.Drawing.Point(12, 44);
            this.labelDeptNo.Name = "labelDeptNo";
            this.labelDeptNo.Size = new System.Drawing.Size(127, 27);
            this.labelDeptNo.TabIndex = 0;
            this.labelDeptNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(9, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "部门编号";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Cornsilk;
            this.panel3.Controls.Add(this.label13);
            this.panel3.Controls.Add(this.textSeUnFinishQty);
            this.panel3.Controls.Add(this.textSeFinishQty);
            this.panel3.Controls.Add(this.textDayFinishQty);
            this.panel3.Controls.Add(this.label12);
            this.panel3.Controls.Add(this.textHourQty);
            this.panel3.Controls.Add(this.label11);
            this.panel3.Controls.Add(this.label10);
            this.panel3.Controls.Add(this.labelHour);
            this.panel3.Controls.Add(this.label9);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(916, 0);
            this.panel3.Margin = new System.Windows.Forms.Padding(0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(450, 353);
            this.panel3.TabIndex = 2;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft YaHei", 25F);
            this.label13.ForeColor = System.Drawing.Color.Black;
            this.label13.Location = new System.Drawing.Point(68, 470);
            this.label13.MaximumSize = new System.Drawing.Size(300, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(41, 45);
            this.label13.TabIndex = 2;
            this.label13.Text = "h";
            // 
            // textSeUnFinishQty
            // 
            this.textSeUnFinishQty.Enabled = false;
            this.textSeUnFinishQty.Font = new System.Drawing.Font("Microsoft YaHei", 39F);
            this.textSeUnFinishQty.ForeColor = System.Drawing.Color.Black;
            this.textSeUnFinishQty.Location = new System.Drawing.Point(294, 263);
            this.textSeUnFinishQty.Margin = new System.Windows.Forms.Padding(0);
            this.textSeUnFinishQty.Name = "textSeUnFinishQty";
            this.textSeUnFinishQty.ReadOnly = true;
            this.textSeUnFinishQty.Size = new System.Drawing.Size(156, 76);
            this.textSeUnFinishQty.TabIndex = 1;
            this.textSeUnFinishQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textSeFinishQty
            // 
            this.textSeFinishQty.Enabled = false;
            this.textSeFinishQty.Font = new System.Drawing.Font("Microsoft YaHei", 39F);
            this.textSeFinishQty.ForeColor = System.Drawing.Color.Black;
            this.textSeFinishQty.Location = new System.Drawing.Point(294, 181);
            this.textSeFinishQty.Margin = new System.Windows.Forms.Padding(0);
            this.textSeFinishQty.Name = "textSeFinishQty";
            this.textSeFinishQty.ReadOnly = true;
            this.textSeFinishQty.Size = new System.Drawing.Size(156, 76);
            this.textSeFinishQty.TabIndex = 1;
            this.textSeFinishQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textDayFinishQty
            // 
            this.textDayFinishQty.Enabled = false;
            this.textDayFinishQty.Font = new System.Drawing.Font("Microsoft YaHei", 39F);
            this.textDayFinishQty.ForeColor = System.Drawing.Color.Black;
            this.textDayFinishQty.Location = new System.Drawing.Point(294, 87);
            this.textDayFinishQty.Margin = new System.Windows.Forms.Padding(0);
            this.textDayFinishQty.Name = "textDayFinishQty";
            this.textDayFinishQty.ReadOnly = true;
            this.textDayFinishQty.Size = new System.Drawing.Size(156, 76);
            this.textDayFinishQty.TabIndex = 1;
            this.textDayFinishQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft YaHei", 25F);
            this.label12.ForeColor = System.Drawing.Color.Black;
            this.label12.Location = new System.Drawing.Point(5, 263);
            this.label12.MaximumSize = new System.Drawing.Size(300, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(288, 45);
            this.label12.TabIndex = 0;
            this.label12.Text = "该制令未扫量(双):";
            // 
            // textHourQty
            // 
            this.textHourQty.Enabled = false;
            this.textHourQty.Font = new System.Drawing.Font("Microsoft YaHei", 39F);
            this.textHourQty.ForeColor = System.Drawing.Color.Black;
            this.textHourQty.Location = new System.Drawing.Point(294, 0);
            this.textHourQty.Margin = new System.Windows.Forms.Padding(0);
            this.textHourQty.Name = "textHourQty";
            this.textHourQty.ReadOnly = true;
            this.textHourQty.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.textHourQty.Size = new System.Drawing.Size(156, 76);
            this.textHourQty.TabIndex = 1;
            this.textHourQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft YaHei", 25F);
            this.label11.ForeColor = System.Drawing.Color.Black;
            this.label11.Location = new System.Drawing.Point(5, 181);
            this.label11.MaximumSize = new System.Drawing.Size(300, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(288, 45);
            this.label11.TabIndex = 0;
            this.label11.Text = "该制令已报量(双):";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft YaHei", 25F);
            this.label10.ForeColor = System.Drawing.Color.Black;
            this.label10.Location = new System.Drawing.Point(5, 87);
            this.label10.MaximumSize = new System.Drawing.Size(300, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(220, 45);
            this.label10.TabIndex = 0;
            this.label10.Text = "当天产量(双):";
            // 
            // labelHour
            // 
            this.labelHour.Font = new System.Drawing.Font("Microsoft YaHei", 25F);
            this.labelHour.ForeColor = System.Drawing.Color.Black;
            this.labelHour.Location = new System.Drawing.Point(13, 470);
            this.labelHour.Name = "labelHour";
            this.labelHour.Size = new System.Drawing.Size(64, 45);
            this.labelHour.TabIndex = 0;
            this.labelHour.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelHour.TextChanged += new System.EventHandler(this.labelHour_TextChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft YaHei", 25F);
            this.label9.ForeColor = System.Drawing.Color.Black;
            this.label9.Location = new System.Drawing.Point(5, 0);
            this.label9.MaximumSize = new System.Drawing.Size(300, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(186, 45);
            this.label9.TabIndex = 0;
            this.label9.Text = "时产量(双):";
            // 
            // tabControl1
            // 
            this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(916, 438);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(450, 352);
            this.tabControl1.TabIndex = 4;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnImage);
            this.tabPage1.Location = new System.Drawing.Point(4, 4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(442, 326);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "输出";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnImage
            // 
            this.btnImage.BackgroundImage = global::ProductionOutput.Properties.Resources.smile;
            this.btnImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnImage.Enabled = false;
            this.btnImage.Location = new System.Drawing.Point(3, 3);
            this.btnImage.Margin = new System.Windows.Forms.Padding(0);
            this.btnImage.Name = "btnImage";
            this.btnImage.Size = new System.Drawing.Size(436, 320);
            this.btnImage.TabIndex = 0;
            this.btnImage.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dataGridView1);
            this.tabPage2.Location = new System.Drawing.Point(4, 4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(442, 431);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "详细信息";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SE_ID,
            this.PO,
            this.SIZE_NO,
            this.ART,
            this.SCAN_DATE});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.GridColor = System.Drawing.Color.Black;
            this.dataGridView1.Location = new System.Drawing.Point(3, 3);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(436, 425);
            this.dataGridView1.TabIndex = 0;
            // 
            // SE_ID
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.SE_ID.DefaultCellStyle = dataGridViewCellStyle1;
            this.SE_ID.HeaderText = "订单号";
            this.SE_ID.Name = "SE_ID";
            this.SE_ID.ReadOnly = true;
            this.SE_ID.Width = 90;
            // 
            // PO
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.PO.DefaultCellStyle = dataGridViewCellStyle2;
            this.PO.HeaderText = "Po";
            this.PO.Name = "PO";
            this.PO.ReadOnly = true;
            this.PO.Width = 85;
            // 
            // SIZE_NO
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.SIZE_NO.DefaultCellStyle = dataGridViewCellStyle3;
            this.SIZE_NO.HeaderText = "Size";
            this.SIZE_NO.Name = "SIZE_NO";
            this.SIZE_NO.ReadOnly = true;
            this.SIZE_NO.Width = 55;
            // 
            // ART
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ART.DefaultCellStyle = dataGridViewCellStyle4;
            this.ART.HeaderText = "Art";
            this.ART.Name = "ART";
            this.ART.ReadOnly = true;
            this.ART.Width = 60;
            // 
            // SCAN_DATE
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.SCAN_DATE.DefaultCellStyle = dataGridViewCellStyle5;
            this.SCAN_DATE.HeaderText = "扫描时间";
            this.SCAN_DATE.Name = "SCAN_DATE";
            this.SCAN_DATE.ReadOnly = true;
            this.SCAN_DATE.Width = 140;
            // 
            // PoPanel1
            // 
            this.PoPanel1.AutoScroll = true;
            this.PoPanel1.BackColor = System.Drawing.Color.Aquamarine;
            this.PoPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PoPanel1.Location = new System.Drawing.Point(260, 0);
            this.PoPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.PoPanel1.Name = "PoPanel1";
            this.PoPanel1.Size = new System.Drawing.Size(656, 353);
            this.PoPanel1.TabIndex = 5;
            // 
            // SizePanel
            // 
            this.SizePanel.AutoScroll = true;
            this.SizePanel.AutoScrollMargin = new System.Drawing.Size(10, 10);
            this.SizePanel.BackColor = System.Drawing.Color.Aquamarine;
            this.SizePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SizePanel.Location = new System.Drawing.Point(260, 438);
            this.SizePanel.Margin = new System.Windows.Forms.Padding(0);
            this.SizePanel.Name = "SizePanel";
            this.SizePanel.Size = new System.Drawing.Size(656, 352);
            this.SizePanel.TabIndex = 6;
            // 
            // labelDetail
            // 
            this.labelDetail.AutoSize = true;
            this.labelDetail.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel1.SetColumnSpan(this.labelDetail, 2);
            this.labelDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDetail.Font = new System.Drawing.Font("SimSun", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelDetail.ForeColor = System.Drawing.Color.White;
            this.labelDetail.Location = new System.Drawing.Point(260, 353);
            this.labelDetail.Margin = new System.Windows.Forms.Padding(0);
            this.labelDetail.Name = "labelDetail";
            this.labelDetail.Size = new System.Drawing.Size(1106, 85);
            this.labelDetail.TabIndex = 7;
            this.labelDetail.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // timer2
            // 
            this.timer2.Enabled = true;
            this.timer2.Interval = 1000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // ProductionOutputForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1366, 768);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ProductionOutputForm";
            this.Text = "ProductionOutputForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ProductionOutputForm_FormClosing);
            this.Load += new System.EventHandler(this.ProductionOutputForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button butQuery;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textLabel;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelOutQty;
        private System.Windows.Forms.Label labelInQty;
        private System.Windows.Forms.Label labelSeId;
        private System.Windows.Forms.Label labelPo;
        private System.Windows.Forms.Label labelArtName;
        private System.Windows.Forms.Label labelArt;
        private System.Windows.Forms.Label labelSize;
        private System.Windows.Forms.Label labelDeptName;
        private System.Windows.Forms.Label labelDeptNo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox textSeUnFinishQty;
        private System.Windows.Forms.TextBox textSeFinishQty;
        private System.Windows.Forms.TextBox textDayFinishQty;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textHourQty;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label labelHour;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button btnImage;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn SE_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn PO;
        private System.Windows.Forms.DataGridViewTextBoxColumn SIZE_NO;
        private System.Windows.Forms.DataGridViewTextBoxColumn ART;
        private System.Windows.Forms.DataGridViewTextBoxColumn SCAN_DATE;
        private System.Windows.Forms.FlowLayoutPanel PoPanel1;
        private System.Windows.Forms.FlowLayoutPanel SizePanel;
        private System.Windows.Forms.Label labelDetail;
        private System.Windows.Forms.Button butSelectDept;
        private System.Windows.Forms.TextBox textTime;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Label label13;
    }
}