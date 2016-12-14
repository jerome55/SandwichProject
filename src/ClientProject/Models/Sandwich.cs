using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ClientProject.Models
{
    [DataContract]
    public class Sandwich
    {
        [DataMember]
        [Key][DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [Display(Name = "Nom")]
        [DataMember][NotMapped]
        public string Name { get; set; }
        [Display(Name = "Description")]
        [DataMember][NotMapped]
        public string Description { get; set; }
        [Display(Name = "Prix")]
        [DataMember][NotMapped]
        public decimal Price { get; set; }
    }
}
