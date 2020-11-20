using ComparisonShoppingEngineAPI.Data;
using ComparisonShoppingEngineAPI.Data.Models;
using ComparisonShoppingEngineAPI.DTOs.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComparisonShoppingEngineAPI.Controllers
{
    [ApiController]
    [Route("products")]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _productService;

        public ProductController(ILogger<ProductController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts() => Ok(await _productService.GetAll());


        [HttpGet("{name}")]
        public async Task<IActionResult> GetProductByName(string name)
        {
            ServiceResponse<ProductDto> serviceResponse = await _productService.GetProductByName(name);

            if (!serviceResponse.Success) return NotFound(serviceResponse);
            else return Ok(serviceResponse);

        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductDto data)
        {
            if (data == null) return BadRequest();
            ServiceResponse<List<ProductDto>> serviceResponse = await _productService.Update(data);

            if (!serviceResponse.Success) return NotFound(serviceResponse);
            else
            {
                return Ok(serviceResponse);
            }

        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductDto data)
        {
            if (data == null) return BadRequest();
            ServiceResponse<List<ProductDto>> serviceResponse = await _productService.Insert(data);

            if (!serviceResponse.Success) return NotFound(serviceResponse);
            else
            {
                return Ok(serviceResponse);
            }

        }

        [HttpDelete("{name}")]
        public async Task<IActionResult> Delete(string name)
        {
            ServiceResponse<List<ProductDto>> serviceResponse = await _productService.Delete(name);

            if (!serviceResponse.Success) return NotFound(serviceResponse);
            else return Ok(serviceResponse);

        }
    }
}
