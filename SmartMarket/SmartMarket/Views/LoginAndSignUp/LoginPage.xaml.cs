using Xamarin.Forms;

namespace SmartMarket.Views.LoginAndSignUp
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void BorderButton_Clicked(object sender, System.EventArgs e)
        {
            var masterPage = this.Parent as TabbedPage;
            masterPage.CurrentPage = masterPage.Children[1];
        }
    }
}
