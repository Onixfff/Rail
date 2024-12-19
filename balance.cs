using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Threading;
//using ru.nvg79.connector;
using DataUpdater;
using System.Configuration;
using S7.Net;
using System.Net.Http;
using System.Linq;
using Newtonsoft.Json;
using System.Text;
using rail.Models;
using Org.BouncyCastle.Bcpg;
using Google.Protobuf.WellKnownTypes;

namespace rail
{
    public partial class balance : Form
    {
        //MySqlConnection mCon = new MySqlConnection("Database=u0550310_aeroblock; Server=31.31.196.234; port=3306; username=u0550_kornev; password=18061981Kornev; charset=utf8 ");
        MySqlConnection mCon = new MySqlConnection(ConfigurationManager.ConnectionStrings["234"].ConnectionString);
        MySqlCommand msd;
        private object label;
        string conSQL = ConfigurationManager.ConnectionStrings["234"].ConnectionString;

        private static readonly HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://192.168.100.100:5048")
        };

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

        private void s1_6(string on)
        {
            //conSQL = "Database=u0550310_aeroblock; Server=31.31.196.234; port=3306; username=u0550_kornev; password=18061981Kornev; charset=utf8 ";
            string strSQL3 = "SELECT * FROM `silo_balance` where `id`='" + on + "' ;";
            string strSQLdata = "select * from zeroing_silos  where `id_silos`='" + on + "' order by id desc limit 1 ";
            MySQLData.GetData.Result tb1 = MySQLData.GetData.Table(strSQL3, conSQL);
            MySQLData.GetData.Result tb2 = MySQLData.GetData.Table(strSQLdata, conSQL);
            string weight = tb1.ResultData.Rows[0][5].ToString();
            string material = tb1.ResultData.Rows[0][3].ToString();

            string weight_sum_in = "1";
            string weight_sum_out = "1";

            if (tb2.ResultData.Rows.Count == 0)
            {
                weight_sum_in = "1";
                weight_sum_out = "1";
            }
            else
            {
                string data = tb2.ResultData.Rows[0][1].ToString();
                string strSQL4;
                DateTime data2 = DateTime.Parse(data);
                data = data2.ToString("yyyy-MM-dd HH:mm");
                if (on == "20")
                { strSQL4 = "SELECT sum(weight) FROM `vagon_vihod` where `data_finish`>'" + data + "' and number_silos='6' ;"; }
                else
                { strSQL4 = "SELECT sum(weight) FROM `vagon_vihod` where `data_finish`>'" + data + "' and number_silos='" + on + "' ;"; }
                MySQLData.GetScalar.Result wsi = MySQLData.GetScalar.Scalar(strSQL4, conSQL);
                weight_sum_in = wsi.ResultText;
                if (weight_sum_in == "")
                    weight_sum_in = "1";

            }

            //ExecutQuery(strSQL3);

            Dictionary<string, string> str = new Dictionary<string, string>();
            str.Add("`date`", MySQLData.MysqlTime(DateTime.Now));
            str.Add("weight", weight);
            str.Add("weight_sum_in", weight_sum_in);
            str.Add("weight_sum_out", weight_sum_out);
            str.Add("material", material);
            str.Add("id_silos", on);

            string keys, values;
            MySQLData.ConvertInsertData(str, out keys, out values);
            string strSQL = "insert into zeroing_silos (" + keys + ") values (" + values + ");";
            bool isok = false;
            while (!isok)
            {
                MySQLData.GetScalar.Result wres = MySQLData.GetScalar.NoResponse(strSQL, conSQL);
                if (wres.HasError == true)
                { isok = false; Thread.Sleep(500); }
                else
                {
                    isok = true;
                    zero_plc_rzd(on);
                }
            }
        }

        private void zero_plc_gb(string silo)
        {
            // ОБНУЛЕНИЕ В КОНТРОЛЛЕРЕ

            libnodave.daveOSserialType fds;
            libnodave.daveInterface di;
            libnodave.daveConnection dc;

            try
            {
                int res = 0;

                try
                {
                    fds.rfd = libnodave.openSocket(102, "192.168.37.102");
                    fds.wfd = fds.rfd;

                    if (fds.rfd > 0)
                    {
                        di = new libnodave.daveInterface(fds, "IF1", 0, libnodave.daveProtoISOTCP, libnodave.daveSpeed187k);
                        di.setTimeout(50);
                        dc = new libnodave.daveConnection(di, 0, 0, 2);

                        if (0 == dc.connectPLC())
                        {
                            byte[] aa = { 0, 0, 0, 0 };
                            if (silo == "7")
                                res = dc.writeBytes(libnodave.daveDB, 305, 88, 4, aa);
                            if (silo == "6")
                                res = dc.writeBytes(libnodave.daveDB, 305, 92, 4, aa);
                            if (silo == "8")
                                res = dc.writeBytes(libnodave.daveDB, 305, 96, 4, aa);
                            if (silo == "10")
                                res = dc.writeBytes(libnodave.daveDB, 305, 84, 4, aa);
                            if (silo == "9")
                                res = dc.writeBytes(libnodave.daveDB, 305, 80, 4, aa);


                            res = dc.readBits(libnodave.daveDB, 160, 3890, 1, null);
                            //MessageBox.Show("результат функции:" + res + " = " + libnodave.daveStrerror(res));
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

        private async Task move_mas(string target_id, string source_id, int mas)
        {
            switch (source_id)
            {
                case "1":
                    await Task.Run(() => Move_mas_pru(target_id, Convert.ToInt32(textBox_weight.Text))); //ПРУ
                    break;
                case "2":
                    await Task.Run(() => Move_mas_pru(target_id, Convert.ToInt32(textBox_weight.Text)));//ПРУ
                    break;
                case "3":
                    await Task.Run(() => Move_mas_pru(target_id, Convert.ToInt32(textBox_weight.Text)));//ПРУ
                    break;
                case "4":
                    await Task.Run(() => Move_mas_pru(target_id, Convert.ToInt32(textBox_weight.Text)));//ПРУ
                    break;
                case "5":
                    await Task.Run(() => Move_mas_pru(target_id, Convert.ToInt32(textBox_weight.Text)));//ПРУ
                    break;
                case "6":
                    await Task.Run(() => Move_mas_gb(source_id, Convert.ToInt32(textBox_weight.Text)));//газобетон
                    break;
                case "7":
                    await Task.Run(() => Move_mas_gb(source_id, Convert.ToInt32(textBox_weight.Text)));//газобетон
                    break;
                case "8":
                    await Task.Run(() => Move_mas_gb(source_id, Convert.ToInt32(textBox_weight.Text)));//газобетон
                    break;
                case "9":
                    await Task.Run(() => Move_mas_gb(source_id, Convert.ToInt32(textBox_weight.Text)));//газобетон
                    break;
                case "10":
                    await Task.Run(() => Move_mas_gb(source_id, Convert.ToInt32(textBox_weight.Text)));//газобетон
                    break;
                case "11":
                    await Task.Run(() => Move_mas_sss(source_id, Convert.ToInt32(textBox_weight.Text)));//сухие смеси
                    break;
                case "12":
                    await Task.Run(() => Move_mas_sss(source_id, Convert.ToInt32(textBox_weight.Text)));//сухие смеси
                    break;
                case "13":
                    await Task.Run(() => Move_mas_sss(source_id, Convert.ToInt32(textBox_weight.Text)));//сухие смеси
                    break;
                case "14":
                    await Task.Run(() => Move_mas_sss(source_id, Convert.ToInt32(textBox_weight.Text)));//сухие смеси
                    break;
                case "15":
                    await Task.Run(() => Move_mas_sss(source_id, Convert.ToInt32(textBox_weight.Text)));//сухие смеси
                    break;
                case "16":
                    break;
                case "17":
                    break;
                case "18":
                    break;
                case "19":
                    break;
                case "20":
                    await Task.Run(() => Move_mas_pru(target_id, Convert.ToInt32(textBox_weight.Text)));//ПРУ
                    break;
                case "21":
                    //await Task.Run(() => Move_mas_pru(target_id, Convert.ToInt32(textBox_weight.Text)));//ПРУ
                    break;
                case "22":
                    //await Task.Run(() => Move_mas_pru(target_id, Convert.ToInt32(textBox_weight.Text)));//ПРУ
                    break;
                case "23":

                    break;
            }
        }

        private void Move_mas_bsu(string silo, int mas)
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
                    ///TODO задать правельный ip
                    fds.rfd = libnodave.openSocket(102, "192.168.37.139");
                    fds.wfd = fds.rfd;
                    if (fds.rfd > 0)
                    {
                        di = new libnodave.daveInterface(fds, "IF1", 0, libnodave.daveProtoISOTCP, libnodave.daveSpeed187k);
                        di.setTimeout(50);
                        dc = new libnodave.daveConnection(di, 0, 0, 2);

                        if (0 == dc.connectPLC())
                        {
                            ///TODO уточнить все адреса bity c 17-19
                            if (silo == "17")
                            {
                                res = dc.readBytes(libnodave.daveDB, 12, 100, 4, null);

                                if (res == 0) //conection OK 
                                {
                                    silo_mas = dc.getU32();

                                }
                                silo_mas = silo_mas - mas;
                                res = dc.writeBytes(libnodave.daveDB, 12, 100, 4, BitConverter.GetBytes(libnodave.daveSwapIed_32(silo_mas)));
                            }

                            if (silo == "18")
                            {
                                res = dc.readBytes(libnodave.daveDB, 12, 104, 4, null);

                                if (res == 0) //conection OK 
                                {
                                    silo_mas = dc.getU32();

                                }
                                silo_mas = silo_mas - mas;
                                res = dc.writeBytes(libnodave.daveDB, 12, 104, 4, BitConverter.GetBytes(libnodave.daveSwapIed_32(silo_mas)));
                            }

                            if (silo == "19")
                            {
                                res = dc.readBytes(libnodave.daveDB, 12, 104, 4, null);

                                if (res == 0) //conection OK 
                                {
                                    silo_mas = dc.getU32();

                                }
                                silo_mas = silo_mas - mas;
                                res = dc.writeBytes(libnodave.daveDB, 12, 104, 4, BitConverter.GetBytes(libnodave.daveSwapIed_32(silo_mas)));
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

        private void Move_mas_pru (string silo,  int mas)
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
                                silo_mas = silo_mas - mas;
                                res = dc.writeBytes(libnodave.daveDB, 12, 100, 4, BitConverter.GetBytes(libnodave.daveSwapIed_32(silo_mas)));
                            }

                            if (silo == "2")
                            {
                                res = dc.readBytes(libnodave.daveDB, 12, 104, 4, null);

                                if (res == 0) //conection OK 
                                {
                                    silo_mas = dc.getU32();

                                }
                                silo_mas = silo_mas - mas;
                                res = dc.writeBytes(libnodave.daveDB, 12, 104, 4, BitConverter.GetBytes(libnodave.daveSwapIed_32(silo_mas)));
                            }

                            if (silo == "3")
                            {
                                res = dc.readBytes(libnodave.daveDB, 12, 108, 4, null);

                                if (res == 0) //conection OK 
                                {
                                    silo_mas = dc.getU32();

                                }
                                silo_mas = silo_mas - mas;
                                res = dc.writeBytes(libnodave.daveDB, 12, 108, 4, BitConverter.GetBytes(libnodave.daveSwapIed_32(silo_mas)));
                            }

                            if (silo == "4")
                            {
                                res = dc.readBytes(libnodave.daveDB, 12, 112, 4, null);

                                if (res == 0) //conection OK 
                                {
                                    silo_mas = dc.getU32();

                                }
                                silo_mas = silo_mas - mas;
                                res = dc.writeBytes(libnodave.daveDB, 12, 112, 4, BitConverter.GetBytes(libnodave.daveSwapIed_32(silo_mas)));
                            }

                            if (silo == "5")
                            {
                                res = dc.readBytes(libnodave.daveDB, 12, 116, 4, null);

                                if (res == 0) //conection OK 
                                {
                                    silo_mas = dc.getU32();

                                }
                                silo_mas = silo_mas - mas;
                                res = dc.writeBytes(libnodave.daveDB, 12, 116, 4, BitConverter.GetBytes(libnodave.daveSwapIed_32(silo_mas)));
                            }
                            ///TODO Сверить адреса silo с 20 по 22 (120-124-128)
                            if (silo == "20")
                            {
                                res = dc.readBytes(libnodave.daveDB, 12, 120, 4, null);

                                if (res == 0) //conection OK 
                                {
                                    silo_mas = dc.getU32();

                                }
                                silo_mas = silo_mas - mas;
                                res = dc.writeBytes(libnodave.daveDB, 12, 120, 4, BitConverter.GetBytes(libnodave.daveSwapIed_32(silo_mas)));
                            }

                            if(silo == "21")
                            {
                                res = dc.readBytes(libnodave.daveDB, 12, 124, 4, null);

                                if (res == 0) //conection OK 
                                {
                                    silo_mas = dc.getU32();

                                }
                                silo_mas = silo_mas - mas;
                                res = dc.writeBytes(libnodave.daveDB, 12, 124, 4, BitConverter.GetBytes(libnodave.daveSwapIed_32(silo_mas)));
                            }

                            if(silo == "22")
                            {
                                res = dc.readBytes(libnodave.daveDB, 12, 128, 4, null);

                                if (res == 0) //conection OK 
                                {
                                    silo_mas = dc.getU32();

                                }
                                silo_mas = silo_mas - mas;
                                res = dc.writeBytes(libnodave.daveDB, 12, 128, 4, BitConverter.GetBytes(libnodave.daveSwapIed_32(silo_mas)));
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

        private void Move_mas_sss(string silo, int mas)
        {
            var isComplite = false;

            libnodave.daveOSserialType fds;
            libnodave.daveInterface di;
            libnodave.daveConnection dc;

            try
            {
                int res = 0;
                int silo_mas = 0;

                try
                {
                    fds.rfd = libnodave.openSocket(102, "192.168.37.199");
                    fds.wfd = fds.rfd;

                    if (fds.rfd > 0)
                    {
                        di = new libnodave.daveInterface(fds, "IF1", 0, libnodave.daveProtoISOTCP, libnodave.daveSpeed187k);
                        di.setTimeout(50);
                        dc = new libnodave.daveConnection(di, 0, 0, 2);

                        if (0 == dc.connectPLC())
                        {
                            if (silo == "11")
                            {
                                res = dc.readBytes(libnodave.daveDB, 10, 0, 4, null);

                                if (res == 0) //conection OK 
                                {
                                    silo_mas = dc.getU32();

                                }
                                silo_mas = silo_mas + mas;
                                res = dc.writeBytes(libnodave.daveDB, 10, 0, 4, BitConverter.GetBytes(libnodave.daveSwapIed_32(silo_mas)));
                            }

                            if (silo == "12")
                            {
                                res = dc.readBytes(libnodave.daveDB, 10, 4, 4, null);

                                if (res == 0) //conection OK 
                                {
                                    silo_mas = dc.getU32();

                                }
                                silo_mas = silo_mas + mas;
                                res = dc.writeBytes(libnodave.daveDB, 10, 4, 4, BitConverter.GetBytes(libnodave.daveSwapIed_32(silo_mas)));
                            }

                            if (silo == "13")
                            {
                                res = dc.readBytes(libnodave.daveDB, 10, 8, 4, null);

                                if (res == 0) //conection OK 
                                {
                                    silo_mas = dc.getU32();

                                }
                                silo_mas = silo_mas + mas;
                                res = dc.writeBytes(libnodave.daveDB, 10, 8, 4, BitConverter.GetBytes(libnodave.daveSwapIed_32(silo_mas)));
                            }

                            if (silo == "14")
                            {
                                res = dc.readBytes(libnodave.daveDB, 10, 12, 4, null);

                                if (res == 0) //conection OK 
                                {
                                    silo_mas = dc.getU32();
                                }
                                silo_mas = silo_mas + mas;
                                res = dc.writeBytes(libnodave.daveDB, 10, 12, 4, BitConverter.GetBytes(libnodave.daveSwapIed_32(silo_mas)));
                            }

                            if (silo == "15")
                            {
                                res = dc.readBytes(libnodave.daveDB, 10, 16, 4, null);

                                if (res == 0) //conection OK 
                                {
                                    silo_mas = dc.getU32();
                                }
                                silo_mas = silo_mas + mas;
                                res = dc.writeBytes(libnodave.daveDB, 10, 16, 4, BitConverter.GetBytes(libnodave.daveSwapIed_32(silo_mas)));
                            }
                            ///TODO сверить silo 16 (20)
                            if (silo == "16")
                            {
                                res = dc.readBytes(libnodave.daveDB, 10, 20, 4, null);

                                if (res == 0) //conection OK 
                                {
                                    silo_mas = dc.getU32();
                                }
                                silo_mas = silo_mas + mas;
                                res = dc.writeBytes(libnodave.daveDB, 10, 20, 4, BitConverter.GetBytes(libnodave.daveSwapIed_32(silo_mas)));
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

        private void Move_mas_gb(string silo, int mas)
        {
            string conSQL_gb = "Database=spslogger; Server=192.168.100.26; port=3306; " +
                "username=%user_2; password=20112004; charset=utf8 ";

            libnodave.daveOSserialType fds;
            libnodave.daveInterface di;
            libnodave.daveConnection dc;

            try
            {
                int res = 0;
                int silo_mas = 0;
                string sql = "";

                try
                {
                    fds.rfd = libnodave.openSocket(102, "192.168.37.102");
                    fds.wfd = fds.rfd;

                    if (fds.rfd > 0)
                    {
                        di = new libnodave.daveInterface(fds, "IF1", 0, libnodave.daveProtoISOTCP, libnodave.daveSpeed187k);
                        di.setTimeout(50);
                        dc = new libnodave.daveConnection(di, 0, 0, 2);

                        if (0 == dc.connectPLC())
                        {
                            //string data = tb2.ResultData.Rows[0][1].ToString();
                            //string strSQL4 = "";
                            //DateTime data2 = DateTime.Parse(data);
                            //data = data2.ToString("yyyy-MM-dd HH:mm");
                            //if (on == "6")
                            //{ strSQL4 = "INSERT INTO `spslogger`.`zugang` (`Timestamp`, `Data_56`) VALUES ('MySQLData.MysqlTime(DateTime.Now)', '56326');;"; }
                            //if (on == "7")
                            //{ strSQL4 = "SELECT sum(data_55) FROM `zugang` where `Timestamp`>'" + data + "'  ;"; }
                            //if (on == "8")
                            //{ strSQL4 = "SELECT sum(data_135) FROM `zugang` where `Timestamp`>'" + data + "'  ;"; }
                            //if (on == "9")
                            //{ strSQL4 = "SELECT sum(data_53) FROM `zugang` where `Timestamp`>'" + data + "'  ;"; }
                            //if (on == "10")
                            //{ strSQL4 = "SELECT sum(data_54) FROM `zugang` where `Timestamp`>'" + data + "'  ;"; }

                            if (silo == "7")
                            {
                                sql = "INSERT INTO `spslogger`.`zugang` (`Timestamp`, `Data_55`) VALUES ('" + MySQLData.MysqlTime(DateTime.Now) + "', '" + mas.ToString() + "');";
                                res = dc.readBytes(libnodave.daveDB, 305, 88, 4, null);

                                if (res == 0) //conection OK 
                                {
                                    silo_mas = dc.getU32();

                                }
                                silo_mas = silo_mas + mas;
                                res = dc.writeBytes(libnodave.daveDB, 305, 88, 4, BitConverter.GetBytes(libnodave.daveSwapIed_32(silo_mas)));
                            }

                            if (silo == "6")
                            {
                                sql = "INSERT INTO `spslogger`.`zugang` (`Timestamp`, `Data_56`) VALUES ('"+MySQLData.MysqlTime(DateTime.Now)+"', '"+mas.ToString()+"');";
                                res = dc.readBytes(libnodave.daveDB, 305, 92, 4, null);

                                if (res == 0) //conection OK 
                                {
                                    silo_mas = dc.getU32();

                                }
                                silo_mas = silo_mas + mas;
                                res = dc.writeBytes(libnodave.daveDB, 305, 92, 4, BitConverter.GetBytes(libnodave.daveSwapIed_32(silo_mas)));
                            }

                            if (silo == "8")
                            {
                                sql = "INSERT INTO `spslogger`.`zugang` (`Timestamp`, `Data_135`) VALUES ('" + MySQLData.MysqlTime(DateTime.Now) + "', '" + mas.ToString() + "');";
                                res = dc.readBytes(libnodave.daveDB, 305, 96, 4, null);

                                if (res == 0) //conection OK 
                                {
                                    silo_mas = dc.getU32();

                                }
                                silo_mas = silo_mas + mas;
                                res = dc.writeBytes(libnodave.daveDB, 305, 96, 4, BitConverter.GetBytes(libnodave.daveSwapIed_32(silo_mas)));
                            }

                            if (silo == "10")
                            {
                                sql = "INSERT INTO `spslogger`.`zugang` (`Timestamp`, `Data_54`) VALUES ('" + MySQLData.MysqlTime(DateTime.Now) + "', '" + mas.ToString() + "');";
                                res = dc.readBytes(libnodave.daveDB, 305, 84, 4, null);

                                if (res == 0) //conection OK 
                                {
                                    silo_mas = dc.getU32();

                                }
                                silo_mas = silo_mas + mas;
                                res = dc.writeBytes(libnodave.daveDB, 305, 84, 4, BitConverter.GetBytes(libnodave.daveSwapIed_32(silo_mas)));
                            }

                            if (silo == "9")
                            {
                                sql = "INSERT INTO `spslogger`.`zugang` (`Timestamp`, `Data_53`) VALUES ('" + MySQLData.MysqlTime(DateTime.Now) + "', '" + mas.ToString() + "');";
                                res = dc.readBytes(libnodave.daveDB, 305, 80, 4, null);

                                if (res == 0) //conection OK 
                                {
                                    silo_mas = dc.getU32();

                                }
                                silo_mas = silo_mas + mas;
                                res = dc.writeBytes(libnodave.daveDB, 305, 80, 4, BitConverter.GetBytes(libnodave.daveSwapIed_32(silo_mas)));
                            }
                        }

                        MySqlConnection mCon2 = new MySqlConnection("Database=spslogger; Server=192.168.100.26; port=3306; " +
                            "username=%user_2; password=20112004; charset=utf8 ");

                        MySqlCommand dsq = new MySqlCommand(sql, mCon2);
                        mCon2.Open();

                        try
                        {
                            if (dsq.ExecuteNonQuery() == 1)
                            {
                                // MessageBox.Show("Запись добавлена");
                            }
                            else
                            {
                                MessageBox.Show("Ошибка записи");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                        finally { mCon2.Close(); }
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

        private void zero_plc_sss(string silo)
        {
            // ОБНУЛЕНИЕ В КОНТРОЛЛЕРЕ

            libnodave.daveOSserialType fds;
            libnodave.daveInterface di;
            libnodave.daveConnection dc;
            try
            {
                int res = 0;

                try
                {
                    fds.rfd = libnodave.openSocket(102, "192.168.37.199");
                    fds.wfd = fds.rfd;
                    if (fds.rfd > 0)
                    {


                        di = new libnodave.daveInterface(fds, "IF1", 0, libnodave.daveProtoISOTCP, libnodave.daveSpeed187k);
                        di.setTimeout(50);
                        dc = new libnodave.daveConnection(di, 0, 0, 2);
                        if (0 == dc.connectPLC())

                        {
                            byte[] aa = { 0, 0, 0, 0 };
                            if (silo == "11")
                                res = dc.writeBytes(libnodave.daveDB, 10, 0, 4, aa);
                            if (silo == "12")
                                res = dc.writeBytes(libnodave.daveDB, 10, 4, 4, aa);
                            if (silo == "13")
                                res = dc.writeBytes(libnodave.daveDB, 10, 8, 4, aa);
                            if (silo == "14")
                                res = dc.writeBytes(libnodave.daveDB, 10, 12, 4, aa);
                            if (silo == "15")
                                res = dc.writeBytes(libnodave.daveDB, 10, 16, 4, aa);
                            if (silo == "16")
                                res = dc.writeBytes(libnodave.daveDB, 10, 20, 4, aa);


                            //res = dc.readBits(libnodave.daveDB, 160, 3890, 1, null);
                            //MessageBox.Show("результат функции:" + res + " = " + libnodave.daveStrerror(res));
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
        private void zero_plc_rzd(string silo)
        {
            // ОБНУЛЕНИЕ В КОНТРОЛЛЕРЕ

            libnodave.daveOSserialType fds;
            libnodave.daveInterface di;
            libnodave.daveConnection dc;
            try
            {
                int res = 0;

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
                            byte[] aa = { 0, 0, 0, 0 };
                            if (silo == "1")
                                res = dc.writeBytes(libnodave.daveDB, 12, 100, 4, aa);
                            if (silo == "2")
                                res = dc.writeBytes(libnodave.daveDB, 12, 104, 4, aa);
                            if (silo == "3")
                                res = dc.writeBytes(libnodave.daveDB, 12, 108, 4, aa);
                            if (silo == "4")
                                res = dc.writeBytes(libnodave.daveDB, 12, 112, 4, aa);
                            if (silo == "5")
                                res = dc.writeBytes(libnodave.daveDB, 12, 116, 4, aa);
                            if (silo == "20")
                                res = dc.writeBytes(libnodave.daveDB, 12, 120, 4, aa);

                            //MessageBox.Show("результат функции:" + res + " = " + libnodave.daveStrerror(res));
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

        private void s7_10(string on)
        {
            //string conSQL = "Database=u0550310_aeroblock; Server=31.31.196.234; port=3306; username=u0550_kornev; password=18061981Kornev; charset=utf8 ";
            string conSQL_gb = "Database=spslogger; Server=192.168.100.26; port=3306; username=%user_2; password=20112004; charset=utf8 ";
            string strSQL3 = "SELECT * FROM `silo_balance` where `id`='" + on + "' ;";
            string strSQLdata = "select * from zeroing_silos  where `id_silos`='" + on + "' order by id desc limit 1 ";
            MySQLData.GetData.Result tb1 = MySQLData.GetData.Table(strSQL3, conSQL);
            MySQLData.GetData.Result tb2 = MySQLData.GetData.Table(strSQLdata, conSQL);
            string weight = tb1.ResultData.Rows[0][5].ToString();
            string material = tb1.ResultData.Rows[0][3].ToString();

            string weight_sum_in = "1";
            string weight_sum_out = "1";

            if (tb2.ResultData.Rows.Count == 0)
            {
                weight_sum_in = "1";
                weight_sum_out = "1";
            }
            else
            {
                string data = tb2.ResultData.Rows[0][1].ToString();
                string strSQL4="";
                DateTime data2 = DateTime.Parse(data);
                data = data2.ToString("yyyy-MM-dd HH:mm");
                if (on == "6")
                { strSQL4 = "SELECT sum(data_56) FROM `zugang` where `Timestamp`>'" + data + "'  ;"; }
                if (on == "7")
                { strSQL4 = "SELECT sum(data_55) FROM `zugang` where `Timestamp`>'" + data + "'  ;"; }
                if (on == "8")
                { strSQL4 = "SELECT sum(data_135) FROM `zugang` where `Timestamp`>'" + data + "'  ;"; }
                if (on == "9")
                { strSQL4 = "SELECT sum(data_53) FROM `zugang` where `Timestamp`>'" + data + "'  ;"; }
                if (on == "10")
                { strSQL4 = "SELECT sum(data_54) FROM `zugang` where `Timestamp`>'" + data + "'  ;"; }
                

                MySQLData.GetScalar.Result wsi = MySQLData.GetScalar.Scalar(strSQL4, conSQL_gb);
                weight_sum_in = wsi.ResultText;
                if (weight_sum_in == "")
                    weight_sum_in = "1";

            }

            //ExecutQuery(strSQL3);

            Dictionary<string, string> str = new Dictionary<string, string>();
            str.Add("`date`", MySQLData.MysqlTime(DateTime.Now));
            str.Add("weight", weight);
            str.Add("weight_sum_in", weight_sum_in);
            str.Add("weight_sum_out", weight_sum_out);
            str.Add("material", material);
            str.Add("id_silos", on);

            string keys, values;
            MySQLData.ConvertInsertData(str, out keys, out values);
            string strSQL = "insert into zeroing_silos (" + keys + ") values (" + values + ");";
            bool isok = false;
            while (!isok)
            {
                MySQLData.GetScalar.Result wres = MySQLData.GetScalar.NoResponse(strSQL, conSQL);
                if (wres.HasError == true)
                { isok = false; Thread.Sleep(500); }
                else
                {
                    isok = true;
                    zero_plc_gb(on);
                }
            }
        }

        private void s11_16(string on)
        {
            string conSQL = ConfigurationManager.ConnectionStrings["234"].ConnectionString;
            //string conSQL_gb = "Database=spslogger; Server=192.168.100.17; port=3306; username=%user_2; password=20112004; charset=utf8 ";
            string strSQL3 = "SELECT * FROM `silo_balance` where `id`='" + on + "' ;";
            string strSQLdata = "select * from zeroing_silos  where `id_silos`='" + on + "' order by id desc limit 1 ";
            MySQLData.GetData.Result tb1 = MySQLData.GetData.Table(strSQL3, conSQL);
            MySQLData.GetData.Result tb2 = MySQLData.GetData.Table(strSQLdata, conSQL);
            string weight = tb1.ResultData.Rows[0][5].ToString();
            string material = tb1.ResultData.Rows[0][3].ToString();

            string weight_sum_in = "1";
            string weight_sum_out = "1";

            if (tb2.ResultData.Rows.Count == 0)
            {
                weight_sum_in = "1";
                weight_sum_out = "1";
            }
            else
            {
                string data = tb2.ResultData.Rows[0][1].ToString();
                string strSQL4 = "";
                DateTime data2 = DateTime.Parse(data);
                data = data2.ToString("yyyy-MM-dd HH:mm");
              
                strSQL4 =("SELECT sum(weight) FROM `move_silo` where `timestamp`>'" + data + "' and `source_silo_id`='" + on+"'  ;"); 

                MySQLData.GetScalar.Result wsi = MySQLData.GetScalar.Scalar(strSQL4, conSQL);
                weight_sum_in = wsi.ResultText;
                if (weight_sum_in == "")
                    weight_sum_in = "1";

            }

            //ExecutQuery(strSQL3);

            Dictionary<string, string> str = new Dictionary<string, string>
            {
                { "`date`", MySQLData.MysqlTime(DateTime.Now) },
                { "weight", weight },
                { "weight_sum_in", weight_sum_in },
                { "weight_sum_out", weight_sum_out },
                { "material", material },
                { "id_silos", on }
            };

            string keys, values;
            MySQLData.ConvertInsertData(str, out keys, out values);
            string strSQL = "insert into zeroing_silos (" + keys + ") values (" + values + ");";
            bool isok = false;
            while (!isok)
            {
                MySQLData.GetScalar.Result wres = MySQLData.GetScalar.NoResponse(strSQL, conSQL);
                if (wres.HasError == true)
                { isok = false; Thread.Sleep(500); }
                else
                {
                    isok = true;
                    zero_plc_sss(on);
                }
            }
        }

        private void Targe_sours_silo (string target_id, string source_id)
        {
            //string conSQL = "Database=u0550310_aeroblock; Server=31.31.196.234; port=3306; username=u0550_kornev; password=18061981Kornev; charset=utf8 ";

            Dictionary<string, string> str = new Dictionary<string, string>
            {
                { "`timestamp`", MySQLData.MysqlTime(DateTime.Now) },
                { "target_silo_id", target_id },
                { "target_silo_material", textBox_name_material_source.Text.ToString() },
                { "target_silo_sendler", textBox_sendler_source.Text.ToString() },
                { "source_silo_id", source_id },
                { "source_silo_material", textBox_material_target.Text.ToString() },
                { "source_silo_sendler", textBox_sendler_source.Text.ToString() },
                { "weight", textBox_weight.Text.ToString() }
            };

            string keys, values;
            MySQLData.ConvertInsertData(str, out keys, out values);
            string strSQL = "insert into move_silo (" + keys + ") values (" + values + ");";
            bool isok = false;
            while (!isok)
            {
                MySQLData.GetScalar.Result wres = MySQLData.GetScalar.NoResponse(strSQL, conSQL);
                if (wres.HasError == true)
                { isok = false; Thread.Sleep(500); }
                else
                {
                    isok = true;
                    
                    MessageBox.Show("Перемещение проведено", "Перемещение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //textBox_weight.Text = "";
                }
            }
        }

        private void ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            pass pass = new pass();
            pass.Show();
            pass.button_pass.MouseClick += (senderSlave, eSlave) =>
            {
                if (pass.textBox_pass.Text == "08082014")
                {
                    pass.Close();
                    DialogResult result = MessageBox.Show("Действительно обнулить силос? ", "Обнуление силоса", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == DialogResult.No)
                    {
                        return;
                    }
                    if (result == DialogResult.Yes)
                    {
                        ToolStripMenuItem cms = (ToolStripMenuItem)sender;
                        ContextMenuStrip strip = (ContextMenuStrip)cms.Owner;
                        Control owner = strip.SourceControl;
                        // MessageBox.Show(owner.Name);
                        string owner_name = owner.Name.ToString();
                        owner_name = owner_name.Remove(0, 1);
                        if (owner_name == "1" | owner_name == "2" | owner_name == "3" | owner_name == "4" | owner_name == "5" | owner_name == "20")//ПРУ
                            s1_6(owner_name);
                        if (owner_name == "6" | owner_name == "7" | owner_name == "8" | owner_name == "9" | owner_name == "10")//газобетон
                            s7_10(owner_name);
                        if (owner_name == "11" | owner_name == "12" | owner_name == "13" | owner_name == "14" | owner_name == "15" | owner_name == "16")//сухие смеси
                            s11_16(owner_name);

                        GetData();
                        Update_visualSilo();
                    }
                }
            };
        }

        private void GetData()
        {
            string sql = ("SELECT * FROM silo_balance where id<>22;");
            MySqlDataAdapter dD = new MySqlDataAdapter(sql, mCon);
            DataSet ds = new DataSet();
            ds.Reset();
            dD.Fill(ds, sql);
            dataGridView1.Invoke((MethodInvoker)delegate
            {
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
                dataGridView1.Columns[5].DefaultCellStyle.Format = "N0";
                foreach (DataGridViewRow item in dataGridView1.Rows)
                {

                    if (item.Cells[1].Value.ToString() == "БСУ")
                    {
                        item.DefaultCellStyle.BackColor = Color.LightBlue;
                    }
                    if (item.Cells[1].Value.ToString() == "ПРУ")
                    {
                        item.DefaultCellStyle.BackColor = Color.LightSalmon;
                    }
                    if (item.Cells[1].Value.ToString() == "ГАЗОБЕТОН")
                    {
                        item.DefaultCellStyle.BackColor = Color.LemonChiffon;
                    }
                    if (item.Cells[1].Value.ToString() == "ССС")
                    {
                        item.DefaultCellStyle.BackColor = Color.GreenYellow;
                    }
                    if (item.Cells[1].Value.ToString() == "Кирпич")
                    {
                        item.DefaultCellStyle.BackColor = Color.Moccasin;
                    }
                }
                Console.WriteLine("Обновление прошло");
#if user
            groupBox5.Visible = false;
            contextMenuStrip1.Enabled = false;
#endif
                this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            });
        }

        private void read_group_box (Control parent, int id)
        {
            string val_1 = parent.Controls["label_s" + (id + 1).ToString() + "_name"].Text.ToString();
            //parent.Controls["label_s" + (id + 1).ToString() + "_sender"].
            //parent.Controls["label_s" + (id + 1).ToString() + "_balance"].Text 
        }

        private void fill_group_box(Control parent, int id)
        {
            try
            {
                int str = Convert.ToInt32(dataGridView1.Rows[id].Cells[5].Value.ToString());
                parent.Controls["label_s" + (id + 1).ToString() + "_name"].Text = dataGridView1.Rows[id].Cells[3].Value.ToString();
                parent.Controls["label_s" + (id + 1).ToString() + "_sender"].Text = dataGridView1.Rows[id].Cells[4].Value.ToString();
                parent.Controls["label_s" + (id + 1).ToString() + "_balance"].Text = string.Format("{0:N0}", str);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            //string str2=string.Format("{0:N0}", str);
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

        private void Update_visualSilo()
        {
            List<GroupBox> gb = new List<GroupBox>
            {
                s1,
                s2,
                s3,
                s4,
                s5,
                s6,
                s7,
                s8,
                s9,
                s10,
                s11,
                s12,
                s13,
                s14,
                s15,
                s16,
                s17,
                s18,
                s19,
                s20,
                s21,
                s22,
                s23
            };

            int id;

            for (id = 0; id <= 22; id++)
            {
                fill_group_box(gb[id], id);
            }
        }

        private void Balance_Load(object sender, EventArgs e)
        {
            PLC_RZD();
            fill_cb();
            GetData();
            Fg();
            fill_cb("manufactur", comboBox_manufaktur_target);
            fill_cb("silo_num", comboBox_manufaktur_target.SelectedValue.ToString(), comboBox_silo_num_target);
            fill_tb("silo_material_name", comboBox_manufaktur_target.SelectedValue.ToString(), comboBox_silo_num_target.SelectedValue.ToString(), textBox_material_target);
            fill_tb("weight", comboBox_manufaktur_target.SelectedValue.ToString(), comboBox_silo_num_target.SelectedValue.ToString(), textBox_weight_target);

            // comboBox_silo_num_target.Text

            comboBox1.Text = "1";

            Update_visualSilo();
        }

        private void Fg()
        {
            string sql;
            string val = comboBox1.SelectedValue.ToString();
            switch (val)
            {
                case "20":
                    sql = ("SELECT * FROM silo_balance where manufactur='ПРУ' and silo_num='6'");
                    break;
                case "23":
                    sql = ("SELECT * FROM silo_balance where manufactur='ПРУ' and silo_num='7'");
                    break;
                case "24":
                    sql = ("SELECT * FROM silo_balance where manufactur='ПРУ' and silo_num='8'");
                    break;
                default:
                    sql = ("SELECT * FROM silo_balance where manufactur='ПРУ' and silo_num='" + val + "'");
                    break;
            }
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

        private async void Button1_Click(object sender, EventArgs e)
        {
            Fg();
            string target_id, source_id;
            string val = comboBox1.SelectedValue.ToString();
            string source_manuf = comboBox_manufaktur_target.Text.ToString();
            string source_num = comboBox_silo_num_target.Text.ToString();
            string sql;
            if (val == "20")
                sql = ("SELECT `id` FROM silo_balance where manufactur='ПРУ' and silo_num='6'");
            else
                sql = ("SELECT `id` FROM silo_balance where manufactur='ПРУ' and silo_num='" + val + "'");
            MySQLData.GetScalar.Result wsi = MySQLData.GetScalar.Scalar(sql, conSQL);
            target_id = wsi.ResultText;          
            string sql2 = ("SELECT `id` FROM silo_balance where manufactur='"+source_manuf+"' and silo_num='" + source_num + "'");
            MySQLData.GetScalar.Result wsi2 = MySQLData.GetScalar.Scalar(sql2, conSQL);
            source_id = wsi2.ResultText;

            if (textBox_weight.Text.ToString() == "")
            {
                MessageBox.Show("Введите вес перемещаемого материала", "ОШИБКА ВВОДА", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                DialogResult result = MessageBox.Show("Провести перемещение материала? ", "Перемещение материала", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.No)
                {
                    return;
                }

                if (result == DialogResult.Yes)
                {
                    button1.Visible = false;
                    //Targe_sours_silo(target_id, source_id);
                    //if (source_num=="6")
                    //Move_mas_pru("20", Convert.ToInt32(textBox_weight.Text));
                    //else


                    if (source_id == "1" | source_id == "2" | source_id == "3" | source_id == "4" | source_id == "5" | source_id == "20")//ПРУ
                        await move_mas(target_id, source_id, Convert.ToInt32(textBox_weight.Text));
                        //await Task.Run(() => Move_mas_pru(target_id, Convert.ToInt32(textBox_weight.Text)));
                    if (source_id == "6" | source_id == "7" | source_id == "8" | source_id == "9" | source_id == "10")//газобетон
                        await move_mas(target_id, source_id, Convert.ToInt32(textBox_weight.Text));
                    //await Task.Run(() => Move_mas_gb(source_id, Convert.ToInt32(textBox_weight.Text)));
                    if (source_id == "11" | source_id == "12" | source_id == "13" | source_id == "14" | source_id == "15")//сухие смеси
                        await move_mas(target_id, source_id, Convert.ToInt32(textBox_weight.Text));
                    //await Task.Run(() => Move_mas_sss(source_id, Convert.ToInt32(textBox_weight.Text))); 
                    //if (source_id == "17" | source_id == "18" | source_id == "19" | source_id == "16" | source_id == "15")//сухие смеси
                    //textBox_weight.Text = "";
                    
                    PLC_RZD();
                    button1.Visible = true;
                }
            }
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

        private void НаименованиеМатериалаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem cms = (ToolStripMenuItem)sender;
            ContextMenuStrip strip = (ContextMenuStrip)cms.Owner;
            Control owner = strip.SourceControl;
            string owner_nmae = owner.Name.ToString();
            owner_nmae = owner_nmae.Remove(0, 1);
            
            switch (owner_nmae)
            {
                case "21":
                    owner_nmae = "23";
                    break;
                case "22":
                    owner_nmae = "24";
                    break;
                case "23":
                    owner_nmae = "25";
                    break;
                default:
                    break;
            }

            material form6 = new material();
            form6.Show();
            string material_name;
            form6.dataGridView1.MouseDoubleClick += (senderSlave, eSlave) =>
            {
               material_name = form6.dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                form6.Close();
                string strSQL3 = "update silo_balance set silo_material_name ='" + material_name + "' where `id`='" + owner_nmae + "' ;";
                ExecutQuery(strSQL3);
                GetData();
                Update_visualSilo();
            };
            form6.button2.Click += (senderSlave, eSlave) =>
            {
               material_name = form6.dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                form6.Close();
                string strSQL3 = "update silo_balance set silo_material_name ='" + material_name + "' where `id`='" + owner_nmae + "' ;";
                ExecutQuery(strSQL3);
                GetData();
                Update_visualSilo();
            };
        }

        private void ПоставщикToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem cms = (ToolStripMenuItem)sender;
            ContextMenuStrip strip = (ContextMenuStrip)cms.Owner;
            Control owner = strip.SourceControl;
            string owner_nmae = owner.Name.ToString();
            owner_nmae = owner_nmae.Remove(0, 1);

            switch (owner_nmae)
            {
                case "21":
                    owner_nmae = "23";
                    break;
                case "22":
                    owner_nmae = "24";
                    break;
                case "23":
                    owner_nmae = "25";
                    break;
                default:
                    break;
            }

            string material_name;
            sendler form4 = new sendler();
            form4.Show();
            form4.dataGridView2.MouseDoubleClick += (senderSlave, eSlave) =>
            {
                material_name = form4.dataGridView2.SelectedRows[0].Cells[1].Value.ToString();
                form4.Close();
                string strSQL3 = "update silo_balance set silo_name_sendler ='" + material_name + "' where `id`='" + owner_nmae + "' ;";
                ExecutQuery(strSQL3);
                GetData();
                Update_visualSilo();
            };
            form4.butenterF4.Click += (senderSlave, eSlave) =>
            {
                material_name = form4.dataGridView2.SelectedRows[0].Cells[1].Value.ToString();
                form4.Close();
                string strSQL3 = "update silo_balance set silo_name_sendler ='" + material_name + "' where `id`='" + owner_nmae + "' ;";
                ExecutQuery(strSQL3);
                GetData();
                Update_visualSilo();
            };

        }

        public void GetValueFromControllerByte_SSS(ref int s11, ref int s12, ref int s13, ref int s14, ref int s15, ref int s16)
        {
            libnodave.daveOSserialType fds;
            libnodave.daveInterface di;
            libnodave.daveConnection dc;

            s16 = 0;
            s15 = 0;
            s14 = 0;
            s13 = 0;
            s12 = 0;
            s11 = 0;
            try
            {
                int res = 0;
                //byte[] buffer = new byte[mvByteValue];
                //byte[] swapBuffer = new byte[mvByteValue];
                //int s6, s7, s8, s9, s10;
                //s6 = Convert.ToDouble(plc.Read("db305.dbd88"));
                //s7 = Convert.ToDouble(plc.Read("db305.dbd92"));
                //s8 = Convert.ToDouble(plc.Read("db305.dbd96"));
                //s9 = Convert.ToDouble(plc.Read("db305.dbd84"));
                //s10 = Convert.ToDouble(plc.Read("db305.dbd80"));

                try
                {
                    //Наверное сухие смеси
                    fds.rfd = libnodave.openSocket(102, "192.168.37.199");
                    fds.wfd = fds.rfd;
                    if (fds.rfd > 0)
                    {

                        di = new libnodave.daveInterface(fds, "IF1", 0, libnodave.daveProtoISOTCP, libnodave.daveSpeed187k);
                        di.setTimeout(500);
                        dc = new libnodave.daveConnection(di, 0, 0, 2);
                        if (0 == dc.connectPLC())

                        {
                            res = dc.readBytes(libnodave.daveDB, 10, 0, 4, null);

                            if (res == 0) //conection OK 
                            {
                                s11 = dc.getU32();

                            }
                            res = dc.readBytes(libnodave.daveDB, 10, 4, 4, null);

                            if (res == 0) //conection OK 
                            {
                                s12 = dc.getU32();

                            }
                            res = dc.readBytes(libnodave.daveDB, 10, 8, 4, null);

                            if (res == 0) //conection OK 
                            {
                                s13 = dc.getU32();

                            }
                            res = dc.readBytes(libnodave.daveDB, 10, 12, 4, null);

                            if (res == 0) //conection OK 
                            {
                                s14 = dc.getU32();

                            }
                            res = dc.readBytes(libnodave.daveDB, 10, 16, 4, null);

                            if (res == 0) //conection OK 
                            {
                                s15 = dc.getU32();

                            }
                            ///TODO Проверить адрес bity для 16
                            res = dc.readBytes(libnodave.daveDB, 10, 20, 4, null);

                            if (res == 0) //conection OK 
                            {
                                s16 = dc.getU32();

                            }
                            //res = dc.readBits(libnodave.daveDB, 160, 3890, 1, null);
                            //MessageBox.Show("результат функции:" + res + " = " + libnodave.daveStrerror(res));
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

        private void GetValueFromControllerByteBrick(ref int s23)
        {
            libnodave.daveOSserialType fds;
            libnodave.daveInterface di;
            libnodave.daveConnection dc;

            s23 = 0;

            try
            {
                int res = 0;

                try
                {
                    //кирпич
                    fds.rfd = libnodave.openSocket(102, "192.168.37.199");
                    fds.wfd = fds.rfd;
                    if (fds.rfd > 0)
                    {

                        di = new libnodave.daveInterface(fds, "IF1", 0, libnodave.daveProtoISOTCP, libnodave.daveSpeed187k);
                        di.setTimeout(500);
                        dc = new libnodave.daveConnection(di, 0, 0, 2);

                        if (0 == dc.connectPLC())
                        {
                            //Заменить данные
                            res = dc.readBytes(libnodave.daveDB, 305, 88, 4, null);

                            if (res == 0) //conection OK 
                            {
                                s23 = dc.getU32();

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

        public void GetValueFromControllerByte(ref int s6, ref int s7, ref int s8, ref int s9, ref int s10)
        {
            libnodave.daveOSserialType fds;
            libnodave.daveInterface di;
            libnodave.daveConnection dc;

            s10 = 0;
            s6 = 0;
            s7 = 0;
            s8 = 0;
            s9 = 0;
            try
            {
                int res = 0;
                //byte[] buffer = new byte[mvByteValue];
                //byte[] swapBuffer = new byte[mvByteValue];
                //int s6, s7, s8, s9, s10;
                //s6 = Convert.ToDouble(plc.Read("db305.dbd88"));
                //s7 = Convert.ToDouble(plc.Read("db305.dbd92"));
                //s8 = Convert.ToDouble(plc.Read("db305.dbd96"));
                //s9 = Convert.ToDouble(plc.Read("db305.dbd84"));
                //s10 = Convert.ToDouble(plc.Read("db305.dbd80"));

                try
                {
                    fds.rfd = libnodave.openSocket(102, "192.168.37.102");
                    fds.wfd = fds.rfd;
                    if (fds.rfd > 0)
                    {

                        di = new libnodave.daveInterface(fds, "IF1", 0, libnodave.daveProtoISOTCP, libnodave.daveSpeed187k);
                        di.setTimeout(500);
                        dc = new libnodave.daveConnection(di, 0, 0, 2);
                        if (0 == dc.connectPLC())

                        {
                            res = dc.readBytes(libnodave.daveDB, 305, 88, 4, null);

                            if (res == 0) //conection OK 
                            {
                                s7 = dc.getU32();

                            }
                            res = dc.readBytes(libnodave.daveDB, 305, 92, 4, null);

                            if (res == 0) //conection OK 
                            {
                                s6 = dc.getU32();

                            }
                            res = dc.readBytes(libnodave.daveDB, 305, 96, 4, null);

                            if (res == 0) //conection OK 
                            {
                                s8 = dc.getU32();

                            }
                            res = dc.readBytes(libnodave.daveDB, 305, 84, 4, null);

                            if (res == 0) //conection OK 
                            {
                                s10 = dc.getU32();

                            }
                            res = dc.readBytes(libnodave.daveDB, 305, 80, 4, null);

                            if (res == 0) //conection OK 
                            {
                                s9 = dc.getU32();

                            }
                            //int a = 0;
                            //a= libnodave.daveSwapIed_32(a+44513);
                            //res=dc.writeBytes(libnodave.daveDB, 305, 92, 4, BitConverter.GetBytes(a));

                            //res = dc.readBits(libnodave.daveDB, 160, 3890, 1, null);
                            //MessageBox.Show("результат функции:" + res + " = " + libnodave.daveStrerror(res));




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

        private async void UpdatePLC()
        {
            //ПРУ
            List<GrouBoxS> grouBoxSPZD = new List<GrouBoxS>() {new GrouBoxS(s1, 100), new GrouBoxS(s2, 104), new GrouBoxS(s3, 108), 
                new GrouBoxS(s4, 112), new GrouBoxS(s5, 116), new GrouBoxS(s20, 120),new GrouBoxS(s21, 124), new GrouBoxS(s22, 128)};
            //Газобетон
            List<GrouBoxS> grouBoxSDaerocrete = new List<GrouBoxS>() { new GrouBoxS(s6), new GrouBoxS(s7), new GrouBoxS(s8), new GrouBoxS(s9), new GrouBoxS(s10) };
            //Сухие смеси
            List<GrouBoxS> grouBoxSDryMixes = new List<GrouBoxS>() { new GrouBoxS(s6), new GrouBoxS(s7), new GrouBoxS(s8), new GrouBoxS(s9), new GrouBoxS(s10) };

            PLC_RZD(grouBoxSPZD);
            GetValueFromControllerByte(ref s6, ref s7, ref s8, ref s9, ref s10);
            GetValueFromControllerByte_SSS(ref s11, ref s12, ref s13, ref s14, ref s15, ref s16);

            List<GrouBoxS> fullItems = new List<GrouBoxS>();
            fullItems.AddRange(grouBoxSPZD);
            fullItems.AddRange(grouBoxSDaerocrete);
            fullItems.AddRange(grouBoxSDryMixes);
            
            List<double> value = new List<double>();
            foreach (var item in fullItems)
            {
                value.Add(item.GetTextInt());
            }

            UpdateData(value);
        }

        private async void PLC_RZD(List<GrouBoxS> grouBoxS)
        {
            string _errorMessage;
            List<int> addresses = new List<int>();

            foreach (var item in grouBoxS)
            {
                var elementAdress = item.GetAdress();

                if (elementAdress != 0 && elementAdress != default)
                {
                    addresses.Add(elementAdress);
                }
            }
            try
            {
                var ipAddress = "192.168.37.139";
                var dbNumber = 12;

                var cancellationToken = new CancellationTokenSource();
                cancellationToken.CancelAfter(TimeSpan.FromSeconds(30)); // Отмена через 30 секунд

                // Формируем строку запроса
                var addressString = string.Join(",", addresses);
                var requestUriString = $"/api/PLCPRU/GetDatePRU?ipAddress={ipAddress}&dbNumber={dbNumber}";


                if (!client.DefaultRequestHeaders.Accept.Any(h => h.MediaType == "text/plain"))
                {
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("text/plain"));
                }

                // Сериализуем тело запроса в JSON
                var content = new StringContent(JsonConvert.SerializeObject(addresses), Encoding.UTF8, "application/json");

                // Выполняем запрос
                var response = await client.PostAsync(requestUriString, content, cancellationToken.Token);

                // Проверяем успешность запроса
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var adresses = JsonConvert.DeserializeObject<List<Adress>>(jsonString);
                    int resultParse = 0;
                    bool isComliteParse;

                    foreach (var date in adresses)
                    {
                        foreach (var item in grouBoxS)
                        {
                            isComliteParse = int.TryParse(date._value.ToString(), out resultParse);

                            if (isComliteParse)
                            {
                                item.SetText(resultParse.ToString(), date._addres);
                            }
                            else
                            {
                                item.SetText(-, date._addres);
                            }
                        }
                    }

                    // Логируем успешный результат
                    Console.WriteLine("Данные успешно получены: {Values}", string.Join(", ", adresses));
                }
                else
                {
                    // Логируем ошибку HTTP
                    _errorMessage = $"Ошибка HTTP-запроса: {(int)response.StatusCode} - {response.ReasonPhrase}";
                    Console.WriteLine(_errorMessage);
                    MessageBox.Show(_errorMessage);
                }
            }
            catch (HttpRequestException ex)
            {
                _errorMessage = $"Ошибка HTTP-запроса: {ex.Message}";
                Console.WriteLine(_errorMessage);
                MessageBox.Show(_errorMessage);
            }
            catch (TaskCanceledException ex)
            {
                _errorMessage = "Запрос был отменён (таймаут или отмена токеном).";
                Console.WriteLine(_errorMessage);
                MessageBox.Show(_errorMessage);
            }
            catch (Exception ex)
            {
                _errorMessage = "Произошла неожиданная ошибка.";
                Console.WriteLine(_errorMessage);
                MessageBox.Show(_errorMessage);
            }
        }

        private bool UpdateData(int id, double value)
        {
            string conSql = "UPDATE `u0550310_aeroblock`.`silo_balance` SET `weight` = '" + value + "' WHERE (`id` = '" + (id) + "');";

            MySqlCommand dsq = new MySqlCommand(conSQL, mCon);
            try
            {
                mCon.Open();
                if (dsq.ExecuteNonQuery() == 1)
                {
                    return true;
                }
                else
                {
                    Update_visualSilo();
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                if (mCon != null && mCon.State != System.Data.ConnectionState.Closed) mCon.Clone();
                return false;
            }
            finally
            {
                mCon.Close();
            }
        }

        private void UpdateData(List<double> var)
         {
            MySqlConnection mCon = new MySqlConnection(ConfigurationManager.ConnectionStrings["234"].ConnectionString);
            //MySqlConnection mCon = new MySqlConnection("Database=spslogger; Server=192.168.37.101; port=3306; username=%user_1; password=20112004; charset=utf8 ");

            int id;
            for (id = 0; id < var.Count; id++)
            {
                string conSQL = null;

                if (id >= 20)
                {
                    conSQL = "UPDATE `u0550310_aeroblock`.`silo_balance` SET `weight` = '" + var[id].ToString() + "' WHERE (`id` = '" + (id + 2).ToString() + "');";
                }
                else
                {
                    conSQL = "UPDATE `u0550310_aeroblock`.`silo_balance` SET `weight` = '" + var[id].ToString() + "' WHERE (`id` = '" + (id + 1).ToString() + "');";
                }
                MySqlCommand dsq = new MySqlCommand(conSQL, mCon);
                try
                {
                    mCon.Open();
                    if (dsq.ExecuteNonQuery() == 1)
                    {
                        // MessageBox.Show("Запись добавлена");
                    }
                    else
                    {
                        MessageBox.Show("Ошибка записи");

                        Update_visualSilo();
                    }
                    // MessageBox.Show("OK");
                    mCon.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    if (mCon != null && mCon.State != System.Data.ConnectionState.Closed) mCon.Clone();
                }

            }
        } 

        private void ToolStripButton1_Click(object sender, EventArgs e)
        {
            fill_cb();
            GetData();
            Fg();
            fill_cb("manufactur", comboBox_manufaktur_target);
            fill_cb("silo_num", comboBox_manufaktur_target.SelectedValue.ToString(), comboBox_silo_num_target);
            fill_tb("silo_material_name", comboBox_manufaktur_target.SelectedValue.ToString(), comboBox_silo_num_target.SelectedValue.ToString(), textBox_material_target);
            fill_tb("weight", comboBox_manufaktur_target.SelectedValue.ToString(), comboBox_silo_num_target.SelectedValue.ToString(), textBox_weight_target);

            // comboBox_silo_num_target.Text

            comboBox1.Text = "1";
            //
            //PLC_RZD();
            Update_visualSilo();
        }

    }
}
    






