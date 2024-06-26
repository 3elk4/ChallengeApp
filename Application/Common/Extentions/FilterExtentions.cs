using Application.Common.Interfaces;
using Domain.Constants;

namespace ChallengeApp.Application.Common.Extentions
{
    public static class FilterExtentions
    {
        public static IQueryable<Challenge> FilterByPublicOrCurrentUser(this IQueryable<Challenge> challenges, ICurrentUser user)
        {
            ChallengeType publicType = ChallengeType.PUBLIC_FINAL;
            return challenges.Where(ch => ch.Type.Equals(publicType) || ch.CreatedBy == user.Id);
        }
    }
}
