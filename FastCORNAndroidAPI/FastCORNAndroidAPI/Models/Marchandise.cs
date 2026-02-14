namespace FastAndroidAPI.Models
{
    using System;
    using System.Collections.Generic;

    public partial class Marchandise
    {
        public long OutletID { get; set; }
        public DateTime DocumentDate { get; set; }
        public string Remarks { get; set; }
        public string BFMarchanImg1 { get; set; }
        public string BFMarchanImg2 { get; set; }
        public string BFMarchanImg3 { get; set; }
        public string BFMarchanImg4 { get; set; }
        public string BFMarchanImg5 { get; set; }
        public string AFMarchanImg1 { get; set; }
        public string AFMarchanImg2 { get; set; }
        public string AFMarchanImg3 { get; set; }
        public string AFMarchanImg4 { get; set; }
        public string AFMarchanImg5 { get; set; }
    }
}