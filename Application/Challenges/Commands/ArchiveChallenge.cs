using Application.Common.Interfaces;

namespace Application.Challenges.Commands
{
    public record ArchiveChallengeCommand(string Id) : IRequest;

    public class ArchiveChallengeCommandHandler : IRequestHandler<ArchiveChallengeCommand>
    {
        private readonly IDbContext _context;
        private readonly ICurrentUser _user;

        public ArchiveChallengeCommandHandler(IDbContext context, ICurrentUser user)
        {
            _context = context;
            _user = user;
        }

        public async Task Handle(ArchiveChallengeCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Challenges.FindAsync(new object[] { request.Id }, cancellationToken);

            Guard.Against.NotFound(request.Id, entity);

            entity.Archived = DateTime.UtcNow;
            entity.ArchivedBy = _user.Id;

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
