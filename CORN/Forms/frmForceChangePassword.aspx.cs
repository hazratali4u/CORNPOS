using System;
using System.Data;
using System.Web;
using System.Web.UI;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;

/// <summary>
/// Form To Change User Password
/// </summary>
public partial class Forms_frmForceChangePassword : System.Web.UI.Page
{
    DataTable dtLogin_ID = new DataTable();
    
    /// <summary>
    /// Page_Load Function
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        UserController UserInfo = new UserController();
        //Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
        //Response.Cache.SetNoStore();
        //Response.AppendHeader("pragma", "no-cache");


        string Id = Request.QueryString["id"];

        txtCurrentPassword.Attributes["value"] = txtCurrentPassword.Text;
        txtNewPassword.Attributes["value"] = txtNewPassword.Text;
        txtConfirmNewPassword.Attributes["value"] = txtConfirmNewPassword.Text;

        if (!Page.IsPostBack)
        {
            GetAppSettingDetail();
            dtLogin_ID = UserInfo.SelectSlashUser(Convert.ToInt32(Id));
            this.txtCurrentPassword.Text = dtLogin_ID.Rows[0]["PASSWORD"].ToString();

            btnSave.Attributes.Add("onclick", "return ValidatePassword();");            
        }
    }
    
    /// <summary>
    /// Updates User Password
    /// </summary>
    /// <remarks>
    /// Returns True on Success And False on Failure
    /// </remarks>
    /// <returns>True on Success And False on Failure</returns>
    protected bool UpdatePassword()
    {
        UserController UserInfo = new UserController();
        try
        {
            string Id = Request.QueryString["id"];
            dtLogin_ID = UserInfo.SelectSlashUser(Convert.ToInt32(Id));
            if (txtCurrentPassword.Text == dtLogin_ID.Rows[0]["PASSWORD"].ToString())
            {
                if (txtCurrentPassword.Text == txtNewPassword.Text)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Current password is same as new password ');", true);
                    return false;
                }
                txtConfirmNewPassword.Text = CheckDeplyed();
                try
                {
                    string result = UserInfo.UpdatePassword(Convert.ToInt32(Id)
                         , dtLogin_ID.Rows[0]["LOGIN_ID"].ToString(), txtConfirmNewPassword.Text);

                    if (result == "true")
                    {
                       Response.Redirect("~/Login.aspx");
                    }
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catchmsg", "alert('" + ex.Message.ToString() + "');", true);
                    return false;
                }

                return true;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Current Password is wrong ');", true);
                return false;
            }

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catchmsg2", "alert('" + ex.Message.ToString() + "');", true);
            return false;
        }
    }
    private string CheckDeplyed()
    {
        DataTable dtConfig = (DataTable)Session["dtAppSettingDetail"];
        bool IsEncrypted = false;
        if(dtConfig.Rows[0]["IsEncreptedCredentials"].ToString() == "1")
        {
            IsEncrypted = true;
        }
        if (IsEncrypted)
        {
            if (dtConfig.Rows[0]["Deployed"].ToString() == Cryptography.Encrypt("Deployed", "b0tin@74"))
            {
                txtConfirmNewPassword.Attributes["type"] = "text";
                return Cryptography.Encrypt(txtConfirmNewPassword.Text, "b0tin@74");
            }
            else
            {
                return txtConfirmNewPassword.Text;
            }
        }
        else
        {
            return txtConfirmNewPassword.Text;
        }
    }

    /// <summary>
    /// Updates User Password Through UpdatePassword() Function
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        bool status = UpdatePassword();

        if (status == true)
        {
            txtCurrentPassword.Attributes["value"] = "";
            txtNewPassword.Attributes["value"] = "";
            txtConfirmNewPassword.Attributes["value"] = "";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Password has changed successfully ');", true);
        }
    }

    /// <summary>
    /// Clears Form Controls
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        txtNewPassword.Attributes.Add("value", "");
        txtCurrentPassword.Attributes.Add("value", "");
        txtConfirmNewPassword.Attributes.Add("value", "");
        //Response.Redirect("~/Forms/Home.aspx");
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