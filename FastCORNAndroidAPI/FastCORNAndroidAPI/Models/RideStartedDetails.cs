namespace FastAndroidAPI.Models
{
    using System;

    public partial class RideStartedDetails
    {
        public long SALE_INVOICE_ID { get; set; }
        public decimal Amount { get; set; }
        public long Customer_ID { get; set; }
        public string CUSTOMER_NAME { get; set; }
        public string CONTACT_NUMBER { get; set; }
        public string Customer_ADDRESS { get; set; }
        public string ORDER_DATE { get; set; }
        public int NO_OF_PARCEL { get; set; }
    }
}