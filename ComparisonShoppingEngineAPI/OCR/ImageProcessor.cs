using FredsImageMagickScripts;
using ImageMagick;
using System;
using System.Drawing;


namespace ComparisonShoppingEngineAPI
{
    internal class ImageProcessor
    {
        public MagickImage ProcessImage(MagickImage img)
        {
            img.Resize((int)(1.5f * img.Width), (int)(1.5f * img.Height));
            //img.Density = new Density(300f);
            TextCleanerScript cleaner = new TextCleanerScript();
            img.Deskew(new Percentage(80));
            
            img = cleaner.Execute(img);
            

            return img;
        }


       
    }
}