namespace Vurdalakov.ClipboardDotNet
{
    using System;
    using System.Globalization;
    using System.Text;
    using System.Windows.Data;
    using System.Windows.Markup;

    // http://drwpf.com/blog/2009/03/17/tips-and-tricks-making-value-converters-more-accessible-in-markup/
    public class HexLineConverter : MarkupExtension, IValueConverter
    {
        private static HexLineConverter _converter = null;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (null == _converter)
            {
                _converter = new HexLineConverter();
            }

            return _converter;
        }

        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            var data = value as Byte[];

            var stringBuilder = new StringBuilder(128);

            for (var i = 0; i < 16; i++)
            {
                if ((parameter as String).Equals("1"))
                {
                    if (8 == i)
                    {
                        stringBuilder.Append(data.Length < 9 ? "  " : "| ");
                    }

                    if (i < data.Length)
                    {
                        stringBuilder.AppendFormat("{0:X02} ", data[i]);
                    }
                    else
                    {
                        stringBuilder.Append("   ");
                    }
                }
                else
                {
                    if (i < data.Length)
                    {
                        stringBuilder.Append(data[i] > 31 ? (char)data[i] : ' ');
                    }
                    else
                    {
                        stringBuilder.Append(' ');
                    }
                }
            }

            return stringBuilder.ToString();
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
