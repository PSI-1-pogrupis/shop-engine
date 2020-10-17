using System;
using Tesseract;
using System.Drawing;
using System.IO;
using ImageMagick;

namespace CSE.BL
{
    public class ImageReader
    {
        public string ReadImage(string imgPath)
        {

            var img = new MagickImage(imgPath);
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
            TesseractEngine engine = new TesseractEngine("./OCR/tessdata", "lit", EngineMode.Default);
            Page page = engine.Process(pixImg, PageSegMode.SparseText);
            string text = page.GetText();

            // temp for debugging purposes
            File.WriteAllText("results.txt", text);


            return text;
        }

    }
}
