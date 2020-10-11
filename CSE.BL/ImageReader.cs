using System;
using Tesseract;
using System.Drawing;
using System.IO;
using ImageMagick;

namespace CSE.BL
{
    public class ImageReader
    {
        public string ReadImage(string imgName)
        {
            /*Bitmap bmp = (Bitmap)Bitmap.FromFile(imgName);
            ImageProcessor imageProcessor = new ImageProcessor();
            bmp = imageProcessor.ProcessImage(bmp);
            bmp.Save("temp.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);*/

            var img = new MagickImage(imgName);
            ImageProcessor imageProcessor = new ImageProcessor();
            img = imageProcessor.ProcessImage(img);
            img.Write("temp1.jpg");


            Pix pixImg;
            try
            {
                pixImg = Pix.LoadFromFile("temp1.jpg");
                File.Delete("temp.jpg");
                //File.Delete("temp1.jpg");
            }
            catch (IOException)
            {
                return "";
            }
            TesseractEngine engine = new TesseractEngine("./tessdata", "lit", EngineMode.Default);
            Page page = engine.Process(pixImg, PageSegMode.Auto);
            string text = page.GetText();



            return text;
        }

    }
}
