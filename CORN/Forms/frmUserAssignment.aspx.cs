using System;
using System.Data;
using System.Web.UI;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using System.Web;

public partial class Forms_frmUserAssignment : System.Web.UI.Page
{
    UserController mUserController = new UserController();
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        Response.Cache.SetNoStore();
        Response.AppendHeader("pragma", "no-cache");

        if (!Page.IsPostBack)
        {
            LoadDISTRIBUTOR();
            LoadVoucherType();
            LoadUser();
            DistributorType();
            LoadUnAssingned();
            LoadAssingned();
            AssignBrand();
            UnAssignBrand();
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

    /// <summary>
    /// Loads Users To User Combo
    /// </summary>
    private void LoadUser()
    {
        ddUser.Items.Clear();
        Distributor_UserController du = new Distributor_UserController();
        DataTable dt = du.SelectDistributorUser(9, Convert.ToInt32(ddlLocation.SelectedItem.Value), int.Parse(Session["CompanyId"].ToString()));
        clsWebFormUtil.FillDxComboBoxList(ddUser, dt, "USER_ID", "USER_NAME");
        if (dt.Rows.Count > 0)
        {
            ddUser.SelectedIndex = 0;
        }
    }

    protected void ddUser_SelectedIndexChanged(object sender, EventArgs e)
    {

        LoadUnAssingned();
        LoadAssingned();
        UnAssignBrand();
        AssignBrand();
    }
    #region Location Tab

    /// <summary>
    /// Loads LocationTypes To LocationType Combo
    /// </summary>
    private void DistributorType()
    {
        DistributorController dController = new DistributorController();
        DataTable dt = dController.SelectDistributorTypeInfo(Constants.IntNullValue);
        clsWebFormUtil.FillDxComboBoxList(ddDistributorType, dt, 0, 2);
        if (dt.Rows.Count > 0)
        {
            ddDistributorType.SelectedIndex = 0;
        }
    }

    /// <summary>
    /// Loads UnAssigned Locations To UnAssigned ListBox on Location Tab
    /// </summary>
    private void LoadUnAssingned()
    {
        if (ddUser.Items.Count > 0 && ddDistributorType.Items.Count > 0)
        {
            mUserController = new UserController();
            DataTable dt = mUserController.SelectUserAssignmentActive(int.Parse(ddUser.SelectedItem.Value.ToString()), int.Parse(ddDistributorType.SelectedItem.Value.ToString()), 0, int.Parse(this.Session["CompanyId"].ToString()));
            clsWebFormUtil.FillListBox(lstUnAssignDistributor, dt, 0, 1, true);
        }
    }

    /// <summary>
    /// Loads Assigned Locations To Assigned ListBox on Location Tab
    /// </summary>
    private void LoadAssingned()
    {
        if (ddUser.Items.Count > 0 && ddDistributorType.Items.Count > 0)
        {
            mUserController = new UserController();
            DataTable dt = mUserController.SelectUserAssignmentActive(int.Parse(ddUser.SelectedItem.Value.ToString()), int.Parse(ddDistributorType.SelectedItem.Value.ToString()), 1, int.Parse(this.Session["CompanyId"].ToString()));
            clsWebFormUtil.FillListBox(lstAssignDistributor, dt, 0, 1, true);
        }
    }

    /// <summary>
    /// Loads Assigned/UnAssigned Locations To Assigned/UnAssigned ListBox on Location Tab
    /// </summary>
    protected void ddDistributorType_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.LoadUnAssingned();
        this.LoadAssingned();
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
            if (lstUnAssignDistributor.Items[i].Selected == true && ddUser.Items.Count > 0)
            {
                mUserController.InsertUserAssignment(int.Parse(ddUser.SelectedItem.Value.ToString()), int.Parse(ddDistributorType.SelectedItem.Value.ToString()), int.Parse(lstUnAssignDistributor.Items[i].Value.ToString()), int.Parse(this.Session["CompanyId"].ToString()));
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
            if (lstAssignDistributor.Items[i].Selected == true && ddUser.Items.Count > 0)
            {
                mUserController.DeleteUserAssignment(int.Parse(ddUser.SelectedItem.Value.ToString()), int.Parse(ddDistributorType.SelectedItem.Value.ToString()), int.Parse(lstAssignDistributor.SelectedItem.Value.ToString()), int.Parse(this.Session["CompanyId"].ToString()));
            }
        }
        this.LoadUnAssingned();
        this.LoadAssingned();
    }

    #endregion

    #region Principal Tab

    /// <summary>
    /// Loads UnAssigned Principals To User To UnAssigned Principal ListBox On Principal Combo
    /// </summary>
    private void UnAssignBrand()
    {
        if (ddUser.Items.Count > 0)
        {
            SkuHierarchyController sController = new SkuHierarchyController();
            DataTable dt = sController.SelectBrandAssignment(0, int.Parse(ddUser.SelectedItem.Value.ToString()));
            clsWebFormUtil.FillListBox(lstUnAssignBrand, dt, 0, 1, true);
        }
    }

    /// <summary>
    /// Loads nAssigned Principals To User To nAssigned Principal ListBox On Principal Combo
    /// </summary>
    private void AssignBrand()
    {
        if (ddUser.Items.Count > 0)
        {
            SkuHierarchyController sController = new SkuHierarchyController();
            DataTable dt = sController.SelectBrandAssignment(1, int.Parse(ddUser.SelectedItem.Value.ToString()));
            clsWebFormUtil.FillListBox(lstAssignBrand, dt, 0, 1, true);
        }
    }

    /// <summary>
    /// Inserts One Principal Assignment To User
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnAssignPrincipal_Click(object sender, EventArgs e)
    {
        SkuHierarchyController sController = new SkuHierarchyController();
        for (int i = 0; i < lstUnAssignBrand.Items.Count; i++)
        {
            if (lstUnAssignBrand.Items[i].Selected == true && ddUser.Items.Count > 0)
            {
                sController.InsertAssignBrand(int.Parse(lstUnAssignBrand.SelectedItem.Value.ToString()), int.Parse(ddUser.SelectedItem.Value.ToString()));
            }
        }
        this.UnAssignBrand();
        this.AssignBrand();
    }

    /// <summary>
    /// Inserts All Principal Assignment To User
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnAssignAllPrincipal_Click(object sender, EventArgs e)
    {
        SkuHierarchyController sController = new SkuHierarchyController();
        for (int i = 0; i < lstUnAssignBrand.Items.Count; i++)
        {
            if (ddUser.Items.Count > 0)
            {
                sController.InsertAssignBrand(int.Parse(lstUnAssignBrand.Items[i].Value.ToString()), int.Parse(ddUser.SelectedItem.Value.ToString()));

            }
        }
        this.UnAssignBrand();
        this.AssignBrand();
    }

    /// <summary>
    /// Deletes One Principal Assignment To User
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnUnAssignPrincipal_Click(object sender, EventArgs e)
    {
        SkuHierarchyController sController = new SkuHierarchyController();
        for (int i = 0; i < lstAssignBrand.Items.Count; i++)
        {
            if (lstAssignBrand.Items[i].Selected == true && ddUser.Items.Count > 0)
            {
                sController.DeleteAssignBrand(int.Parse(lstAssignBrand.SelectedItem.Value.ToString()), int.Parse(ddUser.SelectedItem.Value.ToString()));

            }
        }
        this.UnAssignBrand();
        this.AssignBrand();
    }

    /// <summary>
    /// Deletes All Principal Assignment To User
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnUnAssignAllPrincipal_Click(object sender, EventArgs e)
    {
        SkuHierarchyController sController = new SkuHierarchyController();
        for (int i = 0; i < lstAssignBrand.Items.Count; i++)
        {
            if (ddUser.Items.Count > 0)
            {
                sController.DeleteAssignBrand(int.Parse(lstAssignBrand.Items[i].Value.ToString()), int.Parse(ddUser.SelectedItem.Value.ToString()));
            }
        }
        UnAssignBrand();
        AssignBrand();
    }

    /// <summary>
    /// Updates Promotion For Principal To Manual Or Auto
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        SkuHierarchyController sController = new SkuHierarchyController();
        for (int i = 0; i < lstAssignBrand.Items.Count; i++)
        {
            if (ddUser.Items.Count > 0)
            {
                sController.UpdateBrandAssignment(int.Parse(ddUser.SelectedItem.Value.ToString()), int.Parse(lstAssignBrand.Items[i].Value), lstAssignBrand.Items[i].Selected);
            }
        }
    }

    #endregion    

    #region VoucherType Tab

    /// <summary>
    /// Loads VoucherTypes To VoucherType ListBox on VoucherType Tab
    /// </summary>
    private void LoadVoucherType()
    {
    }
    
    /// <summary>
    /// Assigns/UnAssigns VoucherTypes To User
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnAssign_Click(object sender, EventArgs e)
    {
        
    }

    #endregion

    protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadUser();
        LoadUnAssingned();
        LoadAssingned();
        UnAssignBrand();
        AssignBrand();
    }
}
