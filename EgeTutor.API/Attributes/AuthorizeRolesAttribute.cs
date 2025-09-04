

using EgeTutor.Core.Enums;
using Microsoft.AspNetCore.Authorization;

namespace EgeTutor.Persistence.Attributes
{
    public class AuthorizeRolesAttribute: AuthorizeAttribute
    {
        public AuthorizeRolesAttribute(params Roles[] roles)
        {
            Roles = string.Join(",", roles.Select(r => r.ToString()));
        }
    }
}
