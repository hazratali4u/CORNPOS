using System;
using System.Data;
using System.Web.UI.WebControls;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using System.Web;
using System.Web.UI;
using System.IO;
using OfficeOpenXml;

/// <summary>
/// From To Add, Edit, Delete SKU Hierarchy
/// </summary>
public partial class frmCategory : System.Web.UI.Page
{
    protected string allowed_extensions = "gif|jpeg|jpg|png";
    readonly SkuHierarchyController mController = new SkuHierarchyController();
    readonly SkuController _mSkuController = new SkuController();

    /// <summary>
    /// Page_Load Function Populates All Combos And Grids On The Page
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        Response.Cache.SetNoStore();
        Response.AppendHeader("pragma", "no-cache");

        ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
        scriptManager.RegisterPostBackControl(this.btnExportCategoryTemplate);

        if (!IsPostBack)
        {
            Session.Remove("dtGridData");
            LoadCategoryType();
            LoadParentCategories();
            LoadGridData();
            LoadGrid("");
            btnSaveCategory.Attributes.Add("onclick", "return ValidateForm()");
            mPOPImport.Hide();
        }
    }
    private void LoadCategoryType()
    {
        try
        {
            DataTable dt = _mSkuController.SelectCategoryType();
            clsWebFormUtil.FillDxComboBoxList(ddlType, dt, 0, 1, true);
            if (dt.Rows.Count > 0)
            {
                ddlType.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "CatchMsg", "alert('" + ex.Message.ToString() + "');", true);
        }
    }

    /// <summary>
    /// Loads Categories To Category Grid On Category Tab
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void DrpCategoryPrincipal_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.LoadGrid("");
    }
    private void LoadGridData()
    {
        DataTable dt = new DataTable();
        dt = mController.SelectSkuHierarchy(Constants.SKUCategory, Constants.IntNullValue, Constants.IntNullValue, null, null, true, int.Parse(this.Session["CompanyId"].ToString()), Constants.IntNullValue);
        Session.Add("dtGridData", dt);
    }
    private void LoadGrid(string pType)
    {
        DataTable dt = (DataTable)Session["dtGridData"];
        if (pType == "")
        {
            if (txtSearch.Text != "" || txtSearch.Text != string.Empty)
            {
                dt.DefaultView.RowFilter = "SKU_HIE_NAME LIKE '%" + txtSearch.Text + "%' OR TYPE LIKE '%" + txtSearch.Text + "%' OR IS_ACTIVE LIKE '" + txtSearch.Text + "%'";
            }
            GrdCategory.DataSource = dt;
            GrdCategory.DataBind();
        }
        else
        {
            if (txtSearch.Text != "" || txtSearch.Text != string.Empty)
            {
                dt.DefaultView.RowFilter = "SKU_HIE_NAME LIKE '%" + txtSearch.Text + "%' OR TYPE LIKE '%" + txtSearch.Text + "%' OR IS_ACTIVE LIKE '" + txtSearch.Text + "%'";
            }
            else
            {
                dt.DefaultView.RowFilter = null;
            }
            if (dt.Rows.Count > 0)
            {
                GrdCategory.PageIndex = 0;
            }
            GrdCategory.DataSource = dt;
            GrdCategory.DataBind();
        }
    }

    public void CheckCategoryUsed(int CategoryID)
    {
        DataTable dt = mController.SelectSkuHierarchy(Constants.IntNullValue, CategoryID, Constants.IntNullValue, null, null, true, 17, Constants.IntNullValue);
        if (Convert.ToInt32(dt.Rows[0]["USED"].ToString()) == 1)
        {
            ddlType.Enabled = false;
        }
        else
        {
            ddlType.Enabled = true;
        }
    }
    protected void GrdCategory_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            mPopUpCategory.Show();
            btnSaveCategory.Text = "Update";
            hidSKUImageName.Value = (GrdCategory.Rows[e.NewEditIndex].FindControl("hidSKUImageName") as HiddenField).Value;
            if (skuImageUploadArea.Visible)
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "bindSKUImage", "bindSKUImage();", true);
            }
            hfcategoryId.Value = GrdCategory.Rows[e.NewEditIndex].Cells[1].Text;
            txtCategoryCode.Text = GrdCategory.Rows[e.NewEditIndex].Cells[2].Text;
            txtCategoryName.Text = GrdCategory.Rows[e.NewEditIndex].Cells[3].Text;
            hfStatus.Value = GrdCategory.Rows[e.NewEditIndex].Cells[5].Text;
            ddlType.Value = GrdCategory.Rows[e.NewEditIndex].Cells[7].Text;
            ddlType_SelectedIndexChanged(null, null);
            try
            {
                drpParentCategory.Value = GrdCategory.Rows[e.NewEditIndex].Cells[8].Text.Replace("&nbsp;", "0");
            }
            catch (Exception)
            {
                drpParentCategory.SelectedIndex = 0;
            }

            txtSort.Text = Server.HtmlDecode(GrdCategory.Rows[e.NewEditIndex].Cells[9].Text);
            cbOpenItemCategory.Checked = Convert.ToBoolean(GrdCategory.Rows[e.NewEditIndex].Cells[10].Text);
            chbMultiSelectModifier.Checked = Convert.ToBoolean(GrdCategory.Rows[e.NewEditIndex].Cells[11].Text);

            txtCategoryName.Enabled = true;
            CheckCategoryUsed(Convert.ToInt32(hfcategoryId.Value));
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "CatchMsg", "alert('" + ex.Message.ToString() + "');", true);
        }
    }

    protected void GrdCategory_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GrdCategory.PageIndex = e.NewPageIndex;
        LoadGrid("");
    }

    public bool CheckHierarchyLevel()
    {
        DataTable dt = new DataTable();
        dt = mController.SelectSkuHierarchy(Constants.SKUCategory, Convert.ToInt32(drpParentCategory.SelectedItem.Value), Constants.IntNullValue, null, null, true, 16, Constants.IntNullValue);
        int level = Convert.ToInt32(dt.Rows[0][0].ToString());
        if (level < 4)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    protected void btnSaveCategory_Click(object sender, EventArgs e)
    {
        try
        {
            if (!CheckHierarchyLevel())
            {
                mPopUpCategory.Show();
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "ErrorMessage()", true);
                return;
            }

            UploadImage();
            var sortOrder = DataControl.chkNull_Zero(Server.HtmlDecode(txtSort.Text));
            if (string.IsNullOrEmpty(sortOrder))
            {
                sortOrder = Constants.IntNullValue.ToString();
            }

            if (btnSaveCategory.Text == "Save")
            {
                txtCategoryCode.Text = "";
                mController.InsertHierarchy(Constants.SKUCategory,
                    Convert.ToInt32(ddlType.SelectedItem.Value), txtCategoryCode.Text,
                    txtCategoryName.Text, null, true, 
                    int.Parse(this.Session["CompanyId"].ToString()),
                    Convert.ToInt32(DataControl.chkNull_Zero(drpParentCategory.SelectedItem.Value.ToString())),
                    hidSKUImageName.Value,int.Parse(sortOrder),cbOpenItemCategory.Checked,
                    chbMultiSelectModifier.Checked
                    );
                ClearAll();
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "Message('Record added successfully')", true);
                mPopUpCategory.Show();
            }
            else if (btnSaveCategory.Text == "Update")
            {
                bool status = true;
                if (hfStatus.Value != "Active")
                {
                    status = false;
                }
                mController.UpdateHierarchy(Constants.SKUCategory,
                    Convert.ToInt32(hfcategoryId.Value), Convert.ToInt32(ddlType.SelectedItem.Value),
                    txtCategoryCode.Text,txtCategoryName.Text, null, status,
                    int.Parse(this.Session["CompanyId"].ToString()), 
                    Convert.ToInt32(DataControl.chkNull_Zero(drpParentCategory.SelectedItem.Value.ToString())),
                    hidSKUImageName.Value,int.Parse(sortOrder),cbOpenItemCategory.Checked,
                    chbMultiSelectModifier.Checked
                    );
                ClearAll();
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "Message('Record updated successfully')", true);
                mPopUpCategory.Hide();
            }
            LoadParentCategories();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "CatchMsg", "alert('" + ex.Message.ToString() + "');", true);
            mPopUpCategory.Show();
        }
    }
    private void ClearAll()
    {
        txtCategoryCode.Text = "";
        txtCategoryName.Text = "";
        hidSKUImageName.Value = "";
        hidSKUImageSource.Value = "";
        LoadGridData();
        LoadGrid("");
        btnSaveCategory.Text = "Save";
        ddlType.Enabled = true;
        txtSort.Text = "0";
        cbOpenItemCategory.Checked = false;
        chbMultiSelectModifier.Checked = false;
    }
    private string GetAutoCode(string PreeFix, int CodeType)
    {
        SETTINGS_TABLE_Controller AutoCode = new SETTINGS_TABLE_Controller();
        return AutoCode.GetAutoCode(PreeFix, CodeType, Constants.LongNullValue);
    }

    private void SetAutoCode(string PreeFix, long CValue)
    {
        SETTINGS_TABLE_Controller AutoCode = new SETTINGS_TABLE_Controller();
        string result = AutoCode.GetAutoCode(PreeFix, 1, CValue);
    }
    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        mPopUpCategory.Show();
        if (ddlType.Value.ToString() == "1")
        {
            cbOpenItemCategory.Visible = true;
            sortRow.Visible = true;
        }
        else
        {
            cbOpenItemCategory.Visible = false;
            cbOpenItemCategory.Checked = false;
            sortRow.Visible = false;
            txtSort.Text = "0";
        }
        LoadParentCategories();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        mPopUpCategory.Show();
        ClearAll();
        ddlType.SelectedIndex = 0;
        ddlType_SelectedIndexChanged(null, null);
        drpParentCategory.SelectedIndex = 0;        
    }

    protected void btnFilter_Click(object sender, EventArgs e)
    {
        LoadGrid("filter");
    }
    protected void btnActive_Click(object sender, EventArgs e)
    {
        bool check = false;
        UserController _UserCtrl = new UserController();
        try
        {
            foreach (GridViewRow dr2 in GrdCategory.Rows)
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

            foreach (GridViewRow dr in GrdCategory.Rows)
            {
                var chRelized = (CheckBox)dr.Cells[0].FindControl("ChbIsAssigned");

                if (chRelized.Checked)
                {
                    if (Convert.ToString(dr.Cells[5].Text) == "Active")
                    {


                        _UserCtrl.ActiveInactive(false, Convert.ToInt32(dr.Cells[1].Text), Constants.IntNullValue, 5);

                        flag = true;
                    }
                    else
                    {


                        _UserCtrl.ActiveInactive(true, Convert.ToInt32(dr.Cells[1].Text), Constants.IntNullValue, 5);


                        flag = true;
                    }

                }

                if (flag)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Record updated successfully');", true);
                }
                LoadGridData();
                LoadGrid("");
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "CatchMsg", "alert('" + ex.Message.ToString() + "');", true);
        }
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        mPopUpCategory.Show();
    }
    protected void btnClose_Click(object sender, EventArgs e)
    {
        ClearAll();        
        ddlType.SelectedIndex = 0;
        ddlType_SelectedIndexChanged(null, null);
        drpParentCategory.SelectedIndex = 0;
        mPopUpCategory.Hide();
    }

    public void LoadParentCategories()
    {
        try
        {
            if (ddlType.Items.Count > 0)
            {
                drpParentCategory.Items.Clear();

                DataTable dt = new DataTable();
                dt = mController.SelectSkuHierarchy(Constants.SKUCategory, Constants.IntNullValue, Convert.ToInt32(ddlType.SelectedItem.Value), null, null, true, int.Parse(this.Session["CompanyId"].ToString()), Constants.IntNullValue);
                drpParentCategory.Items.Add(new DevExpress.Web.ListEditItem("---Select---", "0"));

                clsWebFormUtil.FillDxComboBoxList(drpParentCategory, dt, 0, 3);

                if (dt.Rows.Count > 0)
                {
                    drpParentCategory.SelectedIndex = 0;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "CatchMsg", "alert('" + ex.Message.ToString() + "');", true);
        }
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
    public void ValidateMediaExtensions(ref string _error_info, string _extension, string allowed_extensions)
    {
        string[] ext_spliter = allowed_extensions.Split('|');
        bool _is_valid = false;
        for (int i = 0; i <= ext_spliter.Length - 1; i++)
        {
            if (ext_spliter[i] == _extension)
            {
                _is_valid = true;
            }
        }
        if (_is_valid == false)
        {
            _error_info = "Only (" + allowed_extensions + ") file extension(s) is allowed. ";
        }
    }
    #region Import Categories
    protected void btnImportClose_Click(object sender, EventArgs e)
    {
        mPOPImport.Hide();
    }
    protected void btnImport_Click(object sender, EventArgs e)
    {
        mPOPImport.Show();
    }
    protected void btnImport_Category(object sender, EventArgs e)
    {
        string strFilePath;
        string strFolder;
        strFolder = Server.MapPath("~/Files/CategoryImport/");
        // Get the name of the file that is posted.
        string strFileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
        if (strFileName != "")
        {
            // Create the directory if it does not exist.
            if (!Directory.Exists(strFolder))
            {
                Directory.CreateDirectory(strFolder);
            }
            // Save the uploaded file to the server.
            strFilePath = strFolder + strFileName;
            //if (File.Exists(strFilePath))
            //{
            //    lblUploadResult.Text = strFileName + " already exists on the server!";
            //}

            //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
            //string filename = Path.GetFileName(Request.Files[i].FileName);  

            var file = FileUpload1.PostedFile;

            FileUpload1.PostedFile.SaveAs(strFilePath);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var package = new ExcelPackage(file.InputStream);

            ExcelWorksheet workSheet = package.Workbook.Worksheets[0];
            var start = workSheet.Dimension.Start.Row;
            var end = workSheet.Dimension.End.Column;

            int totalCols = workSheet.Dimension.End.Column;
            //var range = workSheet.Cells[1, 1, 1, end];

            DataTable categories = _mSkuController.SelectCategoryType();

            for (int row = start; row <= end; row++)
            { // Row by row...
                int categoryType = 0;
                var categoryName = "";
                for (int col = start; col <= end; col++)
                { // ... Cell by cell...
                    var cellValue = workSheet.Cells[row + 1, col].Text; // This got me the actual value I needed.
                    if (!string.IsNullOrEmpty(cellValue))
                    {
                        if (col == 1)
                        {
                            DataRow[] dr = categories.Select("CategoryType='" + cellValue.ToString().Trim()+"'");
                            if (dr.Length > 0)
                            {
                                categoryType = Convert.ToInt32(dr[0]["CategoryTypeID"].ToString());
                            }
                        }
                        else if (col == 2)
                            categoryName = cellValue.ToString();
                    }
                }
                if (categoryType > 0 && !string.IsNullOrEmpty(categoryName))
                {
                    mController.InsertHierarchy(Constants.SKUCategory, categoryType,"", categoryName, null,
                        true, int.Parse(this.Session["CompanyId"].ToString()),0, null, 
                        Constants.IntNullValue, false, false);
                }
                File.Delete(strFilePath);
            }
            ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Record added successfully.');", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Please Upload the excel File');", true);
        }
    }
    protected void btnExport_Category(object sender, EventArgs e)
    {
        var dataTable = new ItemCategory();
        OfficeOpenXml.ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
        using (ExcelPackage p = new ExcelPackage())
        {
            ExcelWorksheet ws = p.Workbook.Worksheets.Add("ItemCategory_List");
            ExcelWorksheet roughSheet = p.Workbook.Worksheets.Add("Dummy_List");
            int index = 1;
            foreach (var item in dataTable.GetType().GetProperties())
            {
                ws.Cells[1, index++].Value = item.Name.Replace('_', ' ').Replace("  ", "_");
            }
            var headerCells = ws.Cells[1, 1, 1, ExcelPackage.MaxColumns];
            var headerFont = headerCells.Style.Font;
            headerFont.Bold = true;
            GenerateCategoryExportPreFilledDropDown(ws, roughSheet, p);
            Byte[] fileBytes = p.GetAsByteArray();
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=ItemCategory.xlsx");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.BinaryWrite(fileBytes);
            Response.End();
        }
    }

    public void GenerateCategoryExportPreFilledDropDown(ExcelWorksheet ws, ExcelWorksheet roughSheet, ExcelPackage p)
    {
        DataTable categories = _mSkuController.SelectCategoryType();

        roughSheet.Cells["A1"].Value = "Categories";
        var headerCells = roughSheet.Cells[1, 1, 1, ExcelPackage.MaxColumns];
        var headerFont = headerCells.Style.Font;
        headerFont.Bold = true;

        string startFrom = "";
        string startTo = "";
        if (categories.Rows.Count > 0)
        {
            var count = 1;
            foreach (DataRow cat in categories.Rows)
            {
                var categoryName = cat["CategoryType"].ToString();
                roughSheet.Cells[count + 1, 1].Value = categoryName;
                count++;
            }
            //specify the range for the dropdown to appear. //Where GetAddress is: GetAddress(int FromRow, int FromColumn, int ToRow, int ToColumn)
            //First sheet catgories into drop down.
            var range = ExcelRange.GetAddress(2, 1, ExcelPackage.MaxRows, 1);
            var categoryListInExcelDropDown = ws.DataValidations.AddListValidation(range);

            //Get range of values in category from dummy list sheet.
            startFrom = roughSheet.Cells[2, 1].ToString();
            startTo = roughSheet.Cells[count, 1].ToString();

            startFrom = "$" + startFrom.Substring(0, 1) + "$" + startFrom.Substring(1);
            startTo = "$" + startTo.Substring(0, 1) + "$" + startTo.Substring(1);

            var roughSheetCategoriesRange = startFrom + ":" + startTo;
            categoryListInExcelDropDown.Formula.ExcelFormula = "Dummy_List!" + roughSheetCategoriesRange.ToString();
        }

        //Hide Dummy Sheet
        roughSheet.Hidden = OfficeOpenXml.eWorkSheetHidden.Hidden;
    }

    public class ItemCategory
    {
        public string Category_Type { get; set; }
        public string CategoryName { get; set; }
    }
    #endregion
}