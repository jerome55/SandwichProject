using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SnackProject.Models
{
    public class Company
    {
        public int id { get; set; }
        public string name { get; set; }
        public int nbEmployes { get; set; }
        public string mail { get; set; }
        public string address { get; set; }
        public bool status { get; set; }
        public string chkcode { get; set; }

        public ICollection<Order> orders { get; set; }
    }
}
