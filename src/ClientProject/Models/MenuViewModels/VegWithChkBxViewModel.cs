using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ClientProject.Models.MenuViewModels
{
    public class VegWithChkBxViewModel
    {
        public bool Checked { get; set; }
        public int Id { get; set; }
        [Display(Name = "Nom")]
        public string Name { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
    }
}
