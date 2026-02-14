namespace FastAndroidAPI.Models
{
    using System;
    using System.Collections.Generic;

    public partial class StockPosition
    {
        public int SKUID { get; set; }
        public int Quantity { get; set; }      
        public int Unit_In_Case { get; set; }
        public decimal Price { get; set; }
    }
}