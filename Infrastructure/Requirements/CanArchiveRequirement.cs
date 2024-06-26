using Domain.Constants;
using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Requirements
{
    public class CanArchiveRequirement : IAuthorizationRequirement
    {
    }

    public class CanArchiveHandler : AuthorizationHandler<CanArchiveRequirement>
    {
        private readonly IDbContext dbContext;
        private readonly IHttpContextAccessor httpContextAccessor;

        public CanArchiveHandler(IDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            this.dbContext = dbContext;
            this.httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CanArchiveRequirement requirement)
        {
            var SourceId = GetSourceId();
            var challenge = GetSource(SourceId);

            if (!ChallengeIsDraft(challenge))
            {
                context.Succeed(requirement);
            }
            else
            {
                var defaultHttpContext = context.Resource as DefaultHttpContext;
                defaultHttpContext.Response.Headers["X-Forbidden-Reason"] = "Challenge is draft.";

                context.Fail();
            }

            return Task.CompletedTask;
        }

        private string? GetSourceId()
        {
            return httpContextAccessor.HttpContext?.Request.RouteValues["id"].ToString();
        }

        private Challenge? GetSource(string SourceId)
        {
            return dbContext.Challenges.Where(Source => Source.Id.Equals(SourceId)).FirstOrDefault();
        }

        private bool ChallengeIsDraft(Challenge challenge)
        {
            return challenge.Type.Equals(ChallengeType.DRAFT);
        }
    }
}
