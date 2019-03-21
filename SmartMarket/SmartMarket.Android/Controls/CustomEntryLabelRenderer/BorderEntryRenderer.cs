using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Support.V4.Content;
using Android.Text;
using Android.Views;
using SmartMarket.Droid.Controls.CustomEntryLabelRenderer;
using SmartMarket.Enums;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using SmartMarket.Controls.CustomLabelEntry;

[assembly:ExportRenderer(typeof(BorderEntry), typeof(BorderEntryRenderer))]
namespace SmartMarket.Droid.Controls.CustomEntryLabelRenderer
{
    public class BorderEntryRenderer : EntryRenderer
    {
        private double _ratio;

        public BorderEntryRenderer() : base()
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            // Ratio for image standard size
            _ratio = Resources.DisplayMetrics.HeightPixels / 960d;

            if (e.NewElement != null)
            {
                var element = (BorderEntry) e.NewElement;
                this.Control.SetBackgroundResource(GetBackground(element.Theme));
                
                #region image

                var editText = this.Control;

                if (!string.IsNullOrEmpty(element.Icon))
                {
                    editText.SetCompoundDrawablesWithIntrinsicBounds(
                        GetDrawable(element.Icon, element.IconHeight, imageWidth: element.IconWidth), null,
                        null, null);

                    //test vertical
                    editText.Gravity = GravityFlags.CenterVertical;
                }
                editText.CompoundDrawablePadding = (int)(element.IconSpacing * _ratio);

                #endregion

                #region maxLenght

                this.Control.SetFilters(new IInputFilter[] { new InputFilterLengthFilter(element.MaxLength) });

                #endregion

                #region padding
                editText.SetPadding(
                    (int)(element.Padding.Left * _ratio),
                    (int)(element.Padding.Top * _ratio),
                    (int)(element.Padding.Right * _ratio),
                    (int)(element.Padding.Bottom * _ratio));
                #endregion

                #region InputType

                if (element.Keyboard == Keyboard.Numeric)
                {
                    editText.InputType = InputTypes.ClassNumber | InputTypes.NumberFlagDecimal;
                }

                #endregion
            }
        }

        private BitmapDrawable GetDrawable(string imageEntryImage, int imageHeight, int imageWidth)
        {
            var resId = (int)typeof(Resource.Drawable)
                .GetField(imageEntryImage.Replace(".jpg", "").Replace(".png", "")).GetValue(null);
            var drawable = ContextCompat.GetDrawable(this.Context, resId);
            var bitmap = ((BitmapDrawable)drawable).Bitmap;

            return new BitmapDrawable(Resources,
                Bitmap.CreateScaledBitmap(bitmap, (int)(imageWidth * 2 * _ratio), (int)(imageHeight * 2 * _ratio), true));
        }

        private int GetBackground(BorderEntryTheme theme)
        {
            switch (theme)
            {
                case BorderEntryTheme.WhiteTransparent:
                    return Resource.Drawable.bg_border_entry_white_transparent;

                case BorderEntryTheme.GreenTransparent:
                    return Resource.Drawable.bg_border_entry_green_transparent;

                case BorderEntryTheme.White:
                    return Resource.Drawable.bg_border_entry_white;

                case BorderEntryTheme.GreenWhite:
                    return Resource.Drawable.bg_border_entry_green_white;
            }

            return 0;
        }
    }
}