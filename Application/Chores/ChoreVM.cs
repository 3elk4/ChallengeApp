using ChallengeApp.Application.Common.Models;
using ChallengeApp.Domain.Constants;

namespace ChallengeApp.Application.Chores
{
    public class ChoreVM : BaseVM
    {
        public bool Completed { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public int Points { get; set; } = 0;
        public Difficulty Difficulty { get; set; }

        private class Mapping : Profile
        {
            public Mapping()
            {
                CreateMap<Chore, ChoreVM>();
            }
        }
    }
}
