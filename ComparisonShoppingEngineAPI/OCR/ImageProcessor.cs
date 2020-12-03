using FredsImageMagickScripts;
using ImageMagick;
using System;
using System.Drawing;


namespace ComparisonShoppingEngineAPI
{
    internal class ImageProcessor
    {
        private delegate void PerformResize(float f1, float f2);
        public MagickImage ProcessImage(MagickImage img)
        {
            PerformResize resizeImage = (width, height) =>
            {
                int newWidth = (int)(1.5f * width);
                int newHeight = (int)(1.5f * height);
                img.Resize(newWidth, newHeight);
            };
            resizeImage(img.Width, img.Height);
            //img.Density = new Density(300f);
            TextCleanerScript cleaner = new TextCleanerScript();
            img.Deskew(new Percentage(80));
            
            img = cleaner.Execute(img);
            

            return img;
        }


       
    }
}