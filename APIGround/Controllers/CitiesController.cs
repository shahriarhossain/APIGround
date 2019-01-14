using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace APIGround.Controllers
{
    [Route("api/[controller]")]
    public class CitiesController : Controller
    {
        [Route("GetCities")]
        [HttpGet]
        public JsonResult GetCities()
        {
            return new JsonResult(new List<Object>()
            {
                new {id= 1, name= "Dhaka"},
                new {id = 2, name = "Rajshahi"}
            });
        }
    }
}
