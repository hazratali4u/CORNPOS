using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using CORNBusinessLayer.Classes;
using CORNBusinessLayer.Reports;
using CORNCommon.Classes;
using System.Web;

public partial class Forms_frmItemGroup : System.Web.UI.Page
{
    SKUGroupController gController = new SKUGroupController();
    DataView dv;
    private static int SKUGroupID = 0;

    /// <summary>
    /// Page_Load Function
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
            LoadCategory();
            GetSKUName();
            LoadGrid();
        }
    }
    protected void GetSKUName()
    {
        SkuController mContoller = new SkuController();
        DataTable dt = mContoller.SelectSkuInfo(Constants.IntNullValue, Constants.IntNullValue, Convert.ToInt32(ddCatagory.Value), Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue);
        lstUnAssignSKU.DataSource = dt;
        lstUnAssignSKU.DataTextField = "SKUDETAIL";
        lstUnAssignSKU.DataValueField = "SKU_ID";
        lstUnAssignSKU.DataBind();
    }
    protected void LoadGrid()
    {
        DataTable dt = gController.GET_SKUGroupDetail(int.Parse(this.Session["CompanyId"].ToString()));
        dv = new DataView(dt);
        DataTable dt1 = gController.GET_Group_ID(int.Parse(this.Session["CompanyId"].ToString()));
        this.SKUGroup_Grid.DataSource = dt1;
        this.SKUGroup_Grid.DataBind();
        for (int i = 0; i < SKUGroup_Grid.Rows.Count; i++)
        {
            dv.RowFilter = "SKU_GROUP_ID = " + dt1.Rows[i]["SKU_GROUP_ID"].ToString();
            ListBox list = (ListBox)SKUGroup_Grid.Rows[i].Cells[2].FindControl("listbox1");
            list.DataSource = dv;
            list.DataTextField = "SKUDETAIL";
            list.DataValueField = "SKU_ID";
            list.DataBind();
        }
    }
    private void LoadCategory()
    {
        SkuHierarchyController mHer_Controller = new SkuHierarchyController();
        DataTable m_dt = mHer_Controller.SelectSkuHierarchy(Constants.SKUCategory, Constants.IntNullValue, Constants.IntNullValue, null, null, true, int.Parse(this.Session["CompanyId"].ToString()),Constants.IntNullValue);
        clsWebFormUtil.FillDxComboBoxList(this.ddCatagory, m_dt, 0, 3, true);
        if (m_dt.Rows.Count > 0)
        {
            ddCatagory.SelectedIndex = 0;
        }
    }
    protected void ddCatagory_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetSKUName();
    }

    protected void btnAddAll_Click(object sender, EventArgs e)
    {
        int ItemCount = lstUnAssignSKU.Items.Count;
        for (int i = 0; i < ItemCount; i++)
        {
            this.lstAssignSKU.Items.Add(new clsListItems(lstUnAssignSKU.Items[i].Text, lstUnAssignSKU.Items[i].Value.ToString()));

        }
        lstUnAssignSKU.Items.Clear();
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (lstUnAssignSKU.Items.Count > 0)
        {
            this.lstAssignSKU.Items.Add(new clsListItems(lstUnAssignSKU.SelectedItem.Text, lstUnAssignSKU.SelectedValue.ToString()));
            lstUnAssignSKU.Items.RemoveAt(lstUnAssignSKU.SelectedIndex);
        }
    }

    protected void btnRemove_Click(object sender, EventArgs e)
    {
        if (lstAssignSKU.Items.Count > 0)
        {
            this.lstUnAssignSKU.Items.Add(new clsListItems(lstAssignSKU.SelectedItem.Text, lstAssignSKU.SelectedValue.ToString()));
            lstAssignSKU.Items.RemoveAt(lstAssignSKU.SelectedIndex);
        }
    }

    protected void btnRemoveAll_Click(object sender, EventArgs e)
    {
        int ItemCount = lstAssignSKU.Items.Count;
        for (int i = 0; i < ItemCount; i++)
        {
            this.lstUnAssignSKU.Items.Add(new clsListItems(lstAssignSKU.Items[i].Text, lstAssignSKU.Items[i].Value.ToString()));
        }
        lstAssignSKU.Items.Clear();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (txtGroupName.Text.Length == 0)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Must Enter Group Name.');", true);
            return;
        }
        if (btnSave.Text == "Save")
        {
            DataTable dt_uniqueGroupName = GetUnique_GroupName();
            if (dt_uniqueGroupName.Rows.Count > 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Group Name already exists.');", true);
                return;
            }
            string ID = gController.Insert_SKUGroup(txtGroupName.Text, System.DateTime.Today, false, int.Parse(this.Session["UserId"].ToString()), 1, int.Parse(this.Session["CompanyId"].ToString()));

            for (int i = 0; i < lstAssignSKU.Items.Count; i++)
            {
                int SKU_ID = Convert.ToInt32(lstAssignSKU.Items[i].Value.ToString());
                gController.Insert_SKU_GroupDetail(int.Parse(ID), SKU_ID);
            }
        }
        if (btnSave.Text == "Update")
        {
            Update();
        }
        LoadGrid();
        lstAssignSKU.Items.Clear();
        lstUnAssignSKU.Items.Clear();
        txtGroupName.Text = "";
        btnSave.Text = "Save";
    }
    protected void Update()
    {
        if (lstAssignSKU.Items.Count == 0)
        {
            gController.UpdateSKUinGroup(SKUGroupID, 0, txtGroupName.Text, true, 1);
        }
        else
        {
            gController.UpdateSKUinGroup(SKUGroupID, 0, txtGroupName.Text,true, 1);
            for (int i = 0; i < lstAssignSKU.Items.Count; i++)
            {
                gController.UpdateSKUinGroup(SKUGroupID, Convert.ToInt32(lstAssignSKU.Items[i].Value), txtGroupName.Text, true, 1);
            }
        }
    }
    protected DataTable GetUnique_GroupName()
    {
        DataTable dt = gController.GET_UniqueGroupName(Constants.IntNullValue, txtGroupName.Text, Constants.DateNullValue, true, int.Parse(this.Session["UserId"].ToString()), int.Parse(this.Session["CompanyId"].ToString()));
        return dt;
    }
    protected void SKUGroup_Grid_RowEditing(object sender, GridViewEditEventArgs e)
    {
        lstAssignSKU.Items.Clear();
        GridViewRow gvr = (GridViewRow)this.SKUGroup_Grid.Rows[e.NewEditIndex];
        SKUGroupID = int.Parse(gvr.Cells[0].Text);
        this.txtGroupName.Text = gvr.Cells[1].Text;
        ListBox list = (ListBox)gvr.Cells[2].FindControl("listbox1");
        for (int i = 0; i < list.Items.Count; i++)
        {

            lstAssignSKU.DataSource = list.DataSource;
            lstAssignSKU.Items.Add(list.Items[i].Text);
            lstAssignSKU.Items[i].Value = list.Items[i].Value;
            lstAssignSKU.DataBind();
        }

        btnSave.Text = "Update";
    }

    protected void SKUGroup_Grid_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SKUGroup_Grid.PageIndex = e.NewPageIndex;
        LoadGrid();
    }
}