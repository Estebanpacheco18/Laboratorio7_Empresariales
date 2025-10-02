using Lab07_EstebanPacheco.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Lab07_EstebanPacheco.Controller
{
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        [HttpPost]
        public IActionResult CreateProduct([FromBody] CreateProductDto product)
        {
            return Ok(new { message = "Producto creado exitosamente." });
        }
    }
}
