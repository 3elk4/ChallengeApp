using ChallengeApp.Application.Chores.Commands;

namespace ChallangeApp.Server.Endpoints
{
    public class Chores : EndpointGroupBase
    {
        private readonly string OwnChore = "OwnChore";
        private readonly string CanComplete = "CanComplete";
        private readonly string CanUpdate = "CanUpdateChore";
        private readonly string CanDelete = "CanDeleteChore";

        public override void Map(WebApplication app)
        {
            app.MapGroup(this)
                .RequireAuthorization()
                .MapPost(CreateChore)
                .MapPut(UpdateChore, [OwnChore, CanUpdate], "{id}")
                .MapPatch(CompleteChore, [OwnChore, CanComplete], "{id}")
                .MapDelete(DeleteChore, [OwnChore, CanDelete], "{id}");
        }

        public Task<string> CreateChore(ISender sender, CreateChoreCommand command)
        {
            return sender.Send(command);
        }

        public async Task<IResult> UpdateChore(ISender sender, string id, UpdateChoreCommand command)
        {
            if (id != command.Id) return Results.BadRequest();
            await sender.Send(command);
            return Results.Ok();
        }

        public async Task<IResult> DeleteChore(ISender sender, string id)
        {
            await sender.Send(new DeleteChoreCommand(id));
            return Results.Ok();
        }

        public async Task<IResult> CompleteChore(ISender sender, string id, CompleteChoreCommand command)
        {
            if (id != command.Id) return Results.BadRequest();
            await sender.Send(command);
            return Results.StatusCode(204);
        }

    }
}
