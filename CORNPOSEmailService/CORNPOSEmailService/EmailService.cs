using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;
using System.ServiceProcess;
using System.Text;
using System.Timers;
using Timer = System.Timers.Timer;
using CORNCommon.Classes;
using CrystalDecisions.CrystalReports.Engine;
using CORNBusinessLayer.Reports;

namespace CORNPOSEmailService
{
    public partial class EmailService : ServiceBase
    {
        static string path = string.Empty;
        static string conString = "";
        static string conString2 = "";
        Timer timer = new Timer();
        static DataTable dtDatabases = new DataTable();
        static DataTable dtEmailSetting = new DataTable();
      
        public EmailService()
        {
            InitializeComponent();
            path = AppDomain.CurrentDomain.BaseDirectory;
            conString = "server=" + Decrypt(System.Configuration.ConfigurationManager.AppSettings["server"].ToString(), "b0tin@74")
                + ";uid=" + Decrypt(System.Configuration.ConfigurationManager.AppSettings["uid"].ToString(), "b0tin@74")
                + ";pwd=" + Decrypt(System.Configuration.ConfigurationManager.AppSettings["pwd"].ToString(), "b0tin@74")
                + ";database=" + Decrypt(System.Configuration.ConfigurationManager.AppSettings["database"].ToString(), "b0tin@74");
            timer.Elapsed += PerformTimerOperation;
            timer.Interval = TimeSpan.FromMinutes(30).TotalMilliseconds;
            //timer.Interval = TimeSpan.FromSeconds(30).TotalMilliseconds;
            timer.Start();
        }
        protected override void OnStart(string[] args)
        {
            WriteLog("Service Started.");
            WriteLog("Version: 1.0");
            timer.Enabled = false;
            timer.Stop();
            dtDatabases.Columns.Add("DBName", typeof(string));
            dtDatabases.Columns.Add("SendDailySalesReport", typeof(string));

            dtEmailSetting.Columns.Add("TypeID", typeof(int));
            dtEmailSetting.Columns.Add("EmailTime", typeof(string));
            dtEmailSetting.Columns.Add("CompanyName", typeof(string));
            dtEmailSetting.Columns.Add("EmailFrom", typeof(string));
            dtEmailSetting.Columns.Add("EmailTo", typeof(string));
            dtEmailSetting.Columns.Add("Password", typeof(string));
            dtEmailSetting.Columns.Add("SMTP", typeof(string));
            dtEmailSetting.Columns.Add("Subject", typeof(string));
            dtEmailSetting.Columns.Add("Body", typeof(string));

            DataTable dt = GetDatabases();
            foreach (DataRow drDb in dt.Rows)
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand(string.Format("USE {0} IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_NAME = 'tblAppSettingDetail') SELECT strColumnValue AS SendDailySalesReport FROM tblAppSettingDetail WHERE strColumnName = 'SendDailySalesReport' AND ISNULL(strColumnValue,'0') = 1", drDb["name"].ToString()), con))
                        {
                            try
                            {
                                using (IDataReader dr = cmd.ExecuteReader())
                                {
                                    while (dr.Read())
                                    {
                                        DataRow drNew = dtDatabases.NewRow();
                                        drNew["DBName"] = drDb["name"].ToString();
                                        drNew["SendDailySalesReport"] = dr[0].ToString();
                                        dtDatabases.Rows.Add(drNew);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                WriteLog(ex.ToString());
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                    con.Close();
                }
            }
            if (dtDatabases.Rows.Count > 0)
            {
                WriteLog("<<<<<<<<<<------------------------------------------>>>>>>>>>");
                WriteLog("Following Dbs have Email Daily Sales Report enabled");
                foreach (DataRow dr in dtDatabases.Rows)
                {
                    WriteLog(dr["DBName"].ToString());
                }
                WriteLog("<<<<<<<<<<------------------------------------------>>>>>>>>>");
            }

            foreach (DataRow drDB in dtDatabases.Rows)
            {
                GetEmail(drDB["DBName"].ToString());
            }
            
            timer.Enabled = true;
            timer.Start();
        }
        protected override void OnStop()
        {
            WriteLog("Service Stopped.");
        }
        void PerformTimerOperation(object sender, ElapsedEventArgs e)
        {
            timer.Enabled = false;
            timer.Stop();
            //foreach (DataRow drDB in dtDatabases.Rows)
            //{
            //    foreach (DataRow dr in dtEmailSetting.Rows)
            //    {
            //        string[] emailto = dr["EmailTo"].ToString().Split(',');
            //        string EmailFrom = dr["EmailFrom"].ToString();
            //        string SMTP = dr["SMTP"].ToString();
            //        string Password = dr["Password"].ToString();
            //        string Subject = dr["Subject"].ToString();
            //        string Body = dr["Body"].ToString();
            //        if (Convert.ToDateTime(dr["EmailTime"].ToString()).ToString("hh:mm") == DateTime.Now.ToString("hh:mm"))
            //        {
            //            if (dr["TypeID"].ToString() == "1")
            //            {
            //                DataTable dtEmailDailySalesHistory = GetEmailHistory(drDB["DBName"].ToString(), Convert.ToDateTime(System.DateTime.Now.ToString("dd-MMM-yyyy")), 1);
            //                string filenameDailySales = "";
            //                if (dtEmailDailySalesHistory.Rows.Count == 0)
            //                {
            //                    if (GenerateDailySaleReport(dr["CompanyName"].ToString(), drDB["DBName"].ToString()))
            //                    {
            //                        string file_path = System.Configuration.ConfigurationManager.AppSettings["ReportPath"].ToString();
            //                        if (!Directory.Exists(file_path))
            //                        {
            //                            Directory.CreateDirectory(file_path);
            //                        }
            //                        filenameDailySales = file_path + @"\" + dr["CompanyName"].ToString() + "_DailySaleReport.pdf";
            //                        if (EmailReport(filenameDailySales, emailto, EmailFrom, SMTP, Password, Subject, Body))
            //                        {
            //                            InsertEmailHistory(drDB["DBName"].ToString(), 1);
            //                        }
            //                    }
            //                }
            //            }
            //            if (dr["TypeID"].ToString() == "2")
            //            {
            //                DataTable dtEmailItemWiseSaleHistory = GetEmailHistory(drDB["DBName"].ToString(), Convert.ToDateTime(System.DateTime.Now.ToString("dd-MMM-yyyy")), 2);
            //                string filenameItemWiseSales = "";
            //                if (dtEmailItemWiseSaleHistory.Rows.Count == 0)
            //                {
            //                    if (GenerateItemWiseSaleReport(dr["CompanyName"].ToString(), drDB["DBName"].ToString()))
            //                    {
            //                        string file_path = System.Configuration.ConfigurationManager.AppSettings["ReportPath"].ToString();
            //                        if (!Directory.Exists(file_path))
            //                        {
            //                            Directory.CreateDirectory(file_path);
            //                        }
            //                        filenameItemWiseSales = file_path + @"\" + dr["CompanyName"].ToString() + "_ItemWiseSaleReport.pdf";
            //                        if (EmailReport(filenameItemWiseSales, emailto, EmailFrom, SMTP, Password, Subject, Body))
            //                        {
            //                            InsertEmailHistory(drDB["DBName"].ToString(), 2);
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
            WriteLog("PerformTimerOperation started");
            DataTable dtData = GetWrongCalculatedData(Convert.ToDateTime(System.Configuration.ConfigurationManager.AppSettings["Date"].ToString()));
            WriteLog("Wrong Data " + dtData.Rows.Count.ToString() + " records found");
            if (dtData.Rows.Count > 0)
            {
                foreach(DataRow dr in dtData.Rows)
                {
                    WriteLog("Invoice No: " + dr["SaleInvoiceID"].ToString() + " , TimeStamp: " + dr["TimeStamp"].ToString() + " , LastUpdateDate: " + dr["LastUpdateDate"].ToString() + " , Detail Amount: " + dr["DetailAmount"].ToString() + " , AmountDue: " + dr["GrossAmount"].ToString());
                }
                CorrectData(Convert.ToDateTime(System.Configuration.ConfigurationManager.AppSettings["Date"].ToString()));
                WriteLog("Wrong Data corrected");
            }
            timer.Enabled = true;
            timer.Start();
        }
        private bool GenerateDailySaleReport(string CompanyName,string DBName)
        {
            bool flag = true;
            string filename = string.Empty;
            try
            {
                DataSet dsDailySalesReport = GetDailySalesReport(DBName, DateTime.Parse(System.DateTime.Now.AddDays(-1).ToString("dd-MMM-yyyy")), DateTime.Parse(System.DateTime.Now.AddDays(-1).ToString("dd-MMM-yyyy")));
                CrpLocationDSR crpReport = new CrpLocationDSR();
                ReportDocument crCoverTables = crpReport.OpenSubreport("crCoverTables");
                ReportDocument sbDelivery = crpReport.OpenSubreport("sbDelivery");
                crpReport.SetDataSource(dsDailySalesReport);
                crCoverTables.SetDataSource(dsDailySalesReport);
                sbDelivery.SetDataSource(dsDailySalesReport);
                crpReport.Refresh();
                crpReport.SetParameterValue("Location", "All");
                crpReport.SetParameterValue("FROM_DATE", DateTime.Parse(System.DateTime.Now.AddDays(-1).ToString("dd-MMM-yyyy")));
                crpReport.SetParameterValue("TO_DATE", DateTime.Parse(System.DateTime.Now.AddDays(-1).ToString("dd-MMM-yyyy")));
                crpReport.SetParameterValue("CompanyName", CompanyName);
                crpReport.SetParameterValue("Address", "Location");
                crpReport.SetParameterValue("ReportName", "Location Wise Sales Report");
                string file_path = System.Configuration.ConfigurationManager.AppSettings["ReportPath"].ToString();
                if (!Directory.Exists(file_path))
                {
                    Directory.CreateDirectory(file_path);
                }
                filename = file_path + @"\" + CompanyName + "_DailySaleReport.pdf";
                crpReport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, filename);
                crpReport.Close();
                crpReport.Dispose();                
            }
            catch (Exception ex)
            {
                flag = false;
                WriteLog(ex.Message);
            }
            return flag;
        }

        private bool GenerateItemWiseSaleReport(string CompanyName, string DBName)
        {
            bool flag = true;
            string filename = string.Empty;
            try
            {
                DataSet dsItemWiseSalesReport = GetItemWiseSalesReport(DBName, DateTime.Parse(System.DateTime.Now.AddDays(-1).ToString("dd-MMM-yyyy")), DateTime.Parse(System.DateTime.Now.AddDays(-1).ToString("dd-MMM-yyyy")));
                CrpRegionWiseSale crpReport = new CrpRegionWiseSale();
                crpReport.SetDataSource(dsItemWiseSalesReport);
                crpReport.Refresh();

                crpReport.SetParameterValue("fromDate", DateTime.Parse(System.DateTime.Now.AddDays(-1).ToString("dd-MMM-yyyy")));
                crpReport.SetParameterValue("todate", DateTime.Parse(System.DateTime.Now.AddDays(-1).ToString("dd-MMM-yyyy")));
                crpReport.SetParameterValue("CompanyName", CompanyName);
                crpReport.SetParameterValue("ReportTitle", "Item Wise Sales Report");
                string file_path = System.Configuration.ConfigurationManager.AppSettings["ReportPath"].ToString();
                if (!Directory.Exists(file_path))
                {
                    Directory.CreateDirectory(file_path);
                }
                filename = file_path + @"\" + CompanyName + "_ItemWiseSaleReport.pdf";
                crpReport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, filename);
                crpReport.Close();
                crpReport.Dispose();
            }
            catch (Exception ex)
            {
                flag = false;
                WriteLog(ex.Message);
            }
            return flag;
        }

        private bool EmailReport(string filename, string[] emailto,string EmailFrom,string SMTP,string Password,string Subject,string Body)
        {
            bool flag = true;
            try
            {                
                flag = Utility.SendEmail(EmailFrom, emailto, null, SMTP, Password, Subject + DateTime.Now.AddDays(-1).ToString("dd-MMM-yyyy"), Body, filename, 587);
            }
            catch (Exception ex)
            {
                flag = false;
                WriteLog(ex.Message);
            }
            return flag;
        }        

        private static void WriteLog(string Msg)
        {
            using (FileStream fs = File.Open(path + "CORNPOSEmailServiceLog.txt", FileMode.Append, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(string.Format("{0}: {1}", DateTime.Now, Msg));
                    sw.Write(Environment.NewLine);
                }
            }
        }
        public static string Decrypt(string EncryptedText, string Key)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(Key);
            byte[] rgbIV = Encoding.UTF8.GetBytes(Key);
            byte[] buffer = Convert.FromBase64String(EncryptedText);
            MemoryStream stream = new MemoryStream();
            try
            {
                DES des = new DESCryptoServiceProvider();
                CryptoStream stream2 = new CryptoStream(stream, des.CreateDecryptor(bytes, rgbIV), CryptoStreamMode.Write);
                stream2.Write(buffer, 0, buffer.Length);
                stream2.FlushFinalBlock();
            }
            catch (Exception ex)
            {
            }
            return Encoding.UTF8.GetString(stream.ToArray());
        }
        public DataTable GetDatabases()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "uspGetSetEmailServiceData";
                        cmd.CommandType = CommandType.StoredProcedure;

                        IDataParameterCollection pparams = cmd.Parameters;
                        IDataParameter parameter = new SqlParameter() { ParameterName = "@TypeID", DbType = DbType.Int32, Value = 1 };
                        pparams.Add(parameter);

                        IDbDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        dt= ds.Tables[0];
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
            }
            return dt;
        }
        public void GetEmail(string DBName)
        {
            string conString2 = "server=" + Decrypt(System.Configuration.ConfigurationManager.AppSettings["server"].ToString(), "b0tin@74")
                + ";uid=" + Decrypt(System.Configuration.ConfigurationManager.AppSettings["uid"].ToString(), "b0tin@74")
                + ";pwd=" + Decrypt(System.Configuration.ConfigurationManager.AppSettings["pwd"].ToString(), "b0tin@74")
                + ";database=" + DBName;
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            try
            {
                using (SqlConnection con = new SqlConnection(conString2))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "uspGetEmailSetting";
                        cmd.CommandType = CommandType.StoredProcedure;
                        IDbDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;
                        da.Fill(ds);
                        dt = ds.Tables[0];
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
            }

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    DataRow drNew = dtEmailSetting.NewRow();
                    drNew["TypeID"] = dr["TypeID"];
                    drNew["EmailTime"] = dr["EmailTime"];
                    drNew["CompanyName"] = dr["CompanyName"].ToString();
                    drNew["EmailFrom"] = dr["EmailFrom"];
                    drNew["EmailTo"] = dr["EmailTo"].ToString();
                    drNew["Password"] = dr["Password"];
                    drNew["SMTP"] = dr["SMTP"].ToString();
                    drNew["Subject"] = dr["Subject"];
                    drNew["Body"] = dr["Body"].ToString();
                    dtEmailSetting.Rows.Add(drNew);
                }
            }
        }
        public DataSet GetDailySalesReport(string DBName, DateTime FromDate,DateTime ToDate)
        {
            string conString2 = "server=" + Decrypt(System.Configuration.ConfigurationManager.AppSettings["server"].ToString(), "b0tin@74")
                + ";uid=" + Decrypt(System.Configuration.ConfigurationManager.AppSettings["uid"].ToString(), "b0tin@74")
                + ";pwd=" + Decrypt(System.Configuration.ConfigurationManager.AppSettings["pwd"].ToString(), "b0tin@74")
                + ";database=" + DBName;
            CORNBusinessLayer.Reports.DsReport2 ds = new CORNBusinessLayer.Reports.DsReport2();
            try
            {
                using (SqlConnection con = new SqlConnection(conString2))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "UspPrintSaleForceDSRNew";
                        cmd.CommandType = CommandType.StoredProcedure;

                        IDataParameterCollection pparams = cmd.Parameters;

                        IDataParameter parameter = new SqlParameter() { ParameterName = "@TYPE_ID", DbType = DbType.Int32, Value = 1 };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@USER_ID", DbType = DbType.Int32, Value = 1 };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@DISTRIBUTOR_ID", DbType = DbType.Int32, Value = DBNull.Value };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@FROM_DATE", DbType = DbType.DateTime, Value = FromDate };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@TO_DATE", DbType = DbType.DateTime, Value = ToDate };
                        pparams.Add(parameter);

                        DataSet myds = new DataSet();
                        IDbDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;                        
                        da.Fill(myds);

                        foreach (DataRow dr in myds.Tables[0].Rows)
                        {
                            ds.Tables["UspPrintSaleForceDSR"].ImportRow(dr);
                        }

                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
            }

            try
            {
                using (SqlConnection con = new SqlConnection(conString2))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "uspGetServiceWiseCoverTables";
                        cmd.CommandType = CommandType.StoredProcedure;

                        IDataParameterCollection pparams = cmd.Parameters;

                        IDataParameter parameter = new SqlParameter() { ParameterName = "@Rpt_Type", DbType = DbType.Int32, Value = 1 };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@USER_ID", DbType = DbType.Int32, Value = 1 };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@DISTRIBUTOR_ID", DbType = DbType.Int32, Value = DBNull.Value };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@FROMDATE", DbType = DbType.DateTime, Value = FromDate };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@TODATE", DbType = DbType.DateTime, Value = ToDate };
                        pparams.Add(parameter);


                        DataSet myds = new DataSet();
                        IDbDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;
                        da.Fill(myds);

                        foreach (DataRow dr in myds.Tables[0].Rows)
                        {
                            ds.Tables["uspGetServiceWiseCoverTables"].ImportRow(dr);
                        }
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
            }

            try
            {
                using (SqlConnection con = new SqlConnection(conString2))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "uspGetDailyDeliveryData";
                        cmd.CommandType = CommandType.StoredProcedure;

                        IDataParameterCollection pparams = cmd.Parameters;

                        IDataParameter parameter = new SqlParameter() { ParameterName = "@USER_ID", DbType = DbType.Int32, Value = 1 };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@DISTRIBUTOR_ID", DbType = DbType.Int32, Value = DBNull.Value };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@FROMDATE", DbType = DbType.DateTime, Value = FromDate };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@TODATE", DbType = DbType.DateTime, Value = ToDate };
                        pparams.Add(parameter);


                        DataSet myds = new DataSet();
                        IDbDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;
                        da.Fill(myds);

                        foreach (DataRow dr in myds.Tables[0].Rows)
                        {
                            ds.Tables["uspGetDailyDeliveryData"].ImportRow(dr);
                        }
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
            }

            return ds;
        }
        public DataSet GetItemWiseSalesReport(string DBName, DateTime FromDate, DateTime ToDate)
        {
            string conString2 = "server=" + Decrypt(System.Configuration.ConfigurationManager.AppSettings["server"].ToString(), "b0tin@74")
                + ";uid=" + Decrypt(System.Configuration.ConfigurationManager.AppSettings["uid"].ToString(), "b0tin@74")
                + ";pwd=" + Decrypt(System.Configuration.ConfigurationManager.AppSettings["pwd"].ToString(), "b0tin@74")
                + ";database=" + DBName;
            CORNBusinessLayer.Reports.dsSalesPurchaseRegister ds = new CORNBusinessLayer.Reports.dsSalesPurchaseRegister();
            try
            {
                using (SqlConnection con = new SqlConnection(conString2))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "uspRegionWiseItemSales";
                        cmd.CommandType = CommandType.StoredProcedure;

                        IDataParameterCollection pparams = cmd.Parameters;

                        IDataParameter parameter = new SqlParameter() { ParameterName = "@CATEGORY_ID", DbType = DbType.Int32, Value = DBNull.Value };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@distributor_id", DbType = DbType.Int32, Value = DBNull.Value };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@user_id", DbType = DbType.Int32, Value = 1 };
                        pparams.Add(parameter);                        

                        parameter = new SqlParameter() { ParameterName = "@FROM_DATE", DbType = DbType.DateTime, Value = FromDate };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@TO_DATE", DbType = DbType.DateTime, Value = ToDate };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@RptType", DbType = DbType.Int32, Value = 0 };
                        pparams.Add(parameter);

                        DataSet myds = new DataSet();
                        IDbDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;
                        da.Fill(myds);

                        foreach (DataRow dr in myds.Tables[0].Rows)
                        {
                            ds.Tables["uspRegionWiseItemSales"].ImportRow(dr);
                        }

                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
            }
            return ds;
        }
        public bool InsertEmailHistory(string DBName,int TypeID)
        {
            string conStringInsertEmailHistory = "server=" + Decrypt(System.Configuration.ConfigurationManager.AppSettings["server"].ToString(), "b0tin@74")
                + ";uid=" + Decrypt(System.Configuration.ConfigurationManager.AppSettings["uid"].ToString(), "b0tin@74")
                + ";pwd=" + Decrypt(System.Configuration.ConfigurationManager.AppSettings["pwd"].ToString(), "b0tin@74")
                + ";database=" + DBName;

            try
            {
                using (SqlConnection con = new SqlConnection(conStringInsertEmailHistory))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "uspInsertEmailHistoryWS";
                        cmd.CommandType = CommandType.StoredProcedure;

                        IDataParameterCollection pparams = cmd.Parameters;
                        IDataParameter parameter = new SqlParameter() { ParameterName = "@TypeID", DbType = DbType.Int32, Value = TypeID };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@Email_Date", DbType = DbType.DateTime, Value = DateTime.Now.Date };
                        pparams.Add(parameter);
                        
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
                return false;
            }
        }

        public DataTable GetEmailHistory(string DBName,DateTime Email_Date,int TypeID)
        {
            string conString2 = "server=" + Decrypt(System.Configuration.ConfigurationManager.AppSettings["server"].ToString(), "b0tin@74")
                + ";uid=" + Decrypt(System.Configuration.ConfigurationManager.AppSettings["uid"].ToString(), "b0tin@74")
                + ";pwd=" + Decrypt(System.Configuration.ConfigurationManager.AppSettings["pwd"].ToString(), "b0tin@74")
                + ";database=" + DBName;
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            try
            {
                using (SqlConnection con = new SqlConnection(conString2))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "uspGetEmailHistory";
                        cmd.CommandType = CommandType.StoredProcedure;
                        IDataParameterCollection pparams = cmd.Parameters;

                        IDataParameter parameter = new SqlParameter() { ParameterName = "@Email_Date", DbType = DbType.DateTime, Value = Email_Date };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@TypeID", DbType = DbType.Int32, Value = TypeID };
                        pparams.Add(parameter);

                        IDbDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;
                        da.Fill(ds);
                        dt = ds.Tables[0];
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
            }
            return dt;
        }

        public DataTable GetWrongCalculatedData(DateTime pDate)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "uspGetWrongCalculatedDataTest";
                        cmd.CommandType = CommandType.StoredProcedure;
                        IDataParameterCollection pparams = cmd.Parameters;
                        IDataParameter parameter = new SqlParameter() { ParameterName = "@Date", DbType = DbType.DateTime, Value = pDate };
                        pparams.Add(parameter);
                        IDbDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        dt = ds.Tables[0];
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
            }
            return dt;
        }
        public bool CorrectData(DateTime pDate)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "uspCorrectWrongDataTest";
                        cmd.CommandType = CommandType.StoredProcedure;
                        IDataParameterCollection pparams = cmd.Parameters;
                        IDataParameter parameter = new SqlParameter() { ParameterName = "@Date", DbType = DbType.DateTime, Value = pDate };
                        pparams.Add(parameter);
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
                return false;
            }
        }
    }
}