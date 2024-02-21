namespace rail
{
    partial class Add
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Add));
            this.NumVag = new System.Windows.Forms.Label();
            this.NumberBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.SenderBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.CosrunersBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.MaterialBox = new System.Windows.Forms.TextBox();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.WeightBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox_order = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // NumVag
            // 
            this.NumVag.AutoSize = true;
            this.NumVag.Location = new System.Drawing.Point(13, 13);
            this.NumVag.Name = "NumVag";
            this.NumVag.Size = new System.Drawing.Size(56, 13);
            this.NumVag.TabIndex = 0;
            this.NumVag.Text = "№ вагона";
            // 
            // NumberBox
            // 
            this.NumberBox.Location = new System.Drawing.Point(123, 13);
            this.NumberBox.Name = "NumberBox";
            this.NumberBox.Size = new System.Drawing.Size(123, 20);
            this.NumberBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 123);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Дата выхода";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(123, 123);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(123, 20);
            this.dateTimePicker1.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 166);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Отправитель";
            // 
            // SenderBox
            // 
            this.SenderBox.Location = new System.Drawing.Point(123, 166);
            this.SenderBox.Name = "SenderBox";
            this.SenderBox.Size = new System.Drawing.Size(123, 20);
            this.SenderBox.TabIndex = 5;
            this.SenderBox.Click += new System.EventHandler(this.SenderBox_Click);
            this.SenderBox.TextChanged += new System.EventHandler(this.SenderBox_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 217);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Получатель";
            // 
            // CosrunersBox
            // 
            this.CosrunersBox.Location = new System.Drawing.Point(123, 217);
            this.CosrunersBox.Name = "CosrunersBox";
            this.CosrunersBox.Size = new System.Drawing.Size(123, 20);
            this.CosrunersBox.TabIndex = 7;
            this.CosrunersBox.Click += new System.EventHandler(this.CosrunersBox_Click);
            this.CosrunersBox.TextChanged += new System.EventHandler(this.CosrunersBox_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 263);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(30, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Груз";
            // 
            // MaterialBox
            // 
            this.MaterialBox.Location = new System.Drawing.Point(123, 263);
            this.MaterialBox.Name = "MaterialBox";
            this.MaterialBox.Size = new System.Drawing.Size(123, 20);
            this.MaterialBox.TabIndex = 9;
            this.MaterialBox.Click += new System.EventHandler(this.MaterialBox_Click);
            // 
            // buttonAdd
            // 
            this.buttonAdd.Location = new System.Drawing.Point(19, 357);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(75, 23);
            this.buttonAdd.TabIndex = 10;
            this.buttonAdd.Text = "Добавить";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.button1_Click);
            // 
            // WeightBox
            // 
            this.WeightBox.Location = new System.Drawing.Point(123, 304);
            this.WeightBox.Name = "WeightBox";
            this.WeightBox.Size = new System.Drawing.Size(123, 20);
            this.WeightBox.TabIndex = 12;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 304);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Вес вагона, т";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 79);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(104, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "№ заявки по плану";
            // 
            // textBox_order
            // 
            this.textBox_order.Location = new System.Drawing.Point(122, 79);
            this.textBox_order.Name = "textBox_order";
            this.textBox_order.Size = new System.Drawing.Size(123, 20);
            this.textBox_order.TabIndex = 1;
            this.textBox_order.Click += new System.EventHandler(this.textBox_order_Click);
            // 
            // Add
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(329, 459);
            this.Controls.Add(this.WeightBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.MaterialBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.CosrunersBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.SenderBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_order);
            this.Controls.Add(this.NumberBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.NumVag);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Add";
            this.Text = "Добавить Вагон";
            this.Load += new System.EventHandler(this.Add_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label NumVag;
        private System.Windows.Forms.TextBox NumberBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox CosrunersBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox MaterialBox;
        public System.Windows.Forms.TextBox SenderBox;
        private System.Windows.Forms.TextBox WeightBox;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox_order;
    }
}