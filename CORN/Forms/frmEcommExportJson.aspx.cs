
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using CORNCommon.Classes;
using CORNBusinessLayer.Classes;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using Amazon.S3;
using Amazon;
using Amazon.S3.Transfer;
using Amazon.Runtime;

/// <summary>
/// From to Assign, UnAssign Forms To Users
/// </summary>
public partial class frmEcommExportJson : System.Web.UI.Page
{
    protected string allowed_extensions = "gif|jpeg|jpg|png";
    readonly SkuController mController = new SkuController();
    readonly DataControl dc = new DataControl();
    readonly string apiURL = "https://api.cornpos.com/";
    readonly string apiConnectionString = "mSPXAGPP+OfVh6xuSX9sVbyU0uOwDJy+K6SkoNPIg2OYeaFgzsQkt0sq9lxlEVQDtcPp+f5LxbpyqhefwxzGLahWZ0sWlL1Zx63rbiE4+jI=";

    /// <summary>
    /// Page_Load Function Populates All Combos and ListBox On The Page
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
            GetAppSettingDetail();
            DataTable dtConfig = (DataTable)Session["dtAppSettingDetail"];
            if (dtConfig.Rows.Count > 0)
            {
                txtAccessKeyID.Text = dtConfig.Rows[0]["AWSAccessKeyID"].ToString();
                txtSecretAccessKey.Text = dtConfig.Rows[0]["AWSSecretAccessKeyID"].ToString();
                txtBucketName.Text = dtConfig.Rows[0]["AWSBucketName"].ToString();
            }
        }
    }
    protected void btnSaveCategory_Click(object sender, EventArgs e)
    {
        try
        {
            string s3DirectoryName = "DataJSON";
            string file_path = Server.MapPath("../Files/Ecommerce Json");
            if (!Directory.Exists(file_path))
            {
                Directory.CreateDirectory(file_path);
            }

            //Branches Json
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("x-conn", apiConnectionString);
                client.BaseAddress = new Uri(apiURL);
                var response = client.GetAsync("api/Branch/GetBranches").Result;
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;

                    string json = JsonConvert.SerializeObject(result, Formatting.Indented);

                    string branchesPath = Path.Combine(file_path + "/Branches.json");
                    if (File.Exists(branchesPath))
                    {
                        File.Delete(branchesPath);
                    }

                    using (StreamWriter sw = File.CreateText(branchesPath))
                    {
                        sw.Write(json);
                    }

                    var byteArray = File.ReadAllBytes(branchesPath);
                    //byte[] byteArray = Encoding.UTF8.GetBytes(bytes);


                    string s3FileName = "Branches.Json";
                    Stream stream = new MemoryStream(byteArray);

                    bool a;
                    a = sendMyFileToS3(stream, txtBucketName.Text, s3DirectoryName, s3FileName,
                        txtAccessKeyID.Text, txtSecretAccessKey.Text);
                    if (a == true)
                    {
                        stream.Dispose();
                        stream.Close();
                        System.IO.File.Delete(branchesPath);

                    }
                    else
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "CatchMsg", "alert('Some error occured');", true);
                }
            }

            //Categories JSON
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("x-conn", apiConnectionString);
                client.BaseAddress = new Uri(apiURL);
                var response = client.GetAsync("api/Category/GetCategoriesList").Result;
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;

                    string json = JsonConvert.SerializeObject(result, Formatting.Indented);

                    string categoriesPath = Path.Combine(file_path + "/Categories.json");
                    if (File.Exists(categoriesPath))
                    {
                        File.Delete(categoriesPath);
                    }

                    using (StreamWriter sw = File.CreateText(categoriesPath))
                    {
                        sw.Write(json);
                    }

                    var byteArray = File.ReadAllBytes(categoriesPath);
                    //byte[] byteArray = Encoding.UTF8.GetBytes(bytes);


                    string s3FileName = "Categories.Json";
                    Stream stream = new MemoryStream(byteArray);

                    bool a;
                    a = sendMyFileToS3(stream, txtBucketName.Text, s3DirectoryName, s3FileName,
                        txtAccessKeyID.Text, txtSecretAccessKey.Text);
                    if (a == true)
                    {
                        stream.Dispose();
                        stream.Close();
                        System.IO.File.Delete(categoriesPath);

                    }
                    else
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "CatchMsg", "alert('Some error occured');", true);
                }
            }
            ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "Message('Json exported successfully')", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "CatchMsg", "alert('" + ex.Message.ToString() + "');", true);
        }
    }
    public bool sendMyFileToS3(System.IO.Stream localFilePath, string bucketName, string subDirectoryInBucket,
        string fileNameInS3, string accessKeyID, string secretAccessKeyID)
    {
        IAmazonS3 client = new AmazonS3Client(accessKeyID, secretAccessKeyID, RegionEndpoint.USEast1);
        TransferUtility utility = new TransferUtility(client);
        TransferUtilityUploadRequest request = new TransferUtilityUploadRequest();

        if (subDirectoryInBucket == "" || subDirectoryInBucket == null)
        {
            request.BucketName = bucketName; //no subdirectory just bucket name  
        }
        else
        {   // subdirectory and bucket name  
            request.BucketName = bucketName + @"/" + subDirectoryInBucket;
        }
        request.Key = fileNameInS3; //file name up in S3  
        request.InputStream = localFilePath;
        utility.Upload(request); //commensing the transfer  

        return true; //indicate that the file was sent  
    }
    public void GetAppSettingDetail()
    {
        try
        {
            AppSettingDetail _cController = new AppSettingDetail();
            DataTable dtAppSetting = _cController.GetAppSettingDetail(1);
            if (dtAppSetting.Rows.Count > 0)
            {
                Session.Add("dtAppSettingDetail", dtAppSetting);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg3", "alert('Error Occured: \n" + ex + "');", true);
        }
    }
}
