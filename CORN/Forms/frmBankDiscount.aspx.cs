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

public partial class frmBankDiscount : System.Web.UI.Page
{
    readonly DataControl dc = new DataControl();
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
                contentBox.Visible = false;
                lookupBox.Visible = true;
                btnClose.Visible = false;
                btnSave.Visible = false;
                Configuration.SystemCurrentDateTime = (DateTime)this.Session["CurrentWorkDate"];
                txtFromDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
                txtFromDate.Attributes.Add("readonly", "true");
                txtToDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
                txtToDate.Attributes.Add("readonly", "true");
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
    
    private void LoadGridData()
    {
        DataTable dt = new DataTable();
        BankDiscountController discountCtrl = new BankDiscountController();
        dt = discountCtrl.GetBankDiscount("",1);
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
                dt.DefaultView.RowFilter = "DiscountName LIKE '%" + txtSearch.Text + "%' OR Description LIKE '%" + txtSearch.Text + "%' OR CONVERT(DiscountPer, 'System.String') LIKE '" + txtSearch.Text + "%'";
            }
            GrdCustomer.DataSource = dt;
            GrdCustomer.DataBind();
        }
        else
        {
            if (txtSearch.Text != "" || txtSearch.Text != string.Empty)
            {
                dt.DefaultView.RowFilter = "DiscountName LIKE '%" + txtSearch.Text + "%' OR Description LIKE '%" + txtSearch.Text + "%' OR CONVERT(DiscountPer, 'System.String') LIKE '" + txtSearch.Text + "%'";
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
                if (txtDiscountName.Text.Length > 0 && txtDiscount.Text.Length > 0 && txtLimit.Text.Length > 0 && txtCardNo.Text.Length > 0 && txtPortion.Text.Length > 0)
                {
                    bool flag = true;
                    BankDiscountController discountCtrl = new BankDiscountController();
                    switch (btnSave.Text)
                    {
                        case "Save":
                            flag = discountCtrl.InsertBankDiscount(Convert.ToInt32(drpDistributor.SelectedItem.Value), txtDiscountName.Text, Convert.ToDecimal(DataControl.chkNull_Zero(txtDiscount.Text)), Convert.ToDecimal(DataControl.chkNull_Zero(txtPortion.Text)), Convert.ToDecimal(DataControl.chkNull_Zero(txtLimit.Text)), txtCardNo.Text, txtremarks.Text, Convert.ToDateTime(txtFromDate.Text), Convert.ToDateTime(txtToDate.Text), Convert.ToInt32(Session["UserID"]));
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Record added successfully.');", true);
                            break;
                        case "Update":
                            discountCtrl.UpdateBankDiscount(Convert.ToInt64(hfBankDiscount.Value), Convert.ToInt32(drpDistributor.SelectedItem.Value), txtDiscountName.Text, Convert.ToDecimal(DataControl.chkNull_Zero(txtDiscount.Text)), Convert.ToDecimal(DataControl.chkNull_Zero(txtPortion.Text)), Convert.ToDecimal(DataControl.chkNull_Zero(txtLimit.Text)), txtCardNo.Text, txtremarks.Text, Convert.ToDateTime(txtFromDate.Text), Convert.ToDateTime(txtToDate.Text), Convert.ToInt32(Session["UserID"]));
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
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "CatchMsg", "alert('Discount Name, Discount(%), Cap, Bin No and Bank Portion(%) are required');", true);
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
                    if (Convert.ToString(dr.Cells[9].Text) == "Active")
                    {
                        UserCntrl.ActiveInactive(false, Convert.ToInt32(dr.Cells[1].Text), Convert.ToInt32(Session["UserID"]), 12);

                        flag = true;
                    }
                    else
                    {
                        UserCntrl.ActiveInactive(true, Convert.ToInt32(dr.Cells[1].Text), Convert.ToInt32(Session["UserID"]), 12);
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
        txtremarks.Text = "";
        txtDiscount.Text = "";
        txtPortion.Text = "";
        txtDiscountName.Text = "";
        txtCardNo.Text = "";
        txtLimit.Text = "";
        Configuration.SystemCurrentDateTime = (DateTime)this.Session["CurrentWorkDate"];
        txtFromDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
        txtToDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
        btnSave.Text = "Save";
    }

    #region Grid Operations

    protected void GrdCustomer_RowEditing(object sender, GridViewEditEventArgs e)
    {
        UserController _mUController = new UserController();
        try
        {
            hfBankDiscount.Value = GrdCustomer.Rows[e.NewEditIndex].Cells[1].Text;
            drpDistributor.Value = GrdCustomer.Rows[e.NewEditIndex].Cells[2].Text;
            txtDiscountName.Text = GrdCustomer.Rows[e.NewEditIndex].Cells[3].Text.Replace("&nbsp;", "");
            txtDiscount.Text = GrdCustomer.Rows[e.NewEditIndex].Cells[5].Text.Replace("&nbsp;", "");
            txtremarks.Text = GrdCustomer.Rows[e.NewEditIndex].Cells[6].Text.Replace("&nbsp;", "");
            txtFromDate.Text = Convert.ToDateTime(GrdCustomer.Rows[e.NewEditIndex].Cells[7].Text.Replace("&nbsp;", "")).ToString("dd-MMM-yyyy");
            txtToDate.Text = Convert.ToDateTime(GrdCustomer.Rows[e.NewEditIndex].Cells[8].Text.Replace("&nbsp;", "")).ToString("dd-MMM-yyyy");
            txtLimit.Text = GrdCustomer.Rows[e.NewEditIndex].Cells[10].Text.Replace("&nbsp;", "");
            txtCardNo.Text = GrdCustomer.Rows[e.NewEditIndex].Cells[11].Text.Replace("&nbsp;", "");
            txtPortion.Text = GrdCustomer.Rows[e.NewEditIndex].Cells[12].Text.Replace("&nbsp;", "");
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