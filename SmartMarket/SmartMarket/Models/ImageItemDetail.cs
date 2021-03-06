﻿using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartMarket.Models
{
    [Table("ImageItemDetail")]
    public class ImageItemDetail
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }

        [ForeignKey(typeof(ItemModel))]
        public int ProductID { get; set; }

        public string Image { get; set   ; }
        public byte[] ImageByte { get; set; }
    }
}
