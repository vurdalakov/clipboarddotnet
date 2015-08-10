namespace Vurdalakov.ClipboardDotNet
{
    using System;
    using System.Runtime.InteropServices;

    static internal class GlobalMemory
    {
        static public UInt64 GetDataSize(IntPtr memory)
        {
            if (IntPtr.Zero == GlobalLock(memory))
            {
                // Not all handles returned by GetClipboardData are created with GlobalAlloc:
                // http://blogs.msdn.com/b/oldnewthing/archive/2007/10/26/5681471.aspx
                // https://msdn.microsoft.com/en-us/library/ms649014#_win32_Memory_and_the_Clipboard
                // Moreover, in case of CF_BITMAP GlobalFlags() and GlobalSize() just crash - that's why GlobalLock is in use
                return 0;
            }
            GlobalUnlock(memory);

            return GlobalSize(memory).ToUInt64();
        }

        static public Byte[] GetData(IntPtr memory)
        {
            var size = GetDataSize(memory);

            if (0 == size)
            {
                return new Byte[0];
            }

            var data = new Byte[size];

            var ptr = GlobalLock(memory);
            Marshal.Copy(ptr, data, 0, (int)size); //TODO: size more than int max
            GlobalUnlock(memory);

            return data;
        }

        static public IntPtr SetData(Byte[] data)
        {
            var ptr = Marshal.AllocHGlobal(data.Length);
            Marshal.Copy(data, 0, ptr, data.Length);
            return ptr;
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        static public extern IntPtr GlobalLock(IntPtr hMem);

        [DllImport("kernel32.dll", SetLastError = true)]
        static public extern UIntPtr GlobalSize(IntPtr hMem);

        [DllImport("kernel32.dll", SetLastError = true)]
        static public extern Boolean GlobalUnlock(IntPtr hMem);

        //const UInt32 GMEM_INVALID_HANDLE = 0x8000;

        //[DllImport("kernel32.dll", SetLastError = true)]
        //static public extern UInt32 GlobalFlags(IntPtr hMem);
    }
}
