using System;
using System.Data;
using System.Web.UI;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using System.Collections;
using System.Web;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json.Linq;

public partial class Login : Page
{
    readonly UserController _mController = new UserController();
    readonly DistributorController _mDist = new DistributorController();
    public static DataTable dtConfig;

    protected void Page_Load(object sender, EventArgs e)
    {        
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        Response.Cache.SetNoStore();
        Response.AppendHeader("pragma", "no-cache");
        if (!Page.IsPostBack)
        {
            GetAppSettingDetail();
            GetAppPath();
            dtConfig = (DataTable)Session["dtAppSettingDetail"];
            try
            {
                bool IsLicense = true;
                try
                {
                    if (Session["LicenseMessage"].ToString().Length > 0)
                    {
                        IsLicense = false;
                    }
                }
                catch (Exception ex)
                {
                    IsLicense = true;
                }
                if (Convert.ToInt32(Session["UserID"]) > 0 && IsLicense)
                {
                    Response.Redirect("Forms/Home.aspx");
                }
            }
            catch (Exception)
            {
                Session.Remove("LicenseMessage");
                txtLogin.Focus();
            }            
        }
    }
    private void ValidateUser()
    {

        if (txtLogin.Text == "" && txtPassword.Text == "")
        {
            lblErrorMsg.Text = "Plz enter User name / Password";
            dvError.Visible = true;
            return;
        }
        else
        {
            DataTable dt = null;
            bool IsEncrypted = false;
            if (dtConfig.Rows[0]["IsEncreptedCredentials"].ToString() == "1")
            {
                IsEncrypted = true;
            }
            if (IsEncrypted)
            {
                dt = _mController.SelectSlashUserEncrypt(txtLogin.Text, Cryptography.Encrypt(txtPassword.Text, Constants.CryptographyKey));
            }
            else
            {
                dt = _mController.SelectSlashUser(txtLogin.Text, txtPassword.Text);
            }
            Session.Clear();
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["USER_ID"].ToString() != "1") //for admin don't need to verify either base location is active or not.
                {
                    if (dt.Rows[0]["LocationStatus"].ToString() == "In-Active")
                    {
                        lblErrorMsg.Text = "This User Base Location is In Active";
                        dvError.Visible = true;
                        return;
                    }
                }
                bool IsAutoPasswordChangeAlert = false;
                int PasswordChangeAlertDays = 30;
                IsAutoPasswordChangeAlert = false;
                if(dtConfig.Rows[0]["PasswordChangeAlert"].ToString() == "1")
                {
                    IsAutoPasswordChangeAlert = true;
                }
                PasswordChangeAlertDays = Convert.ToInt32(dtConfig.Rows[0]["PasswordChangeDays"]);
                if (IsAutoPasswordChangeAlert)
                {
                    var lastPasswordChanged = dt.Rows[0]["Last_Password_Changed"].ToString();
                    if (!string.IsNullOrEmpty(lastPasswordChanged))
                    {
                        var lastPasswordChangedDated = Convert.ToDateTime(lastPasswordChanged).Date;
                        if (DateTime.Now.Date.AddDays(-Convert.ToDouble(PasswordChangeAlertDays))>= Convert.ToDateTime(lastPasswordChanged))
                        {
                            if (!HttpContext.Current.Request.Path.EndsWith("Forms/frmForceChangePassword.aspx?id=" + dt.Rows[0]["USER_ID"].ToString() + "", StringComparison.InvariantCultureIgnoreCase))
                                Response.Redirect("Forms/frmForceChangePassword.aspx?id=" + dt.Rows[0]["USER_ID"].ToString());
                        }
                    }
                }
                if (Convert.ToString(dt.Rows[0]["IS_ACTIVE"]) == "True" || Convert.ToString(dt.Rows[0]["IS_ACTIVE"]) == "Active")
                {
                    GetConnString();
                    ArrayList list = new ArrayList();
                    {
                        list.Add(dt.Rows[0]["ROLE_ID"].ToString());
                        list.Add(dt.Rows[0]["USER_DETAIL"].ToString());
                    }
                    Session["EmployeeInfo"] = list;                    
                    bool ClosingStockStatus = false;
                    if(dtConfig.Rows[0]["ShowClosingStockStatus"].ToString() == "1")
                    {
                        ClosingStockStatus = true;
                    }
                    Session.Add("VALIDATE_STOCK", ClosingStockStatus);
                    Session.Add("ConsumptionFromProductionPlan", dtConfig.Rows[0]["ConsumptionFromProductionPlan"].ToString());
                    Session.Add("UserID", Convert.ToInt32(dt.Rows[0]["USER_ID"].ToString()));
                    Session.Add("UserName", dt.Rows[0]["USER_DETAIL"].ToString());
                    Session.Add("DISTRIBUTOR_ID", Convert.ToInt32(dt.Rows[0]["DISTRIBUTOR_ID"]));
                    Session.Add("LocationName", dt.Rows[0]["DISTRIBUTOR_NAME"].ToString());
                    Session.Add("CompanyId", Convert.ToInt32(dt.Rows[0]["COMPANY_ID"].ToString()));
                    Session.Add("COMPANY_NAME", dt.Rows[0]["COMPANY_NAME"].ToString());
                    Session.Add("ADDRESS1", dt.Rows[0]["ADDRESS1"].ToString());
                    Session.Add("PHONE", dt.Rows[0]["PHONE"].ToString());
                    Session.Add("RoleID", Convert.ToInt32(dt.Rows[0]["ROLE_ID"]));
                    Session.Add("IS_SystemAdmin", Convert.ToInt32(dt.Rows[0]["IS_SystemAdmin"]));
                    Session.Add("IS_CanReverseDayClose", Convert.ToBoolean(dt.Rows[0]["IS_CanReverseDayClose"]));
                    Session.Add("IS_CanGiveDiscount", Convert.ToBoolean(dt.Rows[0]["IS_CanGiveDiscount"]));
                    Session.Add("Division", Convert.ToInt32(dt.Rows[0]["ISCURRENT"]));
                    Session.Add("UserType", dt.Rows[0]["USER_TYPE_ID"].ToString());
                    Session.Add("TodayMenuID", dt.Rows[0]["TodayMenuID"].ToString());
                    Session.Add("TaxIntegration", dt.Rows[0]["TaxAuthority"].ToString());
                    Session.Add("IsKOTServiceInstalled", dt.Rows[0]["IsKOTServiceInstalled"]);
                    Session.Add("ValidateStockOnPOS", dt.Rows[0]["ValidateStockOnPOS"]);
                    Session.Add("RollBackType", dt.Rows[0]["RollBackType"]);
                    Session.Add("CheckPhysicalStockOnDayClose", dt.Rows[0]["CheckPhysicalStockOnDayClose"]);
                    Session.Add("ProductionInPriceFromBOM", dt.Rows[0]["ProductionInPriceFromBOM"]);
                    Session.Add("ItemWiseGST", dt.Rows[0]["ItemWiseGST"]);
                    Session.Add("ShowParentCategory", dt.Rows[0]["ShowParentCategory"]);
                    Session.Add("IsLocationWiseItem", dt.Rows[0]["IsLocationWiseItem"]);
                    Session.Add("PrintInvoiceFromWS", dt.Rows[0]["PrintInvoiceFromWS"]);
                    Session.Add("HiddenReports", dt.Rows[0]["HiddenReports"]);
                    Session.Add("HiddenReportsDetail", dt.Rows[0]["HiddenReportsDetail"]);
                    Session.Add("EnableAutoStockAdjustment", dt.Rows[0]["AutoStockAdjustment"]);
                    Session.Add("DisablePriceOnProductionIn", dt.Rows[0]["DisablePriceOnProductionIn"]);
                    Session.Add("ItemWiseGSTOnPurchase", dt.Rows[0]["ItemWiseGSTOnPurchase"]);
                    Session.Add("LocationWiseCOA", dt.Rows[0]["LocationWiseCOA"]);
                    Session.Add("LocationWiseRecipe", dt.Rows[0]["LocationWiseRecipe"]);
                    Session.Add("FranchiseModule", dt.Rows[0]["FranchiseModule"]);
                    Session.Add("CustomerWiseGST", dt.Rows[0]["CustomerWiseGST"]);
                    Session.Add("CustomerLocationWise", dt.Rows[0]["CustomerLocationWise"]);
                    Session.Add("KDSImplemented", dt.Rows[0]["KDSImplemented"]);
                    Session.Add("ShowDatesOnPOSReports", dt.Rows[0]["ShowDatesOnPOSReports"]);
                    Session.Add("CanTaxIntegrate", dt.Rows[0]["CanTaxIntegrate"]);
                    Session.Add("VoucherEntryMinimumDate", dt.Rows[0]["VoucherEntryMinimumDate"]);
                    Session.Add("MenuIDs", dt.Rows[0]["MenuIDs"].ToString());
                    if (LastClosedDay(Convert.ToInt32(dt.Rows[0]["USER_ID"]), Convert.ToInt32(dt.Rows[0]["DISTRIBUTOR_ID"])))
                    {
                        if (Session["FranchiseModule"].ToString() == "1")
                        {
                            LoadVendors();
                        }
                        long userLogId = _mController.InsertUserLoginTime(Convert.ToInt32(dt.Rows[0]["USER_ID"]));
                        Session.Add("User_Log_ID", userLogId);


                        DataTable dtLicenseData = _mDist.GetLicenseData(Convert.ToInt32(dt.Rows[0]["DISTRIBUTOR_ID"]));
                        if (dtLicenseData.Rows.Count > 0)
                        {
                            DateTime dtMaxClosingDate = Constants.DateNullValue;
                            DateTime dtMaxDate = Constants.DateNullValue;
                            if (dtLicenseData.Rows[0]["IsLicenseImplemented"].ToString() == "1")
                            {
                                try
                                {
                                    dtMaxClosingDate = Convert.ToDateTime(dtLicenseData.Rows[0]["MaxClosingDate"]);
                                    dtMaxDate = Convert.ToDateTime(Cryptography.Decrypt(dtLicenseData.Rows[0]["LICENSE_DATE"].ToString(), Constants.CryptographyKey));
                                }
                                catch (Exception)
                                {                                    
                                    lblLicenseMsg.Text = "CORN POS license has been expired. Please pay monthly subscription fee to continue uninterpreted services. Thank you!";
                                    btnSignIn.Visible = false;
                                    txtLogin.Visible = false;
                                    txtPassword.Visible = false;
                                    dvLicense.Visible = true;
                                    Session.Clear();
                                    return;
                                }
                                if (dtMaxClosingDate >= dtMaxDate)
                                {
                                    lblLicenseMsg.Text = "CORN POS license has been expired. Please pay monthly subscription fee to continue uninterpreted services. Thank you!";
                                    btnSignIn.Visible = false;
                                    txtLogin.Visible = false;
                                    txtPassword.Visible = false;
                                    dvLicense.Visible = true;
                                    Session.Clear();
                                    return;
                                }
                                else
                                {
                                    if ((dtMaxDate - dtMaxClosingDate).TotalDays <= 5)
                                    {
                                        double _remaingindays = (dtMaxDate - dtMaxClosingDate).TotalDays;
                                        lblLicenseMsg.Text = string.Format("CORN POS license will be expired after {0} Day(s), Please pay monthly subscription fee to continue uninterpreted services. Thank you!", _remaingindays);
                                        Session.Add("LicenseMessage", lblLicenseMsg.Text);
                                    }
                                }
                            }
                        }
                        if (dt.Rows[0]["ROLE_NAME"].ToString() == "Tab")
                        {
                            Response.Redirect("Forms/frmOrderTaking.aspx");
                        }
                        else
                        {
                            Response.Redirect("Forms/Home.aspx");
                        }
                    }
                }
                else
                {
                    lblErrorMsg.Text = "This User is In Active";
                    dvError.Visible = true;
                }
            }
            else
            {
                lblErrorMsg.Text = "Wrong User Id/Password";
                dvError.Visible = true;
            }
        }
    }
    private bool LastClosedDay(int userId, int pDistributor)
    {
        bool IsOk = true;
        if (userId == 1)
        {
            DataTable dt = _mDist.SelectMaxDayClose(userId, pDistributor);
            Session.Add("CurrentWorkDate", dt.Rows.Count > 0 ? DateTime.Parse(dt.Rows[0]["CLOSING_DATE"].ToString()) : DateTime.Now.Date);
        }
        else
        {
            DataTable dt = _mDist.SelectMaxDayClose(userId, pDistributor);
            if (dt.Rows.Count > 0)
            {
                Session.Add("CurrentWorkDate", DateTime.Parse(dt.Rows[0]["CLOSING_DATE"].ToString()));
            }
            else
            {
                lblErrorMsg.Text = "Working Date not found.";
                dvError.Visible = true;
                IsOk = false;
            }
        }
        return IsOk;
    }
    private bool CheckDeplyed()
    {
        try
        {
            bool deployed = false;
            if(dtConfig.Rows[0]["IsDeployed"].ToString() == "1")
            {
                deployed = true;
            }
            if (deployed)
            {
                return true;
            }
            if (dtConfig.Rows.Count > 0)
            {
                string CInfo = Cryptography.Encrypt(ComputerInfo.Value(), Constants.CryptographyKey);
                string ComInfo = dtConfig.Rows[0]["ComputerInfo"].ToString();                
                bool ComCheck = false;
                if (ComInfo.Trim() == CInfo)
                {
                    ComCheck = true;
                }
                return ComCheck;
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg3", "alert('Error Occured: \n" + ex + "');", true);
            return false;
        }
    }
    public static void GetAppPath()
    {
        try
        {
            DataTable dt = (DataTable)HttpContext.Current.Session["dtAppSettingDetail"];
            if (dt.Rows.Count > 0)
            {
                Configuration.AppPath = dt.Rows[0]["AppPath"].ToString();
            }
            else
            {
                Configuration.AppPath = null;
            }
        }
        catch (Exception)
        {
        }
    }
    protected void btnSignIn_Click(object sender, EventArgs e)
    {
        bool Check = CheckDeplyed();
        if (Check)
        {
            if (System.Configuration.ConfigurationManager.AppSettings["IsDeployment"] != null)
            {
                if (System.Configuration.ConfigurationManager.AppSettings["IsDeployment"].ToString() == "1")
                {
                    int DistributorID = Convert.ToInt32(DataControl.chkNull_Zero(System.Configuration.ConfigurationManager.AppSettings["DistributorID"].ToString()));
                    try
                    {
                        ValidateUser();
                    }
                    catch (Exception)
                    {

                    }
                }
                else
                {
                    ValidateUser();
                }
            }
            else
            {
                ValidateUser();
            }
        }
        else
        {
            dvError.Visible = true;
            lblErrorMsg.Text = "Invalid Key. Contact to Administrator.";
        }
    }
    private void LoadVendors()
    {
        DataTable dtVendors = new DataTable();
        dtVendors.Columns.Add("VendorID", typeof(int));
        dtVendors.Columns.Add("SupplierLocationID", typeof(int));
        dtVendors.Columns.Add("VendorName", typeof(string));
        dtVendors.Columns.Add("VendorType", typeof(int));

        var PController = new SKUPriceDetailController();
        DataTable dtVendor = PController.SelectDataPrice(Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, Constants.IntNullValue, int.Parse(Session["UserId"].ToString()), Constants.IntNullValue, 0, Constants.DateNullValue);
        int RowID = 0;
        foreach (DataRow dr in dtVendor.Rows)
        {
            RowID += 1;
            DataRow drVendor = dtVendors.NewRow();
            drVendor["VendorID"] = RowID;
            drVendor["SupplierLocationID"] = dr["Company_Id"];
            drVendor["VendorName"] = dr["Company_Name"];
            drVendor["VendorType"] = 1;
            dtVendors.Rows.Add(drVendor);
        }
        DistributorController DController = new DistributorController();
        DataTable dt = DController.GetDistributorWithMaxDayClose(Constants.IntNullValue, Constants.IntNullValue, int.Parse(this.Session["CompanyId"].ToString()), 3);

        foreach (DataRow dr in dt.Rows)
        {
            RowID += 1;
            DataRow drVendor = dtVendors.NewRow();
            drVendor["VendorID"] = RowID;
            drVendor["SupplierLocationID"] = dr["DISTRIBUTOR_ID"];
            drVendor["VendorName"] = dr["DISTRIBUTOR_NAME"];
            drVendor["VendorType"] = 2;
            dtVendors.Rows.Add(drVendor);
            RowID += 1;
        }

        Session.Add("dtVendors", dtVendors);
    }
    public void GetAppSettingDetail()
    {
        try
        {
            AppSettingDetail _cController = new AppSettingDetail();
            DataTable dtAppSetting = _cController.GetAppSettingDetail(1);
            if (dtAppSetting.Rows.Count > 0)
            {
                Session.Add("dtAppSettingDetail", dtAppSetting);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg3", "alert('Error Occured: \n" + ex + "');", true);
        }
    }
    private void GetConnString()
    {
        //string apiUrl = System.Configuration.ConfigurationManager.AppSettings["ApiUrl"].ToString() + "api/MultiTenant/GetClientInfo?Pin=" + System.Configuration.ConfigurationManager.AppSettings["ConnectionPin"].ToString();
        //using (HttpClient client = new HttpClient())
        //{
        //    try
        //    {
        //        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
        //        HttpResponseMessage response = client.GetAsync(apiUrl).Result;
        //        response.EnsureSuccessStatusCode();
        //        string responseData = response.Content.ReadAsStringAsync().Result;
        //        Session.Add("ConnString",JObject.Parse(responseData)["ClientConnString"].ToString());
        //    }
        //    catch (HttpRequestException ex)
        //    {
        //    }
        //}
    }
}