using System;
using System.Data;
using System.Web;
using System.Web.UI;
using CORNCommon.Classes;
using CORNBusinessLayer.Classes;
using System.Web.UI.WebControls;
using System.IO;

/// <summary>
/// From to Assign, UnAssign Forms To Users
/// </summary>
public partial class frmEcommCatgoryWiseItem : System.Web.UI.Page
{
    protected string allowed_extensions = "gif|jpeg|jpg|png";
    readonly SkuController mController = new SkuController();
    readonly DataControl dc = new DataControl();

    /// <summary>
    /// Page_Load Function Populates All Combos and ListBox On The Page
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
            LoadPOSCategories();
            LoadCategories();
            LoadParentCategories();
        }
    }

    /// <summary>
    /// Loads Categories
    /// </summary>
    private void LoadCategories()
    {
        var categoryController = new SkuHierarchyController();
        DataTable dt = categoryController.SelectECommerceCategories(Constants.IntNullValue, 1);
        dt.DefaultView.Sort = "SortOrder";
       
        clsWebFormUtil.FillDxComboBoxList(ddlCategory, dt, "CategoryID", "Name", true);

        if (dt.Rows.Count > 0)
        {
            ddlCategory.SelectedIndex = 0;

            GetUnAssignedItemsINCategories(Convert.ToInt32(ddlCategory.Value.ToString()), Convert.ToInt32(ddlCategory1.Value.ToString()));
            GetAssignedItemsINCategories(Convert.ToInt32(ddlCategory.Value.ToString()), Constants.IntNullValue);
        }
    }

    private void LoadPOSCategories()
    {
        var categoryController = new SkuHierarchyController();
        DataTable dt = new DataTable();
        dt = categoryController.SelectSkuHierarchy(Constants.SKUCategory, Constants.IntNullValue,
            Constants.IntNullValue, null, null, true,
            int.Parse(this.Session["CompanyId"].ToString()), Constants.IntNullValue);

        clsWebFormUtil.FillDxComboBoxList(ddlCategory1, dt, "SKU_HIE_ID", "SKU_HIE_NAME", true);

        if (dt.Rows.Count > 0)
        {
            ddlCategory1.SelectedIndex = 0;

            //GetUnAssignedItemsINCategories(Convert.ToInt32(ddlCategory.Value.ToString()));
            //GetAssignedItemsINCategories(Convert.ToInt32(ddlCategory.Value.ToString()));
        }
    }

    public void LoadParentCategories()
    {
        try
        {
            drpParentCategory.Items.Clear();

            var categoryController = new SkuHierarchyController();
            DataTable dt = categoryController.SelectECommerceCategories(Constants.IntNullValue,1);
            drpParentCategory.Items.Add(new DevExpress.Web.ListEditItem("---Select---", Constants.IntNullValue.ToString()));

            clsWebFormUtil.FillDxComboBoxList(drpParentCategory, dt, "CategoryID", "Name", false);
            drpParentCategory.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "CatchMsg", "alert('" + ex.Message.ToString() + "');", true);
        }
    }

    /// <summary>
    /// Loads Fourth Layers Modules Not Assigned To Role To ListBox
    /// </summary>
    /// <param name="RoleID"></param>
    private void GetUnAssignedItemsINCategories(int eCommerceCategory, int posCategory)
    {
        DataTable dt = mController.GetAssignUnAssignItemsINECommCategory(
            int.Parse(dc.chkNull(ddlCategory.SelectedItem.Value.ToString())),
            int.Parse(dc.chkNull(ddlCategory1.SelectedItem.Value.ToString())), 0);
        lstUnAssignModule.DataSource = dt.DefaultView;
        lstUnAssignModule.DataTextField = "SKU_NAME";
        lstUnAssignModule.DataValueField = "SKU_ID";
        lstUnAssignModule.DataBind();
    }
    
    /// <summary>
    /// Loads Fourth Layers Modules Assigned To Role To ListBox
    /// </summary>
    /// <param name="RoleID"></param>
    private void GetAssignedItemsINCategories(int eCommerceCategoryID, int posCategoryID)
    {
        DataTable dt = mController.GetAssignUnAssignItemsINECommCategory(int.Parse(dc.chkNull(ddlCategory.SelectedItem.Value.ToString())), Constants.IntNullValue, 1);
        //dt.DefaultView.Sort = "MODULE_DESCRIPTION";
        lstAssignModule.DataSource = dt.DefaultView;
        lstAssignModule.DataTextField = "SKU_NAME";
        lstAssignModule.DataValueField = "SKU_ID";
        lstAssignModule.DataBind();
    }

    /// <summary>
    /// Loads Fourth Layers Modules Assigned To Role To ListBox,Fourth Layers Modules Not Assigned To Role To ListBox,
    /// Loads Second Layer Module To Second Layer Module Combo And Loads Third Layer Module To Third Layer Module Combo
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.GetUnAssignedItemsINCategories(int.Parse(ddlCategory.SelectedItem.Value.ToString()), int.Parse(ddlCategory1.SelectedItem.Value.ToString()));
        this.GetAssignedItemsINCategories(int.Parse(ddlCategory.SelectedItem.Value.ToString()), Constants.IntNullValue);  
    }

    protected void ddlCategory1_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.GetUnAssignedItemsINCategories(int.Parse(ddlCategory.SelectedItem.Value.ToString()), int.Parse(ddlCategory1.SelectedItem.Value.ToString()));
        this.GetAssignedItemsINCategories(int.Parse(ddlCategory.SelectedItem.Value.ToString()), Constants.IntNullValue);
    }

    /// <summary>
    /// Assigns Page To a Role
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnAssign_Click(object sender, EventArgs e)
    {
        if (lstUnAssignModule.Items.Count > 0)
        {
            for (int i = 0; i < lstUnAssignModule.Items.Count; i++)
            {

                if (lstUnAssignModule.Items[i].Selected == true)
                {
                    mController.InsertItemAgainstCategory(int.Parse(ddlCategory.SelectedItem.Value.ToString()), Convert.ToInt32(lstUnAssignModule.SelectedValue.ToString()), 1);
                }
            }
            this.GetUnAssignedItemsINCategories(int.Parse(ddlCategory.SelectedItem.Value.ToString()), int.Parse(ddlCategory1.SelectedItem.Value.ToString()));
            this.GetAssignedItemsINCategories(int.Parse(ddlCategory.SelectedItem.Value.ToString()), Constants.IntNullValue);
        }
    }
    protected void btnAssignAllItem_Click(object sender, EventArgs e)
    {
        if (lstUnAssignModule.Items.Count > 0)
        {
            for (int i = 0; i < lstUnAssignModule.Items.Count; i++)
            {
                   mController.InsertItemAgainstCategory(int.Parse(ddlCategory.SelectedItem.Value.ToString()), Convert.ToInt32(lstUnAssignModule.Items[i].Value), 1);
            }
            this.GetUnAssignedItemsINCategories(int.Parse(ddlCategory.SelectedItem.Value.ToString()), int.Parse(ddlCategory1.SelectedItem.Value.ToString()));
            this.GetAssignedItemsINCategories(int.Parse(ddlCategory.SelectedItem.Value.ToString()), Constants.IntNullValue);
        }

    }

    /// <summary>
    /// UnAssigns Page To a Role
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnUnAssign_Click(object sender, EventArgs e)
    {
        if (this.lstAssignModule.Items.Count > 0)
        {
            for (int i = 0; i < lstAssignModule.Items.Count; i++)
            {
                if (lstAssignModule.Items[i].Selected == true)
                {

                  mController.InsertItemAgainstCategory(int.Parse(ddlCategory.SelectedItem.Value.ToString()), Convert.ToInt32(lstAssignModule.SelectedValue.ToString()), 0);

                }
            }
            this.GetUnAssignedItemsINCategories(int.Parse(ddlCategory.SelectedItem.Value.ToString()), int.Parse(ddlCategory1.SelectedItem.Value.ToString()));
            this.GetAssignedItemsINCategories(int.Parse(ddlCategory.SelectedItem.Value.ToString()), Constants.IntNullValue);
        }
    }
    protected void btnUnAssignAllItem_Click(object sender, EventArgs e)
    {
        if (this.lstAssignModule.Items.Count > 0)
        {
            for (int i = 0; i < lstAssignModule.Items.Count; i++)
            {
               mController.InsertItemAgainstCategory(int.Parse(ddlCategory.SelectedItem.Value.ToString()), Convert.ToInt32(lstAssignModule.Items[i].Value), 0);
            }
            this.GetUnAssignedItemsINCategories(int.Parse(ddlCategory.SelectedItem.Value.ToString()), int.Parse(ddlCategory1.SelectedItem.Value.ToString()));
            this.GetAssignedItemsINCategories(int.Parse(ddlCategory.SelectedItem.Value.ToString()), Constants.IntNullValue);
        }
    }

    protected void ibItemTop_Click(object sender, ImageClickEventArgs e)
    {
        for (int i = 0; i < lstAssignModule.Items.Count; i++)
        {
            if (lstAssignModule.Items[i].Selected)//identify the selected item
            {
                //swap with the top item(move up)
                if (i > 0 && !lstAssignModule.Items[i - 1].Selected)
                {
                    ListItem top = lstAssignModule.Items[i];
                    lstAssignModule.Items.Remove(top);
                    lstAssignModule.Items.Insert(0, top);
                    lstAssignModule.Items[0].Selected = true;
                }
            }
        }
    }

    protected void ibItemUp_Click(object sender, ImageClickEventArgs e)
    {
        for (int i = 0; i < lstAssignModule.Items.Count; i++)
        {
            if (lstAssignModule.Items[i].Selected)//identify the selected item
            {
                //swap with the top item(move up)
                if (i > 0 && !lstAssignModule.Items[i - 1].Selected)
                {
                    ListItem top = lstAssignModule.Items[i];
                    lstAssignModule.Items.Remove(top);
                    lstAssignModule.Items.Insert(i-1, top);
                    lstAssignModule.Items[i-1].Selected = true;
                }
            }
        }
    }

    protected void ibItemDown_Click(object sender, ImageClickEventArgs e)
    {
        for (int i = 0; i < lstAssignModule.Items.Count; i++)
        {
            if (lstAssignModule.Items[i].Selected)//identify the selected item
            {
                //swap with the top item(move up)
                if (i > 0 && !lstAssignModule.Items[i - 1].Selected)
                {
                    ListItem top = lstAssignModule.Items[i];
                    lstAssignModule.Items.Remove(top);
                    lstAssignModule.Items.Insert(i + 1, top);
                    lstAssignModule.Items[i + 1].Selected = true;
                }
            }
        }
    }

    protected void ibItemBottom_Click(object sender, ImageClickEventArgs e)
    {
        for (int i = 0; i < lstAssignModule.Items.Count; i++)
        {
            if (lstAssignModule.Items[i].Selected)//identify the selected item
            {
                //swap with the top item(move up)
                if (i > 0 && !lstAssignModule.Items[i - 1].Selected)
                {
                    var totalItems = lstAssignModule.Items.Count;
                    ListItem top = lstAssignModule.Items[i];
                    lstAssignModule.Items.Remove(top);
                    lstAssignModule.Items.Insert(totalItems - 1, top);
                    lstAssignModule.Items[totalItems - 1].Selected = true;
                }
            }
        }
    }

    /// <summary>
    /// Shows All Roles And Thier Assigned Pages
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnReport_Click(object sender, EventArgs e)
    {
        //DocumentPrintController DPrint = new DocumentPrintController();

        //DataSet ds = DPrint.SelectRoleManagmentReport(int.Parse(ddRole.SelectedItem.Value.ToString()));
        //DataTable dt = DPrint.SelectReportTitle(Constants.IntNullValue);

        //CORNBusinessLayer.Reports.CrpRoleManagement CrpReport = new CORNBusinessLayer.Reports.CrpRoleManagement();
        //CrpReport.SetDataSource(ds);
        //CrpReport.Refresh();

        //CrpReport.SetParameterValue("Company_Name", dt.Rows[0]["COMPANY_NAME"].ToString());

        //this.Session.Add("CrpReport", CrpReport);
        //this.Session.Add("ReportType", 0);
        //string url = "'Default.aspx'";
        //string script = "<script language='JavaScript' type='text/javascript'> window.open(" + url + ",\"Link\",\"toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=800,height=600,left=10,top=10\");</script>";
        //Type cstype = this.GetType();
        //ClientScriptManager cs = Page.ClientScript;
        //cs.RegisterStartupScript(cstype, "OpenWindow", script);
    }
    protected void btnSaveCategory_Click(object sender, EventArgs e)
    {
        try
        {   
            UploadImage();

            if (drpParentCategory.Text.ToLower() == txtCategoryName.Text.ToLower())
            {
                mPopUpCategory.Show();
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "Message('Parent and Child Category Cannot be same')", true);
                return;
            }

            if (btnSaveCategory.Text == "Save")
            {
                mController.InsertECommerceCategory(txtCategoryName.Text,
                    Convert.ToInt32(drpParentCategory.SelectedItem.Value.ToString()),
                    Convert.ToInt32(DataControl.chkNull_Zero(txtSortOrder.Text)),
                    hidSKUImageName.Value, Constants.IntNullValue);
                mPopUpCategory.Show();
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "Message('Record added successfully')", true);
            }
            else
            {
                mController.InsertECommerceCategory(txtCategoryName.Text, 
                    Convert.ToInt32(drpParentCategory.SelectedItem.Value.ToString()),
                    Convert.ToInt32(DataControl.chkNull_Zero(txtSortOrder.Text)),
                    hidSKUImageName.Value, int.Parse(ddlCategory.SelectedItem.Value.ToString()));
            }
            LoadCategories();
            hidSKUImageName.Value = "";
            txtCategoryName.Text = "";
            txtSortOrder.Text = "1";
            btnSaveCategory.Text = "Save";
            LoadParentCategories();
            hidSKUImageSource.Value = "";
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "bindSKUImage", "bindSKUImage();", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "CatchMsg", "alert('" + ex.Message.ToString() + "');", true);
            mPopUpCategory.Show();
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        mPopUpCategory.Show();
        drpParentCategory.SelectedIndex = 0;
        hidSKUImageName.Value = "";
        txtCategoryName.Text = "";
        btnSaveCategory.Text = "Save";
        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "bindSKUImage", "bindSKUImage();", true);
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        mPopUpCategory.Show();
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        mPopUpCategory.Show();
        var categoryController = new SkuHierarchyController();
        if (ddlCategory.SelectedIndex != -1)
        {
            DataTable dt = categoryController.SelectECommerceCategories(int.Parse(ddlCategory.Value.ToString()),1);

            if (dt != null && dt.Rows.Count > 0)
            {
                txtCategoryName.Text = dt.Rows[0]["Name"].ToString();

                if (string.IsNullOrEmpty(dt.Rows[0]["ParentCategoryID"].ToString()) ||
                    dt.Rows[0]["ParentCategoryID"].ToString() == "0")
                {
                    drpParentCategory.SelectedIndex = 0;
                }
                else
                {
                    drpParentCategory.Value = dt.Rows[0]["ParentCategoryID"].ToString();
                }
                txtSortOrder.Text = dt.Rows[0]["SortOrder"].ToString();
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "bindSKUImage", "bindSKUImage();", true);
                hidSKUImageName.Value = dt.Rows[0]["ImgUrl"].ToString();

                btnSaveCategory.Text = "Update";
            }
        }
    }
    protected void btnClose_Click(object sender, EventArgs e)
    {
        mPopUpCategory.Hide();
        drpParentCategory.SelectedIndex = 0;
        hidSKUImageName.Value = "";
        txtCategoryName.Text = "";
        btnSaveCategory.Text = "Save";
    }
    private void UploadImage()
    {
        try
        {
            if (hidSKUImageSource.Value.Length > 0)
            {
                string imageExtention = Path.GetExtension(hidSKUImageName.Value);
                hidSKUImageName.Value = "Promotion_" + Session["UserID"].ToString() + "_" + DateTime.Now.ToString("MMddyyyyhhmmssfff") + imageExtention;
                string file_name = hidSKUImageName.Value;
                if (file_name.Length == 0) return;
                string file_path = Server.MapPath("../UserImages/Category");
                if (!Directory.Exists(file_path))
                {
                    Directory.CreateDirectory(file_path);
                }
                string temp = hidSKUImageSource.Value.Substring(0, hidSKUImageSource.Value.IndexOf("base64") + "base64".Length + 1);
                hidSKUImageSource.Value = hidSKUImageSource.Value.Replace(temp, "");
                byte[] renderedBytes = Convert.FromBase64String(hidSKUImageSource.Value);
                using (FileStream fileStream = File.Create(file_path + "\\" + file_name, renderedBytes.Length))
                {
                    fileStream.Write(renderedBytes, 0, renderedBytes.Length);
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}
