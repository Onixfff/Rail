﻿using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Threading;
using System.Configuration;
using System.Threading.Tasks;
//using ru.nvg79.connector;


namespace rail
{
    public partial class general : Form
    {
        //MySqlConnection mCon = new MySqlConnection("Database=u0550310_aeroblock; Server=127.0.0.1; port=3306; username=root; password=20112004; charset=utf8 ");
        //MySqlConnection mCon = new MySqlConnection("Database=u0550310_aeroblock; Server=31.31.196.234; port=3306; username=u0550_kornev; password=18061981Kornev; charset=utf8 ");
        MySqlConnection mCon = new MySqlConnection(ConfigurationManager.ConnectionStrings["234"].ConnectionString);
        MySqlCommand msd;


        public general()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Paint(object sender, PaintEventArgs e)
        {

        }

        /*  private DataTable GetComments()
          {
              DataTable dt = new DataTable();

              MySqlConnectionStringBuilder mysqlCSB;
              mysqlCSB = new MySqlConnectionStringBuilder();
              mysqlCSB.Server = "127.0.0.1";
              mysqlCSB.Database = "dfg";
              mysqlCSB.UserID = "root";
              mysqlCSB.Password = "12345";

              string queryString = @"SELECT number              
                            FROM   vagon 
                            WHERE  id >=(0)";

              using (MySqlConnection con = new MySqlConnection())
              {
                  con.ConnectionString = mysqlCSB.ConnectionString;

                  MySqlCommand com = new MySqlCommand(queryString, con);

                  try
                  {
                      con.Open();

                      using (MySqlDataReader dr = com.ExecuteReader())
                      {
                          if (dr.HasRows)
                          {
                              dt.Load(dr);
                          }
                      }
                  }

                  catch (Exception ex)
                  {
                      MessageBox.Show(ex.Message);
                  }
              }
              return dt;
          }*/

        private void button1_Click_1(object sender, EventArgs e)
        {
            /* string q = ("INSERT INTO vagon (number, sendler) VALUES ('"+number.Text+"','"+sendler.Text+"') ");
             ExecutQuery(q);*/
        }

        /* public void OpenCon()
         { 
         if (mcon.State== ConnectionState.Closed)
         {
             mcon.Open();
             }
         }
         public void CloseCon()
         {
             if(mcon.State==ConnectionState.Open)
             {
                 mcon.Close();
             }
         }
         public void ExecutQuery(string q)
         { try
               {
                   OpenCon();
             msd=new MySqlCommand(q, mcon);
             if (msd.ExecuteNonQuery()==1)
             {
                 MessageBox.Show("Запись добавлена");
             }
             else
             {
                 MessageBox.Show("Ошибка записи");
             }

                  }
        catch(Exception ex)
              {
                  MessageBox.Show(ex.Message);
                }
         finally { mcon.Close(); }
         }*/

        private void button1_Click_2(object sender, EventArgs e)
        {
            Add add = new Add();
            add.Show();
            //add.buttonAdd.Click += (senderSlave, eSlave) =>
            //    {
            //Method();
            //        sum();


            //    };
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }
        
        private void InitializeDatePickers()
        {
            int mounth = DateTime.Now.Month;
            int year = DateTime.Now.Year;
            DateTime date = new DateTime(year, mounth, 1);
            int days = DateTime.DaysInMonth(year, mounth);
            DateTime date2 = new DateTime(year, mounth, days);

            // Отключаем обработчики событий
            dateTimePicker_start.ValueChanged -= dateTimePicker_start_ValueChanged;
            dateTimePicker_finish.ValueChanged -= dateTimePicker_finish_ValueChanged;

            // Устанавливаем значения
            dateTimePicker_start.Text = date.ToString();
            dateTimePicker_finish.Text = date2.ToString();

            // Включаем обработчики событий
            dateTimePicker_start.ValueChanged += dateTimePicker_start_ValueChanged;
            dateTimePicker_finish.ValueChanged += dateTimePicker_finish_ValueChanged;

            // Вызываем Sum один раз
            Sum();
        }
        
        private void update()
        {
            using (var mConDemo = new MySqlConnection(ConfigurationManager.ConnectionStrings["234"].ConnectionString)) 
            {
                try
                {
                    if(mConDemo.State == ConnectionState.Closed)
                        mConDemo.Open();

                    string sql = ("SELECT * FROM vagon_vihod where status_stop=0 ORDER BY id DESC");
                    using (MySqlDataAdapter dD = new MySqlDataAdapter(sql, mConDemo))
                    {
                        using (DataSet ds = new DataSet())
                        {
                            ds.Reset();
                            dD.Fill(ds, sql);
                            dataGridView1.DataSource = ds.Tables[0];
                            dataGridView1.AutoResizeColumns();
                            dataGridView1.Columns[0].HeaderText = "№ п/п";
                            dataGridView1.Columns[0].Visible = false;
                            dataGridView1.Columns[1].HeaderText = "№ Заявки";
                            dataGridView1.Columns[2].HeaderText = "№ Вагона";
                            dataGridView1.Columns[3].HeaderText = "Отправитель";
                            dataGridView1.Columns[3].Width = 120;
                            dataGridView1.Columns[4].HeaderText = "Получатель";
                            dataGridView1.Columns[4].Width = 120;
                            dataGridView1.Columns[5].HeaderText = "Дата отправления";
                            dataGridView1.Columns[6].HeaderText = "Материал";
                            dataGridView1.Columns[7].HeaderText = "Вес вагона, кг";
                            dataGridView1.Columns[8].HeaderText = "Дата прихода на станцию";
                            dataGridView1.Columns[9].HeaderText = "Статус прихода на станцию";
                            dataGridView1.Columns[9].Width = 60;
                            dataGridView1.Columns[10].HeaderText = "Партия разгрузки";
                            dataGridView1.Columns[11].HeaderText = "Дата постановки на разгрузку";
                            dataGridView1.Columns[13].HeaderText = "Статус постановки на разгрузку";
                            dataGridView1.Columns[12].HeaderText = "Дата окончания выгрузки";
                            dataGridView1.Columns[14].HeaderText = "Статус окончания выгрузки";
                            dataGridView1.Columns[15].HeaderText = "№ силоса";
                            dataGridView1.Columns[15].Width = 60;
                            dataGridView1.Columns[16].HeaderText = "Время выгрузки партии";
                            dataGridView1.Columns[16].Width = 60;
                            dataGridView1.Columns[17].HeaderText = "Время выгрузки вагона";
                        }
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}\n{ex.StackTrace}");
                    MessageBox.Show($"Ошибка: {ex.Message}\n{ex.StackTrace}");
                }

                Console.WriteLine("update complite");
                this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            }
        }

        private void Form2_Load_1(object sender, EventArgs e)
        {
#if OPER
            button1.Visible = false;
            button5.Visible = false;
            button2.Visible = false;
            button4.Visible = false;

#endif
            InitializeDatePickers();
            comboBox1.Text = "Все";
            Task.Delay(100);
            update();
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            datestation form7 = new datestation();
            form7.Show();
            form7.FormClosed += async (senderSlave, eSlave) =>
                {
                    Method();
                    Sum();
                };
        }

        private void button3_Click(object sender, EventArgs e)
        {
            datestart form = new datestart();
            form.Show();
            form.FormClosed += async (senderSlave, eSlave) =>
            {
                Method();
                Sum();
            };
        }

        private void button4_Click(object sender, EventArgs e)
        {
            report form = new report();
            form.Show();
        }

        //private void dataGridView1_CellFormatting_1(object sender, DataGridViewCellFormattingEventArgs e)
        //{

        //}

        private void dataGridView1_RowPrePaint_1(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (e.RowIndex > -1 && e.RowIndex <= dataGridView1.RowCount - 1)
                //вагон на станции            
                if (dataGridView1.Rows[e.RowIndex].Cells[9].Value.ToString() == "True")//сравнение что вагон на станции
                    if (dataGridView1.Rows[e.RowIndex].Cells[13].Value.ToString() == "False")//сравнение вагон на разгрузке
                        if (dataGridView1.Rows[e.RowIndex].Cells[14].Value.ToString() == "False")//сравнение вагон разгружен
                            ((DataGridView)sender).Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightPink;
            //вагон на разгрузке
            if (dataGridView1.Rows[e.RowIndex].Cells[9].Value.ToString() == "True")//сравнение что вагон на станции
                if (dataGridView1.Rows[e.RowIndex].Cells[13].Value.ToString() == "True")//сравнение вагон на разгрузке
                    if (dataGridView1.Rows[e.RowIndex].Cells[14].Value.ToString() == "False")//сравнение вагон разгружен
                        ((DataGridView)sender).Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightBlue;
            //вагон разгружен
            if (dataGridView1.Rows[e.RowIndex].Cells[9].Value.ToString() == "True")//сравнение что вагон на станции
                if (dataGridView1.Rows[e.RowIndex].Cells[13].Value.ToString() == "True")//сравнение вагон на разгрузке
                    if (dataGridView1.Rows[e.RowIndex].Cells[14].Value.ToString() == "True")//сравнение вагон разгружен
                        ((DataGridView)sender).Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGreen;

        }

        public void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Method();
        }

        private void Method()
        {
            try
            {
                string rt = comboBox1.SelectedItem.ToString();
                using (var mConDemo = new MySqlConnection(ConfigurationManager.ConnectionStrings["234"].ConnectionString)) 
                {
                    mConDemo.Open();
                    switch (rt)
                    {
                        case "Вагоны на станции":
                            var sql = ("SELECT * FROM vagon_vihod WHERE prihod='1' and status_start='0' and status_stop='0' ORDER BY id DESC ");
                            MySqlDataAdapter dD = new MySqlDataAdapter(sql, mConDemo);
                            DataSet ds = new DataSet();
                            ds.Reset();
                            dD.Fill(ds, sql);
                            dataGridView1.DataSource = ds.Tables[0];
                            break;
                        case "Вагоны в пути":
                            sql = ("SELECT * FROM vagon_vihod WHERE prihod='0' and status_start='0' and status_stop='0' ORDER BY id DESC  ");
                            dD = new MySqlDataAdapter(sql, mConDemo);
                            ds = new DataSet();
                            ds.Reset();
                            dD.Fill(ds, sql);
                            dataGridView1.DataSource = ds.Tables[0];
                            break;
                        case "Вагоны на рагзрузке":
                            sql = ("SELECT * FROM vagon_vihod WHERE prihod='1' and status_start='1' and status_stop='0'  ORDER BY id DESC");
                            dD = new MySqlDataAdapter(sql, mConDemo);
                            ds = new DataSet();
                            ds.Reset();
                            dD.Fill(ds, sql);
                            dataGridView1.DataSource = ds.Tables[0];
                            break;
                        case "Вагоны рагруженные":
                            sql = ("SELECT * FROM vagon_vihod WHERE prihod='1' and status_start='1' and status_stop='1' ORDER BY id DESC  ");
                            dD = new MySqlDataAdapter(sql, mConDemo);
                            ds = new DataSet();
                            ds.Reset();
                            dD.Fill(ds, sql);
                            dataGridView1.DataSource = ds.Tables[0];
                            break;
                        case "Все":
                            sql = ($"SELECT * FROM vagon_vihod where date >= '{dateTimePicker_start.Value.ToString("yyyy-MM-dd")}' and date <= '{dateTimePicker_finish.Value.ToString("yyyy-MM-dd")}' ORDER BY id DESC");
                            dD = new MySqlDataAdapter(sql, mConDemo);
                            ds = new DataSet();
                            ds.Reset();
                            dD.Fill(ds, sql);
                            dataGridView1.DataSource = ds.Tables[0];
                            break;
                        default:
                            sql = ("SELECT * FROM vagon_vihod  ORDER BY id DESC");
                            using (var command = new MySqlCommand(sql, mConDemo))
                            {
                                using (var reader = command.ExecuteReader())
                                {
                                    var dataTable = new DataTable();
                                    dataTable.Load(reader); // Загружаем данные из DataReader в DataTable
                                    dataGridView1.DataSource = dataTable; // Привязываем DataTable к DataGridView
                                }
                            }
                            break;
                    }
                    mConDemo.Close();
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message + "ошибка бд");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dateTimePicker_start_ValueChanged(object sender, EventArgs e)
        {
            Sum();
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
                    string sql = ("SELECT * FROM vagon_vihod WHERE prihod ='1' and status_start='1' and status_stop='0' ORDER BY id DESC");
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

        private void Sum()
        {
            //DateTime now = DateTime.Now();
            string finish = dateTimePicker_finish.Value.ToString("yyyy-MM-dd HH-mm");
            string start = dateTimePicker_start.Value.ToString("yyyy-MM-dd HH-mm");

            using (var mConDemo = new MySqlConnection(ConfigurationManager.ConnectionStrings["234"].ConnectionString))
            {
                try
                {
                    mConDemo.Open();

                    string selectCmd = "SELECT COUNT(*)   FROM vagon_vihod WHERE prihod='0' and status_start='0' and status_stop='0'  ";
                    using (MySqlCommand cmd = new MySqlCommand(selectCmd, mConDemo))
                    {
                        string result1 = cmd.ExecuteScalar().ToString();
                        label8.Text = ("Вагоны в пути  " + result1);
                    }

                    string selectCmd2 = "SELECT COUNT(*)   FROM vagon_vihod WHERE prihod='1' and status_start='0' and status_stop='0'  ";
                    using (MySqlCommand cmd2 = new MySqlCommand(selectCmd2, mConDemo))
                    {
                        string result2 = cmd2.ExecuteScalar().ToString();
                        label9.Text = ("Вагоны на станции  " + result2);
                    }

                    string selectCmd3 = "SELECT COUNT(*)   FROM vagon_vihod WHERE prihod='1' and status_start='1' and status_stop='0'  ";
                    using (MySqlCommand cmd3 = new MySqlCommand(selectCmd3, mConDemo))
                    {
                        string result3 = cmd3.ExecuteScalar().ToString();
                        label10.Text = ("Вагоны на разгрузке  " + result3);
                    }


                    string selectCmd4 = "SELECT COUNT(*)   FROM vagon_vihod WHERE data_finish  >= '" + start + "' and  data_finish <= '" + finish + "'  ";
                    using (MySqlCommand cmd4 = new MySqlCommand(selectCmd4, mConDemo))
                    {
                        string result4 = cmd4.ExecuteScalar().ToString();
                        label11.Text = ("Разгружено вагонов  " + result4);
                    }

                    string selectCmd5 = "  SELECT SUM(weight) FROM vagon_vihod WHERE data_finish  >= '" + start + "' and  data_finish <= '" + finish + "'  ";
                    using (MySqlCommand cmd5 = new MySqlCommand(selectCmd5, mConDemo))
                    {
                        string result5 = cmd5.ExecuteScalar().ToString();
                        double mas;
                        double.TryParse(result5, out mas);
                        label12.Text = ("Разгружено тонн  " + mas / 1000);
                    }
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine(ex.Message);
                    MessageBox.Show(ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    Console.WriteLine("Sum confirm");
                    mConDemo.Close();
                }
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                dataset();
                backgroundWorker1.ReportProgress(100);
                Thread.Sleep(10000);
            }
        }

        private void dataset()
        {
            try
            {
                OpenCon();
                string q = ("INSERT INTO vagon_vihod (number,sender,costumer,material, weight) VALUE ('" + 1 + "', '" + 1 + "', '" + 1 + "','" + 1 + "','" + 1 + "')");
                ExecutQuery(q);
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                CloseCon();
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void button2_Click_2(object sender, EventArgs e)
        {
            order order = new order();
            order.Show();

        }

        private void dateTimePicker_start_ChangeUICues(object sender, UICuesEventArgs e)
        {
            //DateTime now = DateTime.Now();


        }

        private void dateTimePicker_finish_ValueChanged(object sender, EventArgs e)
        {
            Sum();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            InitializeDatePickers();
        }

        private void Button_report_balance_Click(object sender, EventArgs e)
        {
            
            balance f = new balance();
            if (!f.Visible)
                f.Show();
            
        
        }

        private void Button_zero_Click(object sender, EventArgs e)
        {
            zero_silo f = new zero_silo();
            f.Show();
        }

        private void Button4_Click_1(object sender, EventArgs e)
        {
            datefinish form = new datefinish(256);
            form.Show();
            form.FormClosed += async (senderSlave, eSlave) =>
            {
                Method();
                Sum();
                update();
            };
        }

        private void Button_fill_vag_Click(object sender, EventArgs e)
        {
            datefinish form = new datefinish(128);
            form.Show();
            form.FormClosed += async (senderSlave, eSlave) =>
            {
                Method();
                Sum();
                update();
            };

        }

        private void Button8_Click(object sender, EventArgs e)
        {
            move_silo ms = new move_silo();
            ms.Show();
        }
    }
}









