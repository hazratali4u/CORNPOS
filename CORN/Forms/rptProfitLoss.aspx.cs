using System;
using System.Data;
using System.Web.UI;
using CORNBusinessLayer.Classes;
using CORNBusinessLayer.Reports;
using CORNCommon.Classes;
using System.Web;
using System.Web.UI.WebControls;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Linq;
using System.Collections.Generic;

public partial class Forms_rptProfitLoss : System.Web.UI.Page
{
    readonly DocumentPrintController _mDocumentPrntControl = new DocumentPrintController();
    readonly RptSaleController _rptSaleCtl = new RptSaleController();
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        Response.Cache.SetNoStore();
        Response.AppendHeader("pragma", "no-cache");
        if (!Page.IsPostBack)
        {
            LoadAssingned();
            Configuration.SystemCurrentDateTime = (DateTime)Session["CurrentWorkDate"];
            txtStartDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtEndDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtStartDate.Attributes.Add("readonly", "readonly");
            txtEndDate.Attributes.Add("readonly", "readonly");
        }
    }

    /// <summary>
    /// Loads User Assigned Locations To Location Combo
    /// </summary>
    private void LoadAssingned()
    {
        var mUserController = new UserController();
        DataTable dt = mUserController.SelectUserAssignment(int.Parse(Session["UserId"].ToString()), Constants.IntNullValue, 1, int.Parse(Session["CompanyId"].ToString()));
        clsWebFormUtil.FillListBox(ChbDistributorList, dt, 0, 1);
        foreach (ListItem li in ChbDistributorList.Items)
        {
            li.Selected = true;
        }
    }
    private void ShowReport(int reportType)
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
        DataSet ds = _rptSaleCtl.SelectRptGrossProfitCrossTab(location, Convert.ToInt32(Session["UserID"]), DateTime.Parse(txtStartDate.Text), DateTime.Parse(txtEndDate.Text));
        DataTable dt = _mDocumentPrntControl.SelectReportTitle(Constants.IntNullValue);

        var crpReport = new CrpGrossProfitReportCrossTab();
        crpReport.SetDataSource(ds);
        crpReport.Refresh();
        crpReport.SetParameterValue("Company_Name", dt.Rows[0]["COMPANY_NAME"].ToString());
        crpReport.SetParameterValue("From_Date", txtStartDate.Text);
        crpReport.SetParameterValue("To_date", txtEndDate.Text);
        crpReport.SetParameterValue("Location", locationName);

        Session.Add("CrpReport", crpReport);
        Session.Add("ReportType", reportType);
        const string url = "'Default.aspx'";
        const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
        Type cstype = GetType();
        ClientScriptManager cs = Page.ClientScript;
        cs.RegisterStartupScript(cstype, "OpenWindow", script);
    }
    protected void btnViewPDF_Click(object sender, EventArgs e)
    {
        ShowReport(0);
    }
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

        DataSet ds = _rptSaleCtl.SelectRptGrossProfitCrossTab(location, Convert.ToInt32(Session["UserID"]), DateTime.Parse(txtStartDate.Text), DateTime.Parse(txtEndDate.Text));
        DataTable dt = _mDocumentPrntControl.SelectReportTitle(Constants.IntNullValue);
        OfficeOpenXml.ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
        using (ExcelPackage p = new ExcelPackage())
        {
            ExcelWorksheet ws = p.Workbook.Worksheets.Add("Profit & Loss Statement");
            var headerCells = ws.Cells[1, 1, 1, ExcelPackage.MaxColumns];
            var headerFont = headerCells.Style.Font;
            headerFont.Bold = true;
            ShowReportExel(ws, p, dt.Rows[0]["COMPANY_NAME"].ToString(), locationName, ds.Tables["UspGrossProfitReport"], ds.Tables["uspGetExpenseDetailCrossTab"], ds.Tables["uspGetOtherIncomCrossTab"]);
            Byte[] fileBytes = p.GetAsByteArray();
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=ProfitLossStatement.xlsx");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.BinaryWrite(fileBytes);
            Response.End();
        }
    }
    protected void ChbDistributorList_SelectedIndexChanged(object sender, EventArgs e)
    {
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
    }

    private void ShowReportExel(ExcelWorksheet ws, ExcelPackage p, string CompanyName, string Location, DataTable dtRevenue, DataTable dtExpense, DataTable dtOtherIncome)
    {
        int rowIndex = 1;
        ws.Cells[rowIndex, 6].Value = "Profit & Loss Statement";

        rowIndex++;
        ws.Cells[rowIndex, 1].Value = CompanyName;
        ws.Cells[rowIndex, 12].Value = "From Date";
        ws.Cells[rowIndex, 13].Value = Convert.ToDateTime(txtStartDate.Text);
        ws.Cells[rowIndex, 13].Style.Numberformat.Format = "dd-MMM-yyyy";

        rowIndex++;
        ws.Cells[rowIndex, 1].Value = "Location:";
        ws.Cells[rowIndex, 2].Value = Location;

        ws.Cells[rowIndex, 12].Value = "To Date";
        ws.Cells[rowIndex, 13].Value = Convert.ToDateTime(txtEndDate.Text);
        ws.Cells[rowIndex, 13].Style.Numberformat.Format = "dd-MMM-yyyy";

        rowIndex++;
        rowIndex++;
        ws.Cells[rowIndex, 1].Value = "Financial Statments in Pak Rupees";
        Dictionary<string, int> metricRowMapOverAllSale = new Dictionary<string, int>();
        Dictionary<string, int> metricRowMapNetProfit = new Dictionary<string, int>();
        #region Revenu
        decimal TotaGrossSale = 0;
        decimal TotalCostOfGoods = 0;
        if (dtRevenue.Rows.Count > 0)
        {
            Dictionary<string, int> metricRowMap = new Dictionary<string, int>();
            rowIndex++;
            ws.Cells[rowIndex, 1].Value = "Revenue";
            ws.Cells[rowIndex, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells[rowIndex, 1].Style.Fill.BackgroundColor.SetColor(Color.DarkGray);
            ws.Cells[rowIndex, 1].Style.Font.Color.SetColor(Color.White);
            ws.Cells[rowIndex, 1].Style.Font.Bold = true;
            ws.Cells[rowIndex, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

            var locations = dtRevenue.AsEnumerable()
                      .Select(r => r["Location"].ToString())
                      .Distinct()
                      .ToList();

            int col = 2;

            foreach (var loc in locations)
            {
                ws.Cells[rowIndex, col].Value = loc;
                ws.Cells[rowIndex, col].Style.Font.Bold = true;
                ws.Cells[rowIndex, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                col++;
            }
            ws.Cells[rowIndex, col].Value = "Total";
            ws.Cells[rowIndex, col].Style.Font.Bold = true;
            ws.Cells[rowIndex, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
            int totalCol = col;

            string[] metrics = {
                                "Selling_Exp",
                                "GrossSales",
                                "UnClaimableDiscount",
                                "SchemeAmount"
                            };
            rowIndex++;
            foreach (var metric in metrics)
            {
                decimal rowTotal = 0;

                //Sale Value Excl. of Services Charges & Sales Taxes
                if (metric == "GrossSales")
                {
                    ws.Cells[rowIndex, 1].Value = "Sale Value Excl. of Services Charges & Sales Taxes";
                }
                //Discount On Sale
                else if (metric == "UnClaimableDiscount")
                {
                    ws.Cells[rowIndex, 1].Value = "Discount On Sale";
                }
                //Sales Tax
                else if (metric == "Selling_Exp")
                {
                    ws.Cells[rowIndex, 1].Value = "Sales Tax";
                }
                //Service Charges
                else if (metric == "SchemeAmount")
                {
                    ws.Cells[rowIndex, 1].Value = "Service Charges";
                }
                col = 2;
                foreach (var loc in locations)
                {
                    var value = dtRevenue.AsEnumerable()
                                .Where(r => r["Location"].ToString() == loc)
                                .Select(r => Convert.ToDecimal(r[metric]))
                                .FirstOrDefault();
                    ws.Cells[rowIndex, col].Value = value;
                    ws.Cells[rowIndex, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    rowTotal += value;
                    col++;
                }
                ws.Cells[rowIndex, totalCol].Value = rowTotal;
                ws.Cells[rowIndex, totalCol].Style.Font.Bold = true;
                ws.Cells[rowIndex, totalCol].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                metricRowMap[metric] = rowIndex;
                rowIndex++;
            }

            ws.Cells[rowIndex, 1].Value = "Overall Sale";
            ws.Cells[rowIndex, 1].Style.Font.Bold = true;
            ws.Cells[rowIndex, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

            for (int c = 2; c <= totalCol; c++)
            {
                string grossSalesCell = ws.Cells[metricRowMap["GrossSales"], c].Address;
                string discountCell = ws.Cells[metricRowMap["UnClaimableDiscount"], c].Address;
                string salesTaxCell = ws.Cells[metricRowMap["Selling_Exp"], c].Address;
                string serviceCell = ws.Cells[metricRowMap["SchemeAmount"], c].Address;

                ws.Cells[rowIndex, c].Formula = grossSalesCell
                    + "-" + discountCell
                    + "+" + salesTaxCell
                    + "+" + serviceCell;
                ws.Cells[rowIndex, c].Style.Font.Bold = true;
                ws.Cells[rowIndex, c].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                metricRowMapOverAllSale["OverAllSale"] = rowIndex;
                if (c == totalCol)
                {
                    p.Workbook.Calculate();
                    TotaGrossSale += Convert.ToDecimal(ws.Cells[metricRowMap["GrossSales"], c].Value);
                }
            }
            rowIndex++;

        }
        #endregion

        #region Cost Of Goods Sold
        if (dtRevenue.Rows.Count > 0)
        {
            Dictionary<string, int> metricRowMap = new Dictionary<string, int>();
            rowIndex++;
            ws.Cells[rowIndex, 1].Value = "Cost Of Goods Sold";
            ws.Cells[rowIndex, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells[rowIndex, 1].Style.Fill.BackgroundColor.SetColor(Color.DarkGray);
            ws.Cells[rowIndex, 1].Style.Font.Color.SetColor(Color.White);
            ws.Cells[rowIndex, 1].Style.Font.Bold = true;
            ws.Cells[rowIndex, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

            var locations = dtRevenue.AsEnumerable()
                      .Select(r => r["Location"].ToString())
                      .Distinct()
                      .ToList();

            int col = 2;

            foreach (var loc in locations)
            {
                ws.Cells[rowIndex, col].Value = loc;
                ws.Cells[rowIndex, col].Style.Font.Bold = true;
                ws.Cells[rowIndex, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                col++;
            }
            ws.Cells[rowIndex, col].Value = "Total";
            ws.Cells[rowIndex, col].Style.Font.Bold = true;
            ws.Cells[rowIndex, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
            int totalCol = col;

            string[] metrics = { "CostofGoods" };
            rowIndex++;

            int ConsumptionIndex = 0;
            foreach (var metric in metrics)
            {
                decimal rowTotal = 0;
                ConsumptionIndex = rowIndex;
                col = 2;
                foreach (var loc in locations)
                {
                    var value = dtRevenue.AsEnumerable()
                                .Where(r => r["Location"].ToString() == loc)
                                .Select(r => Convert.ToDecimal(r[metric]))
                                .FirstOrDefault();
                    ws.Cells[rowIndex, col].Value = value;
                    ws.Cells[rowIndex, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    rowTotal += value;
                    col++;
                }
                ws.Cells[rowIndex, totalCol].Value = rowTotal;
                ws.Cells[rowIndex, totalCol].Style.Font.Bold = true;
                ws.Cells[rowIndex, totalCol].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                metricRowMap[metric] = rowIndex;                
                rowIndex++;
            }

            ws.Cells[rowIndex, 1].Value = "Gross Profit(Loss)";
            ws.Cells[rowIndex, 1].Style.Font.Bold = true;
            ws.Cells[rowIndex, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

            for (int c = 2; c <= totalCol; c++)
            {
                string overAllSaleCell = ws.Cells[metricRowMapOverAllSale["OverAllSale"], c].Address;
                string costofGoodsCell = ws.Cells[metricRowMap["CostofGoods"], c].Address;
                ws.Cells[rowIndex, c].Formula = overAllSaleCell
                    + "-" + costofGoodsCell;
                ws.Cells[rowIndex, c].Style.Font.Bold = true;
                ws.Cells[rowIndex, c].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

                if (c == totalCol)
                {
                    p.Workbook.Calculate();
                    TotalCostOfGoods = Convert.ToDecimal(ws.Cells[metricRowMap["CostofGoods"], c].Value);
                }
                metricRowMapNetProfit["NetProfit"] = rowIndex;
            }
            decimal ConsumptionPer = 0;
            if (TotaGrossSale > 0)
            {
                ConsumptionPer = TotalCostOfGoods / TotaGrossSale * 100;
                ConsumptionPer = Math.Round(ConsumptionPer, 2);
            }
            //Cost Of Goods Sold
            ws.Cells[ConsumptionIndex, 1].Value = "Consumption " + ConsumptionPer.ToString() + " %";
            rowIndex++;

        }
        #endregion

        #region Expenses
        if (dtExpense.Rows.Count > 0)
        {
            rowIndex++;
            ws.Cells[rowIndex, 1].Value = "Expenses";
            ws.Cells[rowIndex, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells[rowIndex, 1].Style.Fill.BackgroundColor.SetColor(Color.DarkGray);
            ws.Cells[rowIndex, 1].Style.Font.Color.SetColor(Color.White);
            ws.Cells[rowIndex, 1].Style.Font.Bold = true;
            ws.Cells[rowIndex, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

            var locations = dtExpense.AsEnumerable()
                      .Select(r => r["Location"].ToString())
                      .Distinct()
                      .ToList();

            int col = 2;
            foreach (var loc in locations)
            {
                ws.Cells[rowIndex, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                col++;
            }
            ws.Cells[rowIndex, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
            int totalCol = col;

            rowIndex++;
            List<string> metrics = dtExpense.AsEnumerable()
                .Select(r => r["ACCOUNT_NAME"].ToString())
                .Distinct()
                .ToList();

            decimal rowTotal = 0;
            col = 2;
            foreach (var metric in metrics)
            {
                if (metric.Length > 0)
                {
                    ws.Cells[rowIndex, 1].Value = metric.Trim();
                    foreach (var loc in locations)
                    {
                        var value = dtExpense.AsEnumerable()
                                    .Where(r => r["Location"].ToString() == loc)
                                    .Select(r => Convert.ToDecimal(r["Amount"]))
                                    .FirstOrDefault();
                        ws.Cells[rowIndex, col].Value = value;
                        ws.Cells[rowIndex, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                        rowTotal += value;
                        col++;
                    }

                    ws.Cells[rowIndex, totalCol].Value = rowTotal;
                    ws.Cells[rowIndex, totalCol].Style.Font.Bold = true;
                    ws.Cells[rowIndex, totalCol].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    rowIndex++;
                }
            }
            ws.Cells[rowIndex, 1].Value = "Total";
            ws.Cells[rowIndex, 1].Style.Font.Bold = true;
            ws.Cells[rowIndex, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

            int firstDataRow = rowIndex - metrics.Count;
            int lastDataRow = rowIndex - 1;
            for (int c = 2; c <= totalCol; c++)
            {
                ws.Cells[rowIndex, c].Formula =
                    "SUM(" +
                    ws.Cells[firstDataRow, c].Address +
                    ":" +
                    ws.Cells[lastDataRow, c].Address +
                    ")";
                ws.Cells[rowIndex, c].Style.Font.Bold = true;
                ws.Cells[rowIndex, c].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
            }
        }
        #endregion

        #region Other Income
        if (dtOtherIncome.Rows.Count > 0)
        {
            rowIndex++;
            ws.Cells[rowIndex, 1].Value = "Other Income";
            ws.Cells[rowIndex, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells[rowIndex, 1].Style.Fill.BackgroundColor.SetColor(Color.DarkGray);
            ws.Cells[rowIndex, 1].Style.Font.Color.SetColor(Color.White);
            ws.Cells[rowIndex, 1].Style.Font.Bold = true;
            ws.Cells[rowIndex, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

            var locations = dtOtherIncome.AsEnumerable()
                      .Select(r => r["Location"].ToString())
                      .Distinct()
                      .ToList();

            int col = 2;
            foreach (var loc in locations)
            {
                ws.Cells[rowIndex, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                col++;
            }
            ws.Cells[rowIndex, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
            int totalCol = col;

            rowIndex++;
            List<string> metrics = dtOtherIncome.AsEnumerable()
                .Select(r => r["ACCOUNT_NAME"].ToString())
                .Distinct()
                .ToList();

            decimal rowTotal = 0;
            col = 2;
            foreach (var metric in metrics)
            {
                if (metric.Length > 0)
                {
                    ws.Cells[rowIndex, 1].Value = metric.Trim();
                    foreach (var loc in locations)
                    {
                        var value = dtOtherIncome.AsEnumerable()
                                    .Where(r => r["Location"].ToString() == loc)
                                    .Select(r => Convert.ToDecimal(r["Amount"]))
                                    .FirstOrDefault();
                        ws.Cells[rowIndex, col].Value = value;
                        ws.Cells[rowIndex, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                        rowTotal += value;
                        col++;
                    }

                    ws.Cells[rowIndex, totalCol].Value = rowTotal;
                    ws.Cells[rowIndex, totalCol].Style.Font.Bold = true;
                    ws.Cells[rowIndex, totalCol].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    rowIndex++;
                }
            }
            ws.Cells[rowIndex, 1].Value = "Total";
            ws.Cells[rowIndex, 1].Style.Font.Bold = true;
            ws.Cells[rowIndex, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

            int firstDataRow = rowIndex - metrics.Count;
            int lastDataRow = rowIndex - 1;
            for (int c = 2; c <= totalCol; c++)
            {
                ws.Cells[rowIndex, c].Formula =
                    "SUM(" +
                    ws.Cells[firstDataRow, c].Address +
                    ":" +
                    ws.Cells[lastDataRow, c].Address +
                    ")";
                ws.Cells[rowIndex, c].Style.Font.Bold = true;
                ws.Cells[rowIndex, c].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
            }
        }
        #endregion

        #region Operating Profit
        if (dtRevenue.Rows.Count > 0)
        {
            rowIndex++;
            ws.Cells[rowIndex, 1].Value = "Operating Profit";
            ws.Cells[rowIndex, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells[rowIndex, 1].Style.Fill.BackgroundColor.SetColor(Color.DarkGray);
            ws.Cells[rowIndex, 1].Style.Font.Color.SetColor(Color.White);
            ws.Cells[rowIndex, 1].Style.Font.Bold = true;
            ws.Cells[rowIndex, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

            var locations = dtRevenue.AsEnumerable()
                          .Select(r => r["Location"].ToString())
                          .Distinct()
                          .ToList();

            int col = 2;

            foreach (var loc in locations)
            {
                ws.Cells[rowIndex, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                col++;
            }
            ws.Cells[rowIndex, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
            int totalCol = col;
            string[] metrics = { "Administrative_Exp" };            
            foreach (var metric in metrics)
            {
                decimal rowTotal = 0;
                col = 2;
                foreach (var loc in locations)
                {
                    var value = dtRevenue.AsEnumerable()
                                .Where(r => r["Location"].ToString() == loc)
                                .Select(r => Convert.ToDecimal(r[metric]))
                                .FirstOrDefault();
                    ws.Cells[rowIndex, col].Value = Convert.ToDecimal(ws.Cells[metricRowMapNetProfit["NetProfit"], col].Value) - value;
                    ws.Cells[rowIndex, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    rowTotal += Convert.ToDecimal(ws.Cells[rowIndex, col].Value);
                    col++;
                }
                ws.Cells[rowIndex, totalCol].Value = rowTotal;
                ws.Cells[rowIndex, totalCol].Style.Font.Bold = true;
                ws.Cells[rowIndex, totalCol].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                rowIndex++;
            }
        }
        #endregion
    }
}