using Application.Common.Interfaces;
using ChallengeApp.Domain.Constants;

namespace ChallengeApp.Application.Chores.Commands
{
    public record UpdateChoreCommand : IRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Points { get; set; }
        public Difficulty Difficulty { get; set; }
        public string Id { get; set; }
    }

    public class UpdateChoreCommandHandler : IRequestHandler<UpdateChoreCommand>
    {
        private readonly IDbContext _context;

        public UpdateChoreCommandHandler(IDbContext context)
        {
            _context = context;
        }

        public async Task Handle(UpdateChoreCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Chores.FindAsync(new object[] { request.Id }, cancellationToken);

            Guard.Against.NotFound(request.Id, entity);

            entity.Title = request.Title;
            entity.Description = request.Description;
            entity.Points = request.Points;
            entity.Difficulty = request.Difficulty;

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
