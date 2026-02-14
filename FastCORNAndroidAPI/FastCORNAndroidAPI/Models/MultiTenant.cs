namespace FastAndroidAPI.Models
{
    using System;

    public partial class MultiTenant
    {
        public int ClientID { get; set; }
        public string ClientConnString { get; set; }
        public string BackGroundImage { get; set; }
        public string Logo { get; set; }
        public string ImageServerUrl { get; set; }
        public bool IsSuccess { get; set; }
    }
    public partial class MultiTenant2
    {
        public int ClientID { get; set; }
        public string ClientConnString { get; set; }
        public string Message { get; set; }
        public string BackGroundImage { get; set; }
        public string Logo { get; set; }
        public string ImageServerUrl { get; set; }
        public bool Status { get; set; }
    }
}