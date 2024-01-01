using Microsoft.EntityFrameworkCore;

namespace PGTest.Data
{
    public class MSTestDataContext : DbContext
    {
        public MSTestDataContext(DbContextOptions<MSTestDataContext> options) :base(options) 
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseSerialColumns();
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
    }
}
