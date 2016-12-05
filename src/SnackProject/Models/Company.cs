using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Display(Name = "Nom")]
        //[Index(IsUnique=true)]
        public string Name { get; set; }
        [Display(Name = "Nombre d'employés")]
        [DataMember]
        public int NbEmployees { get; set; }
        [Display(Name = "Adresse mail")]
        [DataMember]
        public string Mail { get; set; }
        [Display(Name = "Adresse")]
        [DataMember]
        public string Address { get; set; }
        [Display(Name = "Status")]
        [DataMember]
        public bool Status { get; set; }

        public ICollection<Order> Orders { get; set; }
        

        /*public Company(string chkcode, string name, int nbEmployes, string mail, string address, bool status, ICollection<Order> orders)
        {
            this.Chkcode = chkcode;
            this.Name = name;
            this.NbEmployes = nbEmployes;
            this.Mail = mail;
            this.Address = address;
            this.Status = status;
            this.Orders = orders;
        }*/
    }
}
