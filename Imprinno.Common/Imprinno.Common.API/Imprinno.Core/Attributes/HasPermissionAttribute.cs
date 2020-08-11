using Imprinno.Models.Entities;
using Imprinno.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace Imprinno.Core.Attributes
{
    public class HasPermissionAttribute : TypeFilterAttribute
    {
        public HasPermissionAttribute(PermEnums permission) : base(typeof(HasPermissionFilter))
        {
            Arguments = new object[] { permission };
        }
    }

    public class HasPermissionFilter : IAuthorizationFilter
    {
        readonly PermEnums? _permission;

        public HasPermissionFilter(PermEnums permission)
        {
            _permission = permission;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.Items.ContainsKey("Permissions") || _permission == null)
            {
                //Validation cannot take place without any permissions so returning unauthorized
                context.Result = new UnauthorizedResult();
                return;
            }
            var permissions = context.HttpContext.Items["Permissions"].ToString();
            var listPermissions = permissions.Split(',').ToList();
            if (!listPermissions.Any() || !listPermissions.Contains(((int)_permission).ToString()))
            {
                //Validation failed so returning unauthorized
                context.Result = new UnauthorizedResult();
                return;
            }

            return;
        }
    }
}
