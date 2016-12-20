using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace SnackProject.Models
{
    [DataContract]
    public class Order
    {
        public int Id { get; set; }
        [Display(Name = "Date de livraison")]
        [DataMember]
        public DateTime DateOfDelivery { get; set; }
        [Display(Name = "Montant total")]
        [DataMember]
        public decimal TotalAmount { get; set; }

        //Concurrency Token, voir le code fluent dans le fichier ApplicationDbContext.
        //Lorsque plusieurs clients d'une même société soumettent leurs Orders au snack en même temps, 
        //le Order total pour cette société peut subir des updates en concurrence.
        //Ce concurrency token permet de savoir lors d'une update si une autre update n'est pas venue
        //s'infiltrer entre temps.
        //Pour plus d'info : https://www.asp.net/mvc/overview/getting-started/getting-started-with-ef-using-mvc/handling-concurrency-with-the-entity-framework-in-an-asp-net-mvc-application  
        public byte[] RowVersion { get; set; }

        [DataMember]
        public ICollection<OrderLine> OrderLines { get; set; }

        
        /*public Order(DateTime dateOfDelivery, decimal totalAmount, ICollection<OrderLine> orderLines)
        {
            this.DateOfDelivery = dateOfDelivery;
            this.TotalAmount = totalAmount;
            this.OrderLines = orderLines;
        }*/

        public void AddOrderLine(ICollection<OrderLine> AddOrderLines)
        {
            lock(this)
            {
                //List<OrderLine> listAdd = AddOrderLines.ToList();
                //List<OrderLine> listCurrent = OrderLines.ToList();

                for(int i=0;i<AddOrderLines.Count;++i)
                {
                    int j = 0;
                    for (j = 0; j < OrderLines.Count && !OrderLines.ElementAt(j).Equals(listAdd); ++j);

                    if(j == listCurrent.Count)
                    {
                        OrderLines.Add(listAdd[i]);
                        TotalAmount += listAdd[i].GetPrice();
                    }
                    else
                    {
                        listCurrent[j].Quantity += listAdd[i].Quantity;
                        TotalAmount += listAdd[i].GetPrice();
                    }
                }
            }
        }
    }
}
