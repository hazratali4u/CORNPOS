using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FastAndroidAPI.Models
{
    public partial class MainBranch
    {
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
        public string OpenFrom { get; set; }
        public string OpenTo { get; set; }
        public string Day { get; set; }
        public bool IsTemporaryClosed { get; set; }
        public int? CITY_ID { get; set; }
        public string CITY_NAME { get; set; }
        public string Email { get; set; }
    }
}