using Microsoft.EntityFrameworkCore;

namespace PGTest.Data
{
    public class PGTestDataContext : DbContext
    {
        public PGTestDataContext(DbContextOptions<PGTestDataContext> options) :base(options) 
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseSerialColumns();
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
    }
}
