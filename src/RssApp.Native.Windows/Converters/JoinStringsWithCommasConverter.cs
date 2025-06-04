using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Data;

namespace RssApp.Native.Windows.Converters;

internal class JoinStringsWithCommasConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is not IEnumerable<string> s)
        {
            throw new InvalidCastException($"Must be of type {nameof(IEnumerable<string>)}");
        }

        return string.Join(", ", s);
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
}
