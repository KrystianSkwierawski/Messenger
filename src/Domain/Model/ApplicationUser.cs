using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace Domain.Model
{
    public class ApplicationUser : IdentityUser
    {
        //public List<RelationShip> RelationShips { get; set; }

        public string ImageUrl { get; set; }

        public IQueryable<Message> Messages { get; set; }
    }
}
