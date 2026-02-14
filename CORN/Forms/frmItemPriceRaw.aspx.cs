using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using System.Web;
using System.IO.Ports;

/// <summary>
/// From To Adjust Stock
/// </summary>
public partial class Forms_frmItemPriceRaw : System.Web.UI.Page
{
    readonly SkuHierarchyController _mHerController = new SkuHierarchyController();
    readonly SKUPriceDetailController SKUPrice = new SKUPriceDetailController();
    readonly DataControl _dc = new DataControl();
    DataTable _purchaseSku;

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        Response.Cache.SetNoStore();
        Response.AppendHeader("pragma", "no-cache");

        if (!Page.IsPostBack)
        {
            LoadDistributor();
            CreatTable();
            LoadCategory();         
            LoadSkuDetail();
        }
    }

    private void CreatTable()
    {
        _purchaseSku = new DataTable();
        _purchaseSku.Columns.Add("STOCK_DEMAND_ID", typeof(long));
        _purchaseSku.Columns.Add("SKU_ID", typeof(int));
        _purchaseSku.Columns.Add("SKU_HIE_NAME", typeof(string));
        _purchaseSku.Columns.Add("SKU_Name", typeof(string));
        _purchaseSku.Columns.Add("UOM_DESC", typeof(string));
        _purchaseSku.Columns.Add("Price", typeof(decimal));
        _purchaseSku.Columns.Add("STOCK_DEMAND_DETAIL_ID", typeof(int)); 
        _purchaseSku.Columns.Add("DISTRIBUTOR_ID", typeof(int)); 
        _purchaseSku.Columns.Add("REMARKS", typeof(string));
        Session.Add("PurchaseSKU", _purchaseSku);
    }
        
    private void LoadDistributor()
    {
        DistributorController DController = new DistributorController();
        DataTable dt = DController.SelectDistributorInfo(-2, int.Parse(Session["UserId"].ToString()), int.Parse(Session["CompanyId"].ToString()));
        clsWebFormUtil.FillListBox(ChbDistributorList, dt, 0, 2, true);
        foreach(ListItem li in ChbDistributorList.Items)
        {
            li.Selected = true;
        }
    }
    
    private void LoadCategory()
    {
        DataTable _mDt = _mHerController.SelectSkuHierarchy(Constants.SKUCategory, Constants.IntNullValue, 2, null, null, true, 15, Constants.IntNullValue);
        ddlCategory.Items.Add(new DevExpress.Web.ListEditItem("All", Constants.IntNullValue.ToString()));
        clsWebFormUtil.FillDxComboBoxList(ddlCategory, _mDt, "SKU_HIE_ID", "SKU_HIE_NAME");
        if (_mDt.Rows.Count > 0)
        {
            ddlCategory.SelectedIndex = 0;
        }
    }
    private void LoadSkuDetail()
    {
        SkuController SKUCtl = new SkuController();
        DataTable dtskuPrice = SKUCtl.GetIRawtemPrice(Constants.IntNullValue);
        GrdPurchase.DataSource = dtskuPrice;
        GrdPurchase.DataBind();
        Session.Add("Dtsku_Price", dtskuPrice);
    }
    private void LoadDocumentDetail()
    {
        DateTime MWorkDate = System.DateTime.Now;
        PurchaseController mPurchase = new PurchaseController();
        {
            DataTable dt = mPurchase.selectStockDemandDetail(Constants.IntNullValue, Constants.IntNullValue,Constants.DateNullValue);
            if (dt.Rows.Count > 0)
            {
                DataTable Dtsku_Price = (DataTable)Session["Dtsku_Price"];

                foreach (DataRow gvr in dt.Rows)
                {
                    foreach (DataRow dr in Dtsku_Price.Rows)
                    {
                        if (gvr["SKU_ID"].ToString() == dr["SKU_ID"].ToString())
                        {
                            dr["Price"] = gvr["Price"];
                        }
                    }
                }
                GrdPurchase.DataSource = Dtsku_Price;
                GrdPurchase.DataBind();
            }
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
    /// <summary>
    /// Saves Document
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnSaveDocument_Click(object sender, EventArgs e)
    {
        bool flag = true;
        lblErrorMsg.Text = "";
        int count = 0;
        foreach (GridViewRow gvr in GrdPurchase.Rows)
        {
            decimal Price = 0;
            DateTime dtDateEffect = Constants.DateNullValue;
            TextBox txtPrice = (TextBox)gvr.FindControl("txtPrice");
            TextBox txtDateEffected = (TextBox)gvr.FindControl("txtDateEffected");

            if (txtPrice.Text.Length > 0)
            {
                Price = Convert.ToDecimal(txtPrice.Text);
            }
            if(txtDateEffected.Text.Length > 0)
            {
                dtDateEffect = Convert.ToDateTime(txtDateEffected.Text);
            }
            if (dtDateEffect != Constants.DateNullValue)
            {
                if (Price > 0)
                {
                    foreach (ListItem li in ChbDistributorList.Items)
                    {
                        if (li.Selected)
                        {
                            flag = SKUPrice.InsertSKU_PRICESRaw(Convert.ToInt32(li.Value), Convert.ToInt32(gvr.Cells[0].Text), Price, dtDateEffect, Convert.ToInt32(Session["UserID"]));
                            if (!flag)
                            {
                                break;
                            }
                            count++;
                        }
                    }
                }
            }
            if(!flag)
            {
                break;
            }
        }

        if (count > 0)
        {
            if (flag)
            {
                LoadSkuDetail();
                ClearAll();
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Successfully Save');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Some error occured');", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('No record inserted');", true);
        }
    }

    /// <summary>
    /// Resets Form Controls
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        CreatTable();
        ClearAll();        
    }
    /// <summary>
    /// Clears Form Controls
    /// </summary>
    private void ClearAll()
    {
        lblErrorMsg.Text = "";
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
    
    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable Dtsku_Price = (DataTable)Session["Dtsku_Price"];

            foreach (GridViewRow gvr in GrdPurchase.Rows)
            {
                foreach (DataRow dr in Dtsku_Price.Rows)
                {
                    if (gvr.Cells[0].Text == dr["SKU_ID"].ToString())
                    {
                        TextBox txtPrice = (TextBox)gvr.FindControl("txtPrice");
                        dr["Price"] = txtPrice.Text;
                    }
                }
            }

            if (ddlCategory.Value.ToString() == Constants.IntNullValue.ToString())
            {
                GrdPurchase.DataSource = Dtsku_Price;
                GrdPurchase.DataBind();
            }
            else
            {   
                DataTable dttableNew = Dtsku_Price.Clone();

                foreach (DataRow drtableOld in Dtsku_Price.Rows)
                {
                    if (drtableOld["SKU_HIE_ID"].ToString() == ddlCategory.Value.ToString())
                    {
                        dttableNew.ImportRow(drtableOld);
                    }
                }
                GrdPurchase.DataSource = dttableNew;
                GrdPurchase.DataBind();
            }
        }
        catch (Exception)
        {
        }
    }
}