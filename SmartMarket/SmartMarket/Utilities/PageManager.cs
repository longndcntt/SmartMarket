using System;
using System.Diagnostics;
using System.Linq;
using SmartMarket.ViewModels.Base;
using Xamarin.Forms;

namespace SmartMarket.Utilities
{
    public class PageManager
    {
        // Home Flow
        public static readonly string NavigationPage = "NavigationPage";
        public static readonly string HomePage = "HomePage";
        public static readonly string LoginPage = "LoginPage";
        public static readonly string TabbedMainPage = "TabbedMainPage";
        public static readonly string MainPage = "MainPage";

        //public static readonly string MerchantHomePage = "MerchantHomePage";
        //public static readonly string ClientHomePage = "ClientHomePage";
        //public static readonly string LeftMenuPage = "LeftMenuPage";

        //// Login Flow
        //public static readonly string LoginPage = "LoginPage";
        //public static readonly string SignUpPage = "SignUpPage";
        //public static readonly string SignUpInformationPage = "SignUpInformationPage";
        //public static readonly string SignUpUploadAvatarPage = "SignUpUploadAvatarPage";

        //// News Feed Flow
        //public static readonly string NewsFeedPage = "NewsFeedPage";
        //public static readonly string NewsFeedCommentsPage = "NewsFeedCommentsPage";
        //public static readonly string NewsFeedCreateNewsPage = "NewsFeedCreateNewsPage";

        //// Notificaiton Flow
        //public static readonly string NotificationPage = "NotificationPage";

        //// Contacts Flow
        //public static readonly string MyClientsPage = "MyClientsPage";
        //public static readonly string MyMerchantsPage = "MyMerchantsPage";
        //public static readonly string AddNewMerchantPage = "AddNewMerchantPage";

        //// Profile Flow
        //public static readonly string MerchantProfilePage = "MerchantProfilePage";
        //public static readonly string ClientProfilePage = "ClientProfilePage";

        //// Voucher Flow
        //public static readonly string ClientVoucherPage = "ClientVoucherPage";
        //public static readonly string ClientVoucherDetailPage = "ClientVoucherDetailPage"; 
        //public static readonly string MerchantVoucherPage = "MerchantVoucherPage";
        //public static readonly string MerchantVoucherDetailPage = "MerchantVoucherDetailPage";
        //public static readonly string MerchantAddStockPage = "MerchantAddStockPage";
        //public static readonly string MerchantDeleteStockPage = "MerchantDeleteStockPage";
        //public static readonly string MerchantShareVoucherPage = "MerchantShareVoucherPage";
        //public static readonly string MerchantAddNewVoucherPage = "MerchantAddNewVoucherPage";
        //public static readonly string MerchantScanVoucherPage = "MerchantScanVoucherPage";

        //// Voucher History Flow
        //public static readonly string VoucherHistoryHomePage = "VoucherHistoryHomePage"; 
        //public static readonly string VoucherHistoryGivenPage = "VoucherHistoryGivenPage";
        //public static readonly string VoucherHistoryRedeemedPage = "VoucherHistoryRedeemedPage";
        //public static readonly string VoucherHistorySharedPage = "VoucherHistorySharedPage";

        //// Common Pages
        //public static readonly string ListSelectItemPage = "ListSelectItemPage";
        //public static readonly string ScanPage = "ScanPage";


        public static string MultiplePage(string[] pages)
        {
            string path = "";
            if (pages == null) return "";
            if (pages.Length < 1) return "";
            for (int i = 0; i < pages.Length; i++)
            {
                path += i == 0 ? pages[i] : "/" + pages[i];
            }
            return path;
        }

        public static Page GetCurrentPage()
        {
            var mainPage = Application.Current.MainPage;
            var navStack = mainPage.Navigation.NavigationStack;

            //var type = mainPage.GetType();

            //if (mainPage.GetType() == typeof(ClientHomePage) || mainPage.GetType() == typeof(MerchantHomePage))
            //{
            //    mainPage = ((TabbedPage)mainPage).CurrentPage;
            //}

            //if (navStack == null)
            //    return mainPage;
            //if (navStack.Count < 1)
            //    return mainPage;

            return navStack.Last();
        }

        public static Page GetCurrentPage(bool withModal)
        {

            if (!withModal) return GetCurrentPage();
            try
            {
                var navPage = GetCurrentPage();
                var modalPage = navPage.Navigation.ModalStack.LastOrDefault();
                var foundedPage = modalPage ?? navPage;
                return foundedPage;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        public static ViewModelBase GetCurrentPageBaseViewModel()
        {
            return (ViewModelBase)GetCurrentPage(true).BindingContext;
        }
    }
}
