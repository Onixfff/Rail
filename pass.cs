using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rail
{
    public partial class pass : Form
    {
        public pass()
        {
            InitializeComponent();
        }

        private void Button_pass_Click(object sender, EventArgs e)
        {
            if (textBox_pass.Text != "08082014")
            {
                label1.Visible = true;
                label1.Text = "неверный пароль";
                //textBox_pass.Text = "";
            }
            
        }

        private void TextBox_pass_TextChanged(object sender, EventArgs e)
        {
            label1.Visible = false;
            label1.Text = "";
        }

        private void Pass_Enter(object sender, EventArgs e)
        {

            if (textBox_pass.Text != "08082014")
            {
                label1.Visible = true;
                label1.Text = "неверный пароль";
                //textBox_pass.Text = "";
            }
        }
    }
}
