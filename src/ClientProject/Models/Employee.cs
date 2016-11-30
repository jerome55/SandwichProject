﻿using System;
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


/**
         * Methode qui debite le porte-monnaie/credit (wallet) de l'employe du prix total de sa commande
         * param : totalAmount : prix totale de sa commande
         * pre : on suppose que le prix est positif, et que la validation de sa commande a pu se faire
         * return : true si apres le debit, wallet >= 0, false sinon
         */
/*public Boolean debitWallet(decimal totalAmount)
{
    if (this.wallet - totalAmount >= 0)
    {
        this.wallet -= totalAmount;
        return true;
    }
    else return false;
}*/

/**
 * Methode qui credite le porte-monnaie/credit (wallet) de l'employe de la somme passée en parametre
 * param : credit : somme a rajouter a wallet
 * return : true si credit > 0, false sinon
 */
/*public Boolean creditWallet(decimal credit)
{
    if (credit > 0)
    {
        this.wallet += credit;
        return true;
    }
    else return false;
}*/