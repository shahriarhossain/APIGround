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
                new CityDTO(){ Id=1, Name="Dhaka", Description= "Capital",
                    POI = new List<PointOfInterestDTO>()
                    {
                        new PointOfInterestDTO(){Id = 1, Name= "Lalbagh Kella", Description="Historical place...."},
                        new PointOfInterestDTO() {Id =2, Name="Parlament House", Description="Parlament house...."}
                    }
                },
                new CityDTO(){ Id=2, Name="Rajshahsi", Description= "Peaceful city",
                     POI = new List<PointOfInterestDTO>()
                     {
                         new PointOfInterestDTO(){Id = 1, Name= "Rajshahi University", Description="Historical place...."},
                         new PointOfInterestDTO() {Id =2, Name="Puthia Temple", Description="Historical Temple.."}
                     }
                }
            };
        }
    }
}
