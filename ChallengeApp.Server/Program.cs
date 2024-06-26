using ChallengeApp.Application;
using ChallengeApp.Infrastructure;
using ChallengeApp.Server;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddWebServices();

var app = builder.Build();

app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200"));

//app.UseDefaultFiles();
//app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    await app.InitialiseDatabaseAsync();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Map("/", () => Results.Redirect("/api"));

app.UseExceptionHandler(options => { });


app.UseAuthentication();
app.UseAuthorization();

app.UseOutputCache();

app.MapEndpoints();

//app.MapFallbackToFile("/index.html");

app.Run();

