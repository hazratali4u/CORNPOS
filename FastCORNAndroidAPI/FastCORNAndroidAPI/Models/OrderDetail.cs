namespace FastAndroidAPI.Models
{
    using System;
    using System.Collections.Generic;

    public partial class OrderDetail
    {
        public int SKUID { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal PRODUCT_CATEGORY_ID { get; set; }
        public int Deal_Assignment_ID { get; set; }
        public int ITEM_DEAL_ID { get; set; }
        public long Deal_Assignment_Detail_ID { get; set; }
        public decimal Deal_Qty { get; set; }
        public decimal Deal_Item_Qty { get; set; }
        public decimal Deal_Price { get; set; }
    }    
}