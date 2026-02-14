using System;
using System.Data;
using System.Globalization;
using System.Web.UI;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using CORNBusinessLayer.Reports;
using System.Web;
using CrystalDecisions.CrystalReports.Engine;
using System.Web.UI.WebControls;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Linq;

/// <summary>
/// Form For Sale Person DSR Report
/// </summary>
public partial class Forms_rptSalePersonDSR : Page
{
    readonly DocumentPrintController _dPrint = new DocumentPrintController();
    readonly RptSaleController _rptSaleCtl = new RptSaleController();
    readonly DistributorController _dController = new DistributorController();

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        Response.Cache.SetNoStore();
        Response.AppendHeader("pragma", "no-cache");

        if (!IsPostBack)
        {
            LoadDistributor();
            LoadCashier();
            Configuration.SystemCurrentDateTime = (DateTime)Session["CurrentWorkDate"];
            txtStartDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtEndDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");

            txtStartDate.Attributes.Add("readonly", "readonly");
            txtEndDate.Attributes.Add("readonly", "readonly");
        }
    }

    /// <summary>
    /// Loads Locations To Location Combo
    /// </summary>
    private void LoadDistributor()
    {
        DataTable dt = _dController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(Session["UserId"].ToString()), int.Parse(Session["CompanyId"].ToString()));
        clsWebFormUtil.FillListBox(ChbDistributorList, dt, 0, 2);
        foreach (ListItem li in ChbDistributorList.Items)
        {
            li.Selected = true;
        }
    }
    protected void RbReportType_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblCashier.Visible = false;
        ddlCashier.Visible = false;
        if (RbReportType.SelectedItem.Value.ToString() == "4")
        {
            lblCashier.Visible = true;
            ddlCashier.Visible = true;
        }
    }
    /// <summary>
    /// Shows Sale Person DSR in PDF
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnViewPDF_Click(object sender, EventArgs e)
    {
        string location = string.Empty;
        string locationName = string.Empty;
        foreach (ListItem li in ChbDistributorList.Items)
        {
            if (li.Selected)
            {
                location += li.Value;
                location += ",";

                locationName += li.Text;
                locationName += ",";
            }
        }
        ShowReportPDF(location, locationName);
    }

    /// <summary>
    /// Shows Sale Person DSR in Excel
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnViewExcel_Click(object sender, EventArgs e)
    {
        string location = string.Empty;
        string locationName = string.Empty;
        foreach (ListItem li in ChbDistributorList.Items)
        {
            if (li.Selected)
            {
                location += li.Value;
                location += ",";

                locationName += li.Text;
                locationName += ",";
            }
        }
        ShowReportExcel(location, locationName);
    }

    private void ShowReportPDF(string location, string locationName)
    {
        DataTable dt = _dPrint.SelectReportTitle(Constants.IntNullValue);
        DataSet ds;
        if (RbReportType.SelectedIndex == 0)
        {
            ds = _rptSaleCtl.SelectSalePersonDSR(location, DateTime.Parse(txtStartDate.Text + " 00:00:00"), DateTime.Parse(txtEndDate.Text + " 23:59:59"), int.Parse(Session["UserId"].ToString()), Convert.ToInt32(RbReportType.SelectedItem.Value));
            CrpLocationDSR crpReport = new CrpLocationDSR();
            ReportDocument crCoverTables = crpReport.OpenSubreport("crCoverTables");
            ReportDocument sbDelivery = crpReport.OpenSubreport("sbDelivery");
            ReportDocument sbCustomerLedger = crpReport.OpenSubreport("sbCustomerLedger");
            ReportDocument sbGSTBreakup = crpReport.OpenSubreport("sbGSTBreakup");
            crpReport.SetDataSource(ds);
            crCoverTables.SetDataSource(ds);
            sbDelivery.SetDataSource(ds);
            sbCustomerLedger.SetDataSource(ds);
            sbGSTBreakup.SetDataSource(ds);
            crpReport.Refresh();
            crpReport.SetParameterValue("Location", locationName);
            crpReport.SetParameterValue("FROM_DATE", DateTime.Parse(txtStartDate.Text));
            crpReport.SetParameterValue("TO_DATE", DateTime.Parse(txtEndDate.Text));
            crpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());
            crpReport.SetParameterValue("Address", RbReportType.SelectedItem.Text);
            crpReport.SetParameterValue("ReportName", RbReportType.SelectedItem.Text + " wise Sales Report");
            Session.Add("CrpReport", crpReport);
            Session.Add("ReportType", 0);
            const string url = "'Default.aspx'";
            const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url +
                            ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
            Type cstype = GetType();
            ClientScriptManager cs = Page.ClientScript;
            cs.RegisterStartupScript(cstype, "OpenWindow", script);
        }
        else if (RbReportType.SelectedIndex == 2)
        {
            ds = _rptSaleCtl.SelectSalePersonDSR(location, DateTime.Parse(txtStartDate.Text + " 00:00:00"), DateTime.Parse(txtEndDate.Text + " 23:59:59"), int.Parse(Session["UserId"].ToString()), Convert.ToInt32(RbReportType.SelectedItem.Value));
            CrpCashierDSR crpReport = new CrpCashierDSR();
            ReportDocument sbGSTBreakup = crpReport.OpenSubreport("sbGSTBreakup");
            sbGSTBreakup.SetDataSource(ds);
            crpReport.SetDataSource(ds);
            crpReport.Refresh();
            crpReport.SetParameterValue("Location", locationName);
            crpReport.SetParameterValue("FROM_DATE", DateTime.Parse(txtStartDate.Text));
            crpReport.SetParameterValue("TO_DATE", DateTime.Parse(txtEndDate.Text));
            crpReport.SetParameterValue("CompanyName", dt.Rows[0]["COMPANY_NAME"].ToString());
            crpReport.SetParameterValue("Address", RbReportType.SelectedItem.Text);
            crpReport.SetParameterValue("ReportName", RbReportType.SelectedItem.Text + " wise Sales Report");
            Session.Add("CrpReport", crpReport);
            Session.Add("ReportType", 0);
            const string url = "'Default.aspx'";
            const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url +
                            ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
            Type cstype = GetType();
            ClientScriptManager cs = Page.ClientScript;
            cs.RegisterStartupScript(cstype, "OpenWindow", script);
        }
        else if (RbReportType.SelectedIndex == 1)
        {
            ds = _rptSaleCtl.PrintDailySalesReportDateWise(location, DateTime.Parse(txtStartDate.Text + " 00:00:00"), DateTime.Parse(txtEndDate.Text + " 23:59:59"), int.Parse(Session["UserId"].ToString()), 1);
            var crpReport = new CrpDateDSR();
            ReportDocument sbGSTBreakup = crpReport.OpenSubreport("sbGSTBreakup");
            sbGSTBreakup.SetDataSource(ds);
            crpReport.SetDataSource(ds);
            crpReport.Refresh();
            crpReport.SetParameterValue("Location", locationName);
            crpReport.SetParameterValue("FROM_DATE", DateTime.Parse(txtStartDate.Text));
            crpReport.SetParameterValue("TO_DATE", DateTime.Parse(txtEndDate.Text));
            crpReport.SetParameterValue("ReportName", "Daily Sales Report (Date Wise)");
            Session.Add("CrpReport", crpReport);
            Session.Add("ReportType", 0);
            const string url = "'Default.aspx'";
            const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url +
                            ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
            Type cstype = GetType();
            ClientScriptManager cs = Page.ClientScript;
            cs.RegisterStartupScript(cstype, "OpenWindow", script);
        }
    }

    private void ShowReportExcel(string location, string locationName)
    {
        DataTable dt = _dPrint.SelectReportTitle(Constants.IntNullValue);
        string ReportName = string.Empty;        
        switch (RbReportType.SelectedItem.Value.ToString())
        {
            case "1":
                ReportName = "Location Wise Sales Report";
                string[] strColumnNames = { "Location", "Sale Value", "Discount", "Value Exclusive Sale Tax", "Sale Tax", "Service / Delivery Charges", "Value Inclusive Sale Tax", "Third Party Delivery", "Credit Card Sales", "Credit Sales", "Cash Sales" };
                OfficeOpenXml.ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                using (ExcelPackage p = new ExcelPackage())
                {
                    ExcelWorksheet ws = p.Workbook.Worksheets.Add(ReportName);
                    var headerCells = ws.Cells[1, 1, 1, ExcelPackage.MaxColumns];
                    var headerFont = headerCells.Style.Font;
                    headerFont.Bold = true;
                    DataSet ds = _rptSaleCtl.SelectSalePersonDSR(location, DateTime.Parse(txtStartDate.Text), DateTime.Parse(txtEndDate.Text), int.Parse(Session["UserId"].ToString()), Convert.ToInt32(RbReportType.SelectedItem.Value));
                    ExportToExcelLocationWise(ws, p, ReportName, "Location: " + locationName, Convert.ToDateTime(txtStartDate.Text), Convert.ToDateTime(txtEndDate.Text), strColumnNames, ds.Tables["UspPrintSaleForceDSR"], ds.Tables["uspGetServiceWiseCoverTables"], ds.Tables["uspGetDailyDeliveryData"], ds.Tables["RptVoucherView"], ds.Tables["dtDailySalesGSTBreakup"]);
                    Byte[] fileBytes = p.GetAsByteArray();
                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment;filename=" + ReportName.Replace(" ", "") + ".xlsx");
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.BinaryWrite(fileBytes);
                    Response.End();
                }
                break;
            case "2":
                ReportName = "Daily Sales Report (Date Wise)";
                string[] strColumnNames2 = { "Date", "Gross Sales", "Discount", "Sale Tax", "Service / Delivery Charges", "Net Sales", "Third Party Delivery", "Credit Card Sales", "Credit Sales", "Cash Sales" };
                OfficeOpenXml.ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                using (ExcelPackage p = new ExcelPackage())
                {
                    ExcelWorksheet ws = p.Workbook.Worksheets.Add(ReportName);
                    var headerCells = ws.Cells[1, 1, 1, ExcelPackage.MaxColumns];
                    var headerFont = headerCells.Style.Font;
                    headerFont.Bold = true;
                    DataSet ds = _rptSaleCtl.PrintDailySalesReportDateWise(location, DateTime.Parse(txtStartDate.Text + " 00:00:00"), DateTime.Parse(txtEndDate.Text + " 23:59:59"), int.Parse(Session["UserId"].ToString()), 1);
                    ExportToExcelDateWise(ws, p, ReportName, "Location: " + locationName, Convert.ToDateTime(txtStartDate.Text), Convert.ToDateTime(txtEndDate.Text), strColumnNames2, ds.Tables["spPrintDailySalesReportDateWise"], ds.Tables["dtDailySalesGSTBreakup"]);
                    Byte[] fileBytes = p.GetAsByteArray();
                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment;filename=" + ReportName.Replace(" ", "") + ".xlsx");
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.BinaryWrite(fileBytes);
                    Response.End();
                }
                break;
            case "4":
                ReportName = "Cashier wise Sales Report";
                string[] strColumnNames3 = { "Cashier", "Sale Value", "Discount", "Value Exclusive Sale Tax", "Sale Tax", "Service / Delivery Charges", "Value Inclusive Sale Tax", "Third Party Delivery", "Credit Card Sales", "Credit Sales", "Cash Sales","Cash Skimmed","Cash Submitted","Difference / Counter" };
                OfficeOpenXml.ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                using (ExcelPackage p = new ExcelPackage())
                {
                    ExcelWorksheet ws = p.Workbook.Worksheets.Add(ReportName);
                    var headerCells = ws.Cells[1, 1, 1, ExcelPackage.MaxColumns];
                    var headerFont = headerCells.Style.Font;
                    headerFont.Bold = true;
                    DataSet ds = _rptSaleCtl.SelectSalePersonDSR(location, DateTime.Parse(txtStartDate.Text), DateTime.Parse(txtEndDate.Text), int.Parse(Session["UserId"].ToString()), Convert.ToInt32(RbReportType.SelectedItem.Value));
                    ExportToExcelCashierWise(ws, p, ReportName, "Location: " + locationName, Convert.ToDateTime(txtStartDate.Text), Convert.ToDateTime(txtEndDate.Text), strColumnNames3, ds.Tables["UspPrintSaleForceDSR"], ds.Tables["dtDailySalesGSTBreakup"]);
                    Byte[] fileBytes = p.GetAsByteArray();
                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment;filename=" + ReportName.Replace(" ", "") + ".xlsx");
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.BinaryWrite(fileBytes);
                    Response.End();
                }
                break;
            default:
                break;


        }
    }
    
    private void LoadCashier()
    {
        string location = string.Empty;
        foreach (ListItem li in ChbDistributorList.Items)
        {
            if (li.Selected)
            {
                location += li.Value;
                location += ",";
            }
        }
        ddlCashier.Items.Clear();
        Distributor_UserController UController = new Distributor_UserController();
        DataTable dtEmployee = UController.GetDistributorUser(1, location);
        if (dtEmployee.Rows.Count > 0)
        {
            ddlCashier.Items.Add(new DevExpress.Web.ListEditItem("All", Constants.IntNullValue.ToString(CultureInfo.InvariantCulture)));
            clsWebFormUtil.FillDxComboBoxList(ddlCashier, dtEmployee, "USER_ID", "USER_NAME");
            ddlCashier.SelectedIndex = 0;
        }
        else
        {
            ddlCashier.Items.Add(new DevExpress.Web.ListEditItem("No Record found.", "-1"));
            ddlCashier.SelectedIndex = 0;
        }
    }

    protected void ChbDistributorList_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCashier();
        int count = 0;
        foreach (ListItem li in ChbDistributorList.Items)
        {
            if (li.Selected)
            {
                count++;
            }
        }
        if (count == ChbDistributorList.Items.Count)
        {
            ChbSelectAll.Checked = true;
        }
        else
        {
            ChbSelectAll.Checked = false;
        }
    }

    protected void ChbSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        foreach (ListItem li in ChbDistributorList.Items)
        {
            li.Selected = ChbSelectAll.Checked;
        }
        LoadCashier();
    }

    public static void ExportToExcelLocationWise(ExcelWorksheet ws, ExcelPackage p, string strHeader, string strLocation, DateTime dtFrom, DateTime dtTo, string[] ColumnNames, DataTable UspPrintSaleForceDSR, DataTable uspGetServiceWiseCoverTables, DataTable uspGetDailyDeliveryData, DataTable RptVoucherView, DataTable dtDailySalesGSTBreakup)
    {
        int rowIndex = 1;

        #region Main Report
        if (UspPrintSaleForceDSR.Rows.Count > 0)
        {
            DataTable groupedTable = UspPrintSaleForceDSR.AsEnumerable()
    .GroupBy(r => r.Field<string>("DISTRIBUTOR_NAME"))
    .Select(g =>
    {
        DataRow row = UspPrintSaleForceDSR.NewRow();
        row["DISTRIBUTOR_NAME"] = g.Key;
        row["GROSS_AMOUNT"] = g.Sum(x => x.Field<decimal?>("GROSS_AMOUNT") ?? 0);
        row["DISCOUNT"] = g.Sum(x => x.Field<decimal?>("DISCOUNT") ?? 0);
        row["GST_AMOUNT"] = g.Sum(x => x.Field<decimal?>("GST_AMOUNT") ?? 0);
        row["SERVICE_CHARGES"] = g.Sum(x => x.Field<decimal?>("SERVICE_CHARGES") ?? 0);
        row["ThirdPartyDelivery"] = g.Sum(x => x.Field<decimal?>("ThirdPartyDelivery") ?? 0);
        row["CreditCardSales"] = g.Sum(x => x.Field<decimal?>("CreditCardSales") ?? 0);
        row["CreditSales"] = g.Sum(x => x.Field<decimal?>("CreditSales") ?? 0);
        row["CashInHand"] = g.Sum(x => x.Field<decimal?>("CashInHand") ?? 0);
        return row;
    })
    .CopyToDataTable();
                        
            ws.Cells[rowIndex, 1].Value = strHeader;

            rowIndex++;
            ws.Cells[rowIndex, 1].Value = strLocation;

            rowIndex++;
            ws.Cells[rowIndex, 1].Value = "From Date:";
            ws.Cells[rowIndex, 2].Value = dtFrom;
            ws.Cells[rowIndex, 2].Style.Numberformat.Format = "dd-MMM-yyyy";
            ws.Cells[rowIndex, 5].Value = "To Date:";
            ws.Cells[rowIndex, 6].Value = dtTo;
            ws.Cells[rowIndex, 6].Style.Numberformat.Format = "dd-MMM-yyyy";

            rowIndex++;
            int ColumnIndex = 0;
            foreach (var col in ColumnNames)
            {
                ColumnIndex++;
                ws.Cells[rowIndex, ColumnIndex].Value = col;
                ws.Cells[rowIndex, ColumnIndex].Style.Font.Bold = true;
                ws.Cells[rowIndex, ColumnIndex].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
            }
            decimal GrossAmount = 0;
            decimal Discount = 0;
            decimal GSTAmount = 0;
            decimal ServiceCharges = 0;
            decimal CreditCardSales = 0;
            decimal CreditSales = 0;
            decimal ThirdPartyDelivery = 0;
            decimal CashInHand = 0;

            decimal TotalGrossAmount = 0;
            decimal TotalDiscount = 0;
            decimal TotalExclusiveGST = 0;
            decimal TotalGSTAmount = 0;
            decimal TotalServiceCharges = 0;
            decimal TotalInclusiveGST = 0;
            decimal TotalThirdPartyDelivery = 0;
            decimal TotalCreditCardSales = 0;
            decimal TotalCreditSales = 0;
            decimal TotalCashSales = 0;
            foreach (DataRow DataTableRow in groupedTable.Rows)
            {
                rowIndex++;
                foreach (DataColumn DataTableColumn in groupedTable.Columns)
                {
                    GrossAmount = 0;
                    Discount = 0;
                    GSTAmount = 0;
                    ServiceCharges = 0;
                    CreditCardSales = 0;
                    CreditSales = 0;
                    ThirdPartyDelivery = 0;
                    CashInHand = 0;
                    if (DataTableColumn.ColumnName == "DISTRIBUTOR_NAME")
                    {
                        var cell = ws.Cells[rowIndex, 1];
                        cell.Value = DataTableRow[DataTableColumn.ColumnName];
                        cell.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    }
                    else if (DataTableColumn.ColumnName == "GROSS_AMOUNT")
                    {
                        var cell = ws.Cells[rowIndex, 2];
                        cell.Value = DataTableRow[DataTableColumn.ColumnName];
                        cell.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    }
                    else if (DataTableColumn.ColumnName == "DISCOUNT")
                    {
                        var cell = ws.Cells[rowIndex, 3];
                        cell.Value = DataTableRow[DataTableColumn.ColumnName];
                        cell.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    }
                    else if (DataTableColumn.ColumnName == "GST_AMOUNT")
                    {
                        var cell = ws.Cells[rowIndex, 5];
                        cell.Value = DataTableRow[DataTableColumn.ColumnName];
                        cell.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    }
                    else if (DataTableColumn.ColumnName == "SERVICE_CHARGES")
                    {
                        var cell = ws.Cells[rowIndex, 6];
                        cell.Value = DataTableRow[DataTableColumn.ColumnName];
                        cell.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    }
                    else if (DataTableColumn.ColumnName == "ThirdPartyDelivery")
                    {
                        var cell = ws.Cells[rowIndex, 8];
                        cell.Value = DataTableRow[DataTableColumn.ColumnName];
                        cell.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    }
                    else if (DataTableColumn.ColumnName == "CreditCardSales")
                    {
                        var cell = ws.Cells[rowIndex, 9];
                        cell.Value = DataTableRow[DataTableColumn.ColumnName];
                        cell.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    }
                    else if (DataTableColumn.ColumnName == "CreditSales")
                    {
                        var cell = ws.Cells[rowIndex, 10];
                        cell.Value = DataTableRow[DataTableColumn.ColumnName];
                        cell.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    }
                    try
                    {
                        GrossAmount = Convert.ToDecimal(DataTableRow["GROSS_AMOUNT"]);
                    }
                    catch (Exception)
                    {
                        GrossAmount = 0;
                    }
                    try
                    {
                        Discount = Convert.ToDecimal(DataTableRow["DISCOUNT"]);
                    }
                    catch (Exception)
                    {
                        Discount = 0;
                    }
                    try
                    {
                        GSTAmount = Convert.ToDecimal(DataTableRow["GST_AMOUNT"]);
                    }
                    catch (Exception)
                    {
                        GSTAmount = 0;
                    }
                    try
                    {
                        ServiceCharges = Convert.ToDecimal(DataTableRow["SERVICE_CHARGES"]);
                    }
                    catch (Exception)
                    {
                        ServiceCharges = 0;
                    }
                    try
                    {
                        CreditCardSales = Convert.ToDecimal(DataTableRow["CreditCardSales"]);
                    }
                    catch (Exception)
                    {
                        CreditCardSales = 0;
                    }
                    try
                    {
                        CreditSales = Convert.ToDecimal(DataTableRow["CreditSales"]);
                    }
                    catch (Exception)
                    {
                        CreditSales = 0;
                    }
                    try
                    {
                        ThirdPartyDelivery = Convert.ToDecimal(DataTableRow["ThirdPartyDelivery"]);
                    }
                    catch (Exception)
                    {
                        ThirdPartyDelivery = 0;
                    }
                    try
                    {
                        CashInHand = Convert.ToDecimal(DataTableRow["CashInHand"]);
                    }
                    catch (Exception)
                    {
                        CashInHand = 0;
                    }
                    var cell4 = ws.Cells[rowIndex, 4];
                    cell4.Value = GrossAmount - Discount;
                    cell4.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

                    var cell7 = ws.Cells[rowIndex, 7];
                    cell7.Value = GrossAmount - Discount + GSTAmount + ServiceCharges;
                    cell7.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

                    var cell11 = ws.Cells[rowIndex, 11];
                    cell11.Value = Convert.ToDecimal(cell7.Value) - CreditCardSales - CreditSales - ThirdPartyDelivery - CashInHand;
                    cell11.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                }
                TotalGrossAmount += GrossAmount;
                TotalDiscount += Discount;
                TotalExclusiveGST += GrossAmount - Discount;
                TotalGSTAmount += GSTAmount;
                TotalServiceCharges += ServiceCharges;
                TotalInclusiveGST += GrossAmount - Discount + GSTAmount + ServiceCharges;
                TotalThirdPartyDelivery += ThirdPartyDelivery;
                TotalCreditCardSales += CreditCardSales;
                TotalCreditSales += CreditSales;
                TotalCashSales += (GrossAmount - Discount + GSTAmount + ServiceCharges) - CreditCardSales - CreditSales - ThirdPartyDelivery - CashInHand;
            }

            rowIndex++;
            ws.Cells[rowIndex, 1].Value = "Total";
            ws.Cells[rowIndex, 1].Style.Font.Bold = true;
            ws.Cells[rowIndex, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

            ws.Cells[rowIndex, 2].Value = TotalGrossAmount;
            ws.Cells[rowIndex, 2].Style.Font.Bold = true;
            ws.Cells[rowIndex, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

            ws.Cells[rowIndex, 3].Value = TotalDiscount;
            ws.Cells[rowIndex, 3].Style.Font.Bold = true;
            ws.Cells[rowIndex, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

            ws.Cells[rowIndex, 4].Value = TotalExclusiveGST;
            ws.Cells[rowIndex, 4].Style.Font.Bold = true;
            ws.Cells[rowIndex, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

            ws.Cells[rowIndex, 5].Value = TotalGSTAmount;
            ws.Cells[rowIndex, 5].Style.Font.Bold = true;
            ws.Cells[rowIndex, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

            ws.Cells[rowIndex, 6].Value = TotalServiceCharges;
            ws.Cells[rowIndex, 6].Style.Font.Bold = true;
            ws.Cells[rowIndex, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

            ws.Cells[rowIndex, 7].Value = TotalInclusiveGST;
            ws.Cells[rowIndex, 7].Style.Font.Bold = true;
            ws.Cells[rowIndex, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

            ws.Cells[rowIndex, 8].Value = TotalThirdPartyDelivery;
            ws.Cells[rowIndex, 8].Style.Font.Bold = true;
            ws.Cells[rowIndex, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

            ws.Cells[rowIndex, 9].Value = TotalCreditCardSales;
            ws.Cells[rowIndex, 9].Style.Font.Bold = true;
            ws.Cells[rowIndex, 9].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

            ws.Cells[rowIndex, 10].Value = TotalCreditSales;
            ws.Cells[rowIndex, 10].Style.Font.Bold = true;
            ws.Cells[rowIndex, 10].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

            ws.Cells[rowIndex, 11].Value = TotalCashSales;
            ws.Cells[rowIndex, 11].Style.Font.Bold = true;
            ws.Cells[rowIndex, 11].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

        }
        #endregion

        #region Service Wise Covers
        if (uspGetServiceWiseCoverTables.Rows.Count > 0)
        {
            rowIndex++;
            rowIndex++;
            ws.Cells[rowIndex, 1].Value = "Service Type";
            ws.Cells[rowIndex, 1].Style.Font.Bold = true;
            ws.Cells[rowIndex, 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells[rowIndex, 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            ws.Cells[rowIndex, 1].Style.Border.Top.Color.SetColor(Color.Black);
            ws.Cells[rowIndex, 1].Style.Border.Bottom.Color.SetColor(Color.Black);

            ws.Cells[rowIndex, 2].Value = "No Of Invoices";
            ws.Cells[rowIndex, 2].Style.Font.Bold = true;
            ws.Cells[rowIndex, 2].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells[rowIndex, 2].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            ws.Cells[rowIndex, 2].Style.Border.Top.Color.SetColor(Color.Black);
            ws.Cells[rowIndex, 2].Style.Border.Bottom.Color.SetColor(Color.Black);

            ws.Cells[rowIndex, 3].Value = "Covers";
            ws.Cells[rowIndex, 3].Style.Font.Bold = true;
            ws.Cells[rowIndex, 3].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells[rowIndex, 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            ws.Cells[rowIndex, 3].Style.Border.Top.Color.SetColor(Color.Black);
            ws.Cells[rowIndex, 3].Style.Border.Bottom.Color.SetColor(Color.Black);

            ws.Cells[rowIndex, 4].Value = "Net Amount";
            ws.Cells[rowIndex, 4].Style.Font.Bold = true;
            ws.Cells[rowIndex, 4].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells[rowIndex, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            ws.Cells[rowIndex, 4].Style.Border.Top.Color.SetColor(Color.Black);
            ws.Cells[rowIndex, 4].Style.Border.Bottom.Color.SetColor(Color.Black);

            decimal TotalNoOfInvoices = 0;
            decimal TotalCovers = 0;
            decimal TotalNetSale = 0;
            decimal TotalNoOfDays = 0;
            foreach (DataRow DataTableRow in uspGetServiceWiseCoverTables.Rows)
            {
                rowIndex++;
                foreach (DataColumn DataTableColumn in uspGetServiceWiseCoverTables.Columns)
                {
                    if (DataTableColumn.ColumnName == "SERVICE_TYPE")
                    {
                        var cell = ws.Cells[rowIndex, 1];
                        cell.Value = DataTableRow[DataTableColumn.ColumnName];
                    }
                    else if (DataTableColumn.ColumnName == "NoOfInvoices")
                    {
                        var cell = ws.Cells[rowIndex, 2];
                        cell.Value = DataTableRow[DataTableColumn.ColumnName];
                    }
                    else if (DataTableColumn.ColumnName == "covertable")
                    {
                        var cell = ws.Cells[rowIndex, 3];
                        cell.Value = DataTableRow[DataTableColumn.ColumnName];
                    }
                    else if (DataTableColumn.ColumnName == "NET_SALE")
                    {
                        var cell = ws.Cells[rowIndex, 4];
                        cell.Value = DataTableRow[DataTableColumn.ColumnName];
                    }
                }
                try
                {
                    TotalNoOfInvoices += Convert.ToDecimal(DataTableRow["NoOfInvoices"]);
                }
                catch (Exception)
                {
                }
                try
                {
                    TotalCovers += Convert.ToDecimal(DataTableRow["covertable"]);
                }
                catch (Exception)
                {
                }
                try
                {
                    TotalNetSale += Convert.ToDecimal(DataTableRow["NET_SALE"]);
                }
                catch (Exception)
                {
                }
                try
                {
                    TotalNoOfDays = Convert.ToDecimal(DataTableRow["NoOfDays"]);
                }
                catch (Exception)
                {
                }
            }
            rowIndex++;
            ws.Cells[rowIndex, 1].Value = "Total";
            ws.Cells[rowIndex, 1].Style.Font.Bold = true;
            ws.Cells[rowIndex, 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells[rowIndex, 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            ws.Cells[rowIndex, 1].Style.Border.Top.Color.SetColor(Color.Black);
            ws.Cells[rowIndex, 1].Style.Border.Bottom.Color.SetColor(Color.Black);

            ws.Cells[rowIndex, 2].Value = TotalNoOfInvoices;
            ws.Cells[rowIndex, 2].Style.Font.Bold = true;
            ws.Cells[rowIndex, 2].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells[rowIndex, 2].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            ws.Cells[rowIndex, 2].Style.Border.Top.Color.SetColor(Color.Black);
            ws.Cells[rowIndex, 2].Style.Border.Bottom.Color.SetColor(Color.Black);

            ws.Cells[rowIndex, 3].Value = TotalCovers;
            ws.Cells[rowIndex, 3].Style.Font.Bold = true;
            ws.Cells[rowIndex, 3].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells[rowIndex, 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            ws.Cells[rowIndex, 3].Style.Border.Top.Color.SetColor(Color.Black);
            ws.Cells[rowIndex, 3].Style.Border.Bottom.Color.SetColor(Color.Black);

            ws.Cells[rowIndex, 4].Value = TotalNetSale;
            ws.Cells[rowIndex, 4].Style.Font.Bold = true;
            ws.Cells[rowIndex, 4].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells[rowIndex, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            ws.Cells[rowIndex, 4].Style.Border.Top.Color.SetColor(Color.Black);
            ws.Cells[rowIndex, 4].Style.Border.Bottom.Color.SetColor(Color.Black);

            decimal AverageSale = 0;
            decimal PerHeadSale = 0;

            if (TotalNoOfDays > 0)
            {
                AverageSale = TotalNetSale / TotalNoOfDays;
            }
            if (TotalCovers > 0)
            {
                PerHeadSale = TotalNetSale / TotalCovers;
            }
            rowIndex++;
            ws.Cells[rowIndex, 1].Value = "Month To Date Average Sale";
            ws.Cells[rowIndex, 4].Value = AverageSale;

            rowIndex++;
            ws.Cells[rowIndex, 1].Value = "Month To Date Per Head Sale";
            ws.Cells[rowIndex, 4].Value = PerHeadSale;
        }
        #endregion

        rowIndex++;
        var range = ws.Cells[rowIndex, 1, rowIndex, 11];

        range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        range.Style.Border.Bottom.Color.SetColor(Color.Black);

        #region Delivery Channel
        if (uspGetDailyDeliveryData.Rows.Count > 0)
        {
            rowIndex++;
            ws.Cells[rowIndex, 1].Value = "Delivery Channel";
            ws.Cells[rowIndex, 1].Style.Font.Bold = true;
            ws.Cells[rowIndex, 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            ws.Cells[rowIndex, 1].Style.Border.Bottom.Color.SetColor(Color.Black);

            ws.Cells[rowIndex, 3].Value = "No Of Invoices";
            ws.Cells[rowIndex, 3].Style.Font.Bold = true;
            ws.Cells[rowIndex, 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            ws.Cells[rowIndex, 3].Style.Border.Bottom.Color.SetColor(Color.Black);

            ws.Cells[rowIndex, 4].Value = "Net Amount";
            ws.Cells[rowIndex, 4].Style.Font.Bold = true;
            ws.Cells[rowIndex, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            ws.Cells[rowIndex, 4].Style.Border.Bottom.Color.SetColor(Color.Black);

            decimal TotalNoOfInvoicesDelivery = 0;
            decimal TotalNetAmount = 0;

            foreach (DataRow DataTableRow in uspGetDailyDeliveryData.Rows)
            {
                rowIndex++;
                foreach (DataColumn DataTableColumn in uspGetDailyDeliveryData.Columns)
                {
                    if (DataTableColumn.ColumnName == "DeliveryChannel")
                    {
                        var cell = ws.Cells[rowIndex, 1];
                        cell.Value = DataTableRow[DataTableColumn.ColumnName];
                    }
                    else if (DataTableColumn.ColumnName == "NoOfInvoices")
                    {
                        var cell = ws.Cells[rowIndex, 3];
                        cell.Value = DataTableRow[DataTableColumn.ColumnName];
                    }
                    else if (DataTableColumn.ColumnName == "NetAmount")
                    {
                        var cell = ws.Cells[rowIndex, 4];
                        cell.Value = DataTableRow[DataTableColumn.ColumnName];
                    }
                }
                try
                {
                    TotalNoOfInvoicesDelivery += Convert.ToDecimal(DataTableRow["NoOfInvoices"]);
                }
                catch (Exception)
                {
                }
                try
                {
                    TotalNetAmount += Convert.ToDecimal(DataTableRow["NetAmount"]);
                }
                catch (Exception)
                {
                }
            }
            rowIndex++;
            ws.Cells[rowIndex, 1].Value = "Total Delivery";
            ws.Cells[rowIndex, 1].Style.Font.Bold = true;
            ws.Cells[rowIndex, 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells[rowIndex, 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            ws.Cells[rowIndex, 1].Style.Border.Top.Color.SetColor(Color.Black);
            ws.Cells[rowIndex, 1].Style.Border.Bottom.Color.SetColor(Color.Black);

            ws.Cells[rowIndex, 3].Value = TotalNoOfInvoicesDelivery;
            ws.Cells[rowIndex, 3].Style.Font.Bold = true;
            ws.Cells[rowIndex, 3].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells[rowIndex, 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            ws.Cells[rowIndex, 3].Style.Border.Top.Color.SetColor(Color.Black);
            ws.Cells[rowIndex, 3].Style.Border.Bottom.Color.SetColor(Color.Black);

            ws.Cells[rowIndex, 4].Value = TotalNetAmount;
            ws.Cells[rowIndex, 4].Style.Font.Bold = true;
            ws.Cells[rowIndex, 4].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells[rowIndex, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            ws.Cells[rowIndex, 4].Style.Border.Top.Color.SetColor(Color.Black);
            ws.Cells[rowIndex, 4].Style.Border.Bottom.Color.SetColor(Color.Black);
        }
        #endregion

        #region Customer Advance
        if (RptVoucherView.Rows.Count > 0)
        {
            rowIndex++;
            ws.Cells[rowIndex, 1].Value = "Customer Advance";
            ws.Cells[rowIndex, 1].Style.Font.Bold = true;
            ws.Cells[rowIndex, 1].Style.Font.Color.SetColor(Color.Pink);

            rowIndex++;
            ws.Cells[rowIndex, 1].Value = "Location";
            ws.Cells[rowIndex, 1].Style.Font.Bold = true;
            ws.Cells[rowIndex, 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells[rowIndex, 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            ws.Cells[rowIndex, 1].Style.Border.Top.Color.SetColor(Color.Black);
            ws.Cells[rowIndex, 1].Style.Border.Bottom.Color.SetColor(Color.Black);

            ws.Cells[rowIndex, 2].Value = "Customer";
            ws.Cells[rowIndex, 2].Style.Font.Bold = true;
            ws.Cells[rowIndex, 2].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells[rowIndex, 2].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            ws.Cells[rowIndex, 2].Style.Border.Top.Color.SetColor(Color.Black);
            ws.Cells[rowIndex, 2].Style.Border.Bottom.Color.SetColor(Color.Black);

            ws.Cells[rowIndex, 3].Value = "Payment Mode";
            ws.Cells[rowIndex, 3].Style.Font.Bold = true;
            ws.Cells[rowIndex, 3].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells[rowIndex, 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            ws.Cells[rowIndex, 3].Style.Border.Top.Color.SetColor(Color.Black);
            ws.Cells[rowIndex, 3].Style.Border.Bottom.Color.SetColor(Color.Black);

            ws.Cells[rowIndex, 4].Value = "Advance Amount";
            ws.Cells[rowIndex, 4].Style.Font.Bold = true;
            ws.Cells[rowIndex, 4].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells[rowIndex, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            ws.Cells[rowIndex, 4].Style.Border.Top.Color.SetColor(Color.Black);
            ws.Cells[rowIndex, 4].Style.Border.Bottom.Color.SetColor(Color.Black);

            decimal TotalAdvanceAmount = 0;
            foreach (DataRow DataTableRow in RptVoucherView.Rows)
            {
                rowIndex++;
                foreach (DataColumn DataTableColumn in RptVoucherView.Columns)
                {
                    if (DataTableColumn.ColumnName == "ACCOUNT_NAME")
                    {
                        var cell = ws.Cells[rowIndex, 1];
                        cell.Value = DataTableRow[DataTableColumn.ColumnName];
                        cell.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        cell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        cell.Style.Border.Top.Color.SetColor(Color.Black);
                        cell.Style.Border.Bottom.Color.SetColor(Color.Black);
                    }
                    else if (DataTableColumn.ColumnName == "PAYEES_NAME")
                    {
                        var cell = ws.Cells[rowIndex, 2];
                        cell.Value = DataTableRow[DataTableColumn.ColumnName];
                        cell.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        cell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        cell.Style.Border.Top.Color.SetColor(Color.Black);
                        cell.Style.Border.Bottom.Color.SetColor(Color.Black);
                    }
                    else if (DataTableColumn.ColumnName == "PayMode")
                    {
                        var cell = ws.Cells[rowIndex, 3];
                        cell.Value = DataTableRow[DataTableColumn.ColumnName];
                        cell.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        cell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        cell.Style.Border.Top.Color.SetColor(Color.Black);
                        cell.Style.Border.Bottom.Color.SetColor(Color.Black);
                    }
                    else if (DataTableColumn.ColumnName == "DEBIT")
                    {
                        var cell = ws.Cells[rowIndex, 4];
                        cell.Value = DataTableRow[DataTableColumn.ColumnName];
                        cell.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        cell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        cell.Style.Border.Top.Color.SetColor(Color.Black);
                        cell.Style.Border.Bottom.Color.SetColor(Color.Black);
                    }
                }

                try
                {
                    TotalAdvanceAmount += Convert.ToDecimal(DataTableRow["DEBIT"]);
                }
                catch (Exception)
                {
                }
            }

            rowIndex++;
            ws.Cells[rowIndex, 3].Value = "Total";
            ws.Cells[rowIndex, 3].Style.Font.Bold = true;

            ws.Cells[rowIndex, 4].Value = TotalAdvanceAmount;
            ws.Cells[rowIndex, 4].Style.Font.Bold = true;
        }
        #endregion

        #region Tax Breakup
        if (dtDailySalesGSTBreakup.Rows.Count > 0)
        {
            rowIndex++;
            ws.Cells[rowIndex, 1].Value = "Sales Tax Breakup";
            ws.Cells[rowIndex, 1].Style.Font.Bold = true;
            ws.Cells[rowIndex, 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            ws.Cells[rowIndex, 1].Style.Border.Bottom.Color.SetColor(Color.Black);
            ws.Cells[rowIndex, 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells[rowIndex, 1].Style.Border.Top.Color.SetColor(Color.Black);

            ws.Cells[rowIndex, 2].Value = "Amount";
            ws.Cells[rowIndex, 2].Style.Font.Bold = true;
            ws.Cells[rowIndex, 2].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            ws.Cells[rowIndex, 2].Style.Border.Bottom.Color.SetColor(Color.Black);
            ws.Cells[rowIndex, 2].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells[rowIndex, 2].Style.Border.Top.Color.SetColor(Color.Black);

            decimal TotalGST = 0;
            foreach (DataRow DataTableRow in dtDailySalesGSTBreakup.Rows)
            {
                rowIndex++;
                foreach (DataColumn DataTableColumn in dtDailySalesGSTBreakup.Columns)
                {
                    if (DataTableColumn.ColumnName == "PaymentMode")
                    {
                        var cell = ws.Cells[rowIndex, 1];
                        cell.Value = DataTableRow[DataTableColumn.ColumnName];
                        cell.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        cell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        cell.Style.Border.Top.Color.SetColor(Color.Black);
                        cell.Style.Border.Bottom.Color.SetColor(Color.Black);
                    }
                    else if (DataTableColumn.ColumnName == "GST")
                    {
                        var cell = ws.Cells[rowIndex, 2];
                        cell.Value = DataTableRow[DataTableColumn.ColumnName];
                        cell.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        cell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        cell.Style.Border.Top.Color.SetColor(Color.Black);
                        cell.Style.Border.Bottom.Color.SetColor(Color.Black);
                    }
                }

                try
                {
                    TotalGST += Convert.ToDecimal(DataTableRow["GST"]);
                }
                catch (Exception)
                {
                }
            }

            rowIndex++;
            ws.Cells[rowIndex, 1].Value = "Total:";
            ws.Cells[rowIndex, 1].Style.Font.Bold = true;

            ws.Cells[rowIndex, 2].Value = TotalGST;
            ws.Cells[rowIndex, 2].Style.Font.Bold = true;
        }
        #endregion
    }

    public static void ExportToExcelDateWise(ExcelWorksheet ws, ExcelPackage p, string strHeader, string strLocation, DateTime dtFrom, DateTime dtTo, string[] ColumnNames, DataTable spPrintDailySalesReportDateWise, DataTable dtDailySalesGSTBreakup)
    {
        int rowIndex = 1;
        #region Main Report
        if (spPrintDailySalesReportDateWise.Rows.Count > 0)
        {
            DataTable groupedTable = spPrintDailySalesReportDateWise.AsEnumerable()
    .GroupBy(r => r.Field<DateTime>("DOCUMENT_DATE"))
    .Select(g =>
    {
        DataRow row = spPrintDailySalesReportDateWise.NewRow();
        row["DOCUMENT_DATE"] = g.Key;
        row["AMOUNTDUE"] = g.Sum(x => x.Field<decimal?>("AMOUNTDUE") ?? 0);
        row["DISCOUNT"] = g.Sum(x => x.Field<decimal?>("DISCOUNT") ?? 0);
        row["GST"] = g.Sum(x => x.Field<decimal?>("GST") ?? 0);
        row["SERVICE_CHARGES"] = g.Sum(x => x.Field<decimal?>("SERVICE_CHARGES") ?? 0);
        row["ThirdPartyDelivery"] = g.Sum(x => x.Field<decimal?>("ThirdPartyDelivery") ?? 0);
        row["CreditCardSales"] = g.Sum(x => x.Field<decimal?>("CreditCardSales") ?? 0);
        row["CreditSales"] = g.Sum(x => x.Field<decimal?>("CreditSales") ?? 0);
        row["PAIDIN"] = g.Sum(x => x.Field<decimal?>("PAIDIN") ?? 0);
        return row;
    })
    .CopyToDataTable();

            ws.Cells[rowIndex, 1].Value = strHeader;

            rowIndex++;
            ws.Cells[rowIndex, 1].Value = strLocation;

            rowIndex++;
            ws.Cells[rowIndex, 1].Value = "From Date:";
            ws.Cells[rowIndex, 2].Value = dtFrom;
            ws.Cells[rowIndex, 2].Style.Numberformat.Format = "dd-MMM-yyyy";
            ws.Cells[rowIndex, 5].Value = "To Date:";
            ws.Cells[rowIndex, 6].Value = dtTo;
            ws.Cells[rowIndex, 6].Style.Numberformat.Format = "dd-MMM-yyyy";

            rowIndex++;
            int ColumnIndex = 0;
            foreach (var col in ColumnNames)
            {
                ColumnIndex++;
                ws.Cells[rowIndex, ColumnIndex].Value = col;
                ws.Cells[rowIndex, ColumnIndex].Style.Font.Bold = true;
                ws.Cells[rowIndex, ColumnIndex].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
            }
            decimal GrossAmount = 0;
            decimal Discount = 0;
            decimal GSTAmount = 0;
            decimal ServiceCharges = 0;
            decimal CreditCardSales = 0;
            decimal CreditSales = 0;
            decimal ThirdPartyDelivery = 0;
            decimal PAIDIN = 0;

            decimal TotalGrossAmount = 0;
            decimal TotalDiscount = 0;
            decimal TotalExclusiveGST = 0;
            decimal TotalGSTAmount = 0;
            decimal TotalServiceCharges = 0;
            decimal TotalInclusiveGST = 0;
            decimal TotalThirdPartyDelivery = 0;
            decimal TotalCreditCardSales = 0;
            decimal TotalCreditSales = 0;
            decimal TotalCashSales = 0;
            foreach (DataRow DataTableRow in groupedTable.Rows)
            {
                rowIndex++;
                foreach (DataColumn DataTableColumn in groupedTable.Columns)
                {
                    GrossAmount = 0;
                    Discount = 0;
                    GSTAmount = 0;
                    ServiceCharges = 0;
                    CreditCardSales = 0;
                    CreditSales = 0;
                    ThirdPartyDelivery = 0;
                    PAIDIN = 0;
                    if (DataTableColumn.ColumnName == "DOCUMENT_DATE")
                    {
                        var cell = ws.Cells[rowIndex, 1];
                        cell.Value = DataTableRow[DataTableColumn.ColumnName];
                        cell.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                        cell.Style.Numberformat.Format = "dd-MMM-yyyy";
                    }
                    else if (DataTableColumn.ColumnName == "AMOUNTDUE")
                    {
                        var cell = ws.Cells[rowIndex, 2];
                        cell.Value = DataTableRow[DataTableColumn.ColumnName];
                        cell.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    }
                    else if (DataTableColumn.ColumnName == "DISCOUNT")
                    {
                        var cell = ws.Cells[rowIndex, 3];
                        cell.Value = DataTableRow[DataTableColumn.ColumnName];
                        cell.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    }
                    else if (DataTableColumn.ColumnName == "GST")
                    {
                        var cell = ws.Cells[rowIndex, 4];
                        cell.Value = DataTableRow[DataTableColumn.ColumnName];
                        cell.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    }
                    else if (DataTableColumn.ColumnName == "SERVICE_CHARGES")
                    {
                        var cell = ws.Cells[rowIndex, 5];
                        cell.Value = DataTableRow[DataTableColumn.ColumnName];
                        cell.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    }
                    else if (DataTableColumn.ColumnName == "ThirdPartyDelivery")
                    {
                        var cell = ws.Cells[rowIndex, 7];
                        cell.Value = DataTableRow[DataTableColumn.ColumnName];
                        cell.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    }
                    else if (DataTableColumn.ColumnName == "CreditCardSales")
                    {
                        var cell = ws.Cells[rowIndex, 8];
                        cell.Value = DataTableRow[DataTableColumn.ColumnName];
                        cell.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    }
                    else if (DataTableColumn.ColumnName == "CreditSales")
                    {
                        var cell = ws.Cells[rowIndex, 9];
                        cell.Value = DataTableRow[DataTableColumn.ColumnName];
                        cell.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    }
                    try
                    {
                        GrossAmount = Convert.ToDecimal(DataTableRow["AMOUNTDUE"]);
                    }
                    catch (Exception)
                    {
                        GrossAmount = 0;
                    }
                    try
                    {
                        Discount = Convert.ToDecimal(DataTableRow["DISCOUNT"]);
                    }
                    catch (Exception)
                    {
                        Discount = 0;
                    }
                    try
                    {
                        GSTAmount = Convert.ToDecimal(DataTableRow["GST"]);
                    }
                    catch (Exception)
                    {
                        GSTAmount = 0;
                    }
                    try
                    {
                        ServiceCharges = Convert.ToDecimal(DataTableRow["SERVICE_CHARGES"]);
                    }
                    catch (Exception)
                    {
                        ServiceCharges = 0;
                    }
                    try
                    {
                        CreditCardSales = Convert.ToDecimal(DataTableRow["CreditCardSales"]);
                    }
                    catch (Exception)
                    {
                        CreditCardSales = 0;
                    }
                    try
                    {
                        CreditSales = Convert.ToDecimal(DataTableRow["CreditSales"]);
                    }
                    catch (Exception)
                    {
                        CreditSales = 0;
                    }
                    try
                    {
                        ThirdPartyDelivery = Convert.ToDecimal(DataTableRow["ThirdPartyDelivery"]);
                    }
                    catch (Exception)
                    {
                        ThirdPartyDelivery = 0;
                    }

                    try
                    {
                        PAIDIN = Convert.ToDecimal(DataTableRow["PAIDIN"]);
                    }
                    catch (Exception)
                    {
                        PAIDIN = 0;
                    }

                    var cell6 = ws.Cells[rowIndex, 6];
                    cell6.Value = GrossAmount - Discount + GSTAmount + ServiceCharges;
                    cell6.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

                    var cell10 = ws.Cells[rowIndex, 10];
                    cell10.Value = Convert.ToDecimal(cell6.Value) - CreditCardSales - CreditSales - ThirdPartyDelivery - PAIDIN;
                    cell10.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                }
                TotalGrossAmount += GrossAmount;
                TotalDiscount += Discount;
                TotalExclusiveGST += GrossAmount - Discount;
                TotalGSTAmount += GSTAmount;
                TotalServiceCharges += ServiceCharges;
                TotalInclusiveGST += GrossAmount - Discount + GSTAmount + ServiceCharges;
                TotalThirdPartyDelivery += ThirdPartyDelivery;
                TotalCreditCardSales += CreditCardSales;
                TotalCreditSales += CreditSales;
                TotalCashSales += (GrossAmount - Discount + GSTAmount + ServiceCharges) - CreditCardSales - CreditSales - ThirdPartyDelivery - PAIDIN;
            }

            rowIndex++;
            ws.Cells[rowIndex, 1].Value = "Total";
            ws.Cells[rowIndex, 1].Style.Font.Bold = true;
            ws.Cells[rowIndex, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

            ws.Cells[rowIndex, 2].Value = TotalGrossAmount;
            ws.Cells[rowIndex, 2].Style.Font.Bold = true;
            ws.Cells[rowIndex, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

            ws.Cells[rowIndex, 3].Value = TotalDiscount;
            ws.Cells[rowIndex, 3].Style.Font.Bold = true;
            ws.Cells[rowIndex, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

            ws.Cells[rowIndex, 4].Value = TotalGSTAmount;
            ws.Cells[rowIndex, 4].Style.Font.Bold = true;
            ws.Cells[rowIndex, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

            ws.Cells[rowIndex, 5].Value = TotalServiceCharges;
            ws.Cells[rowIndex, 5].Style.Font.Bold = true;
            ws.Cells[rowIndex, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

            ws.Cells[rowIndex, 6].Value = TotalInclusiveGST;
            ws.Cells[rowIndex, 6].Style.Font.Bold = true;
            ws.Cells[rowIndex, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

            ws.Cells[rowIndex, 7].Value = TotalThirdPartyDelivery;
            ws.Cells[rowIndex, 7].Style.Font.Bold = true;
            ws.Cells[rowIndex, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

            ws.Cells[rowIndex, 8].Value = TotalCreditCardSales;
            ws.Cells[rowIndex, 8].Style.Font.Bold = true;
            ws.Cells[rowIndex, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

            ws.Cells[rowIndex, 9].Value = TotalCreditSales;
            ws.Cells[rowIndex, 9].Style.Font.Bold = true;
            ws.Cells[rowIndex, 9].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

            ws.Cells[rowIndex, 10].Value = TotalCashSales;
            ws.Cells[rowIndex, 10].Style.Font.Bold = true;
            ws.Cells[rowIndex, 10].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        }
        #endregion

        #region Tax Breakup
        if (dtDailySalesGSTBreakup.Rows.Count > 0)
        {
            rowIndex++;
            ws.Cells[rowIndex, 1].Value = "Sales Tax Breakup";
            ws.Cells[rowIndex, 1].Style.Font.Bold = true;
            ws.Cells[rowIndex, 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            ws.Cells[rowIndex, 1].Style.Border.Bottom.Color.SetColor(Color.Black);
            ws.Cells[rowIndex, 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells[rowIndex, 1].Style.Border.Top.Color.SetColor(Color.Black);

            ws.Cells[rowIndex, 2].Value = "Amount";
            ws.Cells[rowIndex, 2].Style.Font.Bold = true;
            ws.Cells[rowIndex, 2].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            ws.Cells[rowIndex, 2].Style.Border.Bottom.Color.SetColor(Color.Black);
            ws.Cells[rowIndex, 2].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells[rowIndex, 2].Style.Border.Top.Color.SetColor(Color.Black);

            decimal TotalGST = 0;
            foreach (DataRow DataTableRow in dtDailySalesGSTBreakup.Rows)
            {
                rowIndex++;
                foreach (DataColumn DataTableColumn in dtDailySalesGSTBreakup.Columns)
                {
                    if (DataTableColumn.ColumnName == "PaymentMode")
                    {
                        var cell = ws.Cells[rowIndex, 1];
                        cell.Value = DataTableRow[DataTableColumn.ColumnName];
                        cell.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        cell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        cell.Style.Border.Top.Color.SetColor(Color.Black);
                        cell.Style.Border.Bottom.Color.SetColor(Color.Black);
                    }
                    else if (DataTableColumn.ColumnName == "GST")
                    {
                        var cell = ws.Cells[rowIndex, 2];
                        cell.Value = DataTableRow[DataTableColumn.ColumnName];
                        cell.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        cell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        cell.Style.Border.Top.Color.SetColor(Color.Black);
                        cell.Style.Border.Bottom.Color.SetColor(Color.Black);
                    }
                }

                try
                {
                    TotalGST += Convert.ToDecimal(DataTableRow["GST"]);
                }
                catch (Exception)
                {
                }
            }

            rowIndex++;
            ws.Cells[rowIndex, 1].Value = "Total:";
            ws.Cells[rowIndex, 1].Style.Font.Bold = true;

            ws.Cells[rowIndex, 2].Value = TotalGST;
            ws.Cells[rowIndex, 2].Style.Font.Bold = true;
        }
        #endregion
    }

    public static void ExportToExcelCashierWise(ExcelWorksheet ws, ExcelPackage p, string strHeader, string strLocation, DateTime dtFrom, DateTime dtTo, string[] ColumnNames, DataTable UspPrintSaleForceDSR, DataTable dtDailySalesGSTBreakup)
    {
        int rowIndex = 1;
        #region Main Report
        if (UspPrintSaleForceDSR.Rows.Count > 0)
        {
            DataTable groupedTable = UspPrintSaleForceDSR.AsEnumerable()
    .GroupBy(r => r.Field<string>("DISTRIBUTOR_NAME"))
    .Select(g =>
    {
        DataRow row = UspPrintSaleForceDSR.NewRow();
        row["DISTRIBUTOR_NAME"] = g.Key;
        row["GROSS_AMOUNT"] = g.Sum(x => x.Field<decimal?>("GROSS_AMOUNT") ?? 0);
        row["DISCOUNT"] = g.Sum(x => x.Field<decimal?>("DISCOUNT") ?? 0);
        row["GST_AMOUNT"] = g.Sum(x => x.Field<decimal?>("GST_AMOUNT") ?? 0);
        row["SERVICE_CHARGES"] = g.Sum(x => x.Field<decimal?>("SERVICE_CHARGES") ?? 0);
        row["ThirdPartyDelivery"] = g.Sum(x => x.Field<decimal?>("ThirdPartyDelivery") ?? 0);
        row["CreditCardSales"] = g.Sum(x => x.Field<decimal?>("CreditCardSales") ?? 0);
        row["CreditSales"] = g.Sum(x => x.Field<decimal?>("CreditSales") ?? 0);
        row["CashInHand"] = g.Sum(x => x.Field<decimal?>("CashInHand") ?? 0);
        row["TOTAL_AMOUNT"] = g.Sum(x => x.Field<decimal?>("TOTAL_AMOUNT") ?? 0);
        row["NET_TOTAL"] = g.Sum(x => x.Field<decimal?>("NET_TOTAL") ?? 0);
        return row;
    })
    .CopyToDataTable();

            ws.Cells[rowIndex, 1].Value = strHeader;

            rowIndex++;
            ws.Cells[rowIndex, 1].Value = strLocation;

            rowIndex++;
            ws.Cells[rowIndex, 1].Value = "From Date:";
            ws.Cells[rowIndex, 2].Value = dtFrom;
            ws.Cells[rowIndex, 2].Style.Numberformat.Format = "dd-MMM-yyyy";
            ws.Cells[rowIndex, 5].Value = "To Date:";
            ws.Cells[rowIndex, 6].Value = dtTo;
            ws.Cells[rowIndex, 6].Style.Numberformat.Format = "dd-MMM-yyyy";

            rowIndex++;
            int ColumnIndex = 0;
            foreach (var col in ColumnNames)
            {
                ColumnIndex++;
                ws.Cells[rowIndex, ColumnIndex].Value = col;
                ws.Cells[rowIndex, ColumnIndex].Style.Font.Bold = true;
                ws.Cells[rowIndex, ColumnIndex].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
            }
            decimal GrossAmount = 0;
            decimal Discount = 0;
            decimal GSTAmount = 0;
            decimal ServiceCharges = 0;
            decimal CreditCardSales = 0;
            decimal CreditSales = 0;
            decimal ThirdPartyDelivery = 0;
            decimal CashInHand = 0;
            decimal TotalAmount = 0;
            decimal NetAmount = 0;

            decimal TotalGrossAmount = 0;
            decimal TotalDiscount = 0;
            decimal TotalExclusiveGST = 0;
            decimal TotalGSTAmount = 0;
            decimal TotalServiceCharges = 0;
            decimal TotalInclusiveGST = 0;
            decimal TotalThirdPartyDelivery = 0;
            decimal TotalCreditCardSales = 0;
            decimal TotalCreditSales = 0;
            decimal TotalCashSales = 0;
            decimal TotalTotalAmount = 0;
            decimal TotalNetAmount = 0;
            foreach (DataRow DataTableRow in groupedTable.Rows)
            {
                rowIndex++;
                foreach (DataColumn DataTableColumn in groupedTable.Columns)
                {
                    GrossAmount = 0;
                    Discount = 0;
                    GSTAmount = 0;
                    ServiceCharges = 0;
                    CreditCardSales = 0;
                    CreditSales = 0;
                    ThirdPartyDelivery = 0;
                    CashInHand = 0;
                    TotalAmount = 0;
                    NetAmount = 0;
                    if (DataTableColumn.ColumnName == "DISTRIBUTOR_NAME")
                    {
                        var cell = ws.Cells[rowIndex, 1];
                        cell.Value = DataTableRow[DataTableColumn.ColumnName];
                        cell.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    }
                    else if (DataTableColumn.ColumnName == "GROSS_AMOUNT")
                    {
                        var cell = ws.Cells[rowIndex, 2];
                        cell.Value = DataTableRow[DataTableColumn.ColumnName];
                        cell.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    }
                    else if (DataTableColumn.ColumnName == "DISCOUNT")
                    {
                        var cell = ws.Cells[rowIndex, 3];
                        cell.Value = DataTableRow[DataTableColumn.ColumnName];
                        cell.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    }
                    else if (DataTableColumn.ColumnName == "GST_AMOUNT")
                    {
                        var cell = ws.Cells[rowIndex, 5];
                        cell.Value = DataTableRow[DataTableColumn.ColumnName];
                        cell.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    }
                    else if (DataTableColumn.ColumnName == "SERVICE_CHARGES")
                    {
                        var cell = ws.Cells[rowIndex, 6];
                        cell.Value = DataTableRow[DataTableColumn.ColumnName];
                        cell.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    }
                    else if (DataTableColumn.ColumnName == "ThirdPartyDelivery")
                    {
                        var cell = ws.Cells[rowIndex, 8];
                        cell.Value = DataTableRow[DataTableColumn.ColumnName];
                        cell.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    }
                    else if (DataTableColumn.ColumnName == "CreditCardSales")
                    {
                        var cell = ws.Cells[rowIndex, 9];
                        cell.Value = DataTableRow[DataTableColumn.ColumnName];
                        cell.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    }
                    else if (DataTableColumn.ColumnName == "CreditSales")
                    {
                        var cell = ws.Cells[rowIndex, 10];
                        cell.Value = DataTableRow[DataTableColumn.ColumnName];
                        cell.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    }
                    else if (DataTableColumn.ColumnName == "TOTAL_AMOUNT")
                    {
                        var cell = ws.Cells[rowIndex, 12];
                        cell.Value = DataTableRow[DataTableColumn.ColumnName];
                        cell.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    }
                    else if (DataTableColumn.ColumnName == "NET_TOTAL")
                    {
                        var cell = ws.Cells[rowIndex, 13];
                        cell.Value = DataTableRow[DataTableColumn.ColumnName];
                        cell.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    }
                    try
                    {
                        GrossAmount = Convert.ToDecimal(DataTableRow["GROSS_AMOUNT"]);
                    }
                    catch (Exception)
                    {
                        GrossAmount = 0;
                    }
                    try
                    {
                        Discount = Convert.ToDecimal(DataTableRow["DISCOUNT"]);
                    }
                    catch (Exception)
                    {
                        Discount = 0;
                    }
                    try
                    {
                        GSTAmount = Convert.ToDecimal(DataTableRow["GST_AMOUNT"]);
                    }
                    catch (Exception)
                    {
                        GSTAmount = 0;
                    }
                    try
                    {
                        ServiceCharges = Convert.ToDecimal(DataTableRow["SERVICE_CHARGES"]);
                    }
                    catch (Exception)
                    {
                        ServiceCharges = 0;
                    }
                    try
                    {
                        CreditCardSales = Convert.ToDecimal(DataTableRow["CreditCardSales"]);
                    }
                    catch (Exception)
                    {
                        CreditCardSales = 0;
                    }
                    try
                    {
                        CreditSales = Convert.ToDecimal(DataTableRow["CreditSales"]);
                    }
                    catch (Exception)
                    {
                        CreditSales = 0;
                    }
                    try
                    {
                        ThirdPartyDelivery = Convert.ToDecimal(DataTableRow["ThirdPartyDelivery"]);
                    }
                    catch (Exception)
                    {
                        ThirdPartyDelivery = 0;
                    }
                    try
                    {
                        CashInHand = Convert.ToDecimal(DataTableRow["CashInHand"]);
                    }
                    catch (Exception)
                    {
                        CashInHand = 0;
                    }
                    try
                    {
                        TotalAmount = Convert.ToDecimal(DataTableRow["TOTAL_AMOUNT"]);
                    }
                    catch (Exception)
                    {
                        TotalAmount = 0;
                    }
                    try
                    {
                        NetAmount = Convert.ToDecimal(DataTableRow["NET_TOTAL"]);
                    }
                    catch (Exception)
                    {
                        NetAmount = 0;
                    }

                    var cell4 = ws.Cells[rowIndex, 4];
                    cell4.Value = GrossAmount - Discount;
                    cell4.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

                    var cell7 = ws.Cells[rowIndex, 7];
                    cell7.Value = GrossAmount - Discount + GSTAmount + ServiceCharges;
                    cell7.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

                    var cell11 = ws.Cells[rowIndex, 11];
                    cell11.Value = Convert.ToDecimal(cell7.Value) - CreditCardSales - CreditSales - ThirdPartyDelivery - CashInHand;
                    cell11.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

                    var cell14 = ws.Cells[rowIndex, 14];
                    cell14.Value = (Convert.ToDecimal(cell7.Value) - CreditCardSales - CreditSales - ThirdPartyDelivery - CashInHand) - TotalAmount - NetAmount;
                    cell14.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                }
                TotalGrossAmount += GrossAmount;
                TotalDiscount += Discount;
                TotalExclusiveGST += GrossAmount - Discount;
                TotalGSTAmount += GSTAmount;
                TotalServiceCharges += ServiceCharges;
                TotalInclusiveGST += GrossAmount - Discount + GSTAmount + ServiceCharges;
                TotalThirdPartyDelivery += ThirdPartyDelivery;
                TotalCreditCardSales += CreditCardSales;
                TotalCreditSales += CreditSales;
                TotalCashSales += (GrossAmount - Discount + GSTAmount + ServiceCharges) - CreditCardSales - CreditSales - ThirdPartyDelivery - CashInHand;
                TotalTotalAmount += TotalAmount;
                TotalNetAmount += NetAmount;
            }

            rowIndex++;
            ws.Cells[rowIndex, 1].Value = "Total";
            ws.Cells[rowIndex, 1].Style.Font.Bold = true;
            ws.Cells[rowIndex, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

            ws.Cells[rowIndex, 2].Value = TotalGrossAmount;
            ws.Cells[rowIndex, 2].Style.Font.Bold = true;
            ws.Cells[rowIndex, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

            ws.Cells[rowIndex, 3].Value = TotalDiscount;
            ws.Cells[rowIndex, 3].Style.Font.Bold = true;
            ws.Cells[rowIndex, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

            ws.Cells[rowIndex, 4].Value = TotalExclusiveGST;
            ws.Cells[rowIndex, 4].Style.Font.Bold = true;
            ws.Cells[rowIndex, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

            ws.Cells[rowIndex, 5].Value = TotalGSTAmount;
            ws.Cells[rowIndex, 5].Style.Font.Bold = true;
            ws.Cells[rowIndex, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

            ws.Cells[rowIndex, 6].Value = TotalServiceCharges;
            ws.Cells[rowIndex, 6].Style.Font.Bold = true;
            ws.Cells[rowIndex, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

            ws.Cells[rowIndex, 7].Value = TotalInclusiveGST;
            ws.Cells[rowIndex, 7].Style.Font.Bold = true;
            ws.Cells[rowIndex, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

            ws.Cells[rowIndex, 8].Value = TotalThirdPartyDelivery;
            ws.Cells[rowIndex, 8].Style.Font.Bold = true;
            ws.Cells[rowIndex, 8].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

            ws.Cells[rowIndex, 9].Value = TotalCreditCardSales;
            ws.Cells[rowIndex, 9].Style.Font.Bold = true;
            ws.Cells[rowIndex, 9].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

            ws.Cells[rowIndex, 10].Value = TotalCreditSales;
            ws.Cells[rowIndex, 10].Style.Font.Bold = true;
            ws.Cells[rowIndex, 10].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

            ws.Cells[rowIndex, 11].Value = TotalCashSales;
            ws.Cells[rowIndex, 11].Style.Font.Bold = true;
            ws.Cells[rowIndex, 11].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

            ws.Cells[rowIndex, 12].Value = TotalTotalAmount;
            ws.Cells[rowIndex, 12].Style.Font.Bold = true;
            ws.Cells[rowIndex, 12].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

            ws.Cells[rowIndex, 13].Value = TotalNetAmount;
            ws.Cells[rowIndex, 13].Style.Font.Bold = true;
            ws.Cells[rowIndex, 13].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

            ws.Cells[rowIndex, 14].Value = TotalCashSales - TotalTotalAmount - TotalNetAmount;
            ws.Cells[rowIndex, 14].Style.Font.Bold = true;
            ws.Cells[rowIndex, 14].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        }
        #endregion

        #region Tax Breakup
        if (dtDailySalesGSTBreakup.Rows.Count > 0)
        {
            rowIndex++;
            ws.Cells[rowIndex, 1].Value = "Sales Tax Breakup";
            ws.Cells[rowIndex, 1].Style.Font.Bold = true;
            ws.Cells[rowIndex, 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            ws.Cells[rowIndex, 1].Style.Border.Bottom.Color.SetColor(Color.Black);
            ws.Cells[rowIndex, 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells[rowIndex, 1].Style.Border.Top.Color.SetColor(Color.Black);

            ws.Cells[rowIndex, 2].Value = "Amount";
            ws.Cells[rowIndex, 2].Style.Font.Bold = true;
            ws.Cells[rowIndex, 2].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            ws.Cells[rowIndex, 2].Style.Border.Bottom.Color.SetColor(Color.Black);
            ws.Cells[rowIndex, 2].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells[rowIndex, 2].Style.Border.Top.Color.SetColor(Color.Black);

            decimal TotalGST = 0;
            foreach (DataRow DataTableRow in dtDailySalesGSTBreakup.Rows)
            {
                rowIndex++;
                foreach (DataColumn DataTableColumn in dtDailySalesGSTBreakup.Columns)
                {
                    if (DataTableColumn.ColumnName == "PaymentMode")
                    {
                        var cell = ws.Cells[rowIndex, 1];
                        cell.Value = DataTableRow[DataTableColumn.ColumnName];
                        cell.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        cell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        cell.Style.Border.Top.Color.SetColor(Color.Black);
                        cell.Style.Border.Bottom.Color.SetColor(Color.Black);
                    }
                    else if (DataTableColumn.ColumnName == "GST")
                    {
                        var cell = ws.Cells[rowIndex, 2];
                        cell.Value = DataTableRow[DataTableColumn.ColumnName];
                        cell.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        cell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        cell.Style.Border.Top.Color.SetColor(Color.Black);
                        cell.Style.Border.Bottom.Color.SetColor(Color.Black);
                    }
                }

                try
                {
                    TotalGST += Convert.ToDecimal(DataTableRow["GST"]);
                }
                catch (Exception)
                {
                }
            }

            rowIndex++;
            ws.Cells[rowIndex, 1].Value = "Total:";
            ws.Cells[rowIndex, 1].Style.Font.Bold = true;

            ws.Cells[rowIndex, 2].Value = TotalGST;
            ws.Cells[rowIndex, 2].Style.Font.Bold = true;
        }
        #endregion
    }
}