using DevExpress.DashboardCommon;
using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CORNDashBoard
{
    public partial class frmMain : Form
    {
        DataSet objDsMenu = null, objDsDashboard = null;
        DataSource objDataSource = null;
        Int32 intMenuCode = 384;

        String strdashboardFilePath = Application.StartupPath + "Dashboard.xml";
        bool bolIsDesignerAllowed = true, bolIsAdminUser = true;
        String strXMLFile;
        byte[] xmlByteArray;
        string conString = System.Configuration.ConfigurationSettings.AppSettings["conString"].ToString();
        public frmMain()
        {
            InitializeComponent();
        }
        public frmMain(Int32 intMenuCode, Boolean bolIsAdminUser)
        {
            InitializeComponent();
            this.intMenuCode = intMenuCode;
            this.bolIsAdminUser = bolIsAdminUser;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            try
            {
                objDsMenu = DashboardClass.GetDashboardMenuInfo(intMenuCode,conString);
                String[] strDashboardProcName = objDsMenu.Tables[0].Rows[0]["strDashboardProcName"].ToString().Split(',');
                String[] strDashboardNameInfo = objDsMenu.Tables[0].Rows[0]["strDashboardNameInfo"].ToString().Split(',');

                objDsDashboard = DashboardClass.GetDashboardDataSet(strDashboardProcName[0],System.DateTime.Now.AddDays(-50),System.DateTime.Now.AddDays(-45),conString);

                for (Int32 intCount = 0; intCount < objDsDashboard.Tables.Count; intCount++)
                {
                    objDsDashboard.Tables[intCount].TableName = strDashboardNameInfo[intCount];
                    objDataSource = new DataSource(strDashboardNameInfo[intCount], objDsDashboard.Tables[intCount]);
                    dashboardDesigner.Dashboard.DataSources.Add(objDataSource);
                }

                Dashboard dshboard = new Dashboard();
                if (objDsMenu.Tables[0].Rows[0]["vbrFile"] != null && !String.IsNullOrEmpty(Convert.ToString(objDsMenu.Tables[0].Rows[0]["vbrFile"])))
                {
                    using (Stream ms = new MemoryStream((byte[])objDsMenu.Tables[0].Rows[0]["vbrFile"]))
                    {
                        ms.Seek(0L, SeekOrigin.Begin);
                        dshboard.LoadFromXml(ms);
                        //dshViewer.Dashboard = dshboard;
                        dashboardDesigner.Dashboard = dshboard;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }        
        
        private void dashboardDesigner_DataLoading(object sender, DataLoadingEventArgs e)
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

        private void dashboardDesigner_DashboardSaving(object sender, DevExpress.DashboardWin.DashboardSavingEventArgs e)
        {
            MemoryStream ms = new MemoryStream();
            dashboardDesigner.Dashboard.SaveToXml(ms);
            byte[] dashboardFile = ms.ToArray();
            
            
            try
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    using (var sqlWrite = new SqlCommand("UPDATE MODULE SET vbrFile = @File WHERE MODULE_ID=" + intMenuCode, con))
                    {
                        sqlWrite.Parameters.Add("@File", SqlDbType.VarBinary, dashboardFile.Length).Value = dashboardFile;
                        sqlWrite.ExecuteNonQuery();
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}