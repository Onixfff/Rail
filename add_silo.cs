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
using System.Configuration;

namespace rail
{
    public partial class add_silo : Form
    {
        //MySqlConnection mCon = new MySqlConnection("Database=u0550310_aeroblock; Server=31.31.196.234; port=3306; username=u0550_kornev; password=18061981Kornev; charset=utf8 ");
        MySqlConnection mCon = new MySqlConnection(ConfigurationManager.ConnectionStrings["234"].ConnectionString);
        MySqlCommand msd;
        public add_silo()
        {
            InitializeComponent();
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

        private void Add_silo_Load(object sender, EventArgs e)
        {
            fill_cb("silo_num", "ПРУ", comboBox1);
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
