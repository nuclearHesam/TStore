using Microsoft.AspNetCore.Identity;

namespace TStore.Data
{
    public class ApplicationUser : IdentityUser
    {
        #pragma warning disable CS8618 
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Image { get; set; }
    }
}