using System;
using System.Web.UI.WebControls;
using System.Data;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using System.Web;
using System.Web.UI;

public partial class Forms_frmSection : System.Web.UI.Page
{
    readonly GeoHierarchyController GCtl = new GeoHierarchyController();
    private SkuController _skuController = new SkuController();
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        Response.Cache.SetNoStore();
        Response.AppendHeader("pragma", "no-cache");
        if (!Page.IsPostBack)
        {
            Session.Remove("dtGridData");
            LoadGridData();
            LoadGrid("");
            LoadUser();
            btnSave.Attributes.Add("onclick", "return ValidateForm()");
        }
    }
    private void LoadUser()
    {
        ChbAreaList.Items.Clear();
        Distributor_UserController du = new Distributor_UserController();
        DataTable dt = du.SelectDistributorUser(11, Convert.ToInt32(Session["DISTRIBUTOR_ID"].ToString()),int.Parse(Session["CompanyId"].ToString()),Convert.ToInt32(Session["UserID"]));

        clsWebFormUtil.FillListBox(ChbAreaList, dt, "USER_ID", "USER_NAME", true);
    }
    public void Clear()
    {
        txtCodeNo.Text = string.Empty;
        txtSectionName.Text = string.Empty;
        txtPrinterName.Text = string.Empty;
        txtSectionName.Enabled = true;
        hfSectionId.Value = "";
        txtNoOfPrints.Text = "1";
        btnSave.Text = "Save";
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (btnSave.Text == "Save")
            {
                var savedId = _skuController.InsertProductSection(txtCodeNo.Text,txtSectionName.Text, txtPrinterName.Text,chkFullKOT.Checked,Convert.ToInt32(DataControl.chkNull_Zero(txtNoOfPrints.Text)));
                for (int i = 0; i < ChbAreaList.Items.Count; i++)
                {
                    if (ChbAreaList.Items[i].Selected == true)
                    {
                        _skuController.InsertUserSECTION(
                            savedId,
                            int.Parse(ChbAreaList.Items[i].Value.ToString()), true);
                    }
                }

                ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Record added successfully.');", true);
                mPopUpSection.Show();
            }
            else if (btnSave.Text == "Update")
            {
                bool status = true;
                if (hfStatus.Value != "Active")
                {
                    status = false;
                }
                _skuController.UpdateProductSection(Convert.ToInt32(hfSectionId.Value), txtCodeNo.Text, txtSectionName.Text, txtPrinterName.Text, status,chkFullKOT.Checked, Convert.ToInt32(DataControl.chkNull_Zero(txtNoOfPrints.Text)));
                for (int i = 0; i < ChbAreaList.Items.Count; i++)
                {
                    _skuController.InsertUserSECTION(
                        Convert.ToInt32(hfSectionId.Value),
                        int.Parse(ChbAreaList.Items[i].Value.ToString()),
                        ChbAreaList.Items[i].Selected);
                }

                ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Record updated successfully.');", true);
                mPopUpSection.Hide();
            }
            Clear();
            LoadGridData();
            LoadGrid("");
            for (int i = 0; i < ChbAreaList.Items.Count; i++)
            {
                ChbAreaList.Items[i].Selected = false;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "CatchMsg", "alert('" + ex.Message.ToString() + "');", true);
        }
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        txtNoOfPrints.Text = "1";
        mPopUpSection.Show();
        Clear();
    }
    protected void btnFilter_Click(object sender, EventArgs e)
    {
        this.LoadGrid("filter");
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Clear();
        mPopUpSection.Show();
        for (int i = 0; i < ChbAreaList.Items.Count; i++)
        {
            ChbAreaList.Items[i].Selected = false;
        }
    }
    private void LoadGridData()
    {
        DataTable dt = new DataTable();
        dt = _skuController.SelectProductSection(Constants.IntNullValue, null, null);
        Session.Add("dtGridData", dt);
    }
    private void LoadGrid(string pType)
    {
        GrdSection.DataSource = null;
        GrdSection.DataBind();
        DataTable dt = (DataTable)Session["dtGridData"];
        if (pType == "")
        {
            if (txtSearch.Text != "" || txtSearch.Text != string.Empty)
            {
                dt.DefaultView.RowFilter = "SECTION_NAME LIKE '%" + txtSearch.Text + "%' OR PRINTER_NAME LIKE '%" + txtSearch.Text + "%' OR IS_ACTIVE LIKE '" + txtSearch.Text + "%'";
            }
            GrdSection.DataSource = dt;
            GrdSection.DataBind();
        }
        else
        {
            if (txtSearch.Text != "" || txtSearch.Text != string.Empty)
            {
                dt.DefaultView.RowFilter = "SECTION_NAME LIKE '%" + txtSearch.Text + "%' OR PRINTER_NAME LIKE '%" + txtSearch.Text + "%' OR IS_ACTIVE LIKE '" + txtSearch.Text + "%'";
            }
            else
            {
                dt.DefaultView.RowFilter = null;
            }
            if (dt.Rows.Count > 0)
            {
                GrdSection.PageIndex = 0;
            }
            GrdSection.DataSource = dt;
            GrdSection.DataBind();
        }
    }
    protected void GrdSection_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GrdSection.PageIndex = e.NewPageIndex;
        LoadGrid("");
    }
    protected void GrdSection_RowEditing(object sender, GridViewEditEventArgs e)
    {
        hfSectionId.Value = GrdSection.Rows[e.NewEditIndex].Cells[1].Text;
        LoadUsersInSection(int.Parse(hfSectionId.Value));
        txtSectionName.Text = GrdSection.Rows[e.NewEditIndex].Cells[2].Text;
        txtPrinterName.Text = GrdSection.Rows[e.NewEditIndex].Cells[3].Text;
        hfStatus.Value= GrdSection.Rows[e.NewEditIndex].Cells[4].Text;
        chkFullKOT.Checked = Convert.ToBoolean(GrdSection.Rows[e.NewEditIndex].Cells[5].Text);
        txtNoOfPrints.Text = GrdSection.Rows[e.NewEditIndex].Cells[6].Text;
        mPopUpSection.Show();
        btnSave.Text = "Update";
    }
    public void LoadUsersInSection(int sectionId)
    {
        DataTable dt = _skuController.SelectUserInSection(sectionId);
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < ChbAreaList.Items.Count; i++)
            {
                foreach (DataRow item in dt.Rows)
                {
                    if (item["USER_ID"].ToString() == ChbAreaList.Items[i].Value)
                    {
                        ChbAreaList.Items[i].Selected = true;
                        break;
                    }
                }
            }
        }
    }
    protected void btnActive_Click(object sender, EventArgs e)
    {
        bool check = false;
        UserController _UserCtrl = new UserController();
        try
        {
            foreach (GridViewRow dr2 in GrdSection.Rows)
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
            foreach (GridViewRow dr in GrdSection.Rows)
            {
                var chRelized = (CheckBox)dr.Cells[0].FindControl("ChbIsAssigned");
               
                if (chRelized.Checked)
                {
                    if (Convert.ToString(dr.Cells[4].Text) == "Active")
                    {
                        _UserCtrl.ActiveInactive(false, Convert.ToInt32(dr.Cells[1].Text), Convert.ToInt32(Session["UserID"]), 6);
                        flag = true;
                    }
                    else
                    {
                        _UserCtrl.ActiveInactive(true,Convert.ToInt32(dr.Cells[1].Text), Convert.ToInt32(Session["UserID"]), 6);
                        flag = true;

                    }
                }
            }
            if (flag)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Record updated successfully');", true);
            }
            LoadGridData();
            LoadGrid("");
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "CatchMsg", "alert('" + ex.Message.ToString() + "');", true);
        }
    }
    protected void btnClose_Click(object sender, EventArgs e)
    {
        Clear();
        LoadUser();
    }
}