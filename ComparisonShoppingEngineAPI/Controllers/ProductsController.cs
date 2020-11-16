using ComparisonShoppingEngineAPI.Data;
using ComparisonShoppingEngineAPI.Models;
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
        private readonly IProductRepository _repository;

        public ProductsController(ILogger<ProductsController> logger, IProductRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet]
        public IActionResult GetAllProducts() => Ok(_repository.GetAll());


        [HttpGet("{name}")]
        public IActionResult GetProductByName(string name)
        {
            ProductData foundItem = _repository.GetProductByName(name);

            if (foundItem == null) return NotFound();
            else return Ok(foundItem);

        }

        [HttpPut("update")]
        public IActionResult UpdateProduct([FromBody] ProductData data)
        {
            if (data == null) return BadRequest();

            ProductData updatedItem = _repository.Update(data);

            if (updatedItem == null) return NotFound();
            else
            {
                _repository.SaveChanges();
                return Ok(updatedItem);
            }
 
        }

        [HttpPost("create")]
        public IActionResult CreateProduct([FromBody] ProductData data)
        {
            if (data == null) return BadRequest();

            ProductData insertedItem = _repository.Insert(data);

            if (insertedItem == null) return NotFound();
            else
            {
                _repository.SaveChanges();
                return Ok(insertedItem);
            }

        }
    }
}
