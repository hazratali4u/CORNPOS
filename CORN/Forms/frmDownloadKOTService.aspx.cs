using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using System;
using System.Data;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>
/// From For Purchase, TranferOut, Purchase Return, TranferIn And Damage
/// </summary>
public partial class Forms_frmDownloadKOTService : System.Web.UI.Page
{
    readonly DataControl _dc = new DataControl();
    readonly PurchaseController _mPurchaseCtrl = new PurchaseController();
    readonly SKUPriceDetailController _pController = new SKUPriceDetailController();
    DataTable dtPrint;

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        Response.Cache.SetNoStore();
        Response.AppendHeader("pragma", "no-cache");

        if (!Page.IsPostBack)
        {
            LoadDistributor();
            dtPrint = new DataTable();
            dtPrint.Columns.Add("PrinterName", typeof(string));
            dtPrint.Columns.Add("FullKOT", typeof(bool));
            dtPrint.Columns.Add("Expeditor", typeof(bool));
            dtPrint.Columns.Add("PrintInvoice", typeof(bool));
            dtPrint.Columns.Add("LocationName", typeof(bool));
            dtPrint.Columns.Add("Exclude", typeof(bool));
            Session.Add("dtPrint", dtPrint);
        }
    }

    public void btnConnectionString_Click(object sender, EventArgs e)
    {
        string databaseName = "";
        string serverName = "";
        string userID = "";
        string Pass = "";
        CompanyController compny = new CompanyController();
        DataTable dtCompany = compny.SelectCompany(Constants.IntNullValue,Constants.IntNullValue);
        if(dtCompany.Rows.Count > 0)
        {
            serverName = dtCompany.Rows[0]["DB_SERVER_URL"].ToString();
        }
        if (System.Configuration.ConfigurationManager.AppSettings["DBName"] != null)
        {
            databaseName = System.Configuration.ConfigurationManager.AppSettings["DBName"].ToString();
            databaseName = Cryptography.Decrypt(databaseName, Constants.CryptographyKey);
        }
        if (System.Configuration.ConfigurationManager.AppSettings["DBUser"] != null)
        {
            userID = System.Configuration.ConfigurationManager.AppSettings["DBUser"].ToString();
            userID = Cryptography.Decrypt(userID, Constants.CryptographyKey);
        }
        if (System.Configuration.ConfigurationManager.AppSettings["DBPassword"] != null)
        {
            Pass = System.Configuration.ConfigurationManager.AppSettings["DBPassword"].ToString();
            Pass = Cryptography.Decrypt(Pass, Constants.CryptographyKey);
        }
        string connectionString = "server=" + serverName + ";uid=" + userID + ";pwd=" + Pass + "; database= "+ databaseName + ";Encrypt=True;TrustServerCertificate=True;Pooling=True;Max Pool Size=200;Application Name=CORNPOSKOTPrint;Packet Size=32767;";
        string encryptedConnectionString = Cryptography.Encrypt(connectionString, Constants.CryptographyKey);
        string distributorId = drpDistributor.SelectedItem.Value.ToString();
        string IsFullKOT = "";
        string IsPrintInvoice = "";
        string IsXpeditor = "";
        string LocationName = "";
        string ExcludeCancelKOT = "";
        string StickerSectionBoth = "";
        string LenghtySticker = "";
        string PizzaBoxSticker = "";
        string PrinterName = "";
        string SectionWisePrint = "0";
        string FullKOTGroup = "1";
        if (cbSectionWise.Checked)
        {
            SectionWisePrint = "1";
        }

        foreach (GridViewRow gvr in gvPrinter.Rows)
        {
            PrinterName += gvr.Cells[0].Text + ",";
        }
        if(PrinterName.Length > 0)
        {
            PrinterName = PrinterName.Substring(0, PrinterName.Length - 1);
        }

        if (cbFullKOT.Checked)
        {
            IsFullKOT = "sectionPrinter" + ",";
        }
        if (cbLocationName.Checked)
        {
            LocationName = "sectionPrinter" + ",";            
        }
        if (cbExcludeCancelKOT.Checked)
        {
            ExcludeCancelKOT = "sectionPrinter" + ",";
        }

        foreach (GridViewRow gvr in gvPrinter.Rows)
        {
            CheckBox cbExpeditorgvr = (CheckBox)gvr.Cells[3].FindControl("cbExpeditor");
            CheckBox cbPrintInvoicegvr = (CheckBox)gvr.Cells[5].FindControl("cbPrintInvoice");
            CheckBox cbFullKOTgvr = (CheckBox)gvr.Cells[1].FindControl("cbFullKOT");
            CheckBox cbLocationNamegvr = (CheckBox)gvr.Cells[7].FindControl("cbLocationName");
            CheckBox cbExclude = (CheckBox)gvr.Cells[9].FindControl("cbExclude");
            RadioButtonList rblXpeditor = (RadioButtonList)gvr.Cells[11].FindControl("rblXpeditor");


            if (cbExpeditorgvr.Checked)
            {
                IsXpeditor += gvr.Cells[0].Text + ",";
            }
            if (cbPrintInvoicegvr.Checked)
            {
                IsPrintInvoice += gvr.Cells[0].Text + ",";
            }
            if (cbFullKOTgvr.Checked)
            {
                IsFullKOT += gvr.Cells[0].Text + ",";
            }
            if (cbLocationNamegvr.Checked)
            {
                LocationName += gvr.Cells[0].Text + ",";
            }
            if (cbExclude.Checked)
            {
                ExcludeCancelKOT += gvr.Cells[0].Text + ",";
            }
            if(cbExpeditorgvr.Checked || cbFullKOTgvr.Checked)
            {
                FullKOTGroup = rblXpeditor.SelectedValue;
            }
        }
        if (cbFullKOT.Checked)
        {
            FullKOTGroup = rblFullKOT.SelectedValue;
        }
        if (IsXpeditor.Length > 0)
        {
            IsXpeditor = IsXpeditor.Substring(0, IsXpeditor.Length - 1);
        }
        if (IsPrintInvoice.Length > 0)
        {
            IsPrintInvoice = IsPrintInvoice.Substring(0, IsPrintInvoice.Length - 1);
        }
        if (IsFullKOT.Length > 0)
        {
            IsFullKOT = IsFullKOT.Substring(0, IsFullKOT.Length - 1);
        }
        if (LocationName.Length > 0)
        {
            LocationName = LocationName.Substring(0, LocationName.Length - 1);
        }
        if (ExcludeCancelKOT.Length > 0)
        {
            ExcludeCancelKOT = ExcludeCancelKOT.Substring(0, ExcludeCancelKOT.Length - 1);
        }

        if (ddlStickerPrinter.SelectedValue == "1")
        {
            StickerSectionBoth = "1";
        }

        if (ddlStickerPrinter.SelectedValue == "2")
        {
            LenghtySticker = "1";
        }
        if (ddlStickerPrinter.SelectedValue == "3")
        {
            PizzaBoxSticker = "1";
        }
        string configText = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + System.Environment.NewLine;
        configText += "<configuration>" + System.Environment.NewLine;
        configText += "<startup useLegacyV2RuntimeActivationPolicy=\"true\">" + System.Environment.NewLine;
        configText += "<supportedRuntime version=\"v4.0\" sku=\".NETFramework,Version=v4.5.2\" />" + System.Environment.NewLine;
        configText += "</startup>" + System.Environment.NewLine;
        configText += "<appSettings>" + System.Environment.NewLine;
        configText += "<add key=\"connString\" value=\"" + encryptedConnectionString + "\"/>" + System.Environment.NewLine;
        configText += "<add key=\"PrinterName\" value=\"" + PrinterName + "\"/>" + System.Environment.NewLine;
        configText += "<add key=\"DistributorID\" value=\"" + distributorId + "\"/>" + System.Environment.NewLine;
        configText += "<add key=\"IsFullKOT\" value=\"" + IsFullKOT + "\"/>" + System.Environment.NewLine;
        configText += "<add key=\"IsPrintInvoice\" value=\"" + IsPrintInvoice + "\"/>" + System.Environment.NewLine;        
        configText += "<add key=\"LogoPath\" value=\"C:\\Program Files (x86)\\CORN Point of Sale\\CORN KOT Printer\\logo.jpg\"/>" + System.Environment.NewLine;
        configText += "<add key=\"IsXpeditor\" value=\"" + IsXpeditor + "\"/>" + System.Environment.NewLine;
        configText += "<add key=\"IsLocationName\" value=\"" + LocationName + "\"/>" + System.Environment.NewLine;
        configText += "<add key=\"LocationName\" value=\"" + drpDistributor.SelectedItem.Text + "\"/>" + System.Environment.NewLine;
        configText += "<add key=\"ExcludeCancelKOT\" value=\"" + ExcludeCancelKOT + "\"/>" + System.Environment.NewLine;
        configText += "<add key=\"StickerSectionBoth\" value=\"" + StickerSectionBoth + "\"/>" + System.Environment.NewLine;
        configText += "<add key=\"StickerPrinterName\" value=\"" + txtStickerPrinterName.Text + "\"/>" + System.Environment.NewLine;
        configText += "<add key=\"LenghtySticker\" value=\"" + LenghtySticker + "\"/>" + System.Environment.NewLine;
        configText += "<add key=\"PizzaBoxSticker\" value=\"" + PizzaBoxSticker + "\"/>" + System.Environment.NewLine;
        configText += "<add key=\"SectionWisePrint\" value=\"" + SectionWisePrint + "\"/>" + System.Environment.NewLine;
        configText += "<add key=\"FullKOTGroup\" value=\"" + FullKOTGroup + "\"/>" + System.Environment.NewLine;
        configText += "</appSettings>" + System.Environment.NewLine;
        configText += "</configuration>";

        var byteArray = Encoding.ASCII.GetBytes(configText);
        string someString = Encoding.ASCII.GetString(byteArray);
        Stream stream = new MemoryStream(byteArray);

        Response.ContentType = "application/force-download";
        Response.AppendHeader("Content-Disposition", "attachment;filename=CORNPOSKOTPrint.exe.config");

        Response.BinaryWrite(byteArray);

        Response.End();

    }

    private void LoadDistributor()
    {
        DistributorController DController = new DistributorController();
        DataTable dt = DController.SelectDistributorInfo(Constants.IntNullValue,  Constants.IntNullValue, int.Parse(this.Session["CompanyId"].ToString()));
        clsWebFormUtil.FillDxComboBoxList(drpDistributor, dt, "DISTRIBUTOR_ID", "DISTRIBUTOR_NAME", true);

        if (dt.Rows.Count > 0)
        {
            drpDistributor.SelectedIndex = 0;
        }
    }
    
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        
        if (txtPrinterName.Text.Length > 0)
        {
            bool flag = true;
            foreach(GridViewRow gvr in gvPrinter.Rows)
            {
                if(gvr.Cells[0].Text == txtPrinterName.Text)
                {
                    flag = false;
                    break;
                }
            }
            if (flag)
            {
                dtPrint = (DataTable)Session["dtPrint"];
                DataRow dr = dtPrint.NewRow();
                dr["PrinterName"] = txtPrinterName.Text;
                dr["FullKOT"] = false;
                dr["Expeditor"] = false;
                dr["PrintInvoice"] = false;
                dr["LocationName"] = false;
                dr["Exclude"] = false;
                dtPrint.Rows.Add(dr);

                gvPrinter.DataSource = dtPrint;
                gvPrinter.DataBind();

                txtPrinterName.Text = string.Empty;
                txtPrinterName.Focus();
            }
        }
    }

    protected void gvPrinter_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBox cbFullKOT = (CheckBox)e.Row.Cells[1].FindControl("cbFullKOT");
            CheckBox cbExpeditor = (CheckBox)e.Row.Cells[3].FindControl("cbExpeditor");
            CheckBox cbPrintInvoice = (CheckBox)e.Row.Cells[5].FindControl("cbPrintInvoice");
            CheckBox cbLocationName = (CheckBox)e.Row.Cells[7].FindControl("cbLocationName");
            CheckBox cbExclude = (CheckBox)e.Row.Cells[9].FindControl("cbExclude");
            cbFullKOT.Checked = Convert.ToBoolean(e.Row.Cells[2].Text);
            cbExpeditor.Checked = Convert.ToBoolean(e.Row.Cells[4].Text);
            cbPrintInvoice.Checked = Convert.ToBoolean(e.Row.Cells[6].Text);
            cbLocationName.Checked = Convert.ToBoolean(e.Row.Cells[8].Text);
            cbExclude.Checked = Convert.ToBoolean(e.Row.Cells[10].Text);
        }
    }

    protected void cbFullKOT_CheckedChanged(object sender, EventArgs e)
    {
        rblFullKOT.Visible = cbFullKOT.Checked;
    }
}