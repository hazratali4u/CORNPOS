namespace FastAndroidAPI.Models
{
    using System;

    public partial class OutletUnOrder
    {
        public int SectionID { get; set; }
        public long OutletID { get; set; }
        public DateTime DocumentDate { get; set; }
        public int ReasonID { get; set; }
        public DateTime TimeIn { get; set; }
        public DateTime TimeOut { get; set; }
        public string Comments { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string PhotoPath1 { get; set; }
        public string PhotoPath2 { get; set; }
        public string PhotoPath3 { get; set; }
        public string PhotoPath4 { get; set; }
        public string PhotoPath5 { get; set; }
    }
}