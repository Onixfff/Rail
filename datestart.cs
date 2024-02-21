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
    public partial class datestart : Form
    {
        MySqlConnection mCon = new MySqlConnection(ConfigurationManager.ConnectionStrings["234"].ConnectionString);
        MySqlCommand msd;
        public datestart()
        {
            InitializeComponent();
        }

        private void Form8_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string sql = ("SELECT * FROM vagon_vihod WHERE status_start ='0' and prihod='1' ");
            MySqlDataAdapter da = new MySqlDataAdapter(sql, mCon);
            DataSet ds = new DataSet();
            ds.Reset();
            da.Fill(ds, sql);
            dataGridView1.DataSource = ds.Tables[0];
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns["costumer"].Visible = false;
            dataGridView1.Columns["weight"].Visible = false;
            dataGridView1.Columns["date"].Visible = false;
            dataGridView1.Columns["prihod"].Visible = false;
            dataGridView1.Columns["data_start"].Visible = false;
            dataGridView1.Columns["data_finish"].Visible = false;
            dataGridView1.Columns["status_stop"].Visible = false;
            dataGridView1.Columns["partia"].Visible = false;
            dataGridView1.Columns["status_start"].HeaderText = "Статус постановки на разгрузку";
            dataGridView1.Columns[1].HeaderText = "№ вагона";
            dataGridView1.Columns["sender"].HeaderText = "Отправитель";
            dataGridView1.Columns["material"].HeaderText = "Материал";
            dataGridView1.Columns["data_prihod"].HeaderText = "Дата прихода на станцию вагона";
            dataGridView1.Columns[1].Width = 150;
            dataGridView1.Columns["sender"].Width = 240;
            dataGridView1.Columns["status_start"].Width = 80;
            dataGridView1.Columns["material"].Width = 180;
            dataGridView1.Columns["date"].Width = 150;
            textBox1.Text = dateTimePicker1.Value.ToString("HHmm-ddMMyyyy");
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
                    string sql = ("SELECT * FROM vagon_vihod WHERE prihod ='1' and status_start='0'");
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

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            string date_start = dateTimePicker1.Value.ToString("yyyy-MM-dd HH-mm");
            string id = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            string q = ("UPDATE vagon_vihod SET status_start ='1', data_start='" + date_start + "', partia='" + textBox1.Text + "' WHERE id='" + id + "'");
            ExecutQuery(q);
        }

        private void Form8_FormClosed(object sender, FormClosedEventArgs e)
        {

        }
    }
}

