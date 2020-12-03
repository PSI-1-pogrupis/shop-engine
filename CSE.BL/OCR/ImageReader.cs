﻿using System;
using Tesseract;
using System.Drawing;
using System.IO;
using ImageMagick;
using System.Threading.Tasks;

namespace CSE.BL
{
    public class ImageReader
    {
        public async Task<string> ReadImageAsync(string imgPath)
        {
            return await Task.Run(() => ReadImage(imgPath));
        }

        public delegate void ImageProcessedEventHandler(object source, EventArgs args);

        public event ImageProcessedEventHandler ImageProcessed;

        protected virtual void OnImageProcessed()
        {
            ImageProcessed?.Invoke(this, EventArgs.Empty);
        }

        public string ReadImage(string imgPath)
        {
            var img = new MagickImage(imgPath);
            ImageProcessor imageProcessor = new ImageProcessor();
            img = imageProcessor.ProcessImage(img);
            OnImageProcessed();
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
            Page page = engine.Process(pixImg, PageSegMode.SingleBlock);    // this one is perfect if the image is cropped
            //Page page = engine.Process(pixImg, PageSegMode.Auto);
            string text = page.GetText();

            // temp for debugging purposes
            File.WriteAllText("results.txt", text);

            return text;
        }
    }
}
