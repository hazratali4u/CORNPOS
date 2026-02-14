namespace FastAndroidAPI.Models
{
    using System;

    public partial class ChannelSub
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int ParentID { get; set; }
        public bool Status { get; set; }
    }
}