using CORNAttendanceApi.Models;
using FastAndroidAPI.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace FastAndroidAPI.Controllers
{
    public class ExecuteSPController : ApiController
    {
        DatabaseLayer dbLayer = new DatabaseLayer();
        EncryptDecrypt decrypt = new EncryptDecrypt();

        [Route("api/ExecuteSP/Get")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage Get(SpData data)
        {
            Messages returnData = null;
            var req = Request;
            var header = req.Headers;
            string connstring = "";
            try
            {
                if (header.Contains("x-conn"))
                {
                    connstring = header.GetValues("x-conn").First();
                }
                DataTable DataReturned = dbLayer.ExecGetStoreProcedure(data, decrypt.DecryptValue(connstring));
                return Request.CreateResponse(HttpStatusCode.OK, new { DataReturned });
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


        [Route("api/ExecuteSP/GetJSon")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GetJSon(SpData data)
        {
            Messages returnData = null;
            var req = Request;
            var header = req.Headers;
            string connstring = "";
            try
            {
                if (header.Contains("x-conn"))
                {
                    connstring = header.GetValues("x-conn").First();
                }
                DataTable DataReturned = dbLayer.ExecGetStoreProcedure(data, decrypt.DecryptValue(connstring));
                string strData = JsonConvert.SerializeObject(DataReturned);

                string result = strData;
                var resp = new HttpResponseMessage(HttpStatusCode.OK);
                resp.Content = new StringContent(result, System.Text.Encoding.UTF8, "application/json");
                return resp;
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

        [Route("api/ExecuteSP/GetJSonDataSet")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage GetJSonDataSet(SpData data)
        {
            Messages2 returnData = null;
            var req = Request;
            var header = req.Headers;
            string connstring = "";
            try
            {
                if (header.Contains("x-conn"))
                {
                    connstring = header.GetValues("x-conn").First();
                }
                DataSet DataReturned = dbLayer.ExecGetStoreProcedureDataSet(data, decrypt.DecryptValue(connstring));
                string strData = JsonConvert.SerializeObject(DataReturned);

                JObject json = JObject.Parse(strData);

                returnData = new Messages2
                {
                    Rows = json,
                    Status = true
                };
                return Request.CreateResponse(HttpStatusCode.OK, returnData, new JsonMediaTypeFormatter());
            }
            catch (Exception ex)
            {
                returnData = new Messages2
                {
                    Message = ex.ToString(),
                    Status = false
                };
                return Request.CreateResponse(HttpStatusCode.BadRequest, returnData, new JsonMediaTypeFormatter());
            }
        }
        [Route("api/ExecuteSP")]
        [HttpPost]
        public HttpResponseMessage ExecuteSP(SpData data)
        {
            Messages returnData = null;
            var req = Request;
            var header = req.Headers;
            string connstring = "";
            try
            {
                if (header.Contains("x-conn"))
                {
                    connstring = header.GetValues("x-conn").First();
                }
                DataTable DataReturned = dbLayer.ExecGetStoreProcedure(data, decrypt.DecryptValue(connstring));
                return Request.CreateResponse(HttpStatusCode.OK, new { DataReturned });
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
        [Route("api/ExecuteSPPOS")]
        [HttpPost]
        public HttpResponseMessage ExecuteSPPOS(SpData data)
        {
            var req = Request;
            var header = req.Headers;
            string connstring = "";
            try
            {
                if (header.Contains("x-conn"))
                {
                    connstring = header.GetValues("x-conn").First();
                }
                var getModel= dbLayer.ExecGetStoreProcedure(data, decrypt.DecryptValue(connstring));
                if (getModel == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
                return Request.CreateResponse(HttpStatusCode.OK, getModel, new JsonMediaTypeFormatter());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex, new JsonMediaTypeFormatter());
            }
        }
        [HttpPost]
        [Route("~/api/ExecSpAllDataSets")]
        //[CacheFilter(TimeDuration = 60)]
        public HttpResponseMessage ExecSpAllDataSets(SpData data)
        {
            var req = Request;
            var header = req.Headers;
            string connstring = "";

            try
            {
                if (header.Contains("x-conn"))
                {
                    connstring = header.GetValues("x-conn").First();
                }
                var getModel = dbLayer.ExecGetStoreProcedureDataSet(data, decrypt.DecryptValue(connstring));
                if (getModel == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
                return Request.CreateResponse(HttpStatusCode.OK, getModel, new JsonMediaTypeFormatter());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex, new JsonMediaTypeFormatter());
            }
        }
    }
}