//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CustomerClient.Models.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class MoneyOrder
    {
        public int Id { get; set; }
        public string UniqueId { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public bool Signed { get; set; }
        public string Signature { get; set; }
        public string BlindSignature { get; set; }
        public bool Cashed { get; set; }
        public bool CashRequest { get; set; }
    }
}
