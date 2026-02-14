using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public partial class LastDayClose : System.Web.UI.Page
{
    string conString = System.Configuration.ConfigurationManager.AppSettings["connString"].ToString();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadData(conString);
        }
    }
    public void LoadData(string conString)
    {
        DataTable dt = GetData(7, conString);
        DataTable dtLastDayClose = new DataTable();
        dtLastDayClose.Columns.Add("SNo", typeof(int));
        dtLastDayClose.Columns.Add("dbName", typeof(string));
        dtLastDayClose.Columns.Add("LastDayClose", typeof(DateTime));

        DataRow drDayClose;
        int SNo = 1;
        foreach (DataRow dr in dt.Rows)
        {
            drDayClose = dtLastDayClose.NewRow();
            drDayClose["SNo"] = SNo;
            drDayClose["dbName"] = dr["dbName"];
            drDayClose["LastDayClose"] = dr["LastDayClose"];
            dtLastDayClose.Rows.Add(drDayClose);
            SNo++;
        }
        gvLicnes.DataSource = dtLastDayClose;
        gvLicnes.DataBind();
    }
    public DataTable GetData(int pTypeID,string conString)
    {
        DataSet ds = new DataSet();
        try
        {

            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "uspGetDataUtility";
                    cmd.CommandType = CommandType.StoredProcedure;

                    IDataParameterCollection pparams = cmd.Parameters;

                    IDataParameter parameter = new SqlParameter() { ParameterName = "@DISTRIBUTOR_ID", DbType = DbType.Int32, Value = 0 };
                    pparams.Add(parameter);

                    parameter = new SqlParameter() { ParameterName = "@TypeID", DbType = DbType.Int32, Value = pTypeID };
                    pparams.Add(parameter);

                    parameter = new SqlParameter() { ParameterName = "@DocumentID", DbType = DbType.Int64, Value = 0 };
                    pparams.Add(parameter);

                    IDbDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    da.Fill(ds);
                    return ds.Tables[0];
                }
                con.Close();
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}