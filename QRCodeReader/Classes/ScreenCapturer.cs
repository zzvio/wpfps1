using SCapture.Properties;
using System;
using System.IO;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace SCapture.Classes
{
    class ScreenCapturer
    {
        /// <summary>
        /// Captures the graphical content of the given region
        /// </summary>
        /// <returns>The image captured</returns>
        public static BitmapSource CaptureRegion(int Left, int Top, int Width, int Height)
        {
            IntPtr dc1 = NativeMethods.GetDC(NativeMethods.GetDesktopWindow());
            IntPtr dc2 = NativeMethods.CreateCompatibleDC(dc1);

            // Create Bitmap
            IntPtr hBitmap = NativeMethods.CreateCompatibleBitmap(dc1, Width, Height);

            NativeMethods.SelectObject(dc2, hBitmap);
            NativeMethods.BitBlt(dc2, 0, 0, Width, Height, dc1, Left, Top, 0x00CC0020);

            // Get BitmapSource
            BitmapSource bSource = Imaging.CreateBitmapSourceFromHBitmap(hBitmap,
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            // Release resources
            NativeMethods.DeleteObject(hBitmap);
            NativeMethods.ReleaseDC(IntPtr.Zero, dc1);
            NativeMethods.ReleaseDC(IntPtr.Zero, dc2);

            return bSource;
        }

        /// <summary>
        /// Captures the graphical content of the given window
        /// </summary>
        /// <param name="hWnd">Window handle to capture</param>
        /// <returns>The image captured</returns>
        public static BitmapSource CaptureWindow(IntPtr hWnd)
        {
            // Get window rect
            RECT rc;
            NativeMethods.GetWindowRect(hWnd, out rc);

            // Bring window to the front
            NativeMethods.SetForegroundWindow(hWnd);

            // Small hack to fix black border arround window
            int xOffset = 8;
            return CaptureRegion(
                rc.Left + xOffset,
                rc.Top + xOffset,
                rc.Width - xOffset * 2,
                rc.Height - xOffset * 2);
        }

        /// <summary>
        /// Captures the graphical content of screen
        /// </summary>
        /// <returns>The image captured</returns>
        public static BitmapSource CaptureFullScreen()
        {
            return CaptureRegion(0, 0,
                (int)SystemParameters.PrimaryScreenWidth,
                (int)SystemParameters.PrimaryScreenHeight);
        }

        /// <summary>
        /// Saves the capture to a file.
        /// </summary>
        /// <param name="bSource">The image captured</param>
        /// <returns>Capture saved successfully?</returns>
        public static bool Save(BitmapSource bSource)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string extension = ".jpg";

            switch (Settings.Default.SaveFileFormat)
            {
                case 0:
                    extension = ".bmp";
                    break;
                case 1:
                    extension = ".jpeg";
                    break;
                default:
                    extension = ".png";
                    break;

            }

            string fileName = desktopPath +
                $"/screenshot_{DateTime.Now.ToString("yyyy_dd_MM_HH_mm_ss")}" +
                extension;

            return Save(fileName, bSource);
        }

        /// <summary>
        /// Saves the capture to a file
        /// </summary>
        /// <param name="fileName">What's the file name?</param>
        /// <param name="bSource">The image captured</param>
        /// <returns></returns>
        public static bool Save(string fileName, BitmapSource bSource)
        {
            try
            {
                BitmapEncoder encoder;
                var extension = Path.GetExtension(fileName);
                switch (extension)
                {
                    case ".bmp":
                        encoder = new BmpBitmapEncoder();
                        break;
                    case ".jpeg":
                        encoder = new JpegBitmapEncoder();
                        break;
                    default:
                        encoder = new PngBitmapEncoder();
                        break;
                }

                encoder.Frames.Add(BitmapFrame.Create(bSource));

                var fileName2 = @"C:\Taurus\code-test\data\capture.png";
                if (File.Exists(fileName2))
                {
                    File.Delete(fileName2);
                }

                using (var stream = File.Create(fileName2))
                {
                    encoder.Save(stream);
                }


                var qrBitmap = (System.Drawing.Bitmap)System.Drawing.Bitmap.FromFile(fileName2);
                var qrReader = new ZXing.BarcodeReader();
                ZXing.Result qrResult = qrReader.Decode(qrBitmap);

                var s = qrResult.Text;

                var i1 = s.IndexOf("{");
                var i2 = s.IndexOf("}");
                var file = s.Substring(0, i1);
                var seq = s.Substring(i1 + 1, 4);
                var body = s.Substring(i2 + 1);

                Console.WriteLine(body);

                string corrected = body.Replace('-', '+').Replace('_', '/');
                if (corrected.Length % 4 > 0)
                    corrected = corrected.PadRight(corrected.Length + 4 - corrected.Length % 4, '=');

                var decodedBytes = System.Convert.FromBase64String(corrected);
                string codeStr = System.Text.Encoding.ASCII.GetString(decodedBytes);


                var fileNameLog2 = fileName2 + ".log";
                if (File.Exists(fileNameLog2))
                {
                    File.Delete(fileNameLog2);
                }
                File.WriteAllText(fileNameLog2, codeStr);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }



            return true;
        }
    }
}
