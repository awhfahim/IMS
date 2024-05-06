using Autofac;
using IMS.Api.RequestHandlers;
using Microsoft.AspNetCore.Mvc;

namespace IMS.Api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiController]
    public class ProductController(ILogger<ProductController> logger, ILifetimeScope scope) : ControllerBase
    {
        [HttpPost("add-product")]
        public async Task<IActionResult> AddProductAsync(ProductRequestHandler requestHandler)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            
            requestHandler.Resolve(scope);
            await requestHandler.AddProductAsync();
            return Created("add-product", requestHandler);
        }
    }
}
