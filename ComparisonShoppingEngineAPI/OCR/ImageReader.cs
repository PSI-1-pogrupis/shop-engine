using System;
using Tesseract;
using System.Drawing;
using System.IO;
using ImageMagick;
using System.Threading.Tasks;

namespace ComparisonShoppingEngineAPI
{
    public class ImageReader
    {
        public async Task<string> ReadImageAsync(Stream stream)
        {
            return await Task.Run(() => ReadImage(stream));
        }


        public string ReadImage(Stream stream)
        {
            var img = new MagickImage(stream);
            ImageProcessor imageProcessor = new ImageProcessor();
            img = imageProcessor.ProcessImage(img);
            img.Write("temp1.png");

            Pix pixImg;
            try
            {
                pixImg = Pix.LoadFromFile("temp1.png");
                File.Delete("temp1.png");
            }
            catch (IOException)
            {
                return "";
            }

            TesseractEngine engine = new TesseractEngine("./OCR/tessdata", "lit", EngineMode.Default);
            Page page = engine.Process(pixImg, PageSegMode.SingleBlock);    // this one is perfect if the image is cropped
            //Page page = engine.Process(pixImg, PageSegMode.Auto);
            string text = page.GetText();

            // temp for debugging purposes
            //File.WriteAllText("results.txt", text);

            return text;
        }
    }
}
