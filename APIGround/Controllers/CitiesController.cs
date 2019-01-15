using APIGround.Models.Read;
using APIGround.Models.Write;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace APIGround.Controllers
{
    [Route("api/[controller]")]
    public class CitiesController : Controller
    {
        [Route("GetCities")]
        [HttpGet]
        public JsonResult GetCities()
        {
            return new JsonResult(CitiesDataStore.Current);
        }

        [Route("City/{id}")]
        [HttpGet]
        public IActionResult GetCity(int id)
        {
            var cityToReturn = CitiesDataStore.Current.cities.FirstOrDefault(x => x.Id == id);
            if (cityToReturn == null)
            {
                return NotFound();
            }
            return Ok(cityToReturn);
        }

        [Route("City/{cityId}/poi")]
        [HttpGet]
        public IActionResult GetPOI(int cityId)
        {
            var cityToReturn = CitiesDataStore.Current.cities.FirstOrDefault(x => x.Id == cityId);
            if (cityToReturn == null)
            {
                return NotFound();
            }

            var poiToReturn = cityToReturn.POI;

            return Ok(poiToReturn);
        }

        [Route("City/{cityId}/poi/{pId}")]
        [HttpGet]
        public IActionResult GetPOI(int cityId, int pId)
        {
            var cityToReturn = CitiesDataStore.Current.cities.FirstOrDefault(x => x.Id == cityId);
            if (cityToReturn == null)
            {
                return NotFound();
            }

            var poiToReturn = cityToReturn.POI.Where(x => x.Id == pId);
            if(poiToReturn == null)
            {
                return NotFound();
            }

            return Ok(poiToReturn);
        }

        [HttpPost("City/{cityId}/poi")]
        public IActionResult CreatePOI(int cityId, [FromBody] PointOfInterestCreationDTO poi )
        {
            if(poi == null)
            {
                return BadRequest();
            }

            var city = CitiesDataStore.Current.cities.FirstOrDefault(x => x.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            PointOfInterestDTO poiObj = new PointOfInterestDTO()
            {
                Id = city.NumberOfPointOfInterst + 1, //temporary
                Name = poi.Name,
                Description = poi.Description
            };
            city.POI.Add(poiObj);

            return Ok();
        }

    }
}
