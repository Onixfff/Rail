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
//using ru.nvg79.connector;
using DataUpdater;
namespace rail
{
    public partial class balance : Form
    {
        MySqlConnection mCon = new MySqlConnection("Database=u0550310_aeroblock; Server=31.31.196.61; port=3306; username=u0550_kornev; password=18061981Kornev; charset=utf8 ");
        MySqlCommand msd;
        private object label;

        public balance()
        {
            InitializeComponent();
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


        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void Read_group_box(Control parent, int id, out string name, out string sender, out string weight)
        {



            name = parent.Controls["label_s" + (id + 1).ToString() + "_name"].Text.ToString();
            sender = parent.Controls["label_s" + (id + 1).ToString() + "_sender"].Text.ToString();
            weight = parent.Controls["label_s" + (id + 1).ToString() + "_balance"].Text.ToString(); 



        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {

        }
        private void s1_6 (int on)

        {
            List<GroupBox> gb = new List<GroupBox>();
            gb.Add(s1);
            gb.Add(s2);
            gb.Add(s3);
            gb.Add(s4);
            gb.Add(s5);
            gb.Add(s6);
            gb.Add(s7);
            gb.Add(s8);
            gb.Add(s9);
            gb.Add(s10);
            gb.Add(s11);
            gb.Add(s12);
            gb.Add(s13);
            gb.Add(s14);
            gb.Add(s15);
            gb.Add(s16);
            gb.Add(s17);
            gb.Add(s18);
            gb.Add(s19);
            gb.Add(s20);
            //string strSQL3 = "update silo_balance set silo_material_name ='" + material_name + "' where `id`='" + owner_nmae + "' ;";
            //ExecutQuery(strSQL3);

            Read_group_box(gb[on], on, out string name,out string sender, out string weight);
            string conSQL = "Database=u0550310_aeroblock; Server=31.31.196.61; port=3306; username=u0550_kornev; password=18061981Kornev; charset=utf8 ";
            Dictionary<string, string> str = new Dictionary<string, string>();
            str.Add("`date`", MySQLData.MysqlTime(DateTime.Now));
            str.Add("weight", weight);
            str.Add("weight_sum_in", "100");
            str.Add("weight_sum_out", "100");
            str.Add("material", name);
            str.Add("id_silos",on.ToString());
            

            string keys, values;
            MySQLData.ConvertInsertData(str, out keys, out values);
            string strSQL = "insert into zeroing_silo (" + keys + ") values (" + values + ");";
            bool isok = false;
            while (!isok)
            {
                MySQLData.GetScalar.Result wres = MySQLData.GetScalar.NoResponse(strSQL, conSQL);
                if (wres.HasError == true)
                { isok = false; Thread.Sleep(500); }
                else
                    isok = true;
            }


        }

        private void ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem cms = (ToolStripMenuItem)sender;
            ContextMenuStrip strip = (ContextMenuStrip)cms.Owner;
            Control owner = strip.SourceControl;
            // MessageBox.Show(owner.Name);
            string owner_name = owner.Name.ToString();
            owner_name = owner_name.Remove(0, 1);
            s1_6(int.Parse(owner_name));


        }

        private void update()
        {
            string sql = ("SELECT * FROM silo_balance where id<>22 ");
            MySqlDataAdapter dD = new MySqlDataAdapter(sql, mCon);
            DataSet ds = new DataSet();
            ds.Reset();
            dD.Fill(ds, sql);
            dataGridView1.DataSource = ds.Tables[0];
            //dataGridView1.AutoResizeColumns();
            dataGridView1.Columns[0].HeaderText = "№ п/п";
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].HeaderText = "Место положение";
            dataGridView1.Columns[1].Width = 90;
            dataGridView1.Columns[2].HeaderText = "№ силоса";
            dataGridView1.Columns[2].Width = 50;
            dataGridView1.Columns[3].HeaderText = "Наименование материала";
            dataGridView1.Columns[3].Width = 160;
            dataGridView1.Columns[4].HeaderText = "Производитель";
            dataGridView1.Columns[4].Width = 160;
            dataGridView1.Columns[5].HeaderText = "Фактический вес, тн";
            dataGridView1.Columns[5].Width = 80;
            dataGridView1.Columns[6].HeaderText = "MAX вместимость, тн";
            dataGridView1.Columns[6].Width = 80;

            //this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
        }
       


        private void fill_group_box(Control parent, int id)
        {



            parent.Controls["label_s" + (id + 1).ToString() + "_name"].Text = dataGridView1.Rows[id].Cells[3].Value.ToString();
            parent.Controls["label_s" + (id + 1).ToString() + "_sender"].Text = dataGridView1.Rows[id].Cells[4].Value.ToString();
            parent.Controls["label_s" + (id + 1).ToString() + "_balance"].Text = dataGridView1.Rows[id].Cells[5].Value.ToString();



        }

        private void fill_cb()
        {
            string sql = ("SELECT * FROM silo_balance where manufactur='ПРУ' ");
            MySqlDataAdapter dD = new MySqlDataAdapter(sql, mCon);
            DataTable tbl1 = new DataTable();
            dD.Fill(tbl1);

            comboBox1.DataSource = tbl1;
            comboBox1.DisplayMember = "silo_num";// столбец для отображения
            comboBox1.ValueMember = "id";


        }

        private void fill_cb(string sql_disp, ComboBox cb)
        {
            string sql = ("SELECT distinct " + sql_disp + " FROM silo_balance");
            MySqlDataAdapter dD = new MySqlDataAdapter(sql, mCon);
            DataTable tbl1 = new DataTable();
            dD.Fill(tbl1);
            cb.DataSource = tbl1;
            cb.DisplayMember = sql_disp;// столбец для отображения
            cb.ValueMember = sql_disp;


        }
        private void fill_cb(string sql_disp, string sql_us, ComboBox cb)
        {
            string sql = ("SELECT distinct " + sql_disp + " FROM silo_balance where  manufactur= '" + sql_us + "'");
            MySqlDataAdapter dD = new MySqlDataAdapter(sql, mCon);
            DataTable tbl1 = new DataTable();
            dD.Fill(tbl1);

            cb.DataSource = tbl1;
            cb.DisplayMember = sql_disp;// столбец для отображения
            cb.ValueMember = sql_disp;


        }
        private void fill_tb(string sql_disp, string sql_us, string sql_us2, TextBox tb)
        {
            string sql = ("SELECT " + sql_disp + " FROM silo_balance where  manufactur= '" + sql_us + "' and silo_num='" + sql_us2 + "' ");
            msd = new MySqlCommand(sql, mCon);
            mCon.Open();
            try
            {
                tb.Text = msd.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            mCon.Close();



        }

      
        private void Update_silo()
        {
            List<GroupBox> gb = new List<GroupBox>();
            gb.Add(s1);
            gb.Add(s2);
            gb.Add(s3);
            gb.Add(s4);
            gb.Add(s5);
            gb.Add(s6);
            gb.Add(s7);
            gb.Add(s8);
            gb.Add(s9);
            gb.Add(s10);
            gb.Add(s11);
            gb.Add(s12);
            gb.Add(s13);
            gb.Add(s14);
            gb.Add(s15);
            gb.Add(s16);
            gb.Add(s17);
            gb.Add(s18);
            gb.Add(s19);
            gb.Add(s20);


            int id;
            for (id = 0; id <= 19; id++)
            {
                fill_group_box(gb[id], id);
            }

        }
        private void Balance_Load(object sender, EventArgs e)
        {
            fill_cb();
            update();
            Fg();
            fill_cb("manufactur", comboBox_manufaktur_target);
            fill_cb("silo_num", comboBox_manufaktur_target.SelectedValue.ToString(), comboBox_silo_num_target);
            fill_tb("silo_material_name", comboBox_manufaktur_target.SelectedValue.ToString(), comboBox_silo_num_target.SelectedValue.ToString(), textBox_material_target);
            fill_tb("weight", comboBox_manufaktur_target.SelectedValue.ToString(), comboBox_silo_num_target.SelectedValue.ToString(), textBox_weight_target);

            // comboBox_silo_num_target.Text

            comboBox1.Text = "1";
            Update_silo();
            



        }
        private void Fg()
        {

            string val = comboBox1.SelectedValue.ToString();
            string sql = ("SELECT * FROM silo_balance where manufactur='ПРУ' and silo_num='" + val + "'");
            MySqlDataAdapter dD = new MySqlDataAdapter(sql, mCon);

            DataTable tbl1 = new DataTable();
            dD.Fill(tbl1);
            if (val != "System.Data.DataRowView")
            {
                textBox_name_material_source.Text = tbl1.Rows[0][3].ToString();
                textBox_sendler_source.Text = tbl1.Rows[0][4].ToString();
            }

            else
            {
                return;
            }

        }

        private void ComboBox1_TextChanged(object sender, EventArgs e)
        {
            Fg();

        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {



        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Fg();
        }

        private void TextBox_weight_target_TextChanged(object sender, EventArgs e)
        {

        }

        private void ComboBox_manufaktur_target_SelectedValueChanged(object sender, EventArgs e)
        {
            fill_cb("silo_num", comboBox_manufaktur_target.SelectedValue.ToString(), comboBox_silo_num_target);


        }

       

        private void ComboBox_silo_num_target_TextChanged(object sender, EventArgs e)
        {
            string val1 = comboBox_manufaktur_target.SelectedValue.ToString();
            string val2 = comboBox_silo_num_target.SelectedValue.ToString();
            fill_tb("silo_material_name", val1, val2, textBox_material_target);
            fill_tb("weight", comboBox_manufaktur_target.SelectedValue.ToString(), comboBox_silo_num_target.SelectedValue.ToString(), textBox_weight_target);
        }

        private void ComboBox_manufaktur_target_TextChanged(object sender, EventArgs e)
        {

        }

        private void update_silo(Form form6)
        {
          
        }
       

        private void НаименованиеМатериалаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem cms = (ToolStripMenuItem)sender;
            ContextMenuStrip strip = (ContextMenuStrip)cms.Owner;
            Control owner = strip.SourceControl;
            string owner_nmae = owner.Name.ToString();
            owner_nmae = owner_nmae.Remove(0, 1); 
            material form6 = new material();
            form6.Show();
            string material_name;
            form6.dataGridView1.MouseDoubleClick += (senderSlave, eSlave) =>
            {
               material_name = form6.dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                form6.Close();
                string strSQL3 = "update silo_balance set silo_material_name ='" + material_name + "' where `id`='" + owner_nmae + "' ;";
                ExecutQuery(strSQL3);
                update();
                Update_silo();
            };
            form6.button2.Click += (senderSlave, eSlave) =>
            {
               material_name = form6.dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                form6.Close();
                string strSQL3 = "update silo_balance set silo_material_name ='" + material_name + "' where `id`='" + owner_nmae + "' ;";
                ExecutQuery(strSQL3);
                update();
                Update_silo();
            };
           



        }

        private void ПоставщикToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem cms = (ToolStripMenuItem)sender;
            ContextMenuStrip strip = (ContextMenuStrip)cms.Owner;
            Control owner = strip.SourceControl;
            string owner_nmae = owner.Name.ToString();
            owner_nmae = owner_nmae.Remove(0, 1);
            string material_name;
            sendler form4 = new sendler();
            form4.Show();
            form4.dataGridView2.MouseDoubleClick += (senderSlave, eSlave) =>
            {
                material_name = form4.dataGridView2.SelectedRows[0].Cells[1].Value.ToString();
                form4.Close();
                string strSQL3 = "update silo_balance set silo_name_sendler ='" + material_name + "' where `id`='" + owner_nmae + "' ;";
                ExecutQuery(strSQL3);
                update();
                Update_silo();
            };
            form4.butenterF4.Click += (senderSlave, eSlave) =>
            {
                material_name = form4.dataGridView2.SelectedRows[0].Cells[1].Value.ToString();
                form4.Close();
                string strSQL3 = "update silo_balance set silo_name_sendler ='" + material_name + "' where `id`='" + owner_nmae + "' ;";
                ExecutQuery(strSQL3);
                update();
                Update_silo();
            };

        }

        private void Label_s13_name_Click(object sender, EventArgs e)
        {

        }

        private void GroupBox6_Enter(object sender, EventArgs e)
        {

        }
    }
}
    






