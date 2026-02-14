namespace FastAndroidAPI.Models
{
    using System;

    public partial class User
    {
        public int USER_ID { get; set; }
        public int COMPANY_ID { get; set; }
        public int DISTRIBUTOR_ID { get; set; }
        public int ROLE_ID { get; set; }
        public int SUBZONE_ID { get; set; }
        public string LOGIN_ID { get; set; }
        public string PASSWORD { get; set; }
        public bool IS_ACTIVE { get; set; }
        public DateTime LASTUPDATE_DATE { get; set; }
        public int IS_REGISTERED { get; set; }
        public string PIN { get; set; }
    }
}