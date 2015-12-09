using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Common
{
    public class MoneyOrder
    {
        public readonly byte[] UniqueId;
        public readonly byte[] Signature;

        public MoneyOrder(byte[] id, byte[] signature)
        {
            this.UniqueId = id;
            this.Signature = signature;
        }

    }
}