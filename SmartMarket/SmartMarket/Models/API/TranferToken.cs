using System;
using System.Collections.Generic;
using System.Text;

namespace SmartMarket.Models.API
{
    public class TranferToken
    {
        public string from { get; set; }
        public string to { get; set; }
        public int nonce { get; set; }
        public string data { get; set; }
        public int gas { get; set; }
        public int gasPrice { get; set; }
        public int value { get; set; }
    }
}
