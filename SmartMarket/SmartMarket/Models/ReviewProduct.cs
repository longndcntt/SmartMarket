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
        [PrimaryKey]
        public int Id { get; set; }
        [ForeignKey(typeof(ItemModel))]
        public int ProductId { get; set; }
        public UserModel User { get; set; }
        public string WalletAddress { get; set; }
        public int Rate { get; set; }
        public string Content { get; set; }
        public Double Date { get; set; }

        public string ReviewedDateTime
        {
            get => UnixTimeStampToDateTime(Date).ToString("dd/MM/yyyy");
            set => Date.ToString(); 

        }
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }


}
