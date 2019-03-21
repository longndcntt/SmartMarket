using System;
using System.Diagnostics;
using Prism.Navigation;
using SmartMarket.ViewModels.Base;
using Xamarin.Forms;

namespace SmartMarket.Views.Base
{
    public class BasePage : ContentPage, INavigationAware
    {
        #region Properties

        private bool _isAppeared;
        protected ViewModelBase ViewModel;

        #endregion

        #region Navigate

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {
#if DEBUG
            Debug.WriteLine("Navigated from");
#endif
        }

        public virtual void OnNavigatingTo(INavigationParameters parameters)
        {
#if DEBUG
            Debug.WriteLine("Navigating to");
#endif
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
#if DEBUG
            Debug.WriteLine("Navigated to");
#endif 

            if (parameters != null)
            {
                var navMode = parameters.GetNavigationMode();
                switch (navMode)
                {
                    case NavigationMode.New: OnNavigatedNewTo(parameters); break;
                    case NavigationMode.Back: OnNavigatedBackTo(parameters); break;
                }
            }

        }

        public virtual void OnNavigatedNewTo(INavigationParameters parameters)
        {
#if DEBUG
            Debug.WriteLine("Navigate new to");
#endif
        }

        public virtual void OnNavigatedBackTo(INavigationParameters parameters)
        {
#if DEBUG
            Debug.WriteLine("Navigate back to");
#endif
        }

        #endregion

        #region OnBindingContextChanged

        protected override void OnBindingContextChanged()
        {
            if (BindingContext != null)
                ViewModel = (ViewModelBase)BindingContext;
        }

        protected override void OnAppearing()
        {
            try
            {
                if (ViewModel == null && BindingContext != null)
                    ViewModel = (ViewModelBase)BindingContext;

                if (!_isAppeared)
                    ViewModel?.OnFirstTimeAppear();

                _isAppeared = true;
                ViewModel?.OnAppear();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

        }

        protected override void OnDisappearing()
        {
            ViewModel?.OnDisappear();
        }

        #endregion

        #region BackButtonPress

        protected override bool OnBackButtonPressed()
        {
            var bindingContext = BindingContext as ViewModelBase;
            var result = bindingContext?.OnBackButtonPressed() ?? base.OnBackButtonPressed();
            return result;
        }


        public void OnSoftBackButtonPressed()
        {
            var bindingContext = BindingContext as ViewModelBase;
            bindingContext?.OnSoftBackButtonPressed();
        }

        public bool NeedOverrideSoftBackButton { get; set; } = false;

        #endregion
    }
}