using Domain.Constants;
using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Requirements
{
    public class CanCompleteRequirement : IAuthorizationRequirement
    {
    }

    public class CanCompleteHandler : AuthorizationHandler<CanCompleteRequirement>
    {
        private readonly IDbContext dbContext;
        private readonly IHttpContextAccessor httpContextAccessor;

        public CanCompleteHandler(IDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            this.dbContext = dbContext;
            this.httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CanCompleteRequirement requirement)
        {
            var ChoreId = GetChoreId();
            var Challenge = GetChoreChallenge(ChoreId);

            if (CanComplete(Challenge))
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

        private string? GetChoreId()
        {
            return httpContextAccessor.HttpContext?.Request.RouteValues["id"].ToString();
        }

        private Challenge GetChoreChallenge(string ChoreId)
        {
            var chore = dbContext.Chores.Find(new object[] { ChoreId });
            return dbContext.Challenges.Find(new object[] { chore.ChallengeId });
        }

        private bool CanComplete(Challenge challenge)
        {
            return challenge.Type.Equals(ChallengeType.PRIVATE_FINAL) || challenge.Type.Equals(ChallengeType.PUBLIC_FINAL);
        }
    }
}
