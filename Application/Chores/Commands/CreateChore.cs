using Application.Common.Interfaces;
using ChallengeApp.Domain.Constants;

namespace ChallengeApp.Application.Chores.Commands
{
    public record CreateChoreCommand : IRequest<string>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Points { get; set; }
        public Difficulty Difficulty { get; set; }
        public string ChallengeId { get; set; }
    }

    public class CreateChoreCommandHandler : IRequestHandler<CreateChoreCommand, string>
    {
        private readonly IDbContext _context;

        public CreateChoreCommandHandler(IDbContext context)
        {
            _context = context;
        }

        public async Task<string> Handle(CreateChoreCommand request, CancellationToken cancellationToken)
        {
            var entity = new Chore
            {
                Title = request.Title,
                Description = request.Description,
                Points = request.Points,
                Difficulty = request.Difficulty,
                ChallengeId = request.ChallengeId
            };

            _context.Chores.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}
