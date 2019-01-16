using APIGround.Entity;
using Microsoft.EntityFrameworkCore;

namespace APIGround.Context
{
    public class CityContext : DbContext
    {
        public CityContext(DbContextOptions<CityContext> dbContextOptions) :base (dbContextOptions)
        {
            Database.Migrate();
        }

        public DbSet<City> Cities { get; set; }
        public DbSet<PointOfInterest> PointOfInterests { get; set; }
    }
}
