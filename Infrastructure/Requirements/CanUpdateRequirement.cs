using Domain.Constants;
using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Requirements
{
    public class CanUpdateRequirement<T> : IAuthorizationRequirement where T : BaseAuditable
    {
    }

    public class CanUpdateHandler<T> : AuthorizationHandler<CanUpdateRequirement<T>> where T : BaseAuditable
    {
        private readonly IDbContext dbContext;
        private readonly IHttpContextAccessor httpContextAccessor;

        public CanUpdateHandler(IDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            this.dbContext = dbContext;
            this.httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CanUpdateRequirement<T> requirement)
        {
            var SourceId = GetSourceId();
            var challenge = GetSource(SourceId);

            if (ChallengeIsDraft(challenge))
            {
                context.Succeed(requirement);
            }
            else
            {
                var defaultHttpContext = context.Resource as DefaultHttpContext;
                defaultHttpContext.Response.Headers["X-Forbidden-Reason"] = "Challenge is not draft.";

                context.Fail();
            }

            return Task.CompletedTask;
        }

        private string? GetSourceId()
        {
            return httpContextAccessor.HttpContext?.Request.RouteValues["id"].ToString();
        }

        private Challenge GetSource(string SourceId)
        {
            var source = dbContext.GetSet<T>().Where(Source => Source.Id.Equals(SourceId)).FirstOrDefault();
            if (source is Challenge)
            {
                return (source as Challenge);
            }
            else if (source is Chore)
            {
                return dbContext.Challenges.Find(new object[] { (source as Chore).ChallengeId });
            }
            else
            {
                return null;
            }
        }

        private bool ChallengeIsDraft(Challenge challenge)
        {
            return challenge.Type.Equals(ChallengeType.DRAFT);
        }
    }
}
