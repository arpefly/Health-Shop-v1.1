using System;
using System.Collections.Generic;
using System.Text;

namespace Health_Shop.DataModels
{
    public class ProductInformation
    {
        public string ProductTitle { get; set; }
        public string ProductComposition { get; set; }
        public string ProductDescription { get; set; }
        public string[] EnergyComposition { get; set; }
    }
}