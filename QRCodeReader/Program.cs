using System;
using System.IO;
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
            var qrPath = @"C:\Taurus\code-test\data\capture2.png";

            //var qrPath = @"C:\Taurus\code-test\data\codetest\BitBuffer.kt0201.png";
            var qrCodeBitmap = (Bitmap)Bitmap.FromFile(qrPath);
            var qrCodeReader = new BarcodeReader();
            var qrCodeResult = qrCodeReader.Decode(qrCodeBitmap);

            var file = qrPath + ".log";
            if (File.Exists(file))
            {
                File.Delete(file);
            }
            File.WriteAllText(file, qrCodeResult.Text);

            Console.WriteLine(""); Console.WriteLine(""); Console.WriteLine("");
            Console.WriteLine("===== Wrote === " + file);
            Console.WriteLine(qrCodeResult.Text);

            Console.WriteLine(""); Console.WriteLine(""); Console.WriteLine("");

            //var qrPath2 = @"C:\Taurus\code-test\data\codetest\BitBuffer.kt0202.png";
            var qrPath2 = @"C:\Taurus\code-test\data\capture.png";
            var qrCodeBitmap2 = (Bitmap)Bitmap.FromFile(qrPath2);
            var qrCodeReader2 = new BarcodeReader();
            var qrCodeResult2 = qrCodeReader2.Decode(qrCodeBitmap2);

            var file2 = qrPath2 + ".log";
            if (File.Exists(file2))
            {
                File.Delete(file2);
            }
            File.WriteAllText(file2, qrCodeResult2.Text);


            Console.WriteLine(""); Console.WriteLine(""); Console.WriteLine("");
            Console.WriteLine("===== Wrote ===" + file2);
            Console.WriteLine(qrCodeResult2.Text);
            Console.WriteLine("============================================");

            var s = qrCodeResult2.Text;

            var i1 = s.IndexOf("{");
            var i2 = s.IndexOf("}");
            var str = s.Substring(0, i1);
            var seq = s.Substring(i1 + 1, 4);
            var body = s.Substring(i2 + 1);

            Console.WriteLine(body);

            string corrected = body.Replace('-', '+').Replace('_', '/');
            if (corrected.Length % 4 > 0)
                corrected = corrected.PadRight(corrected.Length + 4 - corrected.Length % 4, '=');

            var decodedBytes = System.Convert.FromBase64String(corrected);
            string codeStr = Encoding.ASCII.GetString(decodedBytes);

            Console.WriteLine(codeStr);



        }
    }
}
