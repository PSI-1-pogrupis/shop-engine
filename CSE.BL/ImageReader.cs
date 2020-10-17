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

            var img = new MagickImage(imgName);
            ImageProcessor imageProcessor = new ImageProcessor();
            img = imageProcessor.ProcessImage(img);
            img.Write("temp1.png");


            Pix pixImg;
            try
            {
                pixImg = Pix.LoadFromFile("temp1.png");
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
