using ComparisonShoppingEngineAPI.Data.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComparisonShoppingEngineAPI.DTOs.Image
{
    public class ImageDto
    {
        public ImageTypes ImageType { get; set; }
        public string ImagePath { get; set; }
    }
}
