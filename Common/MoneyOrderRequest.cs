using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Common
{
    public class MoneyOrderRequest
    {
        public  byte[] BlindSignature;
        public byte[] BankSignature;
        public byte[] UniqueID;
        public string BlindSignatureBase64;
        public string BankSignatureBase64;
        public string UniqueIDBase64;
        public decimal? Amount;
        public SecretIdentity[] identity { get; set; }
        public bool IsRequested { get; set; }
        public bool IsSigned { get; set; }

        public MoneyOrderRequest()
        {

        }

        public MoneyOrderRequest(byte[] id,byte[] blindSig, decimal? amount)
        {
            this.UniqueID = id;
            this.BlindSignature = blindSig;
            this.Amount = amount;
        }

        public MoneyOrderRequest(byte[] id, byte[] blindSig, byte[] bankSignature,decimal? amount)
        {
            this.UniqueID = id;
            this.BankSignature = blindSig;
            this.Amount = amount;
        }
    }
}