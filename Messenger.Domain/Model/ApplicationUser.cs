using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Messenger.Domain.Model
{
    public class ApplicationUser : IdentityUser
    {
        public List<ApplicationUser> Friends { get; set; }
        public List<Message> Messages { get; set; }
    }
}
