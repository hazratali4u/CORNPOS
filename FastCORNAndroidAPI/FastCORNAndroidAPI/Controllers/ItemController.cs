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
    public class ItemController : ApiController
    {
        DatabaseLayer dbLayer = new DatabaseLayer();
        EncryptDecrypt decrypt = new EncryptDecrypt();
        
        [Route("api/Item/InsertReplacement")]
        [HttpPost]
        [Authorize]
        public HttpResponseMessage InsertReplacement(int DistributorID,long CustomerID,DateTime DocumentDate,int ReplacementTypeID, int UserID, List<Replacement> dtReplacement)
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
                
                bool flag = dbLayer.InsertReplacement(DistributorID,CustomerID,DocumentDate,ReplacementTypeID, UserID, dtReplacement, decrypt.DecryptValue(connstring));

                if (flag)
                {
                    returnData = new Messages
                    {
                        Message = dtReplacement.Count.ToString() + " Row(s) inserted",
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

        [Route("api/Item/GetItemsINCategory")]
        [HttpGet]
        //[Authorize]
        public HttpResponseMessage GetItemsINCategory(int categoryID, int locationId)
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

                DataTable dt = dbLayer.GetItemsINCategory(categoryID, 3, locationId, decrypt.DecryptValue(connstring));

                List<ItemCategoryMapping> itemsList = new List<ItemCategoryMapping>();

                foreach (DataRow item in dt.Rows)
                {
                    if (item["Modifier"] == DBNull.Value)
                    {
                        item["Modifier"] = string.Empty;
                    }
                    if (item["Modifier_SKU_ID"] == DBNull.Value)
                    {
                        item["Modifier_SKU_ID"] = 0;
                    }
                    if (item["Price"] == DBNull.Value)
                    {
                        item["Price"] = 0;
                    }
                    if (item["Modifier_Price"] == DBNull.Value)
                    {
                        item["Modifier_Price"] = 0;
                    }
                }

                itemsList = Helper.ConvertDataTable<ItemCategoryMapping>(dt);

                var UniquegroupedItemList = itemsList.GroupBy(x => x.SKU_ID).Select(x => x.FirstOrDefault()).ToList();

                var uniqueItemList = new List<Item>();

                foreach (var item in UniquegroupedItemList)
                {
                    Item obj = new Item();
                    obj.SKU_ID = item.SKU_ID;
                    obj.EcommerceItemName = item.EcommerceItemName;
                    obj.SortOrder = item.SortOrder;
                    obj.SKU_IMAGE = item.SKU_IMAGE;
                    obj.DESCRIPTION = item.DESCRIPTION;
                    obj.Price = item.Price;
                    obj.Size = item.Size;
                    obj.Crust = item.Crust;
                    obj.CategoryID = item.CategoryID;
                    obj.CategoryName = item.CategoryName;

                    var singleItem = itemsList.Where(x => x.SKU_ID == item.SKU_ID).ToList();

                    foreach (var itm in singleItem)
                    {
                        obj.ModifierList.Add(new ModifierList
                        {
                            Modifier = itm.Modifier,
                            Modifier_SKU_ID = itm.Modifier_SKU_ID,
                            Modifier_Price = itm.Modifier_Price
                        });
                    }

                    uniqueItemList.Add(obj);
                }

                return Request.CreateResponse(HttpStatusCode.OK, new { uniqueItemList });
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
        [Route("api/Item/GetAllItems")]
        [HttpGet]
        //[Authorize]
        public HttpResponseMessage GetAllItems(int locationId)
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

                DataTable dt = dbLayer.GetItemsINCategory(Constants.IntNullValue, 3, locationId, decrypt.DecryptValue(connstring));

                List<ItemCategoryMapping> itemsList = new List<ItemCategoryMapping>();

                foreach (DataRow item in dt.Rows)
                {
                    if (item["Modifier"] == DBNull.Value)
                    {
                        item["Modifier"] = string.Empty;
                    }
                    if (item["Modifier_SKU_ID"] == DBNull.Value)
                    {
                        item["Modifier_SKU_ID"] = 0;
                    }
                    if (item["Price"] == DBNull.Value)
                    {
                        item["Price"] = 0;
                    }
                    if (item["Modifier_Price"] == DBNull.Value)
                    {
                        item["Modifier_Price"] = 0;
                    }
                }

                itemsList = Helper.ConvertDataTable<ItemCategoryMapping>(dt);

                var UniquegroupedItemList = itemsList.GroupBy(x => x.SKU_ID).Select(x => x.FirstOrDefault()).ToList();

                var uniqueItemList = new List<Item>();

                foreach (var item in UniquegroupedItemList)
                {
                    Item obj = new Item();
                    obj.SKU_ID = item.SKU_ID;
                    obj.EcommerceItemName = item.EcommerceItemName;
                    obj.SortOrder = item.SortOrder;
                    obj.SKU_IMAGE = item.SKU_IMAGE;
                    obj.DESCRIPTION = item.DESCRIPTION;
                    obj.Price = item.Price;
                    obj.Size = item.Size;
                    obj.Crust = item.Crust;
                    obj.CategoryID = item.CategoryID;
                    obj.CategoryName = item.CategoryName;

                    var singleItem = itemsList.Where(x => x.SKU_ID == item.SKU_ID).ToList();

                    foreach (var itm in singleItem)
                    {
                        obj.ModifierList.Add(new ModifierList
                        {
                            Modifier = itm.Modifier,
                            Modifier_SKU_ID = itm.Modifier_SKU_ID,
                            Modifier_Price = itm.Modifier_Price
                        });
                    }

                    uniqueItemList.Add(obj);
                }

                return Request.CreateResponse(HttpStatusCode.OK, new { uniqueItemList });
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

        [Route("api/Item/GetItemById")]
        [HttpGet]
        //[Authorize]
        public HttpResponseMessage GetItemById(string skuIds, int locationId)
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

                DataTable dt = dbLayer.GetECommerceItemById(skuIds, locationId, decrypt.DecryptValue(connstring));

                List<ItemCategoryMapping> itemsList = new List<ItemCategoryMapping>();

                foreach (DataRow item in dt.Rows)
                {
                    if (item["Modifier"] == DBNull.Value)
                    {
                        item["Modifier"] = string.Empty;
                    }
                    if (item["Modifier_SKU_ID"] == DBNull.Value)
                    {
                        item["Modifier_SKU_ID"] = 0;
                    }
                    if (item["Price"] == DBNull.Value)
                    {
                        item["Price"] = 0;
                    }
                    if (item["Modifier_Price"] == DBNull.Value)
                    {
                        item["Modifier_Price"] = 0;
                    }
                }

                itemsList = Helper.ConvertDataTable<ItemCategoryMapping>(dt);

                var UniquegroupedItemList = itemsList.GroupBy(x => x.SKU_ID).Select(x => x.FirstOrDefault()).ToList();

                var uniqueItemList = new List<Item>();

                foreach (var item in UniquegroupedItemList)
                {
                    Item obj = new Item();
                    obj.SKU_ID = item.SKU_ID;
                    obj.EcommerceItemName = item.EcommerceItemName;
                    obj.SortOrder = item.SortOrder;
                    obj.SKU_IMAGE = item.SKU_IMAGE;
                    obj.DESCRIPTION = item.DESCRIPTION;
                    obj.Price = item.Price;
                    obj.Size = item.Size;
                    obj.Crust = item.Crust;

                    var singleItem = itemsList.Where(x => x.SKU_ID == item.SKU_ID).ToList();

                    foreach (var itm in singleItem)
                    {
                        obj.ModifierList.Add(new ModifierList
                        {
                            Modifier = itm.Modifier,
                            Modifier_SKU_ID = itm.Modifier_SKU_ID,
                            Modifier_Price = itm.Modifier_Price
                        });
                    }

                    uniqueItemList.Add(obj);
                }

                return Request.CreateResponse(HttpStatusCode.OK, new { uniqueItemList });
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

        [Route("api/Item/GetItemDeals")]
        [HttpGet]
        //[Authorize]
        public HttpResponseMessage GetItemDeals()
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
                DataTable dt = dbLayer.GetItemDeals(decrypt.DecryptValue(connstring));

                List<ItemDeals> customerOrderHistory = new List<ItemDeals>();
                customerOrderHistory = Helper.ConvertDataTable<ItemDeals>(dt);

                var uniqueDealList = customerOrderHistory.GroupBy(x => x.DEAL_ID).Select(x => x.FirstOrDefault()).ToList();

                List<Deal> dealList = new List<Deal>();

                foreach (var item in uniqueDealList)
                {
                    Deal deal = new Deal();
                    deal.Main_Item = item.Main_Item;
                    deal.Main_Item_ID = item.Main_Item_ID;
                    deal.Deal_Price = item.Deal_Price;
                    deal.Deal_Assignment_ID = item.DEAL_ID;
                    deal.Main_Item_Image = item.Main_Item_Image;
                    deal.Main_Item_Qty = item.Main_Item_Qty;
                    deal.Deal_Description = item.Deal_Description;

                    var dealItems = customerOrderHistory.Where(x => x.DEAL_ID == item.DEAL_ID).ToList();

                    foreach (var dealItem in dealItems)
                    {
                        deal.DealItems.Add(new DealItem
                        {
                            Deal_Item = dealItem.Deal_Item,
                            Deal_Item_ID = dealItem.Deal_Item_ID,
                            Deal_Item_Image = dealItem.Deal_Item_Image,
                            Deal_Item_Qty = dealItem.Deal_Item_Qty,
                            Deal_Item_Category = dealItem.Deal_Item_Category,
                            Deal_Item_Category_ID = dealItem.Deal_Item_Category_ID,
                            Deal_Assignment_Detail_ID = dealItem.Deal_Detail_ID
                        });
                    }

                    dealList.Add(deal);
                }

                return Request.CreateResponse(HttpStatusCode.OK, new { dealList });
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

        [Route("api/Item/GetItemDealById")]
        [HttpGet]
        //[Authorize]
        public HttpResponseMessage GetItemDealById(string dealId)
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
                DataTable dt = dbLayer.GetDealById(dealId, decrypt.DecryptValue(connstring));

                List<ItemDeals> dealList = new List<ItemDeals>();
                dealList = Helper.ConvertDataTable<ItemDeals>(dt);

                return Request.CreateResponse(HttpStatusCode.OK, new { dealList });
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
    public class Deal
    {
        public Deal()
        {
            DealItems = new List<DealItem>();
        }
        public string Main_Item { get; set; }
        public int Main_Item_ID { get; set; }
        public string Main_Item_Image { get; set; }
        public int Main_Item_Qty { get; set; }
        public decimal Deal_Price { get; set; }
        public string Deal_Description { get; set; }
        public int Deal_Assignment_ID { get; set; }
        public List<DealItem> DealItems { get; set; }
    }
    public class DealItem
    {
        public int Deal_Item_ID { get; set; }
        public string Deal_Item { get; set; }
        public string Deal_Item_Image { get; set; }
        public int Deal_Item_Qty { get; set; }
        public int Deal_Item_Category_ID { get; set; }
        public string Deal_Item_Category { get; set; }
        public long Deal_Assignment_Detail_ID { get; set; }
    }

    public class Item
    {
        public Item()
        {
            ModifierList = new List<ModifierList>();
        }
        public int SKU_ID { get; set; }
        public string EcommerceItemName { get; set; }
        public int SortOrder { get; set; }
        public string SKU_IMAGE { get; set; }
        public string DESCRIPTION { get; set; }
        public decimal Price { get; set; }
        public string Size { get; set; }
        public string Crust { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public List<ModifierList> ModifierList { get; set; }
    }

    public class ModifierList
    {
        public string Modifier { get; set; }
        public int Modifier_SKU_ID { get; set; }
        public decimal Modifier_Price { get; set; }
    }
}