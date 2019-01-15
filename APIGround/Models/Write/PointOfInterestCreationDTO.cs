using System.ComponentModel.DataAnnotations;

namespace APIGround.Models.Write
{
    public class PointOfInterestCreationDTO
    {
        [Required(ErrorMessage = "Name field is required.")]
        [MaxLength(15)]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
