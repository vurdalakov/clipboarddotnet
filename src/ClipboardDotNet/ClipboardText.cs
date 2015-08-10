namespace Vurdalakov.ClipboardDotNet
{
    using System;
    using System.Text;

    public static class ClipboardText
    {
        static public Boolean IsTextFormat(ClipboardFormats format)
        {
            return IsTextFormat((UInt16)format);
        }

        static public Boolean IsTextFormat(UInt16 format)
        {
            if (((UInt16)ClipboardFormats.CF_UNICODETEXT == format) || ((UInt16)ClipboardFormats.CF_TEXT == format) || ((UInt16)ClipboardFormats.CF_OEMTEXT == format))
            {
                return true;
            }

            var name = Clipboard.GetFormatName(format);
            return name.Equals("Rich Text Format", StringComparison.CurrentCultureIgnoreCase) || name.Equals("HTML Format", StringComparison.CurrentCultureIgnoreCase);
        }

        static public String ExtractText(ClipboardFormats format, Byte[] data)
        {
            return ExtractText((UInt16)format, data);
        }

        static public String ExtractText(UInt16 format, Byte[] data)
        {
            switch (format)
            {
                case (UInt16)ClipboardFormats.CF_UNICODETEXT:
                    return Encoding.Unicode.GetString(data, 0, data.Length - 2);
                case (UInt16)ClipboardFormats.CF_TEXT:
                    return Encoding.Default.GetString(data, 0, data.Length - 1);
                case (UInt16)ClipboardFormats.CF_OEMTEXT:
                    return Encoding.GetEncoding((int)Win32Api.GetOEMCP()).GetString(data, 0, data.Length - 1);
            }

            var name = Clipboard.GetFormatName(format);

            switch (name)
            {
                case "Rich Text Format":
                case "HTML Format":
                    return Encoding.Default.GetString(data, 0, data.Length - 1);
            }

            return null;
        }
    }
}
