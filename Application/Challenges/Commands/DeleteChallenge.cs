using Application.Common.Interfaces;

namespace ChallengeApp.Application.Challenges.Commands
{
    public record DeleteChallengeCommand(string Id) : IRequest;

    public class DeleteChallengeCommandHandler : IRequestHandler<DeleteChallengeCommand>
    {
        private readonly IDbContext _context;

        public DeleteChallengeCommandHandler(IDbContext context)
        {
            _context = context;
        }

        public async Task Handle(DeleteChallengeCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Challenges.FindAsync(new object[] { request.Id }, cancellationToken);

            Guard.Against.NotFound(request.Id, entity);

            _context.Challenges.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
