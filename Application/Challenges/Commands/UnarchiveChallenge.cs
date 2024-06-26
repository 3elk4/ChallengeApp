using Application.Common.Interfaces;

namespace Application.Challenges.Commands
{
    public record UnarchiveChallengeCommand(string Id) : IRequest;

    public class UnarchiveChallengeCommandHandler : IRequestHandler<UnarchiveChallengeCommand>
    {
        private readonly IDbContext _context;

        public UnarchiveChallengeCommandHandler(IDbContext context)
        {
            _context = context;
        }

        public async Task Handle(UnarchiveChallengeCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Challenges.FindAsync(new object[] { request.Id }, cancellationToken);

            Guard.Against.NotFound(request.Id, entity);

            entity.Archived = null;
            entity.ArchivedBy = null;

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
