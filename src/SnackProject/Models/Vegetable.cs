using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace SnackProject.Models
{
    [DataContract]
    public class Vegetable
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        [Display(Name = "Nom")]
        public string Name { get; set; }
        [DataMember]
        [Display(Name = "Description")]
        public string Description { get; set; }
        [DataMember]
        [Display(Name = "Disponible")]
        public bool Available { get; set; }
    }
}
