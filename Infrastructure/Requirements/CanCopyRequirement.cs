using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Infrastructure.Requirements
{
    public class CanCopyRequirement : IAuthorizationRequirement
    {
    }

    public class CanCopyHandler : AuthorizationHandler<CanCopyRequirement>
    {
        private readonly IDbContext dbContext;
        private readonly IHttpContextAccessor httpContextAccessor;

        public CanCopyHandler(IDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            this.dbContext = dbContext;
            this.httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CanCopyRequirement requirement)
        {
            var UserId = GetUserId(context);
            var SourceId = GetSourceId();
            var challenge = GetSource(SourceId);

            if (ChallengeBelongsToUser(challenge, UserId) || ChallengeIsPublic(challenge))
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

        private Challenge? GetSource(string SourceId)
        {
            return dbContext.Challenges.Where(Source => Source.Id.Equals(SourceId)).FirstOrDefault();
        }

        private bool ChallengeBelongsToUser(Challenge challenge, string userId)
        {
            return challenge.CreatedBy.Equals(userId, StringComparison.Ordinal);
        }

        private bool ChallengeIsPublic(Challenge challenge)
        {
            return challenge.Type.Equals(ChallengeType.PUBLIC_FINAL);
        }
    }
}
