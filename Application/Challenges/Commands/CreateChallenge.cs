using Application.Common.Interfaces;

namespace ChallengeApp.Application.Challenges.Commands
{
    public record CreateChallengeCommand : IRequest<string>
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }

    public class CreateChallengeCommandHandler : IRequestHandler<CreateChallengeCommand, string>
    {
        private readonly IDbContext _context;

        public CreateChallengeCommandHandler(IDbContext context)
        {
            _context = context;
        }

        public async Task<string> Handle(CreateChallengeCommand request, CancellationToken cancellationToken)
        {
            var entity = new Challenge
            {
                Title = request.Title,
                Description = request.Description
            };

            _context.Challenges.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}
