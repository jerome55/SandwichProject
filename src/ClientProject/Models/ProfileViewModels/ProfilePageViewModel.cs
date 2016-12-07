using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientProject.Models.ProfileViewModels
{
    public class ProfilePageViewModel
    {
        private Employee employee { get; set; }
        private List<Order> orderList { get; set; }
        public ProfilePageViewModel(Employee employee, List<Order> orderList)
        {
            this.employee = employee;
            this.orderList = orderList;
        }
    }
}
