using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;

/// <summary>
/// Form To Rollback Order, Invoice, Sale Return And Realized Cheque
/// </summary>
public partial class Forms_frmRollBackOrder : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        Response.Cache.SetNoStore();
        Response.AppendHeader("pragma", "no-cache");

        if (!Page.IsPostBack)
        {
            if (IsDayClosed())
            {
                UserController UserCtl = new UserController();

                UserCtl.InsertUserLogoutTime(Convert.ToInt32(Session["User_Log_ID"]), Convert.ToInt32(Session["UserID"]));
                Session.Clear();
                System.Web.Security.FormsAuthentication.SignOut();
                Response.Redirect("../Login.aspx");
            }

            LoadDistributor();
            LoadRollBackReason();

            DateTime CurrentWorkDate = Constants.DateNullValue;
            DataTable dtLocationInfo = (DataTable)Session["dtLocationInfo"];
            foreach (DataRow dr in dtLocationInfo.Rows)
            {
                if (dr["DISTRIBUTOR_ID"].ToString() == drpDistributor.Value.ToString())
                {
                    if (dr["MaxDayClose"].ToString().Length > 0)
                    {
                        CurrentWorkDate = Convert.ToDateTime(dr["MaxDayClose"]);
                        break;
                    }
                }
            }

            if (CurrentWorkDate != null && CurrentWorkDate != Constants.DateNullValue)
            {
                txtDocumentDate.InnerText = "Working Date: " + CurrentWorkDate.ToString("dd-MMM-yyyy");
            }
        }
    }
   
    #region Load
    
    private void LoadDistributor()
    {
        DistributorController DController = new DistributorController();
        DataTable dt = DController.SelectDistributorInfo(Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), int.Parse(this.Session["CompanyId"].ToString()));
        clsWebFormUtil.FillDxComboBoxList(drpDistributor, dt, 0, 2, true);

        if (dt.Rows.Count > 0)
        {
            drpDistributor.SelectedIndex = 0;
        }

        Session.Add("dtLocationInfo", dt);
    }
    
    private void LoadRollBackReason()
    {
        FranchiseSaleInvoiceController franchiseController = new FranchiseSaleInvoiceController();
        DataTable rollBackDt = franchiseController.SelectROLLBACK_REASON(1);
        clsWebFormUtil.FillDxComboBoxList(this.ddlRollBackReason, rollBackDt, "REASON_ID", "REASON_DESC");

        if (rollBackDt.Rows.Count > 0)
        {
            ddlRollBackReason.SelectedIndex = 0;
        }
    }
    
    private void LoadRollbackDocument()
    {
        DateTime CurrentWorkDate = Constants.DateNullValue;
        DataTable dtLocationInfo = (DataTable)Session["dtLocationInfo"];
        foreach (DataRow dr in dtLocationInfo.Rows)
        {
            if (dr["DISTRIBUTOR_ID"].ToString() == drpDistributor.Value.ToString())
            {
                if (dr["MaxDayClose"].ToString().Length > 0)
                {
                    CurrentWorkDate = Convert.ToDateTime(dr["MaxDayClose"]);
                    break;
                }
            }
        }


        var or = new OrderEntryController();
        DataTable dtOrder = or.SelectRollBackDocument(int.Parse(drpDistributor.SelectedItem.Value.ToString()),Constants.IntNullValue,
           Constants.IntNullValue, int.Parse(DrpDocumentType.SelectedValue), CurrentWorkDate);
        GrdOrder.DataSource = dtOrder;
        GrdOrder.DataBind();
    }
    
    #endregion    

    #region Click

    protected void btnPost_Click(object sender, EventArgs e)
    {
        if (IsDayClosed())
        {
            UserController UserCtl = new UserController();

            UserCtl.InsertUserLogoutTime(Convert.ToInt32(Session["User_Log_ID"]), Convert.ToInt32(Session["UserID"]));
            Session.Clear();
            System.Web.Security.FormsAuthentication.SignOut();
            Response.Redirect("../Login.aspx");
        }
        foreach (GridViewRow dr in GrdOrder.Rows)
        {
            var chbInvoice = (CheckBox)dr.FindControl("ChbInvoice");
            if (chbInvoice.Checked == true)
            {
                var ord = new OrderEntryController();
                ord.UpdateRollBackDocument(Convert.ToInt64(GrdOrder.DataKeys[dr.RowIndex].Values["SALE_INVOICE_ID"]), int.Parse(DrpDocumentType.SelectedValue), int.Parse(DrpLenged.SelectedValue), Convert.ToInt32(Session["UserID"]), Convert.ToInt16(ddlRollBackReason.SelectedItem.Value));
            }
        }
        LoadRollbackDocument();
    }
    protected void btnGetOrder_Click(object sender, EventArgs e)
    {
        if (IsDayClosed())
        {
            UserController UserCtl = new UserController();

            UserCtl.InsertUserLogoutTime(Convert.ToInt32(Session["User_Log_ID"]), Convert.ToInt32(Session["UserID"]));
            Session.Clear();
            System.Web.Security.FormsAuthentication.SignOut();
            Response.Redirect("../Login.aspx");
        }

        GrdOrder.Visible = true;
        GrdCheque.Visible = false;
        LoadRollbackDocument();

    }
    #endregion

    private bool IsDayClosed()
    {
        DistributorController DistrCtl = new DistributorController();
        try
        {
            DataTable dtDayClose = DistrCtl.MaxDayClose(Convert.ToInt32(Session["DISTRIBUTOR_ID"]), 3);
            if (dtDayClose.Rows.Count > 0)
            {
                if (Convert.ToDateTime(Session["CurrentWorkDate"]) == Convert.ToDateTime(dtDayClose.Rows[0]["DayClose"]))
                {
                    return false;
                }
            }
            return true;
        }
        catch (Exception)
        {

            throw;
        }
    }

    protected void drpDistributor_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpDistributor.Items.Count > 0)
        {
            DateTime CurrentWorkDate = Constants.DateNullValue;
            DataTable dtLocationInfo = (DataTable)Session["dtLocationInfo"];
            foreach (DataRow dr in dtLocationInfo.Rows)
            {
                if (dr["DISTRIBUTOR_ID"].ToString() == drpDistributor.Value.ToString())
                {
                    if (dr["MaxDayClose"].ToString().Length > 0)
                    {
                        CurrentWorkDate = Convert.ToDateTime(dr["MaxDayClose"]);
                        break;
                    }
                }
            }

            txtDocumentDate.InnerText = "Working Date: " + CurrentWorkDate.ToString("dd-MMM-yyyy");
        }
    }
}
