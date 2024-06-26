using Application.Common.Interfaces;

namespace ChallengeApp.Application.Challenges.Commands
{
    public record UpdateChallengeCommand : IRequest
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }

    public class UpdateChallengeCommandHandler : IRequestHandler<UpdateChallengeCommand>
    {
        private readonly IDbContext _context;

        public UpdateChallengeCommandHandler(IDbContext context)
        {
            _context = context;
        }

        public async Task Handle(UpdateChallengeCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Challenges.FindAsync(new object[] { request.Id }, cancellationToken);

            Guard.Against.NotFound(request.Id, entity);

            entity.Title = request.Title;
            entity.Description = request.Description;

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
