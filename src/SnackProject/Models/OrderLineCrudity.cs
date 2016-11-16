using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SnackProject.Models
{
    public class OrderLineCrudity
    {
        public Crudity Crudities { get; set; }
        public int CruditiesID { get; set; }
        public Order Orders { get; set; }
        public int OrdersID { get; set; }

    }
}
