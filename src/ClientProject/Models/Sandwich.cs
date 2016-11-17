using System;
using System.Collections.Generic;
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
        public int id { get; set; }
        [DataMember][NotMapped]
        public string name { get; set; }
        [DataMember][NotMapped]
        public string description { get; set; }
        [DataMember][NotMapped]
        public decimal price { get; set; }
    }
}
