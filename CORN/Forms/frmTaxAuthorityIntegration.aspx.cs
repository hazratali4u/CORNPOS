using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using System.Web;

public partial class Forms_frmTaxAuthorityIntegration : System.Web.UI.Page
{

    readonly SKUPriceLevelController SKUPrice = new SKUPriceLevelController();
    readonly TaxAuthorityController TaxCtrl = new TaxAuthorityController();
    readonly DataControl _dc = new DataControl();

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        Response.Cache.SetNoStore();
        Response.AppendHeader("pragma", "no-cache");
        try
        {
            if (!IsPostBack)
            {
                Session.Remove("dtGridData");
                LoadDistributor();
                LoadGridData();
                LoadGridMaster("");
                txtURL.Text = "https://gw.fbr.gov.pk/imsp/v1/api/Live/PostData";
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
        var dController = new DistributorController();
        DataTable dt = dController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(Session["UserId"].ToString()), int.Parse(Session["CompanyId"].ToString()));
        clsWebFormUtil.FillDxComboBoxList(drpDistributor, dt, 0, 2, true);
        if(drpDistributor.Items.Count > 0)
        {
            drpDistributor.SelectedIndex = 0;
        }
    }
    private void LoadGridData()
    {
        DataTable dt = new DataTable();
        dt = TaxCtrl.GetTaxAuthority(Constants.IntNullValue, 1);
        Session.Add("dtGridData", dt);
    }
    private void LoadGridMaster(string pType)
    {
        DataTable dt = new DataTable();
        dt = TaxCtrl.GetTaxAuthority(Constants.IntNullValue,1);
        if (pType == "")
        {
            if (txtSearch.Text != "" || txtSearch.Text != string.Empty)
            {
                dt.DefaultView.RowFilter = " POSID LIKE '%" + txtSearch.Text + "%'";
            }
            grdPrice.DataSource = dt;
            grdPrice.DataBind();
        }
        else
        {
            if (txtSearch.Text != "" || txtSearch.Text != string.Empty)
            {
                dt.DefaultView.RowFilter = " POSID LIKE '%" + txtSearch.Text + "%'";
            }
            else
            {
                dt.DefaultView.RowFilter = null;
            }
            grdPrice.DataSource = dt;
            grdPrice.DataBind();
        }
        Session.Add("dtMaster", dt);
    }

    #endregion

    #region Click

    protected void btnSave_Click(object sender, EventArgs e)
    {
        mPopUpSection.Show();
        if(txtPOSID.Text.Length==0 || txtToken.Text.Length ==0 || txtURL.Text.Length==0)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('All fields are mandatory!');", true);
            return;
        }
        if (txtPOSID.Text.Trim().Length > 0)
        {
            DataTable dtItemPriceDetail = new DataTable();
            dtItemPriceDetail.Columns.Add("PRICE", typeof(decimal));
            dtItemPriceDetail.Columns.Add("SKU_ID", typeof(int));
            if (btnSave.Text == "Save")
            {
                bool IsExist = false;
                foreach (GridViewRow gvr in grdPrice.Rows)
                {
                    if(gvr.Cells[5].Text == drpDistributor.SelectedItem.Value.ToString() && gvr.Cells[4].Text == txtURL.Text)
                    {
                        IsExist = true;
                        break;
                    }
                }
                if (!IsExist)
                {
                    if (TaxCtrl.InsertTaxAuthority(Convert.ToInt32(drpDistributor.SelectedItem.Value), txtPOSID.Text, txtToken.Text, txtURL.Text,txtInvocieLabel.Text, Convert.ToInt32(Session["UserID"])))
                    {
                        txtPOSID.Text = string.Empty;
                        txtToken.Text = string.Empty;
                        LoadGridData();
                        LoadGridMaster("");
                        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Successfully Save');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Some error occured');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Tax Integration already exists for this location');", true);
                }
            }
            else
            {
                if (TaxCtrl.UpdateTaxAuthority(Convert.ToInt32(hfMasterId.Value), Convert.ToInt32(drpDistributor.SelectedItem.Value), txtPOSID.Text, txtToken.Text, txtURL.Text,txtInvocieLabel.Text, Convert.ToInt32(Session["UserID"])))
                {
                    ddlTaxAuthority.Enabled = true;
                    txtPOSID.Text = string.Empty;
                    txtToken.Text = string.Empty;
                    btnSave.Text = "Save";
                    ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Successfully updated');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Some error occured');", true);
                }
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Enter Price Level Name.');", true);
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ddlTaxAuthority.Enabled = true;
        Clear();
        hfMasterId.Value = string.Empty;
    }

    protected void btnsearch_Click(object sender, EventArgs e)
    {
        this.LoadGridMaster("filter");
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {        
        mPopUpSection.Show();
    }

    protected void btnClose_ServerClick(object sender, EventArgs e)
    {
        ddlTaxAuthority.Enabled = true;
        Clear();
        hfMasterId.Value = string.Empty;
    }
    #endregion

    private void Clear()
    {
        txtPOSID.Text = string.Empty;
        txtToken.Text = string.Empty;
        txtURL.Text = string.Empty;
        btnSave.Text = "Save";
        drpDistributor.Enabled = true;
        drpDistributor.SelectedIndex = 0;
    }

    protected void grdPrice_RowEditing(object sender, GridViewEditEventArgs e)
    {
        btnSave.Text = "Update";
        hfMasterId.Value = grdPrice.Rows[e.NewEditIndex].Cells[0].Text;
        txtPOSID.Text = grdPrice.Rows[e.NewEditIndex].Cells[2].Text;
        txtToken.Text = grdPrice.Rows[e.NewEditIndex].Cells[3].Text;
        txtURL.Text = grdPrice.Rows[e.NewEditIndex].Cells[4].Text;
        drpDistributor.SelectedItem.Value = grdPrice.Rows[e.NewEditIndex].Cells[5].Text;
        txtInvocieLabel.Text = grdPrice.Rows[e.NewEditIndex].Cells[6].Text;
        ddlTaxAuthority.Enabled = false;
        mPopUpSection.Show();
    }

    protected void grdPrice_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        mPopUpSection.Show();
    }    

    protected void grdPrice_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            DataTable dtMaster = (DataTable)this.Session["dtMaster"];            
            if(dtMaster.Rows.Count > 0)
            {
                DataRow dr = dtMaster.Rows[e.RowIndex];
                if (TaxCtrl.DeleteTaxAuthority(Convert.ToInt32(dr["FBRIntegrationID"]), Convert.ToInt32(Session["UserID"])))                    
                {
                    LoadGridData();
                    LoadGridMaster("");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Succesfully deleted.')", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Some error occured.')", true);
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    protected void ddlTaxAuthority_SelectedIndexChanged(object sender, EventArgs e)
    {
        mPopUpSection.Show();
        if (ddlTaxAuthority.SelectedItem.Value.ToString() == "1")
        {
            txtURL.Text = "https://gw.fbr.gov.pk/imsp/v1/api/Live/PostData";
        }
        else if (ddlTaxAuthority.SelectedItem.Value.ToString() == "2")
        {
            txtURL.Text = "https://ims.pral.com.pk/ims/production/api/Live/PostData";
        }
        else
        {
            txtURL.Text = "https://pos.srb.gos.pk/PoSService/CloudSalesInvoiceService";
        }
    }
}