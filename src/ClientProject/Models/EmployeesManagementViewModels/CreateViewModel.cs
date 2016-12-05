using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ClientProject.Models.EmployeesManagementViewModels
{
    public class CreateViewModel
    {
        [Required]
        [StringLength(60, ErrorMessage = "Le {0} doit contenir entre {2} et {1} caractères.", MinimumLength = 2)]
        [Display(Name = "Nom d'utilisateur")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Le {0} doit contenir entre {2} et {1} caractères.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmer mot de passe")]
        [Compare("Password", ErrorMessage = "La confirmation et le mot de passe de correspondent pas.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Adresse mail")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Le {0} doit contenir entre {2} et {1} caractères.", MinimumLength = 1)]
        [Display(Name = "Prénom")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Le {0} doit contenir entre {2} et {1} caractères.", MinimumLength = 1)]
        [Display(Name = "Nom")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Display(Name = "Porte-monnaie")]
        public decimal Wallet { get; set; }
        
    }
}
