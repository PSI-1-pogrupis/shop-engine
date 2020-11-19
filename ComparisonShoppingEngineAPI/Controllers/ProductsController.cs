using ComparisonShoppingEngineAPI.Data;
using ComparisonShoppingEngineAPI.Models;
using ComparisonShoppingEngineAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComparisonShoppingEngineAPI.Controllers
{
    [ApiController]
    [Route("products")]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IProductService _productService;

        public ProductsController(ILogger<ProductsController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        [HttpGet]
        public IActionResult GetAllProducts() => Ok(_productService.GetAll());

        
        [HttpGet("{name}")]
        public IActionResult GetProductByName(string name)
        {
            ProductDto foundItem = _productService.GetProductByName(name);

            if (foundItem == null) return NotFound();
            else return Ok(foundItem);

        }

        [HttpPut]
        public IActionResult UpdateProduct([FromBody] ProductDto data)
        {
            if (data == null) return BadRequest();

            ProductDto updatedItem = _productService.Update(data);

            if (updatedItem == null) return NotFound();
            else return Ok(updatedItem);
        }

        [HttpPost]
        public IActionResult CreateProduct([FromBody] ProductDto data)
        {
            if (data == null) return BadRequest();

            ProductDto insertedItem = _productService.Insert(data);

            if (insertedItem == null) return NotFound();
            else return Ok(insertedItem);

        }
        
    }
}
