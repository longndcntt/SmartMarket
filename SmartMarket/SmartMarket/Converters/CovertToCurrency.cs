using Plugin.Multilingual;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace SmartMarket.Coverter
{
    public class CovertToCurrency : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //value = double.Parse(value.ToString());
            //value = string.Format("{0:C2}", value);
            //string s = value.ToString();

            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");   // try with "en-US"
            string a = double.Parse(value.ToString()).ToString("#,###", cul.NumberFormat);
            a += " đ";
            return a;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");   // try with "en-US"
            string s = value.ToString();//.Substring(0, value.ToString().Length - 1);
            double plain = double.Parse(s, NumberStyles.Currency, cul);
            return plain;
        }
    }
}
