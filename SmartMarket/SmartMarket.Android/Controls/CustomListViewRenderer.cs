using Android.Content;
using SmartMarket.Droid.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ListView), typeof(CustomListViewRenderer))]

namespace SmartMarket.Droid.Controls
{
    public class CustomListViewRenderer : ListViewRenderer
    {
        public CustomListViewRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement == null)
                return;

            if (Control != null)
            {
                Control.NestedScrollingEnabled = true;
                Control.HorizontalScrollBarEnabled = false;
                Control.VerticalScrollBarEnabled = false;

            }
        }

    }
}