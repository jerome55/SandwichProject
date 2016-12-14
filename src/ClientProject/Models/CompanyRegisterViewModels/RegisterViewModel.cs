using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ClientProject.Models.CompanyRegisterViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Adresse mail")]
        public string Email { get; set; }

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
        [StringLength(100, ErrorMessage = "Le {0} doit contenir entre {2} et {1} caractères.", MinimumLength = 1)]
        [Display(Name = "Prénom")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Le {0} doit contenir entre {2} et {1} caractères.", MinimumLength = 1)]
        [Display(Name = "Nom")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Téléphone")]
        public string Phone { get; set; }

        //////////////////////////////////////////////////////////////////////////////

        [Required]
        [StringLength(120, ErrorMessage = "Le {0} doit contenir entre {2} et {1} caractères.", MinimumLength = 1)]
        [Display(Name = "Nom de la compagnie")]
        public string CompanyName { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Entrer un nombre entier valide. (Exemple: 24)")]
        [RegularExpression("([0-9]+)")]
        [Display(Name = "Nombre d'employés")]
        public int NumberOfEmployee { get; set; }

        [Required]
        [StringLength(120, ErrorMessage = "La {0} doit contenir entre {2} et {1} caractères.", MinimumLength = 1)]
        [Display(Name = "Rue")]
        public string AddressStreet { get; set; }

        [Required]
        [StringLength(8, ErrorMessage = "Le {0} doit contenir entre {2} et {1} caractères.", MinimumLength = 1)]
        [Display(Name = "Numéro")]
        public string AddressNumber { get; set; }

        [StringLength(6, ErrorMessage = "La {0} doit contenir entre {2} et {1} caractères.", MinimumLength = 1)]
        [Display(Name = "Boite (optionelle)")]
        public string AddressBox { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "La {0} doit contenir entre {2} et {1} caractères.", MinimumLength = 1)]
        [Display(Name = "Ville")]
        public string AddressCity { get; set; }

        [Required]
        [StringLength(15, ErrorMessage = "Le {0} doit contenir entre {2} et {1} caractères.", MinimumLength = 1)]
        [Display(Name = "Code postal")]
        public string AddressPostalCode { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Le {0} doit contenir entre {2} et {1} caractères.", MinimumLength = 1)]
        [Display(Name = "Pays")]
        public string AddressCountry { get; set; }

    }
}
