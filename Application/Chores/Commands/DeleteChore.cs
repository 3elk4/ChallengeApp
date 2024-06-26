using Application.Common.Interfaces;

namespace ChallengeApp.Application.Chores.Commands
{
    public record DeleteChoreCommand(string Id) : IRequest;

    public class DeleteChoreCommandHandler : IRequestHandler<DeleteChoreCommand>
    {
        private readonly IDbContext _context;

        public DeleteChoreCommandHandler(IDbContext context)
        {
            _context = context;
        }

        public async Task Handle(DeleteChoreCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Chores.FindAsync(new object[] { request.Id }, cancellationToken);

            Guard.Against.NotFound(request.Id, entity);

            _context.Chores.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
