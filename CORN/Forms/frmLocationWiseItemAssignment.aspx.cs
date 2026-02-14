using System;
using System.Data;
using System.Web.UI;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using System.Web;
using System.Collections.Generic;
using System.Linq;

public partial class Forms_frmLocationWiseItemAssignment : System.Web.UI.Page
{
    UserController mUserController = new UserController();
    readonly SkuController mController = new SkuController();
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        Response.Cache.SetNoStore();
        Response.AppendHeader("pragma", "no-cache");

        if (!Page.IsPostBack)
        {
            LoadDISTRIBUTOR();
            LoadCategory();
            LoadUnAssingned();
            LoadAssingned();
        }
    }

    private void LoadDISTRIBUTOR()
    {
        DistributorController mController = new DistributorController();
        DataTable dtDistributor = mController.SelectDistributorInfo(Constants.IntNullValue,
            int.Parse(Session["UserId"].ToString()), int.Parse(Session["CompanyId"].ToString()));

        clsWebFormUtil.FillDxComboBoxList(ddlLocation, dtDistributor, "DISTRIBUTOR_ID", "DISTRIBUTOR_NAME");

        if (dtDistributor.Rows.Count > 0)
        {
            ddlLocation.SelectedIndex = 0;
        }
    }
    private void LoadCategory()
    {
        SkuHierarchyController _mHerController = new SkuHierarchyController();

        DataTable _mDt = _mHerController.SelectSkuHierarchy(Constants.SKUCategory,
            Constants.IntNullValue, Constants.IntNullValue, null, null, true, 21,
            Constants.IntNullValue);

        ddlCategory.Items.Add(new DevExpress.Web.ListEditItem("All", Constants.IntNullValue.ToString()));
        clsWebFormUtil.FillDxComboBoxList(ddlCategory, _mDt, "SKU_HIE_ID", "SKU_HIE_NAME");
        if (_mDt.Rows.Count > 0)
        {
            ddlCategory.SelectedIndex = 0;
        }
    }

    #region Location Tab

    /// <summary>
    /// Loads UnAssigned Locations To UnAssigned ListBox on Location Tab
    /// </summary>
    private void LoadUnAssingned()
    {
        if (ddlLocation.Items.Count > 0)
        {
            var hierarchyController = new SkuHierarchyController();

            DataTable dt = hierarchyController.SelectAssignedUnAssignedItems(
                int.Parse(ddlLocation.SelectedItem.Value.ToString()),
                false, Constants.IntNullValue);

            lstUnAssignDistributor.DataSource = dt;
            lstUnAssignDistributor.DataTextField = "SKU_NAME";
            lstUnAssignDistributor.DataValueField = "SKU_ID";
            lstUnAssignDistributor.DataBind();
        }
    }

    /// <summary>
    /// Loads Assigned Locations To Assigned ListBox on Location Tab
    /// </summary>
    private void LoadAssingned()
    {
        if (ddlLocation.Items.Count > 0)
        {
            var hierarchyController = new SkuHierarchyController();

            DataTable dt = hierarchyController.SelectAssignedUnAssignedItems(
                int.Parse(ddlLocation.SelectedItem.Value.ToString()),
                true, Constants.IntNullValue);

            lstAssignDistributor.DataSource = dt.DefaultView;
            lstAssignDistributor.DataTextField = "SKU_NAME";
            lstAssignDistributor.DataValueField = "SKU_ID";
            lstAssignDistributor.DataBind();
        }
    }

    /// <summary>
    /// Inserts Location Assignment To User
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnAssignLocation_Click(object sender, EventArgs e)
    {
        mUserController = new UserController();
        for (int i = 0; i < lstUnAssignDistributor.Items.Count; i++)
        {
            if (lstUnAssignDistributor.Items[i].Selected == true)
            {
                mController.InsertItemAgainstLocation(
                    int.Parse(ddlLocation.SelectedItem.Value.ToString()),
                    Convert.ToInt32(lstUnAssignDistributor.SelectedValue.ToString()),
                    0, int.Parse(Session["UserId"].ToString()));
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
        for (int i = 0; i < lstAssignDistributor.Items.Count; i++)
        {
            if (lstAssignDistributor.Items[i].Selected == true)
            {
                mController.InsertItemAgainstLocation(
                    int.Parse(ddlLocation.SelectedItem.Value.ToString()),
                    Convert.ToInt32(lstAssignDistributor.SelectedValue.ToString()),
                    1, int.Parse(Session["UserId"].ToString()));
            }
        }
        this.LoadUnAssingned();
        this.LoadAssingned();
    }

    #endregion

    protected void btnAssignAllItem_Click(object sender, EventArgs e)
    {
        if (lstUnAssignDistributor.Items.Count > 0)
        {
            for (int i = 0; i < lstUnAssignDistributor.Items.Count; i++)
            {
                mController.InsertItemAgainstLocation(
                    int.Parse(ddlLocation.SelectedItem.Value.ToString()),
                    Convert.ToInt32(lstUnAssignDistributor.Items[i].Value),
                    0, int.Parse(Session["UserId"].ToString()));
            }

            LoadAssingned();
            LoadUnAssingned();
        }
    }

    protected void btnUnAssignAllItem_Click(object sender, EventArgs e)
    {
        if (lstAssignDistributor.Items.Count > 0)
        {
            for (int i = 0; i < lstAssignDistributor.Items.Count; i++)
            {
                mController.InsertItemAgainstLocation(int.Parse(
                    ddlLocation.SelectedItem.Value.ToString()),
                    Convert.ToInt32(lstAssignDistributor.Items[i].Value),
                    1, int.Parse(Session["UserId"].ToString()));
            }

            LoadAssingned();
            LoadUnAssingned();
        }
    }

    protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadUnAssingned();
        LoadAssingned();
    }
    protected void Button1_Click(object sender, System.EventArgs e)
    {
        if (!string.IsNullOrEmpty(TextBox1.Text))
        {
            string searchString = TextBox1.Text.ToString().Trim();
            var list = new List<KeyValuePair<string, int>>();

            LoadUnAssingned();

            for (int i = 0; i < lstUnAssignDistributor.Items.Count; i++)
            {
                list.Add(new KeyValuePair<string, int>(lstUnAssignDistributor.Items[i].Text,
                    int.Parse(lstUnAssignDistributor.Items[i].Value)));
            }

            list = list.Where(x => x.Key.ToLower().Contains(searchString.ToLower())).ToList();

            if (list.Count > 0)
            {
                lstUnAssignDistributor.DataSource = list;
                lstUnAssignDistributor.DataTextField = "Key";
                lstUnAssignDistributor.DataValueField = "Value";
                lstUnAssignDistributor.DataBind();
            }
            else
            {
                LoadUnAssingned();
            }
        }
    }
    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            var hierarchyController = new SkuHierarchyController();
            DataTable dt = new DataTable();

            if (ddlCategory.SelectedIndex != 0)
            {
                dt = hierarchyController.SelectAssignedUnAssignedItems(
                    int.Parse(ddlLocation.SelectedItem.Value.ToString()),
                    false, int.Parse(ddlCategory.SelectedItem.Value.ToString()));
            }
            else
            {
                dt = hierarchyController.SelectAssignedUnAssignedItems(
                int.Parse(ddlLocation.SelectedItem.Value.ToString()),
                false, Constants.IntNullValue);
            }

            lstUnAssignDistributor.DataSource = dt;
            lstUnAssignDistributor.DataTextField = "SKU_NAME";
            lstUnAssignDistributor.DataValueField = "SKU_ID";
            lstUnAssignDistributor.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}
