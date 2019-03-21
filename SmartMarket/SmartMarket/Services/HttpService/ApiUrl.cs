
namespace SmartMarket.Services.HttpService
{
    public class ApiUrl
    {
        //public static string HttpUrl { get; set; } = CrossSecureStorage.Current.GetValue("HttpUrl");

        //public static string Link(string link)
        //{
        //    return $"{App.Settings.HttpUrl}api/{link}";
        //}

        private  readonly string _homeUrl;

        public ApiUrl(string homeUrl)
        {
            _homeUrl = homeUrl;
        }

        private string LinkApi(string api)
        {
            return $"{_homeUrl}{api}";
        }


        #region API URL

        public  string UserLogin()
        {
            return LinkApi("user/login/");
        }

        public  string EventGetByClientId()
        {
            return LinkApi("event/GetByClientIdV1/");
        }

        public  string TestServer()
        {
            //return Link("file/Test");
            return LinkApi("filefastupload/test");
        }

        public  string UploadFile()
        {
            return LinkApi("file/Upload");
        }

        /*"http://test-fastupload.fairflexx.net/api/filefastupload/testdb"*/
        public  string FastUploadFile()
        {
            return LinkApi("filefastupload/upload");
        }

        public  string Test()
        {
            return LinkApi("file/test");
        }

        #endregion
    }
}
