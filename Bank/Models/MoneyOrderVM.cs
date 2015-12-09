using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bank.Models.Entities;

namespace Bank.ViewModels
{
    public class MoneyOrderVM
    {
        public decimal? Amount { get; set; }
        public List<Models.Entities.MoneyOrder> Requests { get; set; }
        public List<Models.Entities.MoneyOrder> CashRequests { get; set; }
        public List<Models.Entities.MoneyOrder> CashedMoneyOrders { get; set; }
    }
}