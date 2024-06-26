using Application.Common.Interfaces;
using ChallengeApp.Application.Common.Extentions;

namespace ChallengeApp.Application.Challenges.Queries
{
    public record GetChallengeQuery(string Id) : IRequest<ChallengeDetailsVM>;

    public class GetChallengeQueryHandler : IRequestHandler<GetChallengeQuery, ChallengeDetailsVM>
    {
        private readonly IDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUser _user;

        public GetChallengeQueryHandler(IDbContext context, IMapper mapper, ICurrentUser user)
        {
            _context = context;
            _mapper = mapper;
            _user = user;
        }

        public async Task<ChallengeDetailsVM> Handle(GetChallengeQuery request, CancellationToken cancellationToken)
        {
            var result = await _context.Challenges.FilterByPublicOrCurrentUser(_user).ProjectToSingle<Challenge, ChallengeDetailsVM>(x => x.Id.Equals(request.Id), _mapper.ConfigurationProvider); ;

            Guard.Against.NotFound(request.Id, result);

            return result;
        }
    }
}
