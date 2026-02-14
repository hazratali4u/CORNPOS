namespace FastAndroidAPI.Models
{
    using System;

    public partial class ItemDeals
    {
        public string Deal_Item { get; set; }
        public int Deal_Item_ID { get; set; }
        public int Deal_Item_Qty { get; set; }
        public int Deal_Item_Category_ID { get; set; }
        public string Deal_Item_Category { get; set; }
        public string Main_Item { get; set; }
        public int Main_Item_ID { get; set; }
        public int Main_Item_Qty { get; set; }
        public int DEAL_ID { get; set; }
        public long Deal_Detail_ID { get; set; }
        public decimal Deal_Price { get; set; }
        public string Deal_Item_Image { get; set; }
        public string Main_Item_Image { get; set; }
        public string Deal_Description { get; set; }
    }
}