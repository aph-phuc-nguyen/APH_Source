namespace ProductionTargets
{
    partial class PlanAdjustmentForm
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.size = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.size_seq = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.work_qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SUPPLEMENT_QTY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.finish_qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.move_qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.text_new_dept_no = new System.Windows.Forms.TextBox();
            this.dt_new_work_day = new System.Windows.Forms.DateTimePicker();
            this.btn_exit = new System.Windows.Forms.Button();
            this.btn_save = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txt_work_day = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.text_d_dept = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tet_se_id = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtMoveDate = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_move_no = new System.Windows.Forms.TextBox();
            this.autocompleteMenu1 = new AutocompleteMenuNS.AutocompleteMenu();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
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
            this.tableLayoutPanel1.Controls.Add(this.dataGridView1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 73);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(969, 536);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.size,
            this.size_seq,
            this.work_qty,
            this.SUPPLEMENT_QTY,
            this.finish_qty,
            this.move_qty});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 163);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(963, 370);
            this.dataGridView1.TabIndex = 1;
            // 
            // size
            // 
            this.size.DataPropertyName = "size_no";
            this.size.HeaderText = "size";
            this.size.Name = "size";
            this.size.ReadOnly = true;
            // 
            // size_seq
            // 
            this.size_seq.DataPropertyName = "size_seq";
            this.size_seq.HeaderText = "size_seq";
            this.size_seq.Name = "size_seq";
            this.size_seq.Visible = false;
            // 
            // work_qty
            // 
            this.work_qty.DataPropertyName = "work_qty";
            this.work_qty.HeaderText = "排产数量";
            this.work_qty.Name = "work_qty";
            this.work_qty.ReadOnly = true;
            // 
            // SUPPLEMENT_QTY
            // 
            this.SUPPLEMENT_QTY.DataPropertyName = "SUPPLEMENT_QTY";
            this.SUPPLEMENT_QTY.HeaderText = "增补数量";
            this.SUPPLEMENT_QTY.Name = "SUPPLEMENT_QTY";
            this.SUPPLEMENT_QTY.ReadOnly = true;
            // 
            // finish_qty
            // 
            this.finish_qty.DataPropertyName = "finish_qty";
            this.finish_qty.HeaderText = "完工数量";
            this.finish_qty.Name = "finish_qty";
            this.finish_qty.ReadOnly = true;
            // 
            // move_qty
            // 
            this.move_qty.DataPropertyName = "move_qty";
            this.move_qty.HeaderText = "调拨数量";
            this.move_qty.Name = "move_qty";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.text_new_dept_no);
            this.panel1.Controls.Add(this.dt_new_work_day);
            this.panel1.Controls.Add(this.btn_exit);
            this.panel1.Controls.Add(this.btn_save);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.txt_work_day);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.text_d_dept);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.tet_se_id);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txtMoveDate);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txt_move_no);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(963, 154);
            this.panel1.TabIndex = 0;
            // 
            // text_new_dept_no
            // 
            this.autocompleteMenu1.SetAutocompleteMenu(this.text_new_dept_no, this.autocompleteMenu1);
            this.text_new_dept_no.Location = new System.Drawing.Point(75, 128);
            this.text_new_dept_no.Name = "text_new_dept_no";
            this.text_new_dept_no.Size = new System.Drawing.Size(141, 20);
            this.text_new_dept_no.TabIndex = 21;
            // 
            // dt_new_work_day
            // 
            this.dt_new_work_day.CustomFormat = "yyyy/MM/dd";
            this.dt_new_work_day.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dt_new_work_day.Location = new System.Drawing.Point(317, 122);
            this.dt_new_work_day.Name = "dt_new_work_day";
            this.dt_new_work_day.Size = new System.Drawing.Size(137, 20);
            this.dt_new_work_day.TabIndex = 19;
            // 
            // btn_exit
            // 
            this.btn_exit.Location = new System.Drawing.Point(486, 122);
            this.btn_exit.Name = "btn_exit";
            this.btn_exit.Size = new System.Drawing.Size(200, 25);
            this.btn_exit.TabIndex = 17;
            this.btn_exit.Text = "退出";
            this.btn_exit.UseVisualStyleBackColor = true;
            this.btn_exit.Click += new System.EventHandler(this.btn_exit_Click);
            // 
            // btn_save
            // 
            this.btn_save.Location = new System.Drawing.Point(486, 67);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(200, 25);
            this.btn_save.TabIndex = 16;
            this.btn_save.Text = "保存并退出";
            this.btn_save.UseVisualStyleBackColor = true;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(229, 129);
            this.label7.MaximumSize = new System.Drawing.Size(70, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(67, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "新包装时间";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(9, 128);
            this.label8.MaximumSize = new System.Drawing.Size(65, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(49, 13);
            this.label8.TabIndex = 13;
            this.label8.Text = "调拨至：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(229, 76);
            this.label5.MaximumSize = new System.Drawing.Size(70, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 26);
            this.label5.TabIndex = 9;
            this.label5.Text = "原计划包装时间";
            // 
            // txt_work_day
            // 
            this.autocompleteMenu1.SetAutocompleteMenu(this.txt_work_day, null);
            this.txt_work_day.Location = new System.Drawing.Point(321, 69);
            this.txt_work_day.Name = "txt_work_day";
            this.txt_work_day.ReadOnly = true;
            this.txt_work_day.Size = new System.Drawing.Size(133, 20);
            this.txt_work_day.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 79);
            this.label4.MaximumSize = new System.Drawing.Size(65, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "原包装部门";
            // 
            // text_d_dept
            // 
            this.autocompleteMenu1.SetAutocompleteMenu(this.text_d_dept, null);
            this.text_d_dept.Location = new System.Drawing.Point(75, 72);
            this.text_d_dept.Name = "text_d_dept";
            this.text_d_dept.ReadOnly = true;
            this.text_d_dept.Size = new System.Drawing.Size(141, 20);
            this.text_d_dept.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(484, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "订单号";
            // 
            // tet_se_id
            // 
            this.autocompleteMenu1.SetAutocompleteMenu(this.tet_se_id, null);
            this.tet_se_id.Location = new System.Drawing.Point(538, 16);
            this.tet_se_id.Name = "tet_se_id";
            this.tet_se_id.ReadOnly = true;
            this.tet_se_id.Size = new System.Drawing.Size(148, 20);
            this.tet_se_id.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(230, 23);
            this.label2.MaximumSize = new System.Drawing.Size(70, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "调拨时间";
            // 
            // txtMoveDate
            // 
            this.autocompleteMenu1.SetAutocompleteMenu(this.txtMoveDate, null);
            this.txtMoveDate.Location = new System.Drawing.Point(317, 16);
            this.txtMoveDate.Name = "txtMoveDate";
            this.txtMoveDate.ReadOnly = true;
            this.txtMoveDate.Size = new System.Drawing.Size(133, 20);
            this.txtMoveDate.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 23);
            this.label1.MaximumSize = new System.Drawing.Size(65, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "调拨单号";
            // 
            // txt_move_no
            // 
            this.autocompleteMenu1.SetAutocompleteMenu(this.txt_move_no, null);
            this.txt_move_no.Location = new System.Drawing.Point(77, 16);
            this.txt_move_no.Name = "txt_move_no";
            this.txt_move_no.ReadOnly = true;
            this.txt_move_no.Size = new System.Drawing.Size(140, 20);
            this.txt_move_no.TabIndex = 0;
            // 
            // autocompleteMenu1
            // 
            this.autocompleteMenu1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.autocompleteMenu1.ImageList = null;
            this.autocompleteMenu1.Items = new string[0];
            this.autocompleteMenu1.TargetControlWrapper = null;
            // 
            // PlanAdjustmentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(980, 622);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "PlanAdjustmentForm";
            this.Text = "PlanAdjustmentForm";
            this.Load += new System.EventHandler(this.PlanAdjustmentForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_move_no;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tet_se_id;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtMoveDate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox text_d_dept;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btn_exit;
        private System.Windows.Forms.Button btn_save;
        private System.Windows.Forms.DateTimePicker dt_new_work_day;
        private System.Windows.Forms.TextBox txt_work_day;
        private System.Windows.Forms.TextBox text_new_dept_no;
        private AutocompleteMenuNS.AutocompleteMenu autocompleteMenu1;
        private System.Windows.Forms.DataGridViewTextBoxColumn size;
        private System.Windows.Forms.DataGridViewTextBoxColumn size_seq;
        private System.Windows.Forms.DataGridViewTextBoxColumn work_qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn SUPPLEMENT_QTY;
        private System.Windows.Forms.DataGridViewTextBoxColumn finish_qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn move_qty;
    }
}