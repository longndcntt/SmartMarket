using System;
using System.Collections.Generic;
using System.Text;

namespace SmartMarket.Models.API
{
    public class ItemAPIUpload
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public string ItemName { get; set; }
        public double Price { get; set; }
        public string Image { get; set; }
        public int Available { get; set; }
        public string Manufacturer { get; set; }
        public string Detail { get; set; }
        public string WalletAddress { get; set; }
        public List<string> Images { get; set; }

    }
}
