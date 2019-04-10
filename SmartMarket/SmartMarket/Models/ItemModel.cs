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
        public Guid Id { get; set; } = new Guid();
        public string ItemName { get; set; }
        public double Prince { get; set; }
        public string Image { get; set; }
        [ManyToMany(typeof(UserModel))]
        public List<UserModel> UserList { get; set; }
    }
}
