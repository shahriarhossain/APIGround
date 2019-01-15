using APIGround.Models.Read;
using APIGround.Models.Write;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace APIGround.Controllers
{
    [Route("api/[controller]")]
    public class CitiesController : Controller
    {
        private ILogger<CitiesController> _logger;
        public CitiesController(ILogger<CitiesController> logger)
        {
            _logger = logger;
        }
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
            try
            {
                var cityToReturn = CitiesDataStore.Current.cities.FirstOrDefault(x => x.Id == id);
                if (cityToReturn == null)
                {
                    _logger.LogInformation($"City with id {id} wasn't found.");
                    return NotFound();
                }
                return Ok(cityToReturn);
            }
            catch (System.Exception ex)
            {
                _logger.LogCritical($"Critical Exception encountered", ex);
                return StatusCode(500, "A problem happened while handling your request");
            }
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

        [Route("City/{cityId}/poi/{pId}", Name ="GetPoiRoute")]
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

            if (poi.Description == poi.Name)
            {
                ModelState.AddModelError("Description", "Name and Description should be different");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            PointOfInterestDTO poiObj = new PointOfInterestDTO()
            {
                Id = city.NumberOfPointOfInterst + 1, //temporary
                Name = poi.Name,
                Description = poi.Description
            };
            city.POI.Add(poiObj);

            return CreatedAtRoute("GetPoiRoute", new { cityId = cityId, pId = poiObj.Id }, poiObj);
        }

        [HttpPut("City/{cityId}/poi/{poiId}")]
        public IActionResult UpdatePOI(int cityId, int poiId, [FromBody] PointOfInterestCreationDTO poi)
        {
            if (poi == null)
            {
                return BadRequest(); 
            }

            var city = CitiesDataStore.Current.cities.FirstOrDefault(x => x.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            if (poi.Description == poi.Name)
            {
                ModelState.AddModelError("Description", "Name and Description should be different");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var poiToUpdate = city.POI.FirstOrDefault(x => x.Id == poiId);
            poiToUpdate.Name = poi.Name;
            poiToUpdate.Description = poi.Description;

            return NoContent();
        }

        //Good resource on Patch: https://dotnetcoretutorials.com/2017/11/29/json-patch-asp-net-core/
        [HttpPatch("City/{cityId}/poi/{poiId}")]
        public IActionResult PartialUpdatePOI(int cityId, int poiId, [FromBody] JsonPatchDocument<PointOfInterestCreationDTO> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var city = CitiesDataStore.Current.cities.FirstOrDefault(x => x.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var poiToUpdate = city.POI.FirstOrDefault(x => x.Id == poiId);
            if (poiToUpdate == null)
            {
                return NotFound();
            }

            var poiToPatch = new PointOfInterestCreationDTO()
            {
                Name = poiToUpdate.Name,
                Description = poiToUpdate.Description
            };

            patchDoc.ApplyTo(poiToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //map the patched obj with the actual object and store
            poiToUpdate.Name = poiToPatch.Name;
            poiToUpdate.Description = poiToPatch.Description;

            return NoContent();
        }

        [HttpDelete("City/{cityId}/poi/{poiId}")]
        public IActionResult DeletePOI(int cityId, int poiId)
        {
            var city = CitiesDataStore.Current.cities.FirstOrDefault(x => x.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var poiToDelete = city.POI.FirstOrDefault(x => x.Id == poiId);

            city.POI.Remove(poiToDelete);

            return NoContent();
        }
    }
}
