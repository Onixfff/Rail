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
using System.Threading;
using System.Configuration;


namespace rail
{
    public partial class datestation : Form
    {
        MySqlConnection mCon = new MySqlConnection(ConfigurationManager.ConnectionStrings["234"].ConnectionString);
        MySqlCommand msd;
        public datestation()
        {
            InitializeComponent();
            //backgroundWorker1.RunWorkerAsync();
        }

        private void Form7_Load(object sender, EventArgs e)
        {

        }

        public void button1_Click(object sender, EventArgs e)
        {
            string sql = ("SELECT * FROM vagon_vihod WHERE prihod ='0' ");
            MySqlDataAdapter da = new MySqlDataAdapter(sql, mCon);
            DataSet ds = new DataSet();
            ds.Reset();
            da.Fill(ds, sql);
            dataGridView1.DataSource = ds.Tables[0];
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns["costumer"].Visible = false;
            dataGridView1.Columns["weight"].Visible = false;
            dataGridView1.Columns["data_prihod"].Visible = false;
            dataGridView1.Columns["prihod"].Visible = false;
            dataGridView1.Columns["partia"].Visible = false;
            dataGridView1.Columns["data_start"].Visible = false;
            dataGridView1.Columns["data_finish"].Visible = false;
            dataGridView1.Columns["status_start"].Visible = false;
            dataGridView1.Columns["status_stop"].Visible = false;
            dataGridView1.Columns[1].HeaderText = "№ вагона";
            dataGridView1.Columns["sender"].HeaderText = "Отправитель";
            dataGridView1.Columns["material"].HeaderText = "Материал";
            dataGridView1.Columns["date"].HeaderText = "Дата выхода вагона";
            dataGridView1.Columns[1].Width = 150;
            dataGridView1.Columns["sender"].Width = 240;
            dataGridView1.Columns["material"].Width = 180;
            dataGridView1.Columns["date"].Width = 150;
            textBox1.Text = dateTimePicker1.Value.ToString("HH:mm dd-MM-yyyy");


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
                if (msd.ExecuteNonQuery() >= 1)
                {
                    MessageBox.Show("Записи/изменения добавлены");
                    string sql = ("SELECT * FROM vagon_vihod WHERE prihod ='0' ");
                    MySqlDataAdapter da = new MySqlDataAdapter(sql, mCon);
                    DataSet ds = new DataSet();
                    ds.Reset();
                    da.Fill(ds, sql);
                    dataGridView1.DataSource = ds.Tables[0];
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        public void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            string date_prihod = dateTimePicker1.Value.ToString("yyyy-MM-dd HH-mm");
            string id = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            string q = ("UPDATE vagon_vihod SET prihod ='1', data_prihod='" + date_prihod + "' WHERE id='" + id + "'");
            ExecutQuery(q);

        }

        private void Form7_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // string q = ("INSERT INTO vagon_vihod (number,sender,costumer,material, weight) VALUE ('" +1+ "', '"+ 1+ "', '" +1+ "','"+ 1+ "','"+ 1+ "')");
            // ExecutQuery(q);
        }

        public void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            /*while (true)
            {
                DataSet();
                Thread.Sleep(100);
            }*/
        }
        private void DataSet()
        {
            /*string q = ("INSERT INTO vagon_vihod (number,sender,costumer,material, weight) VALUE ('" + 1 + "', '" + 1 + "', '" + 1 + "','" + 1 + "','" + 1 + "')");
           ExecutQuery(q);*/
        }

    }
}
