using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using FastAndroidAPI.Models;
using System.Data;
using System;
using CORNAttendanceApi.Models;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using System.Net.Http.Formatting;
using System.Data.Objects;

namespace FastAndroidAPI.Controllers
{
    public class CustomerController : ApiController
    {
        DatabaseLayer dbLayer = new DatabaseLayer();
        EncryptDecrypt decrypt = new EncryptDecrypt();

        [Route("api/Customer/InsertCustomerAndUser")]
        [HttpPost]
        public HttpResponseMessage InsertCustomerAndUser(string firstName, string lastName, string email, string password,
            string address, string country, string province, string city, string phone, string zip)
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

                long customerID = dbLayer.InsertCustomerAndUser(1, email, password, firstName, lastName, address,
                    country, province, city, phone, zip, decrypt.DecryptValue(connstring));

                if (customerID > 0)
                {
                    DataTable dt = dbLayer.GetLoginDetails(customerID, decrypt.DecryptValue(connstring));

                    List<CustomerAndUser> loginDetails = new List<CustomerAndUser>();
                    loginDetails = Helper.ConvertDataTable<CustomerAndUser>(dt);

                    //returnData = new Messages
                    //{
                    //    Message = "Customer added successfully",
                    //    Satus = "OK"
                    //};

                    return Request.CreateResponse(HttpStatusCode.OK, new { loginDetails });

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

        [Route("api/Customer/UpdateCustomer")]
        [HttpPost]
        public HttpResponseMessage UpdateCustomer(long CustomerID, string Email, string Password, string FName, string LName, string Address
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

                bool result = dbLayer.UpdateCustomer(CustomerID, 1, Email, Password, FName, LName, Address, Country, Province, City, ZipCode, decrypt.DecryptValue(connstring));

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

        [Route("api/Customer/GetCustomerProfile")]
        [HttpGet]
        public HttpResponseMessage GetCustomerProfile(long customerId)
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
                DataTable dt = dbLayer.GetCustomerProfile(customerId, decrypt.DecryptValue(connstring));

                List<CustomerProfile> customerProfile = new List<CustomerProfile>();
                customerProfile = Helper.ConvertDataTable<CustomerProfile>(dt);
                return Request.CreateResponse(HttpStatusCode.OK, new { customerProfile });
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

        [Route("api/Customer/GetCustomerOrderHistory")]
        [HttpGet]
        public HttpResponseMessage GetCustomerOrderHistory(long customerId)
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
                DataTable dt = dbLayer.GetCustomerOrderHistory(customerId, decrypt.DecryptValue(connstring));

                List<CustomerOrderHistory> customerOrderHistory = new List<CustomerOrderHistory>();
                customerOrderHistory = Helper.ConvertDataTable<CustomerOrderHistory>(dt);
                return Request.CreateResponse(HttpStatusCode.OK, new { customerOrderHistory });
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