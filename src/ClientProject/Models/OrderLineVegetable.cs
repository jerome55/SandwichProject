using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ClientProject.Models
{
    [DataContract]
    public class OrderLineVegetable
    {
        public virtual OrderLine OrderLine { get; set; }
        public int OrderLineId { get; set; }

        [DataMember]
        public virtual Vegetable Vegetable { get; set; }
        public int VegetableId { get; set; }
    }
}
