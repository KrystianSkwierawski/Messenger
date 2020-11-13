using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        //public List<RelationShip> RelationShips { get; set; }

        public string ImageUrl { get; set; }
        public string Theme { get; set; }
    }
}
