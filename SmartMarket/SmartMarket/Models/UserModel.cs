using Newtonsoft.Json;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SmartMarket.Models
{
    [Table("UserModel")]
    public class UserModel
    {
        [PrimaryKey]
        public int Id { get; set; } = 0;
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public string Keystore { get; set; }
        public string Address { get; set; }
        public string DayOfBirth { get; set; }
        public bool Gender { get; set; }
        public string NumberID { get; set; }
        public string PhoneNumber { get; set; }
        public string Image { get; set; }
        public string IdentityImage { get; set; }
        public double Coin { get; set; }
        public string WalletAddress { get; set; }
        public string PrivateKey { get; set; }

    }
}
