using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace ClientProject.Models
{
    [DataContract]
    public class Company
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string chkCode { get; set; }
        [DataMember][NotMapped]
        [Display(Name = "Nom")]
        public string name { get; set; }
        [DataMember][NotMapped]
        [Display(Name = "Nombre d'Employés")]
        public int nbEmployes { get; set; }
        [DataMember][NotMapped]
        public string mail { get; set; }
        [DataMember][NotMapped]
        [Display(Name = "Adresse")]
        public string address { get; set; }
    }
}
