using Microsoft.EntityFrameworkCore;

namespace DAQToolbox.Data
{
    public class ReadingDbContext : DbContext
    {
        public DbSet<Reading> Readings { get; set; }

        public ReadingDbContext(DbContextOptions<ReadingDbContext> options) : base(options)
        {

        }
    }
}
