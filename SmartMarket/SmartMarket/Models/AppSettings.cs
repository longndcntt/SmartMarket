using SQLite;
using System;

namespace SmartMarket.Models
{
    public class AppSettings
    {
        [PrimaryKey]
        public int Id { get; set; } = 0;

        public bool IsLogin { get; set; }
        public bool IsSaler { get; set; } = false;

        //public string HttpUrl { get; set; } = "http://test-fastupload.fairflexx.net/";
        public string HttpUrl1 { get; set; } = "http://35.194.122.245:3000/";

        public int ClientId { get; set; }

        public string Time1 { get; set; }
        public string Time2 { get; set; }
    }
}
