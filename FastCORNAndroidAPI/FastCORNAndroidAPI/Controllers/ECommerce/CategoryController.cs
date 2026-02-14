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
    public class CategoryController : ApiController
    {
        DatabaseLayer dbLayer = new DatabaseLayer();
        EncryptDecrypt decrypt = new EncryptDecrypt();
        
        [Route("api/Category/GetCategoriesList")]
        [HttpGet]
        public HttpResponseMessage GetCategoriesList()
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
                DataTable dt = dbLayer.GetCategoriesList(decrypt.DecryptValue(connstring));

                foreach (DataRow item in dt.Rows)
                {
                    if (item["ParentCategoryID"] == DBNull.Value)
                    {
                        item["ParentCategoryID"] = 0;
                    }
                }
                List<Category> categoryList = new List<Category>();
                categoryList = Helper.ConvertDataTable<Category>(dt);
                return Request.CreateResponse(HttpStatusCode.OK, new { categoryList });
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