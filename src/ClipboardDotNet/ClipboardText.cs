namespace Vurdalakov.ClipboardDotNet
{
    using System;
    using System.Text;

    public static class ClipboardText
    {
        static public Boolean IsTextFormat(ClipboardFormats format)
        {
            if ((ClipboardFormats.CF_UNICODETEXT == format) || (ClipboardFormats.CF_TEXT == format) || (ClipboardFormats.CF_OEMTEXT == format))
            {
                return true;
            }

            var name = Clipboard.GetFormatName((UInt16)format);

            return name.Equals("Rich Text Format", StringComparison.CurrentCultureIgnoreCase) || name.Equals("HTML Format", StringComparison.CurrentCultureIgnoreCase);
        }

        static internal String ExtractText(ClipboardFormats format, Byte[] data)
        {
            switch (format)
            {
                case ClipboardFormats.CF_UNICODETEXT:
                    return Encoding.Unicode.GetString(data, 0, data.Length - 2);
                case ClipboardFormats.CF_TEXT:
                    return Encoding.Default.GetString(data, 0, data.Length - 1);
                case ClipboardFormats.CF_OEMTEXT:
                    return Encoding.GetEncoding((int)Win32Api.GetOEMCP()).GetString(data, 0, data.Length - 1);
            }

            return null;
        }
    }
}
