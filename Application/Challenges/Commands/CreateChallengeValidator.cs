namespace ChallengeApp.Application.Challenges.Commands
{
    public class CreateChallengeValidator : AbstractValidator<CreateChallengeCommand>
    {
        public CreateChallengeValidator()
        {
            RuleFor(v => v.Title).MaximumLength(250).NotEmpty();
            RuleFor(v => v.Description).MaximumLength(1000).NotEmpty();
        }
    }
}
