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
using DataUpdater;
using System.Configuration;

namespace rail
{
    public partial class Add : Form
    {
        string Connection = ConfigurationManager.ConnectionStrings["234"].ConnectionString;
        MySqlConnection mcon;
        MySqlCommand msd;
        public Add()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (NumberBox.Text == "")
            {
                MessageBox.Show("Введите номер вагона");
                return;

            }

            if (SenderBox.Text == "")
            {
                MessageBox.Show("Введите отправителя");
                return;
            }

            if (CosrunersBox.Text == "")
            {
                MessageBox.Show("Введите получателя");
                return;
            }

            if (MaterialBox.Text == "")
            {
                MessageBox.Show("Введите Материал");
                return;
            }

            if (WeightBox.Text == "")
            {
                MessageBox.Show("Введите вес вагона");
                return;
            }
            if (textBox_order.Text == "")
            {
                MessageBox.Show("Введите номер заявки");
                return;
            }
            else
            {
                string data = dateTimePicker1.Value.ToString("yyyy-MM-dd HH:mm");
                string q = ("INSERT INTO vagon_vihod (id_order, number,sender,costumer,material, weight, date) VALUE ('" + textBox_order.Text + "','" + NumberBox.Text + "', '" + SenderBox.Text + "', '" + CosrunersBox.Text + "','" + MaterialBox.Text + "','" + WeightBox.Text + "','" + data + "')");
                ExecutQuery(q);
                int id; if (!int.TryParse(textBox_order.Text, out id)) id = 0;
                MySQLData.GetScalar.Result result = MySQLData.GetScalar.Scalar("select count(id) from vagon_vihod where id_order='" + id + "';", Connection);
                if (!result.HasError)
                {
                    int count; if (!int.TryParse(result.ResultText, out count)) count = 0;
                    MySQLData.GetScalar.NoResponse("update order_rzd set fact='" + count + "' where id='" + id + "';", Connection);
                }
            }
            NumberBox.Text = "";
            SenderBox.Text = "";
            CosrunersBox.Text = "";
            MaterialBox.Text = "";
            WeightBox.Text = "";
            textBox_order.Text = "";

        }
        private void OpenCon()
        {
            if (mcon.State == ConnectionState.Closed)
            {
                mcon.Open();
            }
        }
        private void CloseCon()
        {
            if (mcon.State == ConnectionState.Open)
            {
                mcon.Close();
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

        private void SenderBox_TextChanged(object sender, EventArgs e)
        {


        }

        private void SenderBox_Click(object sender, EventArgs e)
        {
            sendler form4 = new sendler();
            form4.Show();
            form4.dataGridView2.MouseDoubleClick += (senderSlave, eSlave) =>
            {
                SenderBox.Text = form4.dataGridView2.SelectedRows[0].Cells[1].Value.ToString();
                form4.Close();
            };
            form4.butenterF4.Click += (senderSlave, eSlave) =>
            {
                this.SenderBox.Text = form4.dataGridView2.SelectedRows[0].Cells[1].Value.ToString();
                form4.Close();
            };


        }

        private void CosrunersBox_Click(object sender, EventArgs e)
        {
            Form5 form5 = new Form5();
            form5.Show();
            form5.dataGridView1.MouseDoubleClick += (senderSlave, eSlave) =>
               {
                   this.CosrunersBox.Text = form5.dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                   form5.Close();
               };
            form5.button2.Click += (senderSlave, eSlave) =>
            {
                this.CosrunersBox.Text = form5.dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                form5.Close();
            };
        }

        private void CosrunersBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void MaterialBox_Click(object sender, EventArgs e)
        {
            material form6 = new material();
            form6.Show();
            form6.dataGridView1.MouseDoubleClick += (senderSlave, eSlave) =>
            {
                MaterialBox.Text = form6.dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                form6.Close();
            };
            form6.button2.Click += (senderSlave, eSlave) =>
            {
                MaterialBox.Text = form6.dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                form6.Close();
            };
        }

        private void Add_Load(object sender, EventArgs e)
        {
            mcon = new MySqlConnection(Connection);
        }

        private void textBox_order_Click(object sender, EventArgs e)
        {
            order order = new order();
            order.Show();
            order.dataGridView1.DoubleClick += (senderSlave, eSlave) =>
            {
                if (order.dataGridView1.SelectedRows[0].Cells[8].Value.ToString() == "True")
                {
                    MessageBox.Show("Нельзя выбрать отмененную заявку");
                    return;
                }
                else
                {
                    textBox_order.Text = order.dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                    order.Close();
                }



            };
        }

    }
}


