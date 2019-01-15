using System.Collections.Generic;

namespace APIGround.Models.Read
{
    public class CityDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int NumberOfPointOfInterst { get
            {
                return POI.Count;
            }
        }
        public List<PointOfInterestDTO> POI { get; set; } = new List<PointOfInterestDTO>();
    }
}
