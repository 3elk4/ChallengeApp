using Application.Common.Interfaces;
using ChallangeApp.Server.Services;
using Microsoft.OpenApi.Models;

namespace ChallengeApp.Server
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWebServices(this IServiceCollection services)
        {
            services.AddCors();

            services.AddScoped<ICurrentUser, CurrentUser>();

            services.AddHttpContextAccessor();

            services.AddExceptionHandler<CustomExceptionHandler>();

            services.AddEndpointsApiExplorer();

            services.AddOutputCache(options =>
            {
                options.AddBasePolicy(builder => builder.Expire(TimeSpan.FromSeconds(60)));
            });

            services.AddRouting(options => options.LowercaseUrls = true);

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Challanges API",
                    Description = "An ASP.NET Core Web API for managing challanges"
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme.
                      Enter 'Bearer' [space] and then your token in the text input below.
                      Example: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                  {
                    {
                      new OpenApiSecurityScheme
                      {
                        Reference = new OpenApiReference
                          {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                          },
                          Scheme = "oauth2",
                          Name = "Bearer",
                          In = ParameterLocation.Header,

                        },
                        new List<string>()
                      }
                    });
            });

            return services;
        }
    }
}
