namespace Vurdalakov.ClipboardDotNet
{
    using System;
    using System.Runtime.InteropServices;

    internal class ClipboardApi : IDisposable
    {
        public ClipboardApi()
        {
            ClipboardApiException.ThrowIfFailed(!Win32Api.OpenClipboard(IntPtr.Zero), "OpenClipboard");
        }

        public void Dispose()
        {
            Win32Api.CloseClipboard();
        }

        public UInt16 EnumFormats(UInt16 format)
        {
            var nextFormat = (UInt16)Win32Api.EnumClipboardFormats(format);

            ClipboardApiException.ThrowIfFailed((0 == nextFormat) && (Marshal.GetLastWin32Error() != 0), "EnumClipboardFormats");

            return nextFormat;
        }

        public Byte[] GetData(UInt16 format)
        {
            var ptr = GetDataPtr(format);
            return GlobalMemory.GetData(ptr);
        }

        public UInt64 GetDataSize(UInt16 format)
        {
            var ptr = GetDataPtr(format);
            return GlobalMemory.GetDataSize(ptr);
        }

        private IntPtr GetDataPtr(UInt16 format)
        {
            var ptr = Win32Api.GetClipboardData(format);

            ClipboardApiException.ThrowIfFailed(IntPtr.Zero == ptr, "GetClipboardData");

            return ptr;
        }

        public void SetData(UInt16 format, Byte[] data)
        {
            var ptr = GlobalMemory.SetData(data);
            ptr = Win32Api.SetClipboardData(format, ptr);

            ClipboardApiException.ThrowIfFailed(IntPtr.Zero == ptr, "SetClipboardData");
        }
    }
}
