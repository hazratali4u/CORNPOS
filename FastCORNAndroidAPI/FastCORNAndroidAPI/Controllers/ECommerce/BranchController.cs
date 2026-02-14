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

namespace FastAndroidAPI.Controllers
{
    public class BranchController : ApiController
    {
        DatabaseLayer dbLayer = new DatabaseLayer();
        EncryptDecrypt decrypt = new EncryptDecrypt();
        
        [Route("api/Branch/GetBranches")]
        [HttpGet]
        public HttpResponseMessage GetBranches()
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
                DataTable dt = dbLayer.GetBranches(decrypt.DecryptValue(connstring));

                foreach (DataRow item in dt.Rows)
                {
                    if (item["PIC"] == DBNull.Value)
                    {
                        item["PIC"] = "";
                    }
                    if (item["Latitude"] == DBNull.Value)
                    {
                        item["Latitude"] = "";
                    }
                    if (item["Longitude"] == DBNull.Value)
                    {
                        item["Longitude"] = "";
                    }
                }
                List<MainBranch> mainBranchList = new List<MainBranch>();
                mainBranchList = Helper.ConvertDataTable<MainBranch>(dt);
                var UniquegroupedBranchList = mainBranchList.GroupBy(x => x.DISTRIBUTOR_NAME).Select(x => x.FirstOrDefault()).ToList();

                var branchList = new List<Branch>();

                foreach (var item in UniquegroupedBranchList)
                {
                    Branch obj = new Branch();
                    obj.DISTRIBUTOR_ID = item.DISTRIBUTOR_ID;
                    obj.DISTRIBUTOR_NAME = item.DISTRIBUTOR_NAME;
                    obj.ADDRESS1 = item.ADDRESS1;
                    obj.CONTACT_NUMBER = item.CONTACT_NUMBER;
                    obj.PIC = item.PIC;
                    obj.GST = item.GST;
                    obj.Latitude = item.Latitude;
                    obj.Longitude = item.Longitude;
                    obj.CompanyLogo = item.CompanyLogo;
                    obj.CITY_NAME = item.CITY_NAME;
                    obj.Email = item.Email;
                    obj.Company_Name = item.Company_Name;

                    var singleItem = mainBranchList.Where(x => x.DISTRIBUTOR_NAME == item.DISTRIBUTOR_NAME).ToList();

                    foreach (var itm in singleItem)
                    {
                        obj.CafeTimings.Add(new CafeTimings
                        {
                            Day = itm.Day,
                            OpenFrom = itm.OpenFrom,
                            OpenTo = itm.OpenTo,
                            IsTemporaryClosed = itm.IsTemporaryClosed
                        });
                    }

                    branchList.Add(obj);
                }

                return Request.CreateResponse(HttpStatusCode.OK, new { branchList });
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