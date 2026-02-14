using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using FastAndroidAPI.Models;
using System;
using CORNAttendanceApi.Models;
using System.Net.Http;
using System.Net;
using System.Net.Http.Formatting;

namespace FastAndroidAPI.Controllers
{
    public class MarchandiseController : ApiController
    {
        DatabaseLayer dbLayer = new DatabaseLayer();
        EncryptDecrypt decrypt = new EncryptDecrypt();
        
        [Route("api/Marchandise/InsertMarchandise")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage InsertMarchandise(int DistributorID, int UserID,List<Marchandise> dtMarchandise)
        {
            var req = Request;
            var header = req.Headers;
            Messages returnData = null;
            string connstring = "";
            try
            {
                if (header.Contains("x-conn"))
                {
                    connstring = header.GetValues("x-conn").First();
                }
                
                bool flag = dbLayer.InsertMarchandise(DistributorID, UserID, dtMarchandise, decrypt.DecryptValue(connstring));

                if (flag)
                {
                    returnData = new Messages
                    {
                        Message = dtMarchandise.Count.ToString() + " Row(s) inserted",
                        Satus = "OK"
                    };
                    return Request.CreateResponse(HttpStatusCode.OK, returnData, new JsonMediaTypeFormatter());
                }
                else
                {
                    returnData = new Messages
                    {
                        Message = "Some error occured",
                        Satus = "Error"
                    };
                    return Request.CreateResponse(HttpStatusCode.OK, returnData, new JsonMediaTypeFormatter());

                }
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