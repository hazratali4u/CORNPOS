using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FastAndroidAPI.Models
{
    public class CustomerAndUser
    {
        public int UserID { get; set; }
        public long Customer_ID { get; set; }
        public int DistributionID { get; set; }
        public int DistributorTypeID { get; set; }
        public string UserLogin { get; set; }
        [JsonIgnore]
        public string UserPassword { get; set; }
        public int IsDistributorRegister { get; set; }
        public string DistributorName { get; set; }
        public string WorkingDate { get; set; }
    }
}