using System;
using System.Data;
using System.Web;
using System.Web.UI;
using CORNCommon.Classes;
using CORNBusinessLayer.Classes;
using System.Web.UI.WebControls;

/// <summary>
/// From to Assign, UnAssign Forms To Users
/// </summary>
public partial class Forms_frmSupplierAdvanceKnockOff : System.Web.UI.Page
{
    readonly RoleManagementController mController = new RoleManagementController();
    readonly LedgerController LController = new LedgerController();
    readonly DataControl dc = new DataControl();

    /// <summary>
    /// Page_Load Function Populates All Combos and ListBox On The Page
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        Response.Cache.SetNoStore();
        Response.AppendHeader("pragma", "no-cache");

        if (!Page.IsPostBack)
        {
            LoadDistributor();
            LoadSupplier();
            GetGridData();
            DataTable dtConfig = GetCOAConfiguration();
            Session.Add("dtConfig", dtConfig);
        }
    }

    private void LoadDistributor()
    {
        DistributorController DController = new DistributorController();
        DataTable dt = DController.GetDistributorWithMaxDayClose(Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), int.Parse(this.Session["CompanyId"].ToString()), 1);
        clsWebFormUtil.FillDxComboBoxList(ddlLocation, dt, "DISTRIBUTOR_ID", "DISTRIBUTOR_NAME");
        if (dt.Rows.Count > 0)
        {
            ddlLocation.SelectedIndex = 0;
        }
    }
    private void LoadSupplier()
    {
        if (Session["FranchiseModule"].ToString() == "1")
        {
            DataTable dtVendors = (DataTable)Session["dtVendors"];
            clsWebFormUtil.FillDxComboBoxList(ddlSupplier, dtVendors, "VendorID", "VendorName");
        }
        else
        {
            SKUPriceDetailController PController = new SKUPriceDetailController();
            DataTable m_dt = PController.SelectDataPrice(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), Constants.IntNullValue, 0, DateTime.Parse(this.Session["CurrentWorkDate"].ToString()));
            clsWebFormUtil.FillDxComboBoxList(this.ddlSupplier, m_dt, 0, 1);
        }
        if (ddlSupplier.Items.Count > 0)
        {
            ddlSupplier.SelectedIndex = 0;
        }
        else
        {
            ddlSupplier.SelectedIndex = -1;
        }
    }

    private void GetGridData()
    {
        int VendorID = int.Parse(ddlSupplier.SelectedItem.Value.ToString());
        int DocumentTypeID = 2;
        LedgerController CDC = new LedgerController();
        grdInvoice.DataSource = null;
        grdInvoice.DataBind();
        if (Session["FranchiseModule"].ToString() == "1")
        {
            DataTable dtVendors = (DataTable)Session["dtVendors"];
            foreach (DataRow dr in dtVendors.Rows)
            {
                if (dr["VendorID"].ToString() == ddlSupplier.SelectedItem.Value.ToString())
                {
                    VendorID = Convert.ToInt32(dr["SupplierLocationID"]);
                    if (dr["VendorType"].ToString() == "2")
                    {
                        DocumentTypeID = 11;
                    }
                    break;
                }
            }
        }
        if (ddlSupplier.Items.Count > 0)
        {
            DataTable dtCredit = CDC.SelectCreditPendingInvoice2(int.Parse(ddlLocation.SelectedItem.Value.ToString()), VendorID, 2, DocumentTypeID);
            grdInvoice.DataSource = dtCredit;
            grdInvoice.DataBind();
            decimal totalINvoice = 0;
            if (dtCredit != null)
            {
                if (dtCredit.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCredit.Rows)
                    {
                        totalINvoice = totalINvoice + Convert.ToDecimal(dr["CURRENT_CREDIT_AMOUNT"].ToString());

                    }
                }
                txtInvoicesTotal.Text = totalINvoice.ToString("N2");
            }
        }
        grdAdvance.DataSource = null;
        grdAdvance.DataBind();
        if (ddlSupplier.Items.Count > 0)
        {
            DataTable dtCredit = CDC.SelectCreditPendingInvoice2(int.Parse(ddlLocation.SelectedItem.Value.ToString()), VendorID, 1, DocumentTypeID);
            grdAdvance.DataSource = dtCredit;
            grdAdvance.DataBind();
            decimal totalAdvance = 0;
            if (dtCredit != null)
            {
                if (dtCredit.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCredit.Rows)
                    {
                        totalAdvance = totalAdvance + Convert.ToDecimal(dr["CURRENT_CREDIT_AMOUNT"].ToString());
                    }
                }
                txtAdvanceTotal.Text = totalAdvance.ToString("N2");
            }
        }
    }

    protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetGridData();
    }

    protected void ddlSupplier_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetGridData();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        int VendorID = int.Parse(ddlSupplier.SelectedItem.Value.ToString());
        int DocumentTypeID = 2;
        if (Session["FranchiseModule"].ToString() == "1")
        {
            DataTable dtVendors = (DataTable)Session["dtVendors"];
            foreach (DataRow dr in dtVendors.Rows)
            {
                if (dr["VendorID"].ToString() == ddlSupplier.SelectedItem.Value.ToString())
                {
                    VendorID = Convert.ToInt32(dr["SupplierLocationID"]);
                    if (dr["VendorType"].ToString() == "2")
                    {
                        DocumentTypeID = 11;
                    }
                    break;
                }
            }
        }

        decimal totalamount = 0;
        decimal OfferAmount = 0;
        int Count = 0;
        int Countadvance = 0;
        //decimal p_Amount = 0;
        decimal realizeAmount = 0;
        long AccountHeadID = 0;
        int PaymentMode = 19;
        foreach (GridViewRow dr in grdInvoice.Rows)
        {
            CheckBox chRelized = (CheckBox)dr.Cells[0].FindControl("ChbIsAssigned");
            if (chRelized.Checked == true)
            {
                Count++;
                totalamount = totalamount + decimal.Parse(dr.Cells[3].Text);
            }
        }
        if (Count == 0)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Please Select An Invoice.');", true);
            return;
        }

        foreach (GridViewRow dr in grdAdvance.Rows)
        {
            CheckBox chRelized = (CheckBox)dr.Cells[0].FindControl("ChbIsAssigned");
            if (chRelized.Checked == true)
            {
                OfferAmount = OfferAmount + decimal.Parse(dr.Cells[3].Text);
                //p_Amount = p_Amount + decimal.Parse(dr.Cells[3].Text);
                Countadvance++;
            }
        }
        if (Countadvance == 0)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Please Select An Advance.');", true);
            return;
        }

        try
        {
            string MaxDocumentId = LController.SelectLedgerMaxDocumentId(Constants.Journal_Voucher, int.Parse(ddlLocation.SelectedItem.Value.ToString()), 1);
            Session.Add("VoucherNo", MaxDocumentId);

            DataRow[] drConfig = null;
            DataTable dtConfig = (DataTable)Session["dtConfig"];

            drConfig = dtConfig.Select("CODE = '" + (int)Enums.COAMapping.AccountPayable + "'");
            int PayableAccount = Convert.ToInt32(drConfig[0]["VALUE"].ToString());

            foreach (GridViewRow dr in grdAdvance.Rows)
            {
                CheckBox chRelized = (CheckBox)dr.Cells[0].FindControl("ChbIsAssigned");
                if (chRelized.Checked == true)
                {
                    if (decimal.Parse(dr.Cells[3].Text) <= totalamount)
                    {
                        LController.PostingPrinvipalInvoiceAccount(Constants.Journal_Voucher, long.Parse(MaxDocumentId), PayableAccount, int.Parse(ddlLocation.SelectedItem.Value.ToString()), decimal.Parse(dr.Cells[3].Text), 0,DateTime.Parse(Session["CurrentWorkDate"].ToString()), txtRemarks.Text, DateTime.Now, VendorID, DocumentTypeID, null, int.Parse(Session["UserId"].ToString()), Convert.ToInt64(grdAdvance.DataKeys[dr.RowIndex].Values["PURCHASE_MASTER_ID"]), "0", Convert.ToInt32(dr.Cells[4].Text), "KnockOff", "", Constants.DateNullValue, true);
                        LController.PostingPrinvipalInvoiceAccount(Constants.Journal_Voucher, long.Parse(MaxDocumentId), Convert.ToInt64(dr.Cells[5].Text), int.Parse(ddlLocation.SelectedItem.Value.ToString()), 0, decimal.Parse(dr.Cells[3].Text),DateTime.Parse(Session["CurrentWorkDate"].ToString()), txtRemarks.Text, DateTime.Now, VendorID, DocumentTypeID, null, int.Parse(Session["UserId"].ToString()), Convert.ToInt64(grdAdvance.DataKeys[dr.RowIndex].Values["PURCHASE_MASTER_ID"]), "0", Convert.ToInt32(dr.Cells[4].Text), "KnockOff", "", Constants.DateNullValue, true);
                        totalamount = totalamount - decimal.Parse(dr.Cells[3].Text);

                        PaymentMode = Convert.ToInt32(dr.Cells[4].Text);
                        AccountHeadID = Convert.ToInt64(dr.Cells[5].Text);
                    }
                    else if (decimal.Parse(dr.Cells[3].Text) >= totalamount)
                    {
                        LController.PostingPrinvipalInvoiceAccount(Constants.Journal_Voucher, long.Parse(MaxDocumentId), PayableAccount, int.Parse(ddlLocation.SelectedItem.Value.ToString()), totalamount, 0,DateTime.Parse(Session["CurrentWorkDate"].ToString()), txtRemarks.Text, DateTime.Now, VendorID, DocumentTypeID, null, int.Parse(Session["UserId"].ToString()), Convert.ToInt64(grdAdvance.DataKeys[dr.RowIndex].Values["PURCHASE_MASTER_ID"]), "0", Convert.ToInt32(dr.Cells[4].Text), "KnockOff", "", Constants.DateNullValue, true);

                        LController.PostingPrinvipalInvoiceAccount(Constants.Journal_Voucher, long.Parse(MaxDocumentId), Convert.ToInt64(dr.Cells[5].Text), int.Parse(ddlLocation.SelectedItem.Value.ToString()), 0, totalamount,DateTime.Parse(Session["CurrentWorkDate"].ToString()), txtRemarks.Text, DateTime.Now, VendorID, DocumentTypeID, null, int.Parse(Session["UserId"].ToString()), Convert.ToInt64(grdAdvance.DataKeys[dr.RowIndex].Values["PURCHASE_MASTER_ID"]), "0", Convert.ToInt32(dr.Cells[4].Text), "KnockOff", "", Constants.DateNullValue, true);
                        totalamount = decimal.Parse(dr.Cells[3].Text) - totalamount;
                        PaymentMode = Convert.ToInt32(dr.Cells[4].Text);
                        AccountHeadID = Convert.ToInt64(dr.Cells[5].Text);
                        break;
                    }
                }
            }

            foreach (GridViewRow dr in grdInvoice.Rows)
            {
                CheckBox chRelized = (CheckBox)dr.Cells[0].FindControl("ChbIsAssigned");
                if (chRelized.Checked == true)
                {
                    if (decimal.Parse(dr.Cells[3].Text) >= OfferAmount)
                    {
                        realizeAmount += OfferAmount;
                        long LedgerID = LController.PostingPrinvipalInvoiceAccount(Constants.Journal_Voucher, long.Parse(MaxDocumentId),PayableAccount, int.Parse(ddlLocation.SelectedItem.Value.ToString()), 0, OfferAmount,Convert.ToDateTime(Session["CurrentWorkDate"]), txtRemarks.Text, DateTime.Now, VendorID, DocumentTypeID, "", int.Parse(Session["UserId"].ToString()), Convert.ToInt64(grdInvoice.DataKeys[dr.RowIndex].Values["PURCHASE_MASTER_ID"]), "0", PaymentMode, "KnockOff", "", Constants.DateNullValue, true);
                        if (LedgerID > 0)
                        {
                            LController.PostingPrinvipalInvoiceAccount(Constants.Journal_Voucher, long.Parse(MaxDocumentId), AccountHeadID, int.Parse(ddlLocation.SelectedItem.Value.ToString()), OfferAmount, 0, Convert.ToDateTime(Session["CurrentWorkDate"]), txtRemarks.Text, DateTime.Now, VendorID, DocumentTypeID, "", int.Parse(Session["UserId"].ToString()), Convert.ToInt64(grdInvoice.DataKeys[dr.RowIndex].Values["PURCHASE_MASTER_ID"]), "0", 33, "KnockOff", "", Constants.DateNullValue, true);
                            OfferAmount = decimal.Parse(dr.Cells[3].Text) - OfferAmount;
                            LController.UpdatePurchaseMaster(Convert.ToInt64(grdInvoice.DataKeys[dr.RowIndex].Values["PURCHASE_MASTER_ID"]), int.Parse(ddlLocation.SelectedItem.Value.ToString()), OfferAmount);
                        }
                        break;
                    }
                    else if (decimal.Parse(dr.Cells[3].Text) <= OfferAmount)
                    {
                        realizeAmount += decimal.Parse(dr.Cells[3].Text);
                        long LedgerID = LController.PostingPrinvipalInvoiceAccount(Constants.Journal_Voucher, long.Parse(MaxDocumentId), PayableAccount, int.Parse(ddlLocation.SelectedItem.Value.ToString()), 0, decimal.Parse(dr.Cells[3].Text), Convert.ToDateTime(Session["CurrentWorkDate"]), txtRemarks.Text, DateTime.Now, VendorID, DocumentTypeID, "", int.Parse(Session["UserId"].ToString()), Convert.ToInt64(grdInvoice.DataKeys[dr.RowIndex].Values["PURCHASE_MASTER_ID"]), "0", PaymentMode, "KnockOff", "", Constants.DateNullValue, true);
                        if (LedgerID > 0)
                        {
                            LController.PostingPrinvipalInvoiceAccount(Constants.Journal_Voucher, long.Parse(MaxDocumentId), AccountHeadID, int.Parse(ddlLocation.SelectedItem.Value.ToString()), decimal.Parse(dr.Cells[3].Text), 0, Convert.ToDateTime(Session["CurrentWorkDate"]), txtRemarks.Text, DateTime.Now, VendorID, DocumentTypeID, "", int.Parse(Session["UserId"].ToString()), Convert.ToInt64(grdInvoice.DataKeys[dr.RowIndex].Values["PURCHASE_MASTER_ID"]), "0", PaymentMode, "KnockOff", "", Constants.DateNullValue, true);
                            OfferAmount = OfferAmount - decimal.Parse(dr.Cells[3].Text);
                            LController.UpdatePurchaseMaster(long.Parse(dr.Cells[1].Text), int.Parse(ddlLocation.SelectedItem.Value.ToString()), 0);
                        }
                    }
                }
            }
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Knock off saved successfully.');", true);
            GetGridData();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('" + ex.Message + "');", true);
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
            ScriptManager.RegisterStartupScript(this, GetType(), "msg3", string.Format("alert('Error Occured: \n{0}');", ex), true);
            return null;
        }
    }
}