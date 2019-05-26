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
        public string DateTImeKey { get; set; }
        public string Keystore { get; set; }
        public string Address { get; set; }
        public string DayofBirth { get; set; }
        public bool Gender { get; set; } //True is Male, False is Female
        public string NumberID { get; set; }
        public string PhoneNumber { get; set; }
        public string PortraitImage { get; set; }
        public string IdentityImage { get; set; }
        public double Coin { get; set; }
    }
}
