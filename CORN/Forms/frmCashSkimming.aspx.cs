using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using CORNCommon.Classes;
using CORNBusinessLayer.Classes;
using System.Web;

/// <summary>
/// Form to Add, Edit Users
/// </summary>
public partial class Forms_frmCashSkimming : System.Web.UI.Page
{
    readonly RoleManagementController mController = new RoleManagementController();
    readonly DocumentPrintController _mDocumentPrntControl = new DocumentPrintController();
    readonly SkimmingController sController = new SkimmingController();
    readonly DataControl dc = new DataControl();
    readonly ShiftController _SController = new ShiftController();
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        Response.Cache.SetNoStore();
        Response.AppendHeader("pragma", "no-cache");

        if (!Page.IsPostBack)
        {
            this.GetAppSettingDetail();
            Session.Remove("dtGridData");
            LoadDistributor();
            LoadUser();
            LoadGridData();
            LoadGrid("");
            txtAmount.Attributes.Add("autocomplete", "off");
            txtRemarks.Attributes.Add("autocomplete", "off");
            btnSave.Attributes.Add("onclick", "return ValidateForm()");
            LoadAccountHead();
            expenseRow.Visible = true;
            Grid_users.Columns[5].Visible = true;

            DataTable dtConfig = GetCOAConfiguration();
            bool IsFinanceSetting = GetFinanceConfig();
            Session.Add("dtConfig", dtConfig);
            Session.Add("IsFinanceSetting", IsFinanceSetting);

        }
    }
    private void LoadAccountHead()
    {
        int TypeID = 1;
        if (Session["LocationWiseCOA"].ToString() == "1")
        {
            TypeID = 2;
        }

        AccountHeadController mAccountController = new AccountHeadController();
        DataTable dt = mAccountController.GetAssignAccountHead(TypeID, Constants.IntNullValue, Convert.ToInt32(ddDistributorId.Value));
        clsWebFormUtil.FillDxComboBoxList(DrpAccountHead, dt, 0, 10, true);

        if (dt.Rows.Count > 0)
        {
            DrpAccountHead.SelectedIndex = 0;
        }
    }
    private void LoadDistributor()
    {
       DistributorController mController = new DistributorController();
        try
        {
            DataTable dt = mController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), int.Parse(this.Session["CompanyId"].ToString()));

            clsWebFormUtil.FillDxComboBoxList(ddDistributorId, dt, 0, 2, true);
            if (dt.Rows.Count > 0)
            {
                ddDistributorId.SelectedIndex = 0;
            }
        }
        catch (Exception)
        {

            throw;
        }
    }

    private void LoadUser()
    {
        DrpUser.Items.Clear();
        DrpUser.Enabled = true;
        if (ddDistributorId.Items.Count > 0)
        {
            Distributor_UserController du = new Distributor_UserController();
            DataTable dt1 = du.SelectDistributorUser(3, int.Parse(ddDistributorId.Value.ToString()), int.Parse(Session["CompanyId"].ToString()));
            clsWebFormUtil.FillDxComboBoxList(DrpUser, dt1, 0, 1, true);
            if (dt1.Rows.Count > 0)
            {
                DrpUser.SelectedIndex = 0;
                DrpUser.Value = Session["UserID"].ToString();
                DrpUser.Enabled = false;
            }
        }
        LoadCashAmount();
    }
    public void LoadCashAmount()
    {
        try
        {
            lblCashInHand.Text = "0.00";
            if (ddDistributorId.Items.Count > 0 && DrpUser.Items.Count > 0)
            {
                DataTable dt = _SController.SelectSales(Convert.ToInt32(ddDistributorId.SelectedItem.Value), Convert.ToInt32(DrpUser.SelectedItem.Value), Convert.ToDateTime(Session["CurrentWorkDate"]), Convert.ToDateTime(Session["CurrentWorkDate"]), 8, Constants.IntNullValue);
                DataTable dtsKIM = _SController.SelectSales(Convert.ToInt32(ddDistributorId.SelectedItem.Value), Convert.ToInt32(DrpUser.SelectedItem.Value), Constants.DateNullValue, Constants.DateNullValue, 9, Constants.IntNullValue);
                try
                {
                    if (dt.Rows[0].ItemArray[0] != DBNull.Value)
                    {
                        lblCashInHand.Text = (Math.Round(Convert.ToDecimal(dt.Rows[0].ItemArray[0]), 2) + DataControl.chkDecimalZero(hfOpeningRecieved.Value) + DataControl.chkDecimalZero(hfOpeningCash.Value) - Math.Round(Convert.ToDecimal(dtsKIM.Rows[0].ItemArray[0]), 2)).ToString();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        catch (Exception ex)
        {
            
        }
    }
    public void GetOpeningCash()
    {
        string fromDate = Convert.ToDateTime(Session["CurrentWorkDate"]).ToString("dd-MMM-yyyy");
        string ToDate = Convert.ToDateTime(Session["CurrentWorkDate"]).ToString("dd-MMM-yyyy");
        DateTime _startDate = DateTime.Parse(fromDate + " 00:00:00");
        DateTime _endDate = DateTime.Parse(ToDate + " 23:59:59");
        DataTable dt2 = _SController.SelectSales(Constants.IntNullValue, Convert.ToInt32(DrpUser.SelectedItem.Value), _startDate.AddDays(-1), _endDate.AddDays(-1), 5, Constants.IntNullValue);
        if (dt2.Rows.Count > 0)
        {
            hfOpeningCash.Value = Math.Round(Convert.ToDecimal(dt2.Rows[0].ItemArray[0]), 2).ToString();
        }
        else
        {
            hfOpeningCash.Value = "0";
        }
    }
    public void GetShiftOpeningAmount()
    {
        string fromDate = Convert.ToDateTime(Session["CurrentWorkDate"]).ToString("dd-MMM-yyyy");
        string ToDate = Convert.ToDateTime(Session["CurrentWorkDate"]).ToString("dd-MMM-yyyy");
        DateTime _startDate = DateTime.Parse(fromDate + " 00:00:00");
        DateTime _endDate = DateTime.Parse(ToDate + " 23:59:59");
        //DataTable dt2 = _SController.SelectSales(Constants.IntNullValue, Convert.ToInt32(DrpUser.SelectedItem.Value), _startDate, _endDate, 51, Constants.IntNullValue);
        //if (dt2.Rows.Count > 0)
        //{
        //    hfOpeningRecieved.Value = Math.Round(Convert.ToDecimal(dt2.Rows[0].ItemArray[0]), 2).ToString();
        //}
        //else
        //{
        //    hfOpeningRecieved.Value = "0";
        //}
    }
    private void LoadGridData()
    {
        DataTable dt = new DataTable();
        dt = sController.SelectCashSkimming(Constants.IntNullValue, int.Parse(this.Session["CompanyId"].ToString()), 1);
        Session.Add("dtGridData", dt);
    }
    protected void LoadGrid(string pType)
    {
        Grid_users.DataSource = null;
        Grid_users.DataBind();

        if (ddDistributorId.Items.Count > 0)
        {
            DataTable dt = (DataTable)Session["dtGridData"];
            if (pType == "")
            {
                if (txtSearch.Text != "" || txtSearch.Text != string.Empty)
                {
                    dt.DefaultView.RowFilter = "DISTRIBUTOR_NAME LIKE '%" + txtSearch.Text + "%' OR USER_NAME LIKE '%" + txtSearch.Text + "%' ";
                }
                Grid_users.DataSource = dt;
                Grid_users.DataBind();
            }
            else
            {
                if (txtSearch.Text != "" || txtSearch.Text != string.Empty)
                {
                    dt.DefaultView.RowFilter = "DISTRIBUTOR_NAME LIKE '%" + txtSearch.Text + "%' OR USER_NAME LIKE '%" + txtSearch.Text + "%'  ";
                }
                else
                {
                    dt.DefaultView.RowFilter = null;
                }
                if (dt.Rows.Count > 0)
                {
                    Grid_users.PageIndex = 0;
                }
                Grid_users.DataSource = dt;
                Grid_users.DataBind();
            }
        }
    }

    protected void ddDistributorId_SelectedIndexChanged(object sender, EventArgs e)
    {
        mPopUpLocation.Show();
        LoadUser();
        if (Session["LocationWiseCOA"].ToString() == "1")
        {
            LoadAccountHead();
        }
    }

    protected void DrpUser_SelectedIndexChanged(object sender, EventArgs e)
    {
        mPopUpLocation.Show();
        LoadCashAmount();
    }

    protected void Grid_users_RowEditing(object sender, GridViewEditEventArgs e)
    {
        mPopUpLocation.Show();
        try
        {            
            GridViewRow gvr = Grid_users.Rows[e.NewEditIndex];
            Session.Add("CashSkimmingVoucherNo", Server.HtmlDecode(gvr.Cells[9].Text));
            hfSkimingID.Value = gvr.Cells[0].Text;
            txtAmount.Text = gvr.Cells[6].Text;
            txtRemarks.Text = Server.HtmlDecode(gvr.Cells[7].Text);
            try
            {
                DrpAccountHead.Value = Server.HtmlDecode(gvr.Cells[8].Text);
            }
            catch (Exception)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('No Expense Head mapped')", true);
            }
            LoadDistributor();
            try
            {
                ddDistributorId.Value = gvr.Cells[1].Text;
            }
            catch (Exception)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Relevent location not found')", true);
            }
            try
            {
                LoadUser();
                DrpUser.Value = gvr.Cells[2].Text;
            }
            catch (Exception)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Relevent cashier not found')", true);
            }
            LoadCashAmount();
            Session.Add("OldAmount", gvr.Cells[6].Text);
            btnSave.Text = "Update";
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('" + ex.Message.ToString() + "')", true);
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (DrpUser.Items.Count > 0)
        {
            mPopUpLocation.Show();
            if (Page.IsValid)
            {
                try
                {
                    lblErrorMsg.Text = "";
                    lblErrorMsg.Visible = false;

                    var isvisible = expenseRow.Visible;
                    var expenseHead = Constants.LongNullValue;

                    if (isvisible == true)
                        expenseHead = long.Parse(DrpAccountHead.Value.ToString());

                    if (isvisible == true && DrpAccountHead.SelectedIndex == -1)
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Please select Expense Head');", true);
                        mPopUpLocation.Show();
                        return;
                    }

                    DataTable dtVoucher = new DataTable();
                    LedgerController mLController = new LedgerController();

                    if (expenseHead != Constants.LongNullValue && Convert.ToBoolean(Session["IsFinanceSetting"]))
                    {
                        dtVoucher.Columns.Add("Account_Head_Id", typeof(long));
                        dtVoucher.Columns.Add("Debit", typeof(decimal));
                        dtVoucher.Columns.Add("Credit", typeof(decimal));
                        dtVoucher.Columns.Add("Remarks", typeof(string));
                        dtVoucher.Columns.Add("InvoiceNo", typeof(string));
                        dtVoucher.Columns.Add("Principal_Id", typeof(string));
                        dtVoucher.Columns.Add("Principal", typeof(string));

                        DataRow[] drConfig = null;
                        DataTable dtConfig = (DataTable)Session["dtConfig"];

                        drConfig = dtConfig.Select("CODE = '" + (int)Enums.COAMapping.CashSkimming + "'");
                        int CashSkimming = Convert.ToInt32(drConfig[0]["VALUE"].ToString());

                        //CASH IN HAND PAYMENT voucher
                        DataRow dr1 = dtVoucher.NewRow();
                        dr1["ACCOUNT_HEAD_ID"] = CashSkimming;
                        dr1["REMARKS"] = txtRemarks.Text;
                        dr1["CREDIT"] = Convert.ToDecimal(dc.chkNull_0(txtAmount.Text));
                        dr1["DEBIT"] = 0;
                        dr1["Principal_id"] = "0";
                        dtVoucher.Rows.Add(dr1);

                        //Selected Account Head
                        DataRow dr2 = dtVoucher.NewRow();
                        dr2["ACCOUNT_HEAD_ID"] = DrpAccountHead.SelectedItem.Value.ToString();
                        dr2["REMARKS"] = txtRemarks.Text;
                        dr2["CREDIT"] = 0;
                        dr2["DEBIT"] = Convert.ToDecimal(dc.chkNull_0(txtAmount.Text));
                        dr2["Principal_id"] = "0";
                        dtVoucher.Rows.Add(dr2);
                    }
                    if (btnSave.Text == "Save")
                    {
                        if (Convert.ToDecimal(dc.chkNull_0(txtAmount.Text)) > Convert.ToDecimal(dc.chkNull_0(lblCashInHand.Text)))
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Cash in hand is less than skimming amount.');", true);
                            mPopUpLocation.Show();
                            return;
                        }
                        string SKIMMING_ID = sController.InsertSkimming(Convert.ToInt32(ddDistributorId.SelectedItem.Value), Convert.ToInt32(DrpUser.SelectedItem.Value), Convert.ToDecimal(dc.chkNull_0(txtAmount.Text)), txtRemarks.Text, DateTime.Parse(Session["CurrentWorkDate"].ToString()), expenseHead);
                        if (dtVoucher.Rows.Count > 0)
                        {
                            int VoucherType = 24; //Cash Payment Voucher
                            string MaxDocumentId = mLController.SelectMaxVoucherId(VoucherType, int.Parse(ddDistributorId.SelectedItem.Value.ToString()), Convert.ToDateTime(Session["CurrentWorkDate"]));
                            bool IsSaveVoucher = mLController.AddVoucherCashSkimming(int.Parse(ddDistributorId.SelectedItem.Value.ToString()), 0, MaxDocumentId, VoucherType, Convert.ToDateTime(Session["CurrentWorkDate"]), 19, "", txtRemarks.Text, Constants.DateNullValue, "", dtVoucher, int.Parse(this.Session["UserId"].ToString()), "CashSkimming", Constants.DateNullValue, 27, Convert.ToInt64(SKIMMING_ID));
                        }

                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Record added successfully.');", true);
                        ShowReport();
                    }
                    else if (btnSave.Text == "Update")
                    {
                        if ((Convert.ToDecimal(dc.chkNull_0(txtAmount.Text)) - Convert.ToDecimal(Session["OldAmount"])) > Convert.ToDecimal(dc.chkNull_0(lblCashInHand.Text)))
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Cash in hand is less than skimming amount.');", true);
                            mPopUpLocation.Show();
                            return;
                        }
                        sController.UpdateSkimming(Convert.ToInt32(hfSkimingID.Value), Convert.ToInt32(ddDistributorId.SelectedItem.Value), Convert.ToInt32(DrpUser.SelectedItem.Value), Convert.ToDecimal(dc.chkNull_0(txtAmount.Text)), txtRemarks.Text, expenseHead);
                        if (dtVoucher.Rows.Count > 0)
                        {
                            int VoucherType = 24; //Cash Payment Voucher                            
                            string MaxDocumentId = mLController.SelectMaxVoucherId(VoucherType, int.Parse(ddDistributorId.SelectedItem.Value.ToString()), Convert.ToDateTime(Session["CurrentWorkDate"]));
                            bool IsSaveVoucher = mLController.AddVoucherCashSkimming(int.Parse(ddDistributorId.SelectedItem.Value.ToString()), 0, MaxDocumentId, VoucherType, Convert.ToDateTime(Session["CurrentWorkDate"]), 19, "", txtRemarks.Text, Constants.DateNullValue, "", dtVoucher, int.Parse(this.Session["UserId"].ToString()), "CashSkimming", Constants.DateNullValue, 27, Convert.ToInt64(hfSkimingID.Value));
                        }
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Record updated successfully.');", true);
                        ShowReport();
                        mPopUpLocation.Hide();
                    }
                    LoadCashAmount();
                    LoadGridData();
                    LoadGrid("");
                    ClearControls();
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "CatchMsg", "alert('" + ex.Message.ToString() + "')", true);
                    mPopUpLocation.Show();
                }
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "CatchMsg", "alert('User not slected.')", true);
        }
    }
    private void ShowReport()
    {
        DataTable dt = _mDocumentPrntControl.SelectReportTitle(Convert.ToInt32(ddDistributorId.SelectedItem.Value));
        CORNBusinessLayer.Reports.crpCashSkimmingInvoice crpReport = new CORNBusinessLayer.Reports.crpCashSkimmingInvoice();
        crpReport.Refresh();
        string date = Convert.ToDateTime(Session["CurrentWorkDate"]).ToString("dd-MMM-yyyy");
        crpReport.SetParameterValue("Date", date);
        crpReport.SetParameterValue("USER_NAME", DrpUser.SelectedItem.Text);
        crpReport.SetParameterValue("LOCATION", ddDistributorId.SelectedItem.Text);
        crpReport.SetParameterValue("PHONE_NUMBER", dt.Rows[0]["CONTACT_NUMBER"].ToString());
        crpReport.SetParameterValue("COMPANY_NAME", dt.Rows[0]["COMPANY_NAME"].ToString());

        crpReport.SetParameterValue("AMOUNT", Convert.ToDecimal(txtAmount.Text));
        crpReport.SetParameterValue("REMARKS", txtRemarks.Text);

        var isvisible = expenseRow.Visible;
        crpReport.SetParameterValue("ACCOUNT_HEAD", isvisible == true ? DrpAccountHead.SelectedItem.Text : "");

        Session.Add("CrpReport", crpReport);
        Session.Add("ReportType", 0);
        const string url = "'Default.aspx'";
        const string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
        Type cstype = this.GetType();
        ClientScriptManager cs = Page.ClientScript;
        cs.RegisterStartupScript(cstype, "OpenWindow", script);
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        mPopUpLocation.Show();
        ddDistributorId.SelectedIndex = 0;
        LoadUser();
        ClearControls();
    }

    protected void ClearControls()
    {
        try
        {
            lblErrorMsg.Visible = false;
            lblErrorMsg.Text = "";
            txtAmount.Text = "";
            txtRemarks.Text = "";
            btnSave.Text = "Save";
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
    }
    protected void btnFilter_Click(object sender, EventArgs e)
    {
        this.LoadGrid("filter");
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        mPopUpLocation.Show();
        //DoEmptyTextBox();
    }
    protected void btnActive_Click(object sender, EventArgs e)
    {
        UserController UController = new UserController();
        bool check = false;
        try
        {
            foreach (GridViewRow dr2 in Grid_users.Rows)
            {
                var chRelized2 = (CheckBox)dr2.Cells[0].FindControl("ChbIsAssigned");

                if (chRelized2.Checked)
                {
                    check = true;
                    break;
                }
            }
            if (!check)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Please select record first');", true);
                return;
            }
            bool flag = false;
            foreach (GridViewRow dr in Grid_users.Rows)
            {
                var chRelized = (CheckBox)dr.Cells[0].FindControl("ChbIsAssigned");
                if (chRelized.Checked)
                {
                    if (Convert.ToString(dr.Cells[11].Text) == "Active")
                    {
                        UController.ActiveInactive(false, Constants.IntNullValue, Convert.ToInt32(dr.Cells[1].Text), 2);
                        

                        flag = true;
                    }
                    else
                    {
                        UController.ActiveInactive(true, Constants.IntNullValue, Convert.ToInt32(dr.Cells[1].Text), 2);
                        
                        flag = true;
                    }
                }
            }
            if (flag)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Record updated successfully');", true);
            }
            LoadGridData();
            this.LoadGrid("");
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "CatchMsg", "alert('" + ex.Message.ToString() + "');", true);
        }
    }
    protected void Grid_users_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_users.PageIndex = e.NewPageIndex;
        LoadGrid("");
    }

    protected void btnClose_Click(object sender, EventArgs e)
    {
        ClearControls();
        mPopUpLocation.Hide();
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
            ScriptManager.RegisterStartupScript(this, GetType(), "msg3", string.Format("alert('Error Occured: \n{0}');", ex), true);
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