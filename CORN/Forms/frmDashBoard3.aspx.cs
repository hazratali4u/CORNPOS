using System;
using System.IO;
using System.Data;
using DevExpress.DashboardCommon;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;

public partial class Forms_frmDashBoard3 : System.Web.UI.Page
{

    DataSet objDsMenu = null, objDsDashboard = null;
    Int32 intMenuCode=0;

    String[] strDashboardProcName;
    String[] strDashboardNameInfo;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {   
            if (Session["dtFromDashboard"] != null)
            {
                txtFromDate.Text = Convert.ToDateTime(Session["dtFromDashboard"]).ToString("dd-MMM-yyyy");
                txtToDate.Text = Convert.ToDateTime(Session["dtToDashboard"]).ToString("dd-MMM-yyyy");                
            }
            else
            {
                txtFromDate.Text = Convert.ToDateTime(Session["CurrentWorkDate"]).ToString("dd-MMM-yyyy");
                txtToDate.Text = Convert.ToDateTime(Session["CurrentWorkDate"]).ToString("dd-MMM-yyyy");
            }
        }
        intMenuCode = Convert.ToInt32(Request.QueryString["LevelID"].ToString());
        LoadItems();
        GetDashboard();
    }

    protected void dshViewer_DashboardLoading(object sender, DevExpress.DashboardWeb.DashboardLoadingEventArgs e)
    {
        try
        {
            if (objDsMenu.Tables[0].Rows[0]["vbrFile"] != null && !String.IsNullOrEmpty(Convert.ToString(objDsMenu.Tables[0].Rows[0]["vbrFile"])))
            {
                using (Stream ms = new MemoryStream((byte[])objDsMenu.Tables[0].Rows[0]["vbrFile"]))
                {
                    ms.Seek(0L, SeekOrigin.Begin);
                    using (StreamReader streamReader = new StreamReader(ms))
                        e.DashboardXml = streamReader.ReadToEnd();
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void dshViewer_DataLoading(object sender, DevExpress.DashboardWeb.DataLoadingWebEventArgs e)
    {
        try
        {
            e.Data = objDsDashboard.Tables[e.DataSourceName];
            
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void GetDashboard()
    {
        try
        {
            Session.Add("dtFromDashboard", Convert.ToDateTime(txtFromDate.Text));
            Session.Add("dtToDashboard", Convert.ToDateTime(txtToDate.Text));

            try
            {
                Session.Add("SKUID", Convert.ToInt32(ddlItem.SelectedItem.Value));
            }
            catch (Exception)
            {
            }

            objDsMenu = CornPointHelper.DashboardClass.GetDashboardMenuInfo(intMenuCode);
            objDsDashboard = CornPointHelper.DashboardClass.GetDashboardDataSet(objDsMenu.Tables[0].Rows[0]["strDashboardMainProcName"].ToString(), Convert.ToDateTime(Session["dtFromDashboard"]), Convert.ToDateTime(Session["dtToDashboard"]),Convert.ToInt32(Session["SKUID"]));
            strDashboardProcName = objDsMenu.Tables[0].Rows[0]["strDashboardProcName"].ToString().Split(',');
            strDashboardNameInfo = objDsMenu.Tables[0].Rows[0]["strDashboardNameInfo"].ToString().Split(',');
            dshViewer.DashboardId = strDashboardNameInfo[0];
            for (Int32 intCount = 0; intCount < objDsDashboard.Tables.Count; intCount++)
            {
                objDsDashboard.Tables[intCount].TableName = strDashboardNameInfo[intCount];
            }

            dshViewer.AllowExportDashboard = true;
            dshViewer.AllowExportDashboardItems = true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void LoadItems()
    {
        try
        {
            SkuController mSKUController = new SkuController();
            DataTable dt = mSKUController.SelectSkuInfo(Constants.IntNullValue, Constants.IntNullValue,Constants.IntNullValue, 12, int.Parse(Session["CompanyId"].ToString()), null);
            ddlItem.Items.Add(new DevExpress.Web.ListEditItem("All", "0"));
            clsWebFormUtil.FillDxComboBoxList(ddlItem, dt, "SKU_ID", "SKU_NAME");
            if (ddlItem.Items.Count > 0)
            {
                if (Session["SKUID"] != null)
                {
                    try
                    {
                        ddlItem.SelectedItem.Value = Session["SKUID"].ToString();
                    }
                    catch (Exception)
                    {
                        ddlItem.SelectedIndex = 0;
                    }
                }
                else
                {
                    ddlItem.SelectedIndex = 0;
                }
            }
        }
        catch (Exception)
        {
        }
    }
    protected void btnLoad_Click(object sender, EventArgs e)
    {
        GetDashboard();
    }    
}