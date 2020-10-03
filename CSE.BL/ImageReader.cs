using System;
using Tesseract;
using System.Drawing;

namespace CSE.BL
{
    public class ImageReader
    {
        public string ReadImage(string imgName)
        {
            Pix img;
            try
            {
                img = Pix.LoadFromFile(imgName);
            }
            catch (System.IO.IOException)
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
