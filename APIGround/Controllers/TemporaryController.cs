using APIGround.Context;
using Microsoft.AspNetCore.Mvc;

namespace APIGround.Controllers
{
    [Route("api/[controller]")]
    public class TemporaryController : Controller
    {
        public CityContext cityContext { get; set; }
        public TemporaryController(CityContext _cityContext)
        {
            cityContext = _cityContext;
        }

        [HttpGet("Index")]
        public IActionResult Index()
        {
            return Ok();
        }
    }
}