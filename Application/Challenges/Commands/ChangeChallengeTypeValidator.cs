namespace Application.Challenges.Commands
{
    public class ChangeChallengeTypeValidator : AbstractValidator<ChangeChallengeTypeCommand>
    {
        public ChangeChallengeTypeValidator()
        {
            RuleFor(v => v.Id).NotEmpty();
            RuleFor(v => v.Type).NotEmpty();
        }
    }
}
