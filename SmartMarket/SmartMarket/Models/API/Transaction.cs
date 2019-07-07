using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SmartMarket.Models.API
{
    public class Transaction
    {
        public string To { get; set; }
        public string Data { get; set; }
        public BigInteger Nonce { get; set; }
        public BigInteger GasLimit { get; set; }
        public BigInteger GasPrice { get; set; }
        public BigInteger Value { get; set; }

        public Transaction(string to, string data, long nonce, long gas, long gasPrice, long value)
        {
            this.To = to;
            this.Data = data;
            this.Nonce = new BigInteger(nonce);
            this.GasLimit = new BigInteger(gas);
            this.GasPrice = new BigInteger(gasPrice);
            this.Value = new BigInteger(value);
        }

    }
}
