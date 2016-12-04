using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace SnackProject.Models
{
    [DataContract]
    public class Vegetable
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Description { get; set; }
        public bool Available { get; set; }

        
        /*public Vegetable(string name, string description, bool available)
        {
            this.Name = name;
            this.Description = description;
            this.Available = available;
        }*/
    }
}
