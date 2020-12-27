using ComparisonShoppingEngineAPI.Data.Utilities;

namespace ComparisonShoppingEngineAPI.Data.Models
{
    public class Image
    {
        public int ImageId { get; set; }
        public int UserId { get; set; }
        public ImageTypes ImageType { get; set; }
        public string ImageName { get; set; }
        public string ImagePath { get; set; }
    }
}
