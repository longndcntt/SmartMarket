using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartMarket.Models
{
    [Table("ItemTabel")]
    public class ItemModel
    {
        [PrimaryKey]
        public int Id { get; set; }

        [ForeignKey(typeof(Category))]
        public int CategoryId { get; set; }

        public string ItemName { get; set; }
        public double Price { get; set; }
        public string Image { get; set; }
        public byte[] ImageByte { get; set; }
    }
}
