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
            img.Contrast();
            //img.Density = new Density(200f);
            img.Resize((int)(img.Width * resizeCoef), (int)(img.Height * resizeCoef));
            img.Sharpen();
            //img.Grayscale();
            img.ReduceNoise();
            img.Deskew(new Percentage(80));
            //img.Threshold(new Percentage(85));
            img.Negate();

            return img;
        }


       
    }
}