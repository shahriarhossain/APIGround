using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIGround.Entity
{
    public class PointOfInterest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(15)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string Description { get; set; }
        
        public int CityId { get; set; }

        [ForeignKey("CityId")]
        public City City { get; set; }
    }
}
