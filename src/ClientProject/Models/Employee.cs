using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ClientProject.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class Employee : IdentityUser
    {
        [Required]
        [StringLength(100, ErrorMessage = "Le {0} doit contenir entre {2} et {1} caractères.", MinimumLength = 1)]
        [Display(Name = "Prénom")]
        public string LastName { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Le {0} doit contenir entre {2} et {1} caractères.", MinimumLength = 1)]
        [Display(Name = "Nom")]
        public string FirstName { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        [Display(Name = "Porte-monnaie")]
        public decimal Wallet { get; set; }

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

        public virtual Company Company { get; set; }
        
        /**
         * Methode qui debite le porte-monnaie/credit (wallet) de l'employe du prix total de sa commande
         * param : totalAmount : prix totale de sa commande
         * pre : on suppose que le prix est positif, et que la validation de sa commande a pu se faire
         * return : true si apres le debit, wallet >= 0, false sinon
         */
        public bool DebitWallet(decimal totalAmount)
        {
            if (this.Wallet - totalAmount >= 0)
            {
                this.Wallet -= totalAmount;
                return true;
            }
            else return false;
        }

        /**
         * Methode qui credite le porte-monnaie/credit (wallet) de l'employe de la somme passée en parametre
         * param : credit : somme a rajouter a wallet
         * return : true si credit > 0, false sinon
         */
        public bool CreditWallet(decimal credit)
        {
            if (credit > 0)
            {
                this.Wallet += credit;
                return true;
            }
            else return false;
        }

    }
}
