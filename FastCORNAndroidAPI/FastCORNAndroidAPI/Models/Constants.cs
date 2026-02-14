using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CORNAttendanceApi.Models
{
    public class Constants
    {
        public const int IntNullValue = int.MinValue;  // int 
        public const long LongNullValue = long.MinValue; // bigint
        public static DateTime DateNullValue = new DateTime(1900, 1, 1); // DateTime
        public const decimal DecimalNullValue = decimal.MinValue; // Decimal
        public const int SKUCategory = 59; // SKU Category
        public const int SKUSubCategory = 62; // SKU Sub Category
        public const int Cash_Order_Id = 214;
        public const int Credit_Order_Id = 215;
        public const int Order_Pending_Id = 111;
        public const int CustomerChannelType = 46;
        public const int CustomerSubChannelType = 49;
    }
}