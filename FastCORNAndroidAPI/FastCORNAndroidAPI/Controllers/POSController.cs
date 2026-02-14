using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using FastAndroidAPI.Models;
using System;
using CORNAttendanceApi.Models;
using System.Net.Http;
using System.Net;
using System.Net.Http.Formatting;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FastAndroidAPI.Controllers
{
    public class POSController : ApiController
    {
        DatabaseLayer dbLayer = new DatabaseLayer();
        EncryptDecrypt decrypt = new EncryptDecrypt();

        [Route("api/User")]
        [HttpGet]
        public HttpResponseMessage GetLoginInfo(string username)
        {
            Messages2 returnData = null;
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

                DataTable dt = dbLayer.GetLoginDetailsByLoginID3(username.Trim(), decrypt.DecryptValue(connstring));
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string str = JsonConvert.SerializeObject(dt);
                        if (str.Length > 2)
                        {
                            str = str.Remove(str.Length - 1, 1);
                            str = str.Remove(0, 1);
                        }
                        JObject json = JObject.Parse(str);
                        returnData = new Messages2
                        {
                            Rows = json,
                            Status = true
                        };
                        return Request.CreateResponse(HttpStatusCode.OK, returnData, new JsonMediaTypeFormatter());
                    }
                }
                returnData = new Messages2
                {
                    Message = "Provided username and password is incorrect",
                    Status = false,
                };
                return Request.CreateResponse(HttpStatusCode.NotFound, returnData, new JsonMediaTypeFormatter());

            }
            catch (Exception ex)
            {
                returnData = new Messages2
                {
                    Message = ex.ToString(),
                    Status = false,
                };
                return Request.CreateResponse(HttpStatusCode.BadRequest, returnData, new JsonMediaTypeFormatter());
            }
        }
    }
}