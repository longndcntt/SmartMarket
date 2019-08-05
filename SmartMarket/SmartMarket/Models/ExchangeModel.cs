using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SmartMarket.Models
{
    public class ExchangeModel
    {
        public int productId { get; set; }
        public UserModel buyer { get; set; }
        public UserModel seller { get; set; }
        public double remain { get; set; }
        public bool isDone { get; set; }
        public ItemModel productInfo { get; set; }
        public Color StatusExchange { get; set; }
        public string StatusExchangeString { get; set; }
        public double unixtime { get; set; }
        public string ExchangeDateTime
        {
            get => UnixTimeStampToDateTime(unixtime).ToString("dd/MM/yyyy");
            set => unixtime.ToString();

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
