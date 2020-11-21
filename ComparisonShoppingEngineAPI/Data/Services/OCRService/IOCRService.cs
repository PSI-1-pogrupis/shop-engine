using ComparisonShoppingEngineAPI.Data.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComparisonShoppingEngineAPI.Data.Services.OCRService
{
    public interface IOCRService
    {
        public Task<ServiceResponse<string>> ReadImage(IFormFile file);
    }
}
