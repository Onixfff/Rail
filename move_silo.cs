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
using System.Configuration;

namespace rail
{
    public partial class move_silo : Form
    {
        MySqlConnection mCon = new MySqlConnection(ConfigurationManager.ConnectionStrings["234"].ConnectionString);
        MySqlCommand msd;
        private object label;
        string conSQL = ConfigurationManager.ConnectionStrings["234"].ConnectionString;
        public move_silo()
        {
            InitializeComponent();
        }
        private void update()
        {
            string sql = ("select ms.timestamp `Дата и время`, sb2.manufactur `Местоположение`, " +
                "sb2.silo_num `Силос №`, ms.target_silo_material `Материал`, ms.target_silo_sendler `Производитель`," +
                " '---->' as move , sb.manufactur `Местоположение куда`, sb.silo_num `Силос № куда`, /*ms.source_silo_material," +
                " ms.source_silo_sendler,*/ ms.weight `Вес, кг` from move_silo ms left join silo_balance sb  on ms.source_silo_id=sb.id " +
                "left join silo_balance sb2  on ms.target_silo_id=sb2.id order by ms.timestamp desc");
            MySqlDataAdapter dD = new MySqlDataAdapter(sql, mCon);
            DataSet ds = new DataSet();
            ds.Reset();
            dD.Fill(ds, sql);
            dataGridView_move.DataSource = ds.Tables[0];
            //dataGridView1.AutoResizeColumns();
            //dataGridView_sero_silo.Columns[0].HeaderText = "№ п/п";
            //dataGridView_sero_silo.Columns[0].Visible = false;
            //dataGridView_sero_silo.Columns[1].HeaderText = "Дата последнего обнуления";
            //dataGridView_sero_silo.Columns[1].Width = 120;
            //dataGridView_sero_silo.Columns[2].HeaderText = "Вес при обнулении";
            //dataGridView_sero_silo.Columns[2].Width = 90;
            //dataGridView_sero_silo.Columns[3].HeaderText = "Загруженный вес, кг";
            //dataGridView_sero_silo.Columns[3].Width = 120;
            //dataGridView_sero_silo.Columns[4].HeaderText = "Выгруженный вес,кг";
            //dataGridView_sero_silo.Columns[4].Visible = false;
            //dataGridView_sero_silo.Columns[4].Width = 120;
            //dataGridView_sero_silo.Columns[5].HeaderText = "Наименование материала";
            //dataGridView_sero_silo.Columns[5].Width = 160;
            //dataGridView_sero_silo.Columns[6].HeaderText = "Номер силос";
            //dataGridView_sero_silo.Columns[6].Visible = false;
            //dataGridView_sero_silo.Columns[7].HeaderText = "Местоположение силоса";
            //dataGridView_sero_silo.Columns[7].Width = 50;
            //dataGridView_sero_silo.Columns[8].HeaderText = "№ силоса";
            //dataGridView_sero_silo.Columns[8].Width = 100;
            //dataGridView_sero_silo.Columns[9].HeaderText = "% недостачи";
            //dataGridView_sero_silo.Columns[9].Width = 100;


            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            foreach (DataGridViewRow item in dataGridView_move.Rows)
            {

                if (item.Cells[6].Value.ToString() == "БСУ")
                {
                    item.DefaultCellStyle.BackColor = Color.LightBlue;
                }
                if (item.Cells[6].Value.ToString() == "ПРУ")
                {
                    item.DefaultCellStyle.BackColor = Color.LightSalmon;
                }
                if (item.Cells[6].Value.ToString() == "ГАЗОБЕТОН")
                {
                    item.DefaultCellStyle.BackColor = Color.LemonChiffon;
                }
                if (item.Cells[6].Value.ToString() == "ССС")
                    item.DefaultCellStyle.BackColor = Color.GreenYellow;
            }
        }

        private void Move_silo_Load(object sender, EventArgs e)
        {
            update();
        }
    }
}
