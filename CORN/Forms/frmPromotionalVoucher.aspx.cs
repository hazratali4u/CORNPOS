using System;
using System.Data;
using System.Web.UI.WebControls;
using CORNCommon.Classes;
using CORNBusinessLayer.Classes;
using System.Web;
using System.Web.UI;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using CORNDatabaseLayer.Classes;

public partial class frmPromotionalVoucher : System.Web.UI.Page
{
    readonly DataControl dc = new DataControl();
    readonly CustomerDataController cController = new CustomerDataController();
    readonly LoyaltyController lController = new LoyaltyController();
    readonly DistributorController mController = new DistributorController();
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        Response.Cache.SetNoStore();
        Response.AppendHeader("pragma", "no-cache");
        //=========================================================

        try
        {
            if (!IsPostBack)
            {
                Session.Remove("dtGridData");
                LoadGridData();
                LoadGrid("");
                btnSave.Attributes.Add("onclick", "return ValidateForm()");
                LoadDistributor();
                LoadCustomer();
                contentBox.Visible = false;
                lookupBox.Visible = true;
                btnClose.Visible = false;
                btnSave.Visible = false;

                txtExpiry.Attributes.Add("readonly", "true");
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    #region Load

    private void LoadDistributor()
    {
        DistributorController mController = new DistributorController();
        try
        {
            DataTable dtDistributor = mController.SelectDistributorInfo(Constants.IntNullValue,
                int.Parse(this.Session["UserId"].ToString()), int.Parse(this.Session["CompanyId"].ToString()));

            clsWebFormUtil.FillDxComboBoxList(drpDistributor, dtDistributor, "DISTRIBUTOR_ID", "DISTRIBUTOR_NAME", true);


            if (dtDistributor.Rows.Count > 0)
            {
                drpDistributor.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
            ExceptionPublisher.PublishException(ex);
            ScriptManager.RegisterStartupScript(this, typeof(Page), "CatchMsg", "alert('" + ex.Message.ToString() + "');", true);
        }
    }

    private void LoadCustomer()
    {
        CustomerDataController DController = new CustomerDataController();
        if (drpDistributor.SelectedIndex != -1 && drpDistributor.Items.Count > 0)        {
            DataTable dt = DController.SelectAllCustomer(3);
            clsWebFormUtil.FillDxComboBoxList(ddlCustomer, dt, "CUSTOMER_ID", "CUSTOMER_NAME");
            if (dt.Rows.Count > 0)
            {
                ddlCustomer.SelectedIndex = 0;
            }
        }
    }
    private void LoadGridData()
    {
        DataTable dt = new DataTable();
        ChequeEntryController cEController = new ChequeEntryController();
        dt = cEController.SelectPromotionVoucher();
        Session.Add("dtGridData", dt);
    }
    private void LoadGrid(string pType)
    {
        GrdCustomer.DataSource = null;
        GrdCustomer.DataBind();
        DataTable dt = (DataTable)Session["dtGridData"];
        if (pType == "")
        {
            if (txtSearch.Text != "" || txtSearch.Text != string.Empty)
            {
                dt.DefaultView.RowFilter = "VOUCHER_NAME LIKE '%" + txtSearch.Text + "%' OR VOUCHER_CODE LIKE '%" + txtSearch.Text + "%' OR CUSTOMER_NAME LIKE '" + txtSearch.Text + "%'";
            }
            GrdCustomer.DataSource = dt;
            GrdCustomer.DataBind();
        }
        else
        {
            if (txtSearch.Text != "" || txtSearch.Text != string.Empty)
            {
                dt.DefaultView.RowFilter = "VOUCHER_NAME LIKE '%" + txtSearch.Text + "%' OR VOUCHER_CODE LIKE '%" + txtSearch.Text + "%' OR CUSTOMER_NAME LIKE '" + txtSearch.Text + "%' OR DISTRIBUTOR_NAME LIKE '%" + txtSearch.Text + "%'";
            }
            else
            {
                dt.DefaultView.RowFilter = null;
            }
            if (dt.Rows.Count > 0)
            {
                GrdCustomer.PageIndex = 0;
            }
            GrdCustomer.DataSource = dt;
            GrdCustomer.DataBind();
        }
    }

    #endregion

    #region Index/change

    protected void drpDistributor_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCustomer();
    }

    #endregion

    #region Click

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearAll();
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                long ChqFromNo = Convert.ToInt64(txtCheckfrom.Text);
                long ChqToNo = Convert.ToInt64(txtcheckto.Text);
                if (ChqFromNo > ChqToNo)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Invalid Serial range');", true);
                    return;
                }

                lblErrorMsg.Text = "";
                bool flag = true;
                ChequeEntryController cEController = new ChequeEntryController();

                switch (btnSave.Text)
                {
                    case "Save":

                        cEController.InsertPromotionalVoucher(Convert.ToInt32(drpDistributor.SelectedItem.Value),
                            Convert.ToInt64(ddlCustomer.SelectedItem.Value), txtVoucherCode.Text,
                            txtVoucherName.Text,
                            !string.IsNullOrEmpty(txtExpiry.Text) ? txtExpiry.Text : Constants.DateNullValue.ToString(),
                            txtPrefix.Text, txtCheckfrom.Text, txtcheckto.Text, txtremarks.Text, 0);

                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Record added successfully.');", true);

                        flag = true;
                        break;

                    case "Update":


                        cEController.InsertPromotionalVoucher(Convert.ToInt32(drpDistributor.SelectedItem.Value),
                           Convert.ToInt64(ddlCustomer.SelectedItem.Value), txtVoucherCode.Text,
                           txtVoucherName.Text,
                           !string.IsNullOrEmpty(txtExpiry.Text) ? txtExpiry.Text : Constants.DateNullValue.ToString(),
                           txtPrefix.Text, txtCheckfrom.Text, txtcheckto.Text, txtremarks.Text, 
                           Convert.ToInt64(hfCustomerID.Value.ToString()));


                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Record updated successfully.');", true);
                        flag = true;
                        break;
                }
                if (flag)
                {
                    ClearAll();
                    LoadGridData();
                    LoadGrid("");
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "CatchMsg", "alert('" + ex.Message.ToString() + "');", true);
        }
    }


    protected void btnFilter_Click(object sender, EventArgs e)
    {
        LoadGrid("filter");

    }
    protected void btnClose_Click(object sender, EventArgs e)
    {
        ClearAll();
        contentBox.Visible = false;
        lookupBox.Visible = true;
        searchBox.Visible = true;
        searchBtn.Visible = true;
        btnActive.Visible = true;
        btnClose.Visible = false;
        btnSave.Visible = false;
        btnAdd.Visible = true;
    }
    protected void btnActive_Click(object sender, EventArgs e)
    {
        bool check = false;
        try
        {
            foreach (GridViewRow dr2 in GrdCustomer.Rows)
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
            UserController UserCntrl = new UserController();

            bool flag = false;
            foreach (GridViewRow dr in GrdCustomer.Rows)
            {
                var chRelized = (CheckBox)dr.Cells[0].FindControl("ChbIsAssigned");


                if (chRelized.Checked)
                {
                    if (Convert.ToString(dr.Cells[10].Text) == "Active")
                    {
                        UserCntrl.ActiveInactive(true, Convert.ToInt32(dr.Cells[1].Text), Convert.ToInt32(Session["UserID"]), 22);

                        flag = true;
                    }
                    else
                    {
                        UserCntrl.ActiveInactive(false, Convert.ToInt32(dr.Cells[1].Text), Convert.ToInt32(Session["UserID"]), 22);
                        flag = true;
                    }
                }
                if (flag)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Record updated successfully');", true);
                }
                LoadGridData();
                this.LoadGrid("");
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "CatchMsg", "alert('" + ex.Message.ToString() + "');", true);
        }
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        btnSave.Text = "Save";

        contentBox.Visible = true;
        lookupBox.Visible = false;
        searchBox.Visible = false;
        searchBtn.Visible = false;
        btnActive.Visible = false;
        btnClose.Visible = true;
        btnSave.Visible = true;
        btnAdd.Visible = false;
    }
    #endregion

    private void ClearAll()
    {
        txtCheckfrom.Text = "";
        txtcheckto.Text = "";
        txtPrefix.Text = "";
        txtremarks.Text = "";
        txtVoucherCode.Text = "";
        txtVoucherName.Text = "";
        txtExpiry.Text = "";

        lblErrorMsg.Text = "";
        btnSave.Text = "Save";
    }

    #region Grid Operations

    protected void GrdCustomer_RowEditing(object sender, GridViewEditEventArgs e)
    {
        UserController _mUController = new UserController();
        try
        {
            hfCustomerID.Value = GrdCustomer.Rows[e.NewEditIndex].Cells[1].Text;
            drpDistributor.Value = GrdCustomer.Rows[e.NewEditIndex].Cells[2].Text;
            ddlCustomer.Value = GrdCustomer.Rows[e.NewEditIndex].Cells[3].Text;
            txtVoucherCode.Text = GrdCustomer.Rows[e.NewEditIndex].Cells[6].Text.Replace("&nbsp;", "");
            txtVoucherName.Text = GrdCustomer.Rows[e.NewEditIndex].Cells[7].Text.Replace("&nbsp;", "");
            txtExpiry.Text = GrdCustomer.Rows[e.NewEditIndex].Cells[8].Text.Replace("&nbsp;", "");
            txtCheckfrom.Text = GrdCustomer.Rows[e.NewEditIndex].Cells[9].Text.Replace("&nbsp;", "");
            txtcheckto.Text = GrdCustomer.Rows[e.NewEditIndex].Cells[10].Text.Replace("&nbsp;", "");
            txtPrefix.Text = GrdCustomer.Rows[e.NewEditIndex].Cells[11].Text.Replace("&nbsp;", "");
            txtremarks.Text = GrdCustomer.Rows[e.NewEditIndex].Cells[12].Text.Replace("&nbsp;", "");

            btnSave.Text = "Update";

            contentBox.Visible = true;
            lookupBox.Visible = false;
            contentBox.Visible = true;
            lookupBox.Visible = false;
            searchBox.Visible = false;
            searchBtn.Visible = false;
            btnActive.Visible = false;
            btnClose.Visible = true;
            btnSave.Visible = true;
            btnAdd.Visible = false;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "CatchMsg", "alert('" + ex + "');", true);

        }
    }

    protected void grdData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GrdCustomer.PageIndex = e.NewPageIndex;
        LoadGrid("");
    }

    #endregion

}