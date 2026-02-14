using System;
using System.Data;
using System.Web.UI.WebControls;
using CORNCommon.Classes;
using CORNBusinessLayer.Classes;
using System.Web;
using System.Web.UI;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using CORNDatabaseLayer.Classes;

public partial class frmCustomer : System.Web.UI.Page
{
    readonly DataControl dc = new DataControl();
    readonly CustomerDataController cController = new CustomerDataController();
    readonly LoyaltyController lController = new LoyaltyController();
    readonly DistributorController mController = new DistributorController();
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        Response.Cache.SetNoStore();
        Response.AppendHeader("pragma", "no-cache");
        //=========================================================

        try
        {
            txtDOB.Attributes.Add("readonly", "true");
            dtpMembership.Attributes.Add("readonly", "true");
            txtFromDate.Attributes.Add("readonly", "true");
            txtToDate.Attributes.Add("readonly", "true");
            txtChildDOB.Attributes.Add("readonly", "true");

            if (!IsPostBack)
            {
                if(Session["CustomerLocationWise"].ToString() == "1")
                {
                    rowLocation.Visible = true;
                }
                if(Session["CustomerWiseGST"].ToString() == "1")
                {
                    TabPanelInvoiceCalculation.Visible = true;
                }
                Session.Remove("dtGridData");
                btnSave.Attributes.Add("onclick", "return ValidateForm()");
                LoadProfession();
                LoadGridData();
                LoadGrid("");
                LoadDistributor();
                LoadCardType();
                LoadCardInfo();
                LoadCustomerGroup();
                CreateSKUDataTable();
                LoadFamilyGrid();
                LoadStatus();
                LoadCategory();
                CreateAddressDataTable();
                LoadAddressGrid();
                contentBox.Visible = false;
                lookupBox.Visible = true;
                btnClose.Visible = false;
                btnSave.Visible = false;
                btnFamilyAdd.Text = "Add";
                btnAddress.Text = "Add";
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    #region Load
    private void LoadProfession()
    {
        CompanyController CController = new CompanyController();
        try
        {
            DataTable dt = CController.GetProfession(1);
            clsWebFormUtil.FillDxComboBoxList(ddlProfession, dt, 0, 1, true);

            if (dt.Rows.Count > 0)
            {
                ddlProfession.SelectedIndex = 0;
            }
        }
        catch (Exception)
        {

            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Please assign Location');", true);
        }
    }
    private void LoadStatus()
    {
        CompanyController CController = new CompanyController();
        try
        {
            DataTable dt = cController.GetCustomerDropDown(1);
            clsWebFormUtil.FillDxComboBoxList(drpStatus, dt, "CUSTOMER_STATUS_ID", "STATUS_DESC", true);

            if (dt.Rows.Count > 0)
            {
                drpStatus.SelectedIndex = 0;
            }
        }
        catch (Exception)
        {

            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Please assign Location');", true);
        }
    }
    private void LoadCategory()
    {
        CompanyController CController = new CompanyController();
        try
        {
            DataTable dt = cController.GetCustomerDropDown(2);
            clsWebFormUtil.FillDxComboBoxList(drpCategory, dt, 0, 1, true);

            if (dt.Rows.Count > 0)
            {
                drpCategory.SelectedIndex = 0;
            }
        }
        catch (Exception)
        {

            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Please assign Location');", true);
        }
    }

    private void LoadDistributor()
    {
        DistributorController mController = new DistributorController();
        try
        {
            DataTable dtDistributor = mController.SelectDistributor(Constants.IntNullValue, Constants.IntNullValue, int.Parse(Session["CompanyId"].ToString()));

            clsWebFormUtil.FillDxComboBoxList(ddDistributorId, dtDistributor, "DISTRIBUTOR_ID", "DISTRIBUTOR_NAME", true);


            if (dtDistributor.Rows.Count > 0)
            {
                ddDistributorId.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
            ExceptionPublisher.PublishException(ex);
            ScriptManager.RegisterStartupScript(this, typeof(Page), "CatchMsg", "alert('" + ex.Message.ToString() + "');", true);
        }
    }
    private void LoadGridData()
    {
        int TypeID = 1;
        if(TabPanelInvoiceCalculation.Visible)
        {
            TypeID = 2;
        }
        DataTable dt = new DataTable();
        dt = cController.SelectAllCustomer(TypeID);
        foreach (DataRow item in dt.Rows)
        {
            item["DISTRIBUTOR_ID"] = "1";
        }
        Session.Add("dtGridData", dt);
    }
    private void LoadGrid(string pType)
    {
        GrdCustomer.DataSource = null;
        GrdCustomer.DataBind();
        DataTable dt = (DataTable)Session["dtGridData"];
        if (pType == "")
        {
            if (txtSearch.Text != "" || txtSearch.Text != string.Empty)
            {
                dt.DefaultView.RowFilter = "CUSTOMER_NAME LIKE '%" + txtSearch.Text + "%' OR CUSTOMER_CODE LIKE '%" + txtSearch.Text + "%' OR IS_ACTIVE1 LIKE '" + txtSearch.Text + "%'";
            }
            GrdCustomer.DataSource = dt;
            GrdCustomer.DataBind();
        }
        else
        {
            if (txtSearch.Text != "" || txtSearch.Text != string.Empty)
            {
                dt.DefaultView.RowFilter = "CUSTOMER_NAME LIKE '%" + txtSearch.Text + "%' OR CUSTOMER_CODE LIKE '%" + txtSearch.Text + "%' OR IS_ACTIVE1 LIKE '" + txtSearch.Text + "%' OR CONTACT_NUMBER LIKE '%" + txtSearch.Text + "%' OR ADDRESS LIKE '%" + txtSearch.Text + "%'";
            }
            else
            {
                dt.DefaultView.RowFilter = null;
            }
            if (dt.Rows.Count > 0)
            {
                GrdCustomer.PageIndex = 0;
            }
            GrdCustomer.DataSource = dt;
            GrdCustomer.DataBind();
        }

        if (dt.Rows.Count > 0)
        {
            try
            {
                bool showCustomerInfo = Convert.ToBoolean(dt.Rows[0]["ShowCustomerInfoOnBill"].ToString());
                if (showCustomerInfo == true)
                {
                    GrdCustomer.HeaderRow.Cells[2].Text = "Member ID";
                }
                else
                {
                    GrdCustomer.HeaderRow.Cells[2].Text = "Card ID";
                }
            }
            catch
            {
                GrdCustomer.HeaderRow.Cells[2].Text = "Card ID";
            }
        }
    }

    private void LoadCardType()
    {
        try
        {
            drpCardType.Items.Clear();
            if (ddDistributorId.Items.Count > 0 && ddDistributorId.SelectedIndex != -1)
            {
                DataTable dt = lController.SelectLoyaltyCard(Constants.IntNullValue, Convert.ToInt32(ddDistributorId.Value), Constants.IntNullValue, 3);
                if (dt.Rows.Count > 0)
                {
                    dt = dt.AsEnumerable()
                                    .Where(r => r.Field<int>("ID") != 3)
                                    .CopyToDataTable();
                    drpCardType.Items.Add(new DevExpress.Web.ListEditItem("---Select---","0"));
                    clsWebFormUtil.FillDxComboBoxList(drpCardType, dt, "ID", "CARD_NAME");
                    if (dt.Rows.Count > 0)
                    {
                        drpCardType.SelectedIndex = 0;
                    }
                }
            }
        }
        catch (Exception)
        {

            throw;
        }
    }

    private void LoadCardInfo()
    {
        try
        {

            txtDiscount.Text = "";
            txtPurchasing.Text = "";
            txtPoints.Text = "";
            txtAmountLimit.Text = "";

            if (ddDistributorId.Items.Count > 0 && drpCardType.Items.Count > 0)
            {
                DataTable dtCard = lController.SelectLoyaltyCard(Convert.ToInt32(drpCardType.Value), Convert.ToInt32(ddDistributorId.Value), Constants.IntNullValue, 2);
                if (dtCard.Rows.Count > 0)
                {
                    txtDiscount.Text = dtCard.Rows[0]["DISCOUNT"].ToString();
                    txtPurchasing.Text = dtCard.Rows[0]["PURCHASING"].ToString();
                    txtPoints.Text = dtCard.Rows[0]["POINTS"].ToString();
                    txtAmountLimit.Text = dtCard.Rows[0]["AMOUNT_LIMIT"].ToString();
                    hfLoyaltyCardType.Value = dtCard.Rows[0]["CARD_TYPE_ID"].ToString();
                }
                ShowHideCardInfo();
            }
        }
        catch (Exception ex)
        {
            ExceptionPublisher.PublishException(ex);
            ScriptManager.RegisterStartupScript(this, typeof(Page), "CatchMsg", "alert('" + ex.Message.ToString() + "');", true);
        }
    }

    private void LoadCustomerGroup()
    {
        DataTable dtGroup = cController.GetCustomerGroup(Constants.IntNullValue);
        clsWebFormUtil.FillDxComboBoxList(ddlCustomerGroup, dtGroup, 0, 1);
        ddlCustomerGroup.SelectedIndex = 0;
    }

    #endregion
    private void ShowHideCardInfo()
    {
        if (hfLoyaltyCardType.Value.ToString() == "0")
        {
            txtPurchasing.Visible = false;
            txtDiscount.Visible = false;
            txtCardNo.Visible = false;
            ltrlCardNo.Visible = false;
            ltrlPoints.Visible = false;
            txtPoints.Visible = false;
            ltrlCard.Visible = false;
        }
        else if (hfLoyaltyCardType.Value.ToString() == "2")
        {
            txtCardNo.Visible = true;
            ltrlCardNo.Visible = true;
            txtPurchasing.Visible = true;
            txtDiscount.Visible = false;

            ltrlPoints.Visible = true;
            txtPoints.Visible = true;
            ltrlCard.Visible = true;
            ltrlCard.Text = "<label><span class='fa fa-caret-right rgt_cart'></span>Purchasing</label>";
        }
        else if (hfLoyaltyCardType.Value.ToString() == "1")
        {
            txtPurchasing.Visible = false;
            txtCardNo.Visible = true;
            ltrlCardNo.Visible = true;
            txtDiscount.Visible = true;
            ltrlPoints.Visible = false;
            txtPoints.Visible = false;
            ltrlCard.Visible = true;
            ltrlCard.Text = "<label><span class='fa fa-caret-right rgt_cart'></span>Discount</label>";
        }
    }

    private bool ValidateCard(string btn, int recordId)
    {
        UserController _mUController = new UserController();
        try
        {
            if (hfLoyaltyCardType.Value.ToString() != "0")
            {
                if (txtCardNo.Text != "")
                {
                    if (btn == "Save")
                    {
                        if (_mUController.IsExist(txtCardNo.Text, Convert.ToInt32(ddDistributorId.SelectedItem.Value), int.Parse(Session["UserId"].ToString()), 1))
                        {

                            return false;
                        }
                    }
                    else
                    {
                        if (_mUController.IsExist(txtCardNo.Text, Convert.ToInt32(ddDistributorId.SelectedItem.Value), recordId, 3))
                        {

                            return false;
                        }
                    }
                }
            }
            return true;

        }
        catch (Exception ex)
        {

            ScriptManager.RegisterStartupScript(this, typeof(Page), "CatchMsg", "alert('" + ex.Message.ToString() + "')", true);
            throw;
        }
    }

    #region Index/change

    protected void ddDistributorId_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCardType();
        LoadCardInfo();
    }

    protected void drpCardType_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCardInfo();
    }
    protected void drpStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpStatus.Items.Count > 0)
        {
            if (drpStatus.SelectedItem.Text.Trim() == "Dormant" || drpStatus.SelectedItem.Text.Trim() == "Absentee")
                dormant_row.Visible = true;
            else
                dormant_row.Visible = false;
        }
    }

    #endregion

    #region Click

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearAll();
    }
    protected void btnImport_Click(object sender, EventArgs e)
    {
        try
        {
            Page.MaintainScrollPositionOnPostBack = true;
            HttpFileCollection uploadedFiles = Request.Files;
            if (FupVendor.HasFile)
            {
                HttpPostedFile userPostedFile = uploadedFiles[0];
                if (userPostedFile.ContentLength > 0)
                {
                    string fname;

                    // Checking for Internet Explorer  
                    if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                    {
                        string[] testfiles = userPostedFile.FileName.Split(new char[] { '\\' });
                        fname = testfiles[testfiles.Length - 1];
                    }
                    else
                    {
                        fname = userPostedFile.FileName;
                    }

                    // Get the complete folder path and store the file inside it.  
                    fname = Path.Combine(Server.MapPath("~/images/"), fname);
                    userPostedFile.SaveAs(fname);


                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                    var package = new ExcelPackage(userPostedFile.InputStream);

                    ExcelWorksheet workSheet = package.Workbook.Worksheets[0];
                    var start = 4;
                    var end = workSheet.Dimension.End.Column;

                    int totalRows = workSheet.Dimension.End.Row;
                    //var range = workSheet.Cells[1, 1, 1, end];


                    try
                    {
                        CompanyController CController = new CompanyController();
                        DataTable dt = CController.GetProfession(1);
                        DataTable statusDt = cController.GetCustomerDropDown(1);
                        DataTable categoryDt = cController.GetCustomerDropDown(2);

                        for (int row = start; row <= totalRows; row++)
                        { // Row by row...

                            spInsertCUSTOMER mDistRoute = new spInsertCUSTOMER();
                            mDistRoute.DISTRIBUTOR_ID = Convert.ToInt32(Session["DISTRIBUTOR_ID"]);
                            mDistRoute.IS_GST_REGISTERED = false;
                            mDistRoute.GST_NUMBER = null;
                            mDistRoute.IS_ACTIVE = true;
                            mDistRoute.CONTACT_PERSON = "na";
                            mDistRoute.TIME_STAMP = System.DateTime.Now;
                            mDistRoute.LASTUPDATE_DATE = System.DateTime.Now;
                            mDistRoute.TOWN_ID = 0;
                            mDistRoute.AREA_ID = 0;
                            mDistRoute.ROUTE_ID = 0;
                            mDistRoute.CHANNEL_TYPE_ID = 0;
                            mDistRoute.BUSINESS_TYPE_ID = 0;
                            mDistRoute.PROMOTION_CLASS = 0;
                            mDistRoute.BARCODE = null;
                            mDistRoute.FROM_DATE = Constants.DateNullValue;
                            mDistRoute.TO_DATE = Constants.DateNullValue;
                            
                            for (int col = 1; col <= end; col++)
                            { // ... Cell by cell...
                                var cellValue = workSheet.Cells[row, col].Text;
                                if (!string.IsNullOrEmpty(cellValue))
                                {
                                    cellValue = cellValue.Trim();

                                    if (col == 1)
                                        mDistRoute.CUSTOMER_CODE = cellValue;
                                    else if (col == 2)
                                        mDistRoute.CUSTOMER_NAME = cellValue;
                                    else if (col == 3)
                                        mDistRoute.Membership_No = cellValue;
                                    else if (col == 4)
                                        mDistRoute.EMAIL_ADDRESS = cellValue;
                                    else if (col == 5)
                                        mDistRoute.CONTACT_NUMBER = cellValue;
                                    else if (col == 6)
                                        mDistRoute.CONTACT2 = cellValue;
                                    else if (col == 7)
                                        mDistRoute.ADDRESS = cellValue;
                                    else if (col == 8)
                                        mDistRoute.CNIC = cellValue;
                                    else if (col == 9)
                                        mDistRoute.OPENING_AMOUNT = Convert.ToDecimal(cellValue);
                                    else if (col == 10)
                                        mDistRoute.NATURE = cellValue;
                                    else if (col == 11)
                                        mDistRoute.REGDATE = Convert.ToDateTime(cellValue);
                                    else if (col == 12)
                                        mDistRoute.Membership_Date = !string.IsNullOrEmpty(cellValue) && cellValue != "na" ?
                                           Convert.ToDateTime(cellValue) : Constants.DateNullValue;
                                    else if (col == 13)
                                        mDistRoute.Spouse_Name = !string.IsNullOrEmpty(cellValue) && cellValue != "na" ?
                                            cellValue : null;

                                    else if (col == 14)
                                    {
                                        if (!string.IsNullOrEmpty(cellValue) && cellValue != "na" && dt.Rows.Count > 0)
                                        {
                                            var result = dt.Select("ProfessionName LIKE '%" + cellValue + "%'");
                                            if (result != null && result.Length > 0 && result[0] != null)
                                                mDistRoute.Profession = Convert.ToInt32(result[0]["ProfessionID"].ToString());
                                        }
                                        else
                                        {
                                            mDistRoute.Profession = Constants.IntNullValue;
                                        }
                                    }
                                    else if (col == 15)
                                        mDistRoute.SALES_PER = Convert.ToDecimal(cellValue);

                                    else if (col == 16)
                                    {
                                        if (!string.IsNullOrEmpty(cellValue) && cellValue != "na" && categoryDt.Rows.Count > 0)
                                        {
                                            var result = categoryDt.Select("CATEGORY_NAME LIKE '%" + cellValue + "%'");
                                            if (result != null && result.Length > 0 && result[0] != null)
                                                mDistRoute.CUSTOMER_CATEGORY_ID = Convert.ToInt32(result[0]["CUSTOMER_CATEGORY_ID"].ToString());
                                        }
                                        else
                                        {
                                            mDistRoute.CUSTOMER_CATEGORY_ID = Constants.IntNullValue;
                                        }
                                    }
                                    else if (col == 17 && !string.IsNullOrEmpty(cellValue) && cellValue != "na")
                                        mDistRoute.IsGolfer = cellValue == "1" ? true : false;

                                    else if (col == 18)
                                    {
                                        if (!string.IsNullOrEmpty(cellValue) && cellValue != "na" && statusDt.Rows.Count > 0)
                                        {
                                            var result = statusDt.Select("STATUS_DESC LIKE '%" + cellValue + "%'");
                                            if (result != null && result.Length > 0 && result[0] != null)
                                                mDistRoute.CUSTOMER_STATUS_ID = Convert.ToInt32(result[0]["CUSTOMER_STATUS_ID"].ToString());
                                        }
                                        else
                                        {
                                            mDistRoute.CUSTOMER_STATUS_ID = Constants.IntNullValue;
                                        }
                                    }
                                    else if (col == 19 && !string.IsNullOrEmpty(cellValue) && cellValue != "na")
                                    {
                                        var result = statusDt.Select("STATUS_DESC LIKE '%" + workSheet.Cells[row, col-1].Text.Trim() + "%'");
                                        if (result != null && result.Length > 0 && result[0] != null)
                                        {
                                            if (result[0]["STATUS_DESC"].ToString().Contains("Dormant") ||
                                                result[0]["STATUS_DESC"].ToString().Contains("Absentee"))
                                            {
                                                try
                                                {
                                                    mDistRoute.FROM_DATE = Convert.ToDateTime(cellValue);
                                                }
                                                catch
                                                {
                                                    mDistRoute.FROM_DATE = Constants.DateNullValue;
                                                }
                                            }
                                        }
                                    }
                                    else if (col == 20 && !string.IsNullOrEmpty(cellValue) && cellValue != "na")
                                    {
                                        var result = statusDt.Select("STATUS_DESC LIKE '%" + workSheet.Cells[row, col - 2].Text.Trim() + "%'");
                                        if (result != null && result.Length > 0 && result[0] != null)
                                        {
                                            if (result[0]["STATUS_DESC"].ToString().Contains("Dormant") ||
                                                result[0]["STATUS_DESC"].ToString().Contains("Absentee"))
                                            {
                                                try
                                                {
                                                    mDistRoute.TO_DATE = Convert.ToDateTime(cellValue);
                                                }
                                                catch
                                                {
                                                    mDistRoute.TO_DATE = Constants.DateNullValue;
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            cController.ImportCustomer(mDistRoute);

                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Customers imported successfully.');", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('No file selected.');", true);
            }

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "CatchMsg", "alert('" + ex.Message.ToString() + "')", true);
            throw;
        }
    }
    protected void btnExportOpeningTemplate_Click(object sender, EventArgs e)
    {
        OfficeOpenXml.ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

        using (ExcelPackage p = new ExcelPackage())
        {
            ExcelWorksheet ws = p.Workbook.Worksheets.Add("CustomerList");

            var headerCells = ws.Cells[1, 1, 1, ExcelPackage.MaxColumns];
            var headerFont = headerCells.Style.Font;
            headerFont.Color.SetColor(ExcelIndexedColor.Indexed2);
            headerCells.Merge = true;

            GenerateItemPreFilled(ws, p);

            Byte[] fileBytes = p.GetAsByteArray();

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=CustomerList.xlsx");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.BinaryWrite(fileBytes);
            Response.End();
        }
    }
    public void GenerateItemPreFilled(ExcelWorksheet ws, ExcelPackage p)
    {
        ws.Cells[1, 1].Value = "Note: No column should be empty if you want to set blank then set its default " +
            "value described in Second row";
        ws.Cells[1, 1].AutoFitColumns(2, 3);

        ws.Cells[2, 1].Value = "na";
        var headerCells = ws.Cells[2, 1, 2, 20];
        headerCells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        var headerFont = headerCells.Style.Fill;
        headerCells.Style.Fill.PatternType = ExcelFillStyle.Solid;
        headerFont.BackgroundColor.SetColor(ExcelIndexedColor.Indexed51);
        headerCells.AutoFitColumns();
        headerCells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        ws.Cells[2, 2].Value = "na";
        ws.Cells[2, 3].Value = "na";
        ws.Cells[2, 4].Value = "na";
        ws.Cells[2, 5].Value = "na";
        ws.Cells[2, 6].Value = "na";
        ws.Cells[2, 7].Value = "na";
        ws.Cells[2, 8].Value = "na";
        ws.Cells[2, 9].Value = "0.00";
        ws.Cells[2, 10].Value = "na";
        ws.Cells[2, 11].Value = "12/31/1999";
        ws.Cells[2, 12].Value = "na";
        ws.Cells[2, 13].Value = "na";
        ws.Cells[2, 14].Value = "na";
        ws.Cells[2, 15].Value = "0.00";
        ws.Cells[2, 16].Value = "na";
        ws.Cells[2, 17].Value = "false";
        ws.Cells[2, 18].Value = "na";
        ws.Cells[2, 19].Value = "na";
        ws.Cells[2, 20].Value = "na";

        var headerCells1 = ws.Cells[3, 1, 3, 20];
        headerCells1.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        var headerFont1 = headerCells1.Style.Fill;
        headerCells1.Style.Fill.PatternType = ExcelFillStyle.Solid;
        headerFont1.BackgroundColor.SetColor(ExcelIndexedColor.Indexed24);
        headerCells1.AutoFitColumns();
        headerCells1.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        ws.Cells[3, 1].Value = "Card Id";
        ws.Cells[3, 2].Value = "Name";
        ws.Cells[3, 3].Value = "Membership No";
        ws.Cells[3, 4].Value = "Email";
        ws.Cells[3, 5].Value = "Primary Contact";
        ws.Cells[3, 6].Value = "Secondary Contact";
        ws.Cells[3, 7].Value = "Address";
        ws.Cells[3, 8].Value = "CNIC";
        ws.Cells[3, 9].Value = "Opening Amount";
        ws.Cells[3, 10].Value = "Nature";
        ws.Cells[3, 11].Value = "Date Of Birth";
        ws.Cells[3, 12].Value = "Date Of Membership";
        ws.Cells[3, 13].Value = "Spouse Name";
        ws.Cells[3, 14].Value = "Profession";
        ws.Cells[3, 15].Value = "Sales %";
        ws.Cells[3, 16].Value = "Category";
        ws.Cells[3, 17].Value = "Golfer/Non Golfer";
        ws.Cells[3, 18].Value = "Status";
        ws.Cells[3, 19].Value = "From Date";
        ws.Cells[3, 20].Value = "To Date";
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                if (hfLoyaltyCardType.Value.ToString() == "1")
                {
                    if (txtCardNo.Text == "")
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Card No is required.');", true);
                        return;
                    }
                    if (txtDiscount.Text == "")
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Discount is required.');", true);
                        return;
                    }
                }
                else if (hfLoyaltyCardType.Value.ToString() == "2")
                {
                    if (txtCardNo.Text == "")
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Card No is required.');", true);
                        return;
                    }
                    if (txtPurchasing.Text == "")
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Purchase Amount is required.');", true);
                        return;
                    }
                    if (txtPoints.Text == "")
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Points is required.');", true);
                        return;
                    }
                }

                if (drpStatus.SelectedItem != null && ((drpStatus.SelectedItem.Text.Trim() == "Dormant" || drpStatus.SelectedItem.Text.Trim() == "Absentee")
                    && (string.IsNullOrEmpty(txtFromDate.Text) || string.IsNullOrEmpty(txtToDate.Text))))
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('From & To Date is required.');", true);
                    return;
                }

                lblErrorMsg.Text = "";

                bool flag = true;
                DateTime memberShipDate = Constants.DateNullValue;
                if (!string.IsNullOrEmpty(dtpMembership.Text))
                {
                    memberShipDate = DateTime.Parse(dtpMembership.Text);
                }
                int ProfessionID = 0;
                if (ddlProfession.Items.Count > 0)
                {
                    ProfessionID = Convert.ToInt32(ddlProfession.Value);
                }
                int GroupID = Convert.ToInt32(ddlCustomerGroup.Value);
                string customerId = "";
                bool update = false;
                DateTime from = Constants.DateNullValue;
                DateTime to = Constants.DateNullValue;
                if (!string.IsNullOrEmpty(txtFromDate.Text))
                    from = Convert.ToDateTime(txtFromDate.Text);

                if (!string.IsNullOrEmpty(txtToDate.Text))
                    to = Convert.ToDateTime(txtToDate.Text);
                int distributorid = 0;
                if (Session["CustomerLocationWise"].ToString() == "1")
                {
                    distributorid = Convert.ToInt32(ddDistributorId.SelectedItem.Value);
                }
                else
                {
                    distributorid = Convert.ToInt32(Session["DISTRIBUTOR_ID"]);
                }
                switch (btnSave.Text)
                {
                    case "Save":

                        if (ValidateCard("Save", 0))
                        {
                            
                                var dt = CustomerDataController.InsertCustomer(distributorid,txtCardNo.Text, txtCNIC.Text, txtDOB.Text, txtContact.Text, txtEmail.Text, txtName.Text,
                                txtAddress.Text, null, Convert.ToDecimal(dc.chkNull_0(txtOpeningAmount.Text)),
                                txtNature.Text, txtContact2.Text, Convert.ToInt32(hfLoyaltyCardType.Value),
                                Convert.ToDecimal(dc.chkNull_0(txtDiscount.Text)),
                                Convert.ToDecimal(dc.chkNull_0(txtPurchasing.Text)),
                                Convert.ToDecimal(dc.chkNull_0(txtPoints.Text)),
                                Convert.ToDecimal(dc.chkNull_0(txtAmountLimit.Text)), 0, 0,
                                Convert.ToDecimal(dc.chkNull_0(txtSalesPer.Text)),
                                memberShipDate, txtSpouse.Text, ProfessionID, txtMembershipNo.Text,
                                int.Parse(drpCategory.Value.ToString()), int.Parse(drpStatus.Value.ToString()),
                                Convert.ToBoolean(rdoGolfer.SelectedItem.Value), from, to, GroupID,Convert.ToInt32(drpCardType.Value));

                            customerId = dt.Rows[0]["CUSTOMER_ID"].ToString();
                            if (customerId == "0")
                            {
                                ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Primary Contact Number aleady exist!.');", true);
                                return;
                            }
                            else
                            {
                                if (TabPanelInvoiceCalculation.Visible)
                                {
                                    cController.InsertCustomerInvoiceCalculation(Convert.ToInt64(customerId), Convert.ToDecimal(dc.chkNull_0(txtGST.Text)), Convert.ToInt32(rblGST.SelectedItem.Value), Convert.ToDecimal(dc.chkNull_0(txtDiscountPOS.Text)), Convert.ToInt32(rblDiscount.SelectedItem.Value), Convert.ToDecimal(dc.chkNull_0(txtServiceCharges.Text)), Convert.ToInt32(rblServiceCharges.SelectedItem.Value));                                    
                                }
                                ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Record added successfully.');", true);
                                flag = true;
                            }
                        }
                        else
                        {
                            flag = false;
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Card No already exist.');", true);
                        }
                        break;
                    case "Update":
                        if (ValidateCard("Update", Convert.ToInt32(hfCustomerID.Value)))
                        {
                            customerId = hfCustomerID.Value;
                            update = true;
                            bool status = true;
                            if (hfStatus.Value != "Active")
                            {
                                status = false;
                            }
                            DateTime dtRegDate = Constants.DateNullValue;
                            if (txtDOB.Text.Length > 0)
                            {
                                Convert.ToDateTime(txtDOB.Text);
                            }
                            cController.UpdateCustomer(Convert.ToInt64(hfCustomerID.Value), status, Constants.IntNullValue,
                                txtContact.Text, txtEmail.Text, txtCardNo.Text, txtName.Text, txtAddress.Text, dtRegDate,
                                txtCNIC.Text, null, txtNature.Text, Convert.ToDecimal(dc.chkNull_0(txtOpeningAmount.Text)),
                                txtContact2.Text, Convert.ToInt32(hfLoyaltyCardType.Value),
                                Convert.ToDecimal(dc.chkNull_0(txtDiscount.Text)),
                                Convert.ToDecimal(dc.chkNull_0(txtPurchasing.Text)),
                                Convert.ToDecimal(dc.chkNull_0(txtPoints.Text)),
                                Convert.ToDecimal(dc.chkNull_0(txtAmountLimit.Text)),
                                Convert.ToDecimal(dc.chkNull_0(txtSalesPer.Text)),
                                memberShipDate, txtSpouse.Text, ProfessionID, txtMembershipNo.Text,
                                drpCategory.Value != null ? int.Parse(drpCategory.Value.ToString()) : Constants.IntNullValue,
                                drpStatus.Value != null ? int.Parse(drpStatus.Value.ToString()) : Constants.IntNullValue,
                                Convert.ToBoolean(rdoGolfer.SelectedItem.Value), from, to, GroupID,Convert.ToInt32(drpCardType.Value));
                            if (TabPanelInvoiceCalculation.Visible)
                            {
                                cController.InsertCustomerInvoiceCalculation(Convert.ToInt64(hfCustomerID.Value), Convert.ToDecimal(dc.chkNull_0(txtGST.Text)), Convert.ToInt32(rblGST.SelectedItem.Value), Convert.ToDecimal(dc.chkNull_0(txtDiscountPOS.Text)), Convert.ToInt32(rblDiscount.SelectedItem.Value), Convert.ToDecimal(dc.chkNull_0(txtServiceCharges.Text)), Convert.ToInt32(rblServiceCharges.SelectedItem.Value));                            
                            }
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Record updated successfully.');", true);
                            flag = true;
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Card No already exist.');", true);
                            flag = false;
                        }
                        break;
                }
                if (flag)
                {
                    DataTable dtSKU = (DataTable)Session["dtSKU"];
                    if (dtSKU.Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(customerId))
                        {
                            if (update == true)
                                cController.DeleteCUSTOMER_FAMILY(long.Parse(customerId));

                            foreach (DataRow item in dtSKU.Rows)
                            {
                                cController.InsertFamilyDetails(long.Parse(customerId),
                                    item["CHILD_NAME"].ToString(), Convert.ToDateTime(item["CHILD_DOB"].ToString()),
                                    item["CHILD_GENDER"].ToString()
                                    );
                            }
                            txtChildDOB.Text = "";
                            txtChildName.Text = "";                            
                            CreateSKUDataTable();
                            LoadFamilyGrid();
                        }
                    }

                    //Address
                    DataTable dtAddress = (DataTable)Session["dtAddress"];
                    if (dtAddress.Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(customerId))
                        {
                            if (update == true)
                                cController.DeleteCUSTOMER_Address(long.Parse(customerId));

                            foreach (DataRow item in dtAddress.Rows)
                            {
                                cController.InsertAddressDetails(long.Parse(customerId),
                                    item["CUSTOMER_ADDRESS"].ToString());
                            }
                            txtAddress1.Text = "";
                            CreateAddressDataTable();
                            LoadAddressGrid();
                        }
                    }

                    LoadGridData();
                    ClearAll();
                    LoadGrid("");
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "CatchMsg", "alert('" + ex.Message.ToString() + "');", true);
        }
    }

    protected void btnFilter_Click(object sender, EventArgs e)
    {
        LoadGrid("filter");
    }
    protected void btnClose_Click(object sender, EventArgs e)
    {
        ClearAll();
        contentBox.Visible = false;
        lookupBox.Visible = true;
        searchBox.Visible = true;
        searchBtn.Visible = true;
        btnActive.Visible = true;
        btnClose.Visible = false;
        btnSave.Visible = false;
        btnAdd.Visible = true;
    }
    protected void btnActive_Click(object sender, EventArgs e)
    {
        bool check = false;
        try
        {
            foreach (GridViewRow dr2 in GrdCustomer.Rows)
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
            UserController UserCntrl = new UserController();

            bool flag = false;
            foreach (GridViewRow dr in GrdCustomer.Rows)
            {
                var chRelized = (CheckBox)dr.Cells[0].FindControl("ChbIsAssigned");


                if (chRelized.Checked)
                {
                    if (Convert.ToString(dr.Cells[10].Text) == "Active")
                    {
                        UserCntrl.ActiveInactive(false, Convert.ToInt32(dr.Cells[1].Text), Convert.ToInt32(Session["UserID"]), 4);

                        flag = true;
                    }
                    else
                    {
                        UserCntrl.ActiveInactive(true, Convert.ToInt32(dr.Cells[1].Text), Convert.ToInt32(Session["UserID"]), 4);
                        flag = true;
                    }
                }
                if (flag)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Record updated successfully');", true);
                }
                LoadGridData();
                this.LoadGrid("");
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "CatchMsg", "alert('" + ex.Message.ToString() + "');", true);
        }
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        ddDistributorId.Enabled = true;
        txtName.Enabled = true;
        txtCNIC.Enabled = true;
        txtOpeningAmount.Enabled = true;
        txtDOB.Enabled = true;
        dtpMembership.Enabled = true;
        drpCardType.Enabled = true;
        btnSave.Text = "Save";

        contentBox.Visible = true;
        lookupBox.Visible = false;
        searchBox.Visible = false;
        searchBtn.Visible = false;
        btnActive.Visible = false;
        btnClose.Visible = true;
        btnSave.Visible = true;
        btnAdd.Visible = false;
    }
    #endregion

    private void ClearAll()
    {
        txtGST.Text = "";
        txtDiscountPOS.Text = "";
        txtServiceCharges.Text = "";
        txtCode.Text = "";
        txtName.Text = "";
        txtContact.Text = "";
        txtContact2.Text = "";
        txtAddress.Text = "";
        txtEmail.Text = "";
        txtCNIC.Text = "";
        txtSalesPer.Text = "";
        txtOpeningAmount.Text = "";
        txtNature.Text = "";
        txtDOB.Text = "";
        dtpMembership.Text = "";
        txtMembershipNo.Text = "";
        txtSpouse.Text = "";
        lblErrorMsg.Text = "";
        btnSave.Text = "Save";
        txtCardNo.Text = "";
        LoadCardType();
        LoadCardInfo();

        txtCardNo.Enabled = true;
        txtAmountLimit.Enabled = true;
        drpCardType.Enabled = true;
        txtDiscount.Enabled = true;
    }

    #region Grid Operations

    protected void GrdCustomer_RowEditing(object sender, GridViewEditEventArgs e)
    {
        UserController _mUController = new UserController();
        try
        {
            ddDistributorId.Enabled = true;
            txtName.Enabled = true;
            txtCNIC.Enabled = true;
            txtOpeningAmount.Enabled = true;
            txtDOB.Enabled = true;
            dtpMembership.Enabled = true;
            drpCardType.Enabled = true;
            if (GrdCustomer.Rows[e.NewEditIndex].Cells[21].Text == "1")
            {
                ddDistributorId.Enabled = false;
                txtName.Enabled = false;
                txtCNIC.Enabled = false;
                txtOpeningAmount.Enabled = false;
                txtDOB.Enabled = false;
                dtpMembership.Enabled = false;
                drpCardType.Enabled = false;
            }
            hfCustomerID.Value = GrdCustomer.Rows[e.NewEditIndex].Cells[1].Text;
            txtCode.Text = GrdCustomer.Rows[e.NewEditIndex].Cells[2].Text;
            txtName.Text = GrdCustomer.Rows[e.NewEditIndex].Cells[3].Text.Replace("&nbsp;", "");
            txtContact.Text = GrdCustomer.Rows[e.NewEditIndex].Cells[4].Text.Replace("&nbsp;", "");
            txtContact2.Text = GrdCustomer.Rows[e.NewEditIndex].Cells[7].Text.Replace("&nbsp;", "");
            txtEmail.Text = GrdCustomer.Rows[e.NewEditIndex].Cells[5].Text.Replace("&nbsp;", "");
            txtAddress.Text = GrdCustomer.Rows[e.NewEditIndex].Cells[6].Text.Replace("&nbsp;", "");

            string dob = GrdCustomer.Rows[e.NewEditIndex].Cells[8].Text.Replace("&nbsp;", "");
            if (!string.IsNullOrEmpty(dob))
            {
                dob = Convert.ToDateTime(dob).ToString("dd-MMM-yyyy");
            }

            txtDOB.Text = dob;
            txtOpeningAmount.Text = GrdCustomer.Rows[e.NewEditIndex].Cells[12].Text.Replace("&nbsp;", "");
            hfStatus.Value = GrdCustomer.Rows[e.NewEditIndex].Cells[10].Text;
            txtNature.Text = GrdCustomer.Rows[e.NewEditIndex].Cells[11].Text.Replace("&nbsp;", "");
            txtCNIC.Text = GrdCustomer.Rows[e.NewEditIndex].Cells[13].Text.Replace("&nbsp;", "");
            txtSpouse.Text = GrdCustomer.Rows[e.NewEditIndex].Cells[24].Text.Replace("&nbsp;", "");
            dtpMembership.Text = GrdCustomer.Rows[e.NewEditIndex].Cells[23].Text.Replace("&nbsp;", "");
            ddlProfession.Value = GrdCustomer.Rows[e.NewEditIndex].Cells[25].Text.Replace("&nbsp;", "");
            txtMembershipNo.Text = GrdCustomer.Rows[e.NewEditIndex].Cells[26].Text.Replace("&nbsp;", "");
            drpCategory.Value = GrdCustomer.Rows[e.NewEditIndex].Cells[27].Text.Replace("&nbsp;", "");
            drpStatus.Value = GrdCustomer.Rows[e.NewEditIndex].Cells[28].Text.Replace("&nbsp;", "");
            var isGolfer = GrdCustomer.Rows[e.NewEditIndex].Cells[29].Text.Replace("&nbsp;", "");

            if (!string.IsNullOrEmpty(isGolfer))
            {
                rdoGolfer.Value = Convert.ToBoolean(isGolfer) == true ? "True" : "False"; 
            }
            else
            {
                rdoGolfer.Value = "False";
            }
            

            dormant_row.Visible = false; 
            if (drpStatus.SelectedItem != null && (drpStatus.SelectedItem.Text.Trim() == "Dormant" 
                || drpStatus.SelectedItem.Text.Trim() == "Absentee"))
            {
                dormant_row.Visible = true;
                txtFromDate.Text = GrdCustomer.Rows[e.NewEditIndex].Cells[30].Text.Replace("&nbsp;", "");
                txtToDate.Text = GrdCustomer.Rows[e.NewEditIndex].Cells[31].Text.Replace("&nbsp;", "");
            }

            try
            {
                ddDistributorId.Value = GrdCustomer.Rows[e.NewEditIndex].Cells[20].Text;

            }
            catch (Exception ex)
            {
                ExceptionPublisher.PublishException(ex);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "CatchMsg", "alert('Assigned location is not exist');", true);
            }
            LoadCardType();
            hfLoyaltyCardType.Value = GrdCustomer.Rows[e.NewEditIndex].Cells[19].Text.Replace("&nbsp;", "0");
            drpCardType.Value = GrdCustomer.Rows[e.NewEditIndex].Cells[39].Text.Replace("&nbsp;", "0");

            ShowHideCardInfo();

            txtDiscount.Text = GrdCustomer.Rows[e.NewEditIndex].Cells[14].Text;
            txtPurchasing.Text = GrdCustomer.Rows[e.NewEditIndex].Cells[15].Text;
            txtPoints.Text = GrdCustomer.Rows[e.NewEditIndex].Cells[16].Text;
            txtAmountLimit.Text = GrdCustomer.Rows[e.NewEditIndex].Cells[17].Text;
            txtCardNo.Text = GrdCustomer.Rows[e.NewEditIndex].Cells[18].Text.Replace("&nbsp;", "0");
            txtSalesPer.Text = GrdCustomer.Rows[e.NewEditIndex].Cells[22].Text.Replace("&nbsp;", "0");
            ddlCustomerGroup.Value = GrdCustomer.Rows[e.NewEditIndex].Cells[32].Text.Replace("&nbsp;", "0");
            txtGST.Text = GrdCustomer.Rows[e.NewEditIndex].Cells[33].Text.Replace("&nbsp;", "0");
            rblGST.SelectedValue = GrdCustomer.Rows[e.NewEditIndex].Cells[34].Text.Replace("&nbsp;", "0");
            txtDiscountPOS.Text = GrdCustomer.Rows[e.NewEditIndex].Cells[35].Text.Replace("&nbsp;", "0");
            rblDiscount.SelectedValue = GrdCustomer.Rows[e.NewEditIndex].Cells[36].Text.Replace("&nbsp;", "0");
            txtServiceCharges.Text = GrdCustomer.Rows[e.NewEditIndex].Cells[37].Text.Replace("&nbsp;", "0");
            rblServiceCharges.SelectedValue = GrdCustomer.Rows[e.NewEditIndex].Cells[38].Text.Replace("&nbsp;", "0");
            if (_mUController.IsExist(txtCardNo.Text, Convert.ToInt32(ddDistributorId.Value), Constants.IntNullValue, 4))
            {
                txtCardNo.Enabled = false;
                txtAmountLimit.Enabled = false;
                drpCardType.Enabled = false;
                txtDiscount.Enabled = false;
            }

            btnSave.Text = "Update";

            contentBox.Visible = true;
            lookupBox.Visible = false;
            contentBox.Visible = true;
            lookupBox.Visible = false;
            searchBox.Visible = false;
            searchBtn.Visible = false;
            btnActive.Visible = false;
            btnClose.Visible = true;
            btnSave.Visible = true;
            btnAdd.Visible = false;
            CreateSKUDataTable();
            GetFamilyDetail(Convert.ToInt64(hfCustomerID.Value));
            LoadFamilyGrid();
            CreateAddressDataTable();
            GetAddressDetail(Convert.ToInt64(hfCustomerID.Value));
            LoadAddressGrid();
            btnFamilyAdd.Text = "Add";
            btnAddress.Text = "Add";
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "CatchMsg", "alert('" + ex + "');", true);

        }
    }

    protected void grdData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GrdCustomer.PageIndex = e.NewPageIndex;
        LoadGrid("");
    }

    #endregion
    #region Family Details
    private void GetFamilyDetail(long customerId)
    {
        try
        {
            gvSKU.DataSource = null;
            gvSKU.DataBind();
            if (customerId > 0)
            {
                DataTable dtSKU = cController.GetFamilyDetail(customerId);
                if (dtSKU.Rows.Count > 0)
                {
                    Session.Add("dtSKU", dtSKU);
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void gvSKU_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            RowId.Value = e.NewEditIndex.ToString();
            //drpCustomer.Value = gvSKU.Rows[e.NewEditIndex].Cells[0].Text;
            txtChildName.Text = gvSKU.Rows[e.NewEditIndex].Cells[2].Text;
            txtChildDOB.Text = gvSKU.Rows[e.NewEditIndex].Cells[3].Text;
            drpGender.Value = gvSKU.Rows[e.NewEditIndex].Cells[4].Text == "Male" ? "1" : "2";
            btnFamilyAdd.Text = "Update";
        }
        catch (Exception)
        {
            throw;
        }
    }
    protected void gvSKU_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            DataTable dtSKU = (DataTable)this.Session["dtSKU"];
            dtSKU.Rows.RemoveAt(e.RowIndex);

            gvSKU.DataSource = dtSKU;
            gvSKU.DataBind();
        }
        catch (Exception)
        {
            throw;
        }
    }


    protected void btnAddFamily_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dtSKU = (DataTable)Session["dtSKU"];
            if (CheckDuplicateName())
            {
                if (btnFamilyAdd.Text == "Add")
                {
                    DataRow dr = dtSKU.NewRow();
                    dr["CUSTOMER_ID"] = 0;
                    dr["CUSTOMER_NAME"] = txtName.Text;
                    dr["CHILD_NAME"] = txtChildName.Text;
                    dr["CHILD_DOB"] = txtChildDOB.Text;
                    dr["CHILD_GENDER"] = drpGender.SelectedItem.Text;
                    dtSKU.Rows.Add(dr);
                    txtChildName.Text = string.Empty;
                }
                else
                {
                    DataRow dr = dtSKU.Rows[Convert.ToInt32(RowId.Value)];
                    dr["CUSTOMER_ID"] = string.IsNullOrEmpty(hfCustomerID.Value) ? 0 : Convert.ToInt64(hfCustomerID.Value);
                    dr["CUSTOMER_NAME"] = txtName.Text;
                    dr["CHILD_NAME"] = txtChildName.Text;
                    dr["CHILD_DOB"] = txtChildDOB.Text;
                    dr["CHILD_GENDER"] = drpGender.SelectedItem.Text;
                    btnFamilyAdd.Text = "Add";
                    RowId.Value = "0";
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Record already exists')", true);
            }
            ScriptManager.GetCurrent(Page).SetFocus(txtChildName);

            LoadFamilyGrid();
        }
        catch (Exception)
        {

            throw;
        }
    }
    private void LoadFamilyGrid()
    {
        DataTable dtSKU = (DataTable)Session["dtSKU"];
        gvSKU.DataSource = dtSKU;
        gvSKU.DataBind();
    }
    private bool CheckDuplicateName()
    {
        DataTable dtSKU = (DataTable)Session["dtSKU"];
        if (dtSKU.Rows.Count > 0)
        {
            DataRow[] foundRows = dtSKU.Select("CHILD_NAME  = '" + txtChildName.Text + "'");
            if (foundRows.Length == 0)
            {
                return true;
            }
        }
        else
        {
            return true;
        }
        return false;
    }
    private void CreateSKUDataTable()
    {
        DataTable dtSKU = new DataTable();
        dtSKU.Columns.Add("CUSTOMER_ID", typeof(long));
        dtSKU.Columns.Add("CUSTOMER_NAME", typeof(string));
        dtSKU.Columns.Add("CHILD_NAME", typeof(string));
        dtSKU.Columns.Add("CHILD_GENDER", typeof(string));
        dtSKU.Columns.Add("CHILD_DOB", typeof(string));
        Session.Add("dtSKU", dtSKU);
    }

    #endregion

    #region Address
    private void GetAddressDetail(long customerId)
    {
        try
        {
            GridAdress.DataSource = null;
            GridAdress.DataBind();
            if (customerId > 0)
            {
                DataTable dtAddress = cController.GetAddressDetail(customerId);
                if (dtAddress.Rows.Count > 0)
                {
                    Session.Add("dtAddress", dtAddress);
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void CreateAddressDataTable()
    {
        DataTable dtAddress = new DataTable();
        dtAddress.Columns.Add("CUSTOMER_ID", typeof(long));
        dtAddress.Columns.Add("CUSTOMER_NAME", typeof(string));
        dtAddress.Columns.Add("CUSTOMER_ADDRESS", typeof(string));
        Session.Add("dtAddress", dtAddress);
    }
    protected void btnAddress_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dtAddress = (DataTable)Session["dtAddress"];
            if (CheckDuplicateAddress())
            {
                if (btnAddress.Text == "Add")
                {
                    DataRow dr = dtAddress.NewRow();
                    dr["CUSTOMER_ID"] = 0;
                    dr["CUSTOMER_NAME"] = txtName.Text;
                    dr["CUSTOMER_ADDRESS"] = txtAddress1.Text;
                    dtAddress.Rows.Add(dr);
                    txtAddress1.Text = string.Empty;
                }
                else
                {
                    DataRow dr = dtAddress.Rows[Convert.ToInt32(AddressRowId.Value)];
                    dr["CUSTOMER_ID"] = string.IsNullOrEmpty(hfCustomerID.Value) ? 0 : Convert.ToInt64(hfCustomerID.Value);
                    dr["CUSTOMER_NAME"] = txtName.Text;
                    dr["CUSTOMER_ADDRESS"] = txtAddress1.Text;
                    btnAddress.Text = "Add";
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Record already exists')", true);
            }
            ScriptManager.GetCurrent(Page).SetFocus(txtChildName);
            txtAddress1.Text = "";
            LoadAddressGrid();
        }
        catch (Exception)
        {

            throw;
        }
    }
    private void LoadAddressGrid()
    {
        DataTable dtAddress = (DataTable)Session["dtAddress"];
        GridAdress.DataSource = dtAddress;
        GridAdress.DataBind();
    }
    private bool CheckDuplicateAddress()
    {
        DataTable dtSKU = (DataTable)Session["dtAddress"];

        if (dtSKU.Rows.Count > 0)
        {
            DataRow[] foundRows = dtSKU.Select("CUSTOMER_ADDRESS  = '" + txtAddress1.Text + "'");
            if (foundRows.Length == 0)
            {
                return true;
            }
        }
        else
        {
            return true;
        }
        return false;
    }
    protected void GridAdress_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            DataTable dtAddress = (DataTable)this.Session["dtAddress"];
            dtAddress.Rows.RemoveAt(e.RowIndex);

            GridAdress.DataSource = dtAddress;
            GridAdress.DataBind();
        }
        catch (Exception)
        {
            throw;
        }
    }

    protected void GridAdress_RowEditing(object sender, GridViewEditEventArgs e)
    {
        AddressRowId.Value = e.NewEditIndex.ToString();
        txtAddress1.Text = GridAdress.Rows[e.NewEditIndex].Cells[2].Text;
        btnAddress.Text = "Update";
    }
    #endregion
}