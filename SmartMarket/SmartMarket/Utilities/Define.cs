
using SmartMarket.Services.HttpService;

namespace SmartMarket.Utilities
{
    public class Define
    {
        // SQLite Service
        public const string SqliteDatabaseName = "svoucher.sqlite";

        // Navigation Service
        public const string NavigationHomeUri = "https://svoucher-ise-uit.com/";

        // Http Service
        //private const string HttpHomeUrl = "http://svoucher.azurewebsites.net/";
        private const string HttpHomeUrl = "http://svoucheruit.azurewebsites.net/";
        //private const string HttpHomeUrl = "http://192.168.137.1/"; // Localhost
        public const int TimeoutMiliseconds = 60000;
        private static ApiUrl _apiUrl;
        public static ApiUrl ApiUrl => _apiUrl ?? (_apiUrl = new ApiUrl(HttpHomeUrl));

        // Common
        public static string DateTimeFormat = "dd/MM/yyyy HH:mm:ss";
        public static string DateFormat = "dd/MM/yyyy";

    }
}
