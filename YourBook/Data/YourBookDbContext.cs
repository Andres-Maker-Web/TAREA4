using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using YourBook.Models;

namespace YourBook.Data
{
    public class YourBookDbContext: IdentityDbContext<AccountUser>
    {
        public YourBookDbContext(DbContextOptions<YourBookDbContext> options) :base(options)
        {
            
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<AccountUser> AccountUsers { get; set; }
    }
}
