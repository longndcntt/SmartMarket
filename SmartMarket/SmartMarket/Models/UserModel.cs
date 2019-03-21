using Newtonsoft.Json;
using SQLite;

namespace SmartMarket.Models
{
    [Table("UserTable")]
    public class UserModel
    {
        [PrimaryKey]
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public bool IsDeactivated { get; set; }
        public bool AccountLocked { get; set; }
        public int PreferedUILanguageId { get; set; }

        [JsonProperty("Access_Token")]
        [Ignore]
        public string AccessToken { get; set; }
    }
}
