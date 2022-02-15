using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationWebAPI.Models
{
    public class AuthorizeDBContext:DbContext
    {
        public AuthorizeDBContext()
        {

        }

        public AuthorizeDBContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Authenticate> Credentials { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Authenticate>().HasNoKey();
        }

    }
}
