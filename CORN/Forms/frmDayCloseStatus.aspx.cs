using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using System.Web;
using System.Linq;

/// <summary>
/// Form For Day Close
/// </summary>
public partial class Forms_frmDayCloseStatus : System.Web.UI.Page
{
    DistributorController mController = new DistributorController();
    readonly DistributorController _mDist = new DistributorController();
    DataControl DC = new DataControl();

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        Response.Cache.SetNoStore();
        Response.AppendHeader("pragma", "no-cache");

        if (!Page.IsPostBack)
        {
            GetAppSettingDetail();
            LastClosedDay(int.Parse(this.Session["UserId"].ToString()), int.Parse(this.Session["Distributor_Id"].ToString()));
            if (gvDayClose.Rows.Count > 1)
            {
                gvDayClose.UseAccessibleHeader = true;
                gvDayClose.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            bool chk = bool.Parse(Session["IS_CanReverseDayClose"].ToString());
            if (chk)
            {
                btnReverseDayClose.Visible = true;
            }
        }
    }

    private DataTable SelectPendingBills(int pDistributorID, DateTime pWorkingDate)
    {
        OrderEntryController mSkuController = new OrderEntryController();
        DataTable dtOrders = mSkuController.SelectPendingBills(Constants.LongNullValue, pWorkingDate, pDistributorID, int.Parse(HttpContext.Current.Session["UserID"].ToString()), Constants.IntNullValue, 2);
        return dtOrders;
    }
    /// <summary>
    /// Gets Location(s) Last Day Close(s)
    /// </summary>
    /// <param name="UserId">User</param>
    /// <param name="p_Distributor">Location</param>
    private void LastClosedDay(int UserId, int p_Distributor)
    {
        btnDayClose.Visible = true;
        GetLastClosedDay(UserId, p_Distributor, 4);
    }

    /// <summary>
    /// Loads Location(s) Last Day Close(s) To Grid
    /// </summary>
    /// <param name="UserId">User</param>
    /// <param name="p_Distributor">Location</param>
    /// <param name="p_Status">Status</param>
    private void GetLastClosedDay(int UserId, int p_Distributor, int p_Status)
    {
        DataTable dtable = mController.MaxDayClose(int.Parse(this.Session["UserId"].ToString()), p_Status);
        gvDayClose.DataSource = dtable;
        gvDayClose.DataBind();
    }

    /// <summary>
    /// Performs Following Tasks And LogOuts
    /// </summary>
    /// <remarks>
    /// <list type="bullet">
    /// <item>
    /// Closes Day Transactions
    /// </item>
    /// <item>
    /// Inserts LogOut Time
    /// </item>
    /// </list>
    /// </remarks>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnDayClose_Click(object sender, EventArgs e)
    {
        try
        {
            if (btnDayClose.Visible == true)
            {
                int count = 0;
                bool flag = true;
                foreach (GridViewRow gvr in gvDayClose.Rows)
                {
                    var cbSelect = (CheckBox)(gvr.Cells[0].FindControl("cbLocation"));
                    if (cbSelect.Checked)
                    {
                        count++;                
                        if (Session["CheckPhysicalStockOnDayClose"].ToString() == "1")
                        {
                            DataTable dtPhysicalStock = GetPhysicalStock(Convert.ToInt32(gvr.Cells[6].Text), Convert.ToDateTime(gvr.Cells[7].Text));
                            if (dtPhysicalStock.Rows.Count == 0)
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert(' Plz Insert Physical Stock first Location : " + gvr.Cells[1].Text + "');", true);
                                flag = false;
                                break;
                            }
                        }
                        DataTable settingDt = (DataTable)Session["dtAppSettingDetail"];
                        bool checkNegativeStock = false;
                        if (settingDt.Rows.Count > 0)
                        {
                            checkNegativeStock = Convert.ToInt32(settingDt.Rows[0]["CheckNegativeStockOnDayClose"]) == 1 ? true : false;
                        }

                        DataTable dtOrders = SelectPendingBills(Convert.ToInt32(gvr.Cells[6].Text), Convert.ToDateTime(gvr.Cells[7].Text));
                        DataTable dtTransfer = GetTransferIn(Convert.ToInt32(gvr.Cells[6].Text), Convert.ToDateTime(gvr.Cells[7].Text));                        
                        DataTable dtAutoPurchase = GetAutoPurchase(Convert.ToInt32(gvr.Cells[6].Text), Convert.ToDateTime(gvr.Cells[7].Text));

                        DataTable dtNegativeClosing = new DataTable();
                        if (checkNegativeStock == true)
                        {
                            dtNegativeClosing = GetNegativeClosingStock(Convert.ToInt32(gvr.Cells[6].Text));
                        }
                        if (dtOrders.Rows.Count > 0)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert(' There are Open Orders, Can not Close the Day while orders open Location : " + gvr.Cells[1].Text + "');", true);
                            flag = false;
                            break;
                        }
                        else if (dtTransfer.Rows.Count > 0)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert(' Plz Close your Transfer In(s) first Location : " + gvr.Cells[1].Text + "');", true);
                            flag = false;
                            break;
                        }
                        else if (dtAutoPurchase.Rows.Count > 0)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert(' Plz Close your Auto Purchase(s) first Location : " + gvr.Cells[1].Text + "');", true);
                            flag = false;
                            break;
                        }
                        else if (dtNegativeClosing.Rows.Count > 0)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert(' There are some items below whose Closing Stock in negative Location : " + gvr.Cells[1].Text + "');", true);
                            GridNegativeStock.Visible = true;
                            GridNegativeStock.DataSource = dtNegativeClosing;
                            GridNegativeStock.DataBind();
                            flag = false;
                            break;
                        }
                    }
                }
                if (count > 0 && flag)
                {

                    foreach (GridViewRow gvr in gvDayClose.Rows)
                    {
                        var cbSelect = (CheckBox)(gvr.Cells[0].FindControl("cbLocation"));
                        if (cbSelect.Checked)
                        {   
                            try
                            {
                                DistributorController mDayClose = new DistributorController();
                                bool dt = mController.UspDayClose(Convert.ToDateTime(gvr.Cells[7].Text), Convert.ToInt32(gvr.Cells[6].Text), int.Parse(this.Session["UserID"].ToString()));
                                if (dt == false)
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert(' Some error in day Close Contact System Administrator');", true);
                                }
                                else
                                {
                                    try
                                    {
                                        DataTable dtNotification = GetNotificationData(Convert.ToInt32(gvr.Cells[6].Text), Convert.ToDateTime(gvr.Cells[7].Text));
                                        WebAPINotification webApi = new WebAPINotification();
                                        webApi.SendNotification("Day Close Done", string.Format(string.Format("Location : {{0}} {0} Working Date: {{1:dd-MMM-yyyy}} {1} Day Closed by: {{2}} at {{3}} {2} Net Sale: {{4}}", Environment.NewLine, Environment.NewLine, Environment.NewLine), gvr.Cells[1].Text, Convert.ToDateTime(gvr.Cells[7].Text), Session["UserName"].ToString(), System.DateTime.Now.ToString("hh:mm tt"), (int)Convert.ToDecimal(dtNotification.Rows[0]["TdaySale"])));
                                    }
                                    catch (Exception ex)
                                    {
                                        CORNCommon.Classes.ExceptionPublisher.PublishException(ex);
                                    }
                                    LastClosedDay(int.Parse(this.Session["UserId"].ToString()), int.Parse(this.Session["Distributor_Id"].ToString()));
                                }
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }

                    UserController mUserCtl = new UserController();
                    if (mUserCtl.InsertUserLogoutTime(Convert.ToInt64(Session["User_Log_ID"]), Convert.ToInt32(Session["UserID"])) == "Logout Time Inserted")
                    {
                        this.Session.Clear();
                        Response.Redirect("~/Login.aspx");
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert(' Some error in day Close Contact System Administrator');", true);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btnReverseDayClose_Click(object sender, EventArgs e)
    {
        try
        {
            if (btnReverseDayClose.Visible == true)
            {
                bool flag = true;
                int count = 0;
                foreach (GridViewRow gvr in gvDayClose.Rows)
                {
                    var cbSelect = (CheckBox)(gvr.Cells[0].FindControl("cbLocation"));
                    if (cbSelect.Checked)
                    {
                        count++;
                        flag = mController.UspDayReverse(Convert.ToDateTime(gvr.Cells[7].Text), Convert.ToInt32(gvr.Cells[6].Text), int.Parse(this.Session["UserID"].ToString()));
                        if (!flag)
                        {
                            break;
                        }
                    }
                }
                if (flag && count > 0)
                {
                    Session.Clear();
                    Response.Redirect("~/Login.aspx");                    
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert(' Some error in Reverse Day Close Contact System Administrator');", true);
                    return;
                }
            }
        }
        catch (Exception)
        {

            throw;
        }
    }
    private DataTable GetTransferIn(int pDistributorID, DateTime pWorkingDate)
    {
        OrderEntryController mSkuController = new OrderEntryController();
        DataTable dtTransfer = mSkuController.SelectPendingBills(Constants.LongNullValue,pWorkingDate, pDistributorID, int.Parse(HttpContext.Current.Session["UserID"].ToString()), Constants.IntNullValue, 9);
        return dtTransfer;
    }

    private DataTable GetPhysicalStock(int pDistributorID,DateTime pWorkingDate)
    {
        PurchaseController mPurchase = new PurchaseController();
        DataTable dtPhyscialStock = mPurchase.SelectPurchaseDocumentNo(11, pDistributorID, Convert.ToInt32(Session["UserID"]), pWorkingDate);
        return dtPhyscialStock;
    }

    private DataTable GetNotificationData(int pDistributorID, DateTime pWorkingDate)
    {
        OrderEntryController mController = new OrderEntryController();
        DataTable dtNotification = mController.GetNotificationData(pDistributorID, pWorkingDate,1);
        return dtNotification;
    }

    private DataTable GetAutoPurchase(int pDistributorID, DateTime pWorkingDate)
    {
        FranchiseSaleInvoiceController mFranchise = new FranchiseSaleInvoiceController();
        DataTable dtPurchase = mFranchise.SelectFranchise_Invoice_Lookup(pDistributorID, Convert.ToInt32(Session["UserID"]), pWorkingDate, 2);
        return dtPurchase;
    }
    private DataTable GetNegativeClosingStock(int pDistributorID)
    {
        SkuController RptInventoryCtl = new SkuController();

        DataSet ds = RptInventoryCtl.GetSKUClosingStock(Constants.IntNullValue, pDistributorID);

        DataTable dt = ds.Tables[0];
        DataTable negativeDt = new DataTable();
        if (dt.Rows.Count > 0)
        {
            DataRow[] newDt = dt.AsEnumerable()
            .Where(row => row.Field<decimal>("CLOSING_STOCK") < 0)
            .ToArray();

            if (newDt != null && newDt.Length > 0)
            {
                negativeDt = newDt.CopyToDataTable();
            }
        }

        return negativeDt;
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
}