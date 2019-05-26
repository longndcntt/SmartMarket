using Nethereum.Web3;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace SmartMarket.Services.HttpService
{
    public static class Wallet
    {
        public static string CreateWallet(string password)
        {
            var web3 = new Web3();
            var ecKey = Nethereum.Signer.EthECKey.GenerateKey();
            var privateKey = ecKey.GetPrivateKeyAsBytes();
            var account = new Nethereum.Web3.Accounts.Account(privateKey);
            var keystoreService = new Nethereum.KeyStore.KeyStoreService();
            var keystore = keystoreService.EncryptAndGenerateDefaultKeyStoreAsJson(password, privateKey, account.Address);
            return keystore;
        }

        public static IDictionary<string, string> GetWallet(string keystore, string password)
        {
            var account = Nethereum.Web3.Accounts.Account.LoadFromKeyStore(keystore, password);
            var wallet = new Dictionary<string, string>();
            wallet.Add(account.Address, account.PrivateKey);
            return wallet;
        }

        public static string CryptPassword(string password)
        {
            var key = "SmartMartket@" + DateTime.UtcNow.ToString("yyyy-MM-dd");

            var inputArray = Encoding.UTF8.GetBytes(password);
            var tripleDES = new TripleDESCryptoServiceProvider
            {
                Key = Encoding.UTF8.GetBytes("012345678901234567890123"),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            var cTransform = tripleDES.CreateEncryptor();
            var resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            tripleDES.Clear();

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public static string DecryptPassword(string encryptedPass, string dateTimeKey)
        {
            string key = "ISEUIT@" + dateTimeKey; // DateTime.UtcNow.ToString("yyyy -MM-dd");

            byte[] inputArray = Convert.FromBase64String(encryptedPass);

            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
            tripleDES.Key = UTF8Encoding.UTF8.GetBytes(key);
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            tripleDES.Clear();
            return UTF8Encoding.UTF8.GetString(resultArray);
        }
    }
}
