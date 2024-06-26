using ChallengeApp.Application.Common.Models;
using Domain.Constants;

namespace ChallengeApp.Application.Challenges
{
    public class ChallengeBriefVM : BaseVM
    {
        public string Title { get; init; }
        public string Description { get; init; }
        public ChallengeType Type { get; init; }

        private class Mapping : Profile
        {
            public Mapping()
            {
                CreateMap<Challenge, ChallengeBriefVM>();
            }
        }
    }
}
