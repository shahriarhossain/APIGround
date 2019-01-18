using APIGround.Entity;
using System.Collections.Generic;

namespace APIGround.Repository
{
    public interface ICityRepository
    {
        bool CityExists(int id);
        IEnumerable<City> GetAllCity();
        City GetCity(int id);
        void CreateCity(City city);
        bool Save();
    }
}
