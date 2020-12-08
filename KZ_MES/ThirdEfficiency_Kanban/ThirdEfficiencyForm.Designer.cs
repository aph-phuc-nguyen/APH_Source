namespace ThirdEfficiency_Kanban
{
    partial class ThirdEfficiencyForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Area = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Target = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Actual = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Hours = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TRANIN_HOURS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TRANOUT_HOURS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Operator = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MultiSkill = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AllRounder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TargetPPH = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PPH = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel51 = new System.Windows.Forms.TableLayoutPanel();
            this.label211 = new System.Windows.Forms.Label();
            this.label210 = new System.Windows.Forms.Label();
            this.label209 = new System.Windows.Forms.Label();
            this.label208 = new System.Windows.Forms.Label();
            this.label207 = new System.Windows.Forms.Label();
            this.label206 = new System.Windows.Forms.Label();
            this.label205 = new System.Windows.Forms.Label();
            this.materialRaisedButton1 = new MaterialSkin.Controls.MaterialRaisedButton();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.materialRaisedButton2 = new MaterialSkin.Controls.MaterialRaisedButton();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel51.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CustomFormat = "yyyy/MM/dd";
            this.dateTimePicker1.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(203, 3);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(173, 35);
            this.dateTimePicker1.TabIndex = 1;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeight = 36;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Area,
            this.Target,
            this.Actual,
            this.Hours,
            this.TRANIN_HOURS,
            this.TRANOUT_HOURS,
            this.Operator,
            this.MultiSkill,
            this.AllRounder,
            this.TargetPPH,
            this.PPH});
            this.dataGridView1.Location = new System.Drawing.Point(3, 48);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView1.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView1.RowTemplate.Height = 36;
            this.dataGridView1.Size = new System.Drawing.Size(1283, 516);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            // 
            // Area
            // 
            this.Area.DataPropertyName = "dept";
            this.Area.HeaderText = "组别";
            this.Area.Name = "Area";
            this.Area.ReadOnly = true;
            // 
            // Target
            // 
            this.Target.DataPropertyName = "work_qty";
            this.Target.HeaderText = "目标产量";
            this.Target.Name = "Target";
            this.Target.ReadOnly = true;
            // 
            // Actual
            // 
            this.Actual.DataPropertyName = "label_qty";
            this.Actual.HeaderText = "实际产量";
            this.Actual.Name = "Actual";
            this.Actual.ReadOnly = true;
            // 
            // Hours
            // 
            this.Hours.DataPropertyName = "WORK_HOURS";
            this.Hours.HeaderText = "工时";
            this.Hours.Name = "Hours";
            this.Hours.ReadOnly = true;
            // 
            // TRANIN_HOURS
            // 
            this.TRANIN_HOURS.DataPropertyName = "TRANIN_HOURS";
            this.TRANIN_HOURS.HeaderText = "调入工时";
            this.TRANIN_HOURS.Name = "TRANIN_HOURS";
            this.TRANIN_HOURS.ReadOnly = true;
            // 
            // TRANOUT_HOURS
            // 
            this.TRANOUT_HOURS.DataPropertyName = "TRANOUT_HOURS";
            this.TRANOUT_HOURS.HeaderText = "调出工时";
            this.TRANOUT_HOURS.Name = "TRANOUT_HOURS";
            this.TRANOUT_HOURS.ReadOnly = true;
            // 
            // Operator
            // 
            this.Operator.DataPropertyName = "JOCKEY_QTY";
            this.Operator.HeaderText = "操作工";
            this.Operator.Name = "Operator";
            this.Operator.ReadOnly = true;
            // 
            // MultiSkill
            // 
            this.MultiSkill.DataPropertyName = "PLURIPOTENT_WORKER";
            this.MultiSkill.HeaderText = "多能工";
            this.MultiSkill.Name = "MultiSkill";
            this.MultiSkill.ReadOnly = true;
            // 
            // AllRounder
            // 
            this.AllRounder.DataPropertyName = "OMNIPOTENT_WORKER";
            this.AllRounder.HeaderText = "全能工";
            this.AllRounder.Name = "AllRounder";
            this.AllRounder.ReadOnly = true;
            // 
            // TargetPPH
            // 
            this.TargetPPH.DataPropertyName = "TARGETPPH";
            this.TargetPPH.HeaderText = "目标PPH";
            this.TargetPPH.Name = "TargetPPH";
            this.TargetPPH.ReadOnly = true;
            // 
            // PPH
            // 
            this.PPH.DataPropertyName = "PPH";
            this.PPH.HeaderText = "PPH";
            this.PPH.Name = "PPH";
            this.PPH.ReadOnly = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(0, 6);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(191, 21);
            this.textBox1.TabIndex = 5;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(2, 68);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1303, 649);
            this.tabControl1.TabIndex = 14;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tableLayoutPanel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1295, 623);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "产量效率报表";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.dataGridView1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel51, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1289, 617);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel51
            // 
            this.tableLayoutPanel51.BackColor = System.Drawing.Color.Lavender;
            this.tableLayoutPanel51.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel51.ColumnCount = 8;
            this.tableLayoutPanel51.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel51.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel51.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel51.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel51.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel51.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel51.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel51.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel51.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel51.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel51.Controls.Add(this.label211, 6, 0);
            this.tableLayoutPanel51.Controls.Add(this.label210, 5, 0);
            this.tableLayoutPanel51.Controls.Add(this.label209, 4, 0);
            this.tableLayoutPanel51.Controls.Add(this.label208, 3, 0);
            this.tableLayoutPanel51.Controls.Add(this.label207, 2, 0);
            this.tableLayoutPanel51.Controls.Add(this.label206, 1, 0);
            this.tableLayoutPanel51.Controls.Add(this.label205, 0, 0);
            this.tableLayoutPanel51.Controls.Add(this.materialRaisedButton1, 7, 0);
            this.tableLayoutPanel51.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel51.Location = new System.Drawing.Point(0, 567);
            this.tableLayoutPanel51.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel51.Name = "tableLayoutPanel51";
            this.tableLayoutPanel51.RowCount = 1;
            this.tableLayoutPanel51.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel51.Size = new System.Drawing.Size(1289, 50);
            this.tableLayoutPanel51.TabIndex = 4;
            // 
            // label211
            // 
            this.label211.AutoSize = true;
            this.label211.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label211.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold);
            this.label211.Location = new System.Drawing.Point(970, 1);
            this.label211.Name = "label211";
            this.label211.Size = new System.Drawing.Size(154, 48);
            this.label211.TabIndex = 6;
            this.label211.Text = "<95";
            this.label211.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label210
            // 
            this.label210.AutoSize = true;
            this.label210.BackColor = System.Drawing.Color.Red;
            this.label210.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label210.Location = new System.Drawing.Point(809, 1);
            this.label210.Name = "label210";
            this.label210.Size = new System.Drawing.Size(154, 48);
            this.label210.TabIndex = 5;
            this.label210.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label209
            // 
            this.label209.AutoSize = true;
            this.label209.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label209.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold);
            this.label209.Location = new System.Drawing.Point(648, 1);
            this.label209.Name = "label209";
            this.label209.Size = new System.Drawing.Size(154, 48);
            this.label209.TabIndex = 4;
            this.label209.Text = "Between 95-99";
            this.label209.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label208
            // 
            this.label208.AutoSize = true;
            this.label208.BackColor = System.Drawing.Color.Yellow;
            this.label208.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label208.Location = new System.Drawing.Point(487, 1);
            this.label208.Name = "label208";
            this.label208.Size = new System.Drawing.Size(154, 48);
            this.label208.TabIndex = 3;
            this.label208.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label207
            // 
            this.label207.AutoSize = true;
            this.label207.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label207.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold);
            this.label207.Location = new System.Drawing.Point(326, 1);
            this.label207.Name = "label207";
            this.label207.Size = new System.Drawing.Size(154, 48);
            this.label207.TabIndex = 2;
            this.label207.Text = ">=100";
            this.label207.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label206
            // 
            this.label206.AutoSize = true;
            this.label206.BackColor = System.Drawing.Color.LawnGreen;
            this.label206.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label206.Location = new System.Drawing.Point(165, 1);
            this.label206.Name = "label206";
            this.label206.Size = new System.Drawing.Size(154, 48);
            this.label206.TabIndex = 1;
            this.label206.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label205
            // 
            this.label205.AutoSize = true;
            this.label205.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label205.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold);
            this.label205.Location = new System.Drawing.Point(4, 1);
            this.label205.Name = "label205";
            this.label205.Size = new System.Drawing.Size(154, 48);
            this.label205.TabIndex = 0;
            this.label205.Text = "Color indicators\r\n(Acheivement)";
            this.label205.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // materialRaisedButton1
            // 
            this.materialRaisedButton1.AutoSize = true;
            this.materialRaisedButton1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.materialRaisedButton1.Depth = 0;
            this.materialRaisedButton1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.materialRaisedButton1.Icon = null;
            this.materialRaisedButton1.Location = new System.Drawing.Point(1131, 4);
            this.materialRaisedButton1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialRaisedButton1.Name = "materialRaisedButton1";
            this.materialRaisedButton1.Primary = true;
            this.materialRaisedButton1.Size = new System.Drawing.Size(154, 42);
            this.materialRaisedButton1.TabIndex = 9;
            this.materialRaisedButton1.Text = "Export";
            this.materialRaisedButton1.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.BackColor = System.Drawing.Color.Lavender;
            this.tableLayoutPanel2.ColumnCount = 6;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.dateTimePicker1, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.materialRaisedButton2, 4, 0);
            this.tableLayoutPanel2.Controls.Add(this.label1, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.panel1, 3, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1283, 39);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("黑体", 18F);
            this.label3.Location = new System.Drawing.Point(3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(194, 39);
            this.label3.TabIndex = 0;
            this.label3.Text = "生产时间:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // materialRaisedButton2
            // 
            this.materialRaisedButton2.AutoSize = true;
            this.materialRaisedButton2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.materialRaisedButton2.Depth = 0;
            this.materialRaisedButton2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.materialRaisedButton2.Icon = null;
            this.materialRaisedButton2.Location = new System.Drawing.Point(803, 3);
            this.materialRaisedButton2.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialRaisedButton2.Name = "materialRaisedButton2";
            this.materialRaisedButton2.Primary = true;
            this.materialRaisedButton2.Size = new System.Drawing.Size(194, 33);
            this.materialRaisedButton2.TabIndex = 16;
            this.materialRaisedButton2.Text = "查询";
            this.materialRaisedButton2.UseVisualStyleBackColor = true;
            this.materialRaisedButton2.Click += new System.EventHandler(this.materialRaisedButton2_Click_1);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("黑体", 18F);
            this.label1.Location = new System.Drawing.Point(403, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(194, 39);
            this.label1.TabIndex = 17;
            this.label1.Text = "厂别:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(603, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(194, 33);
            this.panel1.TabIndex = 18;
            // 
            // ThirdEfficiencyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1317, 714);
            this.Controls.Add(this.tabControl1);
            this.Name = "ThirdEfficiencyForm";
            this.Text = "Tier Meeting 2";
            this.Load += new System.EventHandler(this.ThirdEfficiencyForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel51.ResumeLayout(false);
            this.tableLayoutPanel51.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Area;
        private System.Windows.Forms.DataGridViewTextBoxColumn Target;
        private System.Windows.Forms.DataGridViewTextBoxColumn Actual;
        private System.Windows.Forms.DataGridViewTextBoxColumn Hours;
        private System.Windows.Forms.DataGridViewTextBoxColumn TRANIN_HOURS;
        private System.Windows.Forms.DataGridViewTextBoxColumn TRANOUT_HOURS;
        private System.Windows.Forms.DataGridViewTextBoxColumn Operator;
        private System.Windows.Forms.DataGridViewTextBoxColumn MultiSkill;
        private System.Windows.Forms.DataGridViewTextBoxColumn AllRounder;
        private System.Windows.Forms.DataGridViewTextBoxColumn TargetPPH;
        private System.Windows.Forms.DataGridViewTextBoxColumn PPH;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel51;
        private System.Windows.Forms.Label label211;
        private System.Windows.Forms.Label label210;
        private System.Windows.Forms.Label label209;
        private System.Windows.Forms.Label label208;
        private System.Windows.Forms.Label label207;
        private System.Windows.Forms.Label label206;
        private System.Windows.Forms.Label label205;
        private MaterialSkin.Controls.MaterialRaisedButton materialRaisedButton1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label3;
        private MaterialSkin.Controls.MaterialRaisedButton materialRaisedButton2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
    }
}