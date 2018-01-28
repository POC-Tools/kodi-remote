using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace KodiRemote.Uwp.Converters
{
    public class BoolToValidConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            //convert value to xaml value     E8FB
            return (bool)value ? "\uE8FB" : "\uE947";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class BoolInverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }

}
