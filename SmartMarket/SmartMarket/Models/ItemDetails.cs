using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartMarket.Models
{
    [Table("ItemDetails")]
    public class ItemDetails
    {
        [PrimaryKey]
        public Guid Id { get; set; } = new Guid();

        [ForeignKey(typeof(ItemModel))]
        public int ProductId { get; set; }

        public int Count { get; set; }
        public string Manufacturer { get; set; }
        public string Detail { get; set; }
    }
}
