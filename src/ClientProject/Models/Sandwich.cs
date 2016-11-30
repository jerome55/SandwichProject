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
        public int Id { get; set; }
        [DataMember][NotMapped]
        public string Name { get; set; }
        [DataMember][NotMapped]
        public string Description { get; set; }
        [DataMember][NotMapped]
        public decimal Price { get; set; }
    }
}
