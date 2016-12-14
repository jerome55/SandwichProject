using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace SnackProject.Models.VegetableViewModels
{
    public class VegetableViewModel {
        public IList<Vegetable> Vegetables { get; set; }
        [Display(Name = "Tarif du supplément crudité")]
        [DataMember]
        public decimal VegetablesPrice { get; set; }
    }
}
