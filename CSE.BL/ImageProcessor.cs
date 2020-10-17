using FredsImageMagickScripts;
using ImageMagick;
using System;
using System.Drawing;


namespace CSE.BL
{
    internal class ImageProcessor
    {
        private const float resizeCoef = 1.5f;

        public MagickImage ProcessImage(MagickImage img)
        {
            TextCleanerScript cleaner = new TextCleanerScript();
            img.Deskew(new Percentage(80));
            MagickImage img1 = cleaner.Execute(img);

            return img1;
        }


       
    }
}