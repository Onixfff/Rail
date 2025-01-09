using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Threading;
using DataUpdater;
using System.Configuration;
using System.Net.Http;
using System.Linq;
using Newtonsoft.Json;
using System.Text;
using rail.Models;

namespace rail
{
    public partial class balance : Form
    {
        //MySqlConnection mCon = new MySqlConnection("Database=u0550310_aeroblock; Server=31.31.196.234; port=3306; username=u0550_kornev; password=18061981Kornev; charset=utf8 ");
        MySqlConnection mCon = new MySqlConnection(ConfigurationManager.ConnectionStrings["234"].ConnectionString);
        string conSQL = ConfigurationManager.ConnectionStrings["234"].ConnectionString;
        MySqlCommand msd;
        private object label;

        LogFileManager logFileManager = new LogFileManager();

        Dictionary<string, int> plcIpPRU = new Dictionary<string, int>() { { "192.168.37.139", 12 } };
        Dictionary<string, int> plcIpDaerocrete = new Dictionary<string, int>() { { "192.168.37.102", 305 } };
        Dictionary<string, int> plcIpDryMixes =  new Dictionary<string, int>() { { "192.168.37.199", 10 } };

        private static readonly HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://192.168.100.100:5048")
        };

        public balance()
        {
            InitializeComponent();
        }
        
        private async Task OpenConAsync(MySqlConnection mCon)
        {
            try
            {
                if (mCon.State == ConnectionState.Closed)
                {
                    await mCon.OpenAsync();
                }
            }
            catch(MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show("Ошибка открытия подключения к базе данных");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show("Непонятная ошибка");
            }
            finally
            {
                await mCon.CloseAsync();
            }
        }

        private async Task CloseConAsync(MySqlConnection mCon)
        {
            try
            {
                if (mCon.State == ConnectionState.Open)
                {
                    await mCon.CloseAsync();
                }
            }
            catch(MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show("Ошибка закрытия подключения к базе данных");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show("Непонятная ошибка");
            }
            finally
            {
                await mCon.CloseAsync();
            }
        }

        public async Task ExecutQuery(string q, MySqlConnection mCon)
        {
            try
            {
                await OpenConAsync(mCon);

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
            finally { await CloseConAsync(mCon); }
        }

        private async Task<bool> move_mas(string target_id, string source_id, int mas)
        {
            string error = "Ошибка перемещения";
            bool isCompliteSource, isCompliteTarget, isDataReturn;
            (GrouBoxS grouBoxS, Dictionary<string, int> ipAdres , string error) result;

            result = GetNumberId(source_id);

            if (result.error != null)
            {
                MessageBox.Show("Не удалось получить данные об plc");
                return false;
            }

            isCompliteSource = await MoveMasPLC(result.grouBoxS, result.ipAdres , mas);

            if (!isCompliteSource)
            {
                MessageBox.Show(error);
                return false;
            }

            isCompliteTarget = await MoveTargetMas(target_id, plcIpPRU , mas);

            if (!isCompliteTarget)
            {
                isDataReturn = await MoveMasPLC(result.grouBoxS, result.ipAdres, mas);

                if (!isDataReturn)
                {
                    logFileManager.AddLog($"Не получилось вернуть данные обратно в source - {result.grouBoxS.GetAdress()}|| Какие данные - {mas}");
                    return false;
                }
            }

            return isCompliteTarget;
        }

        private async Task<bool> MoveTargetMas(string target_id, Dictionary<string, int> ipAdres, int mas)
        {
            mas = mas * -1;//Конвертирую значение в минусовое для изьятия данных;
            bool isComplite = false;
            string error = "Ошибка перемещения";
            (GrouBoxS grouBoxS, string error) result;

            switch (target_id)
            {
                case "1":
                    result = GetGrouBoxSPZD(1);
                    break;
                case "2":
                    result = GetGrouBoxSPZD(2);
                    break;
                case "3":
                    result = GetGrouBoxSPZD(3);
                    break;
                case "4":
                    result = GetGrouBoxSPZD(4);
                    break;
                case "5":
                    result = GetGrouBoxSPZD(5);
                    break;
                case "20":
                    result = GetGrouBoxSPZD(20);
                    break;
                case "21":
                    result = GetGrouBoxSPZD(21);
                    break;
                case "22":
                    result = GetGrouBoxSPZD(22);
                    break;
                case "23":
                    result = GetGrouBoxSPZD(23);
                    break;
                default:
                    isComplite = false;
                    result = (GetGrouBoxSPZD(1).grouBoxS, "Таких данных нету");
                    break;
            }

            if (result.error != null)
            {
                MessageBox.Show("Не удалось получить данные об plc в списках\n" + result.error);
            }
            else
            {
                isComplite = await MoveMasPLC(result.grouBoxS, ipAdres , mas); //ПРУ

                if (!isComplite)
                {
                    MessageBox.Show(error);
                    logFileManager.AddLog($"Перенос Target каких данных не произошел - {result.grouBoxS.GetAdress()}|| Какие данные - {mas}");
                }
            }

            return isComplite;
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
                    fds.rfd = libnodave.openSocket(102, plcIpPRU.Keys.FirstOrDefault());
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

        private async Task<bool> MoveMasPLC (GrouBoxS grouBoxS, Dictionary<string, int> ipAdres, int mas)
        {
            bool isComplite = false;

            isComplite = await UpdateDatePlc(grouBoxS, ipAdres.Keys.FirstOrDefault(), ipAdres.Values.FirstOrDefault(), mas);

            if (isComplite)
            {
                Console.WriteLine("Успешно прошло перемещение");
            }
            else
            {
                Console.WriteLine("Ошибка перемещения");
                return isComplite;
            }
            return isComplite;
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
                    fds.rfd = libnodave.openSocket(102, plcIpDryMixes.Keys.FirstOrDefault());
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
                    fds.rfd = libnodave.openSocket(102, plcIpDaerocrete.Keys.FirstOrDefault());
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

        private (GrouBoxS grouBoxS, Dictionary<string, int> , string error) GetNumberId(string numberId)
        {
            (GrouBoxS grouBoxS,Dictionary<string, int> ipAdres , string error) result;
            (GrouBoxS grouBoxS, string error) demoResult;

            switch (numberId)
            {
                case "1":
                    demoResult = GetGrouBoxSPZD(1);
                    result = (demoResult.grouBoxS, plcIpPRU, demoResult.error);
                    break;
                case "2":
                    demoResult = GetGrouBoxSPZD(2);
                    result = (demoResult.grouBoxS, plcIpPRU, demoResult.error);
                    break;
                case "3":
                    demoResult = GetGrouBoxSPZD(3);
                    result = (demoResult.grouBoxS, plcIpPRU, demoResult.error);
                    break;
                case "4":
                    demoResult = GetGrouBoxSPZD(4);
                    result = (demoResult.grouBoxS, plcIpPRU, demoResult.error);
                    break;
                case "5":
                    demoResult = GetGrouBoxSPZD(5);
                    result = (demoResult.grouBoxS, plcIpPRU, demoResult.error);
                    break;
                case "6":
                    demoResult = GetGrouBoxSPZD(6);
                    result = (demoResult.grouBoxS, plcIpDaerocrete, demoResult.error);
                    break;
                case "7":
                    demoResult = GetGrouBoxSPZD(7);
                    result = (demoResult.grouBoxS, plcIpDaerocrete, demoResult.error);
                    break;
                case "8":
                    demoResult = GetGrouBoxSPZD(8);
                    result = (demoResult.grouBoxS, plcIpDaerocrete, demoResult.error);
                    break;
                case "9":
                    demoResult = GetGrouBoxSPZD(9);
                    result = (demoResult.grouBoxS, plcIpDaerocrete, demoResult.error);
                    break;
                case "10":
                    demoResult = GetGrouBoxSPZD(10);
                    result = (demoResult.grouBoxS, plcIpDaerocrete, demoResult.error);
                    break;
                case "11":
                    demoResult = GetGrouBoxSPZD(11);
                    result = (demoResult.grouBoxS, plcIpDryMixes, demoResult.error);
                    break;
                case "12":
                    demoResult = GetGrouBoxSPZD(12);
                    result = (demoResult.grouBoxS, plcIpDryMixes, demoResult.error);
                    break;
                case "13":
                    demoResult = GetGrouBoxSPZD(13);
                    result = (demoResult.grouBoxS, plcIpDryMixes, demoResult.error);
                    break;
                case "14":
                    demoResult = GetGrouBoxSPZD(14);
                    result = (demoResult.grouBoxS, plcIpDryMixes, demoResult.error);
                    break;
                case "15":
                    demoResult = GetGrouBoxSPZD(15);
                    result = (demoResult.grouBoxS, plcIpDryMixes, demoResult.error);
                    break;
                case "16":
                    result = (GetGrouBoxSPZD(1).grouBoxS,null , "Нету таких данных");
                    break;
                case "17":
                    result = (GetGrouBoxSPZD(1).grouBoxS, null,"Нету таких данных");
                    break;
                case "18":
                    result = (GetGrouBoxSPZD(1).grouBoxS, null ,"Нету таких данных");
                    break;
                case "19":
                    result = (GetGrouBoxSPZD(1).grouBoxS, null ,"Нету таких данных");
                    break;
                case "20":
                    demoResult = GetGrouBoxSPZD(20);
                    result = (demoResult.grouBoxS, plcIpPRU, demoResult.error);
                    break;
                case "21":
                    demoResult = GetGrouBoxSPZD(21);
                    result = (demoResult.grouBoxS, plcIpPRU, demoResult.error);
                    break;
                case "22":
                    demoResult = GetGrouBoxSPZD(22);
                    result = (demoResult.grouBoxS, plcIpPRU, demoResult.error);
                    break;
                case "23":
                    demoResult = GetGrouBoxSPZD(23);
                    result = (demoResult.grouBoxS, plcIpPRU, demoResult.error);
                    break;
                default:
                    result = (GetGrouBoxSPZD(1).grouBoxS,null , "Нету таких данных");
                    break;
            }

            return result;
        }

        private async Task<bool> zero_plc_gb(string silo)
        {
            bool isComplite = false;
            string error = "Ошибка перемещения";

            (GrouBoxS grouBoxS, Dictionary<string, int> ipAdres, string error) result = GetNumberId(silo);

            if (result.error != null)
            {
                MessageBox.Show("Не удалось получить данные об plc");
                return false;
            }

            isComplite = await UpdateZeroPlc(result.grouBoxS, result.ipAdres.Keys.FirstOrDefault(), result.ipAdres.Values.FirstOrDefault());

            if (isComplite == false)
            {
                MessageBox.Show(error);
            }

            return isComplite;

            // ОБНУЛЕНИЕ В КОНТРОЛЛЕРЕ

            libnodave.daveOSserialType fds;
            libnodave.daveInterface di;
            libnodave.daveConnection dc;

            try
            {
                int res = 0;

                try
                {
                    fds.rfd = libnodave.openSocket(102, plcIpDaerocrete.Keys.FirstOrDefault());
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
                    fds.rfd = libnodave.openSocket(102, plcIpDryMixes.Keys.FirstOrDefault());
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
                    fds.rfd = libnodave.openSocket(102, plcIpPRU.Keys.FirstOrDefault());
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
                    zero_plc_rzd(on);
                }
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
                try
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
                catch(MySqlException ex)
                {
                    Console.WriteLine("Ошибка перемещения " + ex.Message);
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Ошибка перемещения " + ex.Message);
                }
            }
        }

        private void ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem cms = (ToolStripMenuItem)sender;
            ContextMenuStrip strip = (ContextMenuStrip)cms.Owner;
            Control owner = strip.SourceControl;
            string owner_name = owner.Name.ToString();
            owner_name = owner_name.Remove(0, 1);

            owner_name = ChangeOwner_nmae(owner_name);

            pass pass = new pass();
            pass.Show();
            pass.button_pass.MouseClick += (senderSlave, eSlave) =>
            {
                if (pass.textBox_pass.Text == "08082014")
                { 
                    DialogResult result  = MessageBox.Show("Действительно обнулить силос? ", "Обнуление силоса", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == DialogResult.No)
                    {
                        return;
                    }
                    if (result == DialogResult.Yes)
                    {
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
            string sql = ("SELECT * FROM silo_balance where id <> 22;");
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
            try
            {
                string sql = ("SELECT * FROM silo_balance where manufactur='ПРУ' ");
                MySqlDataAdapter dD = new MySqlDataAdapter(sql, mCon);
                DataTable tbl1 = new DataTable();
                dD.Fill(tbl1);

                comboBox1.DataSource = tbl1;
                comboBox1.DisplayMember = "silo_num";// столбец для отображения
                comboBox1.ValueMember = "id";
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Ошибка в fill_cb");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка в fill_cb");
            }
            finally
            {
                mCon.Close();
            }
        }

        private void fill_cb(string sql_disp, ComboBox cb)
        {
            try
            {
                string sql = ("SELECT distinct " + sql_disp + " FROM silo_balance");
                MySqlDataAdapter dD = new MySqlDataAdapter(sql, mCon);
                DataTable tbl1 = new DataTable();
                dD.Fill(tbl1);
                cb.DataSource = tbl1;
                cb.DisplayMember = sql_disp;// столбец для отображения
                cb.ValueMember = sql_disp;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Ошибка в fill_cb");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка в fill_cb");
            }
            finally
            {
                mCon.Close();
            }
        }

        private void fill_cb(string sql_disp, string sql_us, ComboBox cb)
        {
            try
            {
                string sql = ("SELECT distinct " + sql_disp + " FROM silo_balance where  manufactur= '" + sql_us + "'");
                MySqlDataAdapter dD = new MySqlDataAdapter(sql, mCon);
                DataTable tbl1 = new DataTable();
                dD.Fill(tbl1);

                cb.DataSource = tbl1;
                cb.DisplayMember = sql_disp;// столбец для отображения
                cb.ValueMember = sql_disp;
            }
            catch(MySqlException ex)
            {
                MessageBox.Show("Ошибка в fill_cb");
            }
            catch(Exception ex)
            {
                MessageBox.Show("Ошибка в fill_cb");
            }
            finally
            {
                mCon.Close();
            }
        }

        private void fill_tb(string sql_disp, string sql_us, string sql_us2, TextBox tb)
        {
            string sql = ("SELECT " + sql_disp + " FROM silo_balance where  manufactur= '" + sql_us + "' and silo_num='" + sql_us2 + "' ");
            
            using (msd = new MySqlCommand(sql, mCon))
            {
                try
                {
                    mCon.Open();
                    tb.Text = msd.ExecuteScalar().ToString();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                finally 
                { 
                    mCon.Close();
                }
            }
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

            for (id = 0; id <= gb.Count -1; id++)
            {
                fill_group_box(gb[id], id);
            }
        }

        private void Balance_Load(object sender, EventArgs e)
        {
            UpdatePLC(); //Обновление данных из плс в бд
            fill_cb(); //Получение данные из ПРУ (скорее всего ПРУ это стоковое значение) в combobox1
            GetData(); //Загрузка данных
            Fg(); //Загрузка вроде как базовых материалов
            fill_cb("manufactur", comboBox_manufaktur_target);//Выводит данные об manufactur в comboBox_manufaktur_target (Отображает пользователю)
            fill_cb("silo_num", comboBox_manufaktur_target.SelectedValue.ToString(), comboBox_silo_num_target); //
            fill_tb("silo_material_name", comboBox_manufaktur_target.SelectedValue.ToString(), comboBox_silo_num_target.SelectedValue.ToString(), textBox_material_target);//Выводит информацию об label_s?_name из данных по manufaktur и по его silo_num
            fill_tb("weight", comboBox_manufaktur_target.SelectedValue.ToString(), comboBox_silo_num_target.SelectedValue.ToString(), textBox_weight_target);//Короче берет данные и заносит их в Textbox_weight_target

            // comboBox_silo_num_target.Text

            comboBox1.Text = "1";

            Update_visualSilo();
        }

        private void Fg()
        {
            string sql;
            string val = comboBox1.SelectedValue.ToString();
            sql = GetVal(val);
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

            sql = GetVal(val);

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
                    int mas;

                    bool resultParse = Int32.TryParse(textBox_weight.Text, out mas);

                    if (resultParse)
                    {
                        bool isCopmliteMove = await move_mas(target_id, source_id, mas);
                        UpdatePLC();

                        if (!isCopmliteMove)
                        {
                            Console.WriteLine("Ошибка переноса данных");
                        }
                        else
                        {
                            Targe_sours_silo(target_id, source_id);
                            //Сюда добавить работу с базой данных.
                        }
                    }
                    else
                    {
                        MessageBox.Show("Невозможное значение в поле `Вес`");
                    }

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

            owner_nmae = ChangeOwner_nmae(owner_nmae);

            material form6 = new material();
            form6.Show();
            string material_name;
            form6.dataGridView1.MouseDoubleClick += (senderSlave, eSlave) =>
            {
               material_name = form6.dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                form6.Close();
                string strSQL3 = "update silo_balance set silo_material_name ='" + material_name + "' where `id`='" + owner_nmae + "' ;";
                ExecutQuery(strSQL3, new MySqlConnection(ConfigurationManager.ConnectionStrings["234"].ConnectionString));
                GetData();
                Update_visualSilo();
            };
            form6.button2.Click += (senderSlave, eSlave) =>
            {
               material_name = form6.dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                form6.Close();
                string strSQL3 = "update silo_balance set silo_material_name ='" + material_name + "' where `id`='" + owner_nmae + "' ;";
                ExecutQuery(strSQL3, new MySqlConnection(ConfigurationManager.ConnectionStrings["234"].ConnectionString));
                GetData();
                Update_visualSilo();
            };
        }

        private string ChangeOwner_nmae(string owner_nmae)
        {
            string result;

            switch (owner_nmae)
            {
                case "21":
                    result = "23";
                    break;
                case "22":
                    result = "24";
                    break;
                case "23":
                    result = "25";
                    break;
                default:
                    result = owner_nmae;
                    break;
            }

            return result;
        }

        private void ПоставщикToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem cms = (ToolStripMenuItem)sender;
            ContextMenuStrip strip = (ContextMenuStrip)cms.Owner;
            Control owner = strip.SourceControl;
            string owner_nmae = owner.Name.ToString();
            owner_nmae = owner_nmae.Remove(0, 1);

            owner_nmae = ChangeOwner_nmae(owner_nmae);

            string material_name;
            sendler form4 = new sendler();
            form4.Show();
            form4.dataGridView2.MouseDoubleClick += (senderSlave, eSlave) =>
            {
                material_name = form4.dataGridView2.SelectedRows[0].Cells[1].Value.ToString();
                form4.Close();
                string strSQL3 = "update silo_balance set silo_name_sendler ='" + material_name + "' where `id`='" + owner_nmae + "' ;";
                ExecutQuery(strSQL3, new MySqlConnection(ConfigurationManager.ConnectionStrings["234"].ConnectionString));
                GetData();
                Update_visualSilo();
            };
            form4.butenterF4.Click += (senderSlave, eSlave) =>
            {
                material_name = form4.dataGridView2.SelectedRows[0].Cells[1].Value.ToString();
                form4.Close();
                string strSQL3 = "update silo_balance set silo_name_sendler ='" + material_name + "' where `id`='" + owner_nmae + "' ;";
                ExecutQuery(strSQL3 , new MySqlConnection(ConfigurationManager.ConnectionStrings["234"].ConnectionString));
                GetData();
                Update_visualSilo();
            };

        }

        private async void UpdatePLC()
        {
            var grouBoxSPZD = GetListGrouBoxSPZD();
            var grouBoxSDaerocrete = GetListGrouBoxSDaerocrete();
            var grouBoxSDryMixes = GetListGrouBoxSDryMixes();

            await PLC_RZDAsync(grouBoxSPZD, plcIpPRU.Keys.FirstOrDefault(), plcIpPRU.Values.FirstOrDefault());
            await PLC_RZDAsync(grouBoxSDaerocrete, plcIpDaerocrete.Keys.FirstOrDefault(), plcIpDaerocrete.Values.FirstOrDefault());
            //await PLC_RZDAsync(grouBoxSDryMixes, plcIpDryMixes.Keys.FirstOrDefault(), plcIpDryMixes.Values.FirstOrDefault());

            //Делаю компановку данных
            List<GrouBoxS> fullItems = new List<GrouBoxS>();
            fullItems.AddRange(grouBoxSPZD);
            fullItems.AddRange(grouBoxSDaerocrete);
           //fullItems.AddRange(grouBoxSDryMixes);

             UpdateData(fullItems);
        }

        private async Task PLC_RZDAsync(List<GrouBoxS> grouBoxS, string ipAddress, int dbNumber)
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
                    var adresses = JsonConvert.DeserializeObject<List<AdressDto>>(jsonString);
                    int resultParse = 0;
                    bool isComliteParse;

                    foreach (var date in adresses)
                    {
                        foreach (var item in grouBoxS)
                        {
                            if (date.Addres == item.GetAdress())
                            {
                                isComliteParse = int.TryParse(date.ConvertToAdress()._value.ToString(), out resultParse);

                                if (isComliteParse)
                                {
                                    item.SetText(resultParse.ToString(), date.ConvertToAdress()._addres);
                                    break;
                                }
                                else
                                {
                                    item.SetText("0", date.ConvertToAdress()._addres);
                                }
                                break;
                            }
                        }
                    }

                    // Логируем успешный результат
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
                Console.WriteLine(_errorMessage + "\n" + ex.Message);
                MessageBox.Show(_errorMessage);
            }
        }

        private async Task<bool> UpdateDatePlc(GrouBoxS grouBoxS, string ipAddress, int dbNumber, int mas)
        {
            string _errorMessage;
            List<int> addresses = new List<int>();

            if (grouBoxS != null)
            {
                var elementAdress = grouBoxS.GetAdress();

                if (elementAdress != 0 && elementAdress != default)
                {
                    addresses.Add(elementAdress);
                }
            }
            else
            {
                Console.WriteLine("Ошибка UpdateDatePlc grouBoxS не содержит данных");
                MessageBox.Show("Ошибка UpdateDatePlc grouBoxS не содержит данных");
                return false;
            }

            try
            {
                var cancellationToken = new CancellationTokenSource();
                cancellationToken.CancelAfter(TimeSpan.FromSeconds(30)); // Отмена через 30 секунд

                // Формируем строку запроса
                var requestUriString = $"/api/PLCPRU/ChangeDatePRU?ipAddress={ipAddress}&dbNumber={dbNumber}&addresses={addresses}&mas={mas}";

                // Выполняем запрос
                var response = await client.GetAsync(requestUriString, cancellationToken.Token);

                // Проверяем успешность запроса
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<(bool isComplite, string error)>(jsonString);

                    if(result.isComplite == true)
                    {
                        Console.WriteLine("Обновление plc прошло успешно");
                        return true;
                    }
                    else
                    {
                        MessageBox.Show($"Ошибка обновления \n{result.error}");
                        Console.WriteLine($"Ошибка обновления \n{result.error}");
                    }
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
                Console.WriteLine(_errorMessage + "\n" + ex.Message);
                MessageBox.Show(_errorMessage);
            }

            return false;
        }

        private async Task<bool> UpdateZeroPlc(GrouBoxS grouBoxS, string ipAddress, int dbNumber)
        {
            string _errorMessage;
            List<int> addresses = new List<int>();

            if (grouBoxS != null)
            {
                var elementAdress = grouBoxS.GetAdress();

                if (elementAdress != 0 && elementAdress != default)
                {
                    addresses.Add(elementAdress);
                }
            }
            else
            {
                Console.WriteLine("Ошибка UpdateDatePlc grouBoxS не содержит данных");
                MessageBox.Show("Ошибка UpdateDatePlc grouBoxS не содержит данных");
                return false;
            }

            try
            {
                var cancellationToken = new CancellationTokenSource();
                cancellationToken.CancelAfter(TimeSpan.FromSeconds(30)); // Отмена через 30 секунд

                // Формируем строку запроса
                var requestUriString = $"/api/PLCPRU/UpdateDateZeroPlc?ipAddress={ipAddress}&dbNumber={dbNumber}&addresses={addresses}";

                // Выполняем запрос
                var response = await client.GetAsync(requestUriString, cancellationToken.Token);

                // Проверяем успешность запроса
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<(bool isComplite, string error)>(jsonString);

                    if (result.isComplite == true)
                    {
                        Console.WriteLine("Обнуление PLC прошло успешно");
                        return true;
                    }
                    else
                    {
                        MessageBox.Show($"Ошибка обновления \n{result.error}");
                        Console.WriteLine($"Ошибка обновления \n{result.error}");
                    }
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
                Console.WriteLine(_errorMessage + "\n" + ex.Message);
                MessageBox.Show(_errorMessage);
            }

            return false;
        }

        private async Task<bool> UpdateData(int id, double value)
        {
            string conSql = "UPDATE `u0550310_aeroblock`.`silo_balance` SET `weight` = '" + value + "' WHERE (`id` = '" + (id) + "');";
            try
            {
                await mCon.OpenAsync();

                using (MySqlCommand dsq = new MySqlCommand(conSQL, mCon))
                {
                    if (await dsq.ExecuteNonQueryAsync() == 1)
                    {
                        return true;
                    }
                    else
                    {
                        Update_visualSilo();
                        return false;
                    }
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
                await mCon.CloseAsync();
            }
        }

        private async Task UpdateData(List<GrouBoxS> var)
        {
            MySqlConnection mCon = new MySqlConnection(ConfigurationManager.ConnectionStrings["234"].ConnectionString);
            int idDb;
            int id;

            for (id = 0; id < var.Count; id++)
            {
                idDb = var[id].GetIdDb();
                int weight = var[id].GetTextInt();
                string conSQL = null;

                conSQL = "UPDATE `u0550310_aeroblock`.`silo_balance` SET `weight` = '" + weight + "' WHERE (`id` = '" + (idDb).ToString() + "');";

                try
                {
                    await mCon.OpenAsync();

                    using (MySqlCommand dsq = new MySqlCommand(conSQL, mCon))
                    {

                        if (await dsq.ExecuteNonQueryAsync() == 1)
                        {
                            // MessageBox.Show("Запись добавлена");
                        }
                        else
                        {
                            MessageBox.Show("Ошибка записи");
                            Update_visualSilo();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    await mCon.CloseAsync();
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

        private List<GrouBoxS> GetListGrouBoxSDaerocrete()
        {
            string
                s6 = default,
                s7 = default,
                s8 = default,
                s9 = default,
                s10 = default;

            //Газобетонs
            List<GrouBoxS> grouBoxSDaerocrete = new List<GrouBoxS>()
            {
                new GrouBoxS(s6, 92, 6),
                new GrouBoxS(s7, 88, 7),
                new GrouBoxS(s8, 96, 8),
                new GrouBoxS(s9, 80, 9),
                new GrouBoxS(s10, 84, 10)
            };

            return grouBoxSDaerocrete;
        }

        private (GrouBoxS grouBoxS, string error) GetListGrouBoxSDaerocrete(int id)
        {
            GrouBoxS grouBoxS;

            string
                s6 = default,
                s7 = default,
                s8 = default,
                s9 = default,
                s10 = default;

            switch (id)
            {
                case 92:
                    grouBoxS = new GrouBoxS(s6, 92, 1);
                    break;

                case 88:
                    grouBoxS = new GrouBoxS(s7, 88, 2);
                    break;

                case 96:
                    grouBoxS = new GrouBoxS(s8, 96, 2);
                    break;

                case 80:
                    grouBoxS = new GrouBoxS(s9, 80, 2);
                    break;

                case 84:
                    grouBoxS = new GrouBoxS(s10, 84, 2);
                    break;

                default:
                    grouBoxS = null;
                    break;
            }

            if (grouBoxS != null)
            {
                return (grouBoxS, null);
            }
            else
            {
                return (grouBoxS, "grouBoxS == null");
            }
        }

        private List<GrouBoxS> GetListGrouBoxSPZD()
        {
            string
                s1 = default,
                s2 = default,
                s3 = default,
                s4 = default,
                s5 = default,
                s20 = default,
                s21 = default,
                s22 = default;

            //ПРУ
            List<GrouBoxS> grouBoxSPZD = new List<GrouBoxS>()
            {
                new GrouBoxS(s1, 100, 1),
                new GrouBoxS(s2, 104, 2),
                new GrouBoxS(s3, 108, 3),
                new GrouBoxS(s4, 112, 4),
                new GrouBoxS(s5, 116, 5),
                new GrouBoxS(s20, 120, 20),
                new GrouBoxS(s21, 124, 23),
                new GrouBoxS(s22, 128, 24)
            };

            return grouBoxSPZD;
        }

        private (GrouBoxS grouBoxS, string error) GetGrouBoxSPZD(int id)
        {
            GrouBoxS grouBoxS;

            string
                s1 = default,
                s2 = default,
                s3 = default,
                s4 = default,
                s5 = default,
                s20 = default,
                s21 = default,
                s22 = default;

            switch (id)
            {
                case 100:
                    grouBoxS = new GrouBoxS(s1, 100, 1);
                    break;

                case 104:
                    grouBoxS = new GrouBoxS(s2, 104, 2);
                    break;

                case 108:
                    grouBoxS = new GrouBoxS(s3, 108, 2);
                    break;

                case 112:
                    grouBoxS = new GrouBoxS(s4, 112, 2);
                    break;

                case 116:
                    grouBoxS = new GrouBoxS(s5, 116, 2);
                    break;

                case 120:
                    grouBoxS = new GrouBoxS(s20, 120, 2);
                    break;

                case 124:
                    grouBoxS = new GrouBoxS(s21, 124, 2);
                    break;

                case 128:
                    grouBoxS = new GrouBoxS(s22, 128, 2);
                    break;

                default:
                    grouBoxS = null;
                    break;
            }

            if(grouBoxS != null)
            {
                return (grouBoxS, null);
            }
            else
            {
                return (grouBoxS, "grouBoxS == null");
            }

        }

        private List<GrouBoxS> GetListGrouBoxSDryMixes()
        {
            string
                s11 = default,
                s12 = default,
                s13 = default,
                s14 = default,
                s15 = default,
                s16 = default;

            //Сухие смеси
            List<GrouBoxS> grouBoxSDryMixes = new List<GrouBoxS>()
            {
                new GrouBoxS(s11,0, 11),
                new GrouBoxS(s12,4, 12),
                new GrouBoxS(s13,8, 13),
                new GrouBoxS(s14,12, 14),
                new GrouBoxS(s15,16, 15),
                new GrouBoxS(s16,20, 16)
            };

            return grouBoxSDryMixes;
        }

        private (GrouBoxS grouBoxS, string error) GetGrouBoxSDryMixes(int id)
        {
            GrouBoxS grouBoxS;

            string
                s11 = default,
                s12 = default,
                s13 = default,
                s14 = default,
                s15 = default,
                s16 = default;

            switch (id)
            {
                case 0:
                    grouBoxS = new GrouBoxS(s11, 0, 1);
                    break;

                case 4:
                    grouBoxS = new GrouBoxS(s12, 4, 2);
                    break;

                case 8:
                    grouBoxS = new GrouBoxS(s13, 8, 2);
                    break;

                case 12:
                    grouBoxS = new GrouBoxS(s14, 12, 2);
                    break;

                case 16:
                    grouBoxS = new GrouBoxS(s15, 16, 2);
                    break;

                case 20:
                    grouBoxS = new GrouBoxS(s16, 20, 2);
                    break;

                default:
                    grouBoxS = null;
                    break;
            }

            if (grouBoxS != null)
            {
                return (grouBoxS, null);
            }
            else
            {
                return (grouBoxS, "grouBoxS == null");
            }
        }

        private string GetVal(string val)
        {
            string sql;
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

            return sql;
        }
    }
}
    






