namespace Vurdalakov
{
    using System;
    using System.ComponentModel;
    using System.Runtime.InteropServices;

    public class MessageOnlyWindow : IDisposable
    {
        private IntPtr hInstance = IntPtr.Zero;
        private IntPtr classAtom = IntPtr.Zero;

        private WNDPROC windowProc;

        public IntPtr WindowHandle { get; private set; }

        public MessageOnlyWindow()
        {
            this.WindowHandle = IntPtr.Zero;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.DestroyWindow();
            }
        }

        public void CreateWindow(String className = "VurdalakovMessageOnlyWindow")
        {
            // intentionally here, prevents garbage collection
            this.windowProc = this.OnWindowProc;

            this.hInstance = GetModuleHandle(null);

            WNDCLASSEX wndClassEx = new WNDCLASSEX();
            wndClassEx.cbSize = Marshal.SizeOf(typeof(WNDCLASSEX));
            wndClassEx.lpfnWndProc = this.windowProc;
            wndClassEx.hInstance = this.hInstance;
            wndClassEx.lpszClassName = className;

            UInt16 atom = RegisterClassEx(ref wndClassEx);
            if (0 == atom)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error(), "RegisterClassEx failed");
            }

            this.classAtom = new IntPtr(atom);

            this.WindowHandle = CreateWindowEx(0, this.classAtom, IntPtr.Zero, 0, 0, 0, 0, 0, new IntPtr(HWND_MESSAGE), IntPtr.Zero, this.hInstance, IntPtr.Zero);
            if (IntPtr.Zero == this.WindowHandle)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error(), "CreateWindowEx failed");
            }
        }

        public void DestroyWindow()
        {
            if (this.WindowHandle != IntPtr.Zero)
            {
                DestroyWindow(this.WindowHandle);

                this.WindowHandle = IntPtr.Zero;
            }

            if (this.classAtom != IntPtr.Zero)
            {
                UnregisterClass(this.classAtom, this.hInstance);
            }
        }

        protected virtual IntPtr OnWindowProc(IntPtr hWnd, UInt32 msg, UIntPtr wParam, IntPtr lParam)
        {
            return DefWindowProcW(hWnd, msg, wParam, lParam);
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct WNDCLASSEX
        {
            public Int32 cbSize;
            public UInt32 style;
            public WNDPROC lpfnWndProc;
            public Int32 cbClsExtra;
            public Int32 cbWndExtra;
            public IntPtr hInstance;
            public IntPtr hIcon;
            public IntPtr hCursor;
            public IntPtr hbrBackground;
            public String lpszMenuName;
            public String lpszClassName;
            public IntPtr hIconSm;
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetModuleHandle(String lpModuleName);
        
        [DllImport("user32.dll", SetLastError = true)]
        public static extern UInt16 RegisterClassEx(ref WNDCLASSEX lpwcx);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern Boolean UnregisterClass(IntPtr lpClassName, IntPtr hInstance);

        public const Int32 HWND_MESSAGE = -3;

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr CreateWindowEx(UInt32 dwExStyle, IntPtr lpClassName, IntPtr lpWindowName, UInt32 dwStyle,
           Int32 x, Int32 y, Int32 nWidth, Int32 nHeight, IntPtr hWndParent, IntPtr hMenu, IntPtr hInstance, IntPtr lpParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern System.IntPtr DefWindowProcW(IntPtr hWnd, UInt32 msg, UIntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern Boolean DestroyWindow(IntPtr hWnd);

        public delegate IntPtr WNDPROC(IntPtr hWnd, UInt32 msg, UIntPtr wParam, IntPtr lParam);
    }
}
