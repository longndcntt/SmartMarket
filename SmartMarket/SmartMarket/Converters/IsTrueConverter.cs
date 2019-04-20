using System;
using System.Globalization;
using SmartMarket.Utilities;
using Xamarin.Forms;

namespace SmartMarket.Converters
{
    /// <summary>
    /// Check an object is null or not. If null --> return true. Else return false
    /// </summary>
    public class IsTrueConverter : IValueConverter
    {
        /// <summary>
        /// Reverse the result: If null --> return false. Else return true
        /// </summary>
        public bool IsReverse { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString().StringToBool() == true ? IsReverse : !IsReverse;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return false;
        }
    }
}
