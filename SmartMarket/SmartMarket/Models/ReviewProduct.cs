using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartMarket.Models
{
    [Table("ReviewProduct")]
    public class ReviewProduct
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [ForeignKey(typeof(ItemModel))]
        public int ProductId { get; set; }
        [ForeignKey(typeof(UserModel))]
        public int UserId { get; set; }
        
        public int Rate { get; set; }
        public string Content { get; set; }
        public string ReviewedDateTime { get; set; }
    }
}
