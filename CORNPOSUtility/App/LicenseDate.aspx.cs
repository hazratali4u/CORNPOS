using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public partial class LastDayClose : System.Web.UI.Page
{
    string CryptographyKey = "b0tin@74";
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
        DataTable dt = GetData(8, conString);
        DataTable dtLastDayClose = new DataTable();
        dtLastDayClose.Columns.Add("dbName", typeof(string));
        dtLastDayClose.Columns.Add("LocationName", typeof(string));
        dtLastDayClose.Columns.Add("LastDayClose", typeof(DateTime));
        dtLastDayClose.Columns.Add("LicenseDate", typeof(DateTime));

        DataRow drDayClose;
        foreach (DataRow dr in dt.Rows)
        {
            drDayClose = dtLastDayClose.NewRow();
            drDayClose["dbName"] = dr["dbName"];
            drDayClose["LocationName"] = dr["LocationName"];
            drDayClose["LastDayClose"] = dr["LastDayClose"];
            try
            {
                drDayClose["LicenseDate"] = Convert.ToDateTime(Decrypt(dr["LicenseDate"].ToString(), CryptographyKey)).ToString("dd-MMM-yyyy");
            }
            catch (Exception ex)
            {
                drDayClose["LicenseDate"] = Convert.ToDateTime(Decrypt(DateTime.Now.ToShortDateString(), CryptographyKey)).ToString("dd-MMM-yyyy");
            }
            dtLastDayClose.Rows.Add(drDayClose);
        }
        dtLastDayClose.DefaultView.Sort = "LicenseDate DESC";
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
    public static string Decrypt(string EncryptedText, string Key)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(Key);
        byte[] rgbIV = Encoding.UTF8.GetBytes(Key);

        byte[] buffer = Convert.FromBase64String(EncryptedText);
        MemoryStream stream = new MemoryStream();
        try
        {
            DES des = new DESCryptoServiceProvider();
            CryptoStream stream2 = new CryptoStream(stream, des.CreateDecryptor(bytes, rgbIV), CryptoStreamMode.Write);
            stream2.Write(buffer, 0, buffer.Length);
            stream2.FlushFinalBlock();
        }
        catch (Exception ex)
        {

        }



        return Encoding.UTF8.GetString(stream.ToArray());
    }
    protected void gvLicnes_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Text = e.Row.RowIndex.ToString();
        }
    }
}