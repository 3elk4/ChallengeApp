namespace ChallengeApp.Domain.Models
{
    public class Base
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
    }
}
