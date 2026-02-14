namespace FastAndroidAPI.Models
{
    using System;

    public partial class CustomerOrderHistory
    {
        public string ORDER_NO { get; set; }
        public string OrderDate { get; set; }
        public decimal OrderAmount { get; set; }
        public string Delivery_Status { get; set; }
    }
}