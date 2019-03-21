using Prism.Commands;
using Prism.Navigation;
using SmartMarket.ViewModels.Base;

namespace SmartMarket.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public MainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Main Page";
        }
    }
}
