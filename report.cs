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
using System;
using System.Configuration;

namespace rail
{
    public partial class report : Form
    {
        MySqlConnection mCon = new MySqlConnection(ConfigurationManager.ConnectionStrings["234"].ConnectionString);
        //MySqlConnection mCon = new MySqlConnection("Database=u0550310_aeroblock; Server=localhost; port=3306; username=root; password=12345; charset=utf8 ");
        MySqlCommand msd;
        public report()
        {
            InitializeComponent();
        }
        private void picker()
        {
            var mounth = DateTime.Now.Month;
            var year = DateTime.Now.Year;
            var date = new DateTime(year, mounth, 1);
            int days = DateTime.DaysInMonth(year, mounth);
            var date2 = new DateTime(year, mounth, days);
            dateTimePicker_start.Text = date.ToString();
            dateTimePicker_finish.Text = date2.ToString();

        }

        private void select_bd(string sql1, string date_time, string delta, DataGridView dgw)
        {
            string sql = ("SELECT sender, material as mterial, count(prihod) as sum, round(SUM(weight)/1000) as vag_part,  " + date_time + "(now(), " + delta + " ) as t_r FROM u0550310_aeroblock.vagon_vihod  WHERE " + sql1 + " GROUP BY sender, material, t_r");
            MySqlDataAdapter dD = new MySqlDataAdapter(sql, mCon);
            DataSet ds = new DataSet();
            ds.Reset();
            dD.Fill(ds, sql);
            dgw.DataSource = ds.Tables[0];
            dgw.AutoResizeColumns();
            dgw.Columns[0].HeaderText = "Отправитель";
            dgw.Columns[1].HeaderText = "Материал";
            dgw.Columns[2].HeaderText = "Количество вагонов";
            dgw.Columns[3].HeaderText = "Вес вагонов, кг";
            dgw.Columns[4].HeaderText = "Время нахождения, дни/час";

        }

        private void select_bd(string sql1, DataGridView dgw)
        {
            string sql = ("SELECT sender, material as mterial, count(prihod) as sum, round(SUM(weight)/1000) as vag_part FROM u0550310_aeroblock.vagon_vihod  WHERE   " + sql1 + " GROUP BY sender, material");
            MySqlDataAdapter dD = new MySqlDataAdapter(sql, mCon);
            DataSet ds = new DataSet();
            ds.Reset();
            dD.Fill(ds, sql);
            dgw.DataSource = ds.Tables[0];
            dgw.AutoResizeColumns();
            dgw.Columns[0].HeaderText = "Отправитель";
            dgw.Columns[1].HeaderText = "Материал";
            dgw.Columns[2].HeaderText = "Количество вагонов";
            dgw.Columns[3].HeaderText = "Вес вагонов, кг";


        }

        private void select_bd_pru(string sql1, DataGridView dgw)
        {

            string sql = ("sELECT t2.sender, t2.material as mterial, count(t2.prihod) as sum, round(SUM(t2.weight) / 1000) as vag_part, " +
                " (select sum(t1.status_start)   from u0550310_aeroblock.vagon_vihod t1  WHERE t2.material = t1.material and t1.prihod = 1 and t1.status_start = 1 and" +
                " t1.status_stop = 0 and t1.number_silos <> 0) as finish FROM u0550310_aeroblock.vagon_vihod t2 WHERE t2.prihod = 1 and t2.status_start = 1 and t2.status_stop = 0   " +
                "GROUP BY t2.sender, t2.material");
            //string sql = ("SELECT sender, material as mterial, count(prihod) as sum, round(SUM(weight)/1000) as vag_part," +
            //   " (select count(prihod) FROM u0550310_aeroblock.vagon_vihod  WHERE prihod=1 and status_start=1 and status_stop=0 and number_silos<>0) as off `Разгружено`    FROM u0550310_aeroblock.vagon_vihod  WHERE " + sql1 + " GROUP BY sender, material");
            MySqlDataAdapter dD = new MySqlDataAdapter(sql, mCon);
            DataSet ds = new DataSet();
            ds.Reset();
            dD.Fill(ds, sql);
            dgw.DataSource = ds.Tables[0];
            dgw.AutoResizeColumns();
            dgw.Columns[0].HeaderText = "Отправитель";
            dgw.Columns[1].HeaderText = "Материал";
            dgw.Columns[2].HeaderText = "Количество вагонов";
            dgw.Columns[3].HeaderText = "Вес вагонов, кг";
            dgw.Columns[4].HeaderText = "Разгружено";
            foreach (DataGridViewRow item in dgw.Rows)
            {

                if (item.Cells[4].Value.ToString() == item.Cells[2].Value.ToString())
                {
                    item.DefaultCellStyle.BackColor = Color.GreenYellow;
                }




            }
        }
            private void start_load()
        {
            string sql = ("prihod=0");
            string sql2 = ("prihod=1 and status_start=0 ");
            string sql3 = ("prihod=1 and status_start=1 and status_stop=0 ");
            string finish = dateTimePicker_finish.Value.ToString("yyyy-MM-dd HH-mm");
            string start = dateTimePicker_start.Value.ToString("yyyy-MM-dd HH-mm");
            string sql4 = ("prihod=1 and status_start=1 and status_stop=1 and data_finish  >= '" + start + "' and  data_finish <= '" + finish + "' ");
            var date_now = DateTime.Now;
            string date_st = date_now.ToString("yyyy-MM-dd HH:mm");
            select_bd(sql, "datediff", "`date`", dataGridView__in_run);
            select_bd(sql2, "timediff", "`data_prihod`", dataGridView_in_station);
            //select_bd(sql3, "timediff", "`data_start`", dataGridView_in_pru);
            select_bd_pru(sql3, dataGridView_in_pru);
            select_bd(sql4, dataGridView_finish);
            string selectCmd4 = "SELECT COUNT(*)   FROM vagon_vihod WHERE data_finish  >= '" + start + "' and  data_finish <= '" + finish + "'  ";

            mCon.Open();
            MySqlCommand cmd4 = new MySqlCommand(selectCmd4, mCon);
            //MySqlDataAdapter cmd4 = new MySqlDataAdapter(selectCmd4, mCon);
            string result4 = cmd4.ExecuteScalar().ToString();
            sum_vagon.Text = ("Разгружено вагонов  " + result4);
            string selectCmd5 = "  SELECT SUM(weight) FROM vagon_vihod WHERE data_finish  >= '" + start + "' and  data_finish <= '" + finish + "'  ";
            MySqlCommand cmd5 = new MySqlCommand(selectCmd5, mCon);
            string result5 = cmd5.ExecuteScalar().ToString();
            double mas;
            double.TryParse(result5, out mas);
            weight_vag.Text = ("Разгруженный вес вагонов  " + mas / 1000 + "  тонн");
            mCon.Close();
        }

//        SELECT material `Материал`,sender `Отправитель`,  rOUND(AVG(DATEDIFF (`data_prihod`, `date`)),0) as `Среднее время в пути`, max(DATEDIFF(`data_prihod`, `date`)) `МАХ время в пути`, min(DATEDIFF(`data_prihod`, `date`)) `MIN время в пути` FROM vagon_vihod where date(data_prihod) BETWEEN
//DATE_SUB(NOW(), INTERVAL 2 MONTH) AND NOW() group BY material, sender

            private void in_rail()
        {
            string sql = ("SELECT material `Материал`,sender `Отправитель`,  " +
                "rOUND(AVG(DATEDIFF(`data_prihod`, `date`)), 0) as `Среднее время в пути`, " +
                "max(DATEDIFF(`data_prihod`, `date`)) `МАХ время в пути`, min(DATEDIFF(`data_prihod`, `date`))" +
                " `MIN время в пути` FROM vagon_vihod where date(data_prihod) BETWEEN DATE_SUB(NOW(), INTERVAL 2 MONTH) AND NOW() group BY material, sender");
            MySqlDataAdapter dD = new MySqlDataAdapter(sql, mCon);
            DataSet ds = new DataSet();
            ds.Reset();
            dD.Fill(ds, sql);
          dataGridView_rail.DataSource = ds.Tables[0];
            dataGridView_rail.AutoResizeColumns();
           

        }
        private void report_Load(object sender, System.EventArgs e)

        {
            picker();
            start_load();
            in_rail();

        }

        private void button7_Click(object sender, EventArgs e)
        {
            picker();
            start_load();
        }

        private void dateTimePicker_start_ValueChanged(object sender, EventArgs e)
        {
            start_load();

        }

        private void dateTimePicker_finish_ValueChanged(object sender, EventArgs e)
        {
            start_load();

        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }
    }
}
