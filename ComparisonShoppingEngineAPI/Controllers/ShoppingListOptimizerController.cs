using ComparisonShoppingEngineAPI.Data;
using ComparisonShoppingEngineAPI.Data.Models;
using ComparisonShoppingEngineAPI.Data.Services.OptimizerService;
using ComparisonShoppingEngineAPI.DTOs;
using ComparisonShoppingEngineAPI.DTOs.ShoppingList;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComparisonShoppingEngineAPI.Controllers
{
    [ApiController]
    [Route("optimize")]
    public class ShoppingListOptimizerController : ControllerBase
    {
        private readonly IShoppingListOptimizerService _listOptimizer;
        private readonly IProductService _productService;

        public ShoppingListOptimizerController(IShoppingListOptimizerService listOptimizer, IProductService productService)
        {
            _listOptimizer = listOptimizer;
            _productService = productService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> OptimizeList([FromBody] GetOptimizingShoppingListDto dto)
        {
            ServiceResponse<List<ProductDto>> productResponse;
            ServiceResponse<List<ShoppingItem>> optimizerResponse;

            IEnumerable<string> itemNames = dto.ShoppingList.Select(x => x.Name);
            productResponse = await _productService.GetProductsByNames(itemNames);

            if (!productResponse.Success) return BadRequest(productResponse);

            optimizerResponse = await _listOptimizer.OptimizeList(dto, productResponse.Data);

            if (!optimizerResponse.Success) return BadRequest(optimizerResponse);

            return Ok(optimizerResponse.Data);
        }
    }
}
