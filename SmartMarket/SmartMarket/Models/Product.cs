using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartMarket.Models
{
    [Table("Product")]
    public class Product
    {
        [PrimaryKey,AutoIncrement]
        public int IdProduct { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }

        [ManyToMany(typeof(UserModel))]
        public List<UserModel> UserList { get; set; }
    }
}
