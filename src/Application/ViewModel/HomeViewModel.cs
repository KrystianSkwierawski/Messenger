using Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Application.ViewModel
{
    public  class HomeViewModel
    {
        public IQueryable<RelationShip> RelationShips { get; set; }
        public List<ApplicationUser> Friends { get; set; }
    }
}
