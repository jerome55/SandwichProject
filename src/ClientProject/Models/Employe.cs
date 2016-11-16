using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientProject.Models
{
    public class Employe
    {
        public string Mail { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public double Wallet { get; private set; }
        public Boolean Admin { get; set; }

        //liste des commandes (orders) de l'employe
        public ICollection<Order> Orders { get; set; }
        



        /**
         * Methode qui debite le porte-monnaie/credit (wallet) de l'employe du prix total de sa commande
         * param : totalprice : prix totale de sa commande
         * pre : on suppose que le prix est positif, et que la validation de sa commande a pu se faire
         * return : true si apres le debit, wallet >= 0, false sinon
         */
        public Boolean debitWallet(double totalprice)
        {
            if (this.Wallet - totalprice >= 0)
            {
                this.Wallet -= totalprice;
                return true;
            }
            else return false;
        }

        /**
         * Methode qui credite le porte-monnaie/credit (wallet) de l'employe de la somme passée en parametre
         * param : credit : somme a rajouter a wallet
         * return : true si credit > 0, false sinon
         */
        public Boolean creditWallet(double credit)
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
