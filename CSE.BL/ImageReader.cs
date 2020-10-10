using System;
using Tesseract;
using System.Drawing;
using System.IO;

namespace CSE.BL
{
    public class ImageReader
    {
        public string ReadImage(string imgName)
        {
            Bitmap bmp = (Bitmap)Bitmap.FromFile(imgName);
            ImageProcessor imageProcessor = new ImageProcessor();
            bmp = imageProcessor.ProcessImage(bmp);


            bmp.Save("temp.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);

            Pix img;
            try
            {
                img = Pix.LoadFromFile("temp.jpg");
                File.Delete("temp.jpg");
            }
            catch (IOException)
            {
                return "";
            }
            TesseractEngine engine = new TesseractEngine("./tessdata", "lit", EngineMode.Default);
            Page page = engine.Process(img, PageSegMode.Auto);
            string text = page.GetText();



            return text;
        }

    }
}
