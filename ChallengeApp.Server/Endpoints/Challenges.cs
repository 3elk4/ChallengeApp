using Application.Challenges.Commands;
using ChallengeApp.Application.Challenges;
using ChallengeApp.Application.Challenges.Commands;
using ChallengeApp.Application.Challenges.Queries;
using ChallengeApp.Application.Common.Models;

namespace ChallangeApp.Server.Endpoints
{
    public class Challenges : EndpointGroupBase
    {
        private readonly string OwnChallenge = "OwnChallenge";
        private readonly string CanArchive = "CanArchive";
        private readonly string CanCopy = "CanCopy";
        private readonly string CanDelete = "CanDeleteChallenge";
        private readonly string CanUpdate = "CanUpdateChallenge";
        private readonly string CanChangeType = "CanChangeType";

        public override void Map(WebApplication app)
        {
            app.MapGroup(this)
                .RequireAuthorization()
                .MapGet(GetChallengesWithPagination)
                .MapGet(GetChallenge, "{id}")
                .MapPost(CreateChallenge)
                .MapPost(CopyChallenge, [CanCopy], "{id}")
                .MapPatch(ArchiveChallenge, [OwnChallenge, CanArchive], "{id}/archive")
                .MapPatch(UnarchiveChallenge, [OwnChallenge, CanArchive], "{id}/unarchive")
                .MapPatch(ChangeChallengeType, [OwnChallenge, CanChangeType], "{id}/type")
                .MapPut(UpdateChallenge, [OwnChallenge, CanUpdate], "{id}")
                .MapDelete(DeleteChallenge, [OwnChallenge, CanDelete], "{id}");
        }

        public Task<PaginatedList<ChallengeBriefVM>> GetChallengesWithPagination(ISender sender, [AsParameters] GetChallengesWithPaginationQuery query)
        {
            return sender.Send(query);
        }

        public Task<ChallengeDetailsVM> GetChallenge(ISender sender, string id)
        {
            return sender.Send(new GetChallengeQuery(id));
        }

        public Task<string> CreateChallenge(ISender sender, CreateChallengeCommand command)
        {
            return sender.Send(command);
        }

        public async Task<IResult> UpdateChallenge(ISender sender, string id, UpdateChallengeCommand command)
        {
            if (id != command.Id) return Results.BadRequest();
            await sender.Send(command);
            return Results.Ok();
        }

        public async Task<IResult> DeleteChallenge(ISender sender, string id)
        {
            await sender.Send(new DeleteChallengeCommand(id));
            return Results.Ok();
        }

        public Task<string> CopyChallenge(ISender sender, string id)
        {
            return sender.Send(new CopyChallengeCommand(id));
        }

        public async Task<IResult> ArchiveChallenge(ISender sender, string id)
        {
            await sender.Send(new ArchiveChallengeCommand(id));
            return Results.StatusCode(204);
        }

        public async Task<IResult> UnarchiveChallenge(ISender sender, string id)
        {
            await sender.Send(new UnarchiveChallengeCommand(id));
            return Results.StatusCode(204);
        }

        public async Task<IResult> ChangeChallengeType(ISender sender, string id, ChangeChallengeTypeCommand command)
        {
            if (id != command.Id) return Results.BadRequest();
            await sender.Send(command);
            return Results.StatusCode(204);
        }
    }
}
