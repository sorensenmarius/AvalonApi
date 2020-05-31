using Abp.Authorization;
using MultiplayerAvalon.Authorization.Roles;
using MultiplayerAvalon.Authorization.Users;

namespace MultiplayerAvalon.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
