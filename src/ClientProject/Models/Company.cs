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
        [Key][DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [DataMember]
        public string ChkCode { get; set; }
        [Display(Name = "Nom")]
        [DataMember][NotMapped]
        public string Name { get; set; }
        [Display(Name = "Nombre d'employés")]
        [DataMember][NotMapped]
        public int NbEmployees { get; set; }
        [Display(Name = "Adresse mail")]
        [DataMember][NotMapped]
        public string Mail { get; set; }
        [Display(Name = "Adresse")]
        [DataMember][NotMapped]
        public string Address { get; set; }
        [Display(Name = "Status")]
        [DataMember][NotMapped]
        public bool Status { get; set; }


        //Just a test
        public override bool Equals(System.Object obj)
        {
            var item = obj as Company;

            if(item == null) { return false; }
            return (this.Id == item.Id && this.ChkCode == item.ChkCode);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
