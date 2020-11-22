using ComparisonShoppingEngineAPI.Data.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComparisonShoppingEngineAPI.Data.Services.OCRService
{
    public class OCRService : IOCRService
    {
        public async Task<ServiceResponse<string>> ReadImage(IFormFile file)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            ImageReader reader = new ImageReader();
            string text = await reader.ReadImageAsync(file.OpenReadStream());
            response.Data = text;
            return response;
        }
    }
}
