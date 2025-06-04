using System;
using Humanizer;
using Microsoft.UI.Xaml.Data;

namespace RssApp.Native.Windows.Converters;

internal class RelativeDateTimeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is not DateTime dt)
        {
            throw new InvalidCastException($"Value must be of type {nameof(DateTime)}");
        }

        return dt.Humanize();
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
}
