namespace ChallengeApp.Application.Chores.Commands
{
    public class UpdateChoreValidator : AbstractValidator<UpdateChoreCommand>
    {
        public UpdateChoreValidator()
        {
            RuleFor(v => v.Title).MaximumLength(250).NotEmpty();
            RuleFor(v => v.Description).MaximumLength(1000).NotEmpty();
            RuleFor(v => v.Points).GreaterThan(0).NotEmpty();
            RuleFor(v => v.Difficulty).NotNull();
            RuleFor(v => v.Id).NotEmpty();
        }
    }
}
