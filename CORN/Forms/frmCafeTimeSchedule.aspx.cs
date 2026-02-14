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
public partial class Forms_frmCafeTimeSchedule : System.Web.UI.Page
{
    readonly DataControl _dc = new DataControl();
    readonly CustomerDataController _cc = new CustomerDataController();

   protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        Response.Cache.SetNoStore();
        Response.AppendHeader("pragma", "no-cache");

        if (!Page.IsPostBack)
        {
            Session.Remove("dtGridData");
            SetInitialRow();
            LoadLocations();
            ddlLocation.Focus();
            LoadDays();
            LoadGridData();
            LoadLookupGrid("");
            mPopUpLocation.Hide();
            txtFromDate.Text = DateTime.Now.Date.ToString("dd-MMM-yyyy");
            txtToDate.Text = DateTime.Now.Date.ToString("dd-MMM-yyyy");
        }
    }

    #region Load
    private void LoadLocations()
    {
        DistributorController DController = new DistributorController();
        DataTable dt = DController.GetDistributorWithMaxDayClose(Constants.IntNullValue, int.Parse(this.Session["UserId"].ToString()), int.Parse(this.Session["CompanyId"].ToString()), 1);

        clsWebFormUtil.FillDxComboBoxList(ddlLocation, dt, "DISTRIBUTOR_ID", "DISTRIBUTOR_NAME");
        clsWebFormUtil.FillDxComboBoxList(ddlLocation1, dt, "DISTRIBUTOR_ID", "DISTRIBUTOR_NAME");

        if (dt.Rows.Count > 0)
        {
            ddlLocation.SelectedIndex = 0;
            ddlLocation1.SelectedIndex = 0;
        }
        Session.Add("dtLocationInfo", dt);
    }

    private void LoadDays()
    {
        try
        {
            var dt = new DataTable();
            dt.Columns.Add("Day_ID", typeof(int));
            dt.Columns.Add("Day_Name", typeof(string));

            DataRow dr = dt.NewRow();
            dr["Day_ID"] = 1;
            dr["Day_Name"] = "Monday";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Day_ID"] = 2;
            dr["Day_Name"] = "Tuesday";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Day_ID"] = 3;
            dr["Day_Name"] = "Wednesday";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Day_ID"] = 4;
            dr["Day_Name"] = "Thursday";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Day_ID"] = 5;
            dr["Day_Name"] = "Friday";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Day_ID"] = 6;
            dr["Day_Name"] = "Saturday";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Day_ID"] = 7;
            dr["Day_Name"] = "Sunday";
            dt.Rows.Add(dr);

            PopulateScheduleGrid(int.Parse(ddlLocation.SelectedItem.Value.ToString()), dt);
            //clsWebFormUtil.FillDxComboBoxList(ddlDays, dt, "Day_ID", "Day_Name");

            //if (dt.Rows.Count > 0)
            //{
            //    ddlDays.SelectedIndex = 0;
            //}
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg3", "alert('Error Occured: \n" + ex + "');", true);
        }
    }

    private void PopulateScheduleGrid(int locationId, DataTable dt)
    {
        DataTable dtCurrentTable = (DataTable)Session["TimeSchedule"];
        DataRow drCurrentRow = null;

        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                drCurrentRow = dtCurrentTable.NewRow();
                dtCurrentTable.Rows.Add(drCurrentRow);

                dtCurrentTable.Rows[i]["DayName"] = dt.Rows[i]["Day_Name"].ToString();

                //dtCurrentTable.Rows[i]["tsFrom"] = DateTime.Now.ToString();
                //dtCurrentTable.Rows[i]["tsTo"] = DateTime.Now.ToString();
            }

            dtCurrentTable.Rows[dtCurrentTable.Rows.Count - 1].Delete();
            Gridview1.DataSource = dtCurrentTable;
            Gridview1.DataBind();

            var ShiftController = new ShiftController();
            DataTable alreadySaved = ShiftController.SelectCafeTiming(locationId);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TimeSelector box1 = (TimeSelector)Gridview1.Rows[i].Cells[1].FindControl("tsFrom");
                TimeSelector box2 = (TimeSelector)Gridview1.Rows[i].Cells[2].FindControl("tsTo");

                int fromhour = Convert.ToInt32(DateTime.Now.Hour);
                int fromMinute = Convert.ToInt32(DateTime.Now.Minute);
                DateTime currentTime = DateTime.Parse(DateTime.Now.ToString());


                int tohour = Convert.ToInt32(DateTime.Now.Hour);
                int toMinute = Convert.ToInt32(DateTime.Now.Minute);
                DateTime tocurrentTime = DateTime.Parse(DateTime.Now.ToString());

                MKB.TimePicker.TimeSelector.AmPmSpec fromam_pm;
                MKB.TimePicker.TimeSelector.AmPmSpec toam_pm;

                if (alreadySaved.Rows.Count > 0 && alreadySaved.Rows[i] != null && alreadySaved.Rows.Count >= 7)
                {
                    DataRow[] dr = alreadySaved.Select("Day = '" + dt.Rows[i]["Day_Name"] + "'");

                    if (dr != null && dr[0] != null)
                    {
                        fromhour = Convert.ToInt32(Convert.ToDateTime(dr[0]["From"].ToString()).Hour);
                        fromMinute = Convert.ToInt32(Convert.ToDateTime(dr[0]["From"].ToString()).Minute);
                        currentTime = DateTime.Parse(Convert.ToDateTime(dr[0]["From"].ToString()).ToString());

                        tohour = Convert.ToInt32(Convert.ToDateTime(dr[0]["To"].ToString()).Hour);
                        toMinute = Convert.ToInt32(Convert.ToDateTime(dr[0]["To"].ToString()).Minute);
                        tocurrentTime = DateTime.Parse(Convert.ToDateTime(dr[0]["To"].ToString()).ToString());

                        if (currentTime.ToString("tt") == "AM")
                        {
                            fromam_pm = MKB.TimePicker.TimeSelector.AmPmSpec.AM;
                        }
                        else
                        {
                            fromam_pm = MKB.TimePicker.TimeSelector.AmPmSpec.PM;
                        }

                        if (tocurrentTime.ToString("tt") == "AM")
                        {
                            toam_pm = MKB.TimePicker.TimeSelector.AmPmSpec.AM;
                        }
                        else
                        {
                            toam_pm = MKB.TimePicker.TimeSelector.AmPmSpec.PM;
                        }

                        box1.SetTime(fromhour, fromMinute, fromam_pm);

                        box2.SetTime(tohour, toMinute, toam_pm);
                    }
                }
                else
                {
                    if (currentTime.ToString("tt") == "AM")
                    {
                        fromam_pm = MKB.TimePicker.TimeSelector.AmPmSpec.AM;
                    }
                    else
                    {
                        fromam_pm = MKB.TimePicker.TimeSelector.AmPmSpec.PM;
                    }

                    if (tocurrentTime.ToString("tt") == "AM")
                    {
                        toam_pm = MKB.TimePicker.TimeSelector.AmPmSpec.AM;
                    }
                    else
                    {
                        toam_pm = MKB.TimePicker.TimeSelector.AmPmSpec.PM;
                    }

                    box1.SetTime(fromhour, fromMinute, fromam_pm);

                    box2.SetTime(tohour, toMinute, toam_pm);
                }
            }
        }
    }

    private void SetInitialRow()
    {
        DataTable dt = new DataTable();
        DataRow dr = null;

        dt.Columns.Add(new DataColumn("DayName", typeof(string)));
        dt.Columns.Add(new DataColumn("tsFrom", typeof(string)));
        dt.Columns.Add(new DataColumn("tsTo", typeof(string)));

        dr = dt.NewRow();
        dr["DayName"] = string.Empty;
        dr["tsFrom"] = string.Empty;
        dr["tsTo"] = string.Empty;

        dt.Rows.Add(dr);

        Session["TimeSchedule"] = dt;
        Gridview1.DataSource = dt;
        Gridview1.DataBind();
    }

    #endregion

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
    private void LoadGridData()
    {
        DataTable dt = new DataTable();
        var ShiftController = new ShiftController();
        dt = ShiftController.SelectCafeTiming(Constants.IntNullValue);
        Session.Add("dtGridData", dt);
    }
    protected void LoadLookupGrid(string pType)
    {
        Grid_users.DataSource = null;
        Grid_users.DataBind();
        if (ddlLocation.Items.Count > 0)
        {
            DataTable dt = (DataTable)Session["dtGridData"];
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
        }
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

                foreach (GridViewRow dr in Gridview1.Rows)
                {
                    TimeSelector box1 = (TimeSelector)dr.Cells[1].FindControl("tsFrom");
                    TimeSelector box2 = (TimeSelector)dr.Cells[2].FindControl("tsTo");

                    var fromDate = DateTime.Now.Date.ToString("dd MMM yyyy") + " " + box1.Hour + ":" + box1.Minute + " " + box1.AmPm;
                    var toDate = DateTime.Now.ToString("dd MMM yyyy") + " " + box2.Hour + ":" + box2.Minute + " " + box2.AmPm;

                    var shiftController = new ShiftController();
                    shiftController.InserCafeOpenCloseTiming(
                        Convert.ToInt32(ddlLocation.SelectedItem.Value), false,
                        dr.Cells[0].Text, null,
                        Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate));
                }

                if (chkIsTemporaryClosed.Checked)
                {
                    var fromDate1 = string.IsNullOrEmpty(txtFromDate.Text) ?
                        DateTime.Now.Date.ToString("dd MMM yyyy") : txtFromDate.Text;

                    fromDate1 = fromDate1 + " " + tsFrom1.Hour + ":" + tsFrom1.Minute + " " + tsFrom1.AmPm;

                    var toDate1 = string.IsNullOrEmpty(txtToDate.Text) ?
                        DateTime.Now.Date.ToString("dd MMM yyyy") : txtToDate.Text;

                    toDate1 = toDate1 + " " + tsTo1.Hour + ":" + tsTo1.Minute + " " + tsTo1.AmPm;

                    txtFromDate.Text = fromDate1;
                    txtToDate.Text = toDate1;

                    var shiftController = new ShiftController();
                    shiftController.InserCafeOpenCloseTiming(
                            Convert.ToInt32(ddlLocation1.SelectedItem.Value), true,
                            "", txtMessage.Text,
                            Convert.ToDateTime(txtFromDate.Text), Convert.ToDateTime(txtToDate.Text));
                }



                ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Record added successfully.');", true);
                flag = true;
                if (flag)
                {
                    mPopUpLocation.Hide();
                    LoadGridData();
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

    protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetInitialRow();
        mPopUpLocation.Show();
        LoadDays();
    }
    protected void ddlLocation1_SelectedIndexChanged(object sender, EventArgs e)
    {
        mPopUpLocation.Show();
    }

}