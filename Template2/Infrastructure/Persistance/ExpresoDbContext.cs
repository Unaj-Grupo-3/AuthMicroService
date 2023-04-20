using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistance
{
    public class ExpresoDbContext : DbContext
    {
        public DbSet<Authentication> Authentications { get; set; }

        public ExpresoDbContext(DbContextOptions<ExpresoDbContext> options ) : base(options)
        {

        }

        public ExpresoDbContext() { }

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            modelbuilder.Entity<Authentication>(entity =>
            {
                entity.HasKey(e => e.AuthId);
            });
        }
    }
}
