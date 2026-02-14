using System;
using System.Data;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using CORNCommon.Classes;
using CORNBusinessLayer.Classes;

/// <summary>
/// From To Add, Edit SKU Price
/// </summary>
public partial class frmToDayMenu : System.Web.UI.Page
{
    readonly SkuHierarchyController _mHerController = new SkuHierarchyController();
    readonly SKUPriceDetailController mSKUController = new SKUPriceDetailController();
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        Response.Cache.SetNoStore();
        Response.AppendHeader("pragma", "no-cache");

        if (!Page.IsPostBack)
        {
            LoadDistributor();
            txtFromdate.Text = Convert.ToDateTime(Session["CurrentWorkDate"]).ToString("dd-MMM-yyyy");
            CEStartDate.StartDate = Convert.ToDateTime(Session["CurrentWorkDate"]);
            btnSave.Attributes.Add("onclick", "return ValidateForm()");
            txtFromdate.Attributes.Add("readonly", "readonly");
            FillddlSkus();
            LoadGrid(DateTime.Parse(txtFromdate.Text));
        }
    }
    
    private void LoadGrid(DateTime DateEffected)
    {
        DataTable dtsku = mSKUController.GetTodayMenu(DateEffected);
        Grid_pricedetails.DataSource = dtsku.DefaultView;
        Grid_pricedetails.DataBind();        
    }
    private void FillddlSkus()
    {
        try
        {
            SkuController mSKUController = new SkuController();
            DataTable dt = mSKUController.SelectSkuInfo(Constants.IntNullValue, Constants.IntNullValue,Convert.ToInt32(Session["TodayMenuID"]), 20, int.Parse(Session["CompanyId"].ToString()), null);
            clsWebFormUtil.FillDxComboBoxList(ddlSkus, dt, "SKU_ID", "SKU_NAME", true);
            if (ddlSkus.Items.Count > 0)
            {
                ddlSkus.SelectedIndex = 0;
            }
        }
        catch (Exception)
        {
        }
    }

    private void LoadDistributor()
    {
        DistributorController DController = new DistributorController();
        DataTable dt = DController.SelectDistributorInfo(-2, int.Parse(Session["UserId"].ToString()), int.Parse(Session["CompanyId"].ToString()));
        clsWebFormUtil.FillListBox(ChbDistributorList, dt, 0, 2, true);
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (btnSave.Text == "Save")
        {
            SKUPriceDetailController mPriceController = new SKUPriceDetailController();
            DataControl dc = new DataControl();
            int count = 0;
            int SKUID = 0;
            bool flag = false;
            if (ddlSkus.SelectedItem != null)
            {
                SKUID = Convert.ToInt32(dc.chkNull_0(ddlSkus.SelectedItem.Value.ToString()));
            }
            for (int i = 0; i < ChbDistributorList.Items.Count; i++)
            {
                if (ChbDistributorList.Items[i].Selected == true)
                {
                    count++;
                   flag = mPriceController.InsertTodayMenu(int.Parse(ChbDistributorList.Items[i].Value), SKUID,DateTime.Parse(txtFromdate.Text), Convert.ToInt32(Session["UserID"]));
                    if(!flag)
                    {
                        break;
                    }
                }
            }
            LoadGrid(DateTime.Parse(txtFromdate.Text));
            if (count == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Please select location');", true);
                return;
            }
            if (flag)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Item added to Today Menu successfully.');", true);
                return;
            }
        }
    }

    protected void btnLoad_Click(object sender, EventArgs e)
    {
        LoadGrid(DateTime.Parse(txtFromdate.Text));
    }

    protected void Grid_pricedetails_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
    {
        SKUPriceDetailController CtrlTodayMenu = new SKUPriceDetailController();
        int TodayMenuID = Convert.ToInt32(Grid_pricedetails.Rows[e.RowIndex].Cells[0].Text);
        CtrlTodayMenu.DeleteTodayMenu(TodayMenuID);
        LoadGrid(DateTime.Parse(txtFromdate.Text));
    }
}