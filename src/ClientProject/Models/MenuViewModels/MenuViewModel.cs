using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ClientProject.Models.MenuViewModels
{
    public class MenuViewModel
    {
        public List<Sandwich> ListSandwiches { get; set; }
        public string SelectedSandwich { get; set; }

        public decimal VegetablesPrice { get; set; }
        public List<VegWithChkBxViewModel> ListVegetablesWithCheckBoxes { get; set; }
    }
}
