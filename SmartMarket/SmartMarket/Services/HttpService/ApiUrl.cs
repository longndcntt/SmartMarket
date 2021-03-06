﻿
namespace SmartMarket.Services.HttpService
{
    public static class ApiUrl
    {
        //public static string HttpUrl { get; set; } = CrossSecureStorage.Current.GetValue("HttpUrl");

        //public static string Link(string link)
        //{
        //    return $"{App.Settings.HttpUrl}api/{link}";
        //}

        public static string Link(string link)
        {
            return $"{App.Settings.HttpUrl1}{link}";
        }

        #region API URL

        public static string GetWallet()
        {
            return Link("tokens/balance?Address=");
        }

        public static string BuyToken()
        {

            return Link("tokens/buy");
        }

        public static string BuyItem()
        {
            return Link("tokens/transfer");
        }

        public static string DeleteItem()
        {
            return Link($"product/delete");
        }

        public static string UploadItem()
        {
            return Link("product/add");
        }

        public static string GetAllItem()
        {
            return Link("product/get/all");
        }

        public static string GetSearchItem(string content)
        {
            return Link($"product/search/{content}");
        }

        public static string ExchangeProduct()
        {
            return Link("exchange");
        }

        public static string GetPurchasedItems(string walletAdress)
        {
            return Link($"exchange/buy/{walletAdress}");
        }

        public static string GetItemOfShop(string walletAdress)
        {
            return Link($"product/store/{walletAdress}");
        }

        public static string GetSelledItems(string walletAdress)
        {
            return Link($"exchange/sell/{walletAdress}");
        }

        public static string GetRandomItems()
        {
            return Link($"product/get/random");
        }

        public static string GetNewItems()
        {
            return Link($"product/get/new");
        }

        public static string GetItemByCategory(string id)
        {
            return Link($"product/get/category/" + id);
        }

        public static string GetItemSelected(string itemId)
        {
            return Link($"product/display/{itemId}");
        }
        
        public static string GetItemDetails()
        {
            return Link("product/detail/");
        }

        public static string Schedule()
        {
            return Link("tokens/schudele");
        }

        public static string UploadToBlockChain()
        {
            return Link("publish");
        }

        public static string UserRegister()
        {
            return Link("signup");
        }

        public static string EditUser()
        {
            return Link("account");
        }

        public static string UserLogin()
        {
            return Link("login");
        }

        public static string SubmitReview()
        {
            return Link("product/preview");
        }

        public static string GetReview(string ItemModelId)
        {
            return Link($"product/{ItemModelId}/preview");
        }

       
        #endregion
    }
}
