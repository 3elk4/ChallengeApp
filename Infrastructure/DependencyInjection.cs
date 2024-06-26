using ChallengeApp.Infrastructure.Interceptors;
using ChallengeApp.Infrastructure.Requirements;
using Infrastructure.Identity;
using Infrastructure.Interceptors;
using Infrastructure.Requirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace ChallengeApp.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
            services.AddScoped<ISaveChangesInterceptor, ChallengeInterceptor>();

            services.AddDbContext<AppDbContext>((sp, opt) =>
            {
                opt.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
                opt.UseInMemoryDatabase("Challenges");
            });

            services.AddScoped<IDbContext>(provider => provider.GetRequiredService<AppDbContext>());

            services.AddAuthentication().AddBearerToken(IdentityConstants.BearerScheme);

            services.AddAuthorizationBuilder()
                .AddPolicy("OwnChallenge", policy => policy.Requirements.Add(new OwnSourceRequirement<Challenge>()))
                .AddPolicy("OwnChore", policy => policy.Requirements.Add(new OwnSourceRequirement<Chore>()))
                .AddPolicy("CanComplete", policy => policy.Requirements.Add(new CanCompleteRequirement()))
                .AddPolicy("CanArchive", policy => policy.Requirements.Add(new CanArchiveRequirement()))
                .AddPolicy("CanCopy", policy => policy.Requirements.Add(new CanCopyRequirement()))
                .AddPolicy("CanDeleteChallenge", policy => policy.Requirements.Add(new CanDeleteRequirement<Challenge>()))
                .AddPolicy("CanDeleteChore", policy => policy.Requirements.Add(new CanDeleteRequirement<Chore>()))
                .AddPolicy("CanUpdateChallenge", policy => policy.Requirements.Add(new CanUpdateRequirement<Challenge>()))
                .AddPolicy("CanUpdateChore", policy => policy.Requirements.Add(new CanUpdateRequirement<Chore>()))
                .AddPolicy("CanChangeType", policy => policy.Requirements.Add(new CanChangeTypeRequirement()));

            services
                .AddIdentityCore<User>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddApiEndpoints();

            services.AddSingleton(TimeProvider.System);

            services.AddAuthorization();

            services.AddTransient<IAuthorizationHandler, OwnSourceHandler<Challenge>>();
            services.AddTransient<IAuthorizationHandler, OwnSourceHandler<Chore>>();
            services.AddTransient<IAuthorizationHandler, CanCompleteHandler>();
            services.AddTransient<IAuthorizationHandler, CanArchiveHandler>();
            services.AddTransient<IAuthorizationHandler, CanCopyHandler>();
            services.AddTransient<IAuthorizationHandler, CanDeleteHandler<Challenge>>();
            services.AddTransient<IAuthorizationHandler, CanDeleteHandler<Chore>>();
            services.AddTransient<IAuthorizationHandler, CanUpdateHandler<Challenge>>();
            services.AddTransient<IAuthorizationHandler, CanUpdateHandler<Chore>>();
            services.AddTransient<IAuthorizationHandler, CanChangeTypeHandler>();

            services.AddScoped<AppDbContextInitializer>();

            return services;
        }
    }
}
