using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using MySql;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
namespace rail
{
    class conn
    {
        MySqlConnection mcon = new MySqlConnection("Database=spslogger; datasource=localhost; port=3306; username=root; password=12345; charset=utf8");
        MySqlCommand msd;

        private void CloseCon()
        {
            if (mcon.State == ConnectionState.Open)
            {
                mcon.Close();
            }
        }
        private void OpenCon()
        {
            if (mcon.State == ConnectionState.Closed)
            {
                mcon.Open();
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
    }
}
