namespace Vurdalakov
{
    using System;
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Interop;

    public static class WindowHelper
    {
        #region RemoveIcon

        public static void RemoveIcon(Window window)
        {
            if (null == window)
            {
                return;
            }

            var hWnd = new WindowInteropHelper(window).Handle;

            var exStyle = GetWindowLong(hWnd, GWL_EXSTYLE);
            SetWindowLong(hWnd, GWL_EXSTYLE, exStyle | WS_EX_DLGMODALFRAME);

            SendMessage(hWnd, WM_SETICON, IntPtr.Zero, IntPtr.Zero);
            SendMessage(hWnd, WM_SETICON, new IntPtr(1), IntPtr.Zero);

            SetWindowPos(hWnd, IntPtr.Zero, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_NOACTIVATE | SWP_FRAMECHANGED);
        }

        public static void RemoveIcon(Object sender, EventArgs e)
        {
            RemoveIcon(sender as Window);
        }

        #endregion

        #region RemoveMinimizeAndMaximizeBoxes

        public static void RemoveMinimizeAndMaximizeBoxes(Window window, Boolean removeMinimizeBox, Boolean removeMaximizeBox)
        {
            if (null == window)
            {
                return;
            }

            var styleBits = (removeMinimizeBox ? WS_MINIMIZEBOX : 0) | (removeMaximizeBox ? WS_MAXIMIZEBOX : 0);

            var hWnd = new WindowInteropHelper(window).Handle;

            var style = GetWindowLong(hWnd, GWL_STYLE);
            SetWindowLong(hWnd, GWL_STYLE, (int)(style & ~(styleBits)));

            SetWindowPos(hWnd, IntPtr.Zero, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_NOACTIVATE | SWP_FRAMECHANGED);
        }

        public static void RemoveMinimizeAndMaximizeBoxes(Object sender, Boolean removeMinimizeBox, Boolean removeMaximizeBox)
        {
            RemoveMinimizeAndMaximizeBoxes(sender as Window, removeMinimizeBox, removeMaximizeBox);
        }

        public static void RemoveMinimizeAndMaximizeBoxes(Object sender, EventArgs e)
        {
            RemoveMinimizeAndMaximizeBoxes(sender as Window, true, true);
        }

        #endregion

        #region Win32 API Interop

        private const Int32 GWL_STYLE = -16;
        private const Int32 GWL_EXSTYLE = -20;
        private const Int32 WS_MAXIMIZEBOX = 0x10000;
        private const Int32 WS_MINIMIZEBOX = 0x20000;
        private const Int32 WS_EX_DLGMODALFRAME = 0x0001;
        private const Int32 SWP_NOSIZE = 0x0001;
        private const Int32 SWP_NOMOVE = 0x0002;
        private const Int32 SWP_NOZORDER = 0x0004;
        private const Int32 SWP_NOACTIVATE = 0x0010;
        private const Int32 SWP_FRAMECHANGED = 0x0020;
        private const UInt32 WM_SETICON = 0x0080;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern Int32 GetWindowLong(IntPtr hWnd, Int32 nIndex);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern Int32 SetWindowLong(IntPtr hWnd, Int32 nIndex, Int32 dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern Boolean SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, Int32 x, Int32 y, Int32 cx, Int32 cy, UInt32 uFlags);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        #endregion
    }
}
