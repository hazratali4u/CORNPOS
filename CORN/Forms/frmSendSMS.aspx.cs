using GsmComm.GsmCommunication;
using GsmComm.PduConverter;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using CORNBusinessLayer.Classes;
using System.Data;
using CORNCommon.Classes;
using System.Linq;
using System.Xml;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;

/// <summary>
/// Form To Change User Password
/// </summary>
public partial class Forms_frmSendSMS : System.Web.UI.Page
{

   private readonly GeoHierarchyController _gCtl = new GeoHierarchyController();

    /// <summary>
    /// Page_Load Function
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.LoadCustomers();
            DataSet ds = _gCtl.SelectDataForPosLoad(int.Parse(Session["UserID"].ToString()),
                    int.Parse(Session["DISTRIBUTOR_ID"].ToString()), DateTime.Parse(Session["CurrentWorkDate"].ToString())
                    , int.Parse(Session["RoleID"].ToString()));
            if (ds.Tables[5].Rows.Count > 0)
            {
                Session.Add("dbSMSSetting", ds.Tables[5]);
            }
            GetSMSBalance();
        }
    }
    private void GetSMSBalance()
    {
        lblSMSBalance.Visible = false;
        string connString = System.Configuration.ConfigurationManager.AppSettings["DBAuthentication"].ToString() +
            "," + System.Configuration.ConfigurationManager.AppSettings["Server"].ToString()
            + "," + System.Configuration.ConfigurationManager.AppSettings["DBName"].ToString()
            + "," + System.Configuration.ConfigurationManager.AppSettings["DBUser"].ToString()
            + "," + System.Configuration.ConfigurationManager.AppSettings["DBPassword"].ToString();
        StringBuilder strMessage = new StringBuilder();
        DataTable dt = _gCtl.GetSMSSetting(Convert.ToInt32(Session["DISTRIBUTOR_ID"].ToString()));

        if (dt != null)
        {
            if (dt.Rows.Count > 0)
            {
                #region Jazz Mobilink
                if (dt.Rows[0]["TypeID"].ToString() == "1")//1= Jazz Mobilink
                {
                    String result = "";
                    StreamWriter myWriter = null;
                    HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(dt.Rows[0]["URL"].ToString() + "request_sms_check.html?Username=" + dt.Rows[0]["USERID"].ToString() + "&Password=" + dt.Rows[0]["PASSWORD"].ToString());
                    objRequest.Method = "POST";
                    objRequest.ContentLength = Encoding.UTF8.GetByteCount("");
                    objRequest.ContentType = "application/x-www-form-urlencoded";
                    try
                    {
                        myWriter = new StreamWriter(objRequest.GetRequestStream());
                        myWriter.Write("");
                    }
                    catch (Exception e)
                    {
                    }
                    finally
                    {
                        myWriter.Close();
                    }
                    HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
                    using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
                    {
                        result = sr.ReadToEnd();
                        sr.Close();
                    }
                    strMessage.Append(result);
                }
                #endregion
                #region Telenor 
                else if (dt.Rows[0]["TypeID"].ToString() == "2")//2= Telenor
                {
                    strMessage.Append("");
                }
                #endregion
                #region Outreach
                else if (dt.Rows[0]["TypeID"].ToString() == "3")//3= Outreach
                {
                    String result = "";
                    String strPost = "id=" + dt.Rows[0]["USERID"].ToString() + "&pass=" + dt.Rows[0]["PASSWORD"].ToString() + "&mask=" + dt.Rows[0]["MASK"].ToString() + "&type=xml&lang=English";
                    StreamWriter myWriter = null;
                    HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create("http://www.outreach.pk/api/sendsms.php/balance/status");

                    objRequest.Method = "POST";
                    objRequest.ContentLength = Encoding.UTF8.GetByteCount(strPost);
                    objRequest.ContentType = "application/x-www-form-urlencoded";
                    try
                    {
                        myWriter = new StreamWriter(objRequest.GetRequestStream());
                        myWriter.Write(strPost);
                    }
                    catch (Exception e)
                    {
                    }
                    finally
                    {
                        myWriter.Close();
                    }
                    HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
                    using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
                    {
                        result = sr.ReadToEnd();
                        sr.Close();
                    }

                    XmlDocument xmlDoc = new XmlDocument();
                    string myXML = result;
                    xmlDoc.LoadXml(myXML);

                    XmlNodeList parentNode = xmlDoc.GetElementsByTagName("corpsms");
                    foreach (XmlNode childrenNode in parentNode)
                    {
                        if(childrenNode.SelectSingleNode("type").InnerText.ToLower() == "success")
                        {
                            strMessage.Append(childrenNode.SelectSingleNode("response").InnerText.ToLower());
                            strMessage.Append("    |    Expiry : ");
                            strMessage.Append(childrenNode.SelectSingleNode("expiry").InnerText.ToLower());
                            break;
                        }
                        else
                        {
                            strMessage.Append(childrenNode.SelectSingleNode("response").InnerText.ToLower());
                        }
                    }
                    
                }
                #endregion
                #region Tech Garage Now
                else if (dt.Rows[0]["TypeID"].ToString() == "4")//4= Tech Garage Now
                {
                    String result = "";
                    String strPost = string.Format("?username={0}&password={1}", dt.Rows[0]["USERID"], dt.Rows[0]["PASSWORD"]);
                    StreamWriter myWriter = null;
                    HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}AccountBalance?username={1}&password={2}", dt.Rows[0]["URL"], dt.Rows[0]["USERID"], dt.Rows[0]["PASSWORD"]));
                    objRequest.Method = "POST";
                    objRequest.ContentLength = 0;
                    objRequest.ContentType = "application/x-www-form-urlencoded";
                    HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
                    using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
                    {
                        result = sr.ReadToEnd();
                        sr.Close();
                    }
                    strMessage.Append(result);
                }
                #endregion
                #region SMS Point
                else if (dt.Rows[0]["TypeID"].ToString() == "5")//5= SMS Point
                {
                    strMessage.Append("");
                }
                #endregion
                #region Thumb Crowd
                else if (dt.Rows[0]["TypeID"].ToString() == "6")//6= Thumb Crowd
                {
                    strMessage.Append("API does not support Balance");
                }
                #endregion
                #region BizSMS
                else if (dt.Rows[0]["TypeID"].ToString() == "7")//7= BizSMS
                {
                    String result = "";

                    StreamWriter myWriter = null;
                    HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(dt.Rows[0]["MESSAGE"].ToString() + "?username=" + dt.Rows[0]["USERID"].ToString() + "&pass=" + dt.Rows[0]["PASSWORD"].ToString());

                    objRequest.Method = "POST";
                    objRequest.ContentLength = Encoding.UTF8.GetByteCount("");
                    objRequest.ContentType = "application/x-www-form-urlencoded";
                    try
                    {
                        myWriter = new StreamWriter(objRequest.GetRequestStream());
                        myWriter.Write("");
                    }
                    catch (Exception e)
                    {
                    }
                    finally
                    {
                        myWriter.Close();
                    }
                    HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
                    using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
                    {
                        result = sr.ReadToEnd();
                        sr.Close();
                    }
                    result = (System.Xml.Linq.XElement.Parse(result)).Value;
                    strMessage.Append(result.Substring(21));
                }
                #endregion
            }
        }
        if (strMessage.ToString().Length > 0)
        {
            lblSMSBalance.Text = "SMS Balance : " + strMessage.ToString();
            lblSMSBalance.Visible = true;
        }
    }
    private void LoadCustomers()
    {
        LstCustomer.Items.Clear();
        var mController = new CustomerDataController();
        DataTable dtCustomers = mController.UspSelectCustomer(Constants.IntNullValue, "SMS", "", DateTime.Parse(HttpContext.Current.Session["CurrentWorkDate"].ToString()));
        clsWebFormUtil.FillListBox(LstCustomer, dtCustomers, 0, 3, false);

        lblTotalCustomer.Text = "Total Customers : " + dtCustomers.Rows.Count.ToString();
    }
    protected void btnSendSMS_Click(object sender, EventArgs e)
    {
        DataTable dbSMSSetting = (DataTable)Session["dbSMSSetting"];
        if (dbSMSSetting == null)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Please Configure SMS setting for this location');", true);
            return;
        }
        else if (dbSMSSetting.Rows.Count < 1)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Please Configure SMS setting for this location');", true);
            return;
        }
        lblError.Text = string.Empty;

        for (int i = 0; i < LstCustomer.Items.Count; i++)
        {
            if (LstCustomer.Items[i].Selected == true)
            {
                hfNumber.Value += CheckNumber(LstCustomer.Items[i].Value.ToString()) + ",";
            }
        }

        if (hfNumber.Value == "" && txtPhoneNo.Text.ToString() == "")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('Please select customer or enter number.');", true);
            return;
        }

        string receipients = "";
        string PhoneNo = CheckNumber(txtPhoneNo.Text);
        if (hfNumber.Value != "" && PhoneNo != "")
        {
            receipients = hfNumber.Value + PhoneNo;
        }
        if (hfNumber.Value != "" && PhoneNo == "")
        {
            receipients = hfNumber.Value;
        }
        if (hfNumber.Value == "" && PhoneNo != "")
        {
            receipients = PhoneNo;
        }
        
        lblError.Text = SendSMS(Convert.ToInt32(Session["DISTRIBUTOR_ID"]), txtSMS.Text,receipients);
        GetSMSBalance();
    }

    public string SendSMS(int DistributorID, string message, string receipients)
    {
        StringBuilder strMessage = new StringBuilder();
        DataTable dt = _gCtl.GetSMSSetting(DistributorID);
        if (dt != null)
        {
            if (dt.Rows.Count > 0)
            {
                #region Type 1= Jazz Mobilink
                if (dt.Rows[0]["TypeID"].ToString() == "1")//1= Jazz Mobilink
                {
                    String result = "";

                    StreamWriter myWriter = null;
                    HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(dt.Rows[0]["URL"].ToString() + "sendsms_url.html?Username=" + dt.Rows[0]["USERID"].ToString() + "&Password=" + dt.Rows[0]["PASSWORD"].ToString() + "&From=" + dt.Rows[0]["MASK"].ToString() + "&To=" + receipients + "&Message=" + message + "&Identifier=&UniqueId=&ProductId=&Channel=&TransactionId=");
                    objRequest.Method = "POST";
                    objRequest.ContentLength = Encoding.UTF8.GetByteCount("");
                    objRequest.ContentType = "application/x-www-form-urlencoded";
                    try
                    {
                        myWriter = new StreamWriter(objRequest.GetRequestStream());
                        myWriter.Write("");
                    }
                    catch (Exception e)
                    {
                        return e.Message;
                    }
                    finally
                    {
                        myWriter.Close();
                    }
                    HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
                    using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
                    {
                        result = sr.ReadToEnd();
                        sr.Close();
                    }
                    strMessage.Append(result);
                }
                #endregion
                #region Type 2= Telenor
                else if (dt.Rows[0]["TypeID"].ToString() == "2")//2= Telenor
                {
                    String result = "";
                    String strPost = dt.Rows[0]["URL"].ToString() + "auth.jsp?msisdn=" + dt.Rows[0]["USERID"].ToString() + "&password=" + dt.Rows[0]["PASSWORD"].ToString();
                    StreamWriter myWriter = null;
                    HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(dt.Rows[0]["URL"].ToString() + "auth.jsp?msisdn=" + dt.Rows[0]["USERID"].ToString() + "&password=" + dt.Rows[0]["PASSWORD"].ToString());

                    objRequest.Method = "POST";
                    objRequest.ContentLength = Encoding.UTF8.GetByteCount(strPost);
                    objRequest.ContentType = "application/x-www-form-urlencoded";
                    try
                    {
                        myWriter = new StreamWriter(objRequest.GetRequestStream());
                        myWriter.Write(strPost);
                    }
                    catch (Exception e)
                    {
                        return e.Message;
                    }
                    finally
                    {
                        myWriter.Close();
                    }
                    HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
                    using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
                    {
                        result = sr.ReadToEnd();   // Close and clean up the StreamReader   
                        sr.Close();
                    }

                    var xml = System.Xml.Linq.XElement.Parse(result);
                    if (xml.Elements("response").FirstOrDefault().Value.ToLower() == "ok")
                    {
                        string sessionID = xml.Elements("data").FirstOrDefault().Value;
                        if (sessionID.Length > 0)
                        {
                            String result2 = "";
                            String strPost2 = dt.Rows[0]["URL"].ToString() + "sendsms.jsp?session_id=" + sessionID + "&to=" + receipients;
                            StreamWriter myWriter2 = null;
                            HttpWebRequest objRequest2 = (HttpWebRequest)WebRequest.Create(dt.Rows[0]["URL"].ToString() + "sendsms.jsp?session_id=" + sessionID + "&to=" + receipients + "&text=" + message + "&mask=" + dt.Rows[0]["MASK"].ToString());

                            objRequest2.Method = "POST";
                            objRequest2.ContentLength = Encoding.UTF8.GetByteCount(strPost2);
                            objRequest2.ContentType = "application/x-www-form-urlencoded";
                            try
                            {
                                myWriter2 = new StreamWriter(objRequest2.GetRequestStream());
                                myWriter2.Write(strPost2);
                            }
                            catch (Exception e)
                            {
                                return e.Message;
                            }
                            finally
                            {
                                myWriter2.Close();
                            }
                            HttpWebResponse objResponse2 = (HttpWebResponse)objRequest2.GetResponse();
                            using (StreamReader sr = new StreamReader(objResponse2.GetResponseStream()))
                            {
                                result2 = sr.ReadToEnd();   // Close and clean up the StreamReader   
                                sr.Close();
                            }

                            var xml2 = System.Xml.Linq.XElement.Parse(result2);
                            if (xml2.Elements("response").FirstOrDefault().Value.ToLower() == "ok")
                            {
                                strMessage.Append("SMS(s) sent successfully");
                            }
                            else
                            {
                                strMessage.Append(xml2.Elements("data").FirstOrDefault().Value);
                            }
                        }
                    }
                }
                #endregion
                #region Type 3= Outreach
                else if (dt.Rows[0]["TypeID"].ToString() == "3")//3= Outreach
                {
                    String result = "";
                    String strPost = "id=" + dt.Rows[0]["USERID"].ToString() + "&pass=" + dt.Rows[0]["PASSWORD"].ToString() + "&msg=" + message + "&to=" + receipients + "" + "&mask=" + dt.Rows[0]["MASK"].ToString() + "&type=xml&lang=English";
                    StreamWriter myWriter = null;
                    HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(dt.Rows[0]["URL"].ToString());

                    objRequest.Method = "POST";
                    objRequest.ContentLength = Encoding.UTF8.GetByteCount(strPost);
                    objRequest.ContentType = "application/x-www-form-urlencoded";
                    try
                    {
                        myWriter = new StreamWriter(objRequest.GetRequestStream());
                        myWriter.Write(strPost);
                    }
                    catch (Exception e)
                    {
                        return e.Message;
                    }
                    finally
                    {
                        myWriter.Close();
                    }
                    HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
                    using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
                    {
                        result = sr.ReadToEnd();
                        sr.Close();
                    }
                    var xml2 = System.Xml.Linq.XElement.Parse(result);
                    if (xml2.Elements("type").FirstOrDefault().Value.ToLower() == "success")
                    {
                        strMessage.Append(xml2.Elements("response").FirstOrDefault().Value);
                    }
                    else
                    {
                        strMessage.Append("Message not sent");
                    }
                }
                #endregion
                #region Type 4= Tech Garage Now
                else if (dt.Rows[0]["TypeID"].ToString() == "4")//4= Tech Garage Now
                {
                    String result = "";
                    HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(string.Format("https://sms.montymobile.com/API/SendSMS?username={0}&apiId={1}&json=False&destination={2}&source={3}&text={4}", dt.Rows[0]["USERID"], dt.Rows[0]["MESSAGE"], receipients, dt.Rows[0]["MASK"], message));

                    objRequest.Method = "POST";
                    objRequest.ContentLength = 0;
                    objRequest.ContentType = "application/x-www-form-urlencoded";

                    HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
                    using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
                    {
                        result = sr.ReadToEnd();
                        sr.Close();
                    }
                    if (result == "MESSAGE_SUCCESS")
                    {
                        strMessage.Append("SMS(s) sent successfully");
                    }
                    else
                    {
                        strMessage.Append("Message not sent");
                    }
                }
                #endregion
                #region Type 5= SMS Point
                else if (dt.Rows[0]["TypeID"].ToString() == "5")//5= SMS Point
                {

                    String result = "";
                    String strPost = "username=" + dt.Rows[0]["USERID"].ToString() + "&password=" + dt.Rows[0]["PASSWORD"].ToString() + "&clientid=" + dt.Rows[0]["USERID"].ToString() + "&mask=" + dt.Rows[0]["MASK"].ToString() + "&msg=" + message + "" + "&to=" + receipients + "&language=English";
                    StreamWriter myWriter = null;
                    HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(dt.Rows[0]["URL"].ToString());

                    objRequest.Method = "POST";
                    objRequest.ContentLength = Encoding.UTF8.GetByteCount(strPost);
                    objRequest.ContentType = "application/x-www-form-urlencoded";
                    try
                    {
                        myWriter = new StreamWriter(objRequest.GetRequestStream());
                        myWriter.Write(strPost);
                    }
                    catch (Exception e)
                    {
                        return e.Message;
                    }
                    finally
                    {
                        myWriter.Close();
                    }
                    HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
                    using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
                    {
                        result = sr.ReadToEnd();
                        sr.Close();
                    }

                    strMessage.Append(result);
                }
                #endregion
                #region Type 6= Thumb Crowd
                else if (dt.Rows[0]["TypeID"].ToString() == "6")//6= Thumb Crowd
                {
                    SendCalltoWebAPI callapi = new SendCalltoWebAPI();
                    callapi.CallWebAPI(message, receipients, dt.Rows[0]["MASK"].ToString(), dt.Rows[0]["MESSAGE"].ToString(), dt.Rows[0]["URL"].ToString(), dt.Rows[0]["USERID"].ToString());
                }
                #endregion
                #region Type 7= BizSMS
                else if (dt.Rows[0]["TypeID"].ToString() == "7")//7= BizSMS
                {
                    String result = "";

                    StreamWriter myWriter = null;
                    HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(dt.Rows[0]["URL"].ToString() + "?username=" + dt.Rows[0]["USERID"].ToString() + "&pass=" + dt.Rows[0]["PASSWORD"].ToString() + "&text=" + message + "&masking=" + dt.Rows[0]["MASK"].ToString() + "&&destinationnum=" + receipients + "&language=English=&responsetype=text");
                    objRequest.Method = "POST";
                    objRequest.ContentLength = Encoding.UTF8.GetByteCount("");
                    objRequest.ContentType = "application/x-www-form-urlencoded";
                    try
                    {
                        myWriter = new StreamWriter(objRequest.GetRequestStream());
                        myWriter.Write("");
                    }
                    catch (Exception e)
                    {
                        return e.Message;
                    }
                    finally
                    {
                        myWriter.Close();
                    }
                    HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
                    using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
                    {
                        result = sr.ReadToEnd();
                        sr.Close();
                    }
                    strMessage.Append(result);
                }
                #endregion
            }
        }
        return strMessage.ToString();
    }
    private string SendSMS(string customerNo, string userid, string password, string msg, string mask, string Url)
    {
        String result = "";
        String strPost = "id=" + userid + "&pass=" + password + "&msg=" + msg + "&to=" + customerNo + "" + "&mask=" + mask + "&type=xml&lang=English";
        StreamWriter myWriter = null;
        HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(Url);

        objRequest.Method = "POST";
        objRequest.ContentLength = Encoding.UTF8.GetByteCount(strPost);
        objRequest.ContentType = "application/x-www-form-urlencoded";
        try
        {
            myWriter = new StreamWriter(objRequest.GetRequestStream());
            myWriter.Write(strPost);
        }
        catch (Exception e)
        {
            return e.Message;
        }
        finally
        {
            myWriter.Close();
        }
        HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
        using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
        {
            result = sr.ReadToEnd();   // Close and clean up the StreamReader   
            sr.Close();
        }
        return result;
    }

    private string SendSMSTelenor(string customerNo, string userid, string password, string msg, string mask, string Url)
    {
        String result = "";
        String strPost = Url + "auth.jsp?msisdn=" + userid + "&password=" + password;
        StreamWriter myWriter = null;
        HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(Url + "auth.jsp?msisdn=" + userid + "&password=" + password);

        objRequest.Method = "POST";
        objRequest.ContentLength = Encoding.UTF8.GetByteCount(strPost);
        objRequest.ContentType = "application/x-www-form-urlencoded";
        try
        {
            myWriter = new StreamWriter(objRequest.GetRequestStream());
            myWriter.Write(strPost);
        }
        catch (Exception e)
        {
            return e.Message;
        }
        finally
        {
            myWriter.Close();
        }
        HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
        using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
        {
            result = sr.ReadToEnd();   // Close and clean up the StreamReader   
            sr.Close();
        }

        var xml = System.Xml.Linq.XElement.Parse(result);
        if (xml.Elements("response").FirstOrDefault().Value.ToLower() == "ok")
        {
            string sessionID = xml.Elements("data").FirstOrDefault().Value;
            if(sessionID.Length>0)
            {
                String result2 = "";
                String strPost2 = Url + "sendsms.jsp?session_id=" + sessionID + "&to=" + customerNo;
                StreamWriter myWriter2 = null;
                HttpWebRequest objRequest2 = (HttpWebRequest)WebRequest.Create(Url + "sendsms.jsp?session_id=" + sessionID + "&to=" + customerNo + "&text=" + msg + "&mask=" + mask);

                objRequest2.Method = "POST";
                objRequest2.ContentLength = Encoding.UTF8.GetByteCount(strPost2);
                objRequest2.ContentType = "application/x-www-form-urlencoded";
                try
                {
                    myWriter2 = new StreamWriter(objRequest2.GetRequestStream());
                    myWriter2.Write(strPost2);
                }
                catch (Exception e)
                {
                    return e.Message;
                }
                finally
                {
                    myWriter2.Close();
                }
                HttpWebResponse objResponse2 = (HttpWebResponse)objRequest2.GetResponse();
                using (StreamReader sr = new StreamReader(objResponse2.GetResponseStream()))
                {
                    result2 = sr.ReadToEnd();   // Close and clean up the StreamReader   
                    sr.Close();
                }

                var xml2 = System.Xml.Linq.XElement.Parse(result2);
                if (xml2.Elements("response").FirstOrDefault().Value.ToLower() == "ok")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('SMS(s) sent successfully.');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "msg", "alert('" + xml2.Elements("data").FirstOrDefault().Value + "');", true);
                }
            }
        }
        return result;
    }

    private string CheckNumber(string CNO)
    {
        string Customer_No = "";
        string CONTACT_NO = CNO;
        if (CONTACT_NO.Length == 11) // 0300xxxxxxx
        {
            string str = CNO.Substring(0, 2);
            if (str.ToString() == "03")
            {
                string str1 = CNO.Substring(1, 10);
                Customer_No = "92" + str1;
            }
            else
            {
                Customer_No = "0";
            }
        }
        else if (CONTACT_NO.Length == 12) // 92300xxxxxxx
        {
            string str = CNO.Substring(0, 3);
            if (str.ToString() == "923")
            {
                Customer_No = CNO;
            }
            else
            {
                Customer_No = "0";
            }
        }
        else if (CONTACT_NO.Length == 13) // 920300xxxxxxx
        {
            string str = CNO.Substring(0, 3);
            if (str.ToString() == "920")
            {
                string str1 = CNO.Substring(0, 2);
                string str2 = CNO.Substring(3, 10);
                Customer_No = str1 + str2;
            }
            else
            {
                Customer_No = "0";
            }

        }
        else if (CONTACT_NO.Length == 14) // 0092300xxxxxxx
        {
            string str = CNO.Substring(0, 5);
            if (str.ToString() == "00923")
            {
                string str1 = CNO.Substring(2, 2);
                string str2 = CNO.Substring(4, 10);
                Customer_No = str1 + str2;
            }
            else
            {
                Customer_No = "0";
            }
        }
        else if (CONTACT_NO.Length == 15) // 00920300xxxxxxx
        {
            string str = CNO.Substring(0, 5);
            if (str.ToString() == "00920")
            {
                string str1 = CNO.Substring(2, 2);
                string str2 = CNO.Substring(5, 10);
                Customer_No = str1 + str2;
            }
            else
            {
                Customer_No = "0";
            }
        }
        return Customer_No;
    }

    /// <summary>
    /// Clears Form Controls
    /// </summary>
    /// <param name="sender">object</param>
    /// <param name="e">EventArgs</param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        this.txtPhoneNo.Text = null;
        this.txtSMS.Text = null;
    }
}

public class SendCalltoWebAPI
{
    public SendCalltoWebAPI()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public async void CallWebAPI(string p_Text, string p_Receipants, string p_Masking, string p_Token, string p_URL, string p_URL2)
    {
        //CallWebAPIAsync(p_Text, p_Receipants,p_Masking,p_Token, p_URL, p_URL2).Wait();
        await CallWebAPIAsync();
    }
    static async Task CallWebAPIAsync()
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://thumb-crowd.com");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", "TOKEN 6da996de24c87f6e3088dbd9a6d22e967d71059f");

                MessageBody SMSMessage = new MessageBody();
                SMSMessage.text = "Hi";
                SMSMessage.urns = "tel:+923110449206";
                SMSMessage.masking = "Mozarella27";

                var dataAsString = JsonConvert.SerializeObject(SMSMessage);
                var dataContent = new StringContent(dataAsString, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("/api/v2/broadcasts.json", dataContent);

                if (response.IsSuccessStatusCode)
                {
                    Uri ncrUrl = response.Headers.Location;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    static async Task CallWebAPIAsync(string p_Text, string p_Receipants, string p_Masking, string p_Token, string p_URL, string p_URL2)
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(p_URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", "TOKEN " + p_Token);

                MessageBody SMSMessage = new MessageBody() { text = p_Text, urns = "tel:" + p_Receipants, masking = p_Masking };

                var dataAsString = JsonConvert.SerializeObject(SMSMessage);
                var dataContent = new StringContent(dataAsString, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(p_URL2, dataContent);

                if (response.IsSuccessStatusCode)
                {
                    Uri ncrUrl = response.Headers.Location;
                }
            }
        }
        catch (Exception ex)
        {
            CORNCommon.Classes.ExceptionPublisher.PublishException(ex);
        }
    }
}
class MessageBody
{
    public string text { get; set; }
    public string urns { get; set; }
    public string masking { get; set; }
}
public class CommSetting : IDisposable
{
   public GsmCommMain comm;
    
    public CommSetting()
    {

    }
    
    ~CommSetting()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this.comm);
    }

    private void Dispose(bool disposing)
    {
        if (this.comm.IsOpen())
        {
            this.comm.Close();
        }

    }
}