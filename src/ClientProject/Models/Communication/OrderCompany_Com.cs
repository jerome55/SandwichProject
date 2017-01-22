using ClientProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ClientProject.Models.Communication
{
    [DataContract]
    public class OrderCompany_Com
    {
        [DataMember]
        public Order Order_com { get; set; }
        [DataMember]
        public Company Company_com { get; set; }
    }
}
