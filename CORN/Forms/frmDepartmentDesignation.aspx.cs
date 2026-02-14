using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;

/// <summary>
/// Form To Add, Edit, Delete Busines Type
/// </summary>
public partial class Forms_frmDepartmentDesignation : System.Web.UI.Page
{   
    EmployeController _empCtrl = new EmployeController();

    /// <summary>
    /// Page_Load Function Populates All Grids On The Page
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
            LoadChannelType();
            LoadBusinessType();
        }
    }

    #region Designation Tab

    /// <summary>
    /// Loads Channel Types To Channel Type Grid
    /// </summary>
    private void LoadChannelType()
    {
        SLASHCodesController mController = new SLASHCodesController();
        DataTable dt = mController.SelectSlashCodes(Constants.IntNullValue, null, Constants.Employee_Designation_Id, null, Constants.IntNullValue, bool.Parse("True"));
        grdChannelData.DataSource = dt.DefaultView;
        grdChannelData.DataBind();
     }    

    /// <summary>
    /// Sets Channel Type For Edit. This Function Runs When An Existing Channel Type Needs To Be Edited
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">GridViewEditEventArgs</param>
    protected void grdChannelData_RowEditing(object sender, GridViewEditEventArgs e)
    {
        RefId.Value = grdChannelData.Rows[e.NewEditIndex].Cells[0].Text;
        if (RefId.Value == "35" || RefId.Value == "36")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Order Taker and Delivery Man can not be edited.');", true);
            return;
        }
        txtChannelCode.Text = grdChannelData.Rows[e.NewEditIndex].Cells[1].Text;
        txtChannelName.Text = grdChannelData.Rows[e.NewEditIndex].Cells[2].Text;
        btnSaveChannelType.Text = "Update";
        txtChannelName.Enabled = true;
    }

    /// <summary>
    /// Deletes Channel Type
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">GridViewEditEventArgs</param>
    protected void grdChannelData_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        SLASHCodesController mController = new SLASHCodesController();
        RefId.Value = grdChannelData.Rows[e.RowIndex].Cells[0].Text;
        if(RefId.Value == "35" || RefId.Value == "36")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Order Taker and Delivery Man can not be deleted.');", true);
            return;
        }
        mController.UpdateSlashCodes(Convert.ToInt32(RefId.Value), null, Constants.Employee_Designation_Id, null, 1, false, Constants.DateNullValue);

        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Record removed successfully.');", true);

        this.LoadChannelType();

    }
    
    /// <summary>
    /// Save Or Updates a Channel Type
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnSaveChannelType_Click(object sender, EventArgs e)
    {
        SLASHCodesController mController = new SLASHCodesController();
        lblErrorMsg.Visible = false;
        lblErrorMsg.Text = "";
        if (btnSaveChannelType.Text == "New")
        {
            txtChannelCode.Text = this.GetAutoCode("DSG", 0, Constants.LongNullValue);
            txtChannelName.Enabled = true;
            txtChannelName.Focus();
            btnSaveChannelType.Text = "Save";
            ScriptManager.GetCurrent(Page).SetFocus(txtChannelName);
        }
        else if (btnSaveChannelType.Text == "Save")
        {
            if (txtChannelName.Text.Length == 0)
            {
                lblErrorMsg.Visible = true;
                lblErrorMsg.Text = Utility.ShowAlert(false, "Designation name is required");
                return;
            }
            mController.InsertSlashCodes(txtChannelCode.Text, Constants.Employee_Designation_Id, txtChannelName.Text, 1, true);
            this.GetAutoCode("DSG", 1, long.Parse(txtChannelCode.Text.Substring(3)));
            this.LoadChannelType();

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AlertBox", "alert('Record added successfully.');", true);
            txtChannelCode.Text = "";
            txtChannelName.Text = "";
            txtChannelName.Enabled = false;
            btnSaveChannelType.Text = "New";
            
           
        }
        else if (btnSaveChannelType.Text == "Update")
        {
            mController.UpdateSlashCodes(Convert.ToInt32(RefId.Value), null, Constants.Employee_Designation_Id, txtChannelName.Text, 1, true, Constants.DateNullValue);
            this.LoadChannelType();
            txtChannelCode.Text = "";
            txtChannelName.Text = "";
            txtChannelName.Enabled = false;
            btnSaveChannelType.Text = "New";

            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Record updated successfully.');", true);
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e) 
    {
       
        txtChannelCode.Text = "";
        txtChannelName.Text = "";
        txtChannelName.Enabled = false;
        btnSaveChannelType.Text = "New";
        lblErrorMsg.Text = "";
        lblErrorMsg.Visible = false;
    }

    #endregion

    #region Department Type Tab

    /// <summary>
    /// Loads Business Types To Business Type Grid
    /// </summary>
    private void LoadBusinessType()
    {
        SLASHCodesController mController = new SLASHCodesController();
        DataTable dt = mController.SelectSlashCodes(Constants.IntNullValue, null, Constants.Employee_Depoartment_Id, null, Constants.IntNullValue, bool.Parse("True"));
        GrdBusType.DataSource = dt.DefaultView;
        GrdBusType.DataBind();
    }

    /// <summary>
    /// Sets PageIndex Of Business Type Grid
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">GridViewPageEventArgs</param>
    protected void GrdBusType_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GrdBusType.PageIndex = e.NewPageIndex;
        this.LoadBusinessType();
    }

    /// <summary>
    /// Sets Business Type For Edit. This Function Runs When An Existing Business Type Needs To Be Edited
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">GridViewEditEventArgs</param>
    protected void GrdBusType_RowEditing(object sender, GridViewEditEventArgs e)
    {
        RefId.Value = GrdBusType.Rows[e.NewEditIndex].Cells[0].Text;
        txtbustypeCode.Text = GrdBusType.Rows[e.NewEditIndex].Cells[1].Text;
        txtbustypeName.Text = GrdBusType.Rows[e.NewEditIndex].Cells[2].Text;
        txtbustypeName.Enabled = true;
        btnSaveBusType.Text = "Update";

    }

    /// <summary>
    /// Deletes Business Type
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">GridViewEditEventArgs</param>
    protected void GrdBusType_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        SLASHCodesController mController = new SLASHCodesController();
        RefId.Value = GrdBusType.Rows[e.RowIndex].Cells[0].Text;
        mController.UpdateSlashCodes(Convert.ToInt32(RefId.Value), null, Constants.Employee_Depoartment_Id, null, 1, false, Constants.DateNullValue);

        ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Record removed successfully.');", true);

        this.LoadBusinessType();
    }

    /// <summary>
    /// Save Or Updates a Business Type
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnSaveBusType_Click(object sender, EventArgs e)
    {
        SLASHCodesController mController = new SLASHCodesController();
        lblErrorMsgDivsion.Visible = false;
        lblErrorMsgDivsion.Text = "";
        if (btnSaveBusType.Text == "New")
        {
            txtbustypeCode.Text = this.GetAutoCode("DPT", 0, Constants.LongNullValue);
            txtbustypeName.Enabled = true;
            txtbustypeName.Focus();
            btnSaveBusType.Text = "Save";
            ScriptManager.GetCurrent(Page).SetFocus(txtbustypeName);
        }
        else if (btnSaveBusType.Text == "Save")
        {
            if (txtbustypeName.Text.Length == 0)
            {
                lblErrorMsgDivsion.Visible = true;
                lblErrorMsgDivsion.Text = Utility.ShowAlert(false, "Department name is required");
                return;
            }
            mController.InsertSlashCodes(txtbustypeCode.Text, Constants.Employee_Depoartment_Id, txtbustypeName.Text, 1, true);
            this.GetAutoCode("DPT", 1, long.Parse(txtbustypeCode.Text.Substring(3)));
            this.LoadBusinessType();

            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AlertBox", "alert('Record added successfully.');", true);

            txtbustypeCode.Text = "";
            txtbustypeName.Text = "";
            txtbustypeName.Enabled = false;
            btnSaveBusType.Text = "New";
          
        }
        else if (btnSaveBusType.Text == "Update")
        {
            mController.UpdateSlashCodes(Convert.ToInt32(RefId.Value), null, Constants.Employee_Depoartment_Id, txtbustypeName.Text, 1, true, Constants.DateNullValue);
            this.LoadBusinessType();
            txtbustypeCode.Text = "";
            txtbustypeName.Text = "";
            txtbustypeName.Enabled = false;
            btnSaveBusType.Text = "New";
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Record updated successfully.');", true);
        }
    }

    protected void btncancelDestype_Click(object sender, EventArgs e)
    {
        
        txtbustypeCode.Text = "";
        txtbustypeName.Text="";
        txtbustypeName.Enabled = false;
        btnSaveBusType.Text = "New";
        lblErrorMsg.Text = "";
        lblErrorMsgDivsion.Visible = false;
    }
    
    #endregion       

    private string GetAutoCode(string PreeFix, int CodeType,long CValue)
    {
        SETTINGS_TABLE_Controller AutoCode = new SETTINGS_TABLE_Controller();
        return AutoCode.GetAutoCode(PreeFix,CodeType,CValue);
    }
    protected void grdChannelData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdChannelData.PageIndex = e.NewPageIndex;
        LoadChannelType();
    }
}