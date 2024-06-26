using Application.Common.Interfaces;

namespace ChallengeApp.Application.Chores.Commands
{
    public record CompleteChoreCommand(string Id) : IRequest;

    public class CompleteChoreCommandHandler : IRequestHandler<CompleteChoreCommand>
    {
        private readonly IDbContext _context;

        public CompleteChoreCommandHandler(IDbContext context)
        {
            _context = context;
        }

        public async Task Handle(CompleteChoreCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Chores.FindAsync(new object[] { request.Id }, cancellationToken);

            Guard.Against.NotFound(request.Id, entity);

            entity.Completed = true;

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
