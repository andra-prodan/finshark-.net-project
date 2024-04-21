using Microsoft.AspNetCore.Identity;

namespace api.Models
{
    public class AppUser : IdentityUser
    {
        public List<Portofolio> Portofolios { get; set; } = new List<Portofolio>();
    }
}