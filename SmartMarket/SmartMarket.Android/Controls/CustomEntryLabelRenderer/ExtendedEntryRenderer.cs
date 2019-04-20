using Android.Content;
using Android.Graphics.Drawables;
using SmartMarket.Controls.CustomLabelEntry;
using SmartMarket.Droid.Controls.CustomEntryLabelRenderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ExtendedEntry), typeof(ExtendedEntryRenderer))]
namespace SmartMarket.Droid.Controls.CustomEntryLabelRenderer
{
    public class ExtendedEntryRenderer : EntryRenderer
    {
        public ExtendedEntryRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                var element = (ExtendedEntry)e.NewElement;

                if (Control != null)
                {
                    if (element.Borderless)
                    {
                        this.Control.Background = new ColorDrawable(Color.Transparent.ToAndroid());
                        Control.Background = null;
                    }
                    this.Control.SetPadding((int)element.Padding.Left, (int)element.Padding.Top,
                  (int)element.Padding.Right, (int)element.Padding.Bottom);
                }
                
              
            }
        }
    }
}