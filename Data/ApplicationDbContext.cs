using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FPTBOOK.Models;

namespace FPTBOOK.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<FPTBOOK.Models.Book> Book { get; set; }
        public DbSet<FPTBOOK.Models.Category> Category { get; set; }
        public DbSet<FPTBOOK.Models.Order> Order { get; set; }
        public DbSet<FPTBOOK.Models.User> User { get; set; }
    }
}