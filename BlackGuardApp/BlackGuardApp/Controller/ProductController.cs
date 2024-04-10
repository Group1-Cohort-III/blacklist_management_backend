using BlackGuardApp.Application.DTOs;
using BlackGuardApp.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlackGuardApp.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productServices)
        {
            _productService = productServices;
        }

        [HttpGet]
        public IActionResult GetAllItems(int perPage = 10, int page = 1)
        {
            var response = _productService.GetAllProducts(perPage, page);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetProductById(string id)
        {
            var response = _productService.GetProductById(id);
            return Ok(response);
        }

        [HttpPost]
        public IActionResult AddProduct([FromBody] ProductRequestDto productRequestDto)
        {
            var response = _productService.AddProduct(productRequestDto);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateItem(string id, [FromBody] ProductRequestDto productRequestDto)
        {
            var response = _productService.UpdateProduct(id, productRequestDto);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteItem(string id)
        {
            var response = _productService.DeleteProduct(id);
            return Ok(response);
        }
    }
}
