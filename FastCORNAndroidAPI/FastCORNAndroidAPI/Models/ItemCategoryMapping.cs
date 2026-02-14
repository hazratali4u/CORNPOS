namespace FastAndroidAPI.Models
{
    using System;
    using System.Collections.Generic;

    public partial class ItemCategoryMapping
    {
        public int SKU_ID { get; set; }
        public string EcommerceItemName { get; set; }
        public int SortOrder { get; set; }
        public string SKU_IMAGE { get; set; }
        public string DESCRIPTION { get; set; }
        public decimal Price { get; set; }
        public string Size { get; set; }
        public string Crust { get; set; }
        public string Modifier { get; set; }
        public int Modifier_SKU_ID { get; set; }
        public decimal Modifier_Price { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
    }
}