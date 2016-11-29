using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace SnackProject.Models
{
    [DataContract]
    public class Company
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string chkcode { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public int nbEmployes { get; set; }
        [DataMember]
        public string mail { get; set; }
        [DataMember]
        public string address { get; set; }
        public bool status { get; set; }

        public ICollection<Order> orders { get; set; }
    }
}
