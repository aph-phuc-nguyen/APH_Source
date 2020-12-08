﻿namespace F_TMS_TierMeeting2_Main
{
    partial class KaizenActionForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dtp = new System.Windows.Forms.DateTimePicker();
            this.gridData = new System.Windows.Forms.DataGridView();
            this.colCbx = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.G_ITIME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.G_DEPTCODE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.G_GREATEDDATE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.G_FINDER = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.G_PROBLEMPOINT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.G_MEASURE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.G_PRINCTIPAL = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.G_PLANDATE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.G_FINISHDATE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.G_REMARK = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.G_T1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.G_T2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.G_T3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.G_T4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnAdd = new MaterialSkin.Controls.MaterialRaisedButton();
            this.btnClose = new MaterialSkin.Controls.MaterialRaisedButton();
            this.btnUpgrade = new MaterialSkin.Controls.MaterialRaisedButton();
            this.btnSave = new MaterialSkin.Controls.MaterialRaisedButton();
            this.btnEdit = new MaterialSkin.Controls.MaterialRaisedButton();
            this.lblDepartment = new System.Windows.Forms.Label();
            this.txtDepartment = new System.Windows.Forms.TextBox();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.btnQuery = new MaterialSkin.Controls.MaterialRaisedButton();
            this.lblFrom = new System.Windows.Forms.Label();
            this.lblTo = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.gridData)).BeginInit();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dtp
            // 
            this.dtp.Location = new System.Drawing.Point(731, 574);
            this.dtp.Name = "dtp";
            this.dtp.Size = new System.Drawing.Size(200, 20);
            this.dtp.TabIndex = 10015;
            this.dtp.MouseEnter += new System.EventHandler(this.dtp_MouseEnter);
            // 
            // gridData
            // 
            this.gridData.AllowUserToAddRows = false;
            this.gridData.AllowUserToDeleteRows = false;
            this.gridData.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridData.ColumnHeadersHeight = 40;
            this.gridData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCbx,
            this.G_ITIME,
            this.G_DEPTCODE,
            this.G_GREATEDDATE,
            this.G_FINDER,
            this.G_PROBLEMPOINT,
            this.G_MEASURE,
            this.G_PRINCTIPAL,
            this.G_PLANDATE,
            this.G_FINISHDATE,
            this.G_REMARK,
            this.G_T1,
            this.G_T2,
            this.G_T3,
            this.G_T4});
            this.gridData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridData.Location = new System.Drawing.Point(3, 63);
            this.gridData.Name = "gridData";
            this.gridData.RowTemplate.Height = 40;
            this.gridData.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.gridData.Size = new System.Drawing.Size(1641, 686);
            this.gridData.TabIndex = 4;
            this.gridData.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridData_CellClick);
            this.gridData.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridData_CellContentClick);
            this.gridData.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.gridData_ColumnHeaderMouseClick);
            this.gridData.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.gridData_ColumnWidthChanged);
            this.gridData.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.gridData_DefaultValuesNeeded);
            this.gridData.Scroll += new System.Windows.Forms.ScrollEventHandler(this.gridData_Scroll);
            // 
            // colCbx
            // 
            this.colCbx.HeaderText = "";
            this.colCbx.Name = "colCbx";
            // 
            // G_ITIME
            // 
            this.G_ITIME.DataPropertyName = "ID";
            this.G_ITIME.HeaderText = "Item";
            this.G_ITIME.Name = "G_ITIME";
            this.G_ITIME.ReadOnly = true;
            this.G_ITIME.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.G_ITIME.Visible = false;
            // 
            // G_DEPTCODE
            // 
            this.G_DEPTCODE.DataPropertyName = "G_DEPTCODE";
            this.G_DEPTCODE.HeaderText = "Department";
            this.G_DEPTCODE.Name = "G_DEPTCODE";
            this.G_DEPTCODE.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // G_GREATEDDATE
            // 
            this.G_GREATEDDATE.DataPropertyName = "G_GREATEDDATE";
            this.G_GREATEDDATE.HeaderText = "Created date";
            this.G_GREATEDDATE.Name = "G_GREATEDDATE";
            this.G_GREATEDDATE.ReadOnly = true;
            this.G_GREATEDDATE.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // G_FINDER
            // 
            this.G_FINDER.DataPropertyName = "G_FINDER";
            this.G_FINDER.HeaderText = "Finder";
            this.G_FINDER.Name = "G_FINDER";
            this.G_FINDER.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // G_PROBLEMPOINT
            // 
            this.G_PROBLEMPOINT.DataPropertyName = "G_PROBLEMPOINT";
            this.G_PROBLEMPOINT.HeaderText = "Problem point";
            this.G_PROBLEMPOINT.Name = "G_PROBLEMPOINT";
            this.G_PROBLEMPOINT.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // G_MEASURE
            // 
            this.G_MEASURE.DataPropertyName = "G_MEASURE";
            this.G_MEASURE.HeaderText = "Measure";
            this.G_MEASURE.Name = "G_MEASURE";
            this.G_MEASURE.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // G_PRINCTIPAL
            // 
            this.G_PRINCTIPAL.DataPropertyName = "G_PRINCTIPAL";
            this.G_PRINCTIPAL.HeaderText = "Principal";
            this.G_PRINCTIPAL.Name = "G_PRINCTIPAL";
            this.G_PRINCTIPAL.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // G_PLANDATE
            // 
            this.G_PLANDATE.DataPropertyName = "G_PLANDATE";
            this.G_PLANDATE.HeaderText = "Plan date";
            this.G_PLANDATE.Name = "G_PLANDATE";
            this.G_PLANDATE.ReadOnly = true;
            this.G_PLANDATE.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // G_FINISHDATE
            // 
            this.G_FINISHDATE.DataPropertyName = "G_FINISHDATE";
            this.G_FINISHDATE.HeaderText = "Finish date";
            this.G_FINISHDATE.Name = "G_FINISHDATE";
            this.G_FINISHDATE.ReadOnly = true;
            this.G_FINISHDATE.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // G_REMARK
            // 
            this.G_REMARK.DataPropertyName = "G_REMARK";
            this.G_REMARK.HeaderText = "Remark";
            this.G_REMARK.Name = "G_REMARK";
            this.G_REMARK.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // G_T1
            // 
            this.G_T1.DataPropertyName = "G_T1";
            this.G_T1.HeaderText = "T1";
            this.G_T1.Name = "G_T1";
            this.G_T1.ReadOnly = true;
            this.G_T1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.G_T1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // G_T2
            // 
            this.G_T2.DataPropertyName = "G_T2";
            this.G_T2.HeaderText = "T2";
            this.G_T2.Name = "G_T2";
            this.G_T2.ReadOnly = true;
            this.G_T2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.G_T2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // G_T3
            // 
            this.G_T3.DataPropertyName = "G_T3";
            this.G_T3.HeaderText = "T3";
            this.G_T3.Name = "G_T3";
            this.G_T3.ReadOnly = true;
            this.G_T3.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.G_T3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // G_T4
            // 
            this.G_T4.DataPropertyName = "G_T4";
            this.G_T4.HeaderText = "T4";
            this.G_T4.Name = "G_T4";
            this.G_T4.ReadOnly = true;
            this.G_T4.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.G_T4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnAdd);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Controls.Add(this.btnUpgrade);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.btnEdit);
            this.panel1.Controls.Add(this.lblDepartment);
            this.panel1.Controls.Add(this.txtDepartment);
            this.panel1.Controls.Add(this.dtpTo);
            this.panel1.Controls.Add(this.dtpFrom);
            this.panel1.Controls.Add(this.btnQuery);
            this.panel1.Controls.Add(this.lblFrom);
            this.panel1.Controls.Add(this.lblTo);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1641, 54);
            this.panel1.TabIndex = 5;
            // 
            // btnAdd
            // 
            this.btnAdd.AutoSize = true;
            this.btnAdd.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnAdd.Depth = 0;
            this.btnAdd.Icon = null;
            this.btnAdd.Location = new System.Drawing.Point(920, 8);
            this.btnAdd.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Primary = true;
            this.btnAdd.Size = new System.Drawing.Size(48, 36);
            this.btnAdd.TabIndex = 10021;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnClose
            // 
            this.btnClose.AutoSize = true;
            this.btnClose.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnClose.Depth = 0;
            this.btnClose.Icon = null;
            this.btnClose.Location = new System.Drawing.Point(1256, 9);
            this.btnClose.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnClose.Name = "btnClose";
            this.btnClose.Primary = true;
            this.btnClose.Size = new System.Drawing.Size(63, 36);
            this.btnClose.TabIndex = 10020;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnUpgrade
            // 
            this.btnUpgrade.AutoSize = true;
            this.btnUpgrade.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnUpgrade.Depth = 0;
            this.btnUpgrade.Icon = null;
            this.btnUpgrade.Location = new System.Drawing.Point(1153, 9);
            this.btnUpgrade.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnUpgrade.Name = "btnUpgrade";
            this.btnUpgrade.Primary = true;
            this.btnUpgrade.Size = new System.Drawing.Size(82, 36);
            this.btnUpgrade.TabIndex = 10019;
            this.btnUpgrade.Text = "Upgrade";
            this.btnUpgrade.UseVisualStyleBackColor = true;
            this.btnUpgrade.Click += new System.EventHandler(this.btnUpgrade_Click);
            // 
            // btnSave
            // 
            this.btnSave.AutoSize = true;
            this.btnSave.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnSave.Depth = 0;
            this.btnSave.Icon = null;
            this.btnSave.Location = new System.Drawing.Point(1075, 9);
            this.btnSave.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnSave.Name = "btnSave";
            this.btnSave.Primary = true;
            this.btnSave.Size = new System.Drawing.Size(55, 36);
            this.btnSave.TabIndex = 10018;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.AutoSize = true;
            this.btnEdit.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnEdit.Depth = 0;
            this.btnEdit.Icon = null;
            this.btnEdit.Location = new System.Drawing.Point(1000, 8);
            this.btnEdit.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Primary = true;
            this.btnEdit.Size = new System.Drawing.Size(50, 36);
            this.btnEdit.TabIndex = 10017;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // lblDepartment
            // 
            this.lblDepartment.AutoSize = true;
            this.lblDepartment.Location = new System.Drawing.Point(495, 15);
            this.lblDepartment.Name = "lblDepartment";
            this.lblDepartment.Size = new System.Drawing.Size(62, 13);
            this.lblDepartment.TabIndex = 10016;
            this.lblDepartment.Text = "Department";
            this.lblDepartment.Click += new System.EventHandler(this.lblDepartment_Click);
            // 
            // txtDepartment
            // 
            this.txtDepartment.Location = new System.Drawing.Point(630, 9);
            this.txtDepartment.Name = "txtDepartment";
            this.txtDepartment.ReadOnly = true;
            this.txtDepartment.Size = new System.Drawing.Size(122, 20);
            this.txtDepartment.TabIndex = 10015;
            this.txtDepartment.DoubleClick += new System.EventHandler(this.txtDepartment_DoubleClick);
            // 
            // dtpTo
            // 
            this.dtpTo.Location = new System.Drawing.Point(288, 11);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(145, 20);
            this.dtpTo.TabIndex = 10004;
            // 
            // dtpFrom
            // 
            this.dtpFrom.Location = new System.Drawing.Point(56, 11);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(134, 20);
            this.dtpFrom.TabIndex = 10003;
            // 
            // btnQuery
            // 
            this.btnQuery.AutoSize = true;
            this.btnQuery.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnQuery.Depth = 0;
            this.btnQuery.Icon = null;
            this.btnQuery.Location = new System.Drawing.Point(839, 8);
            this.btnQuery.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Primary = true;
            this.btnQuery.Size = new System.Drawing.Size(64, 36);
            this.btnQuery.TabIndex = 10005;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // lblFrom
            // 
            this.lblFrom.AutoSize = true;
            this.lblFrom.Location = new System.Drawing.Point(3, 15);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(30, 13);
            this.lblFrom.TabIndex = 10006;
            this.lblFrom.Text = "From";
            // 
            // lblTo
            // 
            this.lblTo.AutoSize = true;
            this.lblTo.Location = new System.Drawing.Point(241, 15);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(20, 13);
            this.lblTo.TabIndex = 10007;
            this.lblTo.Text = "To";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.gridData, 0, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(2, 68);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1647, 772);
            this.tableLayoutPanel1.TabIndex = 10015;
            // 
            // KaizenActionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1651, 853);
            this.Controls.Add(this.dtp);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "KaizenActionForm";
            this.Text = "Kaizen Action";
            this.Load += new System.EventHandler(this.KaizenAction_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridData)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DateTimePicker dtp;
        private System.Windows.Forms.DataGridView gridData;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private MaterialSkin.Controls.MaterialRaisedButton btnQuery;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox txtDepartment;
        private System.Windows.Forms.Label lblDepartment;
        private MaterialSkin.Controls.MaterialRaisedButton btnUpgrade;
        private MaterialSkin.Controls.MaterialRaisedButton btnSave;
        private MaterialSkin.Controls.MaterialRaisedButton btnEdit;
        private MaterialSkin.Controls.MaterialRaisedButton btnClose;
        private MaterialSkin.Controls.MaterialRaisedButton btnAdd;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colCbx;
        private System.Windows.Forms.DataGridViewTextBoxColumn G_ITIME;
        private System.Windows.Forms.DataGridViewTextBoxColumn G_DEPTCODE;
        private System.Windows.Forms.DataGridViewTextBoxColumn G_GREATEDDATE;
        private System.Windows.Forms.DataGridViewTextBoxColumn G_FINDER;
        private System.Windows.Forms.DataGridViewTextBoxColumn G_PROBLEMPOINT;
        private System.Windows.Forms.DataGridViewTextBoxColumn G_MEASURE;
        private System.Windows.Forms.DataGridViewTextBoxColumn G_PRINCTIPAL;
        private System.Windows.Forms.DataGridViewTextBoxColumn G_PLANDATE;
        private System.Windows.Forms.DataGridViewTextBoxColumn G_FINISHDATE;
        private System.Windows.Forms.DataGridViewTextBoxColumn G_REMARK;
        private System.Windows.Forms.DataGridViewTextBoxColumn G_T1;
        private System.Windows.Forms.DataGridViewTextBoxColumn G_T2;
        private System.Windows.Forms.DataGridViewTextBoxColumn G_T3;
        private System.Windows.Forms.DataGridViewTextBoxColumn G_T4;
    }
}