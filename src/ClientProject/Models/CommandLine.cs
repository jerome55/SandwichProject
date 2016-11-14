using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientProject.Models
{
    public class CommandLine
    {
        private long ID { get; set; }
        private long IDOrders { get; set; }
        private int quantity { get; set; }
        private string nameSandwich { get; set; }
        private double price { get; set; }

        //liste des crudites pour le sandwich de la ligne de commande
        private ICollection<Crudity> Crudities { get; set; }
    }
}
