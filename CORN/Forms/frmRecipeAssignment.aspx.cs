using System;
using System.Data;
using System.Web.UI;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using System.Web;

public partial class Forms_frmRecipeAssignment : System.Web.UI.Page
{
    SkuController mSKU = new SkuController();
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        Response.Cache.SetNoStore();
        Response.AppendHeader("pragma", "no-cache");

        if (!Page.IsPostBack)
        {
            LoadDISTRIBUTOR();
            LoadUnAssingned();
            LoadAssingned();
        }
    }

    private void LoadUnAssingned()
    {
        if (ddlLocation.Items.Count > 0)
        {
            DataTable dt = mSKU.GetRecipeLocation(Convert.ToInt32(ddlLocation.Value), 0);
            clsWebFormUtil.FillListBox(lstUnAssignRecipe, dt, 0, 1, true);
        }
    }
    private void LoadDISTRIBUTOR()
    {
        DistributorController mController = new DistributorController();
        DataTable dtDistributor = mController.SelectDistributorInfo(Constants.IntNullValue,int.Parse(Session["UserId"].ToString()), int.Parse(Session["CompanyId"].ToString()));

        clsWebFormUtil.FillDxComboBoxList(ddlLocation, dtDistributor, "DISTRIBUTOR_ID", "DISTRIBUTOR_NAME");

        if (dtDistributor.Rows.Count > 0)
        {
            ddlLocation.SelectedIndex = 0;
        }
    }


    /// <summary>
    /// Loads Assigned Locations To Assigned ListBox on Location Tab
    /// </summary>
    private void LoadAssingned()
    {
        if (ddlLocation.Items.Count > 0)
        {
            DataTable dt = mSKU.GetRecipeLocation(Convert.ToInt32(ddlLocation.Value), 1);
            clsWebFormUtil.FillListBox(lstAssignRecipe, dt, 0, 1, true);
        }
    }
    
    /// <summary>
    /// Inserts Location Assignment To User
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnAssignLocation_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < lstUnAssignRecipe.Items.Count; i++)
        {
            if (lstUnAssignRecipe.Items[i].Selected == true)
            {
                mSKU.InsertRecipeLocation(Convert.ToInt32(ddlLocation.Value), int.Parse(lstUnAssignRecipe.Items[i].Value.ToString()),1);
            }
        }
        this.LoadUnAssingned();
        this.LoadAssingned();
    }

    /// <summary>
    /// Deletes Location Assignment To User
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnUnAssignLocation_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < lstAssignRecipe.Items.Count; i++)
        {
            if (lstAssignRecipe.Items[i].Selected == true)
            {
                mSKU.InsertRecipeLocation(Convert.ToInt32(ddlLocation.Value), int.Parse(lstAssignRecipe.Items[i].Value.ToString()), 2);
            }
        }
        this.LoadUnAssingned();
        this.LoadAssingned();
    }
    
    protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadUnAssingned();
        LoadAssingned();
    }

    protected void btnAssignAllRecipe_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < lstUnAssignRecipe.Items.Count; i++)
        {
            mSKU.InsertRecipeLocation(Convert.ToInt32(ddlLocation.Value), int.Parse(lstUnAssignRecipe.Items[i].Value.ToString()), 1);
        }
        this.LoadUnAssingned();
        this.LoadAssingned();
    }

    protected void btnUnAssignAllRecipe_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < lstAssignRecipe.Items.Count; i++)
        {
            mSKU.InsertRecipeLocation(Convert.ToInt32(ddlLocation.Value), int.Parse(lstAssignRecipe.Items[i].Value.ToString()), 2);
        }
        this.LoadUnAssingned();
        this.LoadAssingned();
    }
}