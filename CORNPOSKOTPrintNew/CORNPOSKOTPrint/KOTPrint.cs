using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;
using System.ServiceProcess;
using System.Text;
using System.Timers;
using Timer = System.Timers.Timer;
using System.Drawing;
using System.Drawing.Printing;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Printing;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace CORNPOSKOTPrint
{
    public partial class KOTPrint : ServiceBase
    {

        #region Form Variables 
        static string path = string.Empty;
        static string conString = "";
        Timer timer = new Timer();
        static DataTable dtValue = new DataTable();
        static DataTable dtValueInvoice = new DataTable();
        static List<ProductSection> SectionPrinterList = new List<ProductSection>();
        static string customerType = "";
        static string bookerName = "";
        static string tableName = "";
        static string maxOrderNo = "";
        static string OrderNotes = "";
        static string Address = "";
        static string ContactNo = "";
        static string NTN = "";
        static string TaxAuthorirty = "";
        static string GSTCalculation = "0";
        static decimal GSTPer = 0;
        static decimal CreditCardGSTPer = 0;
        static DateTime lastTickTime = new DateTime();
        static string SectionWisePrint = "1";
        static string IsXpeditor = "0";
        static string IsLocationName = "0";
        static string IsPrintInvoice = "0";
        static string StickerSectionBoth = "0";
        static string[] XpeditorPrinterName;
        static string[] PrinterNames;
        static string covertable = string.Empty;
        static string IsKOTLogOnServer = "0";
        DateTime DOCUMENT_DATE = System.DateTime.Now.Date;
        #endregion

        #region Windows Service Functions
        public KOTPrint()
        {
            InitializeComponent();
            path = AppDomain.CurrentDomain.BaseDirectory;
            conString = Decrypt(System.Configuration.ConfigurationManager.AppSettings["connString"].ToString(), "b0tin@74");
            timer.Elapsed += PerformTimerOperationCrystalReport;
            timer.Interval = TimeSpan.FromSeconds(10).TotalMilliseconds;
            timer.Start();
            lastTickTime = System.DateTime.Now;            

        }
        protected override void OnStart(string[] args)
        {
            WriteLog("Service Started.", "OnStart(string[] args)");
            WriteLog("Version: 09-Feb-2026 06:50 PM", "OnStart");
            WriteLog("PerformTimerOperationCrystalReport", "OnStart");

            try
            {
                SectionWisePrint = System.Configuration.ConfigurationManager.AppSettings["SectionWisePrint"].ToString();
                string[] isfullkot = System.Configuration.ConfigurationManager.AppSettings["IsFullKOT"].ToString().Split(',');
                string[] isxpeditor = System.Configuration.ConfigurationManager.AppSettings["IsXpeditor"].ToString().Split(',');
                string checkXpeditor = System.Configuration.ConfigurationManager.AppSettings["IsXpeditor"].ToString();
                string[] excludecancelkot = System.Configuration.ConfigurationManager.AppSettings["ExcludeCancelKOT"].ToString().Split(',');
                string[] islocationname = System.Configuration.ConfigurationManager.AppSettings["IsLocationName"].ToString().Split(',');
                string[] isprintinvoice = System.Configuration.ConfigurationManager.AppSettings["IsPrintInvoice"].ToString().Split(',');
                PrinterNames = System.Configuration.ConfigurationManager.AppSettings["PrinterName"].Split(',');
                StickerSectionBoth = System.Configuration.ConfigurationManager.AppSettings["StickerSectionBoth"].ToString();

                XpeditorPrinterName = isxpeditor;
                
                foreach (string s in islocationname)
                {
                    if (s == "sectionPrinter")
                    {
                        IsLocationName = "1";
                        break;
                    }
                }

                if (checkXpeditor.Length > 0)
                {
                    IsXpeditor = "1";
                }

                if (isprintinvoice.Length > 0)
                {
                    IsPrintInvoice = "1";
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString(), "OnStart(string[] args)");
            }

            if (IsPrintInvoice == "1")
            {
                timer.Stop();
                DataTable dtLocation = GetPrintOrders(4, 0);
                if (dtLocation.Rows.Count > 0)
                {
                    Address = dtLocation.Rows[0]["ADDRESS1"].ToString();
                    ContactNo = dtLocation.Rows[0]["CONTACT_NUMBER"].ToString();
                    NTN = dtLocation.Rows[0]["NTN"].ToString();
                    TaxAuthorirty = dtLocation.Rows[0]["TaxAuthorirty"].ToString();
                    GSTCalculation = dtLocation.Rows[0]["GSTCalculation"].ToString();
                    GSTPer = Convert.ToDecimal(dtLocation.Rows[0]["GSTPer"]);
                    CreditCardGSTPer = Convert.ToDecimal(dtLocation.Rows[0]["CreditCardGSTPer"]);
                    IsKOTLogOnServer = dtLocation.Rows[0]["IsKOTLogOnServer"].ToString();
                }
                lastTickTime = System.DateTime.Now;
                timer.Start();
            }
            else
            {
                DataTable dtLocation = GetPrintOrders(7, 0);
                if (dtLocation.Rows.Count > 0)
                {
                    IsKOTLogOnServer = dtLocation.Rows[0]["IsKOTLogOnServer"].ToString();
                }
            }
        }
        protected override void OnStop()
        {
            WriteLog("Service Stopped.","OnStop()");
        }
        #endregion

        #region Timer Functions
        #region Crystal Report function
        void PerformTimerOperationCrystalReport(object sender, ElapsedEventArgs e)
        {
            timer.Stop();            
            try
            {
                if (SectionWisePrint == "1")
                {                    
                    PrintCrystalReportSectionWise();
                }
                else
                {
                    PrintCrystalReportNonSection();
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString(), "PerformTimerOperationCrystalReport(object sender, ElapsedEventArgs e)");                
            }
            finally
            {
                timer.Start();
            }
        }
        #endregion
        #endregion

        #region Printing Functions
        private void PrintCrystalReportSectionWise()
        {
            try
            {
                DataTable dtOrders = GetOfersForPrintCrystalReport(1, 0, 0);                
                if (dtOrders.Rows.Count > 0)
                {
                    DataView view = new DataView(dtOrders);
                    DataTable dtKOTType = view.ToTable(true, "KOTType");

                    DataView view2 = new DataView(dtOrders);
                    DataTable dtSection = view2.ToTable(true, "SECTION");

                    string KOTType = "NewKOT";
                    foreach (DataRow drKOTTyp in dtKOTType.Rows)
                    {                        
                        switch (drKOTTyp["KOTType"].ToString())
                        {
                            case "1":
                                KOTType = "NewKOT";
                                break;
                            case "2":
                                KOTType = "NewItemKOT";
                                break;
                            case "3":
                                KOTType = "AddQtyKOT";
                                break;
                            case "4":
                                KOTType = "LessQtyKOT";
                                break;
                            case "5":
                                KOTType = "CancelItemKOT";
                                break;
                            default:
                                KOTType = "NewKOT";
                                break;
                        }

                        foreach (DataRow drSection in dtSection.Rows)
                        {
                            DataTable dtOrderDetail = GetOfersForPrintCrystalReport(2, Convert.ToInt64(dtOrders.Rows[0]["SaleInvoiceID"]), 0, Convert.ToInt32(drKOTTyp["KOTType"]), drSection["SECTION"].ToString());
                            
                            if (dtOrderDetail.Rows.Count > 0)
                            {
                                covertable = dtOrderDetail.Rows[0]["CoverTable"].ToString();
                                customerType = dtOrderDetail.Rows[0]["CustomerType"].ToString();
                                bookerName = dtOrderDetail.Rows[0]["bookerName"].ToString();
                                tableName = dtOrderDetail.Rows[0]["tableName"].ToString();
                                maxOrderNo = dtOrderDetail.Rows[0]["maxOrderNo"].ToString();
                                OrderNotes = dtOrderDetail.Rows[0]["ORDER_NOTES"].ToString();

                                WriteLog(string.Format("Order No-{0}-" + KOTType + "-" + drSection["SECTION"].ToString() + " Started Printing.", dtOrderDetail.Rows[0]["SaleInvoiceID"].ToString()),string.Empty);
                                if (PrintKOTCrystalReport(dtOrderDetail, dtOrderDetail.Rows[0]["PrinterName"].ToString(), dtOrderDetail.Rows[0]["SaleInvoiceID"].ToString(), 1, false, dtOrderDetail.Rows[0]["OrderNotes"].ToString(), drSection["SECTION"].ToString().Trim().ToUpper(), KOTType,Convert.ToByte(drKOTTyp["KOTType"])))
                                {

                                }
                            }
                        }
                        #region Xpeditor Print
                        try
                        {
                            if (IsXpeditor == "1")
                            {   
                                foreach (string printer in XpeditorPrinterName)
                                {
                                    DataTable dtOrderDetailX = GetOfersForPrintCrystalReport(3, Convert.ToInt64(dtOrders.Rows[0]["SaleInvoiceID"]), 0, Convert.ToInt32(drKOTTyp["KOTType"]), string.Empty);                                    
                                    if (dtOrderDetailX.Rows.Count > 0)
                                    {
                                        WriteLog(string.Format("Order No-{0}-" + KOTType + "-Xpeditor" + " Started Printing.", dtOrderDetailX.Rows[0]["SaleInvoiceID"].ToString()),string.Empty);
                                        if (PrintKOTCrystalReport(dtOrderDetailX, printer, dtOrderDetailX.Rows[0]["SaleInvoiceID"].ToString(), 1, true, dtOrderDetailX.Rows[0]["OrderNotes"].ToString(), "Xpeditor", "X" + KOTType,Convert.ToByte(drKOTTyp["KOTType"])))
                                        {

                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            WriteLog(ex.ToString(), "if (IsXpeditor == '1')");
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString(), "PrintCrystalReportSectionWise");
            }
        }
        private void PrintCrystalReportNonSection()
        {
            try
            {
                DataTable dtOrders = GetOfersForPrintCrystalReport(4, 0, 0);
                if (dtOrders.Rows.Count > 0)
                {
                    DataView view = new DataView(dtOrders);
                    DataTable dtKOTType = view.ToTable(true, "KOTType");
                                        
                    foreach (DataRow drKOTTyp in dtKOTType.Rows)
                    {
                        string KOTType = "NewKOT";
                        switch (drKOTTyp["KOTType"].ToString())
                        {
                            case "1":
                                KOTType = "NewKOT";
                                break;
                            case "2":
                                KOTType = "NewItemKOT";
                                break;
                            case "3":
                                KOTType = "AddQtyKOT";
                                break;
                            case "4":
                                KOTType = "LessQtyKOT";
                                break;
                            case "5":
                                KOTType = "CancelItemKOT";
                                break;
                            default:
                                KOTType = "NewKOT";
                                break;
                        }

                        foreach (string PrinterName in PrinterNames)
                        {
                            DataTable dtOrderDetail = GetOfersForPrintCrystalReport(3, Convert.ToInt64(dtOrders.Rows[0]["SaleInvoiceID"]), 0, Convert.ToInt32(drKOTTyp["KOTType"]), string.Empty);
                            if (dtOrderDetail.Rows.Count > 0)
                            {
                                covertable = dtOrderDetail.Rows[0]["CoverTable"].ToString();
                                customerType = dtOrderDetail.Rows[0]["CustomerType"].ToString();
                                bookerName = dtOrderDetail.Rows[0]["bookerName"].ToString();
                                tableName = dtOrderDetail.Rows[0]["tableName"].ToString();
                                maxOrderNo = dtOrderDetail.Rows[0]["maxOrderNo"].ToString();
                                OrderNotes = dtOrderDetail.Rows[0]["ORDER_NOTES"].ToString();

                                WriteLog(string.Format("Order No-{0}-" + KOTType + "-" + PrinterName + " Started Printing.", dtOrderDetail.Rows[0]["SaleInvoiceID"].ToString()), string.Empty);
                                if (PrintKOTCrystalReport(dtOrderDetail, PrinterName, dtOrderDetail.Rows[0]["SaleInvoiceID"].ToString(), 1, true, dtOrderDetail.Rows[0]["OrderNotes"].ToString(), "Xpeditor", KOTType, Convert.ToByte(drKOTTyp["KOTType"])))
                                {

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString(), "PrintCrystalReportSectionWise");
            }
        }
        #endregion

        #region Log Files
        private static void WriteLog(string Msg,string FunctionName)
        {            
            string logFile = path + "CORNPOSKOTPrintLog.txt";
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    using (FileStream fs = new FileStream(logFile, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.WriteLine($"{DateTime.Now}: {Msg}");
                        sw.Flush();
                        fs.Flush(true);
                    }
                    break;
                }
                catch (IOException)
                {
                    System.Threading.Thread.Sleep(100);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Logging failed: " + ex.Message);
                    break;
                }
            }
            if(IsKOTLogOnServer.ToLower() == "true")
            {
                try
                {
                    InsertKOTLog(FunctionName, Msg);
                }
                catch (Exception ex)
                {
                    WriteLog(ex.ToString(), "InsertKOTLog(FunctionName, Msg)");
                }
            }
        }

        #endregion

        #region Decryp
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
                WriteLog(ex.ToString(), "Decrypt(string EncryptedText, string Key)");
            }
            return Encoding.UTF8.GetString(stream.ToArray());
        }
        #endregion

        #region Get Data
        public DataTable GetOfersForPrintCrystalReport(int TypeID, long SaleInvoiceID, long CustomerID)
        {
            DataTable dtOrders = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandTimeout = 120;
                        cmd.CommandText = "uspGetOfersForPrintWSCrystalReportNew";
                        cmd.CommandType = CommandType.StoredProcedure;

                        IDataParameterCollection pparams = cmd.Parameters;
                        IDataParameter parameter = new SqlParameter() { ParameterName = "@DISTRIBUTOR_ID", DbType = DbType.Int32, Value = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["DistributorID"].ToString()) };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@TYPE_ID", DbType = DbType.Int32, Value = TypeID };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@SALE_INVOICE_ID", DbType = DbType.Int64, Value = SaleInvoiceID };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@CUSTOMER_ID", DbType = DbType.Int64, Value = CustomerID };
                        pparams.Add(parameter);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            dtOrders.Load(reader);
                        }
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString(), "GetOfersForPrintCrystalReport(int TypeID, long SaleInvoiceID, long CustomerID)");
            }
            return dtOrders;
        }
        public DataTable GetOfersForPrintCrystalReport(int TypeID, long SaleInvoiceID, long CustomerID,int KOTType,string Section)
        {
            DataTable dtOrders = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandTimeout = 120;
                        cmd.CommandText = "uspGetOfersForPrintWSCrystalReportNew";
                        cmd.CommandType = CommandType.StoredProcedure;

                        IDataParameterCollection pparams = cmd.Parameters;
                        IDataParameter parameter = new SqlParameter() { ParameterName = "@DISTRIBUTOR_ID", DbType = DbType.Int32, Value = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["DistributorID"].ToString()) };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@TYPE_ID", DbType = DbType.Int32, Value = TypeID };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@SALE_INVOICE_ID", DbType = DbType.Int64, Value = SaleInvoiceID };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@CUSTOMER_ID", DbType = DbType.Int64, Value = CustomerID };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@KOTType", DbType = DbType.Int32, Value = KOTType };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@Section", DbType = DbType.String, Value = Section };
                        pparams.Add(parameter);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            dtOrders.Load(reader);
                        }
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString(), "GetOfersForPrintCrystalReport(int TypeID, long SaleInvoiceID, long CustomerID,int KOTType,string Section)");
            }
            return dtOrders;
        }
        public DataTable GetPrintOrders(int TypeID, long SaleInvoiceID)
        {
            DataTable dtOrders = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandTimeout = 120;
                        cmd.CommandText = "uspGetPrintInvoiceWS";
                        cmd.CommandType = CommandType.StoredProcedure;

                        IDataParameterCollection pparams = cmd.Parameters;
                        IDataParameter parameter = new SqlParameter() { ParameterName = "@DISTRIBUTOR_ID", DbType = DbType.Int32, Value = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["DistributorID"].ToString()) };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@TYPE_ID", DbType = DbType.Int32, Value = TypeID };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@SALE_INVOICE_ID", DbType = DbType.Int64, Value = SaleInvoiceID };
                        pparams.Add(parameter);

                        IDbDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        dtOrders = ds.Tables[0];
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString(), "GetPrintOrders(int TypeID, long SaleInvoiceID)");
            }
            return dtOrders;
        }
        #endregion

        #region Update Data
        public bool UpdatePrintedKOT(long SaleInvoiceID, byte KOTType, string Section, byte TypeID)
        {
            bool flag = false;
            int maxRetries = 3;
            int delayMilliseconds = 2000;
            for (int attempt = 1; attempt <= maxRetries; attempt++)
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(conString))
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand("uspUpdatePrintedKOT", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 120;

                            cmd.Parameters.Add(new SqlParameter("@SaleInvoiceID", SqlDbType.BigInt) { Value = SaleInvoiceID });
                            cmd.Parameters.Add(new SqlParameter("@KOTType", SqlDbType.TinyInt) { Value = KOTType });
                            cmd.Parameters.Add(new SqlParameter("@Section", SqlDbType.VarChar, 50) { Value = Section });
                            cmd.Parameters.Add(new SqlParameter("@TypeID", SqlDbType.TinyInt) { Value = TypeID });

                            cmd.ExecuteNonQuery();
                            flag = true;
                        }
                    }
                    break;
                }
                catch (SqlException ex)
                {
                    if (IsTransientSqlError(ex))
                    {
                        WriteLog($"Transient SQL error on attempt {attempt}: {ex.Message}", "UpdatePrintedKOT");
                        if (attempt < maxRetries)
                            System.Threading.Thread.Sleep(delayMilliseconds * attempt);
                        else
                            flag = false;
                    }
                    else
                    {
                        WriteLog(ex.ToString(), "UpdatePrintedKOT");
                        flag = false;
                        break;
                    }
                }
                catch (Exception ex)
                {
                    WriteLog(ex.ToString(), "UpdatePrintedKOT");
                    flag = false;
                    break;
                }
            }

            return flag;
        }
        public static bool InsertKOTLog(string FunctionName, string ExceptionMessage)
        {
            bool flag = false;
            int maxRetries = 3; // Retry up to 3 times
            int delayMilliseconds = 2000; // Initial delay (2 seconds)

            for (int attempt = 1; attempt <= maxRetries; attempt++)
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(conString))
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand("uspInsertKOTLog", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 120;

                            cmd.Parameters.Add(new SqlParameter("@LocationID", SqlDbType.Int)
                            {
                                Value = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["DistributorID"].ToString())
                            });
                            cmd.Parameters.Add(new SqlParameter("@FunctionName", SqlDbType.VarChar, 100)
                            {
                                Value = FunctionName
                            });
                            cmd.Parameters.Add(new SqlParameter("@ExceptionMessage", SqlDbType.VarChar, -1)
                            {
                                Value = ExceptionMessage
                            });

                            cmd.ExecuteNonQuery();
                            flag = true;
                        }
                    }

                    break; // ✅ success, exit retry loop
                }
                catch (SqlException ex)
                {
                    // Check for transient SQL errors
                    if (IsTransientSqlError(ex))
                    {
                        WriteLog($"Transient SQL error on attempt {attempt}: {ex.Message}", "InsertKOTLog(string FunctionName, string ExceptionMessage)");
                        if (attempt < maxRetries)
                           System.Threading.Thread.Sleep(delayMilliseconds * attempt); // exponential backoff (2s, 4s, 6s)
                        else
                            flag = false;
                    }
                    else
                    {
                        WriteLog(ex.ToString(), "InsertKOTLog(string FunctionName, string ExceptionMessage)");
                        flag = false;
                        break;
                    }
                }
                catch (Exception ex)
                {
                    WriteLog(ex.ToString(), "InsertKOTLog(string FunctionName, string ExceptionMessage)");
                    flag = false;
                    break;
                }
            }

            return flag;
        }

        #endregion                

        #region Crystal Reports
        private bool PrintKOTCrystalReport(DataTable dtValueOrder,string PrinterName,string OrderNo,int NofOfCopies,bool IsFullKOT,string REMARKS,string SectionName,string KOTType,byte bytKOTType)
        {
            bool flag = true;

            if (dtValueOrder == null || dtValueOrder.Rows.Count == 0)
                return false;
            if (string.IsNullOrWhiteSpace(PrinterName))
                return false;
            try
            {
                using (ReportDocument report = IsFullKOT
                    ? (ReportDocument)new CrpFullKOT()
                    : (ReportDocument)new CrpKOT())
                {
                    // 1️⃣ Data
                    report.SetDataSource(dtValueOrder);

                    // 2️⃣ Printer (BEFORE parameters & print)
                    report.PrintOptions.PrinterName = PrinterName;

                    // 3️⃣ Parameters
                    switch (bytKOTType)
                    {
                        case 3:
                            report.SetParameterValue("KOTHeader", "RUNNING ORDER - ADD ITEM Qty");
                            break;
                        case 2:
                            report.SetParameterValue("KOTHeader", "RUNNING ORDER - ADD ITEM");
                            break;
                        case 5:
                            report.SetParameterValue("KOTHeader", "RUNNING ORDER - CANCELED ITEM");
                            break;
                        case 4:
                            report.SetParameterValue("KOTHeader", "RUNNING ORDER - ITEM LESS");
                            break;
                        default:
                            report.SetParameterValue("KOTHeader", string.Empty);
                            break;
                    }
                    report.SetParameterValue("REMARKS", REMARKS);
                    if (IsLocationName == "1")
                        report.SetParameterValue("LocationName","Location Name: " + System.Configuration.ConfigurationManager.AppSettings["LocationName"]);
                    else
                        report.SetParameterValue("LocationName", "");
                    string FullKOTGroup = System.Configuration.ConfigurationManager.AppSettings["FullKOTGroup"] ?? "1";
                    report.SetParameterValue("FullKOTGroup", FullKOTGroup);
                    report.SetParameterValue("Covers", "Covers: " + covertable);
                    // 4️⃣ PRINT (only once)
                    report.PrintToPrinter(NofOfCopies, false, 0, 0);
                }
                // 5️⃣ Post-print updates
                if (!UpdatePrintedKOT(Convert.ToInt64(OrderNo), bytKOTType, SectionName, 1))
                    WriteLog("KOT Print not updated.", string.Empty);
                if (SectionName == "Xpeditor")
                {
                    if (!UpdatePrintedKOT(Convert.ToInt64(OrderNo), bytKOTType, string.Empty, 2))
                        WriteLog("Xpeditor KOT Print not updated.", string.Empty);
                }
                WriteLog($"Order No-{OrderNo}- {KOTType} Printed.", string.Empty);
                // 6️⃣ Service-safe cleanup
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            catch (Exception ex)
            {
                WriteLog("Print Error: " + ex.ToString(), "PrintKOTCrystalReport");
                flag = false;
            }

            return flag;
        }

        #endregion

        /// <summary>
        /// Checks if SQL exception is likely transient (worth retrying)
        /// </summary>
        private static bool IsTransientSqlError(SqlException ex)
        {
            // Common transient SQL error codes
            int[] transientErrorNumbers = { -2, 1205, 4060, 10928, 10929, 40197, 40501, 40613 };
            return ex.Errors.Cast<SqlError>().Any(e => transientErrorNumbers.Contains(e.Number));
        }
    }

    #region Classess
    public class ProductSection
    {
        public int SectionId { get; set; }
        public string SectionCode { get; set; }
        public string SectionName { get; set; }
        public string PrinterName { get; set; }
        public bool IS_FULL_KOT { get; set; }
        public bool IsPrint { get; set; }
        public bool IsActive { get; set; }
        public int NO_OF_PRINTS { get; set; }
    }
    public class ProductSectionResponse
    {
        public List<ProductSection> ProductSectionList { get; set; }

        public bool IsException { get; set; }
        public string Message { get; set; }
    }
    public class ProductSectionRequest
    {
        public ProductSection ProductSectionInfo { get; set; }
    }
    static public class Utiltiy
    {
        public static List<T> ToCollection<T>(this DataTable dt)
        {
            List<T> lst = new System.Collections.Generic.List<T>();
            Type tClass = typeof(T);
            PropertyInfo[] pClass = tClass.GetProperties();
            List<DataColumn> dc = dt.Columns.Cast<DataColumn>().ToList();
            T cn;
            foreach (DataRow item in dt.Rows)
            {
                cn = (T)Activator.CreateInstance(tClass);
                foreach (PropertyInfo pc in pClass)
                {
                    // Can comment try catch block. 
                    try
                    {
                        DataColumn d = dc.Find(c => c.ColumnName.ToUpper() == pc.Name.ToUpper());
                        if (d != null)
                            pc.SetValue(cn, item[pc.Name], null);
                    }
                    catch
                    {
                    }
                }
                lst.Add(cn);
            }
            return lst;
        }
        public static void DrawCircle(this Graphics g, Pen pen, float centerX, float centerY, float radius)
        {
            g.DrawEllipse(pen, centerX - radius, centerY - radius, radius + radius, radius + radius);
        }
    }
    #endregion    
}