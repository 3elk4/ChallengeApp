using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Challenges.Commands
{
    public record CopyChallengeCommand(string Id) : IRequest<string>;

    public class CopyChallengeCommandHandler : IRequestHandler<CopyChallengeCommand, string>
    {
        private readonly IDbContext _context;

        public CopyChallengeCommandHandler(IDbContext context)
        {
            _context = context;
        }

        public async Task<string> Handle(CopyChallengeCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Challenges.Include(c => c.Chores).FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

            Guard.Against.NotFound(request.Id, entity);

            var challenge = new Challenge
            {
                Title = entity.Title,
                Description = entity.Description,
                Type = entity.Type,
                Author = entity.Author,
            };

            _context.Challenges.Add(challenge);

            CopyChores(challenge, entity.Chores);

            await _context.SaveChangesAsync(cancellationToken);

            return challenge.Id;
        }

        private void CopyChores(Challenge challenge, IEnumerable<Chore> chores)
        {
            foreach (var chore in chores)
            {
                var copiedChore = new Chore
                {
                    Completed = false,
                    Title = chore.Title,
                    Description = chore.Description,
                    Points = chore.Points,
                    Difficulty = chore.Difficulty
                };

                challenge.Chores.Add(copiedChore);
            }
        }
    }
}
