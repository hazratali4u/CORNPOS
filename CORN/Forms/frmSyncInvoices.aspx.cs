using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Net;
using System.IO;

public partial class Forms_frmSyncInvoices : System.Web.UI.Page
{
    readonly DataControl dc = new DataControl();

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        Response.Cache.SetNoStore();
        Response.AppendHeader("pragma", "no-cache");
        if (!Page.IsPostBack)
        {
            LoadDistributor();
            Configuration.SystemCurrentDateTime = (DateTime)this.Session["CurrentWorkDate"];
            txtStartDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtEndDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtStartDate.Attributes.Add("readonly", "readonly");
            txtEndDate.Attributes.Add("readonly", "readonly");
        }
    }

    private void LoadDistributor()
    {
        DistributorController DController = new DistributorController();
        DataTable dt = DController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), int.Parse(this.Session["CompanyId"].ToString()));
        clsWebFormUtil.FillDxComboBoxList(this.drpDistributor, dt, 0, 2, true);
        if (dt.Rows.Count > 0)
        {
            drpDistributor.SelectedIndex = 0;
        }
    }

    private void LoadRollbackDocument()
    {
        var or = new OrderEntryController();
        DataTable dtOrder = or.SelectSyncDocument(Convert.ToInt32(drpDistributor.Value), 2, DateTime.Parse(txtStartDate.Text), DateTime.Parse(txtEndDate.Text));

        GrdOrder.DataSource = dtOrder;
        GrdOrder.DataBind();

        GrdOrder2.DataSource = dtOrder;
        GrdOrder2.DataBind();

    }

    protected void btnGetOrder_Click(object sender, EventArgs e)
    {
        LoadRollbackDocument();
    }

    protected void btnCalculate_Click(object sender, EventArgs e)
    {
        GrdOrder.Visible = true;
        GrdOrder2.Visible = false;

        dvCalculate.Attributes.Add("style", "display:block;");
        dvCalculation.Visible = true;
        hfGrid.Value = "1";

        lblGrossAmount.Text = "0";
        lblDiscount.Text = "0";
        lblGstAmount.Text = "0";
        lblNetAmount.Text = "0";

        foreach (GridViewRow dr in GrdOrder.Rows)
        {
            CheckBox ChbInvoice = (CheckBox)dr.Cells[0].FindControl("ChbInvoice");
            if (ChbInvoice.Checked)
            {
                lblGrossAmount.Text = Convert.ToString(decimal.Parse(dc.chkNull_0(dr.Cells[7].Text)) + decimal.Parse(lblGrossAmount.Text));
                lblDiscount.Text = Convert.ToString(decimal.Parse(lblDiscount.Text) + decimal.Parse(dc.chkNull_0(dr.Cells[8].Text)));
                lblGstAmount.Text = Convert.ToString(decimal.Parse(lblGstAmount.Text) + decimal.Parse(dc.chkNull_0(dr.Cells[9].Text)));
                lblNetAmount.Text = Convert.ToString(decimal.Parse(lblNetAmount.Text) + decimal.Parse(dc.chkNull_0(dr.Cells[10].Text)));
            }
        }
    }
    /// <summary>
    /// insert into rims invoice master, detail tables and sync to rims server 
    /// </summary>
    protected void btnSync_Click(object sender, EventArgs e)
    {
        CompanyController CompanyCtrl = new CompanyController();
        DataTable dtComapny = CompanyCtrl.SelectCompany(Constants.IntNullValue, Constants.IntNullValue);
        if (dtComapny.Rows.Count > 0)
        {
            int COMPANY_PROVINCE = Convert.ToInt32(dtComapny.Rows[0]["COMPANY_PROVINCE"].ToString());
            #region Punjab
            if (COMPANY_PROVINCE == 0)//COMPANY_PROVINCE 0=Punjab,1=KPK,2=Sindh and 3=Baluchistan
            {
                OrderEntryController orCtrl = new OrderEntryController();
                using (var webClient = new WebClient())
                {
                    var getKeyUrl = "https://rims.punjab.gov.pk/api/databaseupdate/formkey";
                    var json = webClient.DownloadString(getKeyUrl);

                    var results = JsonConvert.DeserializeObject<dynamic>(json);
                    lbl_key.Text = Convert.ToString(results.Last.Value);
                }
                if (lbl_key.Text != "")
                {
                    GridView Grid = new GridView();
                    if (hfGrid.Value == "1")
                    {
                        Grid = GrdOrder;
                    }
                    else if (hfGrid.Value == "0")
                    {
                        Grid = GrdOrder2;
                    }
                    foreach (GridViewRow dr in Grid.Rows)
                    {
                        CheckBox chInvoice = (CheckBox)dr.Cells[0].FindControl("ChbInvoice");
                        if (chInvoice.Checked == true)
                        {
                            try
                            {
                                string detail = GetPendingBill(Convert.ToInt64(dr.Cells[2].Text));
                                long ID = orCtrl.Add_RIMSInvoice(Convert.ToInt32(dr.Cells[11].Text), int.Parse(drpDistributor.SelectedItem.Value.ToString())
                                    , int.Parse(Session["UserID"].ToString()), Convert.ToInt64(dr.Cells[2].Text));
                                if (ID > 0)
                                {
                                    if (dr.Cells[13].Text != "" || dr.Cells[14].Text != "")
                                    {
                                        string URI = " https://rims.punjab.gov.pk/api/databaseupdate/formdata";
                                        string myParameters = "key=" + lbl_key.Text + "&data={\"pntn\":\"" + dr.Cells[13].Text + "\",\"branchcode\":\"" + dr.Cells[14].Text + "\",\"invoice_number\":\"" + Convert.ToInt64(dr.Cells[2].Text) + "\",\"invoice_date\": \"" + Convert.ToDateTime(dr.Cells[3].Text).ToString("yyyy-MM-dd") + "\",\"invoice_time\": \"" + dr.Cells[4].Text + "\",\"tax_percent\":\"" + dr.Cells[12].Text + "\",\"table_no\": \"" + dr.Cells[11].Text + "\",\"detail\":" + detail + "}";
                                        using (WebClient wc = new WebClient())
                                        {
                                            wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                                            string HtmlResult = wc.UploadString(URI, myParameters);
                                        }
                                        orCtrl.Update_RIMSInvoice(int.Parse(drpDistributor.SelectedItem.Value.ToString()), true, Convert.ToInt64(dr.Cells[2].Text));
                                    }
                                    else
                                    {
                                        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Please add Branch Code or GST No!');", true);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                ExceptionPublisher.PublishException(ex);
                                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('some error occurred!');", true);
                            }
                        }
                    }
                    LoadRollbackDocument();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Please try again!');", true);
                }
            }
            #endregion
            #region KPK
            else if (COMPANY_PROVINCE == 1)
            {
                OrderEntryController orCtrl = new OrderEntryController();
                GridView Grid = new GridView();
                if (hfGrid.Value == "1")
                {
                    Grid = GrdOrder;
                }
                else if (hfGrid.Value == "0")
                {
                    Grid = GrdOrder2;
                }

                foreach (GridViewRow dr in Grid.Rows)
                {
                    CheckBox chInvoice = (CheckBox)dr.Cells[0].FindControl("ChbInvoice");
                    if (chInvoice.Checked == true)
                    {
                        long ID = orCtrl.Add_RIMSInvoice(Convert.ToInt32(dr.Cells[11].Text), int.Parse(drpDistributor.SelectedItem.Value.ToString())
                            , int.Parse(Session["UserID"].ToString()), Convert.ToInt64(dr.Cells[2].Text));
                        if (ID > 0)
                        {
                            string Url = string.Format("http://kpra.gov.pk/rims/integration/?ntn={0}&key={1}&invoice_no={2}&amount={3}&sts={4}&date={5}", Cryptography.Decrypt(dtComapny.Rows[0]["PRA_NTN"].ToString(),Constants.EncryptionKey), Cryptography.Decrypt(dtComapny.Rows[0]["PRA_KEY"].ToString(), Constants.CryptographyKey), Convert.ToInt64(dr.Cells[2].Text), dr.Cells[7].Text, dr.Cells[9].Text, Convert.ToDateTime(dr.Cells[3].Text).ToString("yyyy-MM-dd hh:mm:ss"));
                            string myParams = "";
                            using (WebClient wc = new WebClient())
                            {
                                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                                string HtmlResult = wc.UploadString(Url, myParams);
                            }
                            orCtrl.Update_RIMSInvoice(int.Parse(drpDistributor.SelectedItem.Value.ToString()), true, Convert.ToInt64(dr.Cells[2].Text));
                        }
                    }
                }
            }
            #endregion
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('No PRA detail found!');", true);
        }
    }
    public string GetPendingBill(long saleInvoiceMasterId)
    {
        var mSkuController = new SkuController();
        DataTable dtSkus = mSkuController.SpGetPendingBill(saleInvoiceMasterId, 2,Constants.IntNullValue);
        return GetJson(dtSkus);
    }

    public string GetJson(DataTable dt)
    {
        System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();        
        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        Dictionary<string, object> row = null;
        foreach (DataRow dr in dt.Rows)
        {
            row = new Dictionary<string, object>();
            foreach (DataColumn col in dt.Columns)
            {
                row.Add(col.ColumnName, dr[col]);
            }
            rows.Add(row);
        }
        return serializer.Serialize(rows);
    }
}