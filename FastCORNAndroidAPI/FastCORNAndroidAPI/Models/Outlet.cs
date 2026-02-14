namespace FastAndroidAPI.Models
{
    using System;

    public partial class Outlet
    {
        public string OutletName { get; set; }
        public string OwnerName { get; set; }
        public string PhoneNumber { get; set; }
        public string OutletAddress { get; set; }
        public int SectionID { get; set; }
        public int ChannelID { get; set; }
        public int TownID { get; set; }        
        public int AreaTypeId { get; set; }
        public int SubChannelID { get; set; }
        public string Comments { get; set; }
        public decimal Latitude { set; get; }
        public decimal Longtidue { get; set; }
        public string PhotoPath1 { get; set; }
        public string PhotoPath2 { get; set; }
        public string PhotoPath3 { get; set; }
        public string PhotoPath4 { get; set; }
        public string PhotoPath5 { get; set; }
        public string LandMark { get; set; }
    }
}