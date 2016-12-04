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
        public int Id { get; set; }
        [DataMember]
        public string Chkcode { get; set; }
        [DataMember]
        //[Index(IsUnique=true)]
        public string Name { get; set; }
        [DataMember]
        public int NbEmployees { get; set; }
        [DataMember]
        public string Mail { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public bool Status { get; set; }

        public ICollection<Order> Orders { get; set; }
        

        /*public Company(string chkcode, string name, int nbEmployes, string mail, string address, bool status, ICollection<Order> orders)
        {
            this.chkcode = chkcode;
            this.name = name;
            this.nbEmployes = nbEmployes;
            this.mail = mail;
            this.address = address;
            this.status = status;
            this.orders = orders;
        }*/
    }
}
