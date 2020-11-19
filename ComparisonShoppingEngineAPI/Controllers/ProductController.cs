using ComparisonShoppingEngineAPI.Data;
using ComparisonShoppingEngineAPI.Data.Models;
using ComparisonShoppingEngineAPI.Dtos.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComparisonShoppingEngineAPI.Controllers
{
    [ApiController]
    [Route("product")]
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
            ServiceResponse<GetProductDto> serviceResponse = await _productService.GetProductByName(name);

            if (!serviceResponse.Success) return NotFound(serviceResponse);
            else return Ok(serviceResponse);

        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateProduct([FromBody] AddProductDto data)
        {
            if (data == null) return BadRequest();
            ServiceResponse<List<GetProductDto>> serviceResponse = await _productService.Update(data);

            if (!serviceResponse.Success) return NotFound(serviceResponse);
            else
            {
                return Ok(serviceResponse);
            }

        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateProduct([FromBody] AddProductDto data)
        {
            if (data == null) return BadRequest();
            ServiceResponse<List<GetProductDto>> serviceResponse = await _productService.Insert(data);

            if (!serviceResponse.Success) return NotFound(serviceResponse);
            else
            {
                return Ok(serviceResponse);
            }

        }

        [HttpDelete("{name}")]
        public async Task<IActionResult> Delete(string name)
        {
            ServiceResponse<List<GetProductDto>> serviceResponse = await _productService.Delete(name);

            if (!serviceResponse.Success) return NotFound(serviceResponse);
            else return Ok(serviceResponse);

        }
    }
}
