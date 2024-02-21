namespace rail
{
    partial class pass
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
            this.textBox_pass = new System.Windows.Forms.TextBox();
            this.button_pass = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBox_pass
            // 
            this.textBox_pass.Location = new System.Drawing.Point(34, 49);
            this.textBox_pass.Name = "textBox_pass";
            this.textBox_pass.Size = new System.Drawing.Size(230, 20);
            this.textBox_pass.TabIndex = 0;
            this.textBox_pass.UseSystemPasswordChar = true;
            this.textBox_pass.TextChanged += new System.EventHandler(this.TextBox_pass_TextChanged);
            // 
            // button_pass
            // 
            this.button_pass.Location = new System.Drawing.Point(111, 85);
            this.button_pass.Name = "button_pass";
            this.button_pass.Size = new System.Drawing.Size(75, 23);
            this.button_pass.TabIndex = 1;
            this.button_pass.Text = "ВВОД";
            this.button_pass.UseVisualStyleBackColor = true;
            this.button_pass.Click += new System.EventHandler(this.Button_pass_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(34, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "label1";
            this.label1.Visible = false;
            // 
            // pass
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(308, 120);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button_pass);
            this.Controls.Add(this.textBox_pass);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "pass";
            this.Text = "Пароль";
            this.Enter += new System.EventHandler(this.Pass_Enter);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox textBox_pass;
        public System.Windows.Forms.Button button_pass;
        private System.Windows.Forms.Label label1;
    }
}