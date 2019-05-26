using System;
using System.Collections.Generic;
using System.Text;

namespace SmartMarket.Models.API
{
    public class SignedTransaction
    {
        public string Signed { get; set; }

        public SignedTransaction()
        {

        }
        public SignedTransaction(string signedString)
        {
            this.Signed = signedString;
        }
    }
}
