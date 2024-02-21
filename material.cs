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
    public partial class material : Form
    {
        MySqlConnection mCon = new MySqlConnection(ConfigurationManager.ConnectionStrings["234"].ConnectionString);
        MySqlCommand msd;
        private int args;
        public material()
        {
            //this.args = args;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void Form6_Load(object sender, EventArgs e)
        {

            string sql = ("SELECT * FROM vagon_material");
            MySqlDataAdapter da = new MySqlDataAdapter(sql, mCon);
            DataSet ds = new DataSet();
            ds.Reset();
            da.Fill(ds, sql);
            dataGridView1.DataSource = ds.Tables[0];
            dataGridView1.AutoResizeColumns();
            dataGridView1.Columns[0].HeaderText = "№ п/п";
            dataGridView1.Columns[1].HeaderText = "Материал";


        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Введите название материвла");
                return;
            }

            string qq = ("  INSERT INTO vagon_material (material) VALUES ('" + textBox1.Text + "')");
            ExecutQuery(qq);
            textBox1.Text = "";
            string sql = ("SELECT * FROM vagon_material");
            MySqlDataAdapter da = new MySqlDataAdapter(sql, mCon);
            DataSet ds = new DataSet();
            ds.Reset();
            da.Fill(ds, sql);
            dataGridView1.DataSource = ds.Tables[0];
            dataGridView1.AutoResizeColumns();

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

        private void button3_Click(object sender, EventArgs e)
        {
            string sql = ("SELECT * FROM vagon_material");
            MySqlDataAdapter da = new MySqlDataAdapter(sql, mCon);
            DataSet ds = new DataSet();
            ds.Reset();
            da.Fill(ds, sql);
            dataGridView1.DataSource = ds.Tables[0];
            dataGridView1.AutoResizeColumns();
        }

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
           
            //textBox2.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //textBox2.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
        }
    }
}
