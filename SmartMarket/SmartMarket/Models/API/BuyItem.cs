using System;
using System.Collections.Generic;
using System.Text;

namespace SmartMarket.Models.API
{
    public class BuyItem
    {
        public string AddressFrom { get; set; }
        public string AddressTo { get; set; }
        public int[] Time { get; set; }
        public double[] Amount { get; set; }
    }
}
