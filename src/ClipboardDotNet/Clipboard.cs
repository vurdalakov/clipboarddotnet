using System;
using System.Collections.Generic;
using System.Text;

namespace Vurdalakov.ClipboardDotNet
{
    public static class Clipboard
    {
        static public void Empty()
        {
            using (var clipboard = new ClipboardApi())
            {
                ClipboardApiException.ThrowIfFailed(!Win32Api.EmptyClipboard(), "EmptyClipboard");
            }
        }

        static public Int32 CountFormats()
        {
            return Win32Api.CountClipboardFormats();
        }

        static public Boolean IsFormatAvailable(UInt16 format)
        {
            return Win32Api.IsClipboardFormatAvailable(format);
        }

        static public ClipboardEntry[] GetEntries()
        {
            var formats = GetFormats();

            var entries = new List<ClipboardEntry>(formats.Length);

            foreach (var format in formats)
            {
                entries.Add(new ClipboardEntry(format, GetFormatName(format), GetDataSize(format)));
            }

            return entries.ToArray();
        }

        static public UInt16[] GetFormats()
        {
            var formats = new List<UInt16>();

            using (var clipboard = new ClipboardApi())
            {
                var format = (UInt16)clipboard.EnumFormats(0);

                while (format != 0)
                {
                    formats.Add(format);

                    format = clipboard.EnumFormats(format);
                }
            }

            return formats.ToArray();
        }

        static public String GetRegisteredFormatName(UInt16 format)
        {
            var stringBuilder = new StringBuilder(256);
            return Win32Api.GetClipboardFormatName(format, stringBuilder, stringBuilder.Capacity) > 0 ? stringBuilder.ToString() : null;
        }

        static public String GetFormatName(UInt16 format)
        {
            // registered Clipboard formats
            var name = GetRegisteredFormatName(format);
            if (name != null)
            {
                return name;
            }

            // standard Clipboard formats
            var clipboardStandardFormats = Enum.GetValues(typeof(ClipboardFormats));
            foreach (ClipboardFormats clipboardStandardFormat in clipboardStandardFormats)
            {
                if ((UInt16)clipboardStandardFormat == format)
                {
                    return clipboardStandardFormat.ToString();
                }
            }

            // private Clipboard formats
            if ((format > Win32Api.CF_PRIVATEFIRST) && (format < Win32Api.CF_PRIVATELAST))
            {
                return String.Format("CF_PRIVATEFIRST + {0}", format - Win32Api.CF_PRIVATEFIRST);
            }
            else if ((format > Win32Api.CF_GDIOBJFIRST) && (format < Win32Api.CF_GDIOBJLAST))
            {
                return String.Format("CF_GDIOBJFIRST + {0}", format - Win32Api.CF_PRIVATEFIRST);
            }

            return format.ToString();
        }

        static public UInt64 GetDataSize(UInt16 format)
        {
            using (var clipboard = new ClipboardApi())
            {
                return clipboard.GetDataSize(format);
            }
        }

        static public Byte[] GetData(UInt16 format)
        {
            using (var clipboard = new ClipboardApi())
            {
                return clipboard.GetData(format);
            }
        }

        static public String GetText()
        {
            var formats = GetFormats();

            foreach (var format in formats)
            {
                var clipboardFormat = (ClipboardFormats)format;
                if (ClipboardText.IsTextFormat(clipboardFormat))
                {
                    var data = Clipboard.GetData(format);
                    return ClipboardText.ExtractText(clipboardFormat, data);
                }
            }

            return null;
        }

        static public void SetData(UInt16 format, Byte[] data)
        {
            using (var clipboard = new ClipboardApi())
            {
                clipboard.SetData(format, data);
            }
        }

        static public void SetText(String text)
        {
            Clipboard.Empty();

            using (var clipboard = new ClipboardApi())
            {
                var data = Encoding.Unicode.GetBytes(text + "\0");
                clipboard.SetData((UInt16)ClipboardFormats.CF_UNICODETEXT, data);
            }
        }

        static public UInt32 GetSequenceNumber()
        {
            return Win32Api.GetClipboardSequenceNumber();
        }
    }
}
