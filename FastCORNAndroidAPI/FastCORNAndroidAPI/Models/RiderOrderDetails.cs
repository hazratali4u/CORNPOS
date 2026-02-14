namespace FastAndroidAPI.Models
{
    using System;

    public partial class RiderOrderDetails
    {
        public long SALE_INVOICE_ID { get; set; }
        public int SKU_ID { get; set; }
        public string SKU_NAME { get; set; }
        public decimal QTY { get; set; }
        public decimal Amount { get; set; }
    }
}