namespace Vurdalakov.ClipboardDotNet
{
    using System;
    using System.ComponentModel;
    using System.Runtime.InteropServices;

    public class ClipboardListener : MessageOnlyWindow
    {
        private Boolean _listening = false;

        public void Start()
        {
            CreateWindow();

            if (!AddClipboardFormatListener(WindowHandle))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error(), "AddClipboardFormatListener failed");
            }

            _listening = true;
        }

        public void Stop()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && _listening)
            {
                RemoveClipboardFormatListener(WindowHandle);
                _listening = false;
            }

            base.Dispose(disposing);
        }

        public event EventHandler<EventArgs> ClipboardUpdate;

        protected override IntPtr OnWindowProc(IntPtr hWnd, UInt32 msg, UIntPtr wParam, IntPtr lParam)
        {
            if ((WM_CLIPBOARDUPDATE == msg) && (ClipboardUpdate != null))
            {
                ClipboardUpdate(this, new EventArgs());
            }

            return base.OnWindowProc(hWnd, msg, wParam, lParam);
        }

        private const UInt32 WM_CLIPBOARDUPDATE = 0x031D;

        [DllImport("user32.dll", SetLastError = true)]
        public static extern Boolean AddClipboardFormatListener(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern Boolean RemoveClipboardFormatListener(IntPtr hWnd);
    }
}
