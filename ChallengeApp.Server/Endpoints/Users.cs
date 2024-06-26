using Infrastructure.Identity;

namespace ChallangeApp.Server.Endpoints
{
    public class Users : EndpointGroupBase
    {
        public override void Map(WebApplication app)
        {
            app.MapGroup(this)
                .MapIdentityApi<User>();
        }
    }
}
