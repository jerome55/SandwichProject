﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SnackProject.Models
{
    public class Order
    {
        public int id { get; set; }
        public string dateOfDelivrery { get; set; }
        public double totalAmount { get; set; }

        public ICollection<OrderLine> orderLines { get; set; }
    }
}
