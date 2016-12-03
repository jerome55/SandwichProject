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
        [DataMember][NotMapped]
        public string Name { get; set; }
        [DataMember][NotMapped]
        public int NbEmployees { get; set; }
        [DataMember][NotMapped]
        public string Mail { get; set; }
        [DataMember][NotMapped]
        public string Address { get; set; }
        [NotMapped]
        public bool Status { get; set; }
    }
}
