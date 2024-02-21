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
    public partial class datefinish : Form
    {
        MySqlConnection mCon = new MySqlConnection(ConfigurationManager.ConnectionStrings["234"].ConnectionString);
        MySqlCommand msd;
        int arg;
        public datefinish(int arg)
        {
            this.arg = arg;
            InitializeComponent();
        }

        private void update()
        {
            string sql = ("SELECT * FROM vagon_vihod WHERE prihod ='1' and status_start='1' and status_stop='0'");
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
            dataGridView1.Columns["date"].Visible = false;
            dataGridView1.Columns["data_start"].Visible = false;
            dataGridView1.Columns["data_finish"].Visible = false;
            dataGridView1.Columns["status_start"].Visible = false;
            dataGridView1.Columns[1].HeaderText = "Партия";
            dataGridView1.Columns[2].HeaderText = "№ вагона";
            dataGridView1.Columns["sender"].HeaderText = "Отправитель";
            dataGridView1.Columns["material"].HeaderText = "Материал";
            dataGridView1.Columns["partia"].HeaderText = "Партия разгрузки";
            dataGridView1.Columns["status_stop"].HeaderText = "Статус окончания разгрузки";
            dataGridView1.Columns[1].Width = 50;
            dataGridView1.Columns["sender"].Width = 240;
            dataGridView1.Columns["material"].Width = 180;
            dataGridView1.Columns["partia"].Width = 150;
            dataGridView1.Columns["status_stop"].Width = 80;

        }

        public void button1_Click(object sender, EventArgs e)
        {

            string sql = ("SELECT * FROM vagon_vihod WHERE prihod ='1' and status_start='1' and status_stop='0'");
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
            dataGridView1.Columns["date"].Visible = false;
            dataGridView1.Columns["data_start"].Visible = false;
            dataGridView1.Columns["data_finish"].Visible = false;
            dataGridView1.Columns["status_start"].Visible = false;
            dataGridView1.Columns[1].HeaderText = "Партия";
            dataGridView1.Columns[2].HeaderText = "№ вагона";
            dataGridView1.Columns["sender"].HeaderText = "Отправитель";
            dataGridView1.Columns["material"].HeaderText = "Материал";
            dataGridView1.Columns["partia"].HeaderText = "Партия разгрузки";
            dataGridView1.Columns["status_stop"].HeaderText = "Статус окончания разгрузки";
            dataGridView1.Columns[1].Width = 50;
            dataGridView1.Columns["sender"].Width = 240;
            dataGridView1.Columns["material"].Width = 180;
            dataGridView1.Columns["partia"].Width = 150;
            dataGridView1.Columns["status_stop"].Width = 80;

            textBox1.Text = dateTimePicker1.Value.ToString("yyyy-MM-dd HH-mm");


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
                    string sql = ("SELECT * FROM vagon_vihod WHERE prihod ='1' and status_start='1' and status_stop='0'");
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
            if (/*textBox_silo.Text == ""||*/dataGridView1.SelectedRows[0].Cells["number_silos"].Value.ToString()=="")
            {
                MessageBox.Show("Введите номер силоса");
                return;

            }
            textBox_silo.Text = dataGridView1.SelectedRows[0].Cells["number_silos"].Value.ToString();
            string date_prihod = dateTimePicker1.Value.ToString("yyyy-MM-dd HH:mm");
            string id = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();



            //string q = ("UPDATE vagon_vihod SET status_stop ='1', data_finish='" + date_prihod + "', number_silos='" + textBox_silo.Text + "' WHERE id='" + id + "'");
            string q = ("UPDATE vagon_vihod SET status_stop ='1', data_finish='" + date_prihod + "', number_silos='" + textBox_silo.Text + "' WHERE id='" + id + "'");
            ExecutQuery(q);
            textBox_silo.Text = "";

            mCon.Open();
            string selectCmd = "SELECT UNIX_TIMESTAMP(data_finish)/3600 -UNIX_TIMESTAMP(data_start)/3600 FROM vagon_vihod WHERE id='" + id + "' ";
            MySqlCommand cmd = new MySqlCommand(selectCmd, mCon);
            string result = cmd.ExecuteScalar().ToString();
            float gh = Convert.ToSingle(result);
            string time2 = String.Format("{0:N2}", gh);
            textBox3.Text = time2;
            string qq = ("UPDATE vagon_vihod SET  time='" + time2 + "' WHERE id='" + id + "'");
            ExecutQuery(qq);

        }



        private void Form9_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void Datefinish_Load(object sender, EventArgs e)
        {
            if (arg == 128)
            {
                button1.Visible = false;
                textBox1.Visible = false;
                update();
                textBox_silo.Enabled = false;

                //contextMenuStrip1.Enabled = true;


            }
            if (arg == 256)
                button1.Visible = true;
           // contextMenuStrip1.Enabled = false;


        }
        private void Move_mas_pru(string silo, int mas)
        {

            libnodave.daveOSserialType fds;
            libnodave.daveInterface di;
            libnodave.daveConnection dc;
            try
            {
                int res = 0;
                int silo_mas = 0;

                try
                {
                    fds.rfd = libnodave.openSocket(102, "192.168.37.139");
                    fds.wfd = fds.rfd;
                    if (fds.rfd > 0)
                    {


                        di = new libnodave.daveInterface(fds, "IF1", 0, libnodave.daveProtoISOTCP, libnodave.daveSpeed187k);
                        di.setTimeout(50);
                        dc = new libnodave.daveConnection(di, 0, 0, 2);
                        if (0 == dc.connectPLC())

                        {

                            if (silo == "1")
                            {
                                res = dc.readBytes(libnodave.daveDB, 12, 100, 4, null);

                                if (res == 0) //conection OK 
                                {
                                    silo_mas = dc.getU32();

                                }
                                silo_mas = silo_mas + mas;
                                res = dc.writeBytes(libnodave.daveDB, 12, 100, 4, BitConverter.GetBytes(libnodave.daveSwapIed_32(silo_mas)));
                            }
                            if (silo == "2")
                            {
                                res = dc.readBytes(libnodave.daveDB, 12, 104, 4, null);

                                if (res == 0) //conection OK 
                                {
                                    silo_mas = dc.getU32();

                                }
                                silo_mas = silo_mas + mas;
                                res = dc.writeBytes(libnodave.daveDB, 12, 104, 4, BitConverter.GetBytes(libnodave.daveSwapIed_32(silo_mas)));
                            }
                            if (silo == "3")
                            {
                                res = dc.readBytes(libnodave.daveDB, 12, 108, 4, null);

                                if (res == 0) //conection OK 
                                {
                                    silo_mas = dc.getU32();

                                }
                                silo_mas = silo_mas + mas;
                                res = dc.writeBytes(libnodave.daveDB, 12, 108, 4, BitConverter.GetBytes(libnodave.daveSwapIed_32(silo_mas)));
                            }
                            if (silo == "4")
                            {
                                res = dc.readBytes(libnodave.daveDB, 12, 112, 4, null);

                                if (res == 0) //conection OK 
                                {
                                    silo_mas = dc.getU32();

                                }
                                silo_mas = silo_mas + mas;
                                res = dc.writeBytes(libnodave.daveDB, 12, 112, 4, BitConverter.GetBytes(libnodave.daveSwapIed_32(silo_mas)));
                            }
                            if (silo == "5")
                            {
                                res = dc.readBytes(libnodave.daveDB, 12, 116, 4, null);

                                if (res == 0) //conection OK 
                                {
                                    silo_mas = dc.getU32();

                                }
                                silo_mas = silo_mas + mas;
                                res = dc.writeBytes(libnodave.daveDB, 12, 116, 4, BitConverter.GetBytes(libnodave.daveSwapIed_32(silo_mas)));
                            }
                            if (silo == "6")
                            {
                                res = dc.readBytes(libnodave.daveDB, 12, 120, 4, null);

                                if (res == 0) //conection OK 
                                {
                                    silo_mas = dc.getU32();

                                }
                                silo_mas = silo_mas + mas;
                                res = dc.writeBytes(libnodave.daveDB, 12, 120, 4, BitConverter.GetBytes(libnodave.daveSwapIed_32(silo_mas)));
                            }

                        }
                        dc.disconnectPLC();
                        libnodave.closeSocket(fds.rfd);
                    }
                    else
                    {

                    }
                }
                catch (Exception exp)
                {
                    MessageBox.Show(exp.Message);

                }

            }
            catch (Exception exp)
            {
                MessageBox.Show("GetValueFromController() - " + exp.Message, "Error");
            }
        }

        private void ЗавершитьВыгрузкуВагонаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (arg == 128)
            {
                textBox_silo.Enabled = false;
                if (dataGridView1.SelectedRows[0].Cells["number_silos"].Value.ToString() == "")
                {
                    add_silo f = new add_silo();
                    f.Show();
                    f.button1.Click += (senderSlave, eSlave) =>
                    {
                        textBox_silo.Text = f.comboBox1.SelectedValue.ToString();
                        f.Close();
                        string date_prihod = dateTimePicker1.Value.ToString("yyyy-MM-dd HH:mm");
                        string id = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                        string mass = dataGridView1.SelectedRows[0].Cells["weight"].Value.ToString();
                        int mas = Convert.ToInt32(mass);



                        string q = ("UPDATE  vagon_vihod  set vag_finish='" + date_prihod + "', number_silos='" + textBox_silo.Text + "' WHERE id='" + id + "'");
                        ExecutQuery(q);
                        Move_mas_pru(textBox_silo.Text.ToString(), mas);
                        textBox_silo.Text = "";
                        update();
                    };



                }
                else MessageBox.Show("Вагон уже разгружен. Повторная проводка запрещена", "ПРЕДУПРЕЖДЕНИЕ", MessageBoxButtons.OK, MessageBoxIcon.Warning);

               
             
            }
        }
    }
}
