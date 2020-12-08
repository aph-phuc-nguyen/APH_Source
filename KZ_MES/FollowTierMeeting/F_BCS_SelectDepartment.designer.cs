namespace FollowTierMeeting
{
    partial class F_BCS_SelectDepartment_ProcessForm
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.dgvLine = new System.Windows.Forms.DataGridView();
            this.cellLine = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UDF01 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dvgSection = new System.Windows.Forms.DataGridView();
            this.cellSection = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvPlant = new System.Windows.Forms.DataGridView();
            this.cellPlant = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLine)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvgSection)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPlant)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Controls.Add(this.dgvLine, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.dvgSection, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.dgvPlant, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 66);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(439, 491);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // dgvLine
            // 
            this.dgvLine.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvLine.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLine.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cellLine,
            this.UDF01});
            this.dgvLine.Location = new System.Drawing.Point(295, 3);
            this.dgvLine.Name = "dgvLine";
            this.dgvLine.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvLine.Size = new System.Drawing.Size(141, 485);
            this.dgvLine.TabIndex = 0;
            this.dgvLine.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvLine_CellDoubleClick);
            this.dgvLine.Enter += new System.EventHandler(this.dgvLine_Enter);
            // 
            // cellLine
            // 
            this.cellLine.DataPropertyName = "DEPARTMENT_CODE";
            this.cellLine.HeaderText = "Line";
            this.cellLine.Name = "cellLine";
            // 
            // UDF01
            // 
            this.UDF01.DataPropertyName = "UDF01";
            this.UDF01.HeaderText = "UDF01";
            this.UDF01.Name = "UDF01";
            this.UDF01.Visible = false;
            // 
            // dvgSection
            // 
            this.dvgSection.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dvgSection.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dvgSection.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cellSection});
            this.dvgSection.Location = new System.Drawing.Point(149, 3);
            this.dvgSection.Name = "dvgSection";
            this.dvgSection.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dvgSection.Size = new System.Drawing.Size(140, 485);
            this.dvgSection.TabIndex = 0;
            this.dvgSection.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dvgSection_CellClick);
            this.dvgSection.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dvgSection_CellDoubleClick);
            this.dvgSection.Enter += new System.EventHandler(this.dvgSection_Enter);
            // 
            // cellSection
            // 
            this.cellSection.DataPropertyName = "DEPARTMENT_CODE";
            this.cellSection.HeaderText = "Section";
            this.cellSection.Name = "cellSection";
            // 
            // dgvPlant
            // 
            this.dgvPlant.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvPlant.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPlant.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPlant.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cellPlant});
            this.dgvPlant.Location = new System.Drawing.Point(3, 3);
            this.dgvPlant.Name = "dgvPlant";
            this.dgvPlant.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvPlant.Size = new System.Drawing.Size(140, 485);
            this.dgvPlant.TabIndex = 0;
            this.dgvPlant.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPlant_CellClick);
            this.dgvPlant.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPlant_CellDoubleClick);
            this.dgvPlant.Enter += new System.EventHandler(this.dgvPlant_Enter);
            // 
            // cellPlant
            // 
            this.cellPlant.DataPropertyName = "DEPARTMENT_CODE";
            this.cellPlant.HeaderText = "Plant";
            this.cellPlant.Name = "cellPlant";
            // 
            // F_BCS_SelectDepartment_ProcessForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(463, 560);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MaximizeBox = false;
            this.Name = "F_BCS_SelectDepartment_ProcessForm";
            this.Sizable = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select department";
            this.Load += new System.EventHandler(this.F_BCS_SelectDepartment_ProcessForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLine)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvgSection)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPlant)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridView dgvPlant;
        private System.Windows.Forms.DataGridViewTextBoxColumn cellPlant;
        private System.Windows.Forms.DataGridView dgvLine;
        private System.Windows.Forms.DataGridViewTextBoxColumn cellLine;
        private System.Windows.Forms.DataGridViewTextBoxColumn UDF01;
        private System.Windows.Forms.DataGridView dvgSection;
        private System.Windows.Forms.DataGridViewTextBoxColumn cellSection;
    }
}