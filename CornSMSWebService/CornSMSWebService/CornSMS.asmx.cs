using System;
using System.Data;
using System.Linq;
using System.Web.Services;
using CORNBusinessLayer.Classes;
using CORNCommon.Classes;
using System.IO;
using System.Net;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CornSMSWebService
{
    /// <summary>
    /// Summary description for CornSMS
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class CornSMS : System.Web.Services.WebService
    {
        [WebMethod]
        public string Test()
        {
            using (FileStream fs = System.IO.File.Open("D:\\CORNPOSKOTPrintLog.txt", FileMode.Append, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(string.Format("{0}: {1}", DateTime.Now, "Hi"));
                    sw.Write(Environment.NewLine);
                }
            }

            StringBuilder strMessage = new StringBuilder();
            strMessage.Append("Tested.");
            return strMessage.ToString();
        }

        [WebMethod]
        public string GetSMSBalance(int DistributorID, string connString)
        {
            StringBuilder strMessage = new StringBuilder();
            if (connString.Length > 0)
            {
                GeoHierarchyController _gCtl = new GeoHierarchyController();
                connString = connString.Replace(" ", "+");
                string[] conn = connString.Split(',');
                if (conn.Length == 5)
                {
                    Configuration.DatabseAuthentication = Cryptography.Decrypt(conn[0].ToString(), "b0tin@74");
                    Configuration.DatabseServer = Cryptography.Decrypt(conn[1].ToString(), "b0tin@74");
                    Configuration.DatabaseName = Cryptography.Decrypt(conn[2].ToString(), "b0tin@74");
                    Configuration.ServerUserName = Cryptography.Decrypt(conn[3].ToString(), "b0tin@74");
                    Configuration.ServerUserPwd = Cryptography.Decrypt(conn[4].ToString(), "b0tin@74");

                    DataTable dt = _gCtl.GetSMSSetting(DistributorID);
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

                                var xml = System.Xml.Linq.XElement.Parse(result);
                                if (xml.Elements("type").FirstOrDefault().Value.ToLower() == "success")
                                {
                                    strMessage.Append(xml.Elements("response").FirstOrDefault().Value.ToLower());
                                    strMessage.Append("    |    Expiry : ");
                                    strMessage.Append(xml.Elements("expiry").FirstOrDefault().Value.ToLower());
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
                }
            }
            return strMessage.ToString();
        }

        [WebMethod]
        public string SendSMS(int DistributorID, string message, string receipients, string connString)
        {
            StringBuilder strMessage = new StringBuilder();
            if (connString.Length > 0)
            {
                GeoHierarchyController _gCtl = new GeoHierarchyController();
                connString = connString.Replace(" ", "+");
                string[] conn = connString.Split(',');
                if (conn.Length == 5)
                {

                    Configuration.DatabseAuthentication = Cryptography.Decrypt(conn[0].ToString(), "b0tin@74");
                    Configuration.DatabseServer = Cryptography.Decrypt(conn[1].ToString(), "b0tin@74");
                    Configuration.DatabaseName = Cryptography.Decrypt(conn[2].ToString(), "b0tin@74");
                    Configuration.ServerUserName = Cryptography.Decrypt(conn[3].ToString(), "b0tin@74");
                    Configuration.ServerUserPwd = Cryptography.Decrypt(conn[4].ToString(), "b0tin@74");

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
                }
            }
            return strMessage.ToString();
        }        
    }
    class clsMessage
    {
        public string text { get; set; }
        public string urns { get; set; }
        public string masking { get; set; }
    }
}