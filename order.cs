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
using System.Data.Odbc;
using System.Threading;
using DataUpdater;
using System.Configuration;
//using ru.nvg79.connector;

namespace rail
{
    public partial class order : Form
    {
        MySqlConnection mCon = new MySqlConnection(ConfigurationManager.ConnectionStrings["234"].ConnectionString);
        MySqlCommand msd;
        string old;
        public order()
        {
            InitializeComponent();
        }
        private void updateDb()
        {
            string sql = ("SELECT * FROM order_rzd where change_order='False' ORDER BY `date` DESC");
            MySqlDataAdapter dD = new MySqlDataAdapter(sql, mCon);
            DataSet ds = new DataSet();
            ds.Reset();
            dD.Fill(ds, sql);
            dataGridView1.DataSource = ds.Tables[0];
            dataGridView1.AutoResizeColumns();
            dataGridView1.Columns[0].HeaderText = "№ п/п";
            // dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].HeaderText = "Плановая дата выхода";
            dataGridView1.Columns[1].Width = 60;
            dataGridView1.Columns[2].HeaderText = "Отправитель";
            dataGridView1.Columns[2].Width = 120;
            dataGridView1.Columns[3].HeaderText = "Материал";
            dataGridView1.Columns[3].Width = 120;
            dataGridView1.Columns[4].HeaderText = "Вес 1 вагона, т";
            dataGridView1.Columns[4].Width = 60;
            dataGridView1.Columns[5].HeaderText = "Количество вагонов";
            dataGridView1.Columns[5].Width = 60;
            dataGridView1.Columns[6].HeaderText = "Вес партии вагонов, кг";
            dataGridView1.Columns[7].HeaderText = "Фактически вышло";
            dataGridView1.Columns[7].Width = 60;
            dataGridView1.Columns[8].HeaderText = "Заявка изменена";
            dataGridView1.Columns[8].Width = 60;
            dataGridView1.Columns[9].HeaderText = "Старая заявка";
            dataGridView1.Columns[10].HeaderText = "Комментарии";
            dataGridView1.Columns[11].HeaderText = "Дата изменения";

            DateTime date = DateTime.Now - new TimeSpan(24, 0, 0);
            foreach (DataGridViewRow item in dataGridView1.Rows)
            {
                DateTime current;
                if (!DateTime.TryParse(item.Cells[1].Value.ToString(), out current))
                    current = new DateTime();
                if (current < date)
                {
                    int col, fact;
                    if (!int.TryParse(item.Cells[5].Value.ToString(), out col)) col = 0;
                    if (!int.TryParse(item.Cells[7].Value.ToString(), out fact)) fact = 0;
                    if (fact < col)
                        item.DefaultCellStyle.BackColor = Color.Orange;
                }
                if (item.Cells[8].Value.ToString() == "True")
                    item.DefaultCellStyle.BackColor = Color.Red;
                if (item.Cells[5].Value.ToString() == item.Cells[7].Value.ToString())
                    item.DefaultCellStyle.BackColor = Color.GreenYellow;

            }



        }
        private void ReloadFact(int id)
        {
            string sql = ("SELECT id,id_order,number,date,weight FROM vagon_vihod where id_order='" + id + "' ORDER BY id DESC");
            MySqlDataAdapter dD = new MySqlDataAdapter(sql, mCon);
            DataSet ds = new DataSet();
            ds.Reset();
            dD.Fill(ds, sql);
            FactDataGridView.DataSource = ds.Tables[0];
            FactDataGridView.AutoResizeColumns();
            FactDataGridView.Columns[0].Visible = false;
            FactDataGridView.Columns[1].Visible = false;
            FactDataGridView.Columns[2].HeaderText = "№ Вагона";
            FactDataGridView.Columns[3].HeaderText = "Дата отправления";
            FactDataGridView.Columns[4].HeaderText = "Вес вагона, кг";
        }
        private void order_Load(object sender, EventArgs e)
        {
            button4.Click += new EventHandler(button4_firstClick); //подключаем первый обработчик
            updateDb();
            DateTime date = DateTime.Now - new TimeSpan(24, 0, 0);
            foreach (DataGridViewRow item in dataGridView1.Rows)
            {
                DateTime current;
                if (!DateTime.TryParse(item.Cells[1].Value.ToString(), out current))
                    current = new DateTime();
                if (current < date)
                {
                    int col, fact;
                    if (!int.TryParse(item.Cells[5].Value.ToString(), out col)) col = 0;
                    if (!int.TryParse(item.Cells[7].Value.ToString(), out fact)) fact = 0;
                    if (fact < col)
                        item.DefaultCellStyle.BackColor = Color.Orange;
                }
                if (item.Cells[8].Value.ToString() == "True")
                    item.DefaultCellStyle.BackColor = Color.Red;
                if (item.Cells[5].Value.ToString() == item.Cells[7].Value.ToString())
                    item.DefaultCellStyle.BackColor = Color.GreenYellow;

            }
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            sendler costumer = new sendler();
            costumer.Show();

            costumer.dataGridView2.DoubleClick += (senderSlave, eSlave) =>
                 {
                     textBox_costumer.Text = costumer.dataGridView2.SelectedRows[0].Cells[1].Value.ToString();
                     costumer.Close();


                 };
        }

        private void textBox_material_Click(object sender, EventArgs e)
        {
            material material = new material();
            material.Show();
            material.dataGridView1.DoubleClick += (senderSlave, eSlave) =>
            {
                textBox_material.Text = material.dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                material.Close();

            };

        }
        private void OpenCon()
        {
            if (mCon.State == ConnectionState.Closed)
            {
                mCon.Open();
            }
        }
        private void CloseCon()
        {
            if (mCon.State == ConnectionState.Open)
            {
                mCon.Close();
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

        private void button4_firstClick(object sender, EventArgs e)

        {
            if (textBox_costumer.Text == "")
            {
                MessageBox.Show("Введите отправителя");
                return;

            }

            if (textBox_material.Text == "")
            {
                MessageBox.Show("Введите наименование материала");
                return;
            }

            if (textBox_weight.Text == "")
            {
                MessageBox.Show("Введите вес 1-го вагона, т");
                return;
            }

            if (textBox_col.Text == "")
            {
                MessageBox.Show("Введите количество вагонов в партии");
                return;
            }


            else
            {

                double mas;
                if (!double.TryParse(textBox_weight.Text, out mas)) mas = 0;
                double col;
                if (!double.TryParse(textBox_col.Text, out col)) col = 0;
                Dictionary<string, string> str = new Dictionary<string, string>();
                str.Add("`date`", MySQLData.MysqlTime(dateTimePicker1.Value));
                str.Add("costumer", textBox_costumer.Text);
                str.Add("material", textBox_material.Text);
                str.Add("weight_1", textBox_weight.Text);
                str.Add("col", textBox_col.Text);
                str.Add("sum_weight", (mas * col).ToString());
                str.Add("comments", textBoxComments.Text);
                str.Add("date_order", MySQLData.MysqlTime(DateTime.Now));


                //MySQLData.ConvertMysqlTime


                string keys, values;
                MySQLData.ConvertInsertData(str, out keys, out values);
                string strSQL = "insert into order_rzd (" + keys + ") values (" + values + ");";
                //bool isok = false;
                //while (!isok)
                //{
                ExecutQuery(strSQL);
                //MySQLData.GetScalar.Result wres = MySQLData.GetScalar.NoResponse(strSQL, mCon);
                //if (msd.HasError == true)
                //{ isok = false; }
                //else
                //isok = true;
                //updateDb();
                //Close();
                //}
                updateDb();
            }


        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection col = dataGridView1.SelectedRows;
            if (col != null && col.Count > 0)
            {
                int index = Convert.ToInt32(col[0].Cells[0].Value);
                ReloadFact(index);
            }
        }

        private void dataGridView1_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {

        }

        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (dataGridView1.SelectedRows[0].Cells[8].Value.ToString() == "True")
            {
                MessageBox.Show("Невозможно поменять");
                return;
            }

            else
            {
                dateTimePicker1.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                textBox_costumer.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                textBox_material.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                textBox_weight.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
                textBox_col.Text = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
                old = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                button4.Click -= new EventHandler(button4_firstClick); //отключаем первый обработчик
                button4.Click += new EventHandler(button4_secondClick); //подключаем второq обработчик
            }

        }
        private void button4_secondClick(object sender, EventArgs e)

        {

            if (textBox_costumer.Text == "")
            {
                MessageBox.Show("Введите отправителя");
                return;

            }

            if (textBox_material.Text == "")
            {
                MessageBox.Show("Введите наименование материала");
                return;
            }

            if (textBox_weight.Text == "")
            {
                MessageBox.Show("Введите вес 1-го вагона, т");
                return;
            }

            if (textBox_col.Text == "")
            {
                MessageBox.Show("Введите количество вагонов в партии");
                return;
            }


            else
            {

                double mas;
                if (!double.TryParse(textBox_weight.Text, out mas)) mas = 0;
                double col;
                if (!double.TryParse(textBox_col.Text, out col)) col = 0;
                Dictionary<string, string> str = new Dictionary<string, string>();
                str.Add("`date`", MySQLData.MysqlTime(dateTimePicker1.Value));
                str.Add("costumer", textBox_costumer.Text);
                str.Add("material", textBox_material.Text);
                str.Add("weight_1", textBox_weight.Text);
                str.Add("col", textBox_col.Text);
                str.Add("sum_weight", (mas * col).ToString());
                str.Add("old", old);

                str.Add("comments", textBoxComments.Text);
                str.Add("date_order", MySQLData.MysqlTime(DateTime.Now));


                //MySQLData.ConvertMysqlTime

                string keys, values;
                MySQLData.ConvertInsertData(str, out keys, out values);
                string strSQL = "insert into order_rzd (" + keys + ") values (" + values + ");";
                //bool isok = false;
                //while (!isok)
                //{
                ExecutQuery(strSQL);
                //MySQLData.GetScalar.Result wres = MySQLData.GetScalar.NoResponse(strSQL, mCon);
                //if (msd.HasError == true)
                //{ isok = false; }
                //else
                //isok = true;
                //updateDb();
                //Close();
                //}
                string strSQL3 = "update order_rzd set change_order ='1', date_order = '" + MySQLData.MysqlTime(DateTime.Now) + "' where `id`='" + old + "' ;";
                ExecutQuery(strSQL3);
                updateDb();
            }


        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void отменитьToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (dataGridView1.SelectedRows[0].Cells[8].Value.ToString() == "True")
            {
                MessageBox.Show("Невозможно поменять");
                return;
            }

            else
            {

                //dateTimePicker1.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                // textBox_costumer.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                //textBox_material.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                //textBox_weight.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
                // textBox_col.Text = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
                old = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                // button4.Click -= new EventHandler(button4_firstClick); //отключаем первый обработчик
                //button4.Click += new EventHandler(button4_secondClick); //подключаем второq обработчик
            }
            DialogResult result = MessageBox.Show("Удалить заявку", "Предупреждение", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                string strSQL3 = "update order_rzd set change_order ='1', date_order = '" + MySQLData.MysqlTime(DateTime.Now) + "' where `id`='" + old + "' ;";
                ExecutQuery(strSQL3);
                updateDb();
            }
            else return;


        }

        private void посмотретьСтаруюЗаявкуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            old = dataGridView1.SelectedRows[0].Cells[9].Value.ToString();

            string sql = ("SELECT * FROM order_rzd where id='" + old + "' or old='" + old + "'");
            MySqlDataAdapter dD = new MySqlDataAdapter(sql, mCon);
            DataSet ds = new DataSet();
            ds.Reset();
            dD.Fill(ds, sql);
            dataGridView1.DataSource = ds.Tables[0];
            dataGridView1.AutoResizeColumns();
            dataGridView1.Columns[0].HeaderText = "№ п/п";
            // dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].HeaderText = "Плановая дата выхода";
            dataGridView1.Columns[1].Width = 60;
            dataGridView1.Columns[2].HeaderText = "Отправитель";
            dataGridView1.Columns[2].Width = 120;
            dataGridView1.Columns[3].HeaderText = "Материал";
            dataGridView1.Columns[3].Width = 120;
            dataGridView1.Columns[4].HeaderText = "Вес 1 вагона, т";
            dataGridView1.Columns[4].Width = 60;
            dataGridView1.Columns[5].HeaderText = "Количество вагонов";
            dataGridView1.Columns[5].Width = 60;
            dataGridView1.Columns[6].HeaderText = "Вес партии вагонов, кг";
            dataGridView1.Columns[7].HeaderText = "Фактически вышло";
            dataGridView1.Columns[7].Width = 60;
            dataGridView1.Columns[8].HeaderText = "Заявка изменена";
            dataGridView1.Columns[8].Width = 60;
            dataGridView1.Columns[9].HeaderText = "Старая заявка";
            dataGridView1.Columns[10].HeaderText = "Комментарии";
            dataGridView1.Columns[11].HeaderText = "Дата изменения";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            updateDb();

        }
    }
}
