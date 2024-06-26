using Domain.Constants;

namespace ChallengeApp.Domain.Models
{
    public class Challenge : BaseAuditable
    {
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public ChallengeType Type { get; set; } = ChallengeType.DRAFT;
        public IList<Chore> Chores { get; private set; } = new List<Chore>();

        public string Author { get; set; }
    }
}
