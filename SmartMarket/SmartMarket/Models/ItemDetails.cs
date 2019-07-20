using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace SmartMarket.Models
{
    [Table("ItemDetails")]
    public class ItemDetails
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; } 

        [ForeignKey(typeof(ItemModel))]
        public int ProductId { get; set; }

        public int Count { get; set; }
        public string Manufacturer { get; set; }
        public string Detail { get; set; }
        public ObservableCollection<string> Images { get; set; }
    }
}
