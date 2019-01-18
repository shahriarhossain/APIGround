using APIGround.Context;
using APIGround.Models.Read;
using APIGround.Models.Write;
using APIGround.Repository;
using AutoMapper;
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
        private ICityRepository _cityRepository;

        public CitiesController(ILogger<CitiesController> logger, ICityRepository cityRepository)
        {
            _logger = logger;
            _cityRepository = cityRepository;
        }

        [Route("GetCities")]
        [HttpGet]
        public IActionResult GetCities()
        {
            var cities = _cityRepository.GetAllCity();
            if(cities == null)
            {
                return NotFound();
            }
            return Ok(cities );
        }

        [Route("City/{id}", Name = "GetCity")]
        [HttpGet]
        public IActionResult GetCity(int id)
        {
            try
            {
                if (!_cityRepository.CityExists(id))
                {
                    _logger.LogInformation($"City with id {id} wasn't found.");
                    return NotFound();
                }

                var city = _cityRepository.GetCity(id);
                var cityToReturn = Mapper.Map<LimitedCityInfoDTO>(city);

                return Ok(cityToReturn);
            }
            catch (System.Exception ex)
            {
                _logger.LogCritical($"Critical Exception encountered", ex);
                return StatusCode(500, "A problem happened while handling your request");
            }
        }

        [Route("City")]
        [HttpPost]
        public IActionResult CreateCity([FromBody]CityCreationDTO city)
        {
            try
            {
                var cityToSave = Mapper.Map<Entity.City>(city);
                _cityRepository.CreateCity(cityToSave);
                bool resp = _cityRepository.Save();
                if (resp)
                {
                    return CreatedAtRoute("GetCity", new { id = cityToSave.Id }, city);
                }
                return BadRequest();
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
            if (!_cityRepository.CityExists(cityId))
            {
                return NotFound();
            }

            var poiToReturn = CitiesDataStore.Current.cities.FirstOrDefault(x => x.Id == cityId).POI;

            return Ok(poiToReturn);
        }

        [Route("City/{cityId}/poi/{pId}", Name ="GetPoiRoute")]
        [HttpGet]
        public IActionResult GetPOI(int cityId, int pId)
        {
            if (!_cityRepository.CityExists(cityId))
            {
                return NotFound();
            }

            var poiToReturn = CitiesDataStore.Current.cities.FirstOrDefault(x => x.Id == cityId).POI.Where(x => x.Id == pId);

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

            if (!_cityRepository.CityExists(cityId))
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

            var city = CitiesDataStore.Current.cities.FirstOrDefault(x => x.Id == cityId);

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

            if (!_cityRepository.CityExists(cityId))
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

            var city = CitiesDataStore.Current.cities.FirstOrDefault(x => x.Id == cityId);
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

            if (!_cityRepository.CityExists(cityId))
            {
                return NotFound();
            }

            var city = CitiesDataStore.Current.cities.FirstOrDefault(x => x.Id == cityId);
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
            if (!_cityRepository.CityExists(cityId))
            {
                return NotFound();
            }

            var city = CitiesDataStore.Current.cities.FirstOrDefault(x => x.Id == cityId);

            var poiToDelete = city.POI.FirstOrDefault(x => x.Id == poiId);

            city.POI.Remove(poiToDelete);

            return NoContent();
        }
    }
}
