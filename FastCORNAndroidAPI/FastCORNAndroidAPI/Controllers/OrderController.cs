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
    public class OrderController : ApiController
    {
        DatabaseLayer dbLayer = new DatabaseLayer();
        EncryptDecrypt decrypt = new EncryptDecrypt();
        
        [Route("api/Order/InsertOrder")]
        [HttpPost]
        //[Authorize]
        public HttpResponseMessage InsertOrder(int LocationID,decimal GST,long CustomerID, string Instructions,
            string DeliveryAddress, int UserID, List<OrderDetail> dtOrderDetail)
        {
            var req = Request;
            var header = req.Headers;
            Messages returnData = null;
            try
            {
                string connstring = "";
                if (header.Contains("x-conn"))
                {
                    connstring = header.GetValues("x-conn").First();
                }
                                
                long InvoiceID = dbLayer.AddInvoice(LocationID, GST, CustomerID, UserID, Instructions, DeliveryAddress, dtOrderDetail , decrypt.DecryptValue(connstring));

                if (InvoiceID>0)
                {
                    returnData = new Messages
                    {
                        Message = string.Format("Order No: {0} Inserted.", InvoiceID),
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