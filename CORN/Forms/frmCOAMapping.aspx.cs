using System;
using System.Data;
using System.Web;
using System.Web.UI;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using System.Web.UI.WebControls;
using DevExpress.Web;

public partial class Forms_frmCOAMapping : System.Web.UI.Page
{   
    COAMappingController _cController = new COAMappingController();

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
            LoadConfiguration("");
            LoadAccountHeadMap(null);
            string strLevel = "Level 4";
            if(drpAccountHeadMap.SelectedItem.Value.ToString() == "2" || drpAccountHeadMap.SelectedItem.Value.ToString() == "30")
            {
                strLevel = "Level 3";
            }
            LoadAccountDetail(strLevel);
        }
    }

    private void LoadGridData()
    {
        DataTable dt = new DataTable();
        dt = _cController.SelectCOAConfiguration(2, Constants.ShortNullValue, Constants.LongNullValue, null);
        Session.Add("dtGridData", dt);
    }

    private void LoadAccountDetail(string strLevel)
    {
        AccountHeadController mAccountController = new AccountHeadController();
        DataTable dtHead = new DataTable();

        drpAccountHead.Items.Clear();

        if (strLevel == "Level 3")
        {
            dtHead = mAccountController.SelectAccountHead(Constants.AC_DetailTypeId, Constants.LongNullValue, 1);
        }
        else
        {
            dtHead = mAccountController.SelectAccountHead(Constants.AC_AccountHeadId, Constants.LongNullValue, 1);
        }

        drpAccountHead.DataSource = dtHead;
        drpAccountHead.ValueField = "ACCOUNT_HEAD_ID";
        drpAccountHead.DataBind();

        if (dtHead.Rows.Count > 0)
        {
            drpAccountHead.SelectedIndex = 0;
        }

    }
    private void LoadAccountHeadMap(string strLevel)
    {
        drpAccountHeadMap.Items.Clear();
        DataTable dt = _cController.SelectCOAConfiguration(1, Constants.ShortNullValue, Constants.LongNullValue, strLevel);
        clsWebFormUtil.FillDxComboBoxList(drpAccountHeadMap, dt, 0, 1, true);
        if (dt.Rows.Count > 0)
        {
            drpAccountHeadMap.SelectedIndex = 0;
        }
    }
    private void LoadConfiguration(string pType)
    {
        try
        {
            DataTable dt = (DataTable)Session["dtGridData"];
            if (pType == "")
            {
                if (txtSearch.Text != "" || txtSearch.Text != string.Empty)
                {
                    dt.DefaultView.RowFilter = "KEY LIKE '%" + txtSearch.Text + "%'  OR ACCOUNT_NAME LIKE '%" + txtSearch.Text + "%' OR IS_ACTIVE LIKE '%" + txtSearch.Text + "%' OR LEVEL LIKE '%" + txtSearch.Text + "%'";
                }
                GridCOAMapping.DataSource = dt;
                GridCOAMapping.DataBind();
            }
            else
            {
                if (txtSearch.Text != "" || txtSearch.Text != string.Empty)
                {
                    dt.DefaultView.RowFilter = "KEY LIKE '%" + txtSearch.Text + "%'  OR ACCOUNT_NAME LIKE '%" + txtSearch.Text + "%' OR IS_ACTIVE LIKE '%" + txtSearch.Text + "%' OR LEVEL LIKE '%" + txtSearch.Text + "%'";
                }
                else
                {
                    dt.DefaultView.RowFilter = null;
                }
                if (dt.Rows.Count > 0)
                {
                    GridCOAMapping.PageIndex = 0;
                }
                GridCOAMapping.DataSource = dt;
                GridCOAMapping.DataBind();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg3", "alert('Error Occured: \n" + ex + "');", true);
        }
    }

    #region Grid Operations   

    protected void GridCOAMapping_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridCOAMapping.PageIndex = e.NewPageIndex;
        LoadConfiguration("");
    }
    protected void GridCOAMapping_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            GridViewRow gvr = GridCOAMapping.Rows[e.NewEditIndex];
            DataTable dt = _cController.ChekCOAConfigurationExistance(3, Convert.ToInt64(gvr.Cells[4].Text.ToString()), gvr.Cells[2].Text.ToString());
            if (Convert.ToInt32(dt.Rows[0]["IS_EXIST"]) == 0)
            {
                mPopUpLocation.Show();
                LoadAccountHeadMap(gvr.Cells[2].Text.ToString());
                LoadAccountDetail(gvr.Cells[2].Text.ToString());
                drpAccountHeadMap.Value = gvr.Cells[1].Text.ToString();
                drpAccountHead.Value = gvr.Cells[4].Text.ToString();
                btnSave.Text = "Update";
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Transaction exist against this Account!');", true);
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
    }

    #endregion

    #region Click

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        //drpAccountLevel.SelectedIndex = 0;
        //drpAccountLevel_SelectedIndexChanged(null, null);
        btnSave.Text = "Save";
        mPopUpLocation.Show();
    }
    protected void btnActive_Click(object sender, EventArgs e)
    {
        UserController _UserCtrl = new UserController();
        bool check = false;
        try
        {
            foreach (GridViewRow dr2 in GridCOAMapping.Rows)
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

            foreach (GridViewRow dr in GridCOAMapping.Rows)
            {

                var chRelized = (CheckBox)dr.Cells[0].FindControl("ChbIsAssigned");

                if (chRelized.Checked)
                {

                    if (Convert.ToString(dr.Cells[7].Text) == "Active")
                    {

                        _UserCtrl.ActiveInactive(false, Convert.ToInt32(dr.Cells[1].Text), 1, 9);


                        flag = true;

                    }
                    else
                    {

                        _UserCtrl.ActiveInactive(true, Convert.ToInt32(dr.Cells[1].Text), 1, 9);

                        flag = true;
                    }
                }
            }
            if (flag)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Record updated successfully');", true);
            }
            LoadGridData();
            LoadConfiguration("");
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "CatchMsg", "alert('" + ex.Message.ToString() + "');", true);
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        btnSave.Text = "Save";
        //drpAccountLevel.SelectedIndex = 0;
        //drpAccountLevel_SelectedIndexChanged(null, null);

    }
    protected void btnFilter_Click(object sender, EventArgs e)
    {
        LoadConfiguration("filter");
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {

            bool flag = false;
            string drpAccountHeadValue = drpAccountHead.Value.ToString();
            flag = _cController.UpdateAppConfiguration(Convert.ToInt16(drpAccountHeadMap.SelectedItem.Value), Convert.ToInt64(drpAccountHead.Value));
            if (flag)
            {
                LoadGridData();
                LoadConfiguration("");
                string strLevel = "Level 4";
                if (drpAccountHeadMap.SelectedItem.Value.ToString() == "2" || drpAccountHeadMap.SelectedItem.Value.ToString() == "30")
                {
                    strLevel = "Level 3";
                }
                LoadAccountDetail(strLevel);
                drpAccountHead.Value = drpAccountHeadValue;
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Mapped Successfully!');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Cannnot Configure')", true);
            }
            mPopUpLocation.Show();
        }
        catch (Exception ex)
        {
            ExceptionPublisher.PublishException(ex);
            ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Error Occured: \n" + ex + "');", true);
        }
    }

    #endregion

    protected void drpAccountHeadMap_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strLevel = "Level 4";
        if (drpAccountHeadMap.SelectedItem.Value.ToString() == "2" || drpAccountHeadMap.SelectedItem.Value.ToString() == "30")
        {
            strLevel = "Level 3";
        }
        LoadAccountDetail(strLevel);
        mPopUpLocation.Show();
    }
}