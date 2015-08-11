namespace Vurdalakov.ClipboardDotNet
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class FormatCheckConverter : IMultiValueConverter
    {
        public Object Convert(Object[] values, Type targetType, Object parameter, CultureInfo culture)
        {
            return !(Boolean)values[0] && (Boolean)values[1];
        }

        public Object[] ConvertBack(Object value, Type[] targetTypes, Object parameter, CultureInfo culture)
        {
            return new Object[0];
        }
    }
}
