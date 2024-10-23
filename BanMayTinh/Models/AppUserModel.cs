using Microsoft.AspNetCore.Identity;

namespace BanMayTinh.Models
{
    public class AppUserModel: IdentityUser
    {
        public string RoleId { get; set; }
    }
}
