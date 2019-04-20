using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartMarket.Models
{
    [Table("Category")]
    public class Category
    {
        [PrimaryKey, AutoIncrement ]
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public byte[] ImageByte { get; set; }
        public string Image { get; set; }
    }
}
