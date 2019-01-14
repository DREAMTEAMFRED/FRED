using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FRED.Models
{
    public class FredContext : DbContext
    {
        public FredContext(DbContextOptions<FredContext> options) : base(options) { }

        public DbSet<User> User { get; set; }
        public DbSet<Log> Log { get; set; }

        // creates singular table names
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Log>().ToTable("Log");
        }
    }
}
