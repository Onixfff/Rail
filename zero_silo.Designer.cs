namespace rail
{
    partial class zero_silo
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
            this.dataGridView_sero_silo = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_sero_silo)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView_sero_silo
            // 
            this.dataGridView_sero_silo.AllowUserToAddRows = false;
            this.dataGridView_sero_silo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView_sero_silo.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView_sero_silo.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridView_sero_silo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_sero_silo.GridColor = System.Drawing.SystemColors.Control;
            this.dataGridView_sero_silo.Location = new System.Drawing.Point(2, 56);
            this.dataGridView_sero_silo.Name = "dataGridView_sero_silo";
            this.dataGridView_sero_silo.RowHeadersVisible = false;
            this.dataGridView_sero_silo.Size = new System.Drawing.Size(1025, 382);
            this.dataGridView_sero_silo.TabIndex = 0;
            // 
            // zero_silo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1039, 450);
            this.Controls.Add(this.dataGridView_sero_silo);
            this.Name = "zero_silo";
            this.Text = "Обнуление силосов";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Zero_silo_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_sero_silo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView_sero_silo;
    }
}