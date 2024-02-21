namespace rail
{
    partial class move_silo
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
            this.dataGridView_move = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_move)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView_move
            // 
            this.dataGridView_move.AllowUserToAddRows = false;
            this.dataGridView_move.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView_move.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView_move.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridView_move.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView_move.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_move.GridColor = System.Drawing.SystemColors.Control;
            this.dataGridView_move.Location = new System.Drawing.Point(13, 31);
            this.dataGridView_move.Name = "dataGridView_move";
            this.dataGridView_move.RowHeadersVisible = false;
            this.dataGridView_move.Size = new System.Drawing.Size(775, 407);
            this.dataGridView_move.TabIndex = 0;
            // 
            // move_silo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dataGridView_move);
            this.Name = "move_silo";
            this.Text = "Перемещения по силосам";
            this.Load += new System.EventHandler(this.Move_silo_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_move)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView_move;
    }
}