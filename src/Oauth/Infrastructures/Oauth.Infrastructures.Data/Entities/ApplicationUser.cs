using System;
using Microsoft.AspNetCore.Identity;

namespace Oauth.Infrastructures.Data.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            RegDateTime = DateTime.Now;
            Id = Guid.NewGuid().ToString();
        }
        
        public DateTime RegDateTime { get; set; }
    }
}
