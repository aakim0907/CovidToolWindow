using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovidWin
{
    public class SiteItem
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Updated { get; set; }
        public string Address { get; set; }
        public string Number { get; set; }

        public SiteItem (string name, string desc, string updated, string address, string number)
        {
            this.Name = name;
            this.Description = desc;
            this.Updated = updated;
            this.Address = address;
            this.Number = number;
        }
    }
}
