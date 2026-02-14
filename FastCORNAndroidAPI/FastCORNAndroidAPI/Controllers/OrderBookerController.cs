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
    public class OrderBookerController : ApiController
    {
        DatabaseLayer dbLayer = new DatabaseLayer();
        EncryptDecrypt decrypt = new EncryptDecrypt();
        
        [Route("api/OrderBooker/InsertOrderBookerStartDay")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage InsertOrderBookerStartDay(int DistributorID, int UserID)
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

                DataTable dtWorkingDate = dbLayer.InsertOrderBookerStartDay(DistributorID, UserID, decrypt.DecryptValue(connstring));

                if (dtWorkingDate.Rows.Count > 0)
                {
                    List<OrderBookerDailyProcess> OBDailyProcess = new List<OrderBookerDailyProcess>();
                    OBDailyProcess = Helper.ConvertDataTable<OrderBookerDailyProcess>(dtWorkingDate);
                    return Request.CreateResponse(HttpStatusCode.OK, new { OBDailyProcess });

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

        [Route("api/OrderBooker/InsertOrderBookerEndDay")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage InsertOrderBookerEndDay(int DistributorID, int UserID, DateTime StartDayDateTime)
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

                bool flag = dbLayer.InsertOrderBookerEndDay(DistributorID, UserID, StartDayDateTime, decrypt.DecryptValue(connstring));

                if (flag)
                {
                    returnData = new Messages
                    {
                        Message = "End Day entered successfully.",
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