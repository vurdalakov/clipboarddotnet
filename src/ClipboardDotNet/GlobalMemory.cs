namespace Vurdalakov.ClipboardDotNet
{
    using System;
    using System.Runtime.InteropServices;

    static internal class GlobalMemory
    {
        static public UInt64 GetDataSize(IntPtr memory)
        {
            return Win32Api.GlobalSize(memory).ToUInt64();
        }

        static public Byte[] GetData(IntPtr memory)
        {
            var size = GetDataSize(memory);

            if (0 == size)
            {
                return new Byte[0];
            }

            var data = new Byte[size];

            var ptr = Win32Api.GlobalLock(memory);
            Marshal.Copy(ptr, data, 0, (int)size); //TODO: size more than int max
            Win32Api.GlobalUnlock(memory);

            return data;
        }

        static public IntPtr SetData(Byte[] data)
        {
            var ptr = Marshal.AllocHGlobal(data.Length);
            Marshal.Copy(data, 0, ptr, data.Length);
            return ptr;
        }
    }
}
