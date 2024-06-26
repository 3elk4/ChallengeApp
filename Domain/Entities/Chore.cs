using ChallengeApp.Domain.Constants;

namespace ChallengeApp.Domain.Models
{
    public class Chore : BaseAuditable
    {
        public bool Completed { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public int Points { get; set; } = 0;
        public Difficulty Difficulty { get; set; }

        public string ChallengeId { get; set; }
        public Challenge Challenge { get; set; } //??
    }
}
