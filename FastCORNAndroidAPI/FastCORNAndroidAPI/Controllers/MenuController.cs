using System.Collections.Generic;
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
    public class MenuController : ApiController
    {
        DatabaseLayer dbLayer = new DatabaseLayer();
        EncryptDecrypt decrypt = new EncryptDecrypt();
        
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetMenu( int UserID)
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
                DataTable dt = dbLayer.GetAndoridMenu(UserID, decrypt.DecryptValue(connstring));
                List<Menu> menu = new List<Menu>();
                menu = Helper.ConvertDataTable<Menu>(dt);
                return Request.CreateResponse(HttpStatusCode.OK, new { menu });
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
