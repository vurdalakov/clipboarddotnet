using System;
using System.Text;
using System.Runtime.InteropServices;

namespace Vurdalakov.ClipboardDotNet
{
    internal class Win32Api
    {
        // kernel32.dll

        [DllImport("kernel32.dll", SetLastError = true)]
        static public extern IntPtr GlobalLock(IntPtr hMem);

        [DllImport("kernel32.dll", SetLastError = true)]
        static public extern UIntPtr GlobalSize(IntPtr hMem);

        [DllImport("kernel32.dll", SetLastError = true)]
        static public extern Boolean GlobalUnlock(IntPtr hMem);

        // kernel32.dll - NOT USED YET

        [DllImport("kernel32.dll", EntryPoint = "RtlMoveMemory", SetLastError = false)]
        public static extern void CopyMemory(IntPtr dest, IntPtr src, int size);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GlobalAlloc(uint uFlags, UIntPtr dwBytes);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GlobalFree(IntPtr hMem);

        public const uint GMEM_DDESHARE = 0x2000;
        public const uint GMEM_MOVEABLE = 0x2;

        // user32.dll

        public const UInt32 CF_PRIVATEFIRST = 0x0200;
        public const UInt32 CF_PRIVATELAST = 0x02FF;
        public const UInt32 CF_GDIOBJFIRST = 0x0300;
        public const UInt32 CF_GDIOBJLAST = 0x03FF;

        [DllImport("user32.dll", SetLastError = true)]
        static public extern Boolean CloseClipboard();

        [DllImport("user32.dll", SetLastError = true)]
        static public extern Int32 CountClipboardFormats();

        [DllImport("user32.dll", SetLastError = true)]
        static public extern Boolean EmptyClipboard();

        [DllImport("user32.dll", SetLastError = true)]
        static public extern UInt32 EnumClipboardFormats(UInt32 format);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetClipboardData(UInt32 uFormat);

        [DllImport("user32.dll", SetLastError = true)]
        static public extern Int32 GetClipboardFormatName(UInt32 format, [Out] StringBuilder lpszFormatName, Int32 cchMaxCount);

        [DllImport("user32.dll", SetLastError = true)]
        static public extern Boolean OpenClipboard(IntPtr hWndNewOwner);

        [DllImport("user32.dll", SetLastError = true)]
        static public extern Boolean IsClipboardFormatAvailable(UInt32 uFormat);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetClipboardData(uint uFormat, IntPtr hMem);

        [DllImport("kernel32.dll", SetLastError = true)]
        static public extern UInt32 GetOEMCP();
    }
}
