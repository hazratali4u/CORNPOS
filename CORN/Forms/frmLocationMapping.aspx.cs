using System;
using System.Data;
using System.Web.UI;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using System.Web;

public partial class Forms_frmLocationMapping : System.Web.UI.Page
{
    UserController mUserController = new UserController();
    DistributorController mDistrCtrl = new DistributorController();
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

    private void LoadDISTRIBUTOR()
    {
        DistributorController mController = new DistributorController();
        DataTable dtDistributor = mController.GetDistributorWithMaxDayClose(Constants.IntNullValue,Constants.IntNullValue,5,2);

        clsWebFormUtil.FillDxComboBoxList(ddlLocation, dtDistributor, "DISTRIBUTOR_ID", "DISTRIBUTOR_NAME");

        if (dtDistributor.Rows.Count > 0)
        {
            ddlLocation.SelectedIndex = 0;
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
            mUserController = new UserController();
            DataTable dt = mDistrCtrl.GetWarehouseMapping(2, Convert.ToInt32(ddlLocation.SelectedItem.Value));
            clsWebFormUtil.FillListBox(lstUnAssignDistributor, dt, 0, 1, true);
        }
    }

    /// <summary>
    /// Loads Assigned Locations To Assigned ListBox on Location Tab
    /// </summary>
    private void LoadAssingned()
    {
        if (ddlLocation.Items.Count > 0)
        {
            mUserController = new UserController();
            DataTable dt = mDistrCtrl.GetWarehouseMapping(3, Convert.ToInt32(ddlLocation.SelectedItem.Value));
            clsWebFormUtil.FillListBox(lstAssignDistributor, dt, 0, 1, true);
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
            if (lstUnAssignDistributor.Items[i].Selected)
            {
                mDistrCtrl.InsertWarehouseMapping(1, Convert.ToInt32(ddlLocation.SelectedItem.Value), int.Parse(lstUnAssignDistributor.Items[i].Value.ToString()), Convert.ToInt32(Session["UserID"]));
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
        mUserController = new UserController();
        for (int i = 0; i < lstAssignDistributor.Items.Count; i++)
        {
            if (lstAssignDistributor.Items[i].Selected)
            {
                mDistrCtrl.InsertWarehouseMapping(4, Convert.ToInt32(ddlLocation.SelectedItem.Value), int.Parse(lstAssignDistributor.Items[i].Value.ToString()), Convert.ToInt32(Session["UserID"]));
            }
        }
        this.LoadUnAssingned();
        this.LoadAssingned();
    }

    #endregion
        
    protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadAssingned();   
    }
}