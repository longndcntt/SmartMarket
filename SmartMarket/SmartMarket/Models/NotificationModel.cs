using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartMarket.Models
{
    [Table(("NotificationModel"))]
    public class NotificationModel
    {
        //[PrimaryKey,AutoIncrement]
        //public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; } 
        public string WalletAddress { get; set; }
        public string Message { get; set; }
        public string Image { get; set; } = "ic_buyItem";
        public string DatTimeStamp { get; set; }
        public string DateTimeReceived
        { get; set; }
    }
}
