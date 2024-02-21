namespace rail
{
    partial class sendler
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(sendler));
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.button1 = new System.Windows.Forms.Button();
            this.butdeletF4 = new System.Windows.Forms.Button();
            this.butenterF4 = new System.Windows.Forms.Button();
            this.butaddF4 = new System.Windows.Forms.Button();
            this.sendlerBox1 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView2
            // 
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.AllowUserToDeleteRows = false;
            this.dataGridView2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dataGridView2.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridView2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.GridColor = System.Drawing.SystemColors.Control;
            this.dataGridView2.Location = new System.Drawing.Point(3, 108);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.ReadOnly = true;
            this.dataGridView2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView2.Size = new System.Drawing.Size(770, 309);
            this.dataGridView2.TabIndex = 3;
            this.dataGridView2.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView2.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.dataGridView2_MouseDoubleClick);
            // 
            // button1
            // 
            this.button1.Image = global::rail.Properties.Resources.Clipboard_Refresh;
            this.button1.Location = new System.Drawing.Point(117, 6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(36, 36);
            this.button1.TabIndex = 4;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // butdeletF4
            // 
            this.butdeletF4.Image = global::rail.Properties.Resources.Clipboard_Delete;
            this.butdeletF4.Location = new System.Drawing.Point(63, 6);
            this.butdeletF4.Name = "butdeletF4";
            this.butdeletF4.Size = new System.Drawing.Size(36, 36);
            this.butdeletF4.TabIndex = 2;
            this.butdeletF4.UseVisualStyleBackColor = true;
            this.butdeletF4.Click += new System.EventHandler(this.button3_Click);
            // 
            // butenterF4
            // 
            this.butenterF4.Image = global::rail.Properties.Resources.Clipboard_Check;
            this.butenterF4.Location = new System.Drawing.Point(12, 6);
            this.butenterF4.Name = "butenterF4";
            this.butenterF4.Size = new System.Drawing.Size(36, 36);
            this.butenterF4.TabIndex = 1;
            this.butenterF4.UseVisualStyleBackColor = true;
            this.butenterF4.Click += new System.EventHandler(this.butenterF4_Click);
            // 
            // butaddF4
            // 
            this.butaddF4.BackgroundImage = global::rail.Properties.Resources.Clipboard_Add;
            this.butaddF4.ForeColor = System.Drawing.SystemColors.Control;
            this.butaddF4.Location = new System.Drawing.Point(479, 6);
            this.butaddF4.Name = "butaddF4";
            this.butaddF4.Size = new System.Drawing.Size(36, 36);
            this.butaddF4.TabIndex = 0;
            this.butaddF4.UseVisualStyleBackColor = true;
            this.butaddF4.Click += new System.EventHandler(this.butaddF4_Click);
            // 
            // sendlerBox1
            // 
            this.sendlerBox1.Location = new System.Drawing.Point(459, 48);
            this.sendlerBox1.Name = "sendlerBox1";
            this.sendlerBox1.Size = new System.Drawing.Size(129, 20);
            this.sendlerBox1.TabIndex = 5;
            this.sendlerBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(459, 74);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(129, 20);
            this.textBox1.TabIndex = 6;
            this.textBox1.Text = "Отправитель";
            this.textBox1.Visible = false;
            // 
            // sendler
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(798, 419);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.sendlerBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dataGridView2);
            this.Controls.Add(this.butdeletF4);
            this.Controls.Add(this.butenterF4);
            this.Controls.Add(this.butaddF4);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "sendler";
            this.Text = "Отправитель";
            this.Load += new System.EventHandler(this.Form4_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button butaddF4;
        private System.Windows.Forms.Button butdeletF4;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox sendlerBox1;
        public System.Windows.Forms.TextBox textBox1;
        public System.Windows.Forms.DataGridView dataGridView2;
        public System.Windows.Forms.Button butenterF4;
    }
}