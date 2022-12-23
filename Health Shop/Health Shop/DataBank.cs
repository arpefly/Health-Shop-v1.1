using System;
using System.Collections.Generic;
using System.Text;

using Health_Shop.DataModels;

namespace Health_Shop
{
    public static class DataBank
    {
        public static Dictionary<string, EInformation> EInfo { get; set; }
        public static Dictionary<string, ProductInformation> ProductInfo { get; set; }
    }
}