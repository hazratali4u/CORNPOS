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
        #region Printing Variables for windows printing                
        private static int CurrentY;
        private static int CurrentX;
        private static int leftMargin;
        private static int rightMargin;
        private static int topMargin;
        private static int bottomMargin;
        private static int InvoiceWidth;
        private static int InvoiceHeight;


        // Font and Color:------------------
        // Title Font
        private static Font InvTitleFont = new Font("sans-serif", 22, FontStyle.Regular);
        // Title Font height
        private static int InvTitleHeight;
        // SubTitle Font
        private static Font InvSubTitleFont = new Font("sans-serif", 12, FontStyle.Regular);
        private static Font InvOrderTitleFont = new Font("sans-serif", 14, FontStyle.Bold);
        // SubTitle Font height
        private static int InvSubTitleHeight;
        // Invoice Font
        private static Font InvoiceFont = new Font("sans-serif", 11, FontStyle.Regular);
        private static Font InvoiceFont2 = new Font("sans-serif", 9, FontStyle.Regular);
        private static Font InvoiceFont3 = new Font("sans-serif", 9, FontStyle.Bold);
        private static Font InvoiceFont4 = new Font("sans-serif", 7, FontStyle.Regular);


        // Invoice Font height
        private static int InvoiceFontHeight;
        // Blue Color
        private static SolidBrush BlueBrush = new SolidBrush(Color.Blue);

        // Black Color
        private static SolidBrush BlackBrush = new SolidBrush(Color.Black);

        #endregion

        #region Form Variables 
        static string path = string.Empty;
        static string conString = "";
        Timer timer = new Timer();
        static DataTable dtValue = new DataTable();
        static DataTable dtValueInvoice = new DataTable();
        static List<ProductSection> SectionPrinterList = new List<ProductSection>();
        static string _section = null;
        static string customerType = "";
        static string bookerName = "";
        static string tableName = "";
        static string maxOrderNo = "";
        static string OrderNotes = "";

        static string PrinterName = "";
        static string Address = "";
        static string ContactNo = "";
        static string MOP = "";
        static DateTime TimeStamp = DateTime.MinValue;
        static string NTN = "";
        static string OrderNo = "";
        static string Cashier = "";
        static string Covers = "";
        static string TableNo = "";
        static string DM_OT = "";
        static string ServiceType = "";
        static string InvoiceNo = "";
        static decimal GST = 0;
        static decimal DISCOUNT = 0;
        static string TaxAuthorirty = "";
        static string GSTCalculation = "0";
        static string DISCOUNT_TYPE = "0";
        static decimal SERVICE_CHARGES = 0;
        static string SERVICE_CHARGES_TYPE = "0";
        static decimal GSTPer = 0;
        static decimal CreditCardGSTPer = 0;
        static string PAYMENT_MODE_ID = "0";
        static DateTime lastTickTime = new DateTime();
        static bool IsTimmerRunning = false;
        static bool IsExceptionOccured = false;
        static string SectionWisePrint = "1";
        static string IsFullKOT = "0";
        static string IsXpeditor = "0";
        static string ExcludeCancelKOT = "0";
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
            timer.Elapsed += PerformTimerOperationCrystalReportNew;
            timer.Interval = TimeSpan.FromSeconds(10).TotalMilliseconds;
            timer.Start();
            IsTimmerRunning = true;
            IsExceptionOccured = false;
            lastTickTime = System.DateTime.Now;            

        }
        protected override void OnStart(string[] args)
        {
            WriteLog("Service Started.");
            WriteLog("Version: 08-Oct-2025 05:30 PM");
            WriteLog("PerformTimerOperationCrystalReportNew");

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

                foreach (string s in excludecancelkot)
                {
                    if (s == "sectionPrinter")
                    {
                        ExcludeCancelKOT = "1";
                        break;
                    }
                }
                foreach (string s in islocationname)
                {
                    if (s == "sectionPrinter")
                    {
                        IsLocationName = "1";
                        break;
                    }
                }

                foreach (string s in isfullkot)
                {
                    if (s == "sectionPrinter")
                    {
                        IsFullKOT = "1";
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
                WriteLog(ex.ToString());
            }

            if (IsPrintInvoice == "1")
            {
                timer.Stop();
                IsTimmerRunning = false;
                IsExceptionOccured = false;
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
                IsTimmerRunning = true;
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
            WriteLog("Service Stopped.");
        }
        #endregion

        #region Timer Functions
        #region Windows Printing function
        void PerformTimerOperation(object sender, ElapsedEventArgs e)
        {
            IsExceptionOccured = false;
            timer.Stop();
            IsTimmerRunning = false;
            DataTable dtOrders = new DataTable();
            try
            {
                dtOrders = GetOfersForPrint(1, 0);
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
                IsExceptionOccured = true;
                timer.Start();
            }
            dtValue = null;
            if (dtOrders.Rows.Count > 0)
            {
                foreach (DataRow drMaster in dtOrders.Rows)
                {
                    ProductSectionRequest request = new ProductSectionRequest();
                    ProductSectionResponse response = GetProductSectionsWithPrinters(request);
                    SectionPrinterList = response.ProductSectionList;
                    try
                    {
                        dtValue = GetOfersForPrint(2, Convert.ToInt64(drMaster["SALE_INVOICE_ID"]));
                    }
                    catch (Exception ex)
                    {
                        WriteLog(ex.ToString());
                        IsExceptionOccured = true;
                        timer.Start();
                    }
                    if (dtValue.Rows.Count > 0)
                    {
                        customerType = dtValue.Rows[0]["CustomerType"].ToString();
                        bookerName = dtValue.Rows[0]["bookerName"].ToString();
                        tableName = dtValue.Rows[0]["tableName"].ToString();
                        maxOrderNo = dtValue.Rows[0]["maxOrderNo"].ToString();
                        OrderNotes = dtValue.Rows[0]["OrderNotes"].ToString();
                        DataTable dtSection = new DataTable();
                        dtSection.Columns.Add("SECTION", typeof(string));
                        try
                        {
                            foreach (DataRow dr in dtValue.Rows)
                            {
                                decimal qty = Convert.ToDecimal(dr["QTY"].ToString()) - Convert.ToDecimal(dr["PR_COUNT"].ToString());
                                string IS_VOID = dr["VOID"].ToString().ToLower();
                                if (IS_VOID == "false")
                                {
                                    if (qty > 0)
                                    {
                                        DataRow drSection = dtSection.NewRow();
                                        drSection["SECTION"] = dr["SECTION"];
                                        dtSection.Rows.Add(drSection);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            WriteLog(ex.ToString());
                            IsExceptionOccured = true;
                            timer.Start();
                        }

                        DataView view = new DataView(dtSection);
                        dtSection = view.ToTable(true, "SECTION");
                        string SinglePrinter = "0";
                        try
                        {
                            SinglePrinter = System.Configuration.ConfigurationManager.AppSettings["SinglePrinter"].ToString();
                        }
                        catch (Exception ex)
                        {
                            WriteLog(ex.ToString());
                            SinglePrinter = "0";
                            timer.Start();
                        }
                        int NofOfPrints = 1;
                        if (SinglePrinter == "0")
                        {
                            List<ProductSection> lstNofOfPrints = (List<ProductSection>)SectionPrinterList;
                            if (lstNofOfPrints != null)
                            {
                                NofOfPrints = lstNofOfPrints[0].NO_OF_PRINTS;
                                foreach (DataRow dr in dtSection.Rows)
                                {
                                    WriteLog(string.Format("Order No-{0}-KOT Started Printing.", drMaster["SALE_INVOICE_ID"].ToString()));
                                    _section = dr.ItemArray[0].ToString();
                                    if (!IsKOTPrinted(drMaster["SALE_INVOICE_ID"].ToString(), _section, "NewKOT", Convert.ToDateTime(drMaster["LASTUPDATE_DATE"]), DOCUMENT_DATE))
                                    {
                                        if (!IsExceptionOccured)
                                        {
                                            for (int i = 0; i < NofOfPrints; i++)
                                            {
                                                PrintReport(false, false);
                                            }
                                            WriteLogPrintedKOT(drMaster["SALE_INVOICE_ID"].ToString(), _section, "NewKOT", Convert.ToDateTime(drMaster["LASTUPDATE_DATE"]), DOCUMENT_DATE);
                                        }
                                        else
                                        {
                                            timer.AutoReset = true;
                                        }
                                    }
                                }
                            }
                        }
                        if (IsFullKOT == "1")
                        {
                            DataTable dtSection2 = new DataTable();
                            dtSection2.Columns.Add("SECTION", typeof(string));
                            foreach (DataRow dr in dtValue.Rows)
                            {
                                decimal qty = Convert.ToDecimal(dr["QTY"].ToString()) - Convert.ToDecimal(dr["PR_COUNT"].ToString());
                                string IS_VOID = dr["VOID"].ToString().ToLower();
                                if (IS_VOID == "false")
                                {
                                    if (qty > 0)
                                    {
                                        DataRow drSection2 = dtSection2.NewRow();
                                        drSection2["SECTION"] = "Full KOT";
                                        dtSection2.Rows.Add(drSection2);
                                        break;
                                    }
                                }
                            }
                            List<ProductSection> ProductSectionList = (List<ProductSection>)SectionPrinterList;
                            if (ProductSectionList.Count > 0)
                            {
                                for (int i = 0; i < ProductSectionList.Count; i++)
                                {
                                    if (ProductSectionList[i].IS_FULL_KOT)
                                    {
                                        _section = ProductSectionList[i].SectionName;
                                        break;
                                    }
                                }
                            }
                            foreach (DataRow drsec in dtValue.Rows)
                            {
                                drsec["SECTION"] = _section;
                            }

                            foreach (DataRow dr2 in dtSection2.Rows)
                            {
                                WriteLog(string.Format("Order No-{0}-32.", drMaster["SALE_INVOICE_ID"]));
                                for (int i = 0; i < NofOfPrints; i++)
                                {
                                    PrintReport(true, false);
                                }
                            }
                        }
                        if (IsXpeditor == "1")
                        {
                            _section = "Xpeditor";
                            foreach (DataRow drsec in dtValue.Rows)
                            {
                                drsec["SECTION"] = "Xpeditor";
                            }

                            WriteLog(string.Format("Order No-{0}-Xpeditor Started Printing.", drMaster["SALE_INVOICE_ID"]));
                            PrintReport(false, true);
                        }
                        foreach (DataRow drDetail in dtValue.Rows)
                        {
                            if (!UpdateSaleDetailPrintQty(Convert.ToInt64(drMaster["SALE_INVOICE_ID"]), Convert.ToInt32(drDetail["SKU_ID"]), Convert.ToDecimal(drDetail["QTY"])))
                            {
                            }
                        }
                    }
                }
            }

            if (IsPrintInvoice == "1")
            {
                DataTable dtPrintOrders = GetPrintOrders(1, 0);
                dtValueInvoice = null;
                if (dtPrintOrders.Rows.Count > 0)
                {
                    foreach (DataRow drMaster in dtPrintOrders.Rows)
                    {
                        PrinterName = drMaster["PRINTER_NAME"].ToString();
                        TimeStamp = Convert.ToDateTime(drMaster["TIME_STAMP"]);
                        MOP = drMaster["MOP"].ToString();
                        OrderNo = drMaster["OrderNo"].ToString();
                        Cashier = drMaster["Cashier"].ToString();
                        Covers = drMaster["Covers"].ToString();
                        TableNo = drMaster["TableNo"].ToString();
                        DM_OT = drMaster["DM_OT"].ToString();
                        ServiceType = drMaster["ServiceType"].ToString();
                        InvoiceNo = drMaster["SALE_INVOICE_ID"].ToString();
                        DISCOUNT = Convert.ToDecimal(drMaster["DISCOUNT"]);
                        DISCOUNT_TYPE = drMaster["DISCOUNT_TYPE"].ToString();
                        GST = Convert.ToDecimal(drMaster["GST"]);
                        SERVICE_CHARGES = Convert.ToDecimal(drMaster["SERVICE_CHARGES"]);
                        SERVICE_CHARGES_TYPE = drMaster["SERVICE_CHARGES_TYPE"].ToString();
                        PAYMENT_MODE_ID = drMaster["PAYMENT_MODE_ID"].ToString();
                        dtValueInvoice = GetPrintOrders(2, Convert.ToInt64(drMaster["SALE_INVOICE_ID"]));
                        if (dtValueInvoice.Rows.Count > 0)
                        {
                            WriteLog(string.Format("Invoice-{0}-Started Printing.", drMaster["SALE_INVOICE_ID"]));
                            PrintReportInvoice();

                            //Update Invoice Print
                            SetPrintOrder(3, Convert.ToInt64(drMaster["SALE_INVOICE_ID"]));
                        }
                    }
                }
            }
            timer.Start();
            IsTimmerRunning = true;
            lastTickTime = System.DateTime.Now;
        }
        #endregion

        #region Crystal Report function
        void PerformTimerOperationCrystalReport(object sender, ElapsedEventArgs e)
        {
            IsTimmerRunning = false;
            IsExceptionOccured = false;

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
                WriteLog(ex.ToString());
                IsTimmerRunning = true;
                lastTickTime = System.DateTime.Now;
                IsExceptionOccured = false;
            }

            IsTimmerRunning = true;
            IsExceptionOccured = false;
            lastTickTime = System.DateTime.Now;
        }
        void PerformTimerOperationCrystalReportNew(object sender, ElapsedEventArgs e)
        {
            timer.Stop();            
            try
            {
                if (SectionWisePrint == "1")
                {                    
                    PrintCrystalReportSectionWiseNew();
                }
                else
                {
                    PrintCrystalReportNonSection();
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());                
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
                DataTable dtValueCR = new DataTable();
                if (dtOrders.Rows.Count > 0)
                {
                    DOCUMENT_DATE = Convert.ToDateTime(dtOrders.Rows[0]["DOCUMENT_DATE"]);
                    timer.Stop();
                    foreach (DataRow drMaster in dtOrders.Rows)
                    {
                        covertable = drMaster["covertable"].ToString();
                        ProductSectionRequest request = new ProductSectionRequest();
                        ProductSectionResponse response = GetProductSectionsWithPrinters(request);
                        SectionPrinterList = response.ProductSectionList;
                        List<string> SaleInvoiceDetailId = new List<string>();
                        List<string> SaleInvoiceDetailIdModifier = new List<string>();
                        dtValueCR = new DataTable();
                        try
                        {
                            dtValueCR = GetOfersForPrintCrystalReport(2, Convert.ToInt64(drMaster["SALE_INVOICE_ID"]), Convert.ToInt64(drMaster["CUSTOMER_ID"]));
                        }
                        catch (Exception ex)
                        {
                            WriteLog(ex.ToString());
                            IsTimmerRunning = true;
                            IsExceptionOccured = true;
                            lastTickTime = System.DateTime.Now;
                            timer.Start();
                            break;
                        }
                        if (dtValueCR.Rows.Count > 0)
                        {
                            customerType = dtValueCR.Rows[0]["CustomerType"].ToString();
                            bookerName = dtValueCR.Rows[0]["bookerName"].ToString();
                            tableName = dtValueCR.Rows[0]["tableName"].ToString();
                            maxOrderNo = dtValueCR.Rows[0]["maxOrderNo"].ToString();
                            OrderNotes = dtValueCR.Rows[0]["OrderNotes"].ToString();

                            DataView view = new DataView(dtValueCR);
                            DataTable dtSection = view.ToTable(true, "SECTION");

                            foreach (DataRow drSec in dtSection.Rows)
                            {
                                #region Declare Tables
                                int Seconds = Convert.ToInt32(drMaster["Seconds"]);
                                DataTable dtValueNewOrder = new DataTable();
                                dtValueNewOrder.Columns.Add("SKU_ID", typeof(int));
                                dtValueNewOrder.Columns.Add("SECTION", typeof(string));
                                dtValueNewOrder.Columns.Add("CustomerType", typeof(string));
                                dtValueNewOrder.Columns.Add("tableName", typeof(string));
                                dtValueNewOrder.Columns.Add("bookerName", typeof(string));
                                dtValueNewOrder.Columns.Add("SKU_NAME", typeof(string));
                                dtValueNewOrder.Columns.Add("ORDER_NOTES", typeof(string));
                                dtValueNewOrder.Columns.Add("DealName", typeof(string));
                                dtValueNewOrder.Columns.Add("SKU_HIE_NAME", typeof(string));
                                dtValueNewOrder.Columns.Add("maxOrderNo", typeof(string));
                                dtValueNewOrder.Columns.Add("QTY", typeof(string));
                                dtValueNewOrder.Columns.Add("DealQTY", typeof(string));
                                dtValueNewOrder.Columns.Add("Modifiers", typeof(string));
                                dtValueNewOrder.Columns.Add("SALE_INVOICE_DETAIL_ID", typeof(long));
                                dtValueNewOrder.Columns.Add("IsUngroup", typeof(bool));
                                dtValueNewOrder.Columns.Add("TIME_STAMP2", typeof(string));
                                dtValueNewOrder.Columns.Add("LastUpdateDateTime", typeof(string));
                                dtValueNewOrder.Columns.Add("ModifierParetn_Row_ID", typeof(string));

                                DataTable dtValueStickerPrint = new DataTable();
                                dtValueStickerPrint.Columns.Add("SKU_ID", typeof(int));
                                dtValueStickerPrint.Columns.Add("tableName", typeof(string));
                                dtValueStickerPrint.Columns.Add("SKU_NAME", typeof(string));
                                dtValueStickerPrint.Columns.Add("ORDER_NOTES", typeof(string));
                                dtValueStickerPrint.Columns.Add("maxOrderNo", typeof(string));
                                dtValueStickerPrint.Columns.Add("QTY", typeof(string));
                                dtValueStickerPrint.Columns.Add("Modifiers", typeof(string));
                                dtValueStickerPrint.Columns.Add("CUSTOMER_NAME", typeof(string));
                                dtValueStickerPrint.Columns.Add("ContactNo", typeof(string));
                                dtValueStickerPrint.Columns.Add("SALE_INVOICE_DETAIL_ID", typeof(long));

                                DataTable dtValueVoid = new DataTable();
                                dtValueVoid.Columns.Add("SKU_ID", typeof(int));
                                dtValueVoid.Columns.Add("SECTION", typeof(string));
                                dtValueVoid.Columns.Add("CustomerType", typeof(string));
                                dtValueVoid.Columns.Add("tableName", typeof(string));
                                dtValueVoid.Columns.Add("bookerName", typeof(string));
                                dtValueVoid.Columns.Add("SKU_NAME", typeof(string));
                                dtValueVoid.Columns.Add("ORDER_NOTES", typeof(string));
                                dtValueVoid.Columns.Add("DealName", typeof(string));
                                dtValueVoid.Columns.Add("SKU_HIE_NAME", typeof(string));
                                dtValueVoid.Columns.Add("maxOrderNo", typeof(string));
                                dtValueVoid.Columns.Add("QTY", typeof(string));
                                dtValueVoid.Columns.Add("DealQTY", typeof(string));
                                dtValueVoid.Columns.Add("Modifiers", typeof(string));
                                dtValueVoid.Columns.Add("SALE_INVOICE_DETAIL_ID", typeof(long));
                                dtValueVoid.Columns.Add("IsUngroup", typeof(bool));
                                dtValueVoid.Columns.Add("TIME_STAMP2", typeof(string));
                                dtValueVoid.Columns.Add("LastUpdateDateTime", typeof(string));
                                dtValueVoid.Columns.Add("ModifierParetn_Row_ID", typeof(string));

                                DataTable dtValueLess = new DataTable();
                                dtValueLess.Columns.Add("SKU_ID", typeof(int));
                                dtValueLess.Columns.Add("SECTION", typeof(string));
                                dtValueLess.Columns.Add("CustomerType", typeof(string));
                                dtValueLess.Columns.Add("tableName", typeof(string));
                                dtValueLess.Columns.Add("bookerName", typeof(string));
                                dtValueLess.Columns.Add("SKU_NAME", typeof(string));
                                dtValueLess.Columns.Add("ORDER_NOTES", typeof(string));
                                dtValueLess.Columns.Add("DealName", typeof(string));
                                dtValueLess.Columns.Add("SKU_HIE_NAME", typeof(string));
                                dtValueLess.Columns.Add("maxOrderNo", typeof(string));
                                dtValueLess.Columns.Add("QTY", typeof(string));
                                dtValueLess.Columns.Add("DealQTY", typeof(string));
                                dtValueLess.Columns.Add("Modifiers", typeof(string));
                                dtValueLess.Columns.Add("SALE_INVOICE_DETAIL_ID", typeof(long));
                                dtValueLess.Columns.Add("IsUngroup", typeof(bool));
                                dtValueLess.Columns.Add("TIME_STAMP2", typeof(string));
                                dtValueLess.Columns.Add("LastUpdateDateTime", typeof(string));
                                dtValueLess.Columns.Add("ModifierParetn_Row_ID", typeof(string));

                                DataTable dtValueAdd = new DataTable();
                                dtValueAdd.Columns.Add("SKU_ID", typeof(int));
                                dtValueAdd.Columns.Add("SECTION", typeof(string));
                                dtValueAdd.Columns.Add("CustomerType", typeof(string));
                                dtValueAdd.Columns.Add("tableName", typeof(string));
                                dtValueAdd.Columns.Add("bookerName", typeof(string));
                                dtValueAdd.Columns.Add("SKU_NAME", typeof(string));
                                dtValueAdd.Columns.Add("ORDER_NOTES", typeof(string));
                                dtValueAdd.Columns.Add("DealName", typeof(string));
                                dtValueAdd.Columns.Add("SKU_HIE_NAME", typeof(string));
                                dtValueAdd.Columns.Add("maxOrderNo", typeof(string));
                                dtValueAdd.Columns.Add("QTY", typeof(string));
                                dtValueAdd.Columns.Add("DealQTY", typeof(string));
                                dtValueAdd.Columns.Add("Modifiers", typeof(string));
                                dtValueAdd.Columns.Add("SALE_INVOICE_DETAIL_ID", typeof(long));
                                dtValueAdd.Columns.Add("IsUngroup", typeof(bool));
                                dtValueAdd.Columns.Add("TIME_STAMP2", typeof(string));
                                dtValueAdd.Columns.Add("LastUpdateDateTime", typeof(string));
                                dtValueAdd.Columns.Add("ModifierParetn_Row_ID", typeof(string));

                                DataTable dtValueItemAdd = new DataTable();
                                dtValueItemAdd.Columns.Add("SKU_ID", typeof(int));
                                dtValueItemAdd.Columns.Add("SECTION", typeof(string));
                                dtValueItemAdd.Columns.Add("CustomerType", typeof(string));
                                dtValueItemAdd.Columns.Add("tableName", typeof(string));
                                dtValueItemAdd.Columns.Add("bookerName", typeof(string));
                                dtValueItemAdd.Columns.Add("SKU_NAME", typeof(string));
                                dtValueItemAdd.Columns.Add("ORDER_NOTES", typeof(string));
                                dtValueItemAdd.Columns.Add("DealName", typeof(string));
                                dtValueItemAdd.Columns.Add("SKU_HIE_NAME", typeof(string));
                                dtValueItemAdd.Columns.Add("maxOrderNo", typeof(string));
                                dtValueItemAdd.Columns.Add("QTY", typeof(string));
                                dtValueItemAdd.Columns.Add("DealQTY", typeof(string));
                                dtValueItemAdd.Columns.Add("Modifiers", typeof(string));
                                dtValueItemAdd.Columns.Add("SALE_INVOICE_DETAIL_ID", typeof(long));
                                dtValueItemAdd.Columns.Add("IsUngroup", typeof(bool));
                                dtValueItemAdd.Columns.Add("TIME_STAMP2", typeof(string));
                                dtValueItemAdd.Columns.Add("LastUpdateDateTime", typeof(string));
                                dtValueItemAdd.Columns.Add("ModifierParetn_Row_ID", typeof(string));

                                #endregion
                                #region Populate Table for printing
                                try
                                {
                                    foreach (DataRow dr in dtValueCR.Rows)
                                    {
                                        //Regular KOT
                                        if (dr["IsStickerPrint"].ToString().ToLower() == "false" || StickerSectionBoth == "1")
                                        {
                                            //New KOT
                                            if (dr["MODIFIER_PARENT_ID"].ToString() == "0" && dr["SECTION"].ToString() == drSec["SECTION"].ToString() && dr["VOID"].ToString().ToLower() == "false" && Convert.ToDecimal(dr["QTY"].ToString()) - Convert.ToDecimal(dr["PR_COUNT"].ToString()) > 0 && Seconds < 2)
                                            {
                                                DataRow row = dtValueNewOrder.NewRow();
                                                row["SKU_ID"] = dr["SKU_ID"];
                                                row["SECTION"] = dr["SECTION"];
                                                row["CustomerType"] = dr["CustomerType"];
                                                row["tableName"] = dr["tableName"];
                                                row["bookerName"] = dr["bookerName"];
                                                row["SKU_NAME"] = dr["SKU_NAME"]; ;
                                                row["ORDER_NOTES"] = dr["ORDER_NOTES"];
                                                row["DealName"] = dr["DealName"];
                                                row["SKU_HIE_NAME"] = dr["SKU_HIE_NAME"];
                                                row["maxOrderNo"] = dr["maxOrderNo"];
                                                row["QTY"] = dr["QTY"];
                                                row["DealQTY"] = dr["DealQTY"];
                                                string Modifiers = string.Empty;
                                                foreach (DataRow drModifierChild in dtValueCR.Rows)
                                                {
                                                    if (drModifierChild["MODIFIER_PARENT_ID"].ToString() != "0")
                                                    {
                                                        if (drModifierChild["MODIFIER_PARENT_ID"].ToString() == dr["SKU_ID"].ToString() && drModifierChild["ModifierParetn_Row_ID"].ToString() == dr["ModifierParetn_Row_ID"].ToString())
                                                        {
                                                            if (!SaleInvoiceDetailId.Contains(drModifierChild["SALE_INVOICE_DETAIL_ID"].ToString()))
                                                            {
                                                                SaleInvoiceDetailId.Add(drModifierChild["SALE_INVOICE_DETAIL_ID"].ToString());

                                                                Modifiers += drModifierChild["SKU_NAME"].ToString();
                                                                Modifiers += "<br>";
                                                            }
                                                        }
                                                    }
                                                }
                                                if (Modifiers.Length > 0)
                                                {
                                                    Modifiers = Modifiers.Substring(0, Modifiers.Length - 4);
                                                }
                                                row["Modifiers"] = Modifiers;
                                                row["SALE_INVOICE_DETAIL_ID"] = dr["SALE_INVOICE_DETAIL_ID"];
                                                row["IsUngroup"] = dr["IsUngroup"];
                                                row["TIME_STAMP2"] = dr["TIME_STAMP2"];
                                                row["LastUpdateDateTime"] = dr["LastUpdateDateTime"];
                                                row["ModifierParetn_Row_ID"] = dr["ModifierParetn_Row_ID"];
                                                dtValueNewOrder.Rows.Add(row);
                                            }
                                            //Void Item Or Item Deleted
                                            else if (dr["MODIFIER_PARENT_ID"].ToString() == "0" && dr["SECTION"].ToString() == drSec["SECTION"].ToString() && dr["VOID"].ToString().ToLower() == "true" && Convert.ToDecimal(dr["QTY"].ToString()) - Convert.ToDecimal(dr["PR_COUNT"].ToString()) > 0 && ExcludeCancelKOT == "0")
                                            {
                                                DataRow row = dtValueVoid.NewRow();
                                                row["SKU_ID"] = dr["SKU_ID"];
                                                row["SECTION"] = dr["SECTION"];
                                                row["CustomerType"] = dr["CustomerType"];
                                                row["tableName"] = dr["tableName"];
                                                row["bookerName"] = dr["bookerName"];
                                                row["SKU_NAME"] = dr["SKU_NAME"];
                                                row["ORDER_NOTES"] = dr["ORDER_NOTES"];
                                                row["DealName"] = dr["DealName"];
                                                row["SKU_HIE_NAME"] = dr["SKU_HIE_NAME"];
                                                row["maxOrderNo"] = dr["maxOrderNo"];
                                                row["QTY"] = dr["QTY"];
                                                row["DealQTY"] = dr["DealQTY"];
                                                string Modifiers = string.Empty;
                                                foreach (DataRow drModifierChild in dtValueCR.Rows)
                                                {
                                                    if (drModifierChild["MODIFIER_PARENT_ID"].ToString() != "0")
                                                    {
                                                        if (drModifierChild["MODIFIER_PARENT_ID"].ToString() == dr["SKU_ID"].ToString() && drModifierChild["ModifierParetn_Row_ID"].ToString() == dr["ModifierParetn_Row_ID"].ToString())
                                                        {
                                                            if (!SaleInvoiceDetailId.Contains(drModifierChild["SALE_INVOICE_DETAIL_ID"].ToString()))
                                                            {
                                                                SaleInvoiceDetailId.Add(drModifierChild["SALE_INVOICE_DETAIL_ID"].ToString());

                                                                Modifiers += drModifierChild["SKU_NAME"].ToString();
                                                                Modifiers += "<br>";
                                                            }
                                                        }
                                                    }
                                                }
                                                if (Modifiers.Length > 0)
                                                {
                                                    Modifiers = Modifiers.Substring(0, Modifiers.Length - 4);
                                                }
                                                row["Modifiers"] = Modifiers;
                                                row["SALE_INVOICE_DETAIL_ID"] = dr["SALE_INVOICE_DETAIL_ID"];
                                                row["IsUngroup"] = dr["IsUngroup"];
                                                row["TIME_STAMP2"] = dr["TIME_STAMP2"];
                                                row["LastUpdateDateTime"] = dr["LastUpdateDateTime"];
                                                row["ModifierParetn_Row_ID"] = dr["ModifierParetn_Row_ID"];
                                                dtValueVoid.Rows.Add(row);
                                            }
                                            //Less Item Or Item Quantity decreased
                                            else if (dr["MODIFIER_PARENT_ID"].ToString() == "0" && dr["SECTION"].ToString() == drSec["SECTION"].ToString() && dr["VOID"].ToString().ToLower() == "false" && Convert.ToDecimal(dr["QTY"].ToString()) - Convert.ToDecimal(dr["PR_COUNT"].ToString()) < 0 && dr["LessAddType"].ToString() == "1" && ExcludeCancelKOT == "0")
                                            {
                                                DataRow row = dtValueLess.NewRow();
                                                row["SKU_ID"] = dr["SKU_ID"];
                                                row["SECTION"] = dr["SECTION"];
                                                row["CustomerType"] = dr["CustomerType"];
                                                row["tableName"] = dr["tableName"];
                                                row["bookerName"] = dr["bookerName"];
                                                row["SKU_NAME"] = dr["SKU_NAME"];
                                                row["ORDER_NOTES"] = dr["ORDER_NOTES"];
                                                row["DealName"] = dr["DealName"];
                                                row["SKU_HIE_NAME"] = dr["SKU_HIE_NAME"];
                                                row["maxOrderNo"] = dr["maxOrderNo"];
                                                row["QTY"] = dr["QTY"];
                                                row["DealQTY"] = dr["DealQTY"];
                                                string Modifiers = string.Empty;
                                                foreach (DataRow drModifierChild in dtValueCR.Rows)
                                                {
                                                    if (drModifierChild["MODIFIER_PARENT_ID"].ToString() != "0")
                                                    {
                                                        if (drModifierChild["MODIFIER_PARENT_ID"].ToString() == dr["SKU_ID"].ToString() && drModifierChild["ModifierParetn_Row_ID"].ToString() == dr["ModifierParetn_Row_ID"].ToString())
                                                        {
                                                            if (!SaleInvoiceDetailId.Contains(drModifierChild["SALE_INVOICE_DETAIL_ID"].ToString()))
                                                            {
                                                                SaleInvoiceDetailId.Add(drModifierChild["SALE_INVOICE_DETAIL_ID"].ToString());

                                                                Modifiers += drModifierChild["SKU_NAME"].ToString();
                                                                Modifiers += "<br>";
                                                            }
                                                        }
                                                    }
                                                }
                                                if (Modifiers.Length > 0)
                                                {
                                                    Modifiers = Modifiers.Substring(0, Modifiers.Length - 4);
                                                }
                                                row["Modifiers"] = Modifiers;
                                                row["SALE_INVOICE_DETAIL_ID"] = dr["SALE_INVOICE_DETAIL_ID"];
                                                row["IsUngroup"] = dr["IsUngroup"];
                                                row["TIME_STAMP2"] = dr["TIME_STAMP2"];
                                                row["LastUpdateDateTime"] = dr["LastUpdateDateTime"];
                                                row["ModifierParetn_Row_ID"] = dr["ModifierParetn_Row_ID"];
                                                dtValueLess.Rows.Add(row);
                                            }
                                            //Add Item Quantity
                                            else if (dr["MODIFIER_PARENT_ID"].ToString() == "0" && dr["SECTION"].ToString() == drSec["SECTION"].ToString() && dr["VOID"].ToString().ToLower() == "false" && Seconds > 2 && Convert.ToDecimal(dr["QtyAddLess"]) > 0 && dr["LessAddType"].ToString() == "2")
                                            {
                                                DataRow row = dtValueAdd.NewRow();
                                                row["SKU_ID"] = dr["SKU_ID"];
                                                row["SECTION"] = dr["SECTION"];
                                                row["CustomerType"] = dr["CustomerType"];
                                                row["tableName"] = dr["tableName"];
                                                row["bookerName"] = dr["bookerName"];
                                                row["SKU_NAME"] = dr["SKU_NAME"];
                                                row["ORDER_NOTES"] = dr["ORDER_NOTES"];
                                                row["DealName"] = dr["DealName"];
                                                row["SKU_HIE_NAME"] = dr["SKU_HIE_NAME"];
                                                row["maxOrderNo"] = dr["maxOrderNo"];
                                                row["QTY"] = dr["QTY"];
                                                row["DealQTY"] = dr["DealQTY"];
                                                string Modifiers = string.Empty;
                                                foreach (DataRow drModifierChild in dtValueCR.Rows)
                                                {
                                                    if (drModifierChild["MODIFIER_PARENT_ID"].ToString() != "0")
                                                    {
                                                        if (drModifierChild["MODIFIER_PARENT_ID"].ToString() == dr["SKU_ID"].ToString() && drModifierChild["ModifierParetn_Row_ID"].ToString() == dr["ModifierParetn_Row_ID"].ToString())
                                                        {
                                                            if (!SaleInvoiceDetailId.Contains(drModifierChild["SALE_INVOICE_DETAIL_ID"].ToString()))
                                                            {
                                                                SaleInvoiceDetailId.Add(drModifierChild["SALE_INVOICE_DETAIL_ID"].ToString());

                                                                Modifiers += drModifierChild["SKU_NAME"].ToString();
                                                                Modifiers += "<br>";
                                                            }
                                                        }
                                                    }
                                                }
                                                if (Modifiers.Length > 0)
                                                {
                                                    Modifiers = Modifiers.Substring(0, Modifiers.Length - 4);
                                                }
                                                row["Modifiers"] = Modifiers;
                                                row["SALE_INVOICE_DETAIL_ID"] = dr["SALE_INVOICE_DETAIL_ID"];
                                                row["IsUngroup"] = dr["IsUngroup"];
                                                row["TIME_STAMP2"] = dr["TIME_STAMP2"];
                                                row["LastUpdateDateTime"] = dr["LastUpdateDateTime"];
                                                row["ModifierParetn_Row_ID"] = dr["ModifierParetn_Row_ID"];
                                                dtValueAdd.Rows.Add(row);
                                            }
                                            //Add New Item 
                                            else if (dr["MODIFIER_PARENT_ID"].ToString() == "0" && dr["SECTION"].ToString() == drSec["SECTION"].ToString() && dr["VOID"].ToString().ToLower() == "false" && Convert.ToDecimal(dr["QTY"].ToString()) - Convert.ToDecimal(dr["PR_COUNT"].ToString()) > 0 && Seconds > 2 && Convert.ToDecimal(dr["QtyAddLess"]) == 0)
                                            {
                                                DataRow row = dtValueItemAdd.NewRow();
                                                row["SKU_ID"] = dr["SKU_ID"];
                                                row["SECTION"] = dr["SECTION"];
                                                row["CustomerType"] = dr["CustomerType"];
                                                row["tableName"] = dr["tableName"];
                                                row["bookerName"] = dr["bookerName"];
                                                row["SKU_NAME"] = dr["SKU_NAME"];
                                                row["ORDER_NOTES"] = dr["ORDER_NOTES"];
                                                row["DealName"] = dr["DealName"];
                                                row["SKU_HIE_NAME"] = dr["SKU_HIE_NAME"];
                                                row["maxOrderNo"] = dr["maxOrderNo"];
                                                row["QTY"] = dr["QTY"];
                                                row["DealQTY"] = dr["DealQTY"];
                                                string Modifiers = string.Empty;
                                                foreach (DataRow drModifierChild in dtValueCR.Rows)
                                                {
                                                    if (drModifierChild["MODIFIER_PARENT_ID"].ToString() != "0")
                                                    {
                                                        if (drModifierChild["MODIFIER_PARENT_ID"].ToString() == dr["SKU_ID"].ToString() && drModifierChild["ModifierParetn_Row_ID"].ToString() == dr["ModifierParetn_Row_ID"].ToString())
                                                        {
                                                            if (!SaleInvoiceDetailId.Contains(drModifierChild["SALE_INVOICE_DETAIL_ID"].ToString()))
                                                            {
                                                                SaleInvoiceDetailId.Add(drModifierChild["SALE_INVOICE_DETAIL_ID"].ToString());

                                                                Modifiers += drModifierChild["SKU_NAME"].ToString();
                                                                Modifiers += "<br>";
                                                            }
                                                        }
                                                    }
                                                }
                                                if (Modifiers.Length > 0)
                                                {
                                                    Modifiers = Modifiers.Substring(0, Modifiers.Length - 4);
                                                }
                                                row["Modifiers"] = Modifiers;
                                                row["SALE_INVOICE_DETAIL_ID"] = dr["SALE_INVOICE_DETAIL_ID"];
                                                row["IsUngroup"] = dr["IsUngroup"];
                                                row["TIME_STAMP2"] = dr["TIME_STAMP2"];
                                                row["LastUpdateDateTime"] = dr["LastUpdateDateTime"];
                                                row["ModifierParetn_Row_ID"] = dr["ModifierParetn_Row_ID"];
                                                dtValueItemAdd.Rows.Add(row);
                                            }
                                        }
                                        //KOT Sticker Print
                                        if (dr["IsStickerPrint"].ToString().ToLower() == "true")
                                        {
                                            //New KOT
                                            if (dr["MODIFIER_PARENT_ID"].ToString() == "0" && dr["SECTION"].ToString() == drSec["SECTION"].ToString() && dr["VOID"].ToString().ToLower() == "false" && Convert.ToDecimal(dr["QTY"].ToString()) - Convert.ToDecimal(dr["PR_COUNT"].ToString()) > 0)
                                            {
                                                DataRow row = dtValueStickerPrint.NewRow();
                                                row["SKU_ID"] = dr["SKU_ID"];
                                                row["tableName"] = dr["tableName"];
                                                row["SKU_NAME"] = dr["SKU_NAME"];
                                                row["ORDER_NOTES"] = dr["ORDER_NOTES"];
                                                row["maxOrderNo"] = dr["maxOrderNo"];
                                                row["QTY"] = dr["QTY"];
                                                row["CUSTOMER_NAME"] = dr["CUSTOMER_NAME"];
                                                row["ContactNo"] = dr["ContactNo"];
                                                string Modifiers = string.Empty;
                                                foreach (DataRow drModifierChild in dtValueCR.Rows)
                                                {
                                                    if (drModifierChild["MODIFIER_PARENT_ID"].ToString() != "0")
                                                    {
                                                        if (drModifierChild["MODIFIER_PARENT_ID"].ToString() == dr["SKU_ID"].ToString() && drModifierChild["ModifierParetn_Row_ID"].ToString() == dr["ModifierParetn_Row_ID"].ToString())
                                                        {
                                                            if (!SaleInvoiceDetailIdModifier.Contains(drModifierChild["SALE_INVOICE_DETAIL_ID"].ToString()))
                                                            {
                                                                SaleInvoiceDetailIdModifier.Add(drModifierChild["SALE_INVOICE_DETAIL_ID"].ToString());
                                                                Modifiers += drModifierChild["SKU_NAME"].ToString();
                                                                Modifiers += "<br>";
                                                            }
                                                        }
                                                    }
                                                }
                                                if (Modifiers.Length > 0)
                                                {
                                                    Modifiers = Modifiers.Substring(0, Modifiers.Length - 4);
                                                }
                                                row["Modifiers"] = Modifiers;
                                                row["SALE_INVOICE_DETAIL_ID"] = dr["SALE_INVOICE_DETAIL_ID"];
                                                row["IsUngroup"] = dr["IsUngroup"];
                                                row["TIME_STAMP2"] = dr["TIME_STAMP2"];
                                                row["LastUpdateDateTime"] = dr["LastUpdateDateTime"];
                                                row["ModifierParetn_Row_ID"] = dr["ModifierParetn_Row_ID"];
                                                dtValueStickerPrint.Rows.Add(row);
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    WriteLog(ex.ToString());
                                    IsTimmerRunning = true;
                                    IsExceptionOccured = true;
                                    lastTickTime = System.DateTime.Now;
                                    timer.Start();
                                    break;
                                }
                                #endregion
                                #region Print Tables Data
                                try
                                {
                                    if (dtValueNewOrder.Rows.Count > 0)
                                    {
                                        List<ProductSection> ProductSectionList = (List<ProductSection>)SectionPrinterList;
                                        string SectionName = drSec["SECTION"].ToString().Trim().ToLower();
                                        var sectionPrinter = ProductSectionList.Find(p => p.SectionName.ToLower().Trim() == SectionName);
                                        if (sectionPrinter != null)
                                        {
                                            int NofOfCopies = sectionPrinter.NO_OF_PRINTS;
                                            if (!IsExceptionOccured)
                                            {
                                                WriteLog(string.Format("Order No-{0}-KOT Started Printing.", drMaster["SALE_INVOICE_ID"].ToString()));
                                                if (!IsExceptionOccured)
                                                {
                                                    if (!PrintKOTCrystalReport(dtValueNewOrder, sectionPrinter.PrinterName, drMaster["SALE_INVOICE_ID"].ToString(), 0, Seconds, NofOfCopies, false, drMaster["REMARKS"].ToString(), drMaster["LastUpdateDateTime"].ToString(), drSec["SECTION"].ToString().Trim().ToUpper(), "NewKOT"))
                                                    {
                                                        timer.Start();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (dtValueVoid.Rows.Count > 0)
                                    {
                                        List<ProductSection> ProductSectionList = (List<ProductSection>)SectionPrinterList;
                                        string SectionName = drSec["SECTION"].ToString().Trim().ToLower();
                                        var sectionPrinter = ProductSectionList.Find(p => p.SectionName.ToLower().Trim() == SectionName);
                                        if (sectionPrinter != null)
                                        {
                                            int NofOfCopies = sectionPrinter.NO_OF_PRINTS;
                                            if (!IsExceptionOccured)
                                            {
                                                WriteLog(string.Format("Order No-{0}-KOT Cancel Item Started Printing.", drMaster["SALE_INVOICE_ID"].ToString()));
                                                if (!IsExceptionOccured)
                                                {
                                                    if (!PrintKOTCrystalReport(dtValueVoid, sectionPrinter.PrinterName, drMaster["SALE_INVOICE_ID"].ToString(), 1, Seconds, NofOfCopies, false, drMaster["REMARKS"].ToString(), drMaster["LastUpdateDateTime"].ToString(), drSec["SECTION"].ToString().Trim().ToUpper(), "CancelKOT"))
                                                    {
                                                        timer.Start();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (dtValueLess.Rows.Count > 0)
                                    {
                                        List<ProductSection> ProductSectionList = (List<ProductSection>)SectionPrinterList;
                                        string SectionName = drSec["SECTION"].ToString().Trim().ToLower();
                                        var sectionPrinter = ProductSectionList.Find(p => p.SectionName.ToLower().Trim() == SectionName);
                                        if (sectionPrinter != null)
                                        {
                                            int NofOfCopies = sectionPrinter.NO_OF_PRINTS;
                                            if (!IsExceptionOccured)
                                            {
                                                WriteLog(string.Format("Order No-{0}-KOT Less Item Qty Started Printing.", drMaster["SALE_INVOICE_ID"].ToString()));
                                                if (!IsExceptionOccured)
                                                {
                                                    if (!PrintKOTCrystalReport(dtValueLess, sectionPrinter.PrinterName, drMaster["SALE_INVOICE_ID"].ToString(), 2, Seconds, NofOfCopies, false, drMaster["REMARKS"].ToString(), drMaster["LastUpdateDateTime"].ToString(), drSec["SECTION"].ToString().Trim().ToUpper(), "LessKOT"))
                                                    {
                                                        timer.Start();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (dtValueAdd.Rows.Count > 0)
                                    {
                                        List<ProductSection> ProductSectionList = (List<ProductSection>)SectionPrinterList;
                                        string SectionName = drSec["SECTION"].ToString().Trim().ToLower();
                                        var sectionPrinter = ProductSectionList.Find(p => p.SectionName.ToLower().Trim() == SectionName);
                                        if (sectionPrinter != null)
                                        {
                                            int NofOfCopies = sectionPrinter.NO_OF_PRINTS;
                                            if (!IsExceptionOccured)
                                            {
                                                WriteLog(string.Format("Order No-{0}-KOT Add Item Qty Started Printing.", drMaster["SALE_INVOICE_ID"].ToString()));
                                                if (!IsExceptionOccured)
                                                {
                                                    if (!PrintKOTCrystalReport(dtValueAdd, sectionPrinter.PrinterName, drMaster["SALE_INVOICE_ID"].ToString(), 5, Seconds, NofOfCopies, false, drMaster["REMARKS"].ToString(), drMaster["LastUpdateDateTime"].ToString(), drSec["SECTION"].ToString().Trim().ToUpper(), "AddItemQtyKOT"))
                                                    {
                                                        timer.Start();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (dtValueItemAdd.Rows.Count > 0)
                                    {
                                        List<ProductSection> ProductSectionList = (List<ProductSection>)SectionPrinterList;
                                        string SectionName = drSec["SECTION"].ToString().Trim().ToLower();
                                        var sectionPrinter = ProductSectionList.Find(p => p.SectionName.ToLower().Trim() == SectionName);
                                        if (sectionPrinter != null)
                                        {
                                            int NofOfCopies = sectionPrinter.NO_OF_PRINTS;
                                            if (!IsExceptionOccured)
                                            {
                                                WriteLog(string.Format("Order No-{0}-KOT Add New Item Started Printing.", drMaster["SALE_INVOICE_ID"].ToString()));
                                                if (!IsExceptionOccured)
                                                {
                                                    if (!PrintKOTCrystalReport(dtValueItemAdd, sectionPrinter.PrinterName, drMaster["SALE_INVOICE_ID"].ToString(), 6, Seconds, NofOfCopies, false, drMaster["REMARKS"].ToString(), drMaster["LastUpdateDateTime"].ToString(), drSec["SECTION"].ToString().Trim().ToUpper(), "AddItemKOT"))
                                                    {
                                                        timer.Start();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    WriteLog(ex.ToString());
                                    IsTimmerRunning = true;
                                    IsExceptionOccured = true;
                                    lastTickTime = System.DateTime.Now;
                                    timer.Start();
                                    break;
                                }
                                #endregion
                                #region Print Sticker Tables Data
                                try
                                {
                                    if (dtValueStickerPrint.Rows.Count > 0)
                                    {
                                        List<ProductSection> ProductSectionList = (List<ProductSection>)SectionPrinterList;
                                        string SectionName = drSec["SECTION"].ToString().Trim().ToLower();
                                        var sectionPrinter = ProductSectionList.Find(p => p.SectionName.ToLower().Trim() == SectionName);
                                        if (sectionPrinter != null)
                                        {
                                            int NoOfCopies = sectionPrinter.NO_OF_PRINTS;
                                            foreach (DataRow drStickerPrint in dtValueStickerPrint.Rows)
                                            {
                                                if ((int)Math.Floor(Convert.ToDecimal(drStickerPrint["QTY"])) > 1)
                                                {
                                                    for (int i = 0; i < (int)Math.Floor(Convert.ToDecimal(drStickerPrint["QTY"])); i++)
                                                    {
                                                        DataTable dtStickerPrintSingleQty = new DataTable();
                                                        dtStickerPrintSingleQty.Columns.Add("SKU_ID", typeof(int));
                                                        dtStickerPrintSingleQty.Columns.Add("SKU_NAME", typeof(string));
                                                        dtStickerPrintSingleQty.Columns.Add("ORDER_NOTES", typeof(string));
                                                        dtStickerPrintSingleQty.Columns.Add("maxOrderNo", typeof(string));
                                                        dtStickerPrintSingleQty.Columns.Add("Modifiers", typeof(string));
                                                        dtStickerPrintSingleQty.Columns.Add("CUSTOMER_NAME", typeof(string));
                                                        dtStickerPrintSingleQty.Columns.Add("ContactNo", typeof(string));

                                                        DataRow drSticker = dtStickerPrintSingleQty.NewRow();
                                                        drSticker["SKU_ID"] = drStickerPrint["SKU_ID"];
                                                        drSticker["SKU_NAME"] = drStickerPrint["SKU_NAME"];
                                                        drSticker["ORDER_NOTES"] = drStickerPrint["ORDER_NOTES"];
                                                        drSticker["maxOrderNo"] = drStickerPrint["maxOrderNo"];
                                                        drSticker["Modifiers"] = drStickerPrint["Modifiers"];
                                                        drSticker["CUSTOMER_NAME"] = drStickerPrint["CUSTOMER_NAME"];
                                                        drSticker["ContactNo"] = drStickerPrint["ContactNo"];
                                                        dtStickerPrintSingleQty.Rows.Add(drSticker);
                                                        WriteLog(string.Format("Order No-{0}, Item-{1} Sticker Started Printing.", drMaster["SALE_INVOICE_ID"].ToString(), dtStickerPrintSingleQty.Rows[0]["SKU_NAME"].ToString()));
                                                        PrintStickerCrystalReport(dtStickerPrintSingleQty, System.Configuration.ConfigurationManager.AppSettings["StickerPrinterName"].ToString(), drMaster["SALE_INVOICE_ID"].ToString(), 1, Seconds, sectionPrinter.NO_OF_PRINTS);
                                                    }
                                                }
                                                else
                                                {
                                                    DataTable dtStickerPrint = new DataTable();
                                                    dtStickerPrint.Columns.Add("SKU_ID", typeof(int));
                                                    dtStickerPrint.Columns.Add("SKU_NAME", typeof(string));
                                                    dtStickerPrint.Columns.Add("ORDER_NOTES", typeof(string));
                                                    dtStickerPrint.Columns.Add("maxOrderNo", typeof(string));
                                                    dtStickerPrint.Columns.Add("Modifiers", typeof(string));
                                                    dtStickerPrint.Columns.Add("CUSTOMER_NAME", typeof(string));
                                                    dtStickerPrint.Columns.Add("ContactNo", typeof(string));

                                                    DataRow drSticker = dtStickerPrint.NewRow();
                                                    drSticker["SKU_ID"] = drStickerPrint["SKU_ID"];
                                                    drSticker["SKU_NAME"] = drStickerPrint["SKU_NAME"];
                                                    drSticker["ORDER_NOTES"] = drStickerPrint["ORDER_NOTES"];
                                                    drSticker["maxOrderNo"] = drStickerPrint["maxOrderNo"];
                                                    drSticker["Modifiers"] = drStickerPrint["Modifiers"];
                                                    drSticker["CUSTOMER_NAME"] = drStickerPrint["CUSTOMER_NAME"];
                                                    drSticker["ContactNo"] = drStickerPrint["ContactNo"];
                                                    dtStickerPrint.Rows.Add(drSticker);
                                                    WriteLog(string.Format("Order No-{0}, Item-{1} Sticker Started Printing.", drMaster["SALE_INVOICE_ID"].ToString(), dtStickerPrint.Rows[0]["SKU_NAME"].ToString()));
                                                    PrintStickerCrystalReport(dtStickerPrint, System.Configuration.ConfigurationManager.AppSettings["StickerPrinterName"].ToString(), drMaster["SALE_INVOICE_ID"].ToString(), 1, Seconds, sectionPrinter.NO_OF_PRINTS);
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    WriteLog(ex.ToString());
                                    IsTimmerRunning = true;
                                    IsExceptionOccured = true;
                                    lastTickTime = System.DateTime.Now;
                                    timer.Start();
                                    break;
                                }
                                #endregion
                            }
                            #region FullKOT Print
                            try
                            {
                                if (IsFullKOT == "1")
                                {
                                    DataTable dtValueNewOrder = new DataTable();
                                    dtValueNewOrder.Columns.Add("SKU_ID", typeof(int));
                                    dtValueNewOrder.Columns.Add("SECTION", typeof(string));
                                    dtValueNewOrder.Columns.Add("CustomerType", typeof(string));
                                    dtValueNewOrder.Columns.Add("tableName", typeof(string));
                                    dtValueNewOrder.Columns.Add("bookerName", typeof(string));
                                    dtValueNewOrder.Columns.Add("SKU_NAME", typeof(string));
                                    dtValueNewOrder.Columns.Add("ORDER_NOTES", typeof(string));
                                    dtValueNewOrder.Columns.Add("DealName", typeof(string));
                                    dtValueNewOrder.Columns.Add("SKU_HIE_NAME", typeof(string));
                                    dtValueNewOrder.Columns.Add("maxOrderNo", typeof(string));
                                    dtValueNewOrder.Columns.Add("QTY", typeof(string));
                                    dtValueNewOrder.Columns.Add("DealQTY", typeof(string));
                                    dtValueNewOrder.Columns.Add("Modifiers", typeof(string));
                                    dtValueNewOrder.Columns.Add("SALE_INVOICE_DETAIL_ID", typeof(long));
                                    dtValueNewOrder.Columns.Add("IsUngroup", typeof(bool));
                                    dtValueNewOrder.Columns.Add("TIME_STAMP2", typeof(string));
                                    dtValueNewOrder.Columns.Add("LastUpdateDateTime", typeof(string));
                                    dtValueNewOrder.Columns.Add("ModifierParetn_Row_ID", typeof(string));

                                    foreach (DataRow dr in dtValueCR.Rows)
                                    {
                                        if (dr["MODIFIER_PARENT_ID"].ToString() == "0" && dr["VOID"].ToString().ToLower() == "false" && Convert.ToDecimal(dr["QTY"].ToString()) - Convert.ToDecimal(dr["PR_COUNT"].ToString()) > 0)
                                        {
                                            DataRow row = dtValueNewOrder.NewRow();
                                            row["SKU_ID"] = dr["SKU_ID"];
                                            row["SECTION"] = dr["SECTION"];
                                            row["CustomerType"] = dr["CustomerType"];
                                            row["tableName"] = dr["tableName"];
                                            row["bookerName"] = dr["bookerName"];
                                            row["SKU_NAME"] = dr["SKU_NAME"];
                                            row["ORDER_NOTES"] = dr["ORDER_NOTES"];
                                            row["DealName"] = dr["DealName"];
                                            row["SKU_HIE_NAME"] = dr["SKU_HIE_NAME"];
                                            row["maxOrderNo"] = dr["maxOrderNo"];
                                            row["QTY"] = dr["QTY"];
                                            row["DealQTY"] = dr["DealQTY"];
                                            string Modifiers = string.Empty;
                                            foreach (DataRow drModifierChild in dtValueCR.Rows)
                                            {
                                                if (drModifierChild["MODIFIER_PARENT_ID"].ToString() != "0")
                                                {
                                                    if (drModifierChild["MODIFIER_PARENT_ID"].ToString() == dr["SKU_ID"].ToString() && drModifierChild["ModifierParetn_Row_ID"].ToString() == dr["ModifierParetn_Row_ID"].ToString())
                                                    {
                                                        Modifiers += drModifierChild["SKU_NAME"].ToString();
                                                        Modifiers += "<br>";
                                                    }
                                                }
                                            }
                                            row["Modifiers"] = Modifiers;
                                            row["SALE_INVOICE_DETAIL_ID"] = dr["SALE_INVOICE_DETAIL_ID"];
                                            row["IsUngroup"] = dr["IsUngroup"];
                                            row["TIME_STAMP2"] = dr["TIME_STAMP2"];
                                            row["LastUpdateDateTime"] = dr["LastUpdateDateTime"];
                                            row["ModifierParetn_Row_ID"] = dr["ModifierParetn_Row_ID"];
                                            dtValueNewOrder.Rows.Add(row);
                                        }
                                    }
                                    if (dtValueNewOrder.Rows.Count > 0)
                                    {
                                        List<ProductSection> ProductSectionList = (List<ProductSection>)SectionPrinterList;
                                        var sectionPrinter = ProductSectionList.Find(p => p.IS_FULL_KOT == true);
                                        if (sectionPrinter != null)
                                        {
                                            int NofOfCopies = sectionPrinter.NO_OF_PRINTS;
                                            if (!IsExceptionOccured)
                                            {
                                                WriteLog(string.Format("Order No-{0}-Full KOT Started Printing.", drMaster["SALE_INVOICE_ID"].ToString()));
                                                if (!IsExceptionOccured)
                                                {
                                                    if (!PrintKOTCrystalReport(dtValueNewOrder, sectionPrinter.PrinterName, drMaster["SALE_INVOICE_ID"].ToString(), 3, 0, NofOfCopies, true, drMaster["REMARKS"].ToString(), drMaster["LastUpdateDateTime"].ToString(), "FULL KOT", "FULLKOT"))
                                                    {
                                                        timer.Start();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                WriteLog(ex.ToString());
                                IsTimmerRunning = true;
                                IsExceptionOccured = true;
                                lastTickTime = System.DateTime.Now;
                                break;
                            }
                            #endregion
                            #region Xpeditor Print
                            try
                            {
                                if (IsXpeditor == "1")
                                {
                                    #region Declare Tables
                                    int Seconds = Convert.ToInt32(drMaster["Seconds"]);
                                    DataTable dtValueNewOrderX = new DataTable();
                                    dtValueNewOrderX.Columns.Add("SKU_ID", typeof(int));
                                    dtValueNewOrderX.Columns.Add("SECTION", typeof(string));
                                    dtValueNewOrderX.Columns.Add("CustomerType", typeof(string));
                                    dtValueNewOrderX.Columns.Add("tableName", typeof(string));
                                    dtValueNewOrderX.Columns.Add("bookerName", typeof(string));
                                    dtValueNewOrderX.Columns.Add("SKU_NAME", typeof(string));
                                    dtValueNewOrderX.Columns.Add("ORDER_NOTES", typeof(string));
                                    dtValueNewOrderX.Columns.Add("DealName", typeof(string));
                                    dtValueNewOrderX.Columns.Add("SKU_HIE_NAME", typeof(string));
                                    dtValueNewOrderX.Columns.Add("maxOrderNo", typeof(string));
                                    dtValueNewOrderX.Columns.Add("QTY", typeof(string));
                                    dtValueNewOrderX.Columns.Add("DealQTY", typeof(string));
                                    dtValueNewOrderX.Columns.Add("Modifiers", typeof(string));
                                    dtValueNewOrderX.Columns.Add("SALE_INVOICE_DETAIL_ID", typeof(long));
                                    dtValueNewOrderX.Columns.Add("IsUngroup", typeof(bool));
                                    dtValueNewOrderX.Columns.Add("TIME_STAMP2", typeof(string));
                                    dtValueNewOrderX.Columns.Add("LastUpdateDateTime", typeof(string));
                                    dtValueNewOrderX.Columns.Add("ModifierParetn_Row_ID", typeof(string));

                                    DataTable dtValueVoidX = new DataTable();
                                    dtValueVoidX.Columns.Add("SKU_ID", typeof(int));
                                    dtValueVoidX.Columns.Add("SECTION", typeof(string));
                                    dtValueVoidX.Columns.Add("CustomerType", typeof(string));
                                    dtValueVoidX.Columns.Add("tableName", typeof(string));
                                    dtValueVoidX.Columns.Add("bookerName", typeof(string));
                                    dtValueVoidX.Columns.Add("SKU_NAME", typeof(string));
                                    dtValueVoidX.Columns.Add("ORDER_NOTES", typeof(string));
                                    dtValueVoidX.Columns.Add("DealName", typeof(string));
                                    dtValueVoidX.Columns.Add("SKU_HIE_NAME", typeof(string));
                                    dtValueVoidX.Columns.Add("maxOrderNo", typeof(string));
                                    dtValueVoidX.Columns.Add("QTY", typeof(string));
                                    dtValueVoidX.Columns.Add("DealQTY", typeof(string));
                                    dtValueVoidX.Columns.Add("Modifiers", typeof(string));
                                    dtValueVoidX.Columns.Add("SALE_INVOICE_DETAIL_ID", typeof(long));
                                    dtValueVoidX.Columns.Add("IsUngroup", typeof(bool));
                                    dtValueVoidX.Columns.Add("TIME_STAMP2", typeof(string));
                                    dtValueVoidX.Columns.Add("LastUpdateDateTime", typeof(string));
                                    dtValueVoidX.Columns.Add("ModifierParetn_Row_ID", typeof(string));

                                    DataTable dtValueLessX = new DataTable();
                                    dtValueLessX.Columns.Add("SKU_ID", typeof(int));
                                    dtValueLessX.Columns.Add("SECTION", typeof(string));
                                    dtValueLessX.Columns.Add("CustomerType", typeof(string));
                                    dtValueLessX.Columns.Add("tableName", typeof(string));
                                    dtValueLessX.Columns.Add("bookerName", typeof(string));
                                    dtValueLessX.Columns.Add("SKU_NAME", typeof(string));
                                    dtValueLessX.Columns.Add("ORDER_NOTES", typeof(string));
                                    dtValueLessX.Columns.Add("DealName", typeof(string));
                                    dtValueLessX.Columns.Add("SKU_HIE_NAME", typeof(string));
                                    dtValueLessX.Columns.Add("maxOrderNo", typeof(string));
                                    dtValueLessX.Columns.Add("QTY", typeof(string));
                                    dtValueLessX.Columns.Add("DealQTY", typeof(string));
                                    dtValueLessX.Columns.Add("Modifiers", typeof(string));
                                    dtValueLessX.Columns.Add("SALE_INVOICE_DETAIL_ID", typeof(long));
                                    dtValueLessX.Columns.Add("IsUngroup", typeof(bool));
                                    dtValueLessX.Columns.Add("TIME_STAMP2", typeof(string));
                                    dtValueLessX.Columns.Add("LastUpdateDateTime", typeof(string));
                                    dtValueLessX.Columns.Add("ModifierParetn_Row_ID", typeof(string));

                                    DataTable dtValueAddX = new DataTable();
                                    dtValueAddX.Columns.Add("SKU_ID", typeof(int));
                                    dtValueAddX.Columns.Add("SECTION", typeof(string));
                                    dtValueAddX.Columns.Add("CustomerType", typeof(string));
                                    dtValueAddX.Columns.Add("tableName", typeof(string));
                                    dtValueAddX.Columns.Add("bookerName", typeof(string));
                                    dtValueAddX.Columns.Add("SKU_NAME", typeof(string));
                                    dtValueAddX.Columns.Add("ORDER_NOTES", typeof(string));
                                    dtValueAddX.Columns.Add("DealName", typeof(string));
                                    dtValueAddX.Columns.Add("SKU_HIE_NAME", typeof(string));
                                    dtValueAddX.Columns.Add("maxOrderNo", typeof(string));
                                    dtValueAddX.Columns.Add("QTY", typeof(string));
                                    dtValueAddX.Columns.Add("DealQTY", typeof(string));
                                    dtValueAddX.Columns.Add("Modifiers", typeof(string));
                                    dtValueAddX.Columns.Add("SALE_INVOICE_DETAIL_ID", typeof(long));
                                    dtValueAddX.Columns.Add("IsUngroup", typeof(bool));
                                    dtValueAddX.Columns.Add("TIME_STAMP2", typeof(string));
                                    dtValueAddX.Columns.Add("LastUpdateDateTime", typeof(string));
                                    dtValueAddX.Columns.Add("ModifierParetn_Row_ID", typeof(string));

                                    DataTable dtValueItemAddX = new DataTable();
                                    dtValueItemAddX.Columns.Add("SKU_ID", typeof(int));
                                    dtValueItemAddX.Columns.Add("SECTION", typeof(string));
                                    dtValueItemAddX.Columns.Add("CustomerType", typeof(string));
                                    dtValueItemAddX.Columns.Add("tableName", typeof(string));
                                    dtValueItemAddX.Columns.Add("bookerName", typeof(string));
                                    dtValueItemAddX.Columns.Add("SKU_NAME", typeof(string));
                                    dtValueItemAddX.Columns.Add("ORDER_NOTES", typeof(string));
                                    dtValueItemAddX.Columns.Add("DealName", typeof(string));
                                    dtValueItemAddX.Columns.Add("SKU_HIE_NAME", typeof(string));
                                    dtValueItemAddX.Columns.Add("maxOrderNo", typeof(string));
                                    dtValueItemAddX.Columns.Add("QTY", typeof(string));
                                    dtValueItemAddX.Columns.Add("DealQTY", typeof(string));
                                    dtValueItemAddX.Columns.Add("Modifiers", typeof(string));
                                    dtValueItemAddX.Columns.Add("SALE_INVOICE_DETAIL_ID", typeof(long));
                                    dtValueItemAddX.Columns.Add("IsUngroup", typeof(bool));
                                    dtValueItemAddX.Columns.Add("TIME_STAMP2", typeof(string));
                                    dtValueItemAddX.Columns.Add("LastUpdateDateTime", typeof(string));
                                    dtValueItemAddX.Columns.Add("ModifierParetn_Row_ID", typeof(string));

                                    #endregion
                                    #region Populate Table for printing
                                    foreach (DataRow dr in dtValueCR.Rows)
                                    {
                                        //New KOT
                                        if (dr["MODIFIER_PARENT_ID"].ToString() == "0" && dr["VOID"].ToString().ToLower() == "false" && Convert.ToDecimal(dr["QTY"].ToString()) - Convert.ToDecimal(dr["PR_COUNT"].ToString()) > 0 && Seconds < 2)
                                        {
                                            DataRow row = dtValueNewOrderX.NewRow();
                                            row["SKU_ID"] = dr["SKU_ID"];
                                            row["SECTION"] = dr["SECTION"];
                                            row["CustomerType"] = dr["CustomerType"];
                                            row["tableName"] = dr["tableName"];
                                            row["bookerName"] = dr["bookerName"];
                                            row["SKU_NAME"] = dr["SKU_NAME"];
                                            row["ORDER_NOTES"] = dr["ORDER_NOTES"];
                                            row["DealName"] = dr["DealName"];
                                            row["SKU_HIE_NAME"] = dr["SKU_HIE_NAME"];
                                            row["maxOrderNo"] = dr["maxOrderNo"];
                                            row["QTY"] = dr["QTY"];
                                            row["DealQTY"] = dr["DealQTY"];
                                            string Modifiers = string.Empty;
                                            foreach (DataRow drModifierChild in dtValueCR.Rows)
                                            {
                                                if (drModifierChild["MODIFIER_PARENT_ID"].ToString() != "0")
                                                {
                                                    if (drModifierChild["MODIFIER_PARENT_ID"].ToString() == dr["SKU_ID"].ToString() && drModifierChild["ModifierParetn_Row_ID"].ToString() == dr["ModifierParetn_Row_ID"].ToString())
                                                    {
                                                        Modifiers += drModifierChild["SKU_NAME"].ToString();
                                                        Modifiers += "<br>";
                                                    }
                                                }
                                            }
                                            row["Modifiers"] = Modifiers;
                                            row["SALE_INVOICE_DETAIL_ID"] = dr["SALE_INVOICE_DETAIL_ID"];
                                            row["IsUngroup"] = dr["IsUngroup"];
                                            row["TIME_STAMP2"] = dr["TIME_STAMP2"];
                                            row["LastUpdateDateTime"] = dr["LastUpdateDateTime"];
                                            row["ModifierParetn_Row_ID"] = dr["ModifierParetn_Row_ID"];
                                            dtValueNewOrderX.Rows.Add(row);
                                        }
                                        //Void Item Or Item Deleted
                                        else if (dr["MODIFIER_PARENT_ID"].ToString() == "0" && dr["VOID"].ToString().ToLower() == "true" && Convert.ToDecimal(dr["QTY"].ToString()) - Convert.ToDecimal(dr["PR_COUNT"].ToString()) > 0 && ExcludeCancelKOT == "0")
                                        {
                                            DataRow row = dtValueVoidX.NewRow();
                                            row["SKU_ID"] = dr["SKU_ID"];
                                            row["SECTION"] = dr["SECTION"];
                                            row["CustomerType"] = dr["CustomerType"];
                                            row["tableName"] = dr["tableName"];
                                            row["bookerName"] = dr["bookerName"];
                                            row["SKU_NAME"] = dr["SKU_NAME"];
                                            row["ORDER_NOTES"] = dr["ORDER_NOTES"];
                                            row["DealName"] = dr["DealName"];
                                            row["SKU_HIE_NAME"] = dr["SKU_HIE_NAME"];
                                            row["maxOrderNo"] = dr["maxOrderNo"];
                                            row["QTY"] = dr["QTY"];
                                            row["DealQTY"] = dr["DealQTY"];
                                            string Modifiers = string.Empty;
                                            foreach (DataRow drModifierChild in dtValueCR.Rows)
                                            {
                                                if (drModifierChild["MODIFIER_PARENT_ID"].ToString() != "0")
                                                {
                                                    if (drModifierChild["MODIFIER_PARENT_ID"].ToString() == dr["SKU_ID"].ToString() && drModifierChild["ModifierParetn_Row_ID"].ToString() == dr["ModifierParetn_Row_ID"].ToString())
                                                    {
                                                        if (!SaleInvoiceDetailId.Contains(drModifierChild["SALE_INVOICE_DETAIL_ID"].ToString()))
                                                        {
                                                            SaleInvoiceDetailId.Add(drModifierChild["SALE_INVOICE_DETAIL_ID"].ToString());

                                                            Modifiers += drModifierChild["SKU_NAME"].ToString();
                                                            Modifiers += "<br>";
                                                        }
                                                    }
                                                }
                                            }
                                            if (Modifiers.Length > 0)
                                            {
                                                Modifiers = Modifiers.Substring(0, Modifiers.Length - 4);
                                            }
                                            row["Modifiers"] = Modifiers;
                                            row["SALE_INVOICE_DETAIL_ID"] = dr["SALE_INVOICE_DETAIL_ID"];
                                            row["IsUngroup"] = dr["IsUngroup"];
                                            row["TIME_STAMP2"] = dr["TIME_STAMP2"];
                                            row["LastUpdateDateTime"] = dr["LastUpdateDateTime"];
                                            row["ModifierParetn_Row_ID"] = dr["ModifierParetn_Row_ID"];
                                            dtValueVoidX.Rows.Add(row);
                                        }
                                        //Less Item Or Item Quantity decreased
                                        else if (dr["MODIFIER_PARENT_ID"].ToString() == "0" && dr["VOID"].ToString().ToLower() == "false" && Convert.ToDecimal(dr["QTY"].ToString()) - Convert.ToDecimal(dr["PR_COUNT"].ToString()) < 0 && dr["LessAddType"].ToString() == "1" && ExcludeCancelKOT == "0")
                                        {
                                            DataRow row = dtValueLessX.NewRow();
                                            row["SKU_ID"] = dr["SKU_ID"];
                                            row["SECTION"] = dr["SECTION"];
                                            row["CustomerType"] = dr["CustomerType"];
                                            row["tableName"] = dr["tableName"];
                                            row["bookerName"] = dr["bookerName"];
                                            row["SKU_NAME"] = dr["SKU_NAME"];
                                            row["ORDER_NOTES"] = dr["ORDER_NOTES"];
                                            row["DealName"] = dr["DealName"];
                                            row["SKU_HIE_NAME"] = dr["SKU_HIE_NAME"];
                                            row["maxOrderNo"] = dr["maxOrderNo"];
                                            row["QTY"] = dr["QTY"];
                                            row["DealQTY"] = dr["DealQTY"];
                                            string Modifiers = string.Empty;
                                            foreach (DataRow drModifierChild in dtValueCR.Rows)
                                            {
                                                if (drModifierChild["MODIFIER_PARENT_ID"].ToString() != "0")
                                                {
                                                    if (drModifierChild["MODIFIER_PARENT_ID"].ToString() == dr["SKU_ID"].ToString() && drModifierChild["ModifierParetn_Row_ID"].ToString() == dr["ModifierParetn_Row_ID"].ToString())
                                                    {
                                                        if (!SaleInvoiceDetailId.Contains(drModifierChild["SALE_INVOICE_DETAIL_ID"].ToString()))
                                                        {
                                                            SaleInvoiceDetailId.Add(drModifierChild["SALE_INVOICE_DETAIL_ID"].ToString());

                                                            Modifiers += drModifierChild["SKU_NAME"].ToString();
                                                            Modifiers += "<br>";
                                                        }
                                                    }
                                                }
                                            }
                                            if (Modifiers.Length > 0)
                                            {
                                                Modifiers = Modifiers.Substring(0, Modifiers.Length - 4);
                                            }
                                            row["Modifiers"] = Modifiers;
                                            row["SALE_INVOICE_DETAIL_ID"] = dr["SALE_INVOICE_DETAIL_ID"];
                                            row["IsUngroup"] = dr["IsUngroup"];
                                            row["TIME_STAMP2"] = dr["TIME_STAMP2"];
                                            row["LastUpdateDateTime"] = dr["LastUpdateDateTime"];
                                            row["ModifierParetn_Row_ID"] = dr["ModifierParetn_Row_ID"];
                                            dtValueLessX.Rows.Add(row);
                                        }
                                        //Add Item Quantity
                                        else if (dr["MODIFIER_PARENT_ID"].ToString() == "0" && dr["VOID"].ToString().ToLower() == "false" && Seconds > 2 && Convert.ToDecimal(dr["QtyAddLess"]) > 0 && dr["LessAddType"].ToString() == "2")
                                        {
                                            DataRow row = dtValueAddX.NewRow();
                                            row["SKU_ID"] = dr["SKU_ID"];
                                            row["SECTION"] = dr["SECTION"];
                                            row["CustomerType"] = dr["CustomerType"];
                                            row["tableName"] = dr["tableName"];
                                            row["bookerName"] = dr["bookerName"];
                                            row["SKU_NAME"] = dr["SKU_NAME"];
                                            row["ORDER_NOTES"] = dr["ORDER_NOTES"];
                                            row["DealName"] = dr["DealName"];
                                            row["SKU_HIE_NAME"] = dr["SKU_HIE_NAME"];
                                            row["maxOrderNo"] = dr["maxOrderNo"];
                                            row["QTY"] = dr["QTY"];
                                            row["DealQTY"] = dr["DealQTY"];
                                            string Modifiers = string.Empty;
                                            foreach (DataRow drModifierChild in dtValueCR.Rows)
                                            {
                                                if (drModifierChild["MODIFIER_PARENT_ID"].ToString() != "0")
                                                {
                                                    if (drModifierChild["MODIFIER_PARENT_ID"].ToString() == dr["SKU_ID"].ToString() && drModifierChild["ModifierParetn_Row_ID"].ToString() == dr["ModifierParetn_Row_ID"].ToString())
                                                    {
                                                        if (!SaleInvoiceDetailId.Contains(drModifierChild["SALE_INVOICE_DETAIL_ID"].ToString()))
                                                        {
                                                            SaleInvoiceDetailId.Add(drModifierChild["SALE_INVOICE_DETAIL_ID"].ToString());

                                                            Modifiers += drModifierChild["SKU_NAME"].ToString();
                                                            Modifiers += "<br>";
                                                        }
                                                    }
                                                }
                                            }
                                            if (Modifiers.Length > 0)
                                            {
                                                Modifiers = Modifiers.Substring(0, Modifiers.Length - 4);
                                            }
                                            row["Modifiers"] = Modifiers;
                                            row["SALE_INVOICE_DETAIL_ID"] = dr["SALE_INVOICE_DETAIL_ID"];
                                            row["IsUngroup"] = dr["IsUngroup"];
                                            row["TIME_STAMP2"] = dr["TIME_STAMP2"];
                                            row["LastUpdateDateTime"] = dr["LastUpdateDateTime"];
                                            row["ModifierParetn_Row_ID"] = dr["ModifierParetn_Row_ID"];
                                            dtValueAddX.Rows.Add(row);
                                        }
                                        //Add New Item
                                        else if (dr["MODIFIER_PARENT_ID"].ToString() == "0" && dr["VOID"].ToString().ToLower() == "false" && Convert.ToDecimal(dr["QTY"].ToString()) - Convert.ToDecimal(dr["PR_COUNT"].ToString()) > 0 && Seconds > 2 && Convert.ToDecimal(dr["QtyAddLess"]) == 0)
                                        {
                                            DataRow row = dtValueItemAddX.NewRow();
                                            row["SKU_ID"] = dr["SKU_ID"];
                                            row["SECTION"] = dr["SECTION"];
                                            row["CustomerType"] = dr["CustomerType"];
                                            row["tableName"] = dr["tableName"];
                                            row["bookerName"] = dr["bookerName"];
                                            row["SKU_NAME"] = dr["SKU_NAME"];
                                            row["ORDER_NOTES"] = dr["ORDER_NOTES"];
                                            row["DealName"] = dr["DealName"];
                                            row["SKU_HIE_NAME"] = dr["SKU_HIE_NAME"];
                                            row["maxOrderNo"] = dr["maxOrderNo"];
                                            row["QTY"] = dr["QTY"];
                                            row["DealQTY"] = dr["DealQTY"];
                                            string Modifiers = string.Empty;
                                            foreach (DataRow drModifierChild in dtValueCR.Rows)
                                            {
                                                if (drModifierChild["MODIFIER_PARENT_ID"].ToString() != "0")
                                                {
                                                    if (drModifierChild["MODIFIER_PARENT_ID"].ToString() == dr["SKU_ID"].ToString() && drModifierChild["ModifierParetn_Row_ID"].ToString() == dr["ModifierParetn_Row_ID"].ToString())
                                                    {
                                                        if (!SaleInvoiceDetailId.Contains(drModifierChild["SALE_INVOICE_DETAIL_ID"].ToString()))
                                                        {
                                                            SaleInvoiceDetailId.Add(drModifierChild["SALE_INVOICE_DETAIL_ID"].ToString());

                                                            Modifiers += drModifierChild["SKU_NAME"].ToString();
                                                            Modifiers += "<br>";
                                                        }
                                                    }
                                                }
                                            }
                                            if (Modifiers.Length > 0)
                                            {
                                                Modifiers = Modifiers.Substring(0, Modifiers.Length - 4);
                                            }
                                            row["Modifiers"] = Modifiers;
                                            row["SALE_INVOICE_DETAIL_ID"] = dr["SALE_INVOICE_DETAIL_ID"];
                                            row["IsUngroup"] = dr["IsUngroup"];
                                            row["TIME_STAMP2"] = dr["TIME_STAMP2"];
                                            row["LastUpdateDateTime"] = dr["LastUpdateDateTime"];
                                            row["ModifierParetn_Row_ID"] = dr["ModifierParetn_Row_ID"];
                                            dtValueItemAddX.Rows.Add(row);
                                        }
                                    }
                                    #endregion
                                    #region Print Tables Data
                                    if (dtValueNewOrderX.Rows.Count > 0)
                                    {
                                        if (!IsExceptionOccured)
                                        {
                                            WriteLog(string.Format("Order No-{0}-Xpeditor KOT Started Printing.", drMaster["SALE_INVOICE_ID"].ToString()));
                                            if (!IsExceptionOccured)
                                            {
                                                foreach (string printer in XpeditorPrinterName)
                                                {
                                                    WriteLog("dtValueNewOrderX No Of Rows:" + dtValueNewOrderX.Rows.Count.ToString());
                                                    if (!PrintKOTCrystalReport(dtValueNewOrderX, printer, drMaster["SALE_INVOICE_ID"].ToString(), 0, 0, 1, true, drMaster["REMARKS"].ToString(), drMaster["LastUpdateDateTime"].ToString(), "Xpeditor", "XNewKOT"))
                                                    {
                                                        timer.Start();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (dtValueVoidX.Rows.Count > 0)
                                    {
                                        if (!IsExceptionOccured)
                                        {
                                            WriteLog(string.Format("Order No-{0}-Xpeditor Cancel Item KOT Started Printing.", drMaster["SALE_INVOICE_ID"].ToString()));
                                            if (!IsExceptionOccured)
                                            {
                                                foreach (string printer in XpeditorPrinterName)
                                                {
                                                    WriteLog("dtValueVoidX No Of Rows:" + dtValueVoidX.Rows.Count.ToString());
                                                    if (!PrintKOTCrystalReport(dtValueVoidX, printer, drMaster["SALE_INVOICE_ID"].ToString(), 1, Seconds, 1, true, drMaster["REMARKS"].ToString(), drMaster["LastUpdateDateTime"].ToString(), "Xpeditor", "XCancelKOT"))
                                                    {
                                                        timer.Start();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (dtValueLessX.Rows.Count > 0)
                                    {
                                        if (!IsExceptionOccured)
                                        {
                                            WriteLog(string.Format("Order No-{0}-Xpeditor Item Qty Less KOT Started Printing.", drMaster["SALE_INVOICE_ID"].ToString()));
                                            if (!IsExceptionOccured)
                                            {
                                                foreach (string printer in XpeditorPrinterName)
                                                {
                                                    WriteLog("dtValueLessX No Of Rows:" + dtValueLessX.Rows.Count.ToString());
                                                    if (!PrintKOTCrystalReport(dtValueLessX, printer, drMaster["SALE_INVOICE_ID"].ToString(), 2, Seconds, 1, true, drMaster["REMARKS"].ToString(), drMaster["LastUpdateDateTime"].ToString(), "Xpeditor", "XLessKOT"))
                                                    {
                                                        timer.Start();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (dtValueAddX.Rows.Count > 0)
                                    {
                                        if (!IsExceptionOccured)
                                        {
                                            WriteLog(string.Format("Order No-{0}-Xpeditor Add Item Qty KOT Started Printing.", drMaster["SALE_INVOICE_ID"].ToString()));
                                            if (!IsExceptionOccured)
                                            {
                                                foreach (string printer in XpeditorPrinterName)
                                                {
                                                    WriteLog("dtValueAddX No Of Rows:" + dtValueAddX.Rows.Count.ToString());
                                                    if (!PrintKOTCrystalReport(dtValueAddX, printer, drMaster["SALE_INVOICE_ID"].ToString(), 5, Seconds, 1, true, drMaster["REMARKS"].ToString(), drMaster["LastUpdateDateTime"].ToString(), "Xpeditor", "XAddItemQtyKOT"))
                                                    {
                                                        timer.Start();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (dtValueItemAddX.Rows.Count > 0)
                                    {
                                        if (!IsExceptionOccured)
                                        {
                                            WriteLog(string.Format("Order No-{0}-Xpeditor Add New Item KOT Started Printing.", drMaster["SALE_INVOICE_ID"].ToString()));
                                            if (!IsExceptionOccured)
                                            {
                                                foreach (string printer in XpeditorPrinterName)
                                                {
                                                    WriteLog("dtValueItemAddX No Of Rows:" + dtValueItemAddX.Rows.Count.ToString());
                                                    if (!PrintKOTCrystalReport(dtValueItemAddX, printer, drMaster["SALE_INVOICE_ID"].ToString(), 6, Seconds, 1, true, drMaster["REMARKS"].ToString(), drMaster["LastUpdateDateTime"].ToString(), "Xpeditor", "XAddItemKOT"))
                                                    {
                                                        timer.Start();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                WriteLog(ex.ToString());
                                IsTimmerRunning = true;
                                IsExceptionOccured = true;
                                lastTickTime = System.DateTime.Now;
                                timer.Start();
                                break;
                            }
                            #endregion
                            #endregion                            
                        }
                    }
                    timer.Start();
                }
                #region Print Invoice
                try
                {
                    if (IsPrintInvoice == "1")
                    {
                        PrintInvoice();
                        PrintPayment();
                    }
                }
                catch (Exception ex)
                {
                    WriteLog(ex.ToString());
                    IsTimmerRunning = true;
                    IsExceptionOccured = true;
                    lastTickTime = System.DateTime.Now;
                }
                #endregion
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
                IsTimmerRunning = true;
                IsExceptionOccured = true;
                lastTickTime = System.DateTime.Now;
            }

        }
        private void PrintCrystalReportSectionWiseNew()
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

                                WriteLogNew(string.Format("Order No-{0}-" + KOTType + "-" + drSection["SECTION"].ToString() + " Started Printing.", dtOrderDetail.Rows[0]["SaleInvoiceID"].ToString()),string.Empty);
                                if (PrintKOTCrystalReportNew(dtOrderDetail, dtOrderDetail.Rows[0]["PrinterName"].ToString(), dtOrderDetail.Rows[0]["SaleInvoiceID"].ToString(), 1, false, dtOrderDetail.Rows[0]["OrderNotes"].ToString(), dtOrderDetail.Rows[0]["LastUpdateDateTime"].ToString(), drSection["SECTION"].ToString().Trim().ToUpper(), KOTType,Convert.ToByte(drKOTTyp["KOTType"]), IsXpeditor))
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
                                        WriteLog(string.Format("Order No-{0}-" + KOTType + "-Xpeditor" + " Started Printing.", dtOrderDetailX.Rows[0]["SaleInvoiceID"].ToString()));
                                        if (PrintKOTCrystalReportNew(dtOrderDetailX, printer, dtOrderDetailX.Rows[0]["SaleInvoiceID"].ToString(), 1, true, dtOrderDetailX.Rows[0]["OrderNotes"].ToString(), dtOrderDetailX.Rows[0]["LastUpdateDateTime"].ToString(), "Xpeditor", "X" + KOTType,Convert.ToByte(drKOTTyp["KOTType"]), IsXpeditor))
                                        {

                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            WriteLog(ex.ToString());
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLogNew(ex.ToString(), "PrintCrystalReportSectionWiseNew");
            }
        }
        private void PrintCrystalReportNonSection()
        {
            try
            {
                DataTable dtOrders = GetOfersForPrintCrystalReport(1, 0, 0);
                DataTable dtValueCR = new DataTable();
                if (dtOrders.Rows.Count > 0)
                {
                    foreach (DataRow drMaster in dtOrders.Rows)
                    {
                        List<string> SaleInvoiceDetailId = new List<string>();
                        ProductSectionRequest request = new ProductSectionRequest();
                        ProductSectionResponse response = GetProductSectionsWithPrinters(request);
                        SectionPrinterList = response.ProductSectionList;

                        try
                        {
                            dtValueCR = GetOfersForPrintCrystalReport(2, Convert.ToInt64(drMaster["SALE_INVOICE_ID"]), Convert.ToInt64(drMaster["CUSTOMER_ID"]));
                        }
                        catch (Exception ex)
                        {
                            WriteLog(ex.ToString());
                            IsTimmerRunning = true;
                            IsExceptionOccured = true;
                            lastTickTime = System.DateTime.Now;
                            break;
                        }
                        if (dtValueCR.Rows.Count > 0)
                        {
                            customerType = dtValueCR.Rows[0]["CustomerType"].ToString();
                            bookerName = dtValueCR.Rows[0]["bookerName"].ToString();
                            tableName = dtValueCR.Rows[0]["tableName"].ToString();
                            maxOrderNo = dtValueCR.Rows[0]["maxOrderNo"].ToString();
                            OrderNotes = dtValueCR.Rows[0]["OrderNotes"].ToString();

                            DataView view = new DataView(dtValueCR);

                            foreach (string PrinterName in PrinterNames)
                            {
                                int Seconds = Convert.ToInt32(drMaster["Seconds"]);
                                DataTable dtValueNewOrder = new DataTable();
                                dtValueNewOrder.Columns.Add("SECTION", typeof(string));
                                dtValueNewOrder.Columns.Add("CustomerType", typeof(string));
                                dtValueNewOrder.Columns.Add("tableName", typeof(string));
                                dtValueNewOrder.Columns.Add("bookerName", typeof(string));
                                dtValueNewOrder.Columns.Add("SKU_NAME", typeof(string));
                                dtValueNewOrder.Columns.Add("ORDER_NOTES", typeof(string));
                                dtValueNewOrder.Columns.Add("DealName", typeof(string));
                                dtValueNewOrder.Columns.Add("maxOrderNo", typeof(string));
                                dtValueNewOrder.Columns.Add("QTY", typeof(string));
                                dtValueNewOrder.Columns.Add("DealQTY", typeof(string));
                                dtValueNewOrder.Columns.Add("Modifiers", typeof(string));

                                DataTable dtValueStickerPrint = new DataTable();
                                dtValueStickerPrint.Columns.Add("tableName", typeof(string));
                                dtValueStickerPrint.Columns.Add("SKU_NAME", typeof(string));
                                dtValueStickerPrint.Columns.Add("ORDER_NOTES", typeof(string));
                                dtValueStickerPrint.Columns.Add("maxOrderNo", typeof(string));
                                dtValueStickerPrint.Columns.Add("QTY", typeof(string));
                                dtValueStickerPrint.Columns.Add("Modifiers", typeof(string));
                                dtValueStickerPrint.Columns.Add("CUSTOMER_NAME", typeof(string));
                                dtValueStickerPrint.Columns.Add("ContactNo", typeof(string));

                                DataTable dtValueVoid = new DataTable();
                                dtValueVoid.Columns.Add("SECTION", typeof(string));
                                dtValueVoid.Columns.Add("CustomerType", typeof(string));
                                dtValueVoid.Columns.Add("tableName", typeof(string));
                                dtValueVoid.Columns.Add("bookerName", typeof(string));
                                dtValueVoid.Columns.Add("SKU_NAME", typeof(string));
                                dtValueVoid.Columns.Add("ORDER_NOTES", typeof(string));
                                dtValueVoid.Columns.Add("DealName", typeof(string));
                                dtValueVoid.Columns.Add("maxOrderNo", typeof(string));
                                dtValueVoid.Columns.Add("QTY", typeof(string));
                                dtValueVoid.Columns.Add("DealQTY", typeof(string));
                                dtValueVoid.Columns.Add("Modifiers", typeof(string));

                                DataTable dtValueLess = new DataTable();
                                dtValueLess.Columns.Add("SECTION", typeof(string));
                                dtValueLess.Columns.Add("CustomerType", typeof(string));
                                dtValueLess.Columns.Add("tableName", typeof(string));
                                dtValueLess.Columns.Add("bookerName", typeof(string));
                                dtValueLess.Columns.Add("SKU_NAME", typeof(string));
                                dtValueLess.Columns.Add("ORDER_NOTES", typeof(string));
                                dtValueLess.Columns.Add("DealName", typeof(string));
                                dtValueLess.Columns.Add("maxOrderNo", typeof(string));
                                dtValueLess.Columns.Add("QTY", typeof(string));
                                dtValueLess.Columns.Add("DealQTY", typeof(string));
                                dtValueLess.Columns.Add("Modifiers", typeof(string));

                                DataTable dtValueAdd = new DataTable();
                                dtValueAdd.Columns.Add("SECTION", typeof(string));
                                dtValueAdd.Columns.Add("CustomerType", typeof(string));
                                dtValueAdd.Columns.Add("tableName", typeof(string));
                                dtValueAdd.Columns.Add("bookerName", typeof(string));
                                dtValueAdd.Columns.Add("SKU_NAME", typeof(string));
                                dtValueAdd.Columns.Add("ORDER_NOTES", typeof(string));
                                dtValueAdd.Columns.Add("DealName", typeof(string));
                                dtValueAdd.Columns.Add("maxOrderNo", typeof(string));
                                dtValueAdd.Columns.Add("QTY", typeof(string));
                                dtValueAdd.Columns.Add("DealQTY", typeof(string));
                                dtValueAdd.Columns.Add("Modifiers", typeof(string));

                                DataTable dtValueItemAdd = new DataTable();
                                dtValueItemAdd.Columns.Add("SECTION", typeof(string));
                                dtValueItemAdd.Columns.Add("CustomerType", typeof(string));
                                dtValueItemAdd.Columns.Add("tableName", typeof(string));
                                dtValueItemAdd.Columns.Add("bookerName", typeof(string));
                                dtValueItemAdd.Columns.Add("SKU_NAME", typeof(string));
                                dtValueItemAdd.Columns.Add("ORDER_NOTES", typeof(string));
                                dtValueItemAdd.Columns.Add("DealName", typeof(string));
                                dtValueItemAdd.Columns.Add("maxOrderNo", typeof(string));
                                dtValueItemAdd.Columns.Add("QTY", typeof(string));
                                dtValueItemAdd.Columns.Add("DealQTY", typeof(string));
                                dtValueItemAdd.Columns.Add("Modifiers", typeof(string));
                                try
                                {
                                    foreach (DataRow dr in dtValueCR.Rows)
                                    {
                                        //Regular KOT
                                        if (dr["IsStickerPrint"].ToString().ToLower() == "false")
                                        {
                                            //New Item
                                            if (dr["MODIFIER_PARENT_ID"].ToString() == "0" && dr["VOID"].ToString().ToLower() == "false" && Convert.ToDecimal(dr["QTY"].ToString()) - Convert.ToDecimal(dr["PR_COUNT"].ToString()) > 0 && Seconds < 2)
                                            {
                                                DataRow row = dtValueNewOrder.NewRow();
                                                row["SECTION"] = PrinterName;
                                                row["CustomerType"] = dr["CustomerType"];
                                                row["tableName"] = dr["tableName"];
                                                row["bookerName"] = dr["bookerName"];
                                                row["SKU_NAME"] = dr["SKU_NAME"];
                                                row["ORDER_NOTES"] = dr["ORDER_NOTES"];
                                                row["DealName"] = dr["DealName"];
                                                row["maxOrderNo"] = dr["maxOrderNo"];
                                                row["QTY"] = dr["QTY"];
                                                row["DealQTY"] = dr["DealQTY"];
                                                string Modifiers = string.Empty;
                                                foreach (DataRow drModifierChild in dtValueCR.Rows)
                                                {
                                                    if (drModifierChild["MODIFIER_PARENT_ID"].ToString() != "0")
                                                    {
                                                        if (drModifierChild["MODIFIER_PARENT_ID"].ToString() == dr["SKU_ID"].ToString() && drModifierChild["ModifierParetn_Row_ID"].ToString() == dr["ModifierParetn_Row_ID"].ToString())
                                                        {
                                                            if (!SaleInvoiceDetailId.Contains(drModifierChild["SALE_INVOICE_DETAIL_ID"].ToString()))
                                                            {
                                                                SaleInvoiceDetailId.Add(drModifierChild["SALE_INVOICE_DETAIL_ID"].ToString());

                                                                Modifiers += drModifierChild["SKU_NAME"].ToString();
                                                                Modifiers += "<br>";
                                                            }
                                                        }
                                                    }
                                                }
                                                if (Modifiers.Length > 0)
                                                {
                                                    Modifiers = Modifiers.Substring(0, Modifiers.Length - 4);
                                                }
                                                row["Modifiers"] = Modifiers;
                                                dtValueNewOrder.Rows.Add(row);
                                            }
                                            //Void Item Or Item Deleted
                                            else if (dr["MODIFIER_PARENT_ID"].ToString() == "0" && dr["VOID"].ToString().ToLower() == "true" && Convert.ToDecimal(dr["QTY"].ToString()) - Convert.ToDecimal(dr["PR_COUNT"].ToString()) > 0)
                                            {
                                                DataRow row = dtValueVoid.NewRow();
                                                row["SECTION"] = PrinterName;
                                                row["CustomerType"] = dr["CustomerType"];
                                                row["tableName"] = dr["tableName"];
                                                row["bookerName"] = dr["bookerName"];
                                                row["SKU_NAME"] = dr["SKU_NAME"];
                                                row["ORDER_NOTES"] = dr["ORDER_NOTES"];
                                                row["DealName"] = dr["DealName"];
                                                row["maxOrderNo"] = dr["maxOrderNo"];
                                                row["QTY"] = dr["QTY"];
                                                row["DealQTY"] = dr["DealQTY"];
                                                string Modifiers = string.Empty;
                                                foreach (DataRow drModifierChild in dtValueCR.Rows)
                                                {
                                                    if (drModifierChild["MODIFIER_PARENT_ID"].ToString() != "0")
                                                    {
                                                        if (drModifierChild["MODIFIER_PARENT_ID"].ToString() == dr["SKU_ID"].ToString() && drModifierChild["ModifierParetn_Row_ID"].ToString() == dr["ModifierParetn_Row_ID"].ToString())
                                                        {
                                                            if (!SaleInvoiceDetailId.Contains(drModifierChild["SALE_INVOICE_DETAIL_ID"].ToString()))
                                                            {
                                                                SaleInvoiceDetailId.Add(drModifierChild["SALE_INVOICE_DETAIL_ID"].ToString());

                                                                Modifiers += drModifierChild["SKU_NAME"].ToString();
                                                                Modifiers += "<br>";
                                                            }
                                                        }
                                                    }
                                                }
                                                if (Modifiers.Length > 0)
                                                {
                                                    Modifiers = Modifiers.Substring(0, Modifiers.Length - 4);
                                                }
                                                row["Modifiers"] = Modifiers;
                                                dtValueVoid.Rows.Add(row);
                                            }
                                            //Less Item Or Item Quantity decreased
                                            else if (dr["MODIFIER_PARENT_ID"].ToString() == "0" && dr["VOID"].ToString().ToLower() == "false" && Convert.ToDecimal(dr["QTY"].ToString()) - Convert.ToDecimal(dr["PR_COUNT"].ToString()) < 0 && dr["LessAddType"].ToString() == "1")
                                            {
                                                DataRow row = dtValueLess.NewRow();
                                                row["SECTION"] = PrinterName;
                                                row["CustomerType"] = dr["CustomerType"];
                                                row["tableName"] = dr["tableName"];
                                                row["bookerName"] = dr["bookerName"];
                                                row["SKU_NAME"] = dr["SKU_NAME"];
                                                row["ORDER_NOTES"] = dr["ORDER_NOTES"];
                                                row["DealName"] = dr["DealName"];
                                                row["maxOrderNo"] = dr["maxOrderNo"];
                                                row["QTY"] = dr["QTY"];
                                                row["DealQTY"] = dr["DealQTY"];
                                                string Modifiers = string.Empty;
                                                foreach (DataRow drModifierChild in dtValueCR.Rows)
                                                {
                                                    if (drModifierChild["MODIFIER_PARENT_ID"].ToString() != "0")
                                                    {
                                                        if (drModifierChild["MODIFIER_PARENT_ID"].ToString() == dr["SKU_ID"].ToString() && drModifierChild["ModifierParetn_Row_ID"].ToString() == dr["ModifierParetn_Row_ID"].ToString())
                                                        {
                                                            if (!SaleInvoiceDetailId.Contains(drModifierChild["SALE_INVOICE_DETAIL_ID"].ToString()))
                                                            {
                                                                SaleInvoiceDetailId.Add(drModifierChild["SALE_INVOICE_DETAIL_ID"].ToString());

                                                                Modifiers += drModifierChild["SKU_NAME"].ToString();
                                                                Modifiers += "<br>";
                                                            }
                                                        }
                                                    }
                                                }
                                                if (Modifiers.Length > 0)
                                                {
                                                    Modifiers = Modifiers.Substring(0, Modifiers.Length - 4);
                                                }
                                                row["Modifiers"] = Modifiers;
                                                dtValueLess.Rows.Add(row);
                                            }
                                            //Add Item Quantity
                                            else if (dr["MODIFIER_PARENT_ID"].ToString() == "0" && dr["VOID"].ToString().ToLower() == "false" && Seconds > 2 && Convert.ToDecimal(dr["QtyAddLess"]) > 0 && dr["LessAddType"].ToString() == "2")
                                            {
                                                DataRow row = dtValueAdd.NewRow();
                                                row["SECTION"] = PrinterName;
                                                row["CustomerType"] = dr["CustomerType"];
                                                row["tableName"] = dr["tableName"];
                                                row["bookerName"] = dr["bookerName"];
                                                row["SKU_NAME"] = dr["SKU_NAME"];
                                                row["ORDER_NOTES"] = dr["ORDER_NOTES"];
                                                row["DealName"] = dr["DealName"];
                                                row["maxOrderNo"] = dr["maxOrderNo"];
                                                row["QTY"] = dr["QTY"];
                                                row["DealQTY"] = dr["DealQTY"];
                                                string Modifiers = string.Empty;
                                                foreach (DataRow drModifierChild in dtValueCR.Rows)
                                                {
                                                    if (drModifierChild["MODIFIER_PARENT_ID"].ToString() != "0")
                                                    {
                                                        if (drModifierChild["MODIFIER_PARENT_ID"].ToString() == dr["SKU_ID"].ToString() && drModifierChild["ModifierParetn_Row_ID"].ToString() == dr["ModifierParetn_Row_ID"].ToString())
                                                        {
                                                            if (!SaleInvoiceDetailId.Contains(drModifierChild["SALE_INVOICE_DETAIL_ID"].ToString()))
                                                            {
                                                                SaleInvoiceDetailId.Add(drModifierChild["SALE_INVOICE_DETAIL_ID"].ToString());

                                                                Modifiers += drModifierChild["SKU_NAME"].ToString();
                                                                Modifiers += "<br>";
                                                            }
                                                        }
                                                    }
                                                }
                                                if (Modifiers.Length > 0)
                                                {
                                                    Modifiers = Modifiers.Substring(0, Modifiers.Length - 4);
                                                }
                                                row["Modifiers"] = Modifiers;
                                                dtValueAdd.Rows.Add(row);
                                            }
                                            //Add New Item 
                                            else if (dr["MODIFIER_PARENT_ID"].ToString() == "0" && dr["VOID"].ToString().ToLower() == "false" && Convert.ToDecimal(dr["QTY"].ToString()) - Convert.ToDecimal(dr["PR_COUNT"].ToString()) > 0 && Seconds > 2 && Convert.ToDecimal(dr["QtyAddLess"]) == 0)
                                            {
                                                DataRow row = dtValueItemAdd.NewRow();
                                                row["SECTION"] = PrinterName;
                                                row["CustomerType"] = dr["CustomerType"];
                                                row["tableName"] = dr["tableName"];
                                                row["bookerName"] = dr["bookerName"];
                                                row["SKU_NAME"] = dr["SKU_NAME"];
                                                row["ORDER_NOTES"] = dr["ORDER_NOTES"];
                                                row["DealName"] = dr["DealName"];
                                                row["maxOrderNo"] = dr["maxOrderNo"];
                                                row["QTY"] = dr["QTY"];
                                                row["DealQTY"] = dr["DealQTY"];
                                                string Modifiers = string.Empty;
                                                foreach (DataRow drModifierChild in dtValueCR.Rows)
                                                {
                                                    if (drModifierChild["MODIFIER_PARENT_ID"].ToString() != "0")
                                                    {
                                                        if (drModifierChild["MODIFIER_PARENT_ID"].ToString() == dr["SKU_ID"].ToString() && drModifierChild["ModifierParetn_Row_ID"].ToString() == dr["ModifierParetn_Row_ID"].ToString())
                                                        {
                                                            if (!SaleInvoiceDetailId.Contains(drModifierChild["SALE_INVOICE_DETAIL_ID"].ToString()))
                                                            {
                                                                SaleInvoiceDetailId.Add(drModifierChild["SALE_INVOICE_DETAIL_ID"].ToString());

                                                                Modifiers += drModifierChild["SKU_NAME"].ToString();
                                                                Modifiers += "<br>";
                                                            }
                                                        }
                                                    }
                                                }
                                                if (Modifiers.Length > 0)
                                                {
                                                    Modifiers = Modifiers.Substring(0, Modifiers.Length - 4);
                                                }
                                                row["Modifiers"] = Modifiers;
                                                dtValueItemAdd.Rows.Add(row);
                                            }
                                        }
                                        //KOT Sticker Print
                                        else if (dr["IsStickerPrint"].ToString().ToLower() == "true")
                                        {
                                            //New Item
                                            if (dr["MODIFIER_PARENT_ID"].ToString() == "0" && dr["VOID"].ToString().ToLower() == "false" && Convert.ToDecimal(dr["QTY"].ToString()) - Convert.ToDecimal(dr["PR_COUNT"].ToString()) > 0)
                                            {
                                                DataRow row = dtValueStickerPrint.NewRow();
                                                row["tableName"] = dr["tableName"];
                                                row["SKU_NAME"] = dr["SKU_NAME"];
                                                row["ORDER_NOTES"] = dr["ORDER_NOTES"];
                                                row["maxOrderNo"] = dr["maxOrderNo"];
                                                row["QTY"] = dr["QTY"];
                                                row["CUSTOMER_NAME"] = dr["CUSTOMER_NAME"];
                                                row["ContactNo"] = dr["ContactNo"];
                                                string Modifiers = string.Empty;
                                                foreach (DataRow drModifierChild in dtValueCR.Rows)
                                                {
                                                    if (drModifierChild["MODIFIER_PARENT_ID"].ToString() != "0")
                                                    {
                                                        if (drModifierChild["MODIFIER_PARENT_ID"].ToString() == dr["SKU_ID"].ToString() && drModifierChild["ModifierParetn_Row_ID"].ToString() == dr["ModifierParetn_Row_ID"].ToString())
                                                        {
                                                            if (!SaleInvoiceDetailId.Contains(drModifierChild["SALE_INVOICE_DETAIL_ID"].ToString()))
                                                            {
                                                                SaleInvoiceDetailId.Add(drModifierChild["SALE_INVOICE_DETAIL_ID"].ToString());

                                                                Modifiers += drModifierChild["SKU_NAME"].ToString();
                                                                Modifiers += "<br>";
                                                            }
                                                        }
                                                    }
                                                }
                                                if (Modifiers.Length > 0)
                                                {
                                                    Modifiers = Modifiers.Substring(0, Modifiers.Length - 4);
                                                }
                                                row["Modifiers"] = Modifiers;
                                                dtValueStickerPrint.Rows.Add(row);
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    WriteLog(ex.ToString());
                                    IsTimmerRunning = true;
                                    IsExceptionOccured = true;
                                    lastTickTime = System.DateTime.Now;
                                    break;
                                }
                                try
                                {
                                    if (dtValueNewOrder.Rows.Count > 0)
                                    {
                                        int NofOfCopies = 1;
                                        if (!IsExceptionOccured)
                                        {
                                            WriteLog(string.Format("Order No-{0}-KOT Started Printing.", drMaster["SALE_INVOICE_ID"].ToString()));
                                            if (!IsKOTPrinted(drMaster["SALE_INVOICE_ID"].ToString(), PrinterName, "NewKOT", Convert.ToDateTime(drMaster["LASTUPDATE_DATE"]), DOCUMENT_DATE))
                                            {
                                                if (WriteLogPrintedKOT(drMaster["SALE_INVOICE_ID"].ToString(), PrinterName, "NewKOT", Convert.ToDateTime(drMaster["LASTUPDATE_DATE"]), DOCUMENT_DATE))
                                                {
                                                    if (!IsExceptionOccured)
                                                    {
                                                        PrintKOTCrystalReport(dtValueNewOrder, PrinterName, drMaster["SALE_INVOICE_ID"].ToString(), 0, Seconds, NofOfCopies, false, drMaster["REMARKS"].ToString(), drMaster["LastUpdateDateTime"].ToString(), PrinterName, "NewKOT");
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (dtValueVoid.Rows.Count > 0)
                                    {

                                        int NofOfCopies = 1;
                                        if (!IsExceptionOccured)
                                        {
                                            WriteLog(string.Format("Order No-{0}-KOT Cancel Item Started Printing.", drMaster["SALE_INVOICE_ID"].ToString()));
                                            if (!IsKOTPrinted(drMaster["SALE_INVOICE_ID"].ToString(), PrinterName, "CancelItem", Convert.ToDateTime(drMaster["LASTUPDATE_DATE"]), DOCUMENT_DATE))
                                            {
                                                if (WriteLogPrintedKOT(drMaster["SALE_INVOICE_ID"].ToString(), PrinterName, "CancelItem", Convert.ToDateTime(drMaster["LASTUPDATE_DATE"]), DOCUMENT_DATE))
                                                {
                                                    if (!IsExceptionOccured)
                                                    {
                                                        PrintKOTCrystalReport(dtValueVoid, PrinterName, drMaster["SALE_INVOICE_ID"].ToString(), 1, Seconds, NofOfCopies, false, drMaster["REMARKS"].ToString(), drMaster["LastUpdateDateTime"].ToString(), PrinterName, "CancelKOT");
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (dtValueLess.Rows.Count > 0)
                                    {
                                        int NofOfCopies = 1;
                                        if (!IsExceptionOccured)
                                        {
                                            WriteLog(string.Format("Order No-{0}-KOT Less Item Qty Started Printing.", drMaster["SALE_INVOICE_ID"].ToString()));
                                            if (!IsKOTPrinted(drMaster["SALE_INVOICE_ID"].ToString(), PrinterName, "LessQty", Convert.ToDateTime(drMaster["LASTUPDATE_DATE"]), DOCUMENT_DATE))
                                            {
                                                if (WriteLogPrintedKOT(drMaster["SALE_INVOICE_ID"].ToString(), PrinterName, "LessQty", Convert.ToDateTime(drMaster["LASTUPDATE_DATE"]), DOCUMENT_DATE))
                                                {
                                                    if (!IsExceptionOccured)
                                                    {
                                                        PrintKOTCrystalReport(dtValueLess, PrinterName, drMaster["SALE_INVOICE_ID"].ToString(), 2, Seconds, NofOfCopies, false, drMaster["REMARKS"].ToString(), drMaster["LastUpdateDateTime"].ToString(), PrinterName, "LessKOT");
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (dtValueAdd.Rows.Count > 0)
                                    {
                                        int NofOfCopies = 1;
                                        if (!IsExceptionOccured)
                                        {
                                            WriteLog(string.Format("Order No-{0}-KOT Add Item Qty Started Printing.", drMaster["SALE_INVOICE_ID"].ToString()));
                                            if (!IsKOTPrinted(drMaster["SALE_INVOICE_ID"].ToString(), PrinterName, "AddQty", Convert.ToDateTime(drMaster["LASTUPDATE_DATE"]), DOCUMENT_DATE))
                                            {
                                                if (WriteLogPrintedKOT(drMaster["SALE_INVOICE_ID"].ToString(), PrinterName, "AddQty", Convert.ToDateTime(drMaster["LASTUPDATE_DATE"]), DOCUMENT_DATE))
                                                {
                                                    if (!IsExceptionOccured)
                                                    {
                                                        PrintKOTCrystalReport(dtValueAdd, PrinterName, drMaster["SALE_INVOICE_ID"].ToString(), 5, Seconds, NofOfCopies, false, drMaster["REMARKS"].ToString(), drMaster["LastUpdateDateTime"].ToString(), PrinterName, "AddItemQtyKOT");
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (dtValueItemAdd.Rows.Count > 0)
                                    {
                                        int NofOfCopies = 1;
                                        if (!IsExceptionOccured)
                                        {
                                            WriteLog(string.Format("Order No-{0}-KOT Add New Item Started Printing.", drMaster["SALE_INVOICE_ID"].ToString()));
                                            if (!IsKOTPrinted(drMaster["SALE_INVOICE_ID"].ToString(), PrinterName, "AddNewItem", Convert.ToDateTime(drMaster["LASTUPDATE_DATE"]), DOCUMENT_DATE))
                                            {
                                                if (WriteLogPrintedKOT(drMaster["SALE_INVOICE_ID"].ToString(), PrinterName, "AddNewItem", Convert.ToDateTime(drMaster["LASTUPDATE_DATE"]), DOCUMENT_DATE))
                                                {
                                                    if (!IsExceptionOccured)
                                                    {
                                                        PrintKOTCrystalReport(dtValueItemAdd, PrinterName, drMaster["SALE_INVOICE_ID"].ToString(), 6, Seconds, NofOfCopies, false, drMaster["REMARKS"].ToString(), drMaster["LastUpdateDateTime"].ToString(), PrinterName, "AddItemKOT");
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    WriteLog(ex.ToString());
                                    IsTimmerRunning = true;
                                    IsExceptionOccured = true;
                                    lastTickTime = System.DateTime.Now;
                                    break;
                                }
                                try
                                {
                                    if (dtValueStickerPrint.Rows.Count > 0)
                                    {
                                        foreach (DataRow drStickerPrint in dtValueStickerPrint.Rows)
                                        {
                                            if ((int)Math.Floor(Convert.ToDecimal(drStickerPrint["QTY"])) > 1)
                                            {
                                                for (int i = 0; i < (int)Math.Floor(Convert.ToDecimal(drStickerPrint["QTY"])); i++)
                                                {
                                                    DataTable dtStickerPrintSingleQty = new DataTable();
                                                    dtStickerPrintSingleQty.Columns.Add("SKU_NAME", typeof(string));
                                                    dtStickerPrintSingleQty.Columns.Add("ORDER_NOTES", typeof(string));
                                                    dtStickerPrintSingleQty.Columns.Add("maxOrderNo", typeof(string));
                                                    dtStickerPrintSingleQty.Columns.Add("Modifiers", typeof(string));
                                                    dtStickerPrintSingleQty.Columns.Add("CUSTOMER_NAME", typeof(string));
                                                    dtStickerPrintSingleQty.Columns.Add("ContactNo", typeof(string));

                                                    DataRow drSticker = dtStickerPrintSingleQty.NewRow();
                                                    drSticker["SKU_NAME"] = drStickerPrint["SKU_NAME"];
                                                    drSticker["ORDER_NOTES"] = drStickerPrint["ORDER_NOTES"];
                                                    drSticker["maxOrderNo"] = drStickerPrint["maxOrderNo"];
                                                    drSticker["Modifiers"] = drStickerPrint["Modifiers"];
                                                    drSticker["CUSTOMER_NAME"] = drStickerPrint["CUSTOMER_NAME"];
                                                    drSticker["ContactNo"] = drStickerPrint["ContactNo"];
                                                    dtStickerPrintSingleQty.Rows.Add(drSticker);
                                                    WriteLog(string.Format("Order No-{0}, Item-{1} Sticker Started Printing.", drMaster["SALE_INVOICE_ID"].ToString(), dtStickerPrintSingleQty.Rows[0]["SKU_NAME"].ToString()));
                                                    PrintStickerCrystalReport(dtStickerPrintSingleQty, System.Configuration.ConfigurationManager.AppSettings["StickerPrinterName"].ToString(), drMaster["SALE_INVOICE_ID"].ToString(), 1, Seconds, 1);
                                                }
                                            }
                                            else
                                            {
                                                DataTable dtStickerPrint = new DataTable();
                                                dtStickerPrint.Columns.Add("SKU_NAME", typeof(string));
                                                dtStickerPrint.Columns.Add("ORDER_NOTES", typeof(string));
                                                dtStickerPrint.Columns.Add("maxOrderNo", typeof(string));
                                                dtStickerPrint.Columns.Add("Modifiers", typeof(string));
                                                dtStickerPrint.Columns.Add("CUSTOMER_NAME", typeof(string));
                                                dtStickerPrint.Columns.Add("ContactNo", typeof(string));

                                                DataRow drSticker = dtStickerPrint.NewRow();
                                                drSticker["SKU_NAME"] = drStickerPrint["SKU_NAME"];
                                                drSticker["ORDER_NOTES"] = drStickerPrint["ORDER_NOTES"];
                                                drSticker["maxOrderNo"] = drStickerPrint["maxOrderNo"];
                                                drSticker["Modifiers"] = drStickerPrint["Modifiers"];
                                                drSticker["CUSTOMER_NAME"] = drStickerPrint["CUSTOMER_NAME"];
                                                drStickerPrint["ContactNo"] = drStickerPrint["ContactNo"];
                                                dtStickerPrint.Rows.Add(drSticker);
                                                WriteLog(string.Format("Order No-{0}, Item-{1} Sticker Started Printing.", drMaster["SALE_INVOICE_ID"].ToString(), dtStickerPrint.Rows[0]["SKU_NAME"].ToString()));
                                                PrintStickerCrystalReport(dtStickerPrint, System.Configuration.ConfigurationManager.AppSettings["StickerPrinterName"].ToString(), drMaster["SALE_INVOICE_ID"].ToString(), 1, Seconds, 1);
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    WriteLog(ex.ToString());
                                    IsTimmerRunning = true;
                                    IsExceptionOccured = true;
                                    lastTickTime = System.DateTime.Now;
                                    break;
                                }
                            }

                            try
                            {
                                foreach (DataRow drDetail in dtValueCR.Rows)
                                {
                                    if (!UpdateSaleDetailPrintQty(Convert.ToInt64(drMaster["SALE_INVOICE_ID"]), Convert.ToInt32(drDetail["SKU_ID"]), Convert.ToDecimal(drDetail["QTY"])))
                                    {
                                        WriteLog("SKUID: " + drDetail["SKU_ID"].ToString() + " not Updated.");
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                WriteLog(ex.ToString());
                                IsTimmerRunning = true;
                                IsExceptionOccured = true;
                                lastTickTime = System.DateTime.Now;
                                break;
                            }
                        }
                    }
                }

                try
                {
                    if (IsPrintInvoice == "1")
                    {
                        PrintInvoice();
                        PrintPayment();
                    }
                }
                catch (Exception ex)
                {
                    WriteLog(ex.ToString());
                    IsTimmerRunning = true;
                    IsExceptionOccured = true;
                    lastTickTime = System.DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
                IsTimmerRunning = true;
                IsExceptionOccured = true;
                lastTickTime = System.DateTime.Now;
            }

        }
        private void PrintCrystalReportNonSectionNew()
        {
            try
            {
                DataTable dtOrders = GetOfersForPrintCrystalReport(1, 0, 0);
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

                                WriteLogNew(string.Format("Order No-{0}-" + KOTType + "-" + PrinterName + " Started Printing.", dtOrderDetail.Rows[0]["SaleInvoiceID"].ToString()), string.Empty);
                                if (PrintKOTCrystalReportNew(dtOrderDetail, PrinterName, dtOrderDetail.Rows[0]["SaleInvoiceID"].ToString(), 1, false, dtOrderDetail.Rows[0]["OrderNotes"].ToString(), dtOrderDetail.Rows[0]["LastUpdateDateTime"].ToString(), string.Empty, KOTType, Convert.ToByte(drKOTTyp["KOTType"]), IsXpeditor))
                                {

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLogNew(ex.ToString(), "PrintCrystalReportSectionWiseNew");
            }
        }
        private void PrintInvoice()
        {
            DataTable dtParams = new DataTable();
            dtParams.Columns.Add("TIME_STAMP", typeof(DateTime));
            dtParams.Columns.Add("MOP", typeof(string));
            dtParams.Columns.Add("OrderNo", typeof(long));
            dtParams.Columns.Add("Cashier", typeof(string));
            dtParams.Columns.Add("Covers", typeof(string));
            dtParams.Columns.Add("TableNo", typeof(string));
            dtParams.Columns.Add("DM_OT", typeof(string));
            dtParams.Columns.Add("ServiceType", typeof(string));
            dtParams.Columns.Add("SALE_INVOICE_ID", typeof(long));
            dtParams.Columns.Add("DISCOUNT", typeof(decimal));
            dtParams.Columns.Add("DISCOUNT_TYPE", typeof(int));
            dtParams.Columns.Add("GST", typeof(decimal));
            dtParams.Columns.Add("SERVICE_CHARGES", typeof(decimal));
            dtParams.Columns.Add("SERVICE_CHARGES_TYPE", typeof(int));
            dtParams.Columns.Add("PAYMENT_MODE_ID", typeof(int));
            dtParams.Columns.Add("Address", typeof(string));
            dtParams.Columns.Add("ContactNo", typeof(string));
            dtParams.Columns.Add("NTN", typeof(string));
            dtParams.Columns.Add("TaxAuthorirty", typeof(string));
            dtParams.Columns.Add("GSTCalculation", typeof(string));
            dtParams.Columns.Add("GSTPer", typeof(decimal));
            dtParams.Columns.Add("CreditCardGSTPer", typeof(decimal));

            DataTable dtPrintOrders = GetPrintOrders(1, 0);
            dtValueInvoice = null;
            if (dtPrintOrders.Rows.Count > 0)
            {
                foreach (DataRow drMaster in dtPrintOrders.Rows)
                {
                    DataRow drParam = dtParams.NewRow();
                    drParam["TIME_STAMP"] = drMaster["TIME_STAMP"];
                    drParam["MOP"] = drMaster["MOP"];
                    drParam["OrderNo"] = drMaster["OrderNo"];
                    drParam["Cashier"] = drMaster["Cashier"];
                    drParam["Covers"] = drMaster["Covers"];
                    drParam["TableNo"] = drMaster["TableNo"];
                    drParam["DM_OT"] = drMaster["DM_OT"];
                    drParam["ServiceType"] = drMaster["ServiceType"];
                    drParam["SALE_INVOICE_ID"] = drMaster["SALE_INVOICE_ID"];
                    drParam["DISCOUNT"] = drMaster["DISCOUNT"];
                    drParam["DISCOUNT_TYPE"] = drMaster["DISCOUNT_TYPE"];
                    drParam["GST"] = drMaster["GST"];
                    drParam["SERVICE_CHARGES"] = drMaster["SERVICE_CHARGES"];
                    drParam["SERVICE_CHARGES_TYPE"] = drMaster["SERVICE_CHARGES_TYPE"];
                    drParam["PAYMENT_MODE_ID"] = drMaster["PAYMENT_MODE_ID"];
                    drParam["Address"] = Address;
                    drParam["ContactNo"] = ContactNo;
                    drParam["NTN"] = NTN;
                    drParam["TaxAuthorirty"] = TaxAuthorirty;
                    drParam["GSTCalculation"] = GSTCalculation;
                    drParam["GSTPer"] = GSTPer;
                    drParam["CreditCardGSTPer"] = CreditCardGSTPer;
                    dtParams.Rows.Add(drParam);
                    dtValueInvoice = GetPrintOrders(2, Convert.ToInt64(drMaster["SALE_INVOICE_ID"]));
                    if (dtValueInvoice.Rows.Count > 0)
                    {
                        WriteLog(string.Format("Invoice-{0}-Started Printing.", drMaster["SALE_INVOICE_ID"]));
                        PrintInvoiceCrystalReport(dtValueInvoice, dtParams, drMaster["PRINTER_NAME"].ToString(), drMaster["SALE_INVOICE_ID"].ToString(), 1, 1);

                        //Update Invoice Print
                        SetPrintOrder(3, Convert.ToInt64(drMaster["SALE_INVOICE_ID"]));
                    }
                }
            }
        }
        private void PrintPayment()
        {
            DataTable dtParams = new DataTable();
            dtParams.Columns.Add("TIME_STAMP", typeof(DateTime));
            dtParams.Columns.Add("MOP", typeof(string));
            dtParams.Columns.Add("OrderNo", typeof(long));
            dtParams.Columns.Add("Cashier", typeof(string));
            dtParams.Columns.Add("Covers", typeof(string));
            dtParams.Columns.Add("TableNo", typeof(string));
            dtParams.Columns.Add("DM_OT", typeof(string));
            dtParams.Columns.Add("ServiceType", typeof(string));
            dtParams.Columns.Add("SALE_INVOICE_ID", typeof(long));
            dtParams.Columns.Add("DISCOUNT", typeof(decimal));
            dtParams.Columns.Add("DISCOUNT_TYPE", typeof(int));
            dtParams.Columns.Add("GST", typeof(decimal));
            dtParams.Columns.Add("SERVICE_CHARGES", typeof(decimal));
            dtParams.Columns.Add("SERVICE_CHARGES_TYPE", typeof(int));
            dtParams.Columns.Add("PAYMENT_MODE_ID", typeof(int));
            dtParams.Columns.Add("Address", typeof(string));
            dtParams.Columns.Add("ContactNo", typeof(string));
            dtParams.Columns.Add("NTN", typeof(string));
            dtParams.Columns.Add("TaxAuthorirty", typeof(string));
            dtParams.Columns.Add("GSTCalculation", typeof(string));
            dtParams.Columns.Add("GSTPer", typeof(decimal));
            dtParams.Columns.Add("CreditCardGSTPer", typeof(decimal));

            DataTable dtPrintOrders = GetPrintOrders(5, 0);
            dtValueInvoice = null;
            if (dtPrintOrders.Rows.Count > 0)
            {
                foreach (DataRow drMaster in dtPrintOrders.Rows)
                {
                    DataRow drParam = dtParams.NewRow();
                    drParam["TIME_STAMP"] = drMaster["TIME_STAMP"];
                    drParam["MOP"] = drMaster["MOP"];
                    drParam["OrderNo"] = drMaster["OrderNo"];
                    drParam["Cashier"] = drMaster["Cashier"];
                    drParam["Covers"] = drMaster["Covers"];
                    drParam["TableNo"] = drMaster["TableNo"];
                    drParam["DM_OT"] = drMaster["DM_OT"];
                    drParam["ServiceType"] = drMaster["ServiceType"];
                    drParam["SALE_INVOICE_ID"] = drMaster["SALE_INVOICE_ID"];
                    drParam["DISCOUNT"] = drMaster["DISCOUNT"];
                    drParam["DISCOUNT_TYPE"] = drMaster["DISCOUNT_TYPE"];
                    drParam["GST"] = drMaster["GST"];
                    drParam["SERVICE_CHARGES"] = drMaster["SERVICE_CHARGES"];
                    drParam["SERVICE_CHARGES_TYPE"] = drMaster["SERVICE_CHARGES_TYPE"];
                    drParam["PAYMENT_MODE_ID"] = drMaster["PAYMENT_MODE_ID"];
                    drParam["Address"] = Address;
                    drParam["ContactNo"] = ContactNo;
                    drParam["NTN"] = NTN;
                    drParam["TaxAuthorirty"] = TaxAuthorirty;
                    drParam["GSTCalculation"] = GSTCalculation;
                    drParam["GSTPer"] = GSTPer;
                    drParam["CreditCardGSTPer"] = CreditCardGSTPer;
                    drParam["PAIDIN"] = drMaster["PAIDIN"];
                    dtParams.Rows.Add(drParam);
                    dtValueInvoice = GetPrintOrders(2, Convert.ToInt64(drMaster["SALE_INVOICE_ID"]));
                    if (dtValueInvoice.Rows.Count > 0)
                    {
                        WriteLog(string.Format("Invoice-{0}-Started Printing.", drMaster["SALE_INVOICE_ID"]));
                        PrintInvoiceCrystalReport(dtValueInvoice, dtParams, drMaster["PRINTER_NAME"].ToString(), drMaster["SALE_INVOICE_ID"].ToString(), 1, 2);

                        //Update Invoice Print
                        SetPrintOrder(6, Convert.ToInt64(drMaster["SALE_INVOICE_ID"]));
                    }
                }
            }
        }
        #endregion

        #region Log Files
        private static void WriteLog(string Msg)
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
        }
        private static void WriteLogNew(string Msg,string FunctionName)
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
                    WriteLog(ex.ToString());
                }
            }
        }

        private static bool WriteLogPrintedKOT(string InvoiceNO, string Section, string Remarks, DateTime LastUpdate, DateTime DOCUMENT_DATE)
        {
            string logFile = path + "KOTDetail" + DOCUMENT_DATE.ToString("yyyyMMdd") + ".txt";
            bool flag = false;

            for (int i = 0; i < 3; i++)
            {
                try
                {
                    using (FileStream fs = new FileStream(logFile, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.WriteLine($"{DateTime.Now}|{InvoiceNO}|{Section}|{Remarks}|{LastUpdate}");
                        sw.Flush();
                        fs.Flush(true);
                    }
                    flag = true;
                    break;
                }
                catch (IOException)
                {
                    // File might be locked temporarily, retry after 100ms
                    System.Threading.Thread.Sleep(100);
                }
                catch (Exception ex)
                {
                    IsExceptionOccured = true;
                    WriteLog("WriteLogPrintedKOT failed: " + ex.ToString());
                    break;
                }
            }

            return flag;
        }
        public bool IsKOTPrinted(string InvoiceID, string Section, string Remarks, DateTime LastUpdate, DateTime DOCUMENT_DATE)
        {
            bool flag = false;
            string pathInvoice = System.IO.Path.GetFullPath(path + "KOTDetail" + DOCUMENT_DATE.ToString("yyyyMMdd") + ".txt");
            if (System.IO.File.Exists(pathInvoice))
            {
                FileStream Sourcefile = null;
                StreamReader ReadSourceFile = null;
                try
                {
                    Sourcefile = new FileStream(pathInvoice, FileMode.Open);
                    ReadSourceFile = new StreamReader(Sourcefile);
                    string FileContents = "";

                    while ((FileContents = ReadSourceFile.ReadLine()) != null)
                    {
                        string[] ParametersArr = FileContents.Split('|');
                        TimeSpan timeDifference = DateTime.Now - Convert.ToDateTime(ParametersArr[0]);
                        double milliseconds = timeDifference.TotalMilliseconds;
                        if (InvoiceID == ParametersArr[1].Trim() && milliseconds < 2000 && ParametersArr[2].ToString() == Section && ParametersArr[3].ToString() == Remarks && ParametersArr[4].ToString() == LastUpdate.ToString())
                        {
                            flag = true;
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    WriteLog(ex.ToString());
                    IsExceptionOccured = true;
                    ReadSourceFile.Close();
                    Sourcefile.Close();
                }
                finally
                {
                    ReadSourceFile.Close();
                    Sourcefile.Close();
                }
            }
            return flag;
        }

        private static bool WriteLogKOTPrinted(string SaleInvoiceID, string Section, string LastUpdateDateTime, string KOTType, DateTime DOCUMENT_DATE)
        {
            string logFile = path + "KOTPrinted" + DOCUMENT_DATE.ToString("yyyyMMdd") + ".txt";
            bool flag = false;

            for (int i = 0; i < 3; i++)
            {
                try
                {
                    using (FileStream fs = new FileStream(logFile, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.WriteLine($"{DateTime.Now}|{SaleInvoiceID}|{Section}|{LastUpdateDateTime}|{KOTType}");
                        sw.Flush();
                        fs.Flush(true);
                    }
                    flag = true;
                    break;
                }
                catch (IOException)
                {
                    // File might be in use, wait and retry
                    System.Threading.Thread.Sleep(100);
                }
                catch (Exception ex)
                {
                    IsExceptionOccured = true;
                    WriteLogNew("WriteLogKOTPrinted failed: " + ex.ToString(), "WriteLogKOTPrinted");
                    break;
                }
            }

            return flag;
        }
        private static void DeleteKOTPrinted(string SaleInvoiceID, string Section, string LastUpdateDateTime, string KOTType, DateTime DOCUMENT_DATE)
        {
            string logFile = path + "KOTPrinted" + DOCUMENT_DATE.ToString("yyyyMMdd") + ".txt";
            if (!File.Exists(logFile))
                return;

            try
            {
                List<string> lines = new List<string>();

                // Read file safely with shared access
                using (FileStream fs = new FileStream(logFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (StreamReader reader = new StreamReader(fs))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        var parts = line.Split('|');
                        bool match = (parts.Length >= 5 &&
                                      parts[1].Trim() == SaleInvoiceID.Trim() &&
                                      parts[2].Trim() == Section.Trim() &&
                                      parts[3].Trim() == LastUpdateDateTime.Trim() &&
                                      parts[4].Trim() == KOTType.Trim());

                        if (!match)
                            lines.Add(line); // keep only non-matching lines
                    }
                }

                // Safely overwrite file
                using (FileStream fs = new FileStream(logFile, FileMode.Create, FileAccess.Write, FileShare.Read))
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    foreach (var line in lines)
                        writer.WriteLine(line);
                    writer.Flush();
                    fs.Flush(true); // ensure data is flushed to disk
                }
            }
            catch (Exception ex)
            {
                IsExceptionOccured = true;
                WriteLog("DeleteKOTPrinted failed: " + ex);
            }
        }
        public bool IsKOTPrinted(string SaleInvoiceID, string Section, string LastUpdateDateTime, string KOTType)
        {
            bool flag = false;
            string pathInvoice = System.IO.Path.GetFullPath(path + "KOTPrinted" + DOCUMENT_DATE.ToString("yyyyMMdd") + ".txt");

            if (System.IO.File.Exists(pathInvoice))
            {
                try
                {
                    using (FileStream sourceFile = new FileStream(pathInvoice, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    using (StreamReader reader = new StreamReader(sourceFile))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            string[] parts = line.Split('|');
                            if (parts.Length >= 5 &&
                                parts[1].Trim() == SaleInvoiceID.Trim() &&
                                parts[2].Trim() == Section.Trim() &&
                                parts[3].Trim() == LastUpdateDateTime.Trim() &&
                                parts[4].Trim() == KOTType.Trim())
                            {
                                flag = true;
                                break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    WriteLogNew("IsKOTPrinted failed: " + ex.ToString(), "IsKOTPrinted");
                    IsExceptionOccured = true;
                }
            }

            return flag;
        }

        private static bool WriteLogUnGroupItem(string SaleInvoiceID, string TimeStamp, string KOTType, DateTime DOCUMENT_DATE)
        {
            string logFile = path + "UngroupItemLogFile" + DOCUMENT_DATE.ToString("yyyyMMdd") + ".txt";
            bool flag = false;

            for (int i = 0; i < 3; i++)
            {
                try
                {
                    using (FileStream fs = new FileStream(logFile, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.WriteLine($"{DateTime.Now}|{SaleInvoiceID}|{TimeStamp}|{KOTType}");
                        sw.Flush();
                        fs.Flush(true);
                    }
                    flag = true;
                    break;
                }
                catch (IOException)
                {
                    // File in use, wait a little and retry
                    System.Threading.Thread.Sleep(100);
                }
                catch (Exception ex)
                {
                    IsExceptionOccured = true;
                    WriteLogNew("WriteLogUnGroupItem failed: " + ex.ToString(), "WriteLogUnGroupItem");
                    break;
                }
            }

            return flag;
        }
        public bool IsUnGroupItemPrinted(string SaleInvoiceID, string TimeStamp, string KOTType)
        {
            bool flag = false;
            string pathInvoice = System.IO.Path.GetFullPath(path + "UngroupItemLogFile" + DOCUMENT_DATE.ToString("yyyyMMdd") + ".txt");

            if (System.IO.File.Exists(pathInvoice))
            {
                try
                {
                    using (FileStream sourceFile = new FileStream(pathInvoice, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    using (StreamReader reader = new StreamReader(sourceFile))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            string[] parts = line.Split('|');
                            if (parts.Length >= 4 &&
                                parts[1].Trim() == SaleInvoiceID.Trim() &&
                                parts[2].Trim() == TimeStamp.Trim() &&
                                parts[3].Trim() == KOTType.Trim())
                            {
                                flag = true;
                                break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    WriteLogNew("IsUnGroupItemPrinted failed: " + ex.ToString(), "IsUnGroupItemPrinted");
                    IsExceptionOccured = true;
                }
            }

            return flag;
        }

        private static bool WriteItemPrint(string SaleInvoiceID, string ItemID, string LastUpdateDateTime, string ModifierParetn_Row_ID, string KOTType, DateTime DOCUMENT_DATE)
        {
            string logFile = path + "PrintedItemLogFile" + DOCUMENT_DATE.ToString("yyyyMMdd") + ".txt";
            bool flag = false;

            for (int i = 0; i < 3; i++)
            {
                try
                {
                    using (FileStream fs = new FileStream(logFile, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.WriteLine($"{DateTime.Now}|{SaleInvoiceID}|{ItemID}|{LastUpdateDateTime}|{ModifierParetn_Row_ID}|{KOTType}");
                        sw.Flush();
                        fs.Flush(true);
                    }
                    flag = true;
                    break;
                }
                catch (IOException)
                {
                    // File in use, wait a little and retry
                    System.Threading.Thread.Sleep(50);
                }
                catch (Exception ex)
                {
                    IsExceptionOccured = true;
                    WriteLogNew("WriteLogUnGroupItem failed: " + ex.ToString(), "WriteItemPrint");
                    break;
                }
            }

            return flag;
        }
        private static void DeleteItemPrinted(string SaleInvoiceID, string ItemID, string LastUpdateDateTime, string ModifierParetn_Row_ID, string KOTType, DateTime DOCUMENT_DATE)
        {
            string logFile = path + "PrintedItemLogFile" + DOCUMENT_DATE.ToString("yyyyMMdd") + ".txt";
            if (!File.Exists(logFile))
                return;

            try
            {
                List<string> lines = new List<string>();

                // Read file safely with shared access
                using (FileStream fs = new FileStream(logFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (StreamReader reader = new StreamReader(fs))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        var parts = line.Split('|');
                        bool match = (parts.Length >= 6 &&
                                      parts[1].Trim() == SaleInvoiceID.Trim() &&
                                      parts[2].Trim() == ItemID.Trim() &&
                                      parts[3].Trim() == LastUpdateDateTime.Trim() &&
                                      parts[4].Trim() == ModifierParetn_Row_ID.Trim() &&
                                      parts[5].Trim() == KOTType.Trim());

                        if (!match)
                            lines.Add(line); // keep only non-matching lines
                    }
                }

                // Safely overwrite file
                using (FileStream fs = new FileStream(logFile, FileMode.Create, FileAccess.Write, FileShare.Read))
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    foreach (var line in lines)
                        writer.WriteLine(line);
                    writer.Flush();
                    fs.Flush(true);
                }
            }
            catch (Exception ex)
            {
                IsExceptionOccured = true;
                WriteLog("DeleteItemPrinted failed: " + ex);
            }
        }
        public bool IsItemPrinted(string SaleInvoiceID, string ItemID, string LastUpdateDateTime, string ModifierParetn_Row_ID, string KOTType)
        {
            bool flag = false;
            string pathInvoice = System.IO.Path.GetFullPath(path + "PrintedItemLogFile" + DOCUMENT_DATE.ToString("yyyyMMdd") + ".txt");

            if (System.IO.File.Exists(pathInvoice))
            {
                try
                {
                    using (FileStream sourceFile = new FileStream(pathInvoice, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    using (StreamReader reader = new StreamReader(sourceFile))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            string[] parts = line.Split('|');
                            if (parts.Length >= 6 &&
                                parts[1].Trim() == SaleInvoiceID.Trim() &&
                                parts[2].Trim() == ItemID.Trim() &&
                                parts[3].Trim() == LastUpdateDateTime.Trim() &&
                                parts[4].Trim() == ModifierParetn_Row_ID.Trim() &&
                                parts[5].Trim() == KOTType.Trim())
                            {
                                flag = true;
                                break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    WriteLogNew("IsItemPrinted failed: " + ex.ToString(), "IsItemPrinted");
                    IsExceptionOccured = true;
                }
            }

            return flag;
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
                WriteLog(ex.ToString());
            }
            return Encoding.UTF8.GetString(stream.ToArray());
        }
        #endregion

        #region Get Data
        public DataTable GetOfersForPrint(int TypeID, long SaleInvoiceID)
        {
            DataTable dtOrders = new DataTable();

            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("uspGetOfersForPrintWS", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 120;

                        cmd.Parameters.Add(new SqlParameter("@DISTRIBUTOR_ID", SqlDbType.Int)
                        {
                            Value = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["DistributorID"])
                        });

                        cmd.Parameters.Add(new SqlParameter("@TYPE_ID", SqlDbType.Int)
                        {
                            Value = TypeID
                        });

                        cmd.Parameters.Add(new SqlParameter("@SALE_INVOICE_ID", SqlDbType.BigInt)
                        {
                            Value = SaleInvoiceID
                        });

                        // ✅ Use SqlDataReader instead of DataAdapter
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            dtOrders.Load(reader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
            }

            return dtOrders;
        }

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
                        IDataParameter parameter = new SqlParameter() { ParameterName = "@DISTRIBUTOR_ID", DbType = DbType.Int32, Value = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["DistributorID"]) };
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
                WriteLogNew(ex.ToString(), "GetOfersForPrintCrystalReport(int TypeID, long SaleInvoiceID, long CustomerID)");
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
                        IDataParameter parameter = new SqlParameter() { ParameterName = "@DISTRIBUTOR_ID", DbType = DbType.Int32, Value = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["DistributorID"]) };
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
                WriteLogNew(ex.ToString(), "GetOfersForPrintCrystalReport(int TypeID, long SaleInvoiceID, long CustomerID,int KOTType,string Section)");
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
                        IDataParameter parameter = new SqlParameter() { ParameterName = "@DISTRIBUTOR_ID", DbType = DbType.Int32, Value = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["DistributorID"]) };
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
                WriteLog(ex.ToString());
            }
            return dtOrders;
        }
        public ProductSectionResponse GetProductSectionsWithPrinters(ProductSectionRequest request)
        {
            ProductSectionResponse response = new ProductSectionResponse();
            response.ProductSectionList = new List<ProductSection>();

            IDbConnection mConnection = null;
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandTimeout = 120;
                        cmd.CommandText = "dbo.CAT_GetProductSections";
                        cmd.CommandType = CommandType.StoredProcedure;
                        IDbDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        if (ds.Tables.Count > 0)
                        {
                            response.ProductSectionList = ds.Tables[0].ToCollection<ProductSection>();
                        }
                        response.IsException = false;

                        return response;
                    }
                    con.Close();
                }


            }
            catch (Exception exp)
            {
                WriteLog(exp.ToString());
                response.IsException = true;
                response.Message = exp.Message;
                return response;
            }
            finally
            {
                if (mConnection != null && mConnection.State == ConnectionState.Open)
                {
                    mConnection.Close();
                }
            }
        }
        #endregion

        #region Update Data
        public bool SetPrintOrder(int TypeID, long SaleInvoiceID)
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
                        IDataParameter parameter = new SqlParameter() { ParameterName = "@DISTRIBUTOR_ID", DbType = DbType.Int32, Value = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["DistributorID"]) };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@TYPE_ID", DbType = DbType.Int32, Value = TypeID };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@SALE_INVOICE_ID", DbType = DbType.Int64, Value = SaleInvoiceID };
                        pparams.Add(parameter);

                        cmd.ExecuteNonQuery();
                    }
                    con.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
                return false;
            }
        }
        public bool UpdateSaleDetailPrintQty(long SaleInvoiceID, int SKUID, decimal Qty)
        {
            bool flag = true;
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandTimeout = 120;
                        cmd.CommandText = "uspGetUpdateSaleDetailPrintQtyWS";
                        cmd.CommandType = CommandType.StoredProcedure;

                        IDataParameterCollection pparams = cmd.Parameters;
                        IDataParameter parameter = new SqlParameter() { ParameterName = "@QTY", DbType = DbType.Decimal, Value = Qty };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@SKU_ID", DbType = DbType.Int32, Value = SKUID };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@SALE_INVOICE_ID", DbType = DbType.Int64, Value = SaleInvoiceID };
                        pparams.Add(parameter);

                        cmd.ExecuteNonQuery();
                        flag = true;
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
                flag = false;
            }
            return flag;
        }
        public bool UpdateSaleDetailPrintQty(long SaleInvoiceID, int SKUID, decimal Qty, long SaleInvoiceDetailID)
        {
            bool flag = true;
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandTimeout = 120;
                        cmd.CommandText = "uspGetUpdateSaleDetailPrintQtyWS2";
                        cmd.CommandType = CommandType.StoredProcedure;

                        IDataParameterCollection pparams = cmd.Parameters;
                        IDataParameter parameter = new SqlParameter() { ParameterName = "@QTY", DbType = DbType.Decimal, Value = Qty };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@SKU_ID", DbType = DbType.Int32, Value = SKUID };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@SALE_INVOICE_ID", DbType = DbType.Int64, Value = SaleInvoiceID };
                        pparams.Add(parameter);

                        parameter = new SqlParameter() { ParameterName = "@SALE_INVOICE_DETAIL_ID", DbType = DbType.Int64, Value = SaleInvoiceDetailID };
                        pparams.Add(parameter);

                        cmd.ExecuteNonQuery();
                        flag = true;
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
                flag = false;
            }
            return flag;
        }
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
                        WriteLogNew($"Transient SQL error on attempt {attempt}: {ex.Message}", "UpdatePrintedKOT");
                        if (attempt < maxRetries)
                            System.Threading.Thread.Sleep(delayMilliseconds * attempt);
                        else
                            flag = false;
                    }
                    else
                    {
                        WriteLogNew(ex.ToString(), "UpdatePrintedKOT");
                        flag = false;
                        break;
                    }
                }
                catch (Exception ex)
                {
                    WriteLogNew(ex.ToString(), "UpdatePrintedKOT");
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
                                Value = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["DistributorID"])
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
                        WriteLog($"Transient SQL error on attempt {attempt}: {ex.Message}");
                        if (attempt < maxRetries)
                           System.Threading.Thread.Sleep(delayMilliseconds * attempt); // exponential backoff (2s, 4s, 6s)
                        else
                            flag = false;
                    }
                    else
                    {
                        WriteLog(ex.ToString());
                        flag = false;
                        break;
                    }
                }
                catch (Exception ex)
                {
                    WriteLog(ex.ToString());
                    flag = false;
                    break;
                }
            }

            return flag;
        }

        #endregion

        #region Print-KOT

        private static void PrintReport(bool IsFullKOT, bool IsXpeditor)
        {
            try
            {
                if (IsFullKOT)
                {
                    PrintDocument prnDocument = new PrintDocument();
                    prnDocument.PrinterSettings.PrinterName = (System.Configuration.ConfigurationManager.AppSettings["PrinterName"]);
                    if (SectionPrinterList != null)
                    {
                        List<ProductSection> ProductSectionList = (List<ProductSection>)SectionPrinterList;
                        var sectionPrinter = ProductSectionList.Find(p => p.IS_FULL_KOT == true);
                        if (sectionPrinter != null)
                        {
                            string SinglePrinter = System.Configuration.ConfigurationManager.AppSettings["SinglePrinter"].ToString();
                            if (SinglePrinter == "1")
                            {
                                prnDocument.PrinterSettings.PrinterName = System.Configuration.ConfigurationManager.AppSettings["PrinterName"].ToString();
                            }
                            else
                            {
                                prnDocument.PrinterSettings.PrinterName = sectionPrinter.PrinterName;
                            }
                        }
                    }
                    prnDocument.PrintPage += new PrintPageEventHandler(prnDocument_PrintPage);
                    prnDocument.Print();
                }
                else if (IsXpeditor)
                {
                    PrintDocument prnDocument = new PrintDocument();
                    prnDocument.PrinterSettings.PrinterName = XpeditorPrinterName[0].ToString();
                    prnDocument.PrintPage += new PrintPageEventHandler(prnDocument_PrintPage);
                    prnDocument.Print();
                }
                else
                {
                    PrintDocument prnDocument = new PrintDocument();
                    prnDocument.PrinterSettings.PrinterName = (System.Configuration.ConfigurationManager.AppSettings["PrinterName"]);
                    if (SectionPrinterList != null && _section != null)
                    {
                        List<ProductSection> ProductSectionList = (List<ProductSection>)SectionPrinterList;
                        string SectionName = _section.Trim().ToLower();
                        var sectionPrinter = ProductSectionList.Find(p => p.SectionName.ToLower().Trim() == SectionName);
                        if (sectionPrinter != null)
                            prnDocument.PrinterSettings.PrinterName = sectionPrinter.PrinterName;
                    }
                    prnDocument.PrintPage += new PrintPageEventHandler(prnDocument_PrintPage);
                    prnDocument.Print();
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
            }
        }

        private static void prnDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            leftMargin = (int)e.MarginBounds.Left;//100
            rightMargin = (int)e.MarginBounds.Right;//215
            topMargin = (int)e.MarginBounds.Top;//100
            topMargin = 1;
            bottomMargin = (int)e.MarginBounds.Bottom;//1069
            InvoiceWidth = (int)e.MarginBounds.Width;//115
            InvoiceHeight = (int)e.MarginBounds.Height;//969

            SetInvoiceHead(e.Graphics); // Draw Invoice Head
            SetInvoiceData(e.Graphics, e); // Draw Invoice Data                   
        }

        private static void SetInvoiceHead(Graphics g)
        {
            // for Invoice Head:
            string InvSubTitle3 = string.Empty;
            string InvSubTitle4 = string.Empty;
            CurrentY = topMargin;
            CurrentX = leftMargin;

            InvTitleHeight = (int)(InvTitleFont.GetHeight(g));
            InvSubTitleHeight = (int)(InvSubTitleFont.GetHeight(g));

            // Get Titles Length:

            int lenInvSubTitle3 = (int)g.MeasureString(InvSubTitle3, InvSubTitleFont).Width;
            int lenInvSubTitle4 = (int)g.MeasureString(InvSubTitle4, InvSubTitleFont).Width;
            // Set Titles Left:

            int xInvSubTitle3 = CurrentX + (InvoiceWidth - lenInvSubTitle3) / 2;
            int xInvSubTitle4 = CurrentX + (InvoiceWidth - lenInvSubTitle4) / 2;

            InvSubTitle3 = "SECTION: " + _section;

            if (InvSubTitle3 != "")
            {
                CurrentY = CurrentY + InvSubTitleHeight + 10;
                g.DrawString(InvSubTitle3, InvSubTitleFont, BlueBrush, 10, CurrentY);
            }
            CurrentY = CurrentY + 25;
            g.DrawString("SERVICE TYPE: " + customerType, InvoiceFont, BlueBrush, 10, CurrentY);

            if (IsLocationName == "1")
            {
                CurrentY = CurrentY + 20;
                g.DrawString("Location Name: " + System.Configuration.ConfigurationManager.AppSettings["LocationName"].ToString(), InvoiceFont, BlueBrush, 10, CurrentY);
            }

            if (customerType.ToLower() == "Delivery".ToLower())
            {
                CurrentY = CurrentY + 20;
                g.DrawString("D-M: " + bookerName, InvoiceFont, BlueBrush, 10, CurrentY);

                CurrentY = CurrentY + 20;
                g.DrawString("Customer: " + tableName, InvoiceFont, BlueBrush, 10, CurrentY);
            }
            else if (customerType.ToLower() == "Dine In".ToLower())
            {
                CurrentY = CurrentY + 20;
                g.DrawString("O-T: " + bookerName, InvoiceFont, BlueBrush, 10, CurrentY);

                CurrentY = CurrentY + 20;
                g.DrawString("TABLE NO: " + tableName, InvoiceFont, BlueBrush, 10, CurrentY);
            }
            else
            {
                CurrentY = CurrentY + 20;
                g.DrawString("O-T: " + bookerName, InvoiceFont, BlueBrush, 10, CurrentY);

                CurrentY = CurrentY + 20;
                g.DrawString("Customer: " + tableName, InvoiceFont, BlueBrush, 10, CurrentY);
            }

            CurrentY = CurrentY + 20;
            g.DrawString("Date: " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm tt"), InvoiceFont2, BlueBrush, 10, CurrentY);
            g.DrawString(maxOrderNo, InvOrderTitleFont, BlueBrush, 230, CurrentY - 20);

            Utiltiy.DrawCircle(g, new Pen(Brushes.Black, 2), 250, CurrentY - 10, 27);

            int XNoOfuntit = (int)g.MeasureString("", InvoiceFont).Width + 180;
            int YNoOfUnit = CurrentY;

            CurrentY = CurrentY + 25;
            g.DrawLine(new Pen(Brushes.Black, 2), 10, CurrentY, 300, CurrentY);//            
            CurrentY = CurrentY + 5;
        }

        private static void SetInvoiceData(Graphics g, PrintPageEventArgs e)
        {
            // Set Invoice Table:
            string FieldValue = "";
            int CurrentRecord = 0;

            // Set Table Head:
            int xProductID = 10;//leftMargin;

            CurrentY = CurrentY + 4;//InvoiceFontHeight;
            g.DrawString("Item Name", InvoiceFont, BlueBrush, xProductID, CurrentY);

            int xProductName = xProductID + (int)g.MeasureString("Item Name", InvoiceFont).Width + 150;//162 Edit by safdar om 20160806//70 Edit by Hazrat Ali on 04-June-2021
            g.DrawString("Qty", InvoiceFont, BlueBrush, xProductName, CurrentY);

            CurrentY = CurrentY + 20;
            g.DrawLine(new Pen(Brushes.Black, 2), 10, CurrentY, 300, CurrentY);
            CurrentY = CurrentY + 10;
            DataView dv = dtValue.DefaultView;
            dv.Sort = "C1";
            dtValue = dv.ToTable();

            if (dtValue.Rows.Count > 0)
            {
                foreach (DataRow dr in dtValue.Rows)
                {
                    if (dr["MODIFIER_PARENT_ID"].ToString() == "0")
                    {
                        if (dr["SECTION"].ToString() == _section || System.Configuration.ConfigurationManager.AppSettings["SinglePrinter"].ToString() == "1")
                        {
                            decimal qty = Convert.ToDecimal(dr["QTY"].ToString()) - Convert.ToDecimal(dr["PR_COUNT"].ToString());

                            string IS_VOID = dr["VOID"].ToString();

                            if (IS_VOID.ToLower() == "false")
                            {
                                if (qty > 0)
                                {
                                    FieldValue = Convert.ToString(dr["SKU_NAME"]);// name
                                    if (FieldValue.Length > 24)
                                    {
                                        string FieldValue2 = FieldValue.Substring(0, 25);
                                        int Yposition = 40;
                                        g.DrawString(FieldValue2, InvoiceFont2, BlackBrush, xProductID, CurrentY);

                                        FieldValue = FieldValue.Substring(25);
                                        g.DrawString(FieldValue, InvoiceFont2, BlackBrush, xProductID, CurrentY + 20);
                                        //Show Modifiers as Indent
                                        FieldValue = string.Empty;
                                        foreach (DataRow drModifierChild in dtValue.Rows)
                                        {
                                            if (drModifierChild["MODIFIER_PARENT_ID"].ToString() != "0")
                                            {
                                                if (drModifierChild["MODIFIER_PARENT_ID"].ToString() == dr["SKU_ID"].ToString() && drModifierChild["ModifierParetn_Row_ID"].ToString() == dr["ModifierParetn_Row_ID"].ToString())
                                                {
                                                    FieldValue += string.Format("{0}\t{1}", System.Environment.NewLine, Convert.ToString(drModifierChild["SKU_NAME"]));
                                                    Yposition += 20;
                                                }
                                            }
                                        }
                                        g.DrawString(FieldValue, InvoiceFont2, BlackBrush, xProductID, CurrentY);
                                        FieldValue = Convert.ToString(qty);// Qty
                                        g.DrawString(FieldValue, InvoiceFont2, BlackBrush, xProductName, CurrentY);

                                        CurrentY = CurrentY + Yposition;
                                        g.DrawLine(new Pen(Brushes.Black, 1), 10, CurrentY, 300, CurrentY);

                                    }
                                    else
                                    {
                                        g.DrawString(FieldValue + " ", InvoiceFont2, BlackBrush, xProductID, CurrentY);
                                        //Show Modifiers as Indent
                                        FieldValue = string.Empty;
                                        int Yposition = 20;
                                        foreach (DataRow drModifierChild in dtValue.Rows)
                                        {
                                            if (drModifierChild["MODIFIER_PARENT_ID"].ToString() != "0")
                                            {
                                                if (drModifierChild["MODIFIER_PARENT_ID"].ToString() == dr["SKU_ID"].ToString() && drModifierChild["ModifierParetn_Row_ID"].ToString() == dr["ModifierParetn_Row_ID"].ToString())
                                                {
                                                    FieldValue += string.Format("{0}\t{1}", System.Environment.NewLine, Convert.ToString(drModifierChild["SKU_NAME"]));
                                                    Yposition += 20;
                                                }
                                            }
                                        }
                                        g.DrawString(FieldValue, InvoiceFont2, BlackBrush, xProductID, CurrentY);

                                        FieldValue = Convert.ToString(qty);// Qty
                                        g.DrawString(FieldValue, InvoiceFont2, BlackBrush, xProductName, CurrentY);
                                        CurrentY = CurrentY + Yposition;
                                        g.DrawLine(new Pen(Brushes.Black, 1), 10, CurrentY, 300, CurrentY);
                                    }
                                    CurrentY = CurrentY + 10;
                                    CurrentRecord++;
                                }
                                if (Convert.ToDecimal(dr["QTY"].ToString()) == 0)
                                {
                                    if (qty == 0)
                                    {
                                        FieldValue = Convert.ToString(dr["SKU_NAME"]);// name

                                        if (FieldValue.Length > 24)
                                        {
                                            int Yposition = 40;
                                            string FieldValue2 = FieldValue.Substring(0, 25);
                                            g.DrawString(FieldValue2, InvoiceFont2, BlackBrush, xProductID, CurrentY);

                                            FieldValue = FieldValue.Substring(25);
                                            g.DrawString(FieldValue, InvoiceFont2, BlackBrush, xProductID, CurrentY + 20);

                                            //Show Modifiers as Indent
                                            FieldValue = string.Empty;
                                            foreach (DataRow drModifierChild in dtValue.Rows)
                                            {
                                                if (drModifierChild["MODIFIER_PARENT_ID"].ToString() != "0")
                                                {
                                                    if (drModifierChild["MODIFIER_PARENT_ID"].ToString() == dr["SKU_ID"].ToString() && drModifierChild["ModifierParetn_Row_ID"].ToString() == dr["ModifierParetn_Row_ID"].ToString())
                                                    {
                                                        FieldValue += string.Format("{0}\t{1}", System.Environment.NewLine, Convert.ToString(drModifierChild["SKU_NAME"]));
                                                        Yposition += 20;
                                                    }
                                                }
                                            }
                                            g.DrawString(FieldValue, InvoiceFont2, BlackBrush, xProductID, CurrentY + 20);

                                            FieldValue = Convert.ToString(qty);// Qty
                                            g.DrawString(FieldValue, InvoiceFont2, BlackBrush, xProductName, CurrentY);

                                            CurrentY = CurrentY + Yposition;
                                            g.DrawLine(new Pen(Brushes.Black, 1), 10, CurrentY, 300, CurrentY);
                                        }
                                        else
                                        {
                                            g.DrawString(FieldValue + " ", InvoiceFont2, BlackBrush, xProductID, CurrentY);

                                            //Show Modifiers as Indent
                                            int Yposition = 20;
                                            FieldValue = string.Empty;
                                            foreach (DataRow drModifierChild in dtValue.Rows)
                                            {
                                                if (drModifierChild["MODIFIER_PARENT_ID"].ToString() != "0")
                                                {
                                                    if (drModifierChild["MODIFIER_PARENT_ID"].ToString() == dr["SKU_ID"].ToString() && drModifierChild["ModifierParetn_Row_ID"].ToString() == dr["ModifierParetn_Row_ID"].ToString())
                                                    {
                                                        FieldValue += string.Format("{0}\t{1}", System.Environment.NewLine, Convert.ToString(drModifierChild["SKU_NAME"]));
                                                        Yposition += 20;
                                                    }
                                                }
                                            }
                                            g.DrawString(FieldValue, InvoiceFont2, BlackBrush, xProductID, CurrentY + 20);

                                            FieldValue = Convert.ToString(qty);// Qty
                                            g.DrawString(FieldValue, InvoiceFont2, BlackBrush, xProductName, CurrentY);

                                            CurrentY = CurrentY + Yposition;
                                            g.DrawLine(new Pen(Brushes.Black, 1), 10, CurrentY, 300, CurrentY);
                                        }
                                        CurrentY = CurrentY + 10;
                                        CurrentRecord++;
                                    }
                                }
                            }
                        }
                    }
                }
                if (OrderNotes.Length > 0)
                {
                    g.DrawString(OrderNotes, InvoiceFont, BlueBrush, 10, CurrentY);
                }
            }
            if (CurrentRecord > 0)
            {
                g.Dispose();
            }
        }

        #endregion

        #region Print-Invoice
        private static void PrintReportInvoice()
        {
            try
            {
                PrintDocument prnDocument = new PrintDocument();
                prnDocument.PrinterSettings.PrinterName = PrinterName;
                prnDocument.PrintPage += new PrintPageEventHandler(prnDocument_PrintPageInvoice);
                prnDocument.Print();
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
            }
        }

        private static void prnDocument_PrintPageInvoice(object sender, PrintPageEventArgs e)
        {
            leftMargin = (int)e.MarginBounds.Left;
            topMargin = 1;
            SetPrintInvoiceData(e.Graphics); // Draw Invoice
        }

        private static void SetPrintInvoiceData(Graphics g)
        {
            CurrentY = topMargin;
            CurrentX = leftMargin;

            //Print Logo
            Image tempImage = null;
            using (FileStream fs = new FileStream(System.Configuration.ConfigurationManager.AppSettings["LogoPath"].ToString(), FileMode.Open, FileAccess.Read))
            {
                tempImage = Image.FromStream(fs);
            }
            g.DrawImage(tempImage, 100, CurrentY, 100, 100);

            CurrentY = CurrentY + 100;
            var rectHeader = new RectangleF(8, CurrentY, 270, 20);
            var formatHeader = new StringFormat() { Alignment = StringAlignment.Center };
            g.DrawString("Sales Tax Invoice", InvOrderTitleFont, BlueBrush, rectHeader, formatHeader);

            CurrentY = CurrentY + 20;
            g.DrawString(Address, InvoiceFont3, BlueBrush, 10, CurrentY);

            CurrentY = CurrentY + 20;
            var rectContactNo = new RectangleF(8, CurrentY, 270, 20);
            g.DrawString("Ph: " + ContactNo, InvoiceFont3, BlueBrush, rectContactNo, formatHeader);

            CurrentY = CurrentY + 20;
            g.DrawString(TimeStamp.ToString("dd-MMM-yyyy hh:mm tt"), InvoiceFont3, BlueBrush, 10, CurrentY);

            CurrentY = CurrentY + 20;
            g.DrawString(ServiceType, InvoiceFont4, BlueBrush, 10, CurrentY);
            g.DrawString("MOP: " + MOP, InvoiceFont4, BlueBrush, 200, CurrentY);

            CurrentY = CurrentY + 20;
            g.DrawString("NTN: " + NTN, InvoiceFont3, BlueBrush, 10, CurrentY);

            CurrentY = CurrentY + 20;
            g.DrawString("Cashier: " + Cashier, InvoiceFont3, BlueBrush, 10, CurrentY);

            if (ServiceType.ToLower() == "Delivery".ToLower())
            {
                CurrentY = CurrentY + 20;
                g.DrawString("D-M: " + DM_OT, InvoiceFont, BlueBrush, 10, CurrentY);

                CurrentY = CurrentY + 20;
                g.DrawString(TableNo.Substring(0, 20), InvoiceFont, BlueBrush, 10, CurrentY);
            }
            else if (ServiceType.ToLower() == "Dine In".ToLower())
            {
                CurrentY = CurrentY + 20;
                g.DrawString("O-T: " + DM_OT, InvoiceFont, BlueBrush, 10, CurrentY);

                CurrentY = CurrentY + 20;
                g.DrawString(TableNo, InvoiceFont, BlueBrush, 10, CurrentY);
            }
            else
            {
                CurrentY = CurrentY + 20;
                g.DrawString("O-T: " + DM_OT, InvoiceFont, BlueBrush, 10, CurrentY);
                CurrentY = CurrentY + 20;
                g.DrawString(TableNo, InvoiceFont, BlueBrush, 10, CurrentY);
            }

            CurrentY = CurrentY + 20;
            g.DrawString("Order No: " + OrderNo, InvoiceFont4, BlueBrush, 10, CurrentY);
            g.DrawString("Bill No: " + InvoiceNo, InvoiceFont4, BlueBrush, 200, CurrentY);

            CurrentY = CurrentY + 30;
            int CurrentY2 = CurrentY - 2;
            Pen blackPen = new Pen(Color.FromArgb(255, 0, 0, 0), 1);
            g.DrawRectangle(blackPen, 8, CurrentY2, 120, 20);
            g.DrawRectangle(blackPen, 128, CurrentY2, 50, 20);
            g.DrawRectangle(blackPen, 178, CurrentY2, 50, 20);
            g.DrawRectangle(blackPen, 228, CurrentY2, 50, 20);

            g.DrawString("Item Name", InvoiceFont4, BlueBrush, 10, CurrentY);
            g.DrawString("Qty", InvoiceFont4, BlueBrush, 130, CurrentY);
            g.DrawString("Rate", InvoiceFont4, BlueBrush, 180, CurrentY);
            g.DrawString("Amount", InvoiceFont4, BlueBrush, 230, CurrentY);

            DataView dv = dtValueInvoice.DefaultView;
            dv.Sort = "C1";
            dtValueInvoice = dv.ToTable();
            decimal amount = 0;

            CurrentY2 = CurrentY2 + 20;
            CurrentY = CurrentY + 30;

            foreach (DataRow dr in dtValueInvoice.Rows)
            {
                if (dr["MODIFIER_PARENT_ID"].ToString() == "0")
                {
                    if (dr["SKU_NAME"].ToString().Length > 16)
                    {
                        g.DrawString(dr["SKU_NAME"].ToString().Substring(0, 16), InvoiceFont4, BlueBrush, 10, CurrentY);
                        g.DrawString(dr["SKU_NAME"].ToString().Substring(17, 16), InvoiceFont4, BlueBrush, 10, CurrentY + 20);

                        //Show Modifiers as Indent
                        string FieldValue = string.Empty;

                        var formatQty = new StringFormat() { Alignment = StringAlignment.Center };
                        var rectQty = new RectangleF(130, CurrentY, 50, 20);
                        g.DrawString(String.Format("{0:0}", dr["QTY"]), InvoiceFont4, BlueBrush, rectQty, formatQty);

                        var formatRate = new StringFormat() { Alignment = StringAlignment.Center };
                        var rectRate = new RectangleF(180, CurrentY, 50, 20);
                        g.DrawString(String.Format("{0:0}", dr["Rate"]), InvoiceFont4, BlueBrush, rectRate, formatRate);

                        var formatAmount = new StringFormat() { Alignment = StringAlignment.Center };
                        var rectAmount = new RectangleF(230, CurrentY, 50, 20);
                        g.DrawString(String.Format("{0:0}", dr["Amount"]), InvoiceFont4, BlueBrush, rectAmount, formatAmount);

                        foreach (DataRow drModifierChild in dtValueInvoice.Rows)
                        {
                            if (drModifierChild["MODIFIER_PARENT_ID"].ToString() != "0")
                            {
                                if (drModifierChild["MODIFIER_PARENT_ID"].ToString() == dr["SKU_ID"].ToString() && drModifierChild["ModifierParetn_Row_ID"].ToString() == dr["ModifierParetn_Row_ID"].ToString())
                                {
                                    FieldValue = string.Format("{0}   {1}", System.Environment.NewLine, Convert.ToString(drModifierChild["SKU_NAME"]));
                                    g.DrawString(FieldValue, InvoiceFont4, BlueBrush, 10, CurrentY);

                                    formatQty = new StringFormat() { Alignment = StringAlignment.Center };
                                    rectQty = new RectangleF(130, CurrentY + 10, 50, 20);
                                    g.DrawString(String.Format("{0:0}", drModifierChild["QTY"]), InvoiceFont4, BlueBrush, rectQty, formatQty);

                                    formatRate = new StringFormat() { Alignment = StringAlignment.Center };
                                    rectRate = new RectangleF(180, CurrentY + 10, 50, 20);
                                    g.DrawString(String.Format("{0:0}", drModifierChild["Rate"]), InvoiceFont4, BlueBrush, rectRate, formatRate);

                                    formatAmount = new StringFormat() { Alignment = StringAlignment.Center };
                                    rectAmount = new RectangleF(230, CurrentY + 10, 50, 20);
                                    g.DrawString(String.Format("{0:0}", drModifierChild["Amount"]), InvoiceFont4, BlueBrush, rectAmount, formatAmount);

                                    amount += Convert.ToDecimal(drModifierChild["Amount"]);

                                    CurrentY += 10;
                                    CurrentY2 += 10;
                                }
                            }
                        }
                        CurrentY2 = CurrentY2 + 20;
                        CurrentY = CurrentY + 20;
                    }
                    else
                    {
                        //Show Modifiers as Indent
                        string FieldValue = dr["SKU_NAME"].ToString();
                        g.DrawString(FieldValue, InvoiceFont4, BlueBrush, 10, CurrentY);

                        var formatQty = new StringFormat() { Alignment = StringAlignment.Center };
                        var rectQty = new RectangleF(130, CurrentY, 50, 20);
                        g.DrawString(String.Format("{0:0}", dr["QTY"]), InvoiceFont4, BlueBrush, rectQty, formatQty);

                        var formatRate = new StringFormat() { Alignment = StringAlignment.Center };
                        var rectRate = new RectangleF(180, CurrentY, 50, 20);
                        g.DrawString(String.Format("{0:0}", dr["Rate"]), InvoiceFont4, BlueBrush, rectRate, formatRate);

                        var formatAmount = new StringFormat() { Alignment = StringAlignment.Center };
                        var rectAmount = new RectangleF(230, CurrentY, 50, 20);
                        g.DrawString(String.Format("{0:0}", dr["Amount"]), InvoiceFont4, BlueBrush, rectAmount, formatAmount);

                        foreach (DataRow drModifierChild in dtValueInvoice.Rows)
                        {
                            if (drModifierChild["MODIFIER_PARENT_ID"].ToString() != "0")
                            {
                                if (drModifierChild["MODIFIER_PARENT_ID"].ToString() == dr["SKU_ID"].ToString() && drModifierChild["ModifierParetn_Row_ID"].ToString() == dr["ModifierParetn_Row_ID"].ToString())
                                {
                                    FieldValue = string.Format("{0}   {1}", System.Environment.NewLine, Convert.ToString(drModifierChild["SKU_NAME"]));
                                    g.DrawString(FieldValue, InvoiceFont4, BlueBrush, 10, CurrentY);

                                    formatQty = new StringFormat() { Alignment = StringAlignment.Center };
                                    rectQty = new RectangleF(130, CurrentY + 10, 50, 20);
                                    g.DrawString(String.Format("{0:0}", drModifierChild["QTY"]), InvoiceFont4, BlueBrush, rectQty, formatQty);

                                    formatRate = new StringFormat() { Alignment = StringAlignment.Center };
                                    rectRate = new RectangleF(180, CurrentY + 10, 50, 20);
                                    g.DrawString(String.Format("{0:0}", drModifierChild["Rate"]), InvoiceFont4, BlueBrush, rectRate, formatRate);

                                    formatAmount = new StringFormat() { Alignment = StringAlignment.Center };
                                    rectAmount = new RectangleF(230, CurrentY + 10, 50, 20);
                                    g.DrawString(String.Format("{0:0}", drModifierChild["Amount"]), InvoiceFont4, BlueBrush, rectAmount, formatAmount);

                                    amount += Convert.ToDecimal(drModifierChild["Amount"]);

                                    CurrentY += 10;
                                    CurrentY2 += 10;
                                }
                            }
                        }
                        CurrentY2 = CurrentY2 + 20;
                        CurrentY = CurrentY + 20;
                    }
                    amount += Convert.ToDecimal(dr["Amount"]);
                }
            }
            decimal discount = 0;
            decimal servicecharges = 0;
            if (DISCOUNT > 0)
            {
                if (DISCOUNT_TYPE == "0")
                {
                    discount = amount * (DISCOUNT / 100);
                }
                else
                {
                    discount = DISCOUNT;
                }
            }
            if (SERVICE_CHARGES > 0)
            {
                if (SERVICE_CHARGES_TYPE == "0")
                {
                    servicecharges = amount * SERVICE_CHARGES / 100;
                }
            }

            CurrentY2 += 20;
            CurrentY += 20;

            var format = new StringFormat() { Alignment = StringAlignment.Far };
            var rectTotal = new RectangleF(8, CurrentY2, 270, 20);
            g.DrawString("Total: " + String.Format("{0:0}", amount), InvoiceFont4, BlueBrush, rectTotal, format);

            if (discount > 0)
            {
                CurrentY2 += 20;
                var rectDiscount = new RectangleF(8, CurrentY2, 270, 20);
                if (DISCOUNT_TYPE == "0")
                {
                    g.DrawString("Disc @" + String.Format("{0:0.00}", DISCOUNT) + "%: " + String.Format("{0:0}", discount), InvoiceFont4, BlueBrush, rectDiscount, format);
                }
                else
                {
                    g.DrawString("Discount :" + String.Format("{0:0}", discount), InvoiceFont4, BlueBrush, rectDiscount, format);
                }
            }
            if (GST > 0)
            {
                CurrentY2 += 20;
                var rectGST = new RectangleF(8, CurrentY2, 270, 20);

                if (PAYMENT_MODE_ID == "2")
                {
                    g.DrawString(TaxAuthorirty + " @" + String.Format("{0:0.00}", CreditCardGSTPer) + "%: " + String.Format("{0:0}", GST), InvoiceFont4, BlueBrush, rectGST, format);
                }
                else
                {
                    g.DrawString(TaxAuthorirty + " @" + String.Format("{0:0.00}", GSTPer) + "%: " + String.Format("{0:0}", GST), InvoiceFont4, BlueBrush, rectGST, format);
                }
            }

            if (servicecharges > 0)
            {
                CurrentY2 += 20;
                var rectServiceCharges = new RectangleF(8, CurrentY2, 270, 20);

                if (SERVICE_CHARGES_TYPE == "0")
                {
                    g.DrawString("Ser. Charges @" + String.Format("{0:0.00}", SERVICE_CHARGES) + "%: " + String.Format("{0:0}", servicecharges), InvoiceFont4, BlueBrush, rectServiceCharges, format);
                }
                else
                {
                    g.DrawString("Service Charges: " + String.Format("{0:0}", servicecharges), InvoiceFont4, BlueBrush, rectServiceCharges, format);
                }
            }
            CurrentY2 += 20;
            var rectGrandTotal = new RectangleF(8, CurrentY2, 270, 20);
            g.DrawString("Grand Total :" + String.Format("{0:0}", amount + GST + servicecharges - discount), InvoiceFont4, BlueBrush, rectGrandTotal, format);

            g.Dispose();
        }


        #endregion

        #region Crystal Reports
        private void PrintKOTCrystalReportOld(DataTable dtValueOrder, string PrinterName, string OrderNo, int ReportType, int Seconds, int NofOfCopies, bool IsFullKOT, string REMARKS, string LastUpdateDateTime, string SectionName, string KOTType)
        {
            if (PrinterName.Length > 0)
            {
                try
                {
                    CrystalDecisions.CrystalReports.Engine.ReportDocument report = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                    if (IsFullKOT)
                    {
                        report = new CrpFullKOT();
                    }
                    else
                    {
                        report = new CrpKOT();
                    }
                    report.SetDataSource(dtValueOrder);
                    report.Refresh();
                    report.PrintOptions.PrinterName = PrinterName;
                    if (ReportType == 3 || ReportType == 4)
                    {
                        report.SetParameterValue("KOTHeader", string.Empty);
                    }
                    else if (ReportType == 5)
                    {
                        report.SetParameterValue("KOTHeader", "RUNNING ORDER - ADD ITEM Qty");
                    }
                    else if (ReportType == 6)
                    {
                        report.SetParameterValue("KOTHeader", "RUNNING ORDER - ADD ITEM");
                    }
                    else if (ReportType == 1)
                    {
                        report.SetParameterValue("KOTHeader", "RUNNING ORDER - CANCELED ITEM");
                    }
                    else if (ReportType == 2)
                    {
                        report.SetParameterValue("KOTHeader", "RUNNING ORDER - ITEM LESS");
                    }
                    else
                    {
                        if (Seconds > 5 && ReportType == 0)
                        {
                            report.SetParameterValue("KOTHeader", "RUNNING ORDER");
                        }
                        else
                        {
                            report.SetParameterValue("KOTHeader", string.Empty);
                        }
                    }
                    report.SetParameterValue("REMARKS", REMARKS);
                    if (IsLocationName == "1")
                    {
                        report.SetParameterValue("LocationName", "Location Name: " + System.Configuration.ConfigurationManager.AppSettings["LocationName"].ToString());
                    }
                    else
                    {
                        report.SetParameterValue("LocationName", "");
                    }
                    string FullKOTGroup = "1";
                    try
                    {
                        FullKOTGroup = System.Configuration.ConfigurationManager.AppSettings["FullKOTGroup"].ToString();
                    }
                    catch (Exception)
                    {
                        FullKOTGroup = "1";
                    }
                    report.SetParameterValue("FullKOTGroup", FullKOTGroup);
                    report.SetParameterValue("Covers", "Covers: " + covertable);

                    report.PrintToPrinter(NofOfCopies, false, 0, 0);
                    report.Close();
                    report.Dispose();
                    GC.Collect();
                    if (ReportType == 3)
                    {
                        WriteLog(string.Format("Order No-{0}- Full KOT Printed.", OrderNo));
                    }
                    else if (ReportType == 4)
                    {
                        WriteLog(string.Format("Order No-{0}- Xpeditor KOT Printed.", OrderNo));
                    }
                    else if (ReportType == 5)
                    {
                        WriteLog(string.Format("Order No-{0}- Add Item Qty KOT Printed.", OrderNo));
                    }
                    else if (ReportType == 6)
                    {
                        WriteLog(string.Format("Order No-{0}- Add New Item KOT Printed.", OrderNo));
                    }
                    else if (ReportType == 1)
                    {
                        WriteLog(string.Format("Order No-{0}-KOT Cancel Item Printed.", OrderNo));
                    }
                    else if (ReportType == 2)
                    {
                        WriteLog(string.Format("Order No-{0}-KOT Less Item Qty Printed.", OrderNo));
                    }
                    else
                    {
                        if (Seconds > 5 && ReportType == 0)
                        {
                            WriteLog(string.Format("Order No-{0}-KOT Running Order Printed.", OrderNo));
                        }
                        else
                        {
                            WriteLog(string.Format("Order No-{0}-KOT Printed.", OrderNo));
                        }
                    }
                }
                catch (Exception ex)
                {
                    WriteLog(ex.ToString());
                }
            }
        }
        private bool PrintKOTCrystalReport(DataTable dtItems, string PrinterName, string OrderNo, int ReportType, int Seconds, int NofOfCopies, bool IsFullKOT, string REMARKS, string LastUpdateDateTime, string SectionName, string KOTType)
        {
            bool flag = true;
            if (IsKOTPrinted(OrderNo, SectionName, LastUpdateDateTime, KOTType))
            {
                WriteLog("KOT already printed.");
                return false;
            }
            else
            {
                flag = WriteLogKOTPrinted(OrderNo, SectionName, LastUpdateDateTime, KOTType, DOCUMENT_DATE);
            }
            DataTable dtValueOrder = dtItems.Clone();
            foreach (DataRow dr in dtItems.Rows)
            {
                if (Convert.ToBoolean(dr["IsUngroup"]))
                {
                    if (IsUnGroupItemPrinted(OrderNo, dr["TIME_STAMP2"].ToString(), KOTType))
                    {
                        WriteLog("Ungroup Item already printed.");
                    }
                    else
                    {
                        dtValueOrder.ImportRow(dr);
                        WriteLogUnGroupItem(OrderNo,dr["TIME_STAMP2"].ToString(), KOTType, DOCUMENT_DATE);
                    }
                }
                else
                {
                    dtValueOrder.ImportRow(dr);
                }
            }

            List<DataRow> rowsToRemove = new List<DataRow>();
            foreach (DataRow dr in dtValueOrder.Rows)
            {
                if (IsItemPrinted(OrderNo, dr["SKU_ID"].ToString(), dr["LastUpdateDateTime"].ToString(), dr["ModifierParetn_Row_ID"].ToString(), SectionName))
                {
                    rowsToRemove.Add(dr);
                    WriteLog("Item already printed.");
                }
                else
                {
                    WriteItemPrint(OrderNo, dr["SKU_ID"].ToString(), dr["LastUpdateDateTime"].ToString(), dr["ModifierParetn_Row_ID"].ToString(), SectionName, DOCUMENT_DATE);
                }
            }
            foreach (DataRow row in rowsToRemove)
            {
                dtValueOrder.Rows.Remove(row);
            }

            if (dtValueOrder.Rows.Count > 0)
            {
                if (PrinterName.Length > 0)
                {
                    try
                    {
                        using (ReportDocument report = IsFullKOT
                            ? (ReportDocument)new CrpFullKOT()
                            : (ReportDocument)new CrpKOT())
                        {
                            report.SetDataSource(dtValueOrder);
                            report.Refresh();
                            report.PrintOptions.PrinterName = PrinterName;

                            // --- Parameters ---
                            if (ReportType == 3 || ReportType == 4)
                                report.SetParameterValue("KOTHeader", string.Empty);
                            else if (ReportType == 5)
                                report.SetParameterValue("KOTHeader", "RUNNING ORDER - ADD ITEM Qty");
                            else if (ReportType == 6)
                                report.SetParameterValue("KOTHeader", "RUNNING ORDER - ADD ITEM");
                            else if (ReportType == 1)
                                report.SetParameterValue("KOTHeader", "RUNNING ORDER - CANCELED ITEM");
                            else if (ReportType == 2)
                                report.SetParameterValue("KOTHeader", "RUNNING ORDER - ITEM LESS");
                            else if (Seconds > 5 && ReportType == 0)
                                report.SetParameterValue("KOTHeader", "RUNNING ORDER");
                            else
                                report.SetParameterValue("KOTHeader", string.Empty);

                            report.SetParameterValue("REMARKS", REMARKS);

                            if (IsLocationName == "1")
                                report.SetParameterValue("LocationName",
                                    "Location Name: " + System.Configuration.ConfigurationManager.AppSettings["LocationName"]);
                            else
                                report.SetParameterValue("LocationName", "");

                            string FullKOTGroup = "1";
                            try { FullKOTGroup = System.Configuration.ConfigurationManager.AppSettings["FullKOTGroup"]; }
                            catch { FullKOTGroup = "1"; }

                            report.SetParameterValue("FullKOTGroup", FullKOTGroup);
                            report.SetParameterValue("Covers", "Covers: " + covertable);

                            // --- Print ---
                            report.PrintToPrinter(NofOfCopies, false, 0, 0);

                            foreach (DataRow drDetail in dtValueOrder.Rows)
                            {
                                if (Convert.ToBoolean(drDetail["IsUngroup"]))
                                {
                                    if (!UpdateSaleDetailPrintQty(Convert.ToInt64(OrderNo), Convert.ToInt32(drDetail["SKU_ID"]), Convert.ToDecimal(drDetail["QTY"]), Convert.ToInt64(drDetail["SALE_INVOICE_DETAIL_ID"])))
                                    {
                                        WriteLog("SKUID: " + drDetail["SKU_ID"].ToString() + " not Updated.");
                                    }
                                }
                                else
                                {
                                    if (!UpdateSaleDetailPrintQty(Convert.ToInt64(OrderNo), Convert.ToInt32(drDetail["SKU_ID"]), Convert.ToDecimal(drDetail["QTY"])))
                                    {
                                        WriteLog("SKUID: " + drDetail["SKU_ID"].ToString() + " not Updated.");
                                    }
                                }
                            }

                        } // ✅ report.Dispose() auto called here

                        // --- Logging after print ---
                        if (ReportType == 3)
                            WriteLog($"Order No-{OrderNo}- Full KOT Printed.");
                        else if (ReportType == 4)
                            WriteLog($"Order No-{OrderNo}- Xpeditor KOT Printed.");
                        else if (ReportType == 5)
                            WriteLog($"Order No-{OrderNo}- Add Item Qty KOT Printed.");
                        else if (ReportType == 6)
                            WriteLog($"Order No-{OrderNo}- Add New Item KOT Printed.");
                        else if (ReportType == 1)
                            WriteLog($"Order No-{OrderNo}- KOT Cancel Item Printed.");
                        else if (ReportType == 2)
                            WriteLog($"Order No-{OrderNo}- KOT Less Item Qty Printed.");
                        else if (Seconds > 5 && ReportType == 0)
                            WriteLog($"Order No-{OrderNo}- KOT Running Order Printed.");
                        else
                            WriteLog($"Order No-{OrderNo}- KOT Printed.");
                    }
                    catch (Exception ex)
                    {
                        WriteLog("Print Error: " + ex.Message);
                        DeleteKOTPrinted(OrderNo, SectionName, LastUpdateDateTime, KOTType, DOCUMENT_DATE);
                        WriteLog("Printer not found entry deleted from KOTPrinted file.");
                        foreach (DataRow drItem in dtValueOrder.Rows)
                        {
                            DeleteItemPrinted(OrderNo, drItem["SKU_ID"].ToString(), drItem["LastUpdateDateTimeDetail"].ToString(), drItem["ModifierParetn_Row_ID"].ToString(), SectionName, DOCUMENT_DATE);
                            WriteLog("ItemID: " + drItem["SKU_ID"].ToString() + " deleted from PrintedItemLogFile");
                        }
                        flag = false;
                    }

                }
                else
                {
                    DeleteKOTPrinted(OrderNo, SectionName, LastUpdateDateTime, KOTType, DOCUMENT_DATE);
                    WriteLog("Printer not found entry deleted from KOTPrinted file.");
                    foreach (DataRow drItem in dtValueOrder.Rows)
                    {
                        DeleteItemPrinted(OrderNo, drItem["SKU_ID"].ToString(), drItem["LastUpdateDateTimeDetail"].ToString(), drItem["ModifierParetn_Row_ID"].ToString(), SectionName, DOCUMENT_DATE);
                        WriteLog("ItemID: " + drItem["SKU_ID"].ToString() + " deleted from PrintedItemLogFile");
                    }
                    flag = false;
                }
            }
            else
            {
                flag = false;
            }
            return flag;
        }
        private bool PrintKOTCrystalReportNew(DataTable dtValueOrder, string PrinterName, string OrderNo, int NofOfCopies, bool IsFullKOT, string REMARKS, string LastUpdateDateTime, string SectionName, string KOTType,byte bytKOTType,string IsXpeditor)
        {
            bool flag = true;
            //if (IsKOTPrinted(OrderNo, SectionName, LastUpdateDateTime, KOTType))
            //{
            //    WriteLogNew("KOT already printed.", string.Empty);
            //    return false;
            //}
            //else
            //{
            //    flag = WriteLogKOTPrinted(OrderNo, SectionName, LastUpdateDateTime, KOTType, DOCUMENT_DATE);
            //}
            //DataTable dtValueOrder = dtItems.Clone();
            //foreach (DataRow dr in dtItems.Rows)
            //{
            //    if (Convert.ToBoolean(dr["IsUngroup"]))
            //    {
            //        if (IsUnGroupItemPrinted(OrderNo, dr["TimeStamp"].ToString(), KOTType))
            //        {
            //            WriteLogNew("Ungroup Item already printed.",string.Empty);
            //        }
            //        else
            //        {
            //            dtValueOrder.ImportRow(dr);
            //            //WriteLogUnGroupItem(OrderNo, dr["TimeStamp"].ToString(), KOTType, DOCUMENT_DATE);
            //        }
            //    }
            //    else
            //    {
            //        dtValueOrder.ImportRow(dr);
            //    }
            //}

            //List<DataRow> rowsToRemove = new List<DataRow>();
            //foreach (DataRow dr in dtValueOrder.Rows)
            //{
            //    if (IsItemPrinted(OrderNo, dr["SKU_ID"].ToString(), dr["LastUpdateDateTimeDetail"].ToString(), dr["ModifierParetn_Row_ID"].ToString(), SectionName))
            //    {
            //        rowsToRemove.Add(dr);
            //        WriteLogNew("Item already printed.", string.Empty);                    
            //    }
            //    else
            //    {
            //        WriteItemPrint(OrderNo, dr["SKU_ID"].ToString(), dr["LastUpdateDateTimeDetail"].ToString(), dr["ModifierParetn_Row_ID"].ToString(), SectionName, DOCUMENT_DATE);
            //    }
            //}
            //foreach (DataRow row in rowsToRemove)
            //{
            //    dtValueOrder.Rows.Remove(row);
            //}

            if (dtValueOrder.Rows.Count > 0)
            {
                if (PrinterName.Length > 0)
                {
                    try
                    {
                        using (ReportDocument report = IsFullKOT
                            ? (ReportDocument)new CrpFullKOT()
                            : (ReportDocument)new CrpKOT())
                        {
                            report.SetDataSource(dtValueOrder);
                            report.Refresh();
                            report.PrintOptions.PrinterName = PrinterName;

                            // --- Parameters ---
                            if (bytKOTType == 1)
                                report.SetParameterValue("KOTHeader", string.Empty);
                            else if (bytKOTType == 3)
                                report.SetParameterValue("KOTHeader", "RUNNING ORDER - ADD ITEM Qty");
                            else if (bytKOTType == 2)
                                report.SetParameterValue("KOTHeader", "RUNNING ORDER - ADD ITEM");
                            else if (bytKOTType == 5)
                                report.SetParameterValue("KOTHeader", "RUNNING ORDER - CANCELED ITEM");
                            else if (bytKOTType == 4)
                                report.SetParameterValue("KOTHeader", "RUNNING ORDER - ITEM LESS");                            
                            else
                                report.SetParameterValue("KOTHeader", string.Empty);

                            report.SetParameterValue("REMARKS", REMARKS);

                            if (IsLocationName == "1")
                                report.SetParameterValue("LocationName",
                                    "Location Name: " + System.Configuration.ConfigurationManager.AppSettings["LocationName"]);
                            else
                                report.SetParameterValue("LocationName", "");

                            string FullKOTGroup = "1";
                            try { FullKOTGroup = System.Configuration.ConfigurationManager.AppSettings["FullKOTGroup"]; }
                            catch { FullKOTGroup = "1"; }

                            report.SetParameterValue("FullKOTGroup", FullKOTGroup);
                            report.SetParameterValue("Covers", "Covers: " + covertable);

                            // --- Print ---
                            report.PrintToPrinter(NofOfCopies, false, 0, 0);

                            //string pdfPath = @"C:\Reports\KOT_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".pdf";
                            //ExportOptions exportOptions = new ExportOptions();
                            //DiskFileDestinationOptions diskOptions = new DiskFileDestinationOptions();
                            //diskOptions.DiskFileName = pdfPath;
                            //exportOptions = report.ExportOptions;
                            //exportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                            //exportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                            //exportOptions.DestinationOptions = diskOptions;
                            //report.Export();

                            if (!UpdatePrintedKOT(Convert.ToInt64(OrderNo), bytKOTType, SectionName, 1))
                            {
                                WriteLogNew("KOT Print not updated.",string.Empty);
                            }

                            if (SectionName == "Xpeditor")
                            {
                                if (!UpdatePrintedKOT(Convert.ToInt64(OrderNo), bytKOTType, string.Empty, 2))
                                {
                                    WriteLogNew("Xpeditor KOT Print not updated.",string.Empty);
                                }
                            }

                        } // ✅ report.Dispose() auto called here

                        // --- Logging after print ---
                            WriteLogNew($"Order No-{OrderNo}- " + KOTType +" Printed.",string.Empty);                        
                    }
                    catch (Exception ex)
                    {
                        WriteLogNew("Print Error: " + ex.Message, "PrintKOTCrystalReportNew");
                        //DeleteKOTPrinted(OrderNo, SectionName, LastUpdateDateTime, KOTType, DOCUMENT_DATE);
                        //WriteLogNew("Printer not found entry deleted from KOTPrinted file.", "PrintKOTCrystalReportNew");
                        //foreach (DataRow drItem in dtValueOrder.Rows)
                        //{
                        //    DeleteItemPrinted(OrderNo, drItem["SKU_ID"].ToString(), drItem["LastUpdateDateTimeDetail"].ToString(), drItem["ModifierParetn_Row_ID"].ToString(), SectionName, DOCUMENT_DATE);
                        //    WriteLogNew("ItemID: " + drItem["SKU_ID"].ToString() + " deleted from PrintedItemLogFile", "PrintKOTCrystalReportNew");
                        //}
                        flag = false;
                    }

                }
                else
                {
                    //DeleteKOTPrinted(OrderNo, SectionName, LastUpdateDateTime, KOTType, DOCUMENT_DATE);
                    //WriteLogNew("Printer not found entry deleted from KOTPrinted file.", "PrintKOTCrystalReportNew");
                    //foreach (DataRow drItem in dtValueOrder.Rows)
                    //{
                    //    DeleteItemPrinted(OrderNo, drItem["SKU_ID"].ToString(), drItem["LastUpdateDateTimeDetail"].ToString(), drItem["ModifierParetn_Row_ID"].ToString(), SectionName, DOCUMENT_DATE);
                    //    WriteLogNew("ItemID: " + drItem["SKU_ID"].ToString() + " deleted from PrintedItemLogFile", "PrintKOTCrystalReportNew");
                    //}
                    flag = false;
                }
            }
            else
            {
                flag = false;
            }
            return flag;
        }
        private static void PrintStickerCrystalReport(DataTable dtValueOrderSticker, string PrinterName, string OrderNo, int ReportType, int Seconds, int NofOfCopies)
        {
            if (PrinterName.Length > 0)
            {
                bool PrinterFound = false;
                var server = new PrintServer();
                var queues = server.GetPrintQueues(new[]
                { EnumeratedPrintQueueTypes.Shared, EnumeratedPrintQueueTypes.Connections });
                foreach (var item in queues)
                {
                    if (item.FullName == PrinterName)
                    {
                        PrinterFound = true;
                        break;
                    }
                }

                queues = server.GetPrintQueues(new[]
                { EnumeratedPrintQueueTypes.Local });
                foreach (var item in queues)
                {
                    if (item.FullName == PrinterName)
                    {
                        PrinterFound = true;
                        break;
                    }
                }
                if (PrinterFound)
                {
                    try
                    {
                        CrystalDecisions.CrystalReports.Engine.ReportDocument report = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                        report = new CrpKOTSticker();
                        string LenghtySticker = "0";
                        string PizzaBoxSticker = "0";
                        try
                        {
                            LenghtySticker = System.Configuration.ConfigurationManager.AppSettings["LenghtySticker"].ToString();
                            PizzaBoxSticker = System.Configuration.ConfigurationManager.AppSettings["PizzaBoxSticker"].ToString();
                        }
                        catch (Exception ex)
                        {
                            WriteLog(ex.ToString());
                            LenghtySticker = "0";
                            PizzaBoxSticker = "0";
                        }
                        if (LenghtySticker == "1")
                        {
                            report = new CrpKOTSticker2();
                        }
                        else if (PizzaBoxSticker == "1")
                        {
                            report = new CrpKOTStickerDelish();
                        }
                        else
                        {
                            report = new CrpKOTSticker();
                        }
                        report.SetDataSource(dtValueOrderSticker);
                        report.Refresh();
                        report.PrintOptions.PrinterName = PrinterName;
                        if (IsLocationName == "1")
                        {
                            report.SetParameterValue("LocationName", System.Configuration.ConfigurationManager.AppSettings["LocationName"].ToString());
                        }
                        else
                        {
                            report.SetParameterValue("LocationName", "");
                        }
                        report.PrintToPrinter(NofOfCopies, false, 0, 0);
                        report.Close();
                        report.Dispose();
                        GC.Collect();
                        if (ReportType == 0)
                        {
                            WriteLog(string.Format("Order No-{0}- Void Sticker Printed.", OrderNo));
                        }
                        else
                        {
                            if (Seconds > 10 && ReportType == 1)
                            {
                                WriteLog(string.Format("Order No-{0}-Sticker Running Printed.", OrderNo));
                            }
                            else
                            {
                                WriteLog(string.Format("Order No-{0}-Sticker Printed.", OrderNo));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        WriteLog(ex.ToString());
                    }
                }
                else
                {
                    WriteLog("Printer not found");
                }
            }
        }
        private static void PrintInvoiceCrystalReport(DataTable dtValueOrder, DataTable dtParam, string PrinterNameInvoice, string OrderNo, int NofOfCopies, int PrintType)
        {
            if (PrinterNameInvoice.Length > 0)
            {
                try
                {
                    CrystalDecisions.CrystalReports.Engine.ReportDocument SubReport = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                    DataSet ds = new dsKOT();
                    foreach (DataRow dr in dtValueOrder.Rows)
                    {
                        if (dr["MODIFIER_PARENT_ID"].ToString() == "0")
                        {
                            ds.Tables["dtInvoice"].ImportRow(dr);
                        }
                        else
                        {
                            ds.Tables["dtInvoiceModifiers"].ImportRow(dr);
                        }
                    }
                    CrpInvoice report = new CrpInvoice();
                    report.SetDataSource(ds.Tables["dtInvoice"]);
                    SubReport = report.OpenSubreport("sbModifier");
                    SubReport.SetDataSource(ds.Tables["dtInvoiceModifiers"]);
                    report.Refresh();

                    report.SetParameterValue("PrintType", PrintType);
                    try
                    {
                        report.SetParameterValue("TIME_STAMP", Convert.ToDateTime(dtParam.Rows[0]["TIME_STAMP"]));
                    }
                    catch (Exception)
                    {
                        report.SetParameterValue("TIME_STAMP", DateTime.Now);
                    }
                    report.SetParameterValue("MOP", dtParam.Rows[0]["MOP"]);
                    try
                    {
                        report.SetParameterValue("OrderNo", Convert.ToInt64(dtParam.Rows[0]["OrderNo"]));
                    }
                    catch (Exception)
                    {
                        report.SetParameterValue("OrderNo", 0);
                    }
                    report.SetParameterValue("Cashier", dtParam.Rows[0]["Cashier"]);
                    report.SetParameterValue("Covers", dtParam.Rows[0]["Covers"]);
                    report.SetParameterValue("TableNo", dtParam.Rows[0]["TableNo"]);
                    report.SetParameterValue("DM_OT", dtParam.Rows[0]["DM_OT"]);
                    report.SetParameterValue("ServiceType", dtParam.Rows[0]["ServiceType"]);
                    try
                    {
                        report.SetParameterValue("SALE_INVOICE_ID", Convert.ToInt64(dtParam.Rows[0]["SALE_INVOICE_ID"]));
                    }
                    catch (Exception)
                    {
                        report.SetParameterValue("SALE_INVOICE_ID", 0);
                    }
                    try
                    {
                        report.SetParameterValue("DISCOUNT", Convert.ToDecimal(dtParam.Rows[0]["DISCOUNT"]));
                    }
                    catch (Exception)
                    {
                        report.SetParameterValue("DISCOUNT", 0);
                    }
                    try
                    {
                        report.SetParameterValue("DISCOUNT_TYPE", Convert.ToInt32(dtParam.Rows[0]["DISCOUNT_TYPE"]));
                    }
                    catch (Exception)
                    {
                        report.SetParameterValue("DISCOUNT_TYPE", 0);
                    }
                    try
                    {
                        report.SetParameterValue("GST", Convert.ToDecimal(dtParam.Rows[0]["GST"]));
                    }
                    catch (Exception)
                    {
                        report.SetParameterValue("GST", 0);
                    }
                    try
                    {
                        report.SetParameterValue("SERVICE_CHARGES", Convert.ToDecimal(dtParam.Rows[0]["SERVICE_CHARGES"]));
                    }
                    catch (Exception)
                    {
                        report.SetParameterValue("SERVICE_CHARGES", 0);
                    }
                    try
                    {
                        report.SetParameterValue("SERVICE_CHARGES_TYPE", Convert.ToInt32(dtParam.Rows[0]["SERVICE_CHARGES_TYPE"]));
                    }
                    catch (Exception)
                    {
                        report.SetParameterValue("SERVICE_CHARGES_TYPE", 0);
                    }
                    try
                    {
                        report.SetParameterValue("PAYMENT_MODE_ID", Convert.ToInt32(dtParam.Rows[0]["PAYMENT_MODE_ID"]));
                    }
                    catch (Exception)
                    {
                        report.SetParameterValue("PAYMENT_MODE_ID", 0);
                    }
                    report.SetParameterValue("Address", dtParam.Rows[0]["Address"]);
                    report.SetParameterValue("ContactNo", dtParam.Rows[0]["ContactNo"]);
                    report.SetParameterValue("NTN", dtParam.Rows[0]["PAYMENT_MODE_ID"]);
                    report.SetParameterValue("TaxAuthorirty", dtParam.Rows[0]["TaxAuthorirty"]);
                    try
                    {
                        report.SetParameterValue("GSTPer", Convert.ToDecimal(dtParam.Rows[0]["GSTPer"]));
                    }
                    catch (Exception)
                    {
                        report.SetParameterValue("GSTPer", 0);
                    }
                    try
                    {
                        report.SetParameterValue("CreditCardGSTPer", Convert.ToDecimal(dtParam.Rows[0]["CreditCardGSTPer"]));
                    }
                    catch (Exception)
                    {
                        report.SetParameterValue("CreditCardGSTPer", 0);
                    }
                    try
                    {
                        report.SetParameterValue("PAIDIN", Convert.ToDecimal(dtParam.Rows[0]["PAIDIN"]));
                    }
                    catch (Exception)
                    {
                        report.SetParameterValue("PAIDIN", 0);
                    }
                    report.SetParameterValue("ImagePath", System.Configuration.ConfigurationManager.AppSettings["LogoPath"].ToString());
                    report.PrintOptions.PrinterName = PrinterNameInvoice;
                    report.PrintToPrinter(NofOfCopies, false, 0, 0);
                    report.Close();
                    report.Dispose();
                    GC.Collect();
                    WriteLog(string.Format("Order No-{0}- Invoice Printed.", OrderNo));
                }
                catch (Exception ex)
                {
                    WriteLog(ex.ToString());
                }
            }
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