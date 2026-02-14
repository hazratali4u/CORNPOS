using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using CORNBusinessLayer.Classes;
using CORNBusinessLayer.Reports;
using CORNCommon.Classes;
using System.Web;

public partial class Forms_frmPromotionWizard : System.Web.UI.Page
{
    string[] cols = { "PROMOTION_ID", "DISTRIBUTOR_ID", "PROMOTION_CODE", "PROMOTION_DESCRIPTION", "START_DATE", "END_DATE", "IS_ACTIVE" };
    /// <summary>
    /// Page_Load Function
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
            Configuration.SystemCurrentDateTime = (DateTime)Session["CurrentWorkDate"];
            txtStartDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtEndDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            txtStartDate.Attributes.Add("readonly", "readonly");
            txtEndDate.Attributes.Add("readonly", "readonly");
        }
    }

    protected void btnGetPromotion_Click(object sender, EventArgs e)
    {
        try
        {
            PromotionController mController = new PromotionController();
            string FromDate = txtStartDate.Text + " 00:00:00";
            string ToDate = txtEndDate.Text + " 23:59:59";
            DataTable dt = mController.SelectPromotion(FromDate, ToDate, Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), cbActive.Checked);
            this.Session.Add("dt", dt);
            DataView dw = dt.DefaultView;
            DataTable dt2 = dw.ToTable(true, cols);
            Grid_pricedetails.DataSource = dt2;
            Grid_pricedetails.DataBind();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('" + ex.ToString() + "');", true);
        }
    }

    protected void btnNewPromotion_Click(object sender, EventArgs e)
    {
        this.Session.Add("IsEdit", false);
        Response.Redirect("frmPromotionWizardSetp1.aspx?LevelType=3&LevelID=" + Request.QueryString["LevelID"].ToString(), true);
    }

    protected void Grid_pricedetails_RowEditing(object sender, GridViewEditEventArgs e)
    {
        bool IsEditing = true;
        string flow = "f";
        string PromotionId = Grid_pricedetails.Rows[e.NewEditIndex].Cells[0].Text;
        this.Session.Add("PromotionId", PromotionId);
        this.Session.Add("IsEdit", IsEditing);
        this.Session.Add("Flow", flow);
        Response.Redirect("frmPromotionWizardSetp1.aspx?LevelType=3&LevelID=" + Request.QueryString["LevelID"].ToString(), true);
    }

    protected void Grid_pricedetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        PromotionController mController = new PromotionController();
        int PromotionId = int.Parse(Grid_pricedetails.Rows[e.RowIndex].Cells[0].Text);
        int SchemeId = 1;
        mController.UpdatePromotion(PromotionId, SchemeId, int.Parse(this.Session["DISTRIBUTOR_ID"].ToString()), null, null, Constants.DateNullValue, false, Constants.DateNullValue, Constants.DateNullValue, false, Constants.IntNullValue, Constants.IntNullValue);
        string FromDate = txtStartDate.Text + " 00:00:00";
        string ToDate = txtEndDate.Text + " 23:59:59";
        DataTable dt = mController.SelectPromotion(FromDate, ToDate, 1, int.Parse(this.Session["UserId"].ToString()), cbActive.Checked);
        this.Session.Add("dt", dt);
        DataView dw = dt.DefaultView;
        DataTable dt2 = dw.ToTable(true, cols);
        Grid_pricedetails.DataSource = dt2;
        Grid_pricedetails.DataBind();
    }

    protected void btnFilter_Click(object sender, EventArgs e)
    {
        if (txtSeach.Text.Length > 0)
        {
            LoadGrid();
        }
    }
    
    private void LoadGrid()
    {
        DataTable dt = (DataTable)this.Session["dt"];
        if (ddSearchType.SelectedIndex == 0)
        {
            dt.DefaultView.RowFilter = "1=1";
        }
        else if (ddSearchType.SelectedIndex == 1)
        {
            dt.DefaultView.RowFilter = ddSearchType.Value.ToString() + " =" + txtSeach.Text;
        }
        else
        {
            dt.DefaultView.RowFilter = ddSearchType.Value.ToString() + " like '%" + txtSeach.Text + "%'";
        }
        DataView dw = dt.DefaultView;
        DataTable dt2 = dw.ToTable(true, cols);
        Grid_pricedetails.DataSource = dt2;
        Grid_pricedetails.DataBind();
    }
}