using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using System.Web;

public partial class Forms_frmTable : System.Web.UI.Page
{
    readonly GeoHierarchyController GCtl = new GeoHierarchyController();
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        Response.Cache.SetNoStore();
        Response.AppendHeader("pragma", "no-cache");

        if (!Page.IsPostBack)
        {
            Session.Remove("dtGridData");
            LoadDistributor();
            LoadFloor();
            LoadGridData();
            LoadGrid("");
            btnSave.Attributes.Add("onclick", "return ValidateForm()");
            rowParentCategory.Visible = false;
            if(Session["ShowParentCategory"].ToString() == "1")
            {
                rowParentCategory.Visible = true;
                LoadParentCategory();
            }
        }
    }
    private void LoadDistributor()
    {
        DistributorController DController = new DistributorController();
        try
        {
            DataTable dt = DController.SelectDistributorInfo(-1, int.Parse(Session["UserId"].ToString()), int.Parse(Session["CompanyId"].ToString()));
            clsWebFormUtil.FillDxComboBoxList(ddlDistributor, dt, 0, 2, true);

            if (dt.Rows.Count > 0)
            {
                ddlDistributor.SelectedIndex = 0;
            }

        }
        catch (Exception)
        {

            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Please assign Location');", true);
        }
    }
    private void LoadFloor()
    {
        FloorController FController = new FloorController();
        try
        {
            ddlFloor.Items.Clear();
            DataTable dt = FController.GetFloor(1, Convert.ToInt32(ddlDistributor.Value));
            ddlFloor.Items.Add(new DevExpress.Web.ListEditItem("---Select---", Constants.IntNullValue.ToString()));
            clsWebFormUtil.FillDxComboBoxList(ddlFloor, dt, 0, 1, false);
            ddlFloor.SelectedIndex = 0;
        }
        catch (Exception)
        {

            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Please assign Location');", true);
        }
    }
    private void LoadParentCategory()
    {
        SkuHierarchyController mController = new SkuHierarchyController();
        DataTable dt = mController.GetParentCategory(int.Parse(ddlDistributor.Value.ToString()));
        clsWebFormUtil.FillDxComboBoxList(ddlParentCategory, dt, "SKU_HIE_ID", "SKU_HIE_NAME", true);

        if (dt.Rows.Count > 0)
        {
            ddlParentCategory.SelectedIndex = 0;
        }
    }
    public void Clear()
    {
        txtTableNo.Text = string.Empty;
        txtDescription.Text = string.Empty;
        hf_tbldefID.Value = "";
        ddlDistributor.Enabled = true;
        btnSave.Text = "Save";
        txtTableNo.Focus();        
        LoadGrid("");
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        const string tblCapacity = "N/A";
        const string tblAbbrivation = "N/A";
        try
        {
            int ParetnCategoryID = 1;
            if (Session["ShowParentCategory"].ToString() == "1")
            {
                if(ddlParentCategory.Items.Count > 0)
                {
                    ParetnCategoryID = Convert.ToInt32(ddlParentCategory.Value);
                }
            }

            int FloorID = Constants.IntNullValue;
            if (ddlFloor.Items.Count > 0)
            {
                FloorID = Convert.ToInt32(ddlFloor.SelectedItem.Value);
            }
            if (btnSave.Text == "Save")
            {
                if (TableNotExists(1))
                {
                    GCtl.InsertTableDefination(int.Parse(ddlDistributor.SelectedItem.Value.ToString()), txtTableNo.Text.Trim(), txtDescription.Text.Trim(), tblCapacity, tblAbbrivation, System.DateTime.Now, int.Parse(this.Session["UserId"].ToString()),FloorID,Convert.ToInt32(DataControl.chkNull_Zero(txtSort.Text)), ParetnCategoryID);
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Record added successfully.');", true);
                    mPopUpSection.Show();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Table Name for this location already exists.');", true);
                    mPopUpSection.Show();
                    return;
                }
            }
            else if (btnSave.Text == "Update")
            {
                bool status = true;
                if (hfStatus.Value != "Active")
                {
                    status = false;
                }
                if (TableNotExists(2))
                {
                    GCtl.UpdateTableDefination(Convert.ToInt32(hf_tbldefID.Value), txtTableNo.Text.Trim(), txtDescription.Text.Trim(), tblCapacity, tblAbbrivation, System.DateTime.Now, int.Parse(this.Session["UserId"].ToString()), status, Convert.ToInt32(ddlDistributor.SelectedItem.Value),FloorID, Convert.ToInt32(DataControl.chkNull_Zero(txtSort.Text)), ParetnCategoryID);
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Record updated successfully.');", true);
                    mPopUpSection.Hide();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Table Name for this location already exists.');", true);
                    mPopUpSection.Show();
                    return;
                }
            }
            Clear();
            LoadGridData();
            LoadGrid("");
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "CatchMsg", "alert('" + ex.Message.ToString() + "');", true);
            mPopUpSection.Show();
        }
    }
    private void LoadGridData()
    {
        DataTable dt = new DataTable();
        dt = GCtl.GetTableDefination(Constants.IntNullValue, Constants.IntNullValue, false, int.Parse(Session["UserId"].ToString()));//false means all
        Session.Add("dtGridData", dt);
    }
    private void LoadGrid(string pType)
    {
        GrdTable.DataSource = null;
        GrdTable.DataBind();
        if (ddlDistributor.Items.Count > 0)
        {
            DataTable dt = (DataTable)Session["dtGridData"];
            int SortID = 0;
            if(dt.Rows.Count>0)
            {
                SortID = Convert.ToInt32(dt.Compute("max([SortID])", string.Empty));
            }
            txtSort.Text = (SortID + 1).ToString();
            if (pType == "")
            {
                if (txtSearch.Text != "" || txtSearch.Text != string.Empty)
                {
                    dt.DefaultView.RowFilter = "TableDefination_No LIKE '%" + txtSearch.Text + "%' OR TableDefination_Description LIKE '%" + txtSearch.Text + "%' OR Status LIKE '" + txtSearch.Text + "%' OR DISTRIBUTOR_NAME LIKE '%" + txtSearch.Text + "%' OR FloorName LIKE '%" + txtSearch.Text + "%'";
                }
                GrdTable.DataSource = dt;
                GrdTable.DataBind();
            }
            else
            {
                if (txtSearch.Text != "" || txtSearch.Text != string.Empty)
                {
                    dt.DefaultView.RowFilter = "TableDefination_No LIKE '%" + txtSearch.Text + "%' OR TableDefination_Description LIKE '%" + txtSearch.Text + "%' OR Status LIKE '" + txtSearch.Text + "%' OR DISTRIBUTOR_NAME LIKE '%" + txtSearch.Text + "%' OR FloorName LIKE '%" + txtSearch.Text + "%'";
                }
                else
                {
                    dt.DefaultView.RowFilter = null;
                }
                if (dt.Rows.Count > 0)
                {
                    GrdTable.PageIndex = 0;
                }
                GrdTable.DataSource = dt;
                GrdTable.DataBind();
            }
        }
    }
    protected void GrdTable_RowEditing(object source, GridViewEditEventArgs e)
    {
        mPopUpSection.Show();
        hf_tbldefID.Value = GrdTable.Rows[e.NewEditIndex].Cells[1].Text;
        txtTableNo.Text = GrdTable.Rows[e.NewEditIndex].Cells[3].Text;
        txtDescription.Text = GrdTable.Rows[e.NewEditIndex].Cells[4].Text;
        hfStatus.Value = GrdTable.Rows[e.NewEditIndex].Cells[6].Text;
        ddlDistributor.Value = GrdTable.Rows[e.NewEditIndex].Cells[8].Text;
        LoadFloor();
        if(GrdTable.Rows[e.NewEditIndex].Cells[9].Text != "0")
        {
            ddlFloor.Value = GrdTable.Rows[e.NewEditIndex].Cells[9].Text;
        }
        txtSort.Text = GrdTable.Rows[e.NewEditIndex].Cells[10].Text;
        if (Session["ShowParentCategory"].ToString() == "1")
        {
            ddlParentCategory.Value = GrdTable.Rows[e.NewEditIndex].Cells[11].Text;
        }
            btnSave.Text = "Update";
    }
    protected void btnActive_Click(object sender, EventArgs e)
    {
        bool check = false;
        try
        {
            foreach (GridViewRow dr2 in GrdTable.Rows)
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
            foreach (GridViewRow dr in GrdTable.Rows)
            {
                var chRelized = (CheckBox)dr.Cells[0].FindControl("ChbIsAssigned");

                if (chRelized.Checked)
                {
                    if (Convert.ToString(dr.Cells[6].Text) == "Active")
                    {
                        GCtl.DeleteTableDefination(Convert.ToInt32(dr.Cells[1].Text), false);
                        flag = true;
                    }
                    else
                    {
                        GCtl.DeleteTableDefination(Convert.ToInt32(dr.Cells[1].Text), true);
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
        catch (Exception)
        {

            throw;
        }
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        mPopUpSection.Show();
    }
    protected void btnFilter_Click(object sender, EventArgs e)
    {
        LoadGrid("filter");
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        mPopUpSection.Show();
        Clear();
    }
    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect("Home.aspx");
    }
    protected void GrdTable_RowEditing(object sender, GridViewPageEventArgs e)
    {
        GrdTable.PageIndex = e.NewPageIndex;
    }
    protected void GrdTable_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GrdTable.PageIndex = e.NewPageIndex;
        LoadGrid("");
    }
    protected void btnClose_Click(object sender, EventArgs e)
    {
        Clear();
    }
    private bool TableNotExists(int type)
    {
        bool flag = true;
        if (type == 1)
        {
            foreach (GridViewRow gvr in GrdTable.Rows)
            {
                if (gvr.Cells[8].Text == ddlDistributor.Value.ToString() && gvr.Cells[9].Text == ddlFloor.Value.ToString())
                {
                    if (txtTableNo.Text.Trim().ToLower() == gvr.Cells[3].Text.ToLower())
                    {
                        flag = false;
                        break;
                    }
                }
            }
        }
        else
        {
            foreach (GridViewRow gvr in GrdTable.Rows)
            {
                if (hf_tbldefID.Value != gvr.Cells[1].Text)
                {
                    if (gvr.Cells[8].Text == ddlDistributor.Value.ToString() && gvr.Cells[9].Text == ddlFloor.Value.ToString())
                    {
                        if (txtTableNo.Text.Trim().ToLower() == gvr.Cells[3].Text.ToLower())
                        {
                            flag = false;
                            break;
                        }
                    }
                }
            }
        }
        return flag;
    }

    protected void ddlDistributor_SelectedIndexChanged(object sender, EventArgs e)
    {
        mPopUpSection.Show();
        LoadFloor();
        if (Session["ShowParentCategory"].ToString() == "1")
        {
            LoadParentCategory();
        }
    }
}