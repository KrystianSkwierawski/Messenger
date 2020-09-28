using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Model
{
    public class ApplicationUser : IdentityUser
    {
        //public List<RelationShip> RelationShips { get; set; }

        public string ImageUrl { get; set; }
    }
}
