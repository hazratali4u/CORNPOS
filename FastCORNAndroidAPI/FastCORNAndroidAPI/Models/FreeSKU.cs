namespace FastAndroidAPI.Models
{
    using System;

    public partial class FreeSKU
    {
        public int PromotionID { get; set; }
        public int BasketID { get; set; }
        public int BasketDetailID { get; set; }
        public int PromotionOfferID { get; set; }
        public int SKUID { get; set; }
        public int Quantity { get; set; }
        public int MasterSKUID { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
        public decimal GSTAmount { get; set; }
        public decimal TSTAmount { get; set; }
        public decimal GSTRate { get; set; }
        public decimal ExtraTax { get; set; }
        public decimal QuantityTo { get; set; }
        public decimal QuantityAd { get; set; }
    }
}