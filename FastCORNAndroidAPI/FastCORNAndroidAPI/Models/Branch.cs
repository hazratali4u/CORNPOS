namespace FastAndroidAPI.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Branch
    {
        public Branch()
        {
            CafeTimings = new List<CafeTimings>();
        }
        public int DISTRIBUTOR_ID { get; set; }
        public string DISTRIBUTOR_NAME { get; set; }
        public string ADDRESS1 { get; set; }
        public string CONTACT_NUMBER { get; set; }
        public string PIC { get; set; }
        public decimal GST { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string CompanyLogo { get; set; }
        public string Company_Name { get; set; }
        public string CITY_NAME { get; set; }
        public string Email { get; set; }
        public List<CafeTimings> CafeTimings { get; set; }
    }
}