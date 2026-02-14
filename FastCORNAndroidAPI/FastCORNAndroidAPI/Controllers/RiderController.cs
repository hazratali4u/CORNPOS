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
    public class RiderController : ApiController
    {
        DatabaseLayer dbLayer = new DatabaseLayer();
        EncryptDecrypt decrypt = new EncryptDecrypt();
        
        [Route("api/Rider/GetOrderListing")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetOrderListing(int riderID)
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
                DataTable dt = dbLayer.GetRiderOrderListing(riderID, decrypt.DecryptValue(connstring));
                List<RiderOrderList> orderList = new List<RiderOrderList>();
                orderList = Helper.ConvertDataTable<RiderOrderList>(dt);
                return Request.CreateResponse(HttpStatusCode.OK, new { orderList });
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

        [Route("api/Rider/GetOrderDetails")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetOrderDetails(int orderId, int riderID)
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
                DataTable dt = dbLayer.GetRiderOrderDetails(orderId, riderID, decrypt.DecryptValue(connstring));
                List<RiderOrderDetails> orderDetail = new List<RiderOrderDetails>();
                orderDetail = Helper.ConvertDataTable<RiderOrderDetails>(dt);
                return Request.CreateResponse(HttpStatusCode.OK, new { orderDetail });
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

        [Route("api/Rider/GetRideDetails")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetRideDetails(int orderId, int riderID)
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
                DataTable dt = dbLayer.GetRiderRideStartedDetails(orderId, riderID, decrypt.DecryptValue(connstring));
                List<RideStartedDetails> rideDetail = new List<RideStartedDetails>();
                rideDetail = Helper.ConvertDataTable<RideStartedDetails>(dt);
                return Request.CreateResponse(HttpStatusCode.OK, new { rideDetail });
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

        [Route("api/Rider/UpdateDeliveryStatus")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage UpdateDeliveryStatus(int orderId, int deliveryStatusId)
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

                bool result = dbLayer.UpdateDeliveryStatus(orderId, deliveryStatusId, decrypt.DecryptValue(connstring));

                if (result == true)
                {
                    returnData = new Messages
                    {
                        Message = string.Format("Record Updated successfully"),
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

        [Route("api/Rider/InsertRiderFeedback")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage InsertRiderFeedback(long orderId, int riderId, int locationId, long customerId,
            int rate, string description)
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

                bool result = dbLayer.InsertRiderFeedback(orderId, riderId, locationId, customerId, rate, description,
                    decrypt.DecryptValue(connstring));

                if (result == true)
                {
                    returnData = new Messages
                    {
                        Message = string.Format("Feedback added successfully"),
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

        [Route("api/Rider/UpdateOrderReturnedReason")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage UpdateOrderReturnedReason(long orderId, int riderId, string reason)
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

                bool result = dbLayer.InsertReturnedReason(orderId, riderId, reason, decrypt.DecryptValue(connstring));

                if (result == true)
                {
                    returnData = new Messages
                    {
                        Message = string.Format("Returned reason added successfully"),
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

        [Route("api/Rider/GetRiderProfile")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetRiderProfile(int riderID)
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
                DataTable dt = dbLayer.GetRiderProfile(riderID, decrypt.DecryptValue(connstring));
                List<RiderProfile> riderProfile = new List<RiderProfile>();
                riderProfile = Helper.ConvertDataTable<RiderProfile>(dt);
                return Request.CreateResponse(HttpStatusCode.OK, new { riderProfile });
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

        [Route("api/Rider/InsertTrackingMaster")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage InsertTrackingMaster(long orderId, int riderId, int locationId, decimal distanceInKM)
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

                DataTable result = dbLayer.InsertRiderTrackingMaster(orderId, riderId, locationId, distanceInKM, decrypt.DecryptValue(connstring));

                if (result.Rows.Count > 0)
                {
                    returnData = new Messages
                    {
                        Message = string.Format("Rider tracking added successfully"),
                        Satus = result.Rows[0]["Tracking_Master_ID"].ToString(),
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

        [Route("api/Rider/InsertTrackingDetail")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage InsertTrackingDetail(long trackingMasterId, decimal latitude, decimal longitude)
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

                bool result = dbLayer.InsertTrackingDetail(trackingMasterId, latitude, longitude, decrypt.DecryptValue(connstring));

                if (result == true)
                {
                    returnData = new Messages
                    {
                        Message = string.Format("Rider tracking details added successfully"),
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

        [Route("api/Rider/GetRiderOrderHistory")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage GetRiderOrderHistory(int riderID)
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
                DataTable dt = dbLayer.GetRiderOrderHistory(riderID, decrypt.DecryptValue(connstring));
                List<RiderOrderHistory> riderOrderHistory = new List<RiderOrderHistory>();
                riderOrderHistory = Helper.ConvertDataTable<RiderOrderHistory>(dt);
                return Request.CreateResponse(HttpStatusCode.OK, new { riderOrderHistory });
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

        [Route("api/Rider/InserRiderTracking")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage InserRiderTracking(long orderId, int riderId, int locationId, decimal latitude,decimal longitude)
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

                DataTable result = dbLayer.InsertRiderTracking(orderId, riderId, locationId, latitude, longitude, decrypt.DecryptValue(connstring));

                if (result.Rows.Count > 0)
                {
                    returnData = new Messages
                    {
                        Message = string.Format("Rider tracking added successfully"),
                        Satus = result.Rows[0]["Tracking_Master_ID"].ToString(),
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