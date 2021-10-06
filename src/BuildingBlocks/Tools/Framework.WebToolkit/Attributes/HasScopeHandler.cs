using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace BuildingBlocks.Framework.WebToolkit.Attributes
{
    public class HasScopeHandler : AuthorizationHandler<HasScopeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasScopeRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == "scope"))
                return Task.CompletedTask;

            if (context.User.HasClaim(c => c.Type == "scope" && c.Value == requirement.Scope))
                context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
