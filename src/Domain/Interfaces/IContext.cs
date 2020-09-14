using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Domain.Interfaces
{
    public interface IContext
    {
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<Message> Messages { get; set; }

        public void SaveChanges();
    }
}
