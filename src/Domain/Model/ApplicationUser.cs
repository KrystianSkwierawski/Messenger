using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Domain.Model
{
    public class ApplicationUser : IdentityUser
    {
        public List<Friend> Friends { get; set; }
        public List<Message> Messages { get; set; }
    }
}
