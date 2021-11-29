using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing;
using System.Drawing;

namespace QRCodeReader
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //generate qr code 
            /*
            var qrCodeWriter = new BarcodeWriter();

            qrCodeWriter.Format = BarcodeFormat.QR_CODE;

            qrCodeWriter.Write("http://amkpro.com")
                .Save(@"C:\Taurus\qrcodereader\QRCodeReader\data\demo.png");
            Console.WriteLine("Qr code is generated.");
            */


            //read the qr code 
            var qrCodeBitmap = (Bitmap)Bitmap.FromFile(@"C:\Taurus\code-test\src\test\kotlin\codetest\BitBuffer.kt.2.1.png");
            var qrCodeReader = new BarcodeReader();
            var qrCodeResult = qrCodeReader.Decode(qrCodeBitmap);
            Console.WriteLine("BitBuffer.kt.2.1.png ============================================ ");
            Console.WriteLine(qrCodeResult.Text);
            Console.WriteLine("============================================");

            qrCodeBitmap = (Bitmap)Bitmap.FromFile(@"C:\Taurus\code-test\src\test\kotlin\codetest\BitBuffer.kt.2.2.png");
            qrCodeReader = new BarcodeReader();
            qrCodeResult = qrCodeReader.Decode(qrCodeBitmap);
            Console.WriteLine("BitBuffer.kt.2.2.png ============================================ ");
            Console.WriteLine(qrCodeResult.Text);
            Console.WriteLine("============================================");




        }
    }
}
