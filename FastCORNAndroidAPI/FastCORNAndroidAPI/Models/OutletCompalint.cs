namespace FastAndroidAPI.Models
{
    using System;

    public partial class OutletCompalint
    {
        public long CustomerID { get; set; }
        public DateTime DocumentDate { get; set; }
        public int ComplaintReasonID { get; set; }
        public string Remarks { get; set; }
    }
}