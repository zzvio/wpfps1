using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace DisplayChat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr onj);


        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
        //Mouse actions
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        private bool _capture = true;
        private System.Drawing.Point p;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void setCursorAtPosition()
        {
            NativeMethods.SetCursorPos((int)p.X, (int)p.Y);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            setCursorAtPosition();
            //Call the imported function with the cursor's current position
            uint X = (uint)p.X;
            uint Y = (uint)p.Y;
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, X, Y, 0, 0);

            //_capture = true;
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            if (_capture)
            {
                System.Drawing.Point mp = NativeMethods.GetMousePosition();
                p = new System.Drawing.Point(mp.X, mp.Y);
                _capture = false;
            }
        }

        public void CaptureScreen(double x, double y, double width, double height)
        {
            int ix, iy, iw, ih;
            ix = Convert.ToInt32(x);
            iy = Convert.ToInt32(y);
            iw = Convert.ToInt32(width);
            ih = Convert.ToInt32(height);
            System.Drawing.Bitmap image = new System.Drawing.Bitmap(iw, ih,
                   System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(image);
            g.CopyFromScreen(ix, iy, ix, iy,
                     new System.Drawing.Size(iw, ih),
                     System.Drawing.CopyPixelOperation.SourceCopy);


            String fileName = "test.png";

            image.Save(fileName, System.Drawing.Imaging.ImageFormat.Png);

        }
    }
}
