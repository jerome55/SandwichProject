using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SnackProject.Models.OrderViewModels
{
    public class OrderViewModel
    {
        public List<OrderLine> OrderLines { get; set; }
        public IList<Company> Companies { get; set; }
        public IList<Sandwich> Sandwiches { get; set; }
        [Display(Name = "Montant total de la commande")]
        public Decimal Total { get; set; }
    }
}
