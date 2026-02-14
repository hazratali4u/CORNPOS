using System.Linq;
using System.Web.Http;
using FastAndroidAPI.Models;
using System.Data;
using System;
using CORNAttendanceApi.Models;
using System.Net.Http;
using System.Net;
using System.Net.Http.Formatting;

namespace FastAndroidAPI.Controllers
{
    public class LoginController : ApiController
    {
        DatabaseLayer dbLayer = new DatabaseLayer();
        EncryptDecrypt decrypt = new EncryptDecrypt();

        [Route("api/Login")]
        [HttpGet]
        public HttpResponseMessage GetLoginInfo(string username, string password)
        {
            Messages returnData = null;
            try
            {
                var req = Request;
                var header = req.Headers;
                string connstring = "";
                if (header.Contains("x-conn"))
                {
                    connstring = header.GetValues("x-conn").First();
                }


                if (!header.Contains("x-conn"))
                    throw new UnauthorizedAccessException("You are not authorized to access this link. Please contact administrator");

                DataTable dt = dbLayer.GetLoginDetailsByLoginID(username.Trim(), decrypt.DecryptValue(connstring));
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["UserLogin"].ToString().ToLower() == username.ToLower() 
                            && dt.Rows[i]["UserPassword"].ToString().ToLower() == password)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, new { loginDetails = dt.Rows[i].Table });
                        }
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, new { loginDetails = new object() });
            }
            catch (Exception ex)
            {
                returnData = new Messages
                {
                    Message = ex.ToString(),
                    Satus = "Exception"
                };
                return Request.CreateResponse(HttpStatusCode.BadRequest, returnData, new JsonMediaTypeFormatter());
            }
        }
    }
}