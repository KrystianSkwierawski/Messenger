﻿using Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Application.Common.ViewModel
{
    public class HomeViewModel
    {
        public IQueryable<RelationShip> RelationShips { get; set; }
        public List<ApplicationUser> Friends { get; set; }
        public string Theme { get; set; }
    }
}
