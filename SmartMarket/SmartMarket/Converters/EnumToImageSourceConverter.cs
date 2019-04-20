using System;
using System.Globalization;
using Xamarin.Forms;

namespace SmartMarket.Converters
{
    public class EnumToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //var statusOfLeadModel = (StatusOfLeadModel)value;

            //switch (statusOfLeadModel)
            //{
            //    case StatusOfLeadModel.EmptyCircle:
            //        return "ic_circle";
            //    case StatusOfLeadModel.HalfCircle:
            //        return "ic_contrast_circle_symbol";
            //    case StatusOfLeadModel.CheckedCircle:
            //        return "ic_circle_with_check_symbol";
            //    case StatusOfLeadModel.QuestionMark:
            //        return "ic_question_mark";
            //}

            return "ic_circle";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (string)value;
            //return $"ic_{((string)value)?.ToLower()}.png";
        }
    }
}
