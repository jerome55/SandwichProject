using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientProject.Models
{
    public class Employe
    {
        private string Mail { get; set; }
        private string LastName { get; set; }
        private string FirstName { get; set; }
        private string Login { get; set; }
        private string Password { get; set; }
        private double Wallet { get; set; }
        private Boolean Admin { get; set; }

        //liste des commandes (orders) de l'employe
        private ICollection<Command> Orders { get; set; }
    }
}
