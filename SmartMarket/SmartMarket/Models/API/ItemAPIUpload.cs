using System;
using System.Collections.Generic;
using System.Text;

namespace SmartMarket.Models.API
{
    public class ItemAPIUpload
    {
        public int CategoryId { get; set; }
        public string ItemName { get; set; }
        public double Price { get; set; }
        public string Image { get; set; }
        public int Count { get; set; }
        public string Manufacturer { get; set; }
        public string Detail { get; set; }
        public string WalletAddress { get; set; }

    }
}
