using System.ComponentModel;
using SmartMarket.Controls.CustomLabelEntry;
using SmartMarket.Droid.Controls.CustomEntryLabelRenderer;
using SmartMarket.Enums;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly:ExportRenderer(typeof(BorderButton), typeof(BorderButtonRenderer))]
namespace SmartMarket.Droid.Controls.CustomEntryLabelRenderer
{
    public class BorderButtonRenderer : ButtonRenderer
    {
        public BorderButtonRenderer() : base()
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                var element = (BorderButton) e.NewElement;
                
                this.Control.SetBackgroundResource(GetBackground(element.Theme));
                this.Control.SetAllCaps(false);
                //this.Control.SetPadding(0,0,0,0);
                this.Control.SetPadding((int)element.Padding.Left, (int)element.Padding.Top,
                   (int)element.Padding.Right, (int)element.Padding.Bottom);
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var button = (BorderButton)sender;

            if (e.PropertyName == "Theme")
            {
                this.Control.SetBackgroundResource(GetBackground(button.Theme));
            }
        }

        private int GetBackground(BorderButtonTheme theme)
        {
            switch (theme)
            {
                case BorderButtonTheme.White:
                    return Resource.Drawable.bg_border_button_white;

                case BorderButtonTheme.Green:
                    return Resource.Drawable.bg_border_button_green;

                case BorderButtonTheme.Red:
                    return Resource.Drawable.bg_border_button_red_white;

                case BorderButtonTheme.GreenWhite:
                    return Resource.Drawable.bg_border_button_green_white;

                case BorderButtonTheme.WhiteGreen:
                    return Resource.Drawable.bg_border_button_white_green;

                case BorderButtonTheme.WhiteRed:
                    return Resource.Drawable.bg_border_button_white_red;

                case BorderButtonTheme.WhiteTransparent:
                    return Resource.Drawable.bg_border_button_white_transparent;

                case BorderButtonTheme.Yellow:
                    return Resource.Drawable.bg_border_button_yellow;

            }

            return 0;
        }
    }
}