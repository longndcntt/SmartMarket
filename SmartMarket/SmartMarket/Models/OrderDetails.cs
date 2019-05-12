using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartMarket.Models
{
    [Table("OrderDetails")]
    public class OrderDetails
    {
        [PrimaryKey]
        public string Id { get; set; }

        [ForeignKey(typeof(ItemModel))]
        public int ProductId { get; set; }

        [ForeignKey(typeof(Order))]
        public int OrderId { get; set; }

        public int Amount { get; set; }
        public double Price { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Manufacturer { get; set; }
        public byte[] ImageByte { get; set; }

    }
}
