namespace FastAndroidAPI.Models
{
    using System;

    public partial class CustomerProfile
    {
        public string EMAIL_ADDRESS { get; set; }
        public string CUSTOMER_NAME { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string CONTACT_NUMBER { get; set; }
    }
}