namespace FastAndroidAPI.Models
{
    using System;

    public partial class RiderOrderList
    {
        public long SALE_INVOICE_ID { get; set; }
        public decimal Amount { get; set; }
        public long CUSTOMER_ID { get; set; }
        public string CUSTOMER_NAME { get; set; }
        public string CONTACT_NUMBER { get; set; }
        public string Customer_ADDRESS { get; set; }
        public decimal LAT { get; set; }
        public decimal LON { get; set; }
        public decimal DeliveryCharges { get; set; }
        public decimal Discount { get; set; }
        public decimal GSTAmount { get; set; }
        public string ORDER_DATE { get; set; }
        public string OrderStatus { get; set; }
    }
}