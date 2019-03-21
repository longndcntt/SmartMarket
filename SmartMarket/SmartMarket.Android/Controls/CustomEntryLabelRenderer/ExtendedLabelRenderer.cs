using SmartMarket.Controls.CustomLabelEntry;
using SmartMarket.Droid.Controls.CustomEntryLabelRenderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly:ExportRenderer(typeof(ExtendedLabel), typeof(ExtendedLabelRenderer))]
namespace SmartMarket.Droid.Controls.CustomEntryLabelRenderer
{
    public class ExtendedLabelRenderer : LabelRenderer
    {
        public ExtendedLabelRenderer() : base()
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                var element = (ExtendedLabel) e.NewElement;

                if (element.IsUnderline)
                    this.Control.PaintFlags = Android.Graphics.PaintFlags.UnderlineText;

                this.Control.SetPadding((int)element.Padding.Left, (int)element.Padding.Top,
                    (int)element.Padding.Right, (int)element.Padding.Bottom);
            }
        }
    }
}