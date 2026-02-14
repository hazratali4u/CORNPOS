using System;
using System.Data;
using System.Web;
using System.Web.UI;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Web.Services;
using System.Linq;

public partial class Forms_frmCustomerFeedback : System.Web.UI.Page
{
    readonly CustomerFeedbackController cCustomerFeedback = new CustomerFeedbackController();
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        Response.Cache.SetNoStore();
        Response.AppendHeader("pragma", "no-cache");
        if (!Page.IsPostBack)
        {
            txtOther.Attributes.Add("readonly", "readonly");
        }
    }
    //=======Rates========
    // 1 = POOR
    // 2 = FAIR
    // 3 = GOOD
    // 4 = VERY GOOD
    // 5 = EXCELLENT 
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            DateTime currentWorkDate = DateTime.Parse(HttpContext.Current.Session["CurrentWorkDate"].ToString());
            cCustomerFeedback.InsertFeedBack(Convert.ToInt32(Session["DISTRIBUTOR_ID"]), 
                Convert.ToInt32(hfServiceRating.Value), Convert.ToInt32(hfFoodRating.Value), Convert.ToInt32(hfEnvironmentRating.Value),
                Convert.ToInt32(hfOverallRating.Value), txtComments.Text, FindRate("Hear"), txtOther.Text, 
                txtName.Text, txtContact.Text, txtEmail.Text, txtAddress.Text, currentWorkDate,
                FindRate("Return"), FindRate("Visit"), txtLikeSuggestion.Text,
                txtImproveSuggestion.Text, txtMenuSuggestion.Text, txtCity.Text);

            CustomerDataController.InsertCustomer(Convert.ToInt32(Session["DISTRIBUTOR_ID"]), "", "",
                "", txtContact.Text, txtEmail.Text, txtName.Text, "", null, 0, "", "", 0,0,0,0,0,0,0);
        }
        catch (Exception ex)
        {
            ExceptionPublisher.PublishException(ex);
        }
        clearAll();
        ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.Page), "ErrorMessage", "ShowPopup()", true);
    }
    public void clearAll()
    {        
        Hear1.Checked = false;
        Hear2.Checked = false;
        Hear3.Checked = false;
        Hear4.Checked = false;
        Hear5.Checked = false;
        Hear6.Checked = false;
        Hear7.Checked = false;

        Visit1.Checked = false;
        Visit2.Checked = false;
        Visit3.Checked = false;
        Visit4.Checked = false;
        Visit5.Checked = false;

        Return1.Checked = false;
        Return2.Checked = false;
        Return3.Checked = false;


        txtComments.Text = "";
        txtContact.Text = "";
        txtEmail.Text = "";
        txtName.Text = "";
        txtOther.Text = "";
        txtCity.Text = "";
        txtAddress.Text = "";
        txtImproveSuggestion.Text = "";
        txtLikeSuggestion.Text = "";
        txtMenuSuggestion.Text = "";
    }

    public int FindRate(String ListName)
    {
        if (ListName == "Hear")
        {
            if (Hear1.Checked)
            {
                return 1;
            }
            if (Hear2.Checked)
            {
                return 2;
            }
            if (Hear3.Checked)
            {
                return 3;
            }
            if (Hear4.Checked)
            {
                return 4;
            }
            if (Hear5.Checked)
            {
                return 5;
            }
            if (Hear6.Checked)
            {
                return 6;
            }
            if (Hear7.Checked)
            {
                return 7;
            }
            else
            {
                return 0;
            }
        }
        if (ListName == "Return")
        {
            if (Return1.Checked)
            {
                return 1;
            }
            if (Return2.Checked)
            {
                return 2;
            }
            if (Return3.Checked)
            {
                return 3;
            }
            else
            {
                return 0;
            }
        }
        if (ListName == "Visit")
        {
            if (Visit1.Checked)
            {
                return 1;
            }
            if (Visit2.Checked)
            {
                return 2;
            }
            if (Visit3.Checked)
            {
                return 3;
            }
            if (Visit4.Checked)
            {
                return 4;
            }
            if (Visit5.Checked)
            {
                return 5;
            }
            else
            {
                return 0;
            }
        }
        else
        {
            return 0;
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
    }

    [WebMethod]
    public static string GetDistributorInfo()
    {
        DistributorController mDController = new DistributorController();
        DataTable dt = mDController.SelectDistributor(Convert.ToInt32(HttpContext.Current.Session["DISTRIBUTOR_ID"]));
        return GetJson(dt);
    }

    public static string GetJson(DataTable dt)
    {
        System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        serializer.MaxJsonLength = Int32.MaxValue;
        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        Dictionary<string, object> row = null;

        foreach (DataRow dr in dt.Rows)
        {
            row = dt.Columns.Cast<DataColumn>().ToDictionary(col => col.ColumnName, col => dr[col]);
            rows.Add(row);
        }
        return serializer.Serialize(rows);
    }

}
