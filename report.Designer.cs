namespace rail
{
    partial class report
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dataGridView__in_run = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dataGridView_rail = new System.Windows.Forms.DataGridView();
            this.dataGridView_in_station = new System.Windows.Forms.DataGridView();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dataGridView_in_pru = new System.Windows.Forms.DataGridView();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.status = new System.Windows.Forms.StatusStrip();
            this.weight_vag = new System.Windows.Forms.ToolStripStatusLabel();
            this.sum_vagon = new System.Windows.Forms.ToolStripStatusLabel();
            this.dataGridView_finish = new System.Windows.Forms.DataGridView();
            this.button7 = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.dateTimePicker_finish = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker_start = new System.Windows.Forms.DateTimePicker();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView__in_run)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_rail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_in_station)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_in_pru)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.status.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_finish)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.dataGridView__in_run);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(624, 316);
            this.groupBox2.TabIndex = 18;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Вагоны в пути";
            // 
            // dataGridView__in_run
            // 
            this.dataGridView__in_run.AllowUserToAddRows = false;
            this.dataGridView__in_run.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView__in_run.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridView__in_run.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView__in_run.Location = new System.Drawing.Point(6, 19);
            this.dataGridView__in_run.Name = "dataGridView__in_run";
            this.dataGridView__in_run.ReadOnly = true;
            this.dataGridView__in_run.RowHeadersVisible = false;
            this.dataGridView__in_run.Size = new System.Drawing.Size(596, 291);
            this.dataGridView__in_run.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.dataGridView_rail);
            this.groupBox1.Controls.Add(this.dataGridView_in_station);
            this.groupBox1.Location = new System.Drawing.Point(12, 334);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(624, 368);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Вагоны на станции";
            // 
            // dataGridView_rail
            // 
            this.dataGridView_rail.AllowUserToAddRows = false;
            this.dataGridView_rail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView_rail.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridView_rail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_rail.Location = new System.Drawing.Point(6, 229);
            this.dataGridView_rail.Name = "dataGridView_rail";
            this.dataGridView_rail.RowHeadersVisible = false;
            this.dataGridView_rail.Size = new System.Drawing.Size(593, 133);
            this.dataGridView_rail.TabIndex = 21;
            // 
            // dataGridView_in_station
            // 
            this.dataGridView_in_station.AllowUserToAddRows = false;
            this.dataGridView_in_station.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView_in_station.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridView_in_station.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_in_station.Location = new System.Drawing.Point(6, 19);
            this.dataGridView_in_station.Name = "dataGridView_in_station";
            this.dataGridView_in_station.RowHeadersVisible = false;
            this.dataGridView_in_station.Size = new System.Drawing.Size(593, 213);
            this.dataGridView_in_station.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.dataGridView_in_pru);
            this.groupBox3.Location = new System.Drawing.Point(642, 13);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(756, 315);
            this.groupBox3.TabIndex = 20;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Вагоны на разгрузке";
            // 
            // dataGridView_in_pru
            // 
            this.dataGridView_in_pru.AllowUserToAddRows = false;
            this.dataGridView_in_pru.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView_in_pru.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridView_in_pru.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_in_pru.Location = new System.Drawing.Point(6, 19);
            this.dataGridView_in_pru.Name = "dataGridView_in_pru";
            this.dataGridView_in_pru.RowHeadersVisible = false;
            this.dataGridView_in_pru.Size = new System.Drawing.Size(744, 290);
            this.dataGridView_in_pru.TabIndex = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.status);
            this.groupBox4.Controls.Add(this.dataGridView_finish);
            this.groupBox4.Controls.Add(this.button7);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.dateTimePicker_finish);
            this.groupBox4.Controls.Add(this.dateTimePicker_start);
            this.groupBox4.Location = new System.Drawing.Point(642, 334);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(756, 378);
            this.groupBox4.TabIndex = 20;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Разгруженные вагоны";
            // 
            // status
            // 
            this.status.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.weight_vag,
            this.sum_vagon});
            this.status.Location = new System.Drawing.Point(3, 353);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(750, 22);
            this.status.TabIndex = 23;
            this.status.Text = "statusStrip1";
            // 
            // weight_vag
            // 
            this.weight_vag.Name = "weight_vag";
            this.weight_vag.Size = new System.Drawing.Size(153, 17);
            this.weight_vag.Text = "Всего разгружено вагонов";
            this.weight_vag.Click += new System.EventHandler(this.toolStripStatusLabel1_Click);
            // 
            // sum_vagon
            // 
            this.sum_vagon.Name = "sum_vagon";
            this.sum_vagon.Size = new System.Drawing.Size(118, 17);
            this.sum_vagon.Text = "toolStripStatusLabel2";
            // 
            // dataGridView_finish
            // 
            this.dataGridView_finish.AllowUserToAddRows = false;
            this.dataGridView_finish.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView_finish.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridView_finish.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_finish.Location = new System.Drawing.Point(19, 57);
            this.dataGridView_finish.Name = "dataGridView_finish";
            this.dataGridView_finish.RowHeadersVisible = false;
            this.dataGridView_finish.Size = new System.Drawing.Size(732, 269);
            this.dataGridView_finish.TabIndex = 0;
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(321, 31);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(175, 23);
            this.button7.TabIndex = 22;
            this.button7.Text = "Текущий месяц";
            this.button7.UseMnemonic = false;
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(188, -2);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(45, 13);
            this.label7.TabIndex = 21;
            this.label7.Text = "Период";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(233, 15);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(38, 13);
            this.label6.TabIndex = 20;
            this.label6.Text = "Конец";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(137, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 13);
            this.label5.TabIndex = 19;
            this.label5.Text = "Начало";
            // 
            // dateTimePicker_finish
            // 
            this.dateTimePicker_finish.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker_finish.Location = new System.Drawing.Point(236, 31);
            this.dateTimePicker_finish.Name = "dateTimePicker_finish";
            this.dateTimePicker_finish.Size = new System.Drawing.Size(79, 20);
            this.dateTimePicker_finish.TabIndex = 18;
            this.dateTimePicker_finish.ValueChanged += new System.EventHandler(this.dateTimePicker_finish_ValueChanged);
            // 
            // dateTimePicker_start
            // 
            this.dateTimePicker_start.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker_start.Location = new System.Drawing.Point(140, 31);
            this.dateTimePicker_start.Name = "dateTimePicker_start";
            this.dateTimePicker_start.Size = new System.Drawing.Size(79, 20);
            this.dateTimePicker_start.TabIndex = 17;
            this.dateTimePicker_start.ValueChanged += new System.EventHandler(this.dateTimePicker_start_ValueChanged);
            // 
            // report
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1410, 714);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Name = "report";
            this.Text = "Краткий отчет по состоянию";
            this.Load += new System.EventHandler(this.report_Load);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView__in_run)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_rail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_in_station)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_in_pru)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.status.ResumeLayout(false);
            this.status.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_finish)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.DataGridView dataGridView__in_run;
        private System.Windows.Forms.DataGridView dataGridView_in_station;
        private System.Windows.Forms.DataGridView dataGridView_in_pru;
        private System.Windows.Forms.DataGridView dataGridView_finish;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dateTimePicker_finish;
        private System.Windows.Forms.DateTimePicker dateTimePicker_start;
        private System.Windows.Forms.StatusStrip status;
        private System.Windows.Forms.ToolStripStatusLabel weight_vag;
        private System.Windows.Forms.ToolStripStatusLabel sum_vagon;
        private System.Windows.Forms.DataGridView dataGridView_rail;
    }
}