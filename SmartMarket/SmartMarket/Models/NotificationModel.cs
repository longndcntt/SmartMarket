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
        public string Time { get; set; }
        public double DateTimeSend
        { get; set; }

        public string TimeBinding
        {
            get => UnixTimeStampToDateTime(DateTimeSend).ToString("dd/MM/yyyy hh:mm:ss");
            set => DateTimeSend.ToString();

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
