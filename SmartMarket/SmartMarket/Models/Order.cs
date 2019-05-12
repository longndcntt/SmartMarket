using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartMarket.Models
{
    [Table("Order")]
    public class Order
    {
        [PrimaryKey]
        public int Id { get; set; } = 0;

        [ForeignKey(typeof(UserModel))]
        public int UserId { get; set; }

        public double Total { get; set; } = 0;
        public string CreatedDateTime { get; set; }
        public string ShippingAddress { get; set; }
    }
}
