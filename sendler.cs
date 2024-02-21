using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using MySql;
using System.Configuration;

namespace rail
{
    public partial class sendler : Form
    {
        MySqlConnection mcon = new MySqlConnection(ConfigurationManager.ConnectionStrings["234"].ConnectionString);
        MySqlCommand msd;

        public sendler()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void Form4_Load(object sender, EventArgs e)
        {
            string sql = ("SELECT * FROM vagon_sendler");
            MySqlDataAdapter da = new MySqlDataAdapter(sql, mcon);
            DataSet ds = new DataSet();
            ds.Reset();
            da.Fill(ds, sql);
            dataGridView2.DataSource = ds.Tables[0];
            dataGridView2.Columns[0].HeaderText = "№ п/п";
            dataGridView2.Columns[1].HeaderText = "Отправитель";
            dataGridView2.AutoResizeColumns();

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Refresh();
            string sql = ("SELECT * FROM vagon_sendler");
            MySqlDataAdapter da = new MySqlDataAdapter(sql, mcon);
            DataSet ds = new DataSet();
            ds.Reset();
            da.Fill(ds, sql);
            dataGridView2.DataSource = ds.Tables[0];
            dataGridView2.AutoResizeColumns();
        }

        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {

        }

        private void butaddF4_Click(object sender, EventArgs e)
        {

            string qq = ("  INSERT INTO vagon_sendler (sendler) VALUES ('" + sendlerBox1.Text + "')");
            ExecutQuery(qq);
            string sql = ("SELECT * FROM vagon_sendler");
            MySqlDataAdapter da = new MySqlDataAdapter(sql, mcon);
            DataSet ds = new DataSet();
            ds.Reset();
            da.Fill(ds, sql);
            dataGridView2.DataSource = ds.Tables[0];


        }
        private void CloseCon()
        {
            if (mcon.State == ConnectionState.Open)
            {
                mcon.Close();
            }
        }
        private void OpenCon()
        {
            if (mcon.State == ConnectionState.Closed)
            {
                mcon.Open();
            }
        }
        public void ExecutQuery(string q)
        {
            try
            {
                OpenCon();
                msd = new MySqlCommand(q, mcon);
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
            finally { mcon.Close(); }
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        public void dataGridView2_MouseDoubleClick(object sender, MouseEventArgs e)
        {

            // textBox1.Text = dataGridView2.SelectedRows[0].Cells[1].Value.ToString();

        }

        private void butenterF4_Click(object sender, EventArgs e)
        {
            //textBox1.Text = dataGridView2.SelectedRows[0].Cells[1].Value.ToString();
        }
    }
}
