using Rg.Plugins.Popup.Animations;
using Rg.Plugins.Popup.Enums;
using Xamarin.Forms;

namespace SmartMarket.Views.Base
{
    public class PopupAnimation : MoveAnimation
    {
        #region Constructors

        public PopupAnimation()
        {
            PositionIn = MoveAnimationOptions.Left;
            PositionOut = MoveAnimationOptions.Left;
            DurationIn = 200;
            DurationOut = 200;
            EasingIn = Easing.CubicOut;
            EasingOut = Easing.CubicOut;
            HasBackgroundAnimation = true;
        }

        #endregion
    }

    public class PopupNoAnimation : ScaleAnimation
    {
        #region Constructors

        public PopupNoAnimation()
        {
            PositionIn = MoveAnimationOptions.Center;
            PositionOut = MoveAnimationOptions.Center;
            ScaleIn = 1.2;
            ScaleOut = 0.8;
            DurationIn = 400;
            DurationOut = 300;
            EasingIn = Easing.SinOut;
            EasingOut = Easing.SinIn;
            HasBackgroundAnimation = false;
        }

        #endregion
    }

    public class PopupFadeAnimation : FadeAnimation
    {
        #region Constructors

        public PopupFadeAnimation()
        {
            DurationIn = 100;
            DurationOut = 100;
            EasingIn = Easing.CubicIn;
            EasingOut = Easing.CubicOut;
            HasBackgroundAnimation = true;
        }

        #endregion
    }
}
