namespace ChallengeApp.Application.Challenges.Commands
{
    public class UpdateChallengeValidator : AbstractValidator<UpdateChallengeCommand>
    {
        public UpdateChallengeValidator()
        {
            RuleFor(v => v.Title).MaximumLength(250).NotEmpty();
            RuleFor(v => v.Description).MaximumLength(1000).NotEmpty();
            RuleFor(v => v.Id).NotEmpty();
        }
    }
}
