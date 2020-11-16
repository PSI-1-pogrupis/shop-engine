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

        public ProductsController(ILogger<ProductsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetAllProducts()
        {
            using (IProductRepository repo = new MySqlProductRepository())
            {
                return Ok(repo.GetAll());
            }
        }

        [HttpGet("{name}")]
        public IActionResult GetProductByName(string name)
        {
            using (IProductRepository repo = new MySqlProductRepository())
            {
                ProductData foundItem = repo.GetProductByName(name);

                if (foundItem == null) return NotFound();
                else return Ok(foundItem);
            }
        }

        [HttpPut("update")]
        public IActionResult UpdateProduct([FromBody] ProductData data)
        {
            if (data == null) return BadRequest();

            using (IProductRepository repo = new MySqlProductRepository())
            {
                ProductData updatedItem = repo.Update(data);

                if (updatedItem == null) return NotFound();
                else
                {
                    repo.SaveChanges();
                    return Ok(updatedItem);
                }
                
            }
        }

        [HttpPost("create")]
        public IActionResult CreateProduct([FromBody] ProductData data)
        {
            if (data == null) return BadRequest();

            using (IProductRepository repo = new MySqlProductRepository())
            {
                ProductData insertedItem = repo.Insert(data);

                if (insertedItem == null) return NotFound();
                else
                {
                    repo.SaveChanges();
                    return Ok(insertedItem);
                }

            }
        }
    }
}
