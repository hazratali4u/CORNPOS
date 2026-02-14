using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using CORNBusinessLayer.Classes;
using CORNBusinessLayer.Reports;
using CORNCommon.Classes;
using System.Globalization;
using System.Web;
using System.IO.Ports;
using DevExpress.Web;
using MKB.TimePicker;
using System.Drawing;

/// <summary>
/// From For Purchase, TranferOut, Purchase Return, TranferIn And Damage
/// </summary>
public partial class Forms_frmEcommWebImages : System.Web.UI.Page
{
    readonly DataControl _dc = new DataControl();
    readonly CompanyController _cc = new CompanyController();

   protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        Response.Cache.SetNoStore();
        Response.AppendHeader("pragma", "no-cache");

        if (!Page.IsPostBack)
        {
            LoadLookupGrid("");
            mPopUpLocation.Hide();
        }
    }

    //#region Grid Operations
    protected void GrdPurchase_RowEditing(object sender, GridViewEditEventArgs e)
    {
        //mPopUpLocation.Show();
        //_rowNo.Value = e.NewEditIndex.ToString();
        //cafeScheduleID.Value = Grid_users.Rows[e.NewEditIndex].Cells[1].Text;
        //ddlLocation.Value = Grid_users.Rows[e.NewEditIndex].Cells[2].Text;
        //if (!string.IsNullOrEmpty(Grid_users.Rows[e.NewEditIndex].Cells[5].Text) &&
        //    Grid_users.Rows[e.NewEditIndex].Cells[5].Text != "&nbsp;")
        //{
        //    ddlDays.Value = ddlDays.Items.FindByText(Grid_users.Rows[e.NewEditIndex].Cells[5].Text.Trim())
        //        .Value;
        //}
        //txtFromDate.Text = Convert.ToDateTime(Grid_users.Rows[e.NewEditIndex].Cells[6].Text).ToString("dd-MMM-yyyy");
        //txtToDate.Text = Convert.ToDateTime(Grid_users.Rows[e.NewEditIndex].Cells[7].Text).ToString("dd-MMM-yyyy");

        //int fromhour = Convert.ToInt32(Convert.ToDateTime(Grid_users.Rows[e.NewEditIndex].Cells[6].Text).Hour);
        //int fromMinute = Convert.ToInt32(Convert.ToDateTime(Grid_users.Rows[e.NewEditIndex].Cells[6].Text).Minute);

        //tsFrom.SetTime(fromhour, fromMinute, tsFrom.AmPm);

        //int tohour = Convert.ToInt32(Convert.ToDateTime(Grid_users.Rows[e.NewEditIndex].Cells[7].Text).Hour);
        //int toMinute = Convert.ToInt32(Convert.ToDateTime(Grid_users.Rows[e.NewEditIndex].Cells[7].Text).Minute);

        //tsFrom.SetTime(tohour, toMinute, tsTo.AmPm);

        //chkIsTemporaryClosed.Checked = Convert.ToBoolean(Grid_users.Rows[e.NewEditIndex].Cells[8].Text);
        //txtMessage.Text = Grid_users.Rows[e.NewEditIndex].Cells[3].Text;
        //btnSaveDocument.Text = "Update";

        //if (chkIsTemporaryClosed.Checked == true)
        //{
        //    ceDiv.Attributes.Add("style", "display:block;margin-top: 25px;");
        //    fromDateDiv.Attributes.Add("style", "display:block");
        //    ceToDiv.Attributes.Add("style", "display:block;margin-top: 25px;");
        //    toDateDiv.Attributes.Add("style", "display:block");
        //    dayDiv.Attributes.Add("style", "display:none;");
        //}
        //else
        //{
        //    ceDiv.Attributes.Add("style", "display:none;margin-top: 25px;");
        //    fromDateDiv.Attributes.Add("style", "display:none");
        //    ceToDiv.Attributes.Add("style", "display:none;margin-top: 25px;");
        //    toDateDiv.Attributes.Add("style", "display:none");
        //    dayDiv.Attributes.Add("style", "display:block;");
        //}

    }

    protected void GrdPurchase_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
    }
    protected void LoadLookupGrid(string pType)
    {
        Grid_users.DataSource = null;
        Grid_users.DataBind();
        //if (ddlLocation.Items.Count > 0)
        //{
            DataTable dt = _cc.SelectEcommWebImages();
            if (pType == "")
            {
                if (txtSearch.Text != "" || txtSearch.Text != string.Empty)
                {
                    dt.DefaultView.RowFilter = "DISTRIBUTOR_NAME LIKE '%" + txtSearch.Text + "%' OR USER_NAME LIKE '%" + txtSearch.Text + "%'  OR LOGIN_ID LIKE '%" + txtSearch.Text + "%'  OR PASSWORD LIKE '%" + txtSearch.Text + "%'  OR role_name LIKE '%" + txtSearch.Text + "%' OR IS_ACTIVE LIKE '" + txtSearch.Text + "%'";
                }
                Grid_users.DataSource = dt;
                Grid_users.DataBind();
            }
            else
            {
                if (txtSearch.Text != "" || txtSearch.Text != string.Empty)
                {
                    dt.DefaultView.RowFilter = "DISTRIBUTOR_NAME LIKE '%" + txtSearch.Text + "%' OR USER_NAME LIKE '%" + txtSearch.Text + "%'  OR LOGIN_ID LIKE '%" + txtSearch.Text + "%'  OR PASSWORD LIKE '%" + txtSearch.Text + "%'  OR role_name LIKE '%" + txtSearch.Text + "%' OR IS_ACTIVE LIKE '" + txtSearch.Text + "%'";
                }
                else
                {
                    dt.DefaultView.RowFilter = null;
                }
                if (dt.Rows.Count > 0)
                {
                    Grid_users.PageIndex = 0;
                }
                Grid_users.DataSource = dt;
                Grid_users.DataBind();
            }
        //}
    }

    protected void Grid_users_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
          // ((LinkButton)e.Row.FindControl("btnEdit")).OnClientClick = "return HideUnhideFields("+e+ "',' " + chkIsTemporaryClosed+ ");";
        }
    }
    protected void Grid_users_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_users.PageIndex = e.NewPageIndex;
        LoadLookupGrid("");
    }
    //#endregion

    //#region Click OPerations

    protected void btnFilter_Click(object sender, EventArgs e)
    {
    }

    protected void btnSave_Document(object sender, EventArgs e)
    {
        try
        {
            mPopUpLocation.Show();

            if (Page.IsValid)
            {
                bool flag = true;


                ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Record added successfully.');", true);
                flag = true;
                if (flag)
                {
                    mPopUpLocation.Hide();
                    LoadLookupGrid("");
                    btnSaveDocument.Text = "Save";
                }
            }
            else
            {
                mPopUpLocation.Show();
            }
        }
        catch (Exception ex)
        {
            ExceptionPublisher.PublishException(ex);
            ScriptManager.RegisterStartupScript(this, typeof(Page), "CatchMsg", "alert('" + ex.Message.ToString() + "')", true);
            mPopUpLocation.Show();
        }
    }

    protected void btnClose_Click(object sender, EventArgs e)
    {
        btnSaveDocument.Text = "Save";
        mPopUpLocation.Hide();

    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        mPopUpLocation.Show();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        //LoadGird();
    }

    //#endregion

}