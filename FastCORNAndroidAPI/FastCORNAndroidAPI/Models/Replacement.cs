namespace FastAndroidAPI.Models
{
    using System;
    using System.Collections.Generic;

    public partial class Replacement
    {
        public int ReasonID { get; set; }
        public int SKUID { get; set; }
        public string BATCHNO { get; set; }
        public decimal Price { get; set; }     
        public int Quantity { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string Description { get; set; }
        public string StockImage1 { get; set; }
        public string StockImage2 { get; set; }
        public string StockImage3 { get; set; }
        public string StockImage4 { get; set; }
        public string StockImage5 { get; set; }
        public string InvoiceImage1 { get; set; }
        public string InvoiceImage2 { get; set; }
        public string InvoiceImage3 { get; set; }
        public string InvoiceImage4 { get; set; }
        public string InvoiceImage5 { get; set; }
    }
}