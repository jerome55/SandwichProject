using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ClientProject.Models.EmployeesManagementViewModels
{
    public class AddToWalletViewModel
    {
        public string Id { get; set; }

        public string UserName { get; set; }
        public string Email { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public decimal Wallet { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Display(Name = "Créditer de : ")]
        public decimal AddToWallet { get; set; }
    }
}
