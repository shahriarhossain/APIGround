using APIGround.Models;
using System.Collections.Generic;

namespace APIGround
{
    public class CitiesDataStore
    {
        public static CitiesDataStore Current { get; } = new CitiesDataStore();

        public List<CityDTO> cities { get; }
        public CitiesDataStore()
        {
            cities = new List<CityDTO>()
            {
                new CityDTO(){ Id=1, Name="Dhaka", Description= "Capital"},
                new CityDTO(){ Id=2, Name="Rajshahsi", Description= "Peaceful city"}
            };
        }
    }
}
