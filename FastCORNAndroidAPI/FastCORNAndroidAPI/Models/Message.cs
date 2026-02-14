namespace FastAndroidAPI.Models
{
    using Newtonsoft.Json.Linq;
    using System;
    using System.Net.Http;

    public partial class Messages
    {
        public string Message { get; set; }
        public string Satus { get; set; }
    }
    public partial class Messages2
    {
        public string Message { get; set; }
        public JObject Rows { get; set; }
        public bool Status { get; set; }
    }
}