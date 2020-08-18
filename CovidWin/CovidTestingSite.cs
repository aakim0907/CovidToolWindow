using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovidWin
{
    class CovidTestingSite
    {
        public string name { get; set; }
        public string alternate_name { get; set; }
        public string description { get; set; }
        public string updated { get; set; }
        public List<Address>? physical_address { get; set; }
        public List<Number>? phones { get; set; }

        internal class Address
        {
            public string address_1 { get; set; }
            public string region { get; set; }
            public string city { get; set; }
            public string postal_code { get; set; }
        }

        internal class Number
        {
            public string number { get; set; }
        }
    }
}
