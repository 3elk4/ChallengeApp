using Microsoft.AspNetCore.Authorization;

using System.Security.Claims;

namespace ChallengeApp.Infrastructure.Requirements
{
    public class OwnSourceRequirement<T> : IAuthorizationRequirement where T : BaseAuditable
    {
    }

    public class OwnSourceHandler<T> : AuthorizationHandler<OwnSourceRequirement<T>> where T : BaseAuditable
    {
        private readonly IDbContext dbContext;
        private readonly IHttpContextAccessor httpContextAccessor;

        public OwnSourceHandler(IDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            this.dbContext = dbContext;
            this.httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OwnSourceRequirement<T> requirement)
        {
            var userId = GetUserId(context);
            var SourceId = GetSourceId();
            var Source = GetSourceByUser(userId, SourceId);

            if (Source != null)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }

            return Task.CompletedTask;
        }

        private string? GetUserId(AuthorizationHandlerContext context)
        {
            return httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        private string? GetSourceId()
        {
            return httpContextAccessor.HttpContext?.Request.RouteValues["id"].ToString();
        }

        private object GetSourceByUser(string userId, string SourceId)
        {
            return dbContext.GetSet<T>().Where(Source => Source.Id.Equals(SourceId) && Source.CreatedBy.Equals(userId)).FirstOrDefault();
        }
    }
}
