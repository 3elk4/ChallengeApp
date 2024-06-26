using Application.Common.Interfaces;
using ChallengeApp.Application.Common.Extentions;
using ChallengeApp.Application.Common.Models;

namespace ChallengeApp.Application.Challenges.Queries
{
    public record GetChallengesWithPaginationQuery : IRequest<PaginatedList<ChallengeBriefVM>>
    {
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
        public string? SearchString { get; set; } = null;
    }

    public class GetTodoItemsWithPaginationQueryHandler : IRequestHandler<GetChallengesWithPaginationQuery, PaginatedList<ChallengeBriefVM>>
    {
        private readonly IDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUser _user;

        public GetTodoItemsWithPaginationQueryHandler(IDbContext context, IMapper mapper, ICurrentUser user)
        {
            _context = context;
            _mapper = mapper;
            _user = user;
        }

        public async Task<PaginatedList<ChallengeBriefVM>> Handle(GetChallengesWithPaginationQuery request, CancellationToken cancellationToken)
        {

            IQueryable<Challenge> challenges = _context.Challenges.FilterByPublicOrCurrentUser(_user).OrderBy(x => x.Title);

            if (!String.IsNullOrEmpty(request.SearchString))
            {
                var ss = request.SearchString;
                challenges = challenges.Where(ch => ch.Title.ToLower().Contains(ss) || ch.Description.ToLower().Contains(ss));
            }

            var challengesVm = challenges.ProjectToQueryable<ChallengeBriefVM>(_mapper.ConfigurationProvider);
            return await challengesVm.PaginatedListAsync(request.PageNumber, request.PageSize);
        }


    }
}
