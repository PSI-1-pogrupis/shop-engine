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
        public IActionResult GetAll()
        {
            using (IProductRepository repo = new MySqlProductRepository())
            {
                return Ok(repo.GetAll());
            }
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            using (IProductRepository repo = new MySqlProductRepository())
            {
                ProductData foundItem = repo.GetProductByName(name);

                if (foundItem == null) return NotFound();
                else return Ok(foundItem);
            }
        }
    }
}
