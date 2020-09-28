﻿using System.Collections.Generic;
using System.Linq;

namespace Domain.Model
{
    public class RelationShip
    {
        public int Id { get; set; }
        public string InvitingUserId { get; set; }
        public ApplicationUser InvitingUser { get; set; }

        public string InvitedUserId { get; set; }
        public ApplicationUser InvitedUser { get; set; }

        public bool IsAccepted { get; set; }

        public List<Message> Messages { get; set; }
    }
}
