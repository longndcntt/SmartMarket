using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace SmartMarket.Converters
{
    public class NullToBoolConverter : IValueConverter
    {
        /// <summary>
        /// Reverse the result: If any result --> return false. Else return true
        /// </summary>
        public bool IsReverse { get; set; } = true;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return !IsReverse;
            return ((ObservableCollection<string>)value).Any() ? IsReverse : !IsReverse;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return false;
        }
    }
}
