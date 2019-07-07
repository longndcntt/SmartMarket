using Nethereum.Signer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMarket.Services.HttpService
{
    public class Signer
    {
        private TransactionSigner signer = new TransactionSigner();

        public string Sign(string privkey, SmartMarket.Models.API.Transaction tx)
        {
            var signed = signer.SignTransaction(
                privateKey: privkey,
                to: tx.To,
                data: tx.Data,
                nonce: tx.Nonce,
                gasLimit: tx.GasLimit,
                gasPrice: tx.GasPrice,
                amount: tx.Value
                );
            return "0x" + signed;
        }
    }
}
