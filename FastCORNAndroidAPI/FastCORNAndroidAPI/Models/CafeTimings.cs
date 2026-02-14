using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FastAndroidAPI.Models
{
    public partial class CafeTimings
    {
        public string OpenFrom { get; set; }
        public string OpenTo { get; set; }
        public string Day { get; set; }
        public bool IsTemporaryClosed { get; set; }
    }
}