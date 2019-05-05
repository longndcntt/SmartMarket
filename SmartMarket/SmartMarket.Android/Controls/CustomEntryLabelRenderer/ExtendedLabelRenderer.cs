using Android.Content;
using Android.Text;
using SmartMarket.Controls.CustomLabelEntry;
using SmartMarket.Droid.Controls.CustomEntryLabelRenderer;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly:ExportRenderer(typeof(ExtendedLabel), typeof(ExtendedLabelRenderer))]
namespace SmartMarket.Droid.Controls.CustomEntryLabelRenderer
{
    public class ExtendedLabelRenderer : LabelRenderer
    {
        public ExtendedLabelRenderer(Context context) : base(context: context)
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

                if (element.MaxLinesLabel == -1 || element.MaxLinesLabel == int.MaxValue)
                    return;
                this.Control.SetMaxLines(element.MaxLinesLabel);
                this.Control.Ellipsize = TextUtils.TruncateAt.End;
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == ExtendedLabel.MaxLinesLabelProperty.PropertyName)
            {
                if (((ExtendedLabel)this.Element).MaxLinesLabel == -1 || ((ExtendedLabel)this.Element).MaxLinesLabel == int.MaxValue)
                    return;
                this.Control.SetMaxLines(((ExtendedLabel)this.Element).MaxLinesLabel);
                this.Control.Ellipsize = TextUtils.TruncateAt.End;
            }
        }
    }
}