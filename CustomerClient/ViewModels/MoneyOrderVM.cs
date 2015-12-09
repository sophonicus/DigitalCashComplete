using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CustomerClient.Models.Entities;

namespace CustomerClient.ViewModels
{
    public class MoneyOrderVM
    {
        public decimal? Amount { get; set; }
        public List<Models.Entities.MoneyOrder> Requests { get; set; }
        public List<Models.Entities.MoneyOrder> MoneyOrders { get; set; }
    }
}