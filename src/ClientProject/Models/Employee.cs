using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ClientProject.Models
{
    public class Employee
    {
        public int Id { get; set; }
        /*public string login { get; set; }
        public string password { get; set; }
        public string mail { get; set; }*/
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public decimal Wallet { get; set; }
        /*public Boolean Admin { get; set; }*/

        public ICollection<Order> Orders { get; set; }

        public Company Company { get; set; }
        [Required]
        public ApplicationUser ApplicationUser { get; set; }
        
    }
}
