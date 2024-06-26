using Application.Challenges.Commands;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;

namespace Infrastructure.Requirements
{
    public class CanChangeTypeRequirement : IAuthorizationRequirement
    {
    }

    public class CanChangeTypeHandler : AuthorizationHandler<CanChangeTypeRequirement>
    {
        private readonly IDbContext dbContext;
        private readonly IHttpContextAccessor httpContextAccessor;

        public CanChangeTypeHandler(IDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            this.dbContext = dbContext;
            this.httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CanChangeTypeRequirement requirement)
        {
            var SourceId = GetSourceId();
            var NewChallengeType = GetNewChallengeTypeAsync();
            var Challenge = GetChallenge(SourceId);

            if (CanChangeType(Challenge, NewChallengeType))
            {
                context.Succeed(requirement);
            }
            else
            {
                var defaultHttpContext = context.Resource as DefaultHttpContext;
                defaultHttpContext.Response.Headers["X-Forbidden-Reason"] = "Can't change type.";

                context.Fail();
            }

            return Task.CompletedTask;
        }

        private string? GetSourceId()
        {
            return httpContextAccessor.HttpContext?.Request.RouteValues["id"].ToString();
        }

        private ChallengeType? GetNewChallengeTypeAsync()
        {
            using var reader = new StreamReader(httpContextAccessor.HttpContext?.Request.Body);
            var body = reader.ReadToEnd();

            var request = JsonSerializer.Deserialize<ChangeChallengeTypeCommand>(body);
            return request.Type;
        }

        private Challenge? GetChallenge(string SourceId)
        {
            return dbContext.Challenges.Where(Source => Source.Id.Equals(SourceId)).FirstOrDefault();
        }

        private bool CanChangeType(Challenge? challenge, ChallengeType? newChallengeType)
        {
            if (challenge.Type.Equals(newChallengeType)) return true;

            return (challenge.Type.Equals(ChallengeType.DRAFT) && newChallengeType.Equals(ChallengeType.PRIVATE_FINAL)) ||
                   (challenge.Type.Equals(ChallengeType.PRIVATE_FINAL) && (newChallengeType.Equals(ChallengeType.PUBLIC_FINAL) || newChallengeType.Equals(ChallengeType.DRAFT)));
        }
    }
}
