using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace SmartMarket.Converters
{
    public class BoolToImageConverter : IValueConverter
    {
        public ImageSource TrueValue { get; set; }

        public ImageSource FalseValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                bool? syncState = value as bool?;

                if (syncState != null)
                {
                    if (syncState.Value) return TrueValue;
                    else return FalseValue;
                }
            }
            return FalseValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (string)value;
        }
    }
}
