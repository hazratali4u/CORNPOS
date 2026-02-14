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
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CORNPOSFBRService
{
    public partial class CORNPOSFBRService : ServiceBase
    {
        static string path = string.Empty;
        static string conString = "";
        Timer timer = new Timer();
        static DataTable dtFBRSetting = new DataTable();
        static DataTable dtInvoices = new DataTable();
        static DataTable dtInvoiceDetail = new DataTable();
        public CORNPOSFBRService()
        {
            InitializeComponent();
            int TicKSec = 60;
            try
            {
                TicKSec = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["TickTime"].ToString());
            }
            catch (Exception)
            {
                TicKSec = 6000;
            }
            path = AppDomain.CurrentDomain.BaseDirectory;
            conString = Decrypt(System.Configuration.ConfigurationManager.AppSettings["connString"].ToString(), "b0tin@74");
            timer.Elapsed += PerformTimerOperation;
            timer.Interval = TimeSpan.FromSeconds(TicKSec).TotalMilliseconds;
            timer.Start();
        }
        protected override void OnStart(string[] args)
        {
            WriteLog("Service Started.");
        }
        protected override void OnStop()
        {
            WriteLog("Service Stopped.");
        }
        void PerformTimerOperation(object sender, ElapsedEventArgs e)
        {
            string[] dbs = System.Configuration.ConfigurationManager.AppSettings["Dbs"].ToString().Split(',');
            foreach (string s in dbs)
            {
                timer.Stop();
                dtFBRSetting = new DataTable();
                 dtFBRSetting = GetFBRSetting(s);
                if (dtFBRSetting.Rows.Count > 0)
                {
                    dtInvoices = new DataTable();
                    dtInvoices = GetMissingInvoices(1, 0, s);
                    foreach(DataRow dr in dtInvoices.Rows)
                    {
                        if (dtInvoices.Rows.Count > 0)
                        {
                            dtInvoiceDetail = new DataTable();
                            dtInvoiceDetail = GetInvoiceDetail(4, Convert.ToInt64(dr["SALE_INVOICE_ID"]),s);
                            if(dtInvoiceDetail.Rows.Count>0)
                            {
                                InvoiceFBR objInvoice = new InvoiceFBR();
                                List<InvoiceFBRDetail> lstItems = new List<InvoiceFBRDetail>();
                                InvoiceFBRDetail ObjInvoiceDetail = new InvoiceFBRDetail();
                                int TotalQty = 0;
                                double GrossValue = 0;
                                double NetAmount = 0;
                                double TotalTax = 0;
                                double TaxRate = 0;
                                decimal Discount = 0;
                                int PaymentMode = 1;
                                if (dtInvoiceDetail.Rows.Count > 0)
                                {
                                    TotalTax = Convert.ToDouble(dtInvoiceDetail.Rows[0]["GST"]);
                                    TaxRate = Convert.ToDouble(dtInvoiceDetail.Rows[0]["GST"]) / Convert.ToDouble(dtInvoiceDetail.Rows[0]["AMOUNTDUE"]) * 100;

                                    if (decimal.Parse(dtInvoiceDetail.Rows[0]["DISCOUNT"].ToString()) > 0)
                                    {
                                        if (int.Parse(dtInvoiceDetail.Rows[0]["DISCOUNT_TYPE"].ToString()) == 0)
                                        {
                                            Discount = decimal.Parse(dtInvoiceDetail.Rows[0]["AMOUNTDUE"].ToString()) * decimal.Parse(dtInvoiceDetail.Rows[0]["DISCOUNT"].ToString()) / 100;
                                        }
                                        else
                                        {
                                            Discount = decimal.Parse(dtInvoiceDetail.Rows[0]["AMOUNTDUE"].ToString());
                                        }
                                    }

                                    foreach (DataRow drDetail in dtInvoiceDetail.Rows)
                                    {
                                        ObjInvoiceDetail.ItemCode = drDetail["SKU_ID"].ToString();
                                        ObjInvoiceDetail.ItemName = drDetail["SKU_NAME"].ToString();
                                        ObjInvoiceDetail.Quantity = Convert.ToInt32(drDetail["QTY"]);
                                        ObjInvoiceDetail.SaleValue = Convert.ToDouble(drDetail["AMOUNT"]);
                                        ObjInvoiceDetail.TaxCharged = Convert.ToDouble(drDetail["AMOUNT"]) * TaxRate / 100;
                                        ObjInvoiceDetail.TotalAmount = Convert.ToDouble(drDetail["AMOUNT"]) + ObjInvoiceDetail.TaxCharged;
                                        ObjInvoiceDetail.TaxRate = TaxRate;
                                        ObjInvoiceDetail.PCTCode = "10101";
                                        ObjInvoiceDetail.FurtherTax = 0;
                                        ObjInvoiceDetail.InvoiceType = 1;//1=New,2=Debit,3=Credit
                                        ObjInvoiceDetail.Discount = Discount / dtInvoiceDetail.Rows.Count;
                                        ObjInvoiceDetail.RefUSIN = dr["SALE_INVOICE_ID"].ToString();
                                        lstItems.Add(ObjInvoiceDetail);
                                        TotalQty += ObjInvoiceDetail.Quantity;
                                        GrossValue += ObjInvoiceDetail.SaleValue;
                                        NetAmount += ObjInvoiceDetail.TotalAmount;
                                        TotalTax += ObjInvoiceDetail.TaxCharged;
                                    }

                                    if (int.Parse(dtInvoiceDetail.Rows[0]["PAYMENT_MODE_ID"].ToString()) == 0)
                                    {
                                        PaymentMode = 1;
                                    }
                                    else if (int.Parse(dtInvoiceDetail.Rows[0]["PAYMENT_MODE_ID"].ToString()) == 1)
                                    {
                                        PaymentMode = 2;
                                    }
                                    else
                                    {
                                        PaymentMode = 6;//Cheque For Credit invoices
                                    }

                                    objInvoice.Items = lstItems;
                                    objInvoice.InvoiceNumber = string.Empty;
                                    objInvoice.POSID = dtFBRSetting.Rows[0]["POSID"].ToString();
                                    objInvoice.USIN = dr["SALE_INVOICE_ID"].ToString();
                                    objInvoice.DateTime = Convert.ToDateTime(dr["DOCUMENT_DATE"]); ;
                                    objInvoice.BuyerNTN = "";
                                    objInvoice.BuyerCNIC = "";
                                    objInvoice.BuyerName = "";
                                    objInvoice.BuyerPhoneNumber = "";
                                    objInvoice.PaymentMode = PaymentMode;//1=Cash,2=Card,3=Gift Voucher,4=Loyality Card,5=Mixed,6=Cheque
                                    objInvoice.TotalSaleValue = GrossValue;
                                    objInvoice.TotalQuantity = TotalQty;
                                    objInvoice.TotalBillAmount = NetAmount;
                                    objInvoice.TotalTaxCharged = TotalTax;
                                    objInvoice.Discount = Discount;
                                    objInvoice.FurtherTax = 0;
                                    objInvoice.InvoiceType = 1;//1=New,2=Debit,3=Credit
                                    objInvoice.RefUSIN = dr["SALE_INVOICE_ID"].ToString();

                                    try
                                    {
                                        HttpClient Client = new HttpClient();

                                        Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", dtFBRSetting.Rows[0]["Token"].ToString());
                                        var content = new StringContent(JsonConvert.SerializeObject(objInvoice), Encoding.UTF8, "application/json");
                                        System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                                        HttpResponseMessage response = Client.PostAsync(dtFBRSetting.Rows[0]["FBRURL"].ToString(), content).Result;

                                        string InvoiceNumberFBR = string.Empty;
                                        string CodeFBR = string.Empty;
                                        if (response.IsSuccessStatusCode)
                                        {
                                            string responseFBR = response.Content.ReadAsStringAsync().Result;
                                            InvoiceNumberFBR = JObject.Parse(responseFBR)["InvoiceNumber"].ToString();
                                            CodeFBR = JObject.Parse(responseFBR)["Code"].ToString();

                                            if (UpdateFBRInvoiceCode(Convert.ToInt64(dr["SALE_INVOICE_ID"]), InvoiceNumberFBR, s))
                                            {
                                                WriteLog(dr["SALE_INVOICE_ID"].ToString() + " updated.");
                                            }
                                        }
                                        else
                                        {
                                            WriteLog("Message from FBR Server: " + response.ToString());
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        WriteLog("Message from FBR Server: " + ex.ToString());
                                    }
                                }
                            }
                        }
                    }
                }
                timer.Start();
            }
        }
        public DataTable GetMissingInvoices(int TypeID, long SaleInvoiceID,string db)
        {
            DataTable dtOrders = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(conString+db))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "uspGetFBRMissingInvoices";
                        cmd.CommandType = CommandType.StoredProcedure;

                        IDataParameterCollection pparams = cmd.Parameters;
                        IDataParameter parameter = new SqlParameter() { ParameterName = "@SALE_INVOICE_ID", DbType = DbType.Int64, Value = SaleInvoiceID };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@TypeID", DbType = DbType.Int32, Value = TypeID };
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
                timer.Start();
                WriteLog(ex.ToString());
            }
            return dtOrders;
        }
        public DataTable GetInvoiceDetail(int TypeID, long SaleInvoiceID, string db)
        {
            DataTable dtOrders = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(conString + db))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "spGetPendingBill";
                        cmd.CommandType = CommandType.StoredProcedure;

                        IDataParameterCollection pparams = cmd.Parameters;
                        IDataParameter parameter = new SqlParameter() { ParameterName = "@SALE_INVOICE_ID", DbType = DbType.Int64, Value = SaleInvoiceID };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@TYPE_ID", DbType = DbType.Int32, Value = TypeID };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@LOCKED_BY", DbType = DbType.Int32, Value = 0 };
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
                timer.Start();
                WriteLog(ex.ToString());
            }
            return dtOrders;
        }
        public DataTable GetFBRSetting(string db)
        {
            DataTable dtOrders = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(conString + db))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "uspGetFBRIntegration";
                        cmd.CommandType = CommandType.StoredProcedure;

                        IDataParameterCollection pparams = cmd.Parameters;
                        IDataParameter parameter = new SqlParameter() { ParameterName = "@DistributorID", DbType = DbType.Int64, Value = DBNull.Value };
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
                timer.Start();
                WriteLog(ex.ToString());
            }
            return dtOrders;
        }
        public bool UpdateFBRInvoiceCode(long SaleInvoiceID,string FBRInoiceCode, string db)
        {
            DataTable dtOrders = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(conString + db))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "uspUpdateFBRInvoiceCode";
                        cmd.CommandType = CommandType.StoredProcedure;

                        IDataParameterCollection pparams = cmd.Parameters;
                        IDataParameter parameter = new SqlParameter() { ParameterName = "@SALE_INVOICE_ID", DbType = DbType.Int64, Value = SaleInvoiceID };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@InvoiceNumberFBR", DbType = DbType.String, Value = FBRInoiceCode};
                        pparams.Add(parameter);


                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                timer.Start();
                WriteLog(ex.ToString());
                return false;
            }
        }
        private static void WriteLog(string Msg)
        {
            using (FileStream fs = File.Open(path + "CORNPOSFBRServiceLog.txt", FileMode.Append, FileAccess.Write))
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
    }

    public class InvoiceFBRDetail
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public double TotalAmount { get; set; }
        public double SaleValue { get; set; }
        public double TaxCharged { get; set; }
        public double TaxRate { get; set; }
        public string PCTCode { get; set; }
        public decimal FurtherTax { get; set; }
        public int InvoiceType { get; set; }
        public decimal Discount { get; set; }
        public string RefUSIN { get; set; }
    }

    public class InvoiceFBR
    {
        public string InvoiceNumber { get; set; }
        public string POSID { get; set; }
        public string USIN { get; set; }
        public DateTime DateTime { get; set; }
        public string BuyerNTN { get; set; }
        public string BuyerCNIC { get; set; }
        public string BuyerName { get; set; }
        public string BuyerPhoneNumber { get; set; }
        public int PaymentMode { get; set; }
        public double TotalSaleValue { get; set; }
        public int TotalQuantity { get; set; }
        public double TotalBillAmount { get; set; }
        public double TotalTaxCharged { get; set; }
        public decimal Discount { get; set; }
        public decimal FurtherTax { get; set; }
        public int InvoiceType { get; set; }
        public string RefUSIN { get; set; }
        public List<InvoiceFBRDetail> Items { get; set; }
    }
}