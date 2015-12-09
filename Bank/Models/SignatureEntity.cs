using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bank.Models
{
    public class SignatureEntity
    {
        public string PublicKey { get; set; }
        public string SecretKey { get; set; }
    }
}