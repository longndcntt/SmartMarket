using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SmartMarket.Models
{
    public class ExchangeModel
    {
        public int productId { get; set; }
        public UserIdentity buyer { get; set; }
        public UserIdentity seller { get; set; }
        public double remain { get; set; }
        public bool isDone { get; set; }
        public ItemModel productInfo { get; set; }
        public Color StatusExchange { get; set; }

    }
}
