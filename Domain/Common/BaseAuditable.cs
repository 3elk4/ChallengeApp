namespace ChallengeApp.Domain.Models
{
    public class BaseAuditable : Base
    {
        public DateTime Created { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime LastModified { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime? Archived { get; set; }
        public string? ArchivedBy { get; set; }
    }
}
