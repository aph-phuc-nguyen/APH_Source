﻿namespace F_TMS_TierMeeting3_Main
{
    partial class MaturityAssessmentForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.gridData = new System.Windows.Forms.DataGridView();
            this.Dept = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UPDATEDDATE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NAMECN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NAMEEN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NAMEYN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.STATUS = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.NOTE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MATURITYCODE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CODE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblDepartment = new System.Windows.Forms.Label();
            this.txtDepartment = new System.Windows.Forms.TextBox();
            this.btnSave = new MaterialSkin.Controls.MaterialRaisedButton();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.btnQuery = new MaterialSkin.Controls.MaterialRaisedButton();
            this.lblNameText = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridData)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.gridData, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 75);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1896, 993);
            this.tableLayoutPanel1.TabIndex = 10016;
            // 
            // gridData
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.gridData.ColumnHeadersHeight = 40;
            this.gridData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Dept,
            this.UPDATEDDATE,
            this.NAMECN,
            this.NAMEEN,
            this.NAMEYN,
            this.STATUS,
            this.NOTE,
            this.MATURITYCODE,
            this.CODE});
            this.gridData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridData.Location = new System.Drawing.Point(3, 63);
            this.gridData.Name = "gridData";
            this.gridData.RowTemplate.Height = 40;
            this.gridData.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.gridData.Size = new System.Drawing.Size(1890, 907);
            this.gridData.TabIndex = 4;
            // 
            // Dept
            // 
            this.Dept.DataPropertyName = "DEPTCODE";
            this.Dept.HeaderText = "Department";
            this.Dept.Name = "Dept";
            this.Dept.Visible = false;
            // 
            // UPDATEDDATE
            // 
            this.UPDATEDDATE.DataPropertyName = "UPDATEDDATE";
            this.UPDATEDDATE.HeaderText = "Date";
            this.UPDATEDDATE.Name = "UPDATEDDATE";
            this.UPDATEDDATE.Visible = false;
            // 
            // NAMECN
            // 
            this.NAMECN.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.NAMECN.DataPropertyName = "NAMECN";
            this.NAMECN.HeaderText = "Name";
            this.NAMECN.Name = "NAMECN";
            this.NAMECN.ReadOnly = true;
            this.NAMECN.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.NAMECN.Visible = false;
            this.NAMECN.Width = 84;
            // 
            // NAMEEN
            // 
            this.NAMEEN.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.NAMEEN.DataPropertyName = "NAMEEN";
            this.NAMEEN.HeaderText = "Name";
            this.NAMEEN.Name = "NAMEEN";
            this.NAMEEN.ReadOnly = true;
            this.NAMEEN.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.NAMEEN.Visible = false;
            this.NAMEEN.Width = 84;
            // 
            // NAMEYN
            // 
            this.NAMEYN.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.NAMEYN.DataPropertyName = "NAMEYN";
            this.NAMEYN.HeaderText = "Name";
            this.NAMEYN.Name = "NAMEYN";
            this.NAMEYN.ReadOnly = true;
            this.NAMEYN.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.NAMEYN.Visible = false;
            this.NAMEYN.Width = 84;
            // 
            // STATUS
            // 
            this.STATUS.DataPropertyName = "STATUS";
            this.STATUS.HeaderText = "Status";
            this.STATUS.Items.AddRange(new object[] {
            "Yes",
            "No",
            "N/A"});
            this.STATUS.Name = "STATUS";
            this.STATUS.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.STATUS.Width = 200;
            // 
            // NOTE
            // 
            this.NOTE.DataPropertyName = "NOTE";
            this.NOTE.HeaderText = "Note";
            this.NOTE.Name = "NOTE";
            this.NOTE.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.NOTE.Width = 300;
            // 
            // MATURITYCODE
            // 
            this.MATURITYCODE.DataPropertyName = "MATURITYCODE";
            this.MATURITYCODE.HeaderText = "Maturity Code";
            this.MATURITYCODE.Name = "MATURITYCODE";
            this.MATURITYCODE.Visible = false;
            // 
            // CODE
            // 
            this.CODE.DataPropertyName = "CODE";
            this.CODE.HeaderText = "Code";
            this.CODE.Name = "CODE";
            this.CODE.Visible = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblNameText);
            this.panel1.Controls.Add(this.lblDepartment);
            this.panel1.Controls.Add(this.txtDepartment);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.dtpDate);
            this.panel1.Controls.Add(this.btnQuery);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1890, 54);
            this.panel1.TabIndex = 5;
            // 
            // lblDepartment
            // 
            this.lblDepartment.AutoSize = true;
            this.lblDepartment.Location = new System.Drawing.Point(308, 12);
            this.lblDepartment.Name = "lblDepartment";
            this.lblDepartment.Size = new System.Drawing.Size(62, 13);
            this.lblDepartment.TabIndex = 10021;
            this.lblDepartment.Text = "Department";
            // 
            // txtDepartment
            // 
            this.txtDepartment.Location = new System.Drawing.Point(442, 12);
            this.txtDepartment.Name = "txtDepartment";
            this.txtDepartment.ReadOnly = true;
            this.txtDepartment.Size = new System.Drawing.Size(122, 20);
            this.txtDepartment.TabIndex = 10020;
            this.txtDepartment.DoubleClick += new System.EventHandler(this.txtDepartment_DoubleClick);
            // 
            // btnSave
            // 
            this.btnSave.AutoSize = true;
            this.btnSave.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnSave.Depth = 0;
            this.btnSave.Icon = null;
            this.btnSave.Location = new System.Drawing.Point(861, 6);
            this.btnSave.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnSave.Name = "btnSave";
            this.btnSave.Primary = true;
            this.btnSave.Size = new System.Drawing.Size(55, 36);
            this.btnSave.TabIndex = 10019;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // dtpDate
            // 
            this.dtpDate.Location = new System.Drawing.Point(47, 12);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(200, 20);
            this.dtpDate.TabIndex = 10003;
            this.dtpDate.ValueChanged += new System.EventHandler(this.dtpDate_ValueChanged);
            // 
            // btnQuery
            // 
            this.btnQuery.AutoSize = true;
            this.btnQuery.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnQuery.Depth = 0;
            this.btnQuery.Icon = null;
            this.btnQuery.Location = new System.Drawing.Point(691, 6);
            this.btnQuery.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Primary = true;
            this.btnQuery.Size = new System.Drawing.Size(64, 36);
            this.btnQuery.TabIndex = 10005;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // lblNameText
            // 
            this.lblNameText.AutoSize = true;
            this.lblNameText.Location = new System.Drawing.Point(1185, 15);
            this.lblNameText.Name = "lblNameText";
            this.lblNameText.Size = new System.Drawing.Size(35, 13);
            this.lblNameText.TabIndex = 10022;
            this.lblNameText.Text = "Name";
            this.lblNameText.Visible = false;
            // 
            // MaturityAssessmentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1920, 1080);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "MaturityAssessmentForm";
            this.Text = "Maturity Assessment";
            this.Load += new System.EventHandler(this.MaturityAssessmentForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridData)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private MaterialSkin.Controls.MaterialRaisedButton btnQuery;
        private System.Windows.Forms.DataGridView gridData;
        private MaterialSkin.Controls.MaterialRaisedButton btnSave;
        private System.Windows.Forms.DataGridViewTextBoxColumn Dept;
        private System.Windows.Forms.DataGridViewTextBoxColumn UPDATEDDATE;
        private System.Windows.Forms.DataGridViewTextBoxColumn NAMECN;
        private System.Windows.Forms.DataGridViewTextBoxColumn NAMEEN;
        private System.Windows.Forms.DataGridViewTextBoxColumn NAMEYN;
        private System.Windows.Forms.DataGridViewComboBoxColumn STATUS;
        private System.Windows.Forms.DataGridViewTextBoxColumn NOTE;
        private System.Windows.Forms.DataGridViewTextBoxColumn MATURITYCODE;
        private System.Windows.Forms.DataGridViewTextBoxColumn CODE;
        private System.Windows.Forms.Label lblDepartment;
        private System.Windows.Forms.TextBox txtDepartment;
        private System.Windows.Forms.Label lblNameText;
    }
}