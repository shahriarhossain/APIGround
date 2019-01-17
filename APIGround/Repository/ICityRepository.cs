using APIGround.Entity;
using System.Collections.Generic;

namespace APIGround.Repository
{
    public interface ICityRepository
    {
        IEnumerable<City> GetAllCity();
        City GetCity(int id);
    }
}
