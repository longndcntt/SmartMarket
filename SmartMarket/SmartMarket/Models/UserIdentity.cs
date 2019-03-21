using System;
using System.Security.Cryptography;
using System.Text;

namespace SmartMarket.Models
{
    public class UserIdentity
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public string CryptPassword(string password)
        {
            var key = "FairFlexxUser@" + DateTime.UtcNow.ToString("yyyy-MM-dd");

            var inputArray = Encoding.UTF8.GetBytes(password);
            var tripleDES = new TripleDESCryptoServiceProvider
            {
                Key = Encoding.UTF8.GetBytes(key),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            var cTransform = tripleDES.CreateEncryptor();
            var resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            tripleDES.Clear();

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
    }
}
