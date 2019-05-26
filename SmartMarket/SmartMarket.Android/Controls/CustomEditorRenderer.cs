using SmartMarket.Droid.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Editor), typeof(CustomEditorRenderer))]
namespace SmartMarket.Droid.Controls
{
    public class CustomEditorRenderer : EditorRenderer
    {
        public CustomEditorRenderer() : base()
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                this.Control.Background = null;

            }

            if (e.OldElement != null)
            {

            }

        }
    }
}