using System;
using System.Data;
using System.Web.UI;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;


public partial class Forms_frmPurchaseAuto : System.Web.UI.Page
{
    DataTable _PurchaseSKUS;
    private void CreateTable()
    {
        _PurchaseSKUS = new DataTable();
        _PurchaseSKUS.Columns.Add("SKU_ID", typeof(int));
        _PurchaseSKUS.Columns.Add("SKU_Code", typeof(string));
        _PurchaseSKUS.Columns.Add("SKU_Name", typeof(string));
        _PurchaseSKUS.Columns.Add("Quantity", typeof(decimal));
        _PurchaseSKUS.Columns.Add("UOM_ID", typeof(int));
        _PurchaseSKUS.Columns.Add("S_UOM_ID", typeof(int));
        _PurchaseSKUS.Columns.Add("S_Quantity", typeof(decimal));
        this.Session.Add("PurchaseSKUS", _PurchaseSKUS);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.GetAppSettingDetail();
            this.LoadDistributor();
            this.LoadToDistributor();
            this.GetDocumentNo();            
            this.LoadDocumentDetail();
        }
    }
    private void GetDocumentNo()
    {
        try
        {
            drpDocumentNo.Items.Clear();
            FranchiseSaleInvoiceController mFranchise = new FranchiseSaleInvoiceController();
            DataTable dt = mFranchise.SelectFranchise_Invoice_Lookup(Convert.ToInt32(Session["DISTRIBUTOR_ID"]), Convert.ToInt32(Session["UserID"]),Convert.ToDateTime(Session["CurrentWorkDate"]), 2);
            clsWebFormUtil.FillDxComboBoxList(drpDocumentNo, dt, "FRANCHISE_MASTER_ID", "FRANCHISE_MASTER_ID");
            if (dt.Rows.Count > 0)
            {
                drpDocumentNo.SelectedIndex = 0;
                DrpTransferFrom.Value = dt.Rows[0]["PurchaseFrom"].ToString();
                drpDistributor.Value = dt.Rows[0]["PurchaseFor"].ToString();
            }
            else
            {
                drpDocumentNo.SelectedIndex = -1;
            }
            Session.Add("dtPurchase", dt);
        }
        catch (Exception EX)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert(' Error:   " + EX.Message.ToString() + " ');", true);
        }
    }
    protected void drpDocumentNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dtPurchase = (DataTable)Session["dtPurchase"];
        foreach(DataRow dr in dtPurchase.Rows)
        {
            if(dr["FRANCHISE_MASTER_ID"].ToString() == drpDocumentNo.Value.ToString())
            {
                DrpTransferFrom.Value = dr["PurchaseFrom"].ToString();
                drpDistributor.Value = dr["PurchaseFor"].ToString();
                break;
            }
        }
        this.LoadDocumentDetail();
    }
    private void LoadDocumentDetail()
    {
        if (drpDocumentNo.Items.Count > 0)
        {
            FranchiseSaleInvoiceController mPurchase = new FranchiseSaleInvoiceController();
            DataTable PurchaseSKUS = mPurchase.SelectFranchise_Invoice_Details2(long.Parse(drpDocumentNo.Value.ToString()));
            GrdPurchase.DataSource = PurchaseSKUS;
            GrdPurchase.DataBind();
            this.Session.Add("PurchaseSKUS", PurchaseSKUS);
        }
    }
    private void LoadDistributor()
    {
        try
        {
            DistributorController DController = new DistributorController();
            DataTable dt = DController.GetDistributorWithMaxDayClose(Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), int.Parse(this.Session["CompanyId"].ToString()), 4);
            clsWebFormUtil.FillDxComboBoxList(drpDistributor, dt, "CUSTOMER_ID", "CUSTOMER_NAME");
            if (dt.Rows.Count > 0)
            {
                drpDistributor.SelectedIndex = 0;
            }
            Session.Add("dtLocationInfo", dt);
        }
        catch (Exception EX)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert(' Error:   " + EX.Message.ToString() + " ');", true);
        }
    }
    private void LoadToDistributor()
    {
        try
        {
            DistributorController DController = new DistributorController();
            DataTable dt = DController.GetDistributorWithMaxDayClose(Constants.IntNullValue, Constants.IntNullValue, 5, 2);
            clsWebFormUtil.FillDxComboBoxList(DrpTransferFrom, dt, "DISTRIBUTOR_ID", "DISTRIBUTOR_NAME");
            if (dt.Rows.Count > 0)
            {
                DrpTransferFrom.SelectedIndex = 0;
            }
        }
        catch (Exception EX)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert(' Error:   " + EX.Message.ToString() + " ');", true);
        }
    }
    public DataTable GetCOAConfiguration()
    {
        try
        {
            COAMappingController _cController = new COAMappingController();
            DataTable dt = _cController.SelectCOAConfiguration(5, Constants.ShortNullValue, Constants.LongNullValue, "Level 4");
            if (dt.Rows.Count > 0)
            {
                return dt;
            }
            else
            {
                return null;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg3", "alert('Error Occured: \n" + ex + "');", true);
            return null;
        }
    }

    private bool GetFinanceConfig()
    {
        try
        {
            DataTable dt = (DataTable)Session["dtAppSettingDetail"];
            if (dt.Rows.Count > 0)
            {
                return Convert.ToInt32(dt.Rows[0]["IsFinanceIntegrate"]) == 1 ? true : false;
            }
            return false;
        }
        catch (Exception)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Error in Financial Setting!');", true);
            throw;
        }
    }
    protected void btnSavePurchase_Click(object sender, EventArgs e)
    {
        DateTime CurrentWorkDate = Constants.DateNullValue;
        DataTable dtLocationInfo = (DataTable)Session["dtLocationInfo"];
        foreach (DataRow dr in dtLocationInfo.Rows)
        {
            if (dr["CUSTOMER_ID"].ToString() == drpDistributor.Value.ToString())
            {
                if (dr["MaxDayClose"].ToString().Length > 0)
                {
                    CurrentWorkDate = Convert.ToDateTime(dr["MaxDayClose"]);
                    break;
                }
            }
        }
        if (CurrentWorkDate != Constants.DateNullValue)
        {
            PurchaseController mController = new PurchaseController();
            DataTable dtPurchaseDetail = (DataTable)this.Session["PurchaseSKUS"];
            decimal mTotalAmount = 0;

            foreach (DataRow dr in dtPurchaseDetail.Rows)
            {
                mTotalAmount += decimal.Parse(dr["AMOUNT"].ToString());

            }
            DataTable dtConfig = GetCOAConfiguration();
            bool IsFinanceSetting = GetFinanceConfig();
            long mResult = Constants.LongNullValue;

            mResult = mController.InsertFranchisePurchase(int.Parse(drpDistributor.SelectedItem.Value.ToString()),txtDocumentNo.Text, Constants.Document_Purchase, CurrentWorkDate,int.Parse(drpDistributor.SelectedItem.Value.ToString()),int.Parse(DrpTransferFrom.SelectedItem.Value.ToString()), mTotalAmount,false, dtPurchaseDetail, 0, txtBuiltyNo.Text, int.Parse(Session["UserId"].ToString()), Convert.ToInt32(DrpTransferFrom.Value),0, 0, 0,mTotalAmount, DrpTransferFrom.SelectedItem.Text, 1,Convert.ToInt64(drpDocumentNo.Value),dtConfig, IsFinanceSetting);
            if (mResult > 0)
            {
                FranchiseSaleInvoiceController mFranchise = new FranchiseSaleInvoiceController();
                mFranchise.Update_FranchiseSaleInvoice(int.Parse(drpDocumentNo.SelectedItem.Value.ToString()));
                GrdPurchase.DataSource = null;
                GrdPurchase.DataBind();
                Session.Remove("PurchaseSKUS");
                GetDocumentNo();
                LoadDocumentDetail();
                txtBuiltyNo.Text = "";
                txtDocumentNo.Text = "";
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Purchased saved successfully. ');", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Dayclose not found for selected location!');", true);
        }
    }
    private void LoadGird()
    {
        try
        {
            _PurchaseSKUS = (DataTable)this.Session["PurchaseSKUS"];
            GrdPurchase.DataSource = _PurchaseSKUS;
            GrdPurchase.DataBind();
        }
        catch (Exception EX)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert(' Error:   " + EX.Message.ToString() + " ');", true);
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        //  Respo
    }

    public void GetAppSettingDetail()
    {
        try
        {
            AppSettingDetail _cController = new AppSettingDetail();
            DataTable dtAppSetting = _cController.GetAppSettingDetail(1);
            if (dtAppSetting.Rows.Count > 0)
            {
                Session.Add("dtAppSettingDetail", dtAppSetting);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg3", "alert('Error Occured: \n" + ex + "');", true);
        }
    }
}