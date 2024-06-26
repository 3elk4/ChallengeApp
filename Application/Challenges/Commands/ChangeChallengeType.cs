using Application.Common.Interfaces;
using Domain.Constants;

namespace Application.Challenges.Commands
{

    public record ChangeChallengeTypeCommand : IRequest
    {
        public string Id { get; set; }
        public ChallengeType Type { get; set; }
    }

    public class ChangeChallengeTypeCommandHandler : IRequestHandler<ChangeChallengeTypeCommand>
    {
        private readonly IDbContext _context;

        public ChangeChallengeTypeCommandHandler(IDbContext context)
        {
            _context = context;
        }

        public async Task Handle(ChangeChallengeTypeCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Challenges.FindAsync(new object[] { request.Id }, cancellationToken);

            Guard.Against.NotFound(request.Id, entity);

            entity.Type = request.Type;

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
