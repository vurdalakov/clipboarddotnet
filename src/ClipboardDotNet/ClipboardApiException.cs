namespace Vurdalakov.ClipboardDotNet
{
    using System;
    using System.ComponentModel;
    using System.Runtime.InteropServices;

    public class ClipboardApiException : Win32Exception
    {
        public String FunctionName { get; private set; }

        internal ClipboardApiException(String functionName, Int32 error, String message) : base(error, message)
        {
            this.FunctionName = functionName;
        }

        internal static void ThrowIfFailed(Boolean failed, String functionName)
        {
            if (failed)
            {
                var error = Marshal.GetLastWin32Error();
                var message = String.Format("Function {0}() failed with error {1} (0x{1:X8}).", functionName, error);

                throw new ClipboardApiException(functionName, error, message);
            }
        }
    }
}
