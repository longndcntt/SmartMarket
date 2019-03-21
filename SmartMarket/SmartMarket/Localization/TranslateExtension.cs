using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartMarket.Localization
{
    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        private const string ResourceId = "SmartMarket.Localization.Translate";

        public string Text { get; set; }

        public bool Upper { get; set; }

        public bool Colon { get; set; }

        public bool LineBreak { get; set; }

        #region Get resource

        public static string Get(string text, bool upper = false, bool colon = false)
        {
            var resourceManager = new ResourceManager(ResourceId,typeof(TranslateExtension).GetTypeInfo().Assembly);
            var translation = resourceManager.GetString(text, CultureInfo.CurrentCulture) ?? text;

            if (upper)
            {
                translation = translation.ToUpper();
                text = text.ToUpper();
            }
            if (colon)
            {
                translation += ":";
                text += ":";
            }

            return translation ?? text;
        }
        #endregion

        #region IMarkupExtension implementation

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
                return string.Empty;

            var resmgr = new ResourceManager(ResourceId, typeof (TranslateExtension).GetTypeInfo().Assembly);
            var translation = resmgr.GetString(Text, new CultureInfo(CultureInfo.CurrentCulture.Name));
            if (translation == null)
            {
#if DEBUG
                throw new ArgumentException(
                    $"Key '{Text}' was not found in resources '{ResourceId}' for culture '{CultureInfo.CurrentCulture.Name}'.",
                    nameof(Text));
#else
				translation = Text;
#endif
            }
            if (Upper)
                translation = translation.ToUpper();
            if (Colon)
                translation += ":";
            translation = string.Format(translation, LineBreak ? Environment.NewLine : " ");

#if DEBUG
            System.Diagnostics.Debug.WriteLine($"Get string: {translation}, culture: { CultureInfo.CurrentCulture.Name}");
#endif

            return translation;
        }

        #endregion
    }
}