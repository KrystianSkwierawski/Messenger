using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IContext
    {
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<Message> Messages { get; set; }
        public DbSet<RelationShip> RelationShips { get; set; }

        public Task SaveChangesAsync();
    }
}
