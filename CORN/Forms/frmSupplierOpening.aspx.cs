using System;
using System.Web.UI;
using CORNCommon.Classes;
using CORNBusinessLayer.Classes;
using System.Data;

public partial class Forms_frmSupplierOpening : System.Web.UI.Page
{   
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.GetAppSettingDetail();
            LoadDistributor();
            LoadPrincipal();
            Configuration.SystemCurrentDateTime = (DateTime)this.Session["CurrentWorkDate"];
            txtOpeningDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            LoadOpeningInformation();
            txtOpeningDate.Attributes.Add("readonly", "readonly");
        }
    }
    private void LoadDistributor()
    {
        DistributorController DController = new DistributorController();
        DataTable dt = DController.GetDistributorWithMaxDayClose(Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), int.Parse(this.Session["CompanyId"].ToString()), 1);
        clsWebFormUtil.FillDxComboBoxList(drpDistributor, dt, "DISTRIBUTOR_ID", "DISTRIBUTOR_NAME");

        if (dt.Rows.Count > 0)
        {
            drpDistributor.SelectedIndex = 0;
        }
        Session.Add("dtLocationInfo", dt);
    }
    private void LoadPrincipal()
    {
        if (Session["FranchiseModule"].ToString() == "1")
        {
            DataTable dtVendors = (DataTable)Session["dtVendors"];
            clsWebFormUtil.FillDxComboBoxList(drpPrincipal, dtVendors, "VendorID", "VendorName");
        }
        else
        {
            var PController = new SKUPriceDetailController();
            DataTable dtVendor = PController.SelectDataPrice(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, int.Parse(Session["UserId"].ToString()), Constants.IntNullValue, 0, Constants.DateNullValue);
            clsWebFormUtil.FillDxComboBoxList(drpPrincipal, dtVendor, 0, 1);
        }

        if (drpPrincipal.Items.Count > 0)
        {
            drpPrincipal.SelectedIndex = 0;
        }
        else
        {
            drpPrincipal.SelectedIndex = -1;
        }
    }

    #region Opening Balance

    private void LoadOpeningInformation()
    {
        DateTime CurrentWorkDate = Constants.DateNullValue;
        DataTable dtLocationInfo = (DataTable)Session["dtLocationInfo"];
        foreach (DataRow dr in dtLocationInfo.Rows)
        {
            if (dr["DISTRIBUTOR_ID"].ToString() == drpDistributor.Value.ToString())
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
            PurchaseController mPurchase = new PurchaseController();
            Configuration.SystemCurrentDateTime = CurrentWorkDate;
            txtOpeningDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");

            txtOpeningBalance.Text = "0";
            txtOpeningBalanceRemarks.Text = "";
            rblOpening.SelectedValue = "0";
            masterId.Value = "0";
            btnSaveOpeningBalance.Enabled = true;
            btnSaveOpeningBalance.Text = "Save";

            if (drpPrincipal.Items.Count > 0)
            {
                int VendorID = 0;
                int TypeID = 0;
                if (Session["FranchiseModule"].ToString() == "1")
                {
                    TypeID = 26;
                    DataTable dtVendors = (DataTable)Session["dtVendors"];
                    foreach (DataRow dr in dtVendors.Rows)
                    {
                        if (dr["VendorID"].ToString() == drpPrincipal.SelectedItem.Value.ToString())
                        {
                            VendorID = Convert.ToInt32(dr["SupplierLocationID"]);                            
                            break;
                        }
                    }
                }
                DataTable dtOpening = mPurchase.SelectPrincipalOpening(Convert.ToInt32(drpDistributor.SelectedItem.Value), VendorID, TypeID);
                if (dtOpening.Rows.Count > 0)
                {
                    if (string.Format("{0:0.00}", dtOpening.Rows[0]["TOTAL_AMOUNT"]) == string.Format("{0:0.00}", dtOpening.Rows[0]["DEBIT_AMOUNT"]))
                    {
                        txtOpeningDate.Text = Convert.ToDateTime(dtOpening.Rows[0]["DOCUMENT_DATE"]).ToString("dd-MMM-yyyy");

                        txtOpeningBalance.Text = string.Format("{0:0.00}", dtOpening.Rows[0]["TOTAL_AMOUNT"]);
                        txtOpeningBalanceRemarks.Text = dtOpening.Rows[0]["BUILTY_NO"].ToString();
                        rblOpening.SelectedValue = dtOpening.Rows[0]["TYPE_ID"].ToString();
                        masterId.Value = dtOpening.Rows[0]["PURCHASE_MASTER_ID"].ToString();
                        btnSaveOpeningBalance.Enabled = true;
                        btnSaveOpeningBalance.Text = "Update";
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Plz Delete Realization for Update.');", true);
                        btnSaveOpeningBalance.Enabled = false;
                    }
                }
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Dayclose not found for selected location!');", true);
        }
    }

    protected void btnOpeningBalance_Click(object sender, EventArgs e)
    {
        DataControl dc = new DataControl();
        LedgerController ledgerCtl = new LedgerController();
        DateTime dtOpening = Constants.DateNullValue;
        if (txtOpeningDate.Text.Length > 0)
        {
            dtOpening = Convert.ToDateTime(txtOpeningDate.Text);
        }
        DataTable dtConfig = GetCOAConfiguration();
        bool IsFinanceSetting = GetFinanceConfig();
        long MasterID = Constants.LongNullValue;
        try
        {
            if (Convert.ToDecimal(txtOpeningBalance.Text) == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Please enter opening.');", true);
                return;
            }
        }
        catch (Exception)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Please enter opening.');", true);
            return;
        }
        if (Session["FranchiseModule"].ToString() == "1")
        {
            int VendorID = 0;
            DataTable dtVendors = (DataTable)Session["dtVendors"];
            foreach (DataRow dr in dtVendors.Rows)
            {
                if (dr["VendorID"].ToString() == drpPrincipal.SelectedItem.Value.ToString())
                {
                    VendorID = Convert.ToInt32(dr["SupplierLocationID"]);
                    break;
                }
            }
            if (btnSaveOpeningBalance.Text == "Save" && masterId.Value == "0")
            {
                if (ledgerCtl.InsertVendorOpeningFranchise(Convert.ToInt32(drpDistributor.SelectedItem.Value), txtOpeningBalanceRemarks.Text, int.Parse(rblOpening.SelectedValue), Convert.ToDateTime(txtOpeningDate.Text), VendorID, Convert.ToDecimal(dc.chkNull_0(txtOpeningBalance.Text)), int.Parse(this.Session["UserId"].ToString()), ref MasterID, dtConfig, IsFinanceSetting))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Opening Added Successfully.');", true);
                    btnSaveOpeningBalance.Text = "Update";
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('some error occurred.');", true);
                }
            }
            else
            {
                if (ledgerCtl.DeletePrincipalOpening(Convert.ToInt32(drpDistributor.SelectedItem.Value), VendorID, Convert.ToInt64(masterId.Value)))
                {
                    if (ledgerCtl.InsertVendorOpeningFranchise(Convert.ToInt32(drpDistributor.SelectedItem.Value), txtOpeningBalanceRemarks.Text, int.Parse(rblOpening.SelectedValue), Convert.ToDateTime(txtOpeningDate.Text), VendorID, Convert.ToDecimal(dc.chkNull_0(txtOpeningBalance.Text)), int.Parse(this.Session["UserId"].ToString()), ref MasterID, dtConfig, IsFinanceSetting))
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Opening Updated Successfully.');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('some error occurred.');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('some error occurred.');", true);
                }
            }
        }
        else
        {
            if (btnSaveOpeningBalance.Text == "Save" && masterId.Value == "0")
            {
                if (ledgerCtl.InsertVendorOpening(Convert.ToInt32(drpDistributor.SelectedItem.Value), txtOpeningBalanceRemarks.Text, int.Parse(rblOpening.SelectedValue), Convert.ToDateTime(txtOpeningDate.Text), Convert.ToInt32(drpPrincipal.SelectedItem.Value), Convert.ToDecimal(dc.chkNull_0(txtOpeningBalance.Text)), int.Parse(this.Session["UserId"].ToString()), ref MasterID, dtConfig, IsFinanceSetting))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Opening Added Successfully.');", true);
                    btnSaveOpeningBalance.Text = "Update";
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('some error occurred.');", true);
                }
            }
            else
            {
                if (ledgerCtl.DeletePrincipalOpening(Convert.ToInt32(drpDistributor.SelectedItem.Value), Convert.ToInt32(drpPrincipal.SelectedItem.Value), Convert.ToInt64(masterId.Value)))
                {
                    if (ledgerCtl.InsertVendorOpening(Convert.ToInt32(drpDistributor.SelectedItem.Value), txtOpeningBalanceRemarks.Text, int.Parse(rblOpening.SelectedValue), Convert.ToDateTime(txtOpeningDate.Text), Convert.ToInt32(drpPrincipal.SelectedItem.Value), Convert.ToDecimal(dc.chkNull_0(txtOpeningBalance.Text)), int.Parse(this.Session["UserId"].ToString()), ref MasterID, dtConfig, IsFinanceSetting))
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Opening Updated Successfully.');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('some error occurred.');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('some error occurred.');", true);
                }
            }
        }
    }
   
    #endregion
    protected void drpDistributor_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadOpeningInformation();
    }
    protected void drpPrincipal_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadOpeningInformation();
    }

    private bool IsDayClosed()
    {
        DistributorController DistrCtl = new DistributorController();
        try
        {
            DataTable dtDayClose = DistrCtl.MaxDayClose(Convert.ToInt32(Session["DISTRIBUTOR_ID"]), 3);
            if (dtDayClose.Rows.Count > 0)
            {
                if (Convert.ToDateTime(Session["CurrentWorkDate"]) == Convert.ToDateTime(dtDayClose.Rows[0]["DayClose"]))
                {
                    return false;
                }
            }
            return true;
        }
        catch (Exception)
        {

            throw;
        }
    }

    private DataTable GetCOAConfiguration()
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
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        txtOpeningBalance.Text = "0";
        txtOpeningBalanceRemarks.Text = string.Empty;
        btnSaveOpeningBalance.Text = "Save";
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