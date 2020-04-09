using Microsoft.EntityFrameworkCore;

namespace DAQToolbox.Data
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Reading> readings { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        { }
    }
}
