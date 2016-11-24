using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ClientProject.Models
{
    //Pas besoin d'indiquer de [NotMapped] partout pour Menu (Côté Client)
    //On va simplement pas le lister dans le fichier ApplicationDbContext (Côté Client)
    [DataContract]
    public class Menu
    {
        [DataMember][NotMapped]
        public decimal vegetablesPrice { get; set; }

        [DataMember][NotMapped]
        public ICollection<Sandwich> sandwiches { get; set; }
        [DataMember][NotMapped]
        public ICollection<Vegetable> vegetables { get; set; }

    }
}
