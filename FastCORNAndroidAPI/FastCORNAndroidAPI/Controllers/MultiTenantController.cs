using System.Web.Http;
using FastAndroidAPI.Models;
using System.Data;
using CORNAttendanceApi.Models;
using System;
using System.Net.Http;
using System.Net;
using System.Net.Http.Formatting;
using System.Linq;
using System.Web.Http.Cors;

namespace FastAndroidAPI.Controllers
{
    public class MultiTenantController : ApiController
    {
        DatabaseLayer dbLayer = new DatabaseLayer();

        [Route("api/MultiTenant/GetClientInfo")]
        [HttpGet]
        public HttpResponseMessage GetClientInfo(string Pin)
        {
            var req = Request;
            var headers = req.Headers;
            MultiTenant returnData = null;
            MultiTenant2 returnData2 = null;
            Messages returnmsg = null;

            var header = req.Headers;
            string format = null;
            if (header.Contains("format"))
            {
                format = header.GetValues("format").First();
            }
            bool IsNew = false;
            if (format != null)
            {
                if (format == "new")
                {
                    IsNew = true;
                }
            }

            if (IsNew)
            {
                try
                {
                    DataTable dtConn = dbLayer.GetClientConnection(Pin);
                    if (dtConn.Rows.Count > 0)
                    {
                        returnData2 = new MultiTenant2
                        {
                            ClientID = Convert.ToInt32(dtConn.Rows[0]["clientID"]),
                            ClientConnString = dtConn.Rows[0]["ClientConnString"].ToString(),
                            BackGroundImage = dtConn.Rows[0]["ImagePath1"].ToString(),
                            Logo = dtConn.Rows[0]["ImagePath2"].ToString(),
                            ImageServerUrl = dtConn.Rows[0]["ImageServerUrl"].ToString(),
                            Status = true
                        };
                        return Request.CreateResponse(HttpStatusCode.OK, returnData2, new JsonMediaTypeFormatter());
                    }
                    else
                    {
                        returnData2 = new MultiTenant2
                        {
                            Message = "Pin not found",
                            Status = false
                        };
                        return Request.CreateResponse(HttpStatusCode.NotFound, returnData2, new JsonMediaTypeFormatter());
                    }                    
                }
                catch (Exception ex)
                {
                    returnData2 = new MultiTenant2
                    {
                        Message = ex.ToString(),
                        Status = false
                    };
                    return Request.CreateResponse(HttpStatusCode.BadRequest, returnmsg, new JsonMediaTypeFormatter());
                }
            }        
            else
            {
                try
                {
                    DataTable dtConn = dbLayer.GetClientConnection(Pin);
                    if (dtConn.Rows.Count > 0)
                    {
                        returnData = new MultiTenant
                        {
                            ClientID = Convert.ToInt32(dtConn.Rows[0]["clientID"]),
                            ClientConnString = dtConn.Rows[0]["clientConnString"].ToString(),
                            BackGroundImage = dtConn.Rows[0]["ImagePath1"].ToString(),
                            Logo = dtConn.Rows[0]["ImagePath2"].ToString(),
                            ImageServerUrl = dtConn.Rows[0]["ImageServerUrl"].ToString(),
                            IsSuccess = true
                        };
                        return Request.CreateResponse(HttpStatusCode.OK, returnData, new JsonMediaTypeFormatter());
                    }
                    else
                    {
                        returnData = new MultiTenant
                        {
                            ClientID = 0,
                            ClientConnString = "Pin not verfied",
                            BackGroundImage = "",
                            Logo = "",
                            IsSuccess = false
                        };
                        return Request.CreateResponse(HttpStatusCode.NotFound, returnData, new JsonMediaTypeFormatter());
                    }                    
                }
                catch (Exception ex)
                {
                    returnmsg = new Messages
                    {
                        Message = ex.ToString(),
                        Satus = "Exception"
                    };
                    return Request.CreateResponse(HttpStatusCode.BadRequest, returnmsg, new JsonMediaTypeFormatter());
                }
            }
        }        
    }
}