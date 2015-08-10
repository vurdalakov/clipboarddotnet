namespace Vurdalakov.ClipboardDotNet
{
    using System;
    using System.Runtime.InteropServices;

    internal class ClipboardApi : IDisposable
    {
        public ClipboardApi()
        {
            for (var i = 0; i < 5; i++) // sometimes OpenClipboard fails with error 5; retry after small delay always helps
            {
                if (Win32Api.OpenClipboard(IntPtr.Zero))
                {
                    return;
                }

                System.Threading.Thread.Sleep(100);
            }

            ClipboardApiException.ThrowIfFailed(true, "OpenClipboard");
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
            return IntPtr.Zero == ptr ? new Byte[0] : GlobalMemory.GetData(ptr);
        }

        public UInt64 GetDataSize(UInt16 format)
        {
            var ptr = GetDataPtr(format);
            return IntPtr.Zero == ptr ? 0 : GlobalMemory.GetDataSize(ptr);
        }

        private IntPtr GetDataPtr(UInt16 format)
        {
            return Win32Api.GetClipboardData(format);
        }

        public void SetData(UInt16 format, Byte[] data)
        {
            var ptr = GlobalMemory.SetData(data);
            ptr = Win32Api.SetClipboardData(format, ptr);

            ClipboardApiException.ThrowIfFailed(IntPtr.Zero == ptr, "SetClipboardData");
        }
    }
}
