using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Health_Shop.DataModels
{
    public class EInformation
    {
        public string Name { get; set; }
        public string OtherNames { get; set; }
        public string Danger { get; set; }
        public Color DangerColor { get; set; }
        public string Origin { get; set; }
        public Color OriginColor { get; set; }
        public string Category { get; set; }
        public string GeneralIfno { get; set; }
        public string MainParameters { get; set; }
        public string InfluenceOnTheBody { get; set; }
        public string Goods { get; set; }
        public string Poor { get; set; }
        public string Usage { get; set; }
        public string Legislation { get; set; }
    }
}