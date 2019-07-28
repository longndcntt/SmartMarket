using Newtonsoft.Json;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartMarket.Models
{
    [Table("ItemModel")]
    public class ItemModel
    {
        [PrimaryKey]
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string ItemName { get; set; }
        public double Price
        {
            get;
            set;
        }
        public string Image { get; set; }
        public string Seller { get; set; }
        public bool isLast { get; set; }
        [Ignore]
        public string StringPrice
        {
            get => string.Format("{0:0,0}", Price);
            set => Price.ToString();
        }
        //  public string CategoryID { get; set; }

    }
}
