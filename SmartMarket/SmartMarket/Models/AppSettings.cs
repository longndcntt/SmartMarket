using SQLite;

namespace SmartMarket.Models
{
    public class AppSettings
    {
        [PrimaryKey]
        public int Id { get; set; } = 0;

        public bool IsLogin { get; set; }

        //public string HttpUrl { get; set; } = "http://test-fastupload.fairflexx.net/";
        public string HttpUrl { get; set; } = "http://192.168.11.110/";

        public int ClientId { get; set; }
    }
}
