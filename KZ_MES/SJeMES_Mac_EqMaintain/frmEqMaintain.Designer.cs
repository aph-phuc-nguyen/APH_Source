namespace SJeMES_Mac_EqMaintain
{
    partial class frmEqMaintain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEqMaintain));
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.dgv_OldData = new System.Windows.Forms.DataGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgv_QueryPaln = new System.Windows.Forms.DataGridView();
            this.panel4 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel7 = new System.Windows.Forms.Panel();
            this.btn_Save = new System.Windows.Forms.Button();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_Query = new System.Windows.Forms.Button();
            this.cbo_QueryName = new System.Windows.Forms.ComboBox();
            this.txt_EqNo = new System.Windows.Forms.TextBox();
            this.txt_State = new System.Windows.Forms.TextBox();
            this.txt_CopyBook = new System.Windows.Forms.TextBox();
            this.txt_EqName = new System.Windows.Forms.TextBox();
            this.txt_People = new System.Windows.Forms.TextBox();
            this.txt_QueryVal = new System.Windows.Forms.TextBox();
            this.txt_EqCode = new System.Windows.Forms.TextBox();
            this.lbl_People = new System.Windows.Forms.Label();
            this.lbl_State = new System.Windows.Forms.Label();
            this.lbl_EqName = new System.Windows.Forms.Label();
            this.lbl_EqNo = new System.Windows.Forms.Label();
            this.lbl_EqCode = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dgv_MaintainData = new System.Windows.Forms.DataGridView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dgv_HistoryData = new System.Windows.Forms.DataGridView();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_OldData)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_QueryPaln)).BeginInit();
            this.panel4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel7.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_MaintainData)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_HistoryData)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(316, 604);
            this.panel1.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.dgv_OldData);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panel3.Location = new System.Drawing.Point(0, 433);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(316, 171);
            this.panel3.TabIndex = 0;
            // 
            // dgv_OldData
            // 
            this.dgv_OldData.AllowUserToAddRows = false;
            this.dgv_OldData.AllowUserToDeleteRows = false;
            this.dgv_OldData.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv_OldData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_OldData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_OldData.Location = new System.Drawing.Point(0, 0);
            this.dgv_OldData.Name = "dgv_OldData";
            this.dgv_OldData.ReadOnly = true;
            this.dgv_OldData.RowHeadersVisible = false;
            this.dgv_OldData.RowTemplate.Height = 23;
            this.dgv_OldData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_OldData.Size = new System.Drawing.Size(316, 171);
            this.dgv_OldData.TabIndex = 0;
            this.dgv_OldData.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_OldData_CellClick);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgv_QueryPaln);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(316, 433);
            this.panel2.TabIndex = 1;
            // 
            // dgv_QueryPaln
            // 
            this.dgv_QueryPaln.AllowUserToAddRows = false;
            this.dgv_QueryPaln.AllowUserToDeleteRows = false;
            this.dgv_QueryPaln.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv_QueryPaln.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_QueryPaln.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_QueryPaln.Location = new System.Drawing.Point(0, 0);
            this.dgv_QueryPaln.Name = "dgv_QueryPaln";
            this.dgv_QueryPaln.ReadOnly = true;
            this.dgv_QueryPaln.RowHeadersVisible = false;
            this.dgv_QueryPaln.RowTemplate.Height = 23;
            this.dgv_QueryPaln.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_QueryPaln.Size = new System.Drawing.Size(316, 433);
            this.dgv_QueryPaln.TabIndex = 0;
            this.dgv_QueryPaln.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_QueryPaln_CellClick);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.groupBox1);
            this.panel4.Controls.Add(this.panel7);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel4.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panel4.Location = new System.Drawing.Point(1004, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(254, 604);
            this.panel4.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.pictureBox1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(254, 520);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "设备图：";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(3, 22);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(248, 495);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.btn_Save);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel7.Location = new System.Drawing.Point(0, 520);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(254, 84);
            this.panel7.TabIndex = 14;
            // 
            // btn_Save
            // 
            this.btn_Save.Location = new System.Drawing.Point(86, 33);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(81, 28);
            this.btn_Save.TabIndex = 13;
            this.btn_Save.Text = "保存";
            this.btn_Save.UseVisualStyleBackColor = true;
            this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.label1);
            this.panel5.Controls.Add(this.btn_Query);
            this.panel5.Controls.Add(this.cbo_QueryName);
            this.panel5.Controls.Add(this.txt_EqNo);
            this.panel5.Controls.Add(this.txt_State);
            this.panel5.Controls.Add(this.txt_CopyBook);
            this.panel5.Controls.Add(this.txt_EqName);
            this.panel5.Controls.Add(this.txt_People);
            this.panel5.Controls.Add(this.txt_QueryVal);
            this.panel5.Controls.Add(this.txt_EqCode);
            this.panel5.Controls.Add(this.lbl_People);
            this.panel5.Controls.Add(this.lbl_State);
            this.panel5.Controls.Add(this.lbl_EqName);
            this.panel5.Controls.Add(this.lbl_EqNo);
            this.panel5.Controls.Add(this.lbl_EqCode);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panel5.Location = new System.Drawing.Point(316, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(688, 201);
            this.panel5.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(308, 111);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 16);
            this.label1.TabIndex = 13;
            this.label1.Text = "参考手册：";
            // 
            // btn_Query
            // 
            this.btn_Query.Location = new System.Drawing.Point(593, 17);
            this.btn_Query.Name = "btn_Query";
            this.btn_Query.Size = new System.Drawing.Size(75, 27);
            this.btn_Query.TabIndex = 12;
            this.btn_Query.Text = "查询";
            this.btn_Query.UseVisualStyleBackColor = true;
            this.btn_Query.Click += new System.EventHandler(this.btn_Query_Click);
            // 
            // cbo_QueryName
            // 
            this.cbo_QueryName.FormattingEnabled = true;
            this.cbo_QueryName.Items.AddRange(new object[] {
            "设备编号",
            "设备名称"});
            this.cbo_QueryName.Location = new System.Drawing.Point(311, 19);
            this.cbo_QueryName.Name = "cbo_QueryName";
            this.cbo_QueryName.Size = new System.Drawing.Size(115, 24);
            this.cbo_QueryName.TabIndex = 11;
            // 
            // txt_EqNo
            // 
            this.txt_EqNo.BackColor = System.Drawing.SystemColors.Info;
            this.txt_EqNo.Location = new System.Drawing.Point(104, 61);
            this.txt_EqNo.Name = "txt_EqNo";
            this.txt_EqNo.Size = new System.Drawing.Size(155, 26);
            this.txt_EqNo.TabIndex = 10;
            this.txt_EqNo.DoubleClick += new System.EventHandler(this.txt_EqNo_DoubleClick);
            // 
            // txt_State
            // 
            this.txt_State.Location = new System.Drawing.Point(104, 108);
            this.txt_State.Name = "txt_State";
            this.txt_State.Size = new System.Drawing.Size(155, 26);
            this.txt_State.TabIndex = 9;
            // 
            // txt_CopyBook
            // 
            this.txt_CopyBook.BackColor = System.Drawing.SystemColors.Info;
            this.txt_CopyBook.Location = new System.Drawing.Point(396, 108);
            this.txt_CopyBook.Name = "txt_CopyBook";
            this.txt_CopyBook.Size = new System.Drawing.Size(155, 26);
            this.txt_CopyBook.TabIndex = 8;
            this.txt_CopyBook.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.txt_CopyBook_MouseDoubleClick);
            // 
            // txt_EqName
            // 
            this.txt_EqName.Location = new System.Drawing.Point(396, 61);
            this.txt_EqName.Name = "txt_EqName";
            this.txt_EqName.Size = new System.Drawing.Size(155, 26);
            this.txt_EqName.TabIndex = 8;
            // 
            // txt_People
            // 
            this.txt_People.Location = new System.Drawing.Point(104, 153);
            this.txt_People.Name = "txt_People";
            this.txt_People.Size = new System.Drawing.Size(155, 26);
            this.txt_People.TabIndex = 7;
            // 
            // txt_QueryVal
            // 
            this.txt_QueryVal.Location = new System.Drawing.Point(432, 18);
            this.txt_QueryVal.Name = "txt_QueryVal";
            this.txt_QueryVal.Size = new System.Drawing.Size(155, 26);
            this.txt_QueryVal.TabIndex = 6;
            // 
            // txt_EqCode
            // 
            this.txt_EqCode.Location = new System.Drawing.Point(104, 15);
            this.txt_EqCode.Name = "txt_EqCode";
            this.txt_EqCode.Size = new System.Drawing.Size(155, 26);
            this.txt_EqCode.TabIndex = 5;
            this.txt_EqCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_EqCode_KeyDown);
            // 
            // lbl_People
            // 
            this.lbl_People.AutoSize = true;
            this.lbl_People.Location = new System.Drawing.Point(22, 156);
            this.lbl_People.Name = "lbl_People";
            this.lbl_People.Size = new System.Drawing.Size(88, 16);
            this.lbl_People.TabIndex = 4;
            this.lbl_People.Text = "负责人员：";
            // 
            // lbl_State
            // 
            this.lbl_State.AutoSize = true;
            this.lbl_State.Location = new System.Drawing.Point(22, 111);
            this.lbl_State.Name = "lbl_State";
            this.lbl_State.Size = new System.Drawing.Size(88, 16);
            this.lbl_State.TabIndex = 3;
            this.lbl_State.Text = "机械状态：";
            // 
            // lbl_EqName
            // 
            this.lbl_EqName.AutoSize = true;
            this.lbl_EqName.Location = new System.Drawing.Point(308, 64);
            this.lbl_EqName.Name = "lbl_EqName";
            this.lbl_EqName.Size = new System.Drawing.Size(88, 16);
            this.lbl_EqName.TabIndex = 2;
            this.lbl_EqName.Text = "设备名称：";
            // 
            // lbl_EqNo
            // 
            this.lbl_EqNo.AutoSize = true;
            this.lbl_EqNo.Location = new System.Drawing.Point(22, 64);
            this.lbl_EqNo.Name = "lbl_EqNo";
            this.lbl_EqNo.Size = new System.Drawing.Size(88, 16);
            this.lbl_EqNo.TabIndex = 1;
            this.lbl_EqNo.Text = "设备编号：";
            // 
            // lbl_EqCode
            // 
            this.lbl_EqCode.AutoSize = true;
            this.lbl_EqCode.Location = new System.Drawing.Point(22, 18);
            this.lbl_EqCode.Name = "lbl_EqCode";
            this.lbl_EqCode.Size = new System.Drawing.Size(88, 16);
            this.lbl_EqCode.TabIndex = 0;
            this.lbl_EqCode.Text = "设备条码：";
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.tabControl1);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panel6.Location = new System.Drawing.Point(316, 201);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(688, 403);
            this.panel6.TabIndex = 3;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(688, 403);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dgv_MaintainData);
            this.tabPage1.Location = new System.Drawing.Point(4, 26);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(680, 373);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "保养明细";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // dgv_MaintainData
            // 
            this.dgv_MaintainData.AllowUserToAddRows = false;
            this.dgv_MaintainData.AllowUserToDeleteRows = false;
            this.dgv_MaintainData.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv_MaintainData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_MaintainData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_MaintainData.Location = new System.Drawing.Point(3, 3);
            this.dgv_MaintainData.Name = "dgv_MaintainData";
            this.dgv_MaintainData.RowHeadersVisible = false;
            this.dgv_MaintainData.RowTemplate.Height = 23;
            this.dgv_MaintainData.Size = new System.Drawing.Size(674, 367);
            this.dgv_MaintainData.TabIndex = 0;
            this.dgv_MaintainData.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_MaintainData_CellDoubleClick);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dgv_HistoryData);
            this.tabPage2.Location = new System.Drawing.Point(4, 26);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(680, 373);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "历史保养明细";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dgv_HistoryData
            // 
            this.dgv_HistoryData.AllowUserToAddRows = false;
            this.dgv_HistoryData.AllowUserToDeleteRows = false;
            this.dgv_HistoryData.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv_HistoryData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_HistoryData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_HistoryData.Location = new System.Drawing.Point(3, 3);
            this.dgv_HistoryData.Name = "dgv_HistoryData";
            this.dgv_HistoryData.ReadOnly = true;
            this.dgv_HistoryData.RowTemplate.Height = 23;
            this.dgv_HistoryData.Size = new System.Drawing.Size(674, 367);
            this.dgv_HistoryData.TabIndex = 0;
            // 
            // frmEqMaintain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1258, 604);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmEqMaintain";
            this.Text = "设备保养";
            this.Load += new System.EventHandler(this.frmEqMaintain_Load);
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_OldData)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_QueryPaln)).EndInit();
            this.panel4.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel7.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_MaintainData)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_HistoryData)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.DataGridView dgv_OldData;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dgv_QueryPaln;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Button btn_Save;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_Query;
        private System.Windows.Forms.ComboBox cbo_QueryName;
        private System.Windows.Forms.TextBox txt_EqNo;
        private System.Windows.Forms.TextBox txt_State;
        private System.Windows.Forms.TextBox txt_CopyBook;
        private System.Windows.Forms.TextBox txt_EqName;
        private System.Windows.Forms.TextBox txt_People;
        private System.Windows.Forms.TextBox txt_QueryVal;
        private System.Windows.Forms.TextBox txt_EqCode;
        private System.Windows.Forms.Label lbl_People;
        private System.Windows.Forms.Label lbl_State;
        private System.Windows.Forms.Label lbl_EqName;
        private System.Windows.Forms.Label lbl_EqNo;
        private System.Windows.Forms.Label lbl_EqCode;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGridView dgv_MaintainData;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView dgv_HistoryData;
    }
}