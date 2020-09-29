using System;
using Tesseract;
using System.Drawing;

namespace CSE.BL
{
    public class ImageReader
    {
        public string ReadImage(string imgName)
        {
            //TODO: HANDLE EXCEPTION
            Pix img = Pix.LoadFromFile(imgName);       // image must be copied to output dir (on image properties -> copy to output dir = true)
                                                       // ofc for phone app other solution will be found
            TesseractEngine engine = new TesseractEngine("./tessdata", "lit", EngineMode.Default);
            Page page = engine.Process(img, PageSegMode.Auto);
            string text = page.GetText();

            return text;
        }
    }
}
