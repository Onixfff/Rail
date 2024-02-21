using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace rail
{
    public partial class Form5 : Form
    {
        MySqlConnection mCon = new MySqlConnection(ConfigurationManager.ConnectionStrings["234"].ConnectionString);
        MySqlCommand msd;
        string add;

        public Form5()
        {
            // this.add = add;
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            string qq = ("INSERT INTO vagon_costumers (costumers, adress) VALUES ('" + textBox1.Text + "', '" + textBox2.Text + "')");
            ExecutQuery(qq);
            textBox1.Text = "";
            textBox2.Text = "";
            dataGridView1.AutoResizeColumns();
            string sql = ("SELECT * FROM vagon_costumers");
            MySqlDataAdapter da = new MySqlDataAdapter(sql, mCon);
            DataSet ds = new DataSet();
            ds.Reset();
            da.Fill(ds, sql);
            dataGridView1.DataSource = ds.Tables[0];

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form5_Load(object sender, EventArgs e)
        {
            string sql = ("SELECT * FROM vagon_costumers");
            MySqlDataAdapter da = new MySqlDataAdapter(sql, mCon);
            DataSet ds = new DataSet();
            ds.Reset();
            da.Fill(ds, sql);
            dataGridView1.DataSource = ds.Tables[0];
            dataGridView1.AutoResizeColumns();
            dataGridView1.Columns[0].HeaderText = "№ п/п";
            dataGridView1.Columns[1].HeaderText = "Фирма";
            dataGridView1.Columns[2].HeaderText = "Адрес";
        }
        private void CloseCon()
        {
            if (mCon.State == ConnectionState.Open)
            {
                mCon.Close();
            }
        }
        private void OpenCon()
        {
            if (mCon.State == ConnectionState.Closed)
            {
                mCon.Open();
            }
        }
        public void ExecutQuery(string q)
        {
            try
            {
                OpenCon();
                msd = new MySqlCommand(q, mCon);
                if (msd.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Запись добавлена");
                }
                else
                {
                    MessageBox.Show("Ошибка записи");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally { mCon.Close(); }
        }

        public void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            textBox3.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
        }

        public void button2_Click(object sender, EventArgs e)
        {
            textBox3.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
        }

    }
}
