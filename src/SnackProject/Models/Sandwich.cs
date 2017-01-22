using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace SnackProject.Models
{
    [DataContract]
    public class Sandwich {
        [DataMember]
        public int Id { get; set; }
        [Display(Name = "Nom")]
        [DataMember]
        public string Name { get; set; }
        [Display(Name = "Description")]
        [DataMember]
        public string Description { get; set; }
        [Display(Name = "Prix")]
        [DataMember]
        public decimal Price { get; set; }
        [DataMember]
        [Display(Name = "Disponible")]
        public bool Available { get; set; }
    }
}
