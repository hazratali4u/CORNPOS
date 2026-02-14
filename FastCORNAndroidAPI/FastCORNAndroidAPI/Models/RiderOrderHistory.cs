namespace FastAndroidAPI.Models
{
    using System;

    public partial class RiderOrderHistory
    {
        public int DeliveredOrder { get; set; }
        public int CancelledOrder { get; set; }
        public decimal TotalDistance { get; set; }
        public string TimePeriod { get; set; }
    }
}