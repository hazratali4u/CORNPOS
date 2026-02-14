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
    public class OutletController : ApiController
    {
        DatabaseLayer dbLayer = new DatabaseLayer();
        EncryptDecrypt decrypt = new EncryptDecrypt();
        
        [Route("api/Outlet/InsertOutlet")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage InsertOutlet(int LocationID, string Email, string Password, string FName, string LName, string Address
            , string Country, string Province, string City, string PhoneNo, string ZipCode)
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

                long result = dbLayer.InsertCustomer(LocationID, Email, Password, FName, LName, Address,Country,Province,City,PhoneNo,ZipCode, decrypt.DecryptValue(connstring));

                if (result > 0)
                {
                    returnData = new Messages
                    {
                        Message = "1 Row(s) inserted",
                        Satus = result.ToString()
                    };
                    return Request.CreateResponse(HttpStatusCode.OK, returnData, new JsonMediaTypeFormatter());
                }
                else if (result == -1)
                {
                    returnData = new Messages
                    {
                        Message = "Phone No already exist.",
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

        [Route("api/Outlet/UpdateOutlet")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage UpdateOutlet(long CustomerID,int LocationID, string Email, string Password, string FName, string LName, string Address
            , string Country, string Province, string City, string ZipCode)
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

                bool result = dbLayer.UpdateCustomer(CustomerID,LocationID, Email, Password, FName, LName, Address, Country, Province, City, ZipCode, decrypt.DecryptValue(connstring));

                if (result)
                {
                    returnData = new Messages
                    {
                        Message = "1 Customer updated.",
                        Satus = result.ToString()
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

        [Route("api/Outlet/GetCafeStatus")]
        [HttpGet]
       // [Authorize]
        public HttpResponseMessage GetCafeStatus(DateTime dateAndTime, string day, int locationId)
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
                DataTable dt = dbLayer.GetCafeStatus(dateAndTime, day, locationId, decrypt.DecryptValue(connstring));
                var status = "";
                if (dt.Rows.Count > 0)
                    status = dt.Rows[0]["Status"].ToString();

                return Request.CreateResponse(HttpStatusCode.OK, new { status = status });
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