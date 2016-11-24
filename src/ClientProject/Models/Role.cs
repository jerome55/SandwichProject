using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientProject.Models
{
    public class Role : IRole<int>
    {
        public int id { get; set; }
        public string name { get; set; }

        public int Id {
            get { return this.id; }
        }

        public string Name {
            get { return this.name; }
            set { this.name = value; }
        }
    }
}
