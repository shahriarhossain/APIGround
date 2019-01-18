using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APIGround.Context;
using APIGround.Entity;

namespace APIGround.Repository
{
    public class CityRepository : ICityRepository
    {
        private readonly CityContext _cityContext;

        public CityRepository(CityContext cityContext)
        {
            _cityContext = cityContext;
        }

        public bool CityExists(int id)
        {
            return _cityContext.Cities.Any(x => x.Id == id);
        }

        public void CreateCity(City city)
        {
            _cityContext.Cities.Add(city);
        }

        public IEnumerable<City> GetAllCity()
        {
            return _cityContext.Cities.ToList();
        }

        public City GetCity(int id)
        {
            return _cityContext.Cities.FirstOrDefault(c => c.Id == id);
        }

        public bool Save()
        {
            return (_cityContext.SaveChanges() >= 0);
        }
    }
}
