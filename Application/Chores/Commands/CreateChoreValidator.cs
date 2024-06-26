namespace ChallengeApp.Application.Chores.Commands
{
    public class CreateChoreValidator : AbstractValidator<CreateChoreCommand>
    {
        public CreateChoreValidator()
        {
            RuleFor(v => v.Title).MaximumLength(250).NotEmpty();
            RuleFor(v => v.Description).MaximumLength(1000).NotEmpty();
            RuleFor(v => v.ChallengeId).NotEmpty();
            RuleFor(v => v.Points).GreaterThan(0).NotEmpty();
            RuleFor(v => v.Difficulty).NotNull();
        }
    }
}
