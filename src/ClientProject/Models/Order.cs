﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientProject.Models
{
    public class Order
    {
        private long ID { get; set; }
        private string MailEmploye { get; set; }
        private DateTime DateOfDelivery { get; set; }
        private double TotalPrice { get; set; }

        //liste des lignes de commande (orderLines) de la commande
        private ICollection<OrderLine> CommandLine { get; set; }
    }
}