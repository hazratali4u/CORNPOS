using CORNCommon.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Forms_frmTableBill : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        Response.Cache.SetNoStore();
        Response.AppendHeader("pragma", "no-cache");
        if (!IsPostBack)
        {
            Configuration.SystemCurrentDateTime = (DateTime)Session["CurrentWorkDate"];
            txtDate.Text = Configuration.SystemCurrentDateTime.ToString("dd-MMM-yyyy");
            hidDistributorId.Value = Session["DISTRIBUTOR_ID"].ToString();
        }
    }
}